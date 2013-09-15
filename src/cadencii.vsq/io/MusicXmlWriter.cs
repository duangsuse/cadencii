/*
 * MusicXmlWriter.cs
 * Copyright © 2013 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MusicXML;

namespace cadencii.vsq.io
{
    interface ISequenceWriter
    {
        void write(VsqFile sequence, string file_path);
    }

    static class MusicXmlNoteUtil
    {
        public static note Duration(this note note, decimal duration)
        {
            note.duration.Add(duration);
            return note;
        }

        public static note Type(this note note, notetypevalue type)
        {
            note.type.Value = type;
            return note;
        }

        public static T Clone<T>(this T note)
            where T : class
        {
            using (var stream = new MemoryStream()) {
                var serializer = new BinaryFormatter();
                try {
                    serializer.Serialize(stream, note);
                    stream.Position = 0;
                    return (T)serializer.Deserialize(stream);
                } catch {
                    return null;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// MusicXML exporter class.
    /// </summary>
    public class MusicXmlWriter : ISequenceWriter
    {
        /// <summary>
        /// A sequence of notation, connected by ties.
        /// </summary>
        class TiedEvent : IEnumerable<note>
        {
            /// <summary>
            /// Template of note. All notes are constructed by connecting these tempate notes.
            /// </summary>
            private static readonly List<note> templates_;

            private List<MusicXML.note> notes_ = new List<note>();
            private int clock_;
            private int length_;

            static TiedEvent()
            {
                templates_ = new List<note>();
                Func<note> Create = () => {
                    var result = new note();
                    result.voice = "1";
                    result.type = new notetype();
                    return result;
                };
                templates_.Add(Create().Duration(1920).Type(notetypevalue.whole));
                templates_.Add(Create().Duration(960).Type(notetypevalue.half));
                templates_.Add(Create().Duration(480).Type(notetypevalue.quarter));
                templates_.Add(Create().Duration(240).Type(notetypevalue.eighth));
                templates_.Add(Create().Duration(120).Type(notetypevalue.Item16th));
                templates_.Add(Create().Duration(60).Type(notetypevalue.Item32nd));
                templates_.Add(Create().Duration(30).Type(notetypevalue.Item64th));
                templates_.Add(Create().Duration(15).Type(notetypevalue.Item128th));

                templates_.RemoveAll((note) => note.duration.First < (decimal)MusicXmlWriter.QUANTIZE_UNIT);
            }

            /// <summary>
            /// Initialize by VsqEvent object.
            /// </summary>
            /// <param name="item"></param>
            /// <param name="timesig_table"></param>
            public TiedEvent(VsqEvent item, TimesigVector timesig_table)
            {
                Init(timesig_table, item.Clock, item.ID.getLength(), item.ID.Note, false);
                if (notes_.Count > 0 && item.ID.LyricHandle != null && item.ID.LyricHandle.getCount() > 0) {
                    var lyric = new lyric();
                    lyric.text.Add(new textelementdata());
                    lyric.text.First.Value = item.ID.LyricHandle.getLyricAt(0).Phrase;
                    notes_[0].lyric = new lyric[] { lyric };
                }
            }

            /// <summary>
            /// Initialize by start clock and length. This constructor creates a 'rest'.
            /// </summary>
            /// <param name="clock_start"></param>
            /// <param name="length"></param>
            /// <param name="timesig_table"></param>
            public TiedEvent(int clock_start, int length, TimesigVector timesig_table)
            {
                Init(timesig_table, clock_start, length, 0, true);
            }

            public int Clock
            {
                get { return clock_; }
            }

            public int Length
            {
                get { return length_; }
            }

            private void Init(TimesigVector timesig_table, int clock_start, int length, int note_number, bool create_rest)
            {
                clock_ = clock_start;
                length_ = length;

                MusicXML.pitch pitch = null;
                if (!create_rest) {
                    pitch = new pitch();
                    int octave = VsqNote.getNoteOctave(note_number) + 1;
                    step step;
                    if (Enum.TryParse<step>(VsqNote.getNoteStringBase(note_number), out step)) {
                        pitch.step = step;
                    }
                    pitch.octave = octave.ToString();
                    int alter = VsqNote.getNoteAlter(note_number);
                    if (alter != 0) {
                        pitch.alter = alter;
                    }
                }

                int clock_end = clock_start + length;
                int current_clock = clock_start;
                while (current_clock < clock_start + length) {
                    int next_bar_clock = timesig_table.getClockFromBarCount(timesig_table.getBarCountFromClock(current_clock) + 1);
                    int remain = (next_bar_clock < clock_end ? next_bar_clock : clock_end) - current_clock;
                    var template =
                        templates_
                            .OrderByDescending((note) => note.duration.First)
                            .FirstOrDefault((note) => note.duration.First <= remain);
                    if (template == null) {
                        break;
                    } else {
                        var note = template.Clone();
                        if (create_rest) {
                            note.rest.Add(new rest());
                        } else {
                            note.pitch.Add(pitch.Clone());
                            note.stem = new stem();
                            note.stem.Value = stemvalue.up;
                        }
                        notes_.Add(note);
                        current_clock += (int)note.duration.First;
                    }
                }

                // connect note with tie
                if (!create_rest && notes_.Count >= 2) {
                    for (int i = 0; i < notes_.Count; ++i) {
                        var note = notes_[i];
                        var tied_list = new List<tied>();
                        if (i < notes_.Count - 1) {
                            var start_tie = new tie();
                            start_tie.type = startstop.start;
                            note.tie.Add(start_tie);

                            var tied = new tied();
                            tied.type = startstopcontinue.start;
                            tied_list.Add(tied);
                        }
                        if (0 < i) {
                            var stop_tie = new tie();
                            stop_tie.type = startstop.stop;
                            note.tie.Add(stop_tie);

                            var tied = new tied();
                            tied.type = startstopcontinue.stop;
                            tied_list.Add(tied);
                        }
                        var notations = new notations();
                        notations.Items = tied_list.ToArray();
                        note.notations = new MusicXML.notations[] { notations };
                    }
                }
            }

            public IEnumerator<MusicXML.note> GetEnumerator()
            {
                return notes_.GetEnumerator();
            }

            System.Collections.IEnumerator
                System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private const int QUANTIZE_UNIT = 15;

        /// <summary>
        /// Do export to MusicXML file.
        /// </summary>
        /// <param name="sequence">A sequence to be exported.</param>
        /// <param name="file_path">A file path.</param>
        public void write(VsqFile sequence, string file_path)
        {
            var score = new scorepartwise();
            
            score.identification = new identification();
            score.identification.encoding = new encoding();
            score.identification.encoding.software.Add(this.GetType().FullName);

            score.partlist = new partlist();
            score.partlist.scorepart = new scorepart();
            score.partlist.scorepart.id = "P1";
            score.partlist.scorepart.partname = new partname();
            score.partlist.scorepart.partname.Value = sequence.Track[1].getName();
            var partlist = new List<scorepart>();
            for (int i = 2; i < sequence.Track.Count; ++i) {
                var track = sequence.Track[i];
                var scorepart = new scorepart();
                scorepart.id = "P" + i;
                scorepart.partname = new partname();
                scorepart.partname.Value = track.getName();
                partlist.Add(scorepart);
            }
            score.partlist.Items = partlist.ToArray();

            score.part = 
                sequence.Track.Skip(1).Select((track) => {
                    return createScorePart(track, sequence.TimesigTable);
                }).ToArray();
            for (int i = 0; i < score.part.Length; i++) {
                score.part[i].id = "P" + (i + 1);
            }

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(scorepartwise));
            using (var stream = new FileStream(file_path, FileMode.Create, FileAccess.Write)) {
                var writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);
                writer.Formatting = System.Xml.Formatting.Indented;
                serializer.Serialize(writer, score);
            }
        }

        /// <summary>
        /// Create MusicXML.scorepartwisePart object by VsqTrack instance.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="timesig_table"></param>
        /// <returns></returns>
        private scorepartwisePart createScorePart(VsqTrack track, TimesigVector timesig_table)
        {
            var part = new scorepartwisePart();
            var note_list = quantizeTrack(track, timesig_table);
            var measures = new List<scorepartwisePartMeasure>();
            int measure = 0;
            Timesig timesig = new Timesig(0, 0);

            while (0 < note_list.Count) {
                int measure_start_clock = timesig_table.getClockFromBarCount(measure);
                int measure_end_clock = timesig_table.getClockFromBarCount(measure + 1);
                // get the list of TiedEvent, contained in target measure.
                var in_measure_tied_note_list = 
                    note_list
                        .Where((tied_event) => {
                            int tied_event_start = tied_event.Clock;
                            int tied_event_end = tied_event.Clock + tied_event.Length;
                            return
                                (measure_start_clock <= tied_event_start && tied_event_start < measure_end_clock)
                                || (measure_start_clock <= tied_event_end && tied_event_end < measure_end_clock)
                                || (tied_event_start <= measure_start_clock && measure_end_clock <= tied_event_end);
                        });

                // get the list of MusicXML.note.
                var in_measure_note_list =
                    in_measure_tied_note_list
                        .SelectMany((tied_event) => {
                            var result = new List<note>();
                            int clock = tied_event.Clock;
                            foreach (var note in tied_event) {
                                int length = (int)note.duration.First;
                                if (measure_start_clock <= clock && clock + length <= measure_end_clock) {
                                    result.Add(note);
                                }
                                clock += length;
                            }
                            return result;
                        });

                var partwise_measure = new scorepartwisePartMeasure();
                partwise_measure.number = (measure + 1).ToString();
                var items = new List<object>();

                var measure_timesig = timesig_table.getTimesigAt(measure_start_clock);
                if (!measure_timesig.Equals(timesig)) {
                    var attributes = new MusicXML.attributes();
                    attributes.divisions = 480;
                    attributes.divisionsSpecified = true;
                    attributes.time = new time[] { new time() };
                    attributes.time[0].beats.Add(measure_timesig.numerator.ToString());
                    attributes.time[0].beattype.Add(measure_timesig.denominator.ToString());
                    attributes.time[0].symbol = timesymbol.common;
                    attributes.time[0].symbolSpecified = true;
                    items.Add(attributes);
                }
                timesig = measure_timesig;

                items.AddRange(in_measure_note_list);
                partwise_measure.Items = items.ToArray();
                measures.Add(partwise_measure);

                note_list.RemoveAll((tied_event) => tied_event.Clock + tied_event.Length <= measure_end_clock);

                measure++;
            }

            part.measure = measures.ToArray();
            return part;
        }

        private int quantize(int clock)
        {
            const int unit = QUANTIZE_UNIT;
            int odd = clock % unit;
            int new_clock = clock - odd;
            if (odd > unit / 2) {
                new_clock += unit;
            }
            return new_clock;
        }

        private List<TiedEvent> quantizeTrack(VsqTrack track, TimesigVector timesig_table)
        {
            var result = new List<TiedEvent>();
            if (track.MetaText == null) {
                return result;
            }

            track.MetaText.Events.Events
                .AsParallel()
                .ForAll((item) => {
                    if (item.ID != null) {
                        var start_clock = quantize(item.Clock);
                        var end_clock = quantize(item.Clock + item.ID.getLength());
                        item.Clock = start_clock;
                        item.ID.setLength(end_clock - start_clock);
                    }
                });

            track.MetaText.Events.Events
                .RemoveAll((item) => {
                    if (item.ID == null) {
                        return false;
                    }
                    return item.ID.getLength() <= 0;
                });

            int count = track.MetaText.Events.Events.Count;
            int clock = 0;
            for (int i = 0; i < count; ++i) {
                var item = track.MetaText.Events.Events[i];
                if (item.ID.type == VsqIDType.Anote) {
                    int rest_length = item.Clock - clock;
                    if (rest_length > 0) {
                        result.Add(new TiedEvent(clock, rest_length, timesig_table));
                    }
                    result.Add(new TiedEvent(item, timesig_table));
                    clock = item.Clock + item.ID.getLength();
                }
            }
            return result;
        }
    }
}