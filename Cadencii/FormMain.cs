/*
 * FormMain.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.Cadencii;

//INCLUDE-SECTION IMPORT ..\BuildJavaUI\src\FormMain.java
import java.awt.*;
import java.awt.event.*;
import java.util.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.componentModel.*;
import org.kbinani.media.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
//#define MONITOR_FPS
#define AUTHOR_LIST_SAVE_BUTTON_VISIBLE
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Threading;
using Boare.Lib.AppUtil;
using Boare.Lib.Media;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.java.awt;
using bocoree.java.awt.event_;
using bocoree.java.io;
using bocoree.java.util;
using bocoree.windows.forms;
using bocoree.xml;
using bocoree.javax.swing;
using bocoree.componentModel;

namespace Boare.Cadencii {
    using BCancelEventArgs = System.ComponentModel.CancelEventArgs;
    using BDoWorkEventArgs = System.ComponentModel.DoWorkEventArgs;
    using BEventArgs = System.EventArgs;
    using BEventHandler = System.EventHandler;
    using BFormClosedEventArgs = System.Windows.Forms.FormClosedEventArgs;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
    using BKeyEventArgs = System.Windows.Forms.KeyEventArgs;
    using BKeyPressEventArgs = System.Windows.Forms.KeyPressEventArgs;
    using BMouseButtons = System.Windows.Forms.MouseButtons;
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using boolean = System.Boolean;
    using BPaintEventArgs = System.Windows.Forms.PaintEventArgs;
    using BPreviewKeyDownEventArgs = System.Windows.Forms.PreviewKeyDownEventArgs;
    using Integer = System.Int32;
    using java = bocoree.java;
    using Long = System.Int64;

#endif

#if JAVA
    public class FormMain extends BForm{
#else
    public class FormMain : BForm {
#endif
        #region Static Readonly Field
        private readonly Color s_pen_105_105_105 = new Color( 105, 105, 105 );
        private readonly Color s_pen_187_187_255 = new Color( 187, 187, 255 );
        private readonly Color s_pen_007_007_151 = new Color( 7, 7, 151 );
        private readonly Color s_pen_065_065_065 = new Color( 65, 65, 65 );
        private readonly Color s_txtbox_backcolor = new Color( 128, 128, 128 );
        private readonly Color s_note_fill = new Color( 181, 220, 86 );
        private readonly AuthorListEntry[] _CREDIT = new AuthorListEntry[]{
            new AuthorListEntry( "is developped by:", 2 ),
            new AuthorListEntry( "kbinani" ),
            new AuthorListEntry( "HAL(修羅場P)" ),
            new AuthorListEntry(),
            new AuthorListEntry(),
            new AuthorListEntry( "Special Thanks to", 3 ),
            new AuthorListEntry(),
            new AuthorListEntry( "tool icons designer:", 2 ),
            new AuthorListEntry( "Yusuke KAMIYAMANE" ),
            new AuthorListEntry(),
            new AuthorListEntry( "developper of STRAIGHT LIBRARY:", 2 ),
            new AuthorListEntry( "Hideki KAWAHARA" ),
            new AuthorListEntry( "Tetsu TAKAHASHI" ),
            new AuthorListEntry( "Hideki BANNO" ),
            new AuthorListEntry( "Masanori MORISE" ),
            new AuthorListEntry( "(sorry, list is not complete)", 2 ),
            new AuthorListEntry(),
            new AuthorListEntry( "developper of v.Connect:", 2 ),
            new AuthorListEntry( "HAL(修羅場P)" ),
            new AuthorListEntry(),
            new AuthorListEntry( "developper of UTAU:", 2 ),
            new AuthorListEntry( "飴屋/菖蒲" ),
            new AuthorListEntry(),
            new AuthorListEntry( "promoter:", 2 ),
            new AuthorListEntry( "zhuo" ),
            new AuthorListEntry(),
            new AuthorListEntry( "library tester:", 2 ),
            new AuthorListEntry( "evm" ),
            new AuthorListEntry( "そろそろP" ),
            new AuthorListEntry( "めがね１１０" ),
            new AuthorListEntry( "上総" ),
            new AuthorListEntry( "NOIKE" ),
            new AuthorListEntry( "逃亡者" ),
            new AuthorListEntry(),
            new AuthorListEntry( "translator:", 2 ),
            new AuthorListEntry( "Eji (zh-TW translation)" ),
            new AuthorListEntry( "kankan (zh-TW translation)" ),
            new AuthorListEntry(),
            new AuthorListEntry(),
            new AuthorListEntry( "Thanks to", 3 ),
            new AuthorListEntry(),
            new AuthorListEntry( "ないしょの人" ),
            new AuthorListEntry( "naquadah" ),
            new AuthorListEntry( "1zo" ),
            new AuthorListEntry( "Amby" ),
            new AuthorListEntry( "ケロッグ" ),
            new AuthorListEntry( "beginner" ),
            new AuthorListEntry( "b2ox" ),
            new AuthorListEntry( "麻太郎" ),
            new AuthorListEntry( "もみじぱん" ),
            new AuthorListEntry( "PEX" ),
            new AuthorListEntry( "やなぎがうら" ),
            new AuthorListEntry( "cocoonP" ),
            new AuthorListEntry( "かつ" ),
            new AuthorListEntry( "ちゃそ" ),
            new AuthorListEntry( "ちょむ" ),
            new AuthorListEntry( "whimsoft" ),
            new AuthorListEntry( "all members of Cadencii bbs", 2 ),
            new AuthorListEntry(),
            new AuthorListEntry( "     ... and you !", 3 ),
        };
        private readonly Font s_F9PT = new Font( java.awt.Font.SANS_SERIF, java.awt.Font.PLAIN, 9 );
        #endregion

        #region Constants and internal enums
        /// <summary>
        /// カーブエディタ画面の編集モード
        /// </summary>
        enum CurveEditMode {
            /// <summary>
            /// 何もしていない
            /// </summary>
            NONE,
            /// <summary>
            /// 鉛筆ツールで編集するモード
            /// </summary>
            EDIT,
            /// <summary>
            /// ラインツールで編集するモード
            /// </summary>
            LINE,
            /// <summary>
            /// 鉛筆ツールでVELを編集するモード
            /// </summary>
            EDIT_VEL,
            /// <summary>
            /// ラインツールでVELを編集するモード
            /// </summary>
            LINE_VEL,
            /// <summary>
            /// 真ん中ボタンでドラッグ中
            /// </summary>
            MIDDLE_DRAG,
        }

        enum ExtDragXMode {
            RIGHT,
            LEFT,
            NONE,
        }

        enum ExtDragYMode {
            UP,
            DOWN,
            NONE,
        }

        enum GameControlMode {
            DISABLED,
            NORMAL,
            KEYBOARD,
            CURSOR,
        }

        enum OverviewMouseDownMode {
            NONE,
            LEFT,
            MIDDLE,
        }

        /// <summary>
        /// スクロールバーの最小サイズ(ピクセル)
        /// </summary>
        private const int MIN_BAR_ACTUAL_LENGTH = 17;
        /// <summary>
        /// エントリの端を移動する時の、ハンドル許容範囲の幅
        /// </summary>
        private const int _EDIT_HANDLE_WIDTH = 7;
        private const int _TOOL_BAR_HEIGHT = 46;
        /// <summary>
        /// 単音プレビュー時に、wave生成完了を待つ最大の秒数
        /// </summary>
        private const double _WAIT_LIMIT = 5.0;
        public const String _APP_NAME = "Cadencii";
        /// <summary>
        /// 表情線の先頭部分のピクセル幅
        /// </summary>
        private const int _PX_ACCENT_HEADER = 21;
        /// <summary>
        /// パフォーマンスカウンタ用バッファの容量
        /// </summary>
        private const int _NUM_PCOUNTER = 50;
        private const String _VERSION_HISTORY_URL = "http://www.ne.jp/asahi/kbinani/home/cadencii/version_history.xml";
        /// <summary>
        /// コントロールカーブが不可視状態における、splitContainer1.Panel2の最小サイズ
        /// </summary>
        private const int _SPL1_PANEL2_MIN_HEIGHT = 34;
        /// <summary>
        /// splitContainer2.Panel2の最小サイズ
        /// </summary>
        private const int _SPL2_PANEL2_MIN_HEIGHT = 25;
        /// <summary>
        /// splitContainer*で使用するSplitterWidthプロパティの値
        /// </summary>
        private const int _SPL_SPLITTER_WIDTH = 4;
        const int _PICT_POSITION_INDICATOR_HEIGHT = 48;
        const int _SCROLL_WIDTH = 16;
        /// <summary>
        /// Overviewペインの高さ
        /// </summary>
        const int _OVERVIEW_HEIGHT = 50;
        /// <summary>
        /// splitContainerPropertyの最小幅
        /// </summary>
        const int _PROPERTY_DOCK_MIN_WIDTH = 50;
        /// <summary>
        /// Overviewに描く音符を表す円の直径
        /// </summary>
        const int _OVERVIEW_DOT_DIAM = 2;
        /// <summary>
        /// btnLeft, btnRightを押した時の、スクロール速度(px/sec)。
        /// </summary>
        const float _OVERVIEW_SCROLL_SPEED = 500.0f;
        const int _OVERVIEW_SCALE_COUNT_MAX = 7;
        const int _OVERVIEW_SCALE_COUNT_MIN = 3;
        #endregion

        #region Static Field
        /// <summary>
        /// CTRLキー。MacOSXの場合はMenu
        /// </summary>
        private int s_modifier_key = InputEvent.CTRL_MASK;
        #endregion

        #region Fields
        private VersionInfo m_versioninfo = null;
#if !JAVA
        private System.Windows.Forms.Cursor HAND;
#endif
        /// <summary>
        /// ボタンがDownされた位置。(座標はpictureBox基準)
        /// </summary>
        private Point m_button_initial;
        /// <summary>
        /// 真ん中ボタンがダウンされたときのvscrollのvalue値
        /// </summary>
        private int m_middle_button_vscroll;
        /// <summary>
        /// 真ん中ボタンがダウンされたときのhscrollのvalue値
        /// </summary>
        private int m_middle_button_hscroll;
        private boolean m_edited = false;
        /// <summary>
        /// パフォーマンスカウンタ
        /// </summary>
        private float[] m_performance = new float[_NUM_PCOUNTER];
        /// <summary>
        /// 最後にpictureBox1_Paintが実行された時刻(秒単位)
        /// </summary>
        private double m_last_ignitted;
        /// <summary>
        /// パフォーマンスカウンタから算出される画面の更新速度
        /// </summary>
        private float m_fps = 0f;
        /// <summary>
        /// カーブエディタの編集モード
        /// </summary>
        private CurveEditMode m_edit_curve_mode = CurveEditMode.NONE;
        /// <summary>
        /// ピアノロールの右クリックが表示される直前のマウスの位置
        /// </summary>
        private Point m_cMenuOpenedPosition;
        /// <summary>
        /// トラック名の入力に使用するテキストボックス
        /// </summary>
        private TextBoxEx m_txtbox_track_name;
        /// <summary>
        /// ピアノロールの画面外へのドラッグ時、前回自動スクロール操作を行った時刻
        /// </summary>
        private double m_timer_drag_last_ignitted;
        /// <summary>
        /// 画面外への自動スクロールモード
        /// </summary>
        private ExtDragXMode m_ext_dragx = ExtDragXMode.NONE;
        private ExtDragYMode m_ext_dragy = ExtDragYMode.NONE;
        /// <summary>
        /// EditMode=MoveEntryで，移動を開始する直前のマウスの仮想スクリーン上の位置
        /// </summary>
        private Point m_mouse_move_init;
        /// <summary>
        /// EditMode=MoveEntryで，移動を開始する直前のマウスの位置と，音符の先頭との距離(ピクセル)
        /// </summary>
        private int m_mouse_move_offset;
        /// <summary>
        /// マウスが降りているかどうかを表すフラグ．AppManager.isPointerDownedとは別なので注意
        /// </summary>
        private boolean m_mouse_downed = false;
        /// <summary>
        /// テンポ変更の位置を、マウスドラッグで移動させているかどうかを表すフラグ
        /// </summary>
        private boolean m_tempo_dragging = false;
        private int m_tempo_dragging_deltaclock = 0;
        /// <summary>
        /// 拍子変更の位置を、マウスドラッグで移動させているかどうかを表すフラグ
        /// </summary>
        private boolean m_timesig_dragging = false;
        private int m_timesig_dragging_deltaclock = 0;
        private boolean m_mouse_downed_trackselector = false;
        private ExtDragXMode m_ext_dragx_trackselector = ExtDragXMode.NONE;
        private boolean m_mouse_moved = false;
#if ENABLE_MOUSEHOVER
        /// <summary>
        /// マウスホバーを発生させるスレッド
        /// </summary>
        private Thread m_mouse_hover_thread = null;
#endif
        private boolean m_last_is_imemode_on = true;
        private boolean m_last_symbol_edit_mode = false;
        /// <summary>
        /// 鉛筆のモード
        /// </summary>
        private PencilMode m_pencil_mode = new PencilMode();
        private boolean m_startmark_dragging = false;
        private boolean m_endmark_dragging = false;
        /// <summary>
        /// ビブラート範囲を編集中の音符のInternalID
        /// </summary>
        private int m_vibrato_editing_id = -1;
        /// <summary>
        /// このフォームがアクティブ化されているかどうか
        /// </summary>
        private boolean m_form_activated = true;
        private GameControlMode m_game_mode = GameControlMode.DISABLED;
        /// <summary>
        /// 直接再生モード時の、再生開始した位置の曲頭からの秒数
        /// </summary>
        private float m_direct_play_shift = 0.0f;
        /// <summary>
        /// プレビュー再生の長さ
        /// </summary>
        private double m_preview_ending_time;
        private BTimer m_timer;
        private boolean m_last_pov_r = false;
        private boolean m_last_pov_l = false;
        private boolean m_last_pov_u = false;
        private boolean m_last_pov_d = false;
        private boolean m_last_btn_x = false;
        private boolean m_last_btn_o = false;
        private boolean m_last_btn_re = false;
        private boolean m_last_btn_tr = false;
        private boolean m_last_select = false;
        /// <summary>
        /// 前回ゲームコントローラのイベントを処理した時刻
        /// </summary>
        private double m_last_event_processed;
        /// <summary>
        /// splitContainer2.Panel2を最小化する直前の、splitContainer2.SplitterDistance
        /// </summary>
        private int m_last_splitcontainer2_split_distance = -1;
        private boolean m_spacekey_downed = false;
#if ENABLE_MIDI
        private MidiInDevice m_midi_in = null;
#endif
        private FormMidiImExport m_midi_imexport_dialog = null;
        private TreeMap<EditTool, Cursor> m_cursor = new TreeMap<EditTool, Cursor>();
        private Preference m_preference_dlg;
#if ENABLE_MIDI
        private BToolStripButton m_strip_ddbtn_metronome;
#endif
#if ENABLE_PROPERTY
        private PropertyPanelContainer m_property_panel_container;
#endif
#if ENABLE_SCRIPT
        private Vector<Object> m_palette_tools = new Vector<Object>();
#endif

        private int m_overview_direction = 1;
        private Thread m_overview_update_thread = null;
        private int m_overview_start_to_draw_clock_initial_value;
        /// <summary>
        /// btnLeftまたはbtnRightが下りた時刻
        /// </summary>
        private double m_overview_btn_downed;
        /// <summary>
        /// Overview画面左端でのクロック
        /// </summary>
        private int m_overview_start_to_draw_clock = 0;
        /// <summary>
        /// Overview画面の表示倍率
        /// </summary>
        private float m_overview_px_per_clock = 0.01f;
        /// <summary>
        /// Overview画面に表示されている音符の，平均ノートナンバー．これが，縦方向の中心線に反映される
        /// </summary>
        private float m_overview_average_note = 60.0f;
        /// <summary>
        /// Overview画面でマウスが降りている状態かどうか
        /// </summary>
        private OverviewMouseDownMode m_overview_mouse_down_mode = OverviewMouseDownMode.NONE;
        /// <summary>
        /// Overview画面で、マウスが下りた位置のx座標
        /// </summary>
        private int m_overview_mouse_downed_locationx;
#if JAVA
        private JPanel pictKeyLengthSplitter;
#else
        private BPictureBox pictKeyLengthSplitter;
#endif
        private int m_overview_scale_count = 5;
        /// <summary>
        /// AppManager.keyWidthを調節するモードに入ったかどうか
        /// </summary>
        private boolean m_key_length_splitter_mouse_downed = false;
        /// <summary>
        /// AppManager.keyWidthを調節するモードに入る直前での、マウスのスクリーン座標
        /// </summary>
        private Point m_key_length_splitter_initial_mouse;
        /// <summary>
        /// AppManager.keyWidthを調節するモードに入る直前での、keyWidthの値
        /// </summary>
        private int m_key_length_init_value = 68;
        private BFileChooser openXmlVsqDialog;
        private BFileChooser saveXmlVsqDialog;
        private BFileChooser openUstDialog;
        private BFileChooser openMidiDialog;
        private BFileChooser saveMidiDialog;
        private BFileChooser openWaveDialog;
        private BTimer timer;
        private BBackgroundWorker bgWorkScreen;
        #endregion

        public FormMain() {
#if JAVA
		    super();
#endif

#if DEBUG
            bocoree.debug.push_log( "FormMain..ctor()" );
            bocoree.debug.push_log( "    " + Environment.OSVersion.ToString() );
            bocoree.debug.push_log( "    FormID=" + AppManager.getID() );
            AppManager.debugWriteLine( "FormMain..ctor()" );
#endif
            AppManager.baseFont10Bold = new Font( AppManager.editorConfig.BaseFontName, java.awt.Font.BOLD, 10 );
            AppManager.baseFont8 = new Font( AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, 8 );
            AppManager.baseFont10 = new Font( AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, 10 );
            AppManager.baseFont9 = new Font( AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, 9 );

            s_modifier_key = ((AppManager.editorConfig.Platform == PlatformEnum.Macintosh) ? InputEvent.META_MASK : InputEvent.CTRL_MASK);

            AppManager.setVsqFile( new VsqFileEx( AppManager.editorConfig.DefaultSingerName,
                                                  AppManager.editorConfig.DefaultPreMeasure,
                                                  4,
                                                  4,
                                                  500000 ) );
#if JAVA
		    initialize();
            timer = new BTimer();
#else
            InitializeComponent();
            timer = new BTimer( this.components );
#endif
            bgWorkScreen = new BBackgroundWorker();
            registerEventHandlers();
            setResources();

            openXmlVsqDialog = new BFileChooser( "" );
            openXmlVsqDialog.addFileFilter( "VSQ Format(*.vsq)|*.vsq" );
            openXmlVsqDialog.addFileFilter( "Original Format(*.evsq)|*.evsq" );

            saveXmlVsqDialog = new BFileChooser( "" );
            saveXmlVsqDialog.addFileFilter( "VSQ Format(*.vsq)|*.vsq" );
            saveXmlVsqDialog.addFileFilter( "Original Format(*.evsq)|*.evsq" );
            saveXmlVsqDialog.addFileFilter( "All files(*.*)|*.*" );

            openUstDialog = new BFileChooser( "" );
            openUstDialog.addFileFilter( "UTAU Project File(*.ust)|*.ust" );
            openUstDialog.addFileFilter( "All Files(*.*)|*.*" );

            openMidiDialog = new BFileChooser( "" );
            saveMidiDialog = new BFileChooser( "" );
            openWaveDialog = new BFileChooser( "" );

            m_overview_scale_count = AppManager.editorConfig.OverviewScaleCount;
            m_overview_px_per_clock = getOverviewScaleX( m_overview_scale_count );

            menuVisualOverview.setSelected( AppManager.editorConfig.OverviewEnabled );
#if ENABLE_PROPERTY
            m_property_panel_container = new PropertyPanelContainer();
#endif

#if !ENABLE_SCRIPT
            menuSettingPaletteTool.setVisible( false );
            menuScript.setVisible( false );
#endif

#if ENABLE_MIDI
            m_strip_ddbtn_metronome = new BToolStripButton();
            m_strip_ddbtn_metronome.setText( "Metronome" );
            m_strip_ddbtn_metronome.Name = "m_strip_ddbtn_metronome";
            m_strip_ddbtn_metronome.CheckOnClick = true;
            m_strip_ddbtn_metronome.setSelected( AppManager.editorConfig.MetronomeEnabled );
            m_strip_ddbtn_metronome.CheckedChanged += new EventHandler( m_strip_ddbtn_metronome_CheckedChanged );
            toolStripBottom.Items.Add( m_strip_ddbtn_metronome );
#endif

            trackSelector = new TrackSelector();
            updateTrackSelectorVisibleCurve();
            trackSelector.setBackground( new Color( 108, 108, 108 ) );
            trackSelector.setCurveVisible( true );
#if JAVA
            // TODO: FormMain#.ctor; trackSelectorの初期化
            /*trackSelector.MouseClick += new MouseEventHandler( this.trackSelector_MouseClick );
            trackSelector.SelectedTrackChanged += new SelectedTrackChangedEventHandler( this.trackSelector_SelectedTrackChanged );
            trackSelector.MouseUp += new MouseEventHandler( this.trackSelector_MouseUp );
            trackSelector.MouseDown += new MouseEventHandler( trackSelector_MouseDown );
            trackSelector.SelectedCurveChanged += new SelectedCurveChangedEventHandler( this.trackSelector_SelectedCurveChanged );
            trackSelector.MouseMove += new MouseEventHandler( this.trackSelector_MouseMove );
            trackSelector.RenderRequired += new RenderRequiredEventHandler( this.trackSelector_RenderRequired );
            trackSelector.PreviewKeyDown += new PreviewKeyDownEventHandler( this.trackSelector_PreviewKeyDown );
            trackSelector.KeyDown += new KeyEventHandler( commonCaptureSpaceKeyDown );
            trackSelector.KeyUp += new KeyEventHandler( commonCaptureSpaceKeyUp );
            trackSelector.PreferredMinHeightChanged += new EventHandler( trackSelector_PreferredMinHeightChanged );*/
#else
            trackSelector.setLocation( new Point( 0, 242 ) );
            trackSelector.Margin = new System.Windows.Forms.Padding( 0 );
            trackSelector.Name = "trackSelector";
            trackSelector.setSelectedCurve( CurveType.VEL );
            trackSelector.setSize( 446, 250 );
            trackSelector.TabIndex = 0;
            trackSelector.MouseClick += new System.Windows.Forms.MouseEventHandler( this.trackSelector_MouseClick );
            trackSelector.SelectedTrackChanged += new SelectedTrackChangedEventHandler( this.trackSelector_SelectedTrackChanged );
            trackSelector.MouseUp += new System.Windows.Forms.MouseEventHandler( this.trackSelector_MouseUp );
            trackSelector.MouseDown += new System.Windows.Forms.MouseEventHandler( trackSelector_MouseDown );
            trackSelector.SelectedCurveChanged += new SelectedCurveChangedEventHandler( this.trackSelector_SelectedCurveChanged );
            trackSelector.MouseMove += new System.Windows.Forms.MouseEventHandler( this.trackSelector_MouseMove );
            trackSelector.RenderRequired += new RenderRequiredEventHandler( this.trackSelector_RenderRequired );
            trackSelector.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.trackSelector_PreviewKeyDown );
            trackSelector.KeyDown += new System.Windows.Forms.KeyEventHandler( commonCaptureSpaceKeyDown );
            trackSelector.KeyUp += new System.Windows.Forms.KeyEventHandler( commonCaptureSpaceKeyUp );
            trackSelector.PreferredMinHeightChanged += new EventHandler( trackSelector_PreferredMinHeightChanged );
#endif

#if !JAVA
            splitContainer1.Panel2MinSize = trackSelector.getPreferredMinSize();
            this.setMinimumSize( getWindowMinimumSize() );
#endif
            stripBtnScroll.setSelected( AppManager.autoScroll );

            applySelectedTool();
            applyQuantizeMode();

            // Palette Toolの読み込み
#if ENABLE_SCRIPT
            updatePaletteTool();
#endif


#if !JAVA
            // toolStipの位置を，前回終了時の位置に戻す
            Vector<BToolBar> top = new Vector<BToolBar>();
            Vector<BToolBar> bottom = new Vector<BToolBar>();
            if ( toolStripTool.Parent != null ) {
                if ( toolStripTool.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    toolStripContainer.TopToolStripPanel.Controls.Remove( toolStripTool );
                    top.add( toolStripTool );
                } else if ( toolStripTool.Parent.Equals( toolStripContainer.BottomToolStripPanel ) ) {
                    toolStripContainer.BottomToolStripPanel.Controls.Remove( toolStripTool );
                    bottom.add( toolStripTool );
                }
            }
            if ( toolStripMeasure.Parent != null ) {
                if ( toolStripMeasure.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    toolStripContainer.TopToolStripPanel.Controls.Remove( toolStripMeasure );
                    top.add( toolStripMeasure );
                } else if ( toolStripMeasure.Parent.Equals( toolStripContainer.BottomToolStripPanel ) ) {
                    toolStripContainer.BottomToolStripPanel.Controls.Remove( toolStripMeasure );
                    bottom.add( toolStripMeasure );
                }
            }
            if ( toolStripPosition.Parent != null ) {
                if ( toolStripPosition.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    toolStripContainer.TopToolStripPanel.Controls.Remove( toolStripPosition );
                    top.add( toolStripPosition );
                } else if ( toolStripPosition.Parent.Equals( toolStripContainer.BottomToolStripPanel ) ) {
                    toolStripContainer.BottomToolStripPanel.Controls.Remove( toolStripPosition );
                    bottom.add( toolStripPosition );
                }
            }
            if ( toolStripFile.Parent != null ) {
                if ( toolStripFile.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    toolStripContainer.TopToolStripPanel.Controls.Remove( toolStripFile );
                    top.add( toolStripFile );
                } else if ( toolStripFile.Parent.Equals( toolStripContainer.BottomToolStripPanel ) ) {
                    toolStripContainer.BottomToolStripPanel.Controls.Remove( toolStripFile );
                    bottom.add( toolStripFile );
                }
            }
#endif

#if !JAVA
            splitContainer1.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            splitContainer1.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            splitContainer1.BackColor = System.Drawing.Color.FromArgb( 212, 212, 212 );
            splitContainer2.Panel1.Controls.Add( panel1 );
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Panel2.Controls.Add( panel2 );
            splitContainer2.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            splitContainer2.Panel2.BorderColor = System.Drawing.Color.FromArgb( 112, 112, 112 );
            splitContainer1.Panel1.Controls.Add( splitContainer2 );
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add( trackSelector );
            trackSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Panel2MinSize = trackSelector.getPreferredMinSize();
            splitContainerProperty.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
#endif

#if ENABLE_PROPERTY
            splitContainerProperty.Panel1.Controls.Add( m_property_panel_container );
            m_property_panel_container.Dock = System.Windows.Forms.DockStyle.Fill;
#else
            splitContainerProperty.setDividerLocation( 0 );
            splitContainerProperty.setEnabled( false );
            menuVisualProperty.setVisible( false );
#endif

#if !JAVA
            splitContainerProperty.Panel2.Controls.Add( splitContainer1 );
            splitContainerProperty.Dock = System.Windows.Forms.DockStyle.Fill;

            // コントロールの位置・サイズを調節
            splitContainer2.Panel1.SuspendLayout();
            panel1.SuspendLayout();
            pictPianoRoll.SuspendLayout();
            vScroll.SuspendLayout();
            // panel1の中身は
            // picturePositionIndicator
            picturePositionIndicator.Left = 0;
            picturePositionIndicator.Top = 0;
            picturePositionIndicator.Width = panel1.Width;
            // pictPianoRoll
            pictPianoRoll.setBounds( 0, picturePositionIndicator.Height, panel1.Width - vScroll.getWidth(), panel1.Height - picturePositionIndicator.Height - hScroll.getHeight() );
            // vScroll
            vScroll.Left = pictPianoRoll.getWidth();
            vScroll.Top = picturePositionIndicator.Height;
            vScroll.Height = pictPianoRoll.getHeight();
            // pictureBox3
            pictureBox3.Left = 0;
            pictureBox3.Top = panel1.Height - hScroll.getHeight();
            pictureBox3.Height = hScroll.getHeight();
            // hScroll
            hScroll.Left = pictureBox3.Width;
            hScroll.Top = panel1.Height - hScroll.Height;
            hScroll.Width = panel1.Width - pictureBox3.Width - trackBar.getWidth() - pictureBox2.Width;
            // trackBar
            trackBar.Left = pictureBox3.Width + hScroll.Width;
            trackBar.Top = panel1.Height - hScroll.Height;
            // pictureBox2
            pictureBox2.Left = panel1.Width - vScroll.Width;
            pictureBox2.Top = panel1.Height - hScroll.Height;

            vScroll.ResumeLayout();
            pictPianoRoll.ResumeLayout();
            panel1.ResumeLayout();
            splitContainer2.Panel1.ResumeLayout();
#endif

#if JAVA
            // TODO: FormMain#.ctor
#else
            pictPianoRoll.MouseWheel += new System.Windows.Forms.MouseEventHandler( pictPianoRoll_MouseWheel );
            trackSelector.MouseWheel += new System.Windows.Forms.MouseEventHandler( trackSelector_MouseWheel );
            picturePositionIndicator.MouseWheel += new System.Windows.Forms.MouseEventHandler( picturePositionIndicator_MouseWheel );
            menuVisualOverview.CheckedChanged += new EventHandler( menuVisualOverview_CheckedChanged );
#endif

            hScroll.setMaximum( AppManager.getVsqFile().TotalClocks + 240 );
            hScroll.setVisibleAmount( 240 * 4 );

            vScroll.setMaximum( AppManager.editorConfig.PxTrackHeight * 128 );
            vScroll.setVisibleAmount( 24 * 4 );
#if !JAVA
            hScroll.SmallChange = 240;
            vScroll.SmallChange = 24;
#endif

            trackSelector.setCurveVisible( true );

#if !JAVA
            // 左上のやつから順に登録
            XmlPoint p = AppManager.editorConfig.ToolEditTool.Location;
            toolStripTool.Location = new System.Drawing.Point( p.x, p.y );
            p = AppManager.editorConfig.ToolMeasureLocation.Location;
            toolStripMeasure.Location = new System.Drawing.Point( p.x, p.y );
            p = AppManager.editorConfig.ToolPositionLocation.Location;
            toolStripPosition.Location = new System.Drawing.Point( p.x, p.y );
            p = AppManager.editorConfig.ToolFileLocation.Location;
            toolStripFile.Location = new System.Drawing.Point( p.x, p.y );
            addToolStripInPositionOrder( toolStripContainer.TopToolStripPanel, top );
            addToolStripInPositionOrder( toolStripContainer.BottomToolStripPanel, bottom );
            
            toolStripTool.ParentChanged += new System.EventHandler( this.toolStripEdit_ParentChanged );
            toolStripTool.Move += new System.EventHandler( this.toolStripEdit_Move );
            toolStripMeasure.ParentChanged += new System.EventHandler( this.toolStripMeasure_ParentChanged );
            toolStripMeasure.Move += new System.EventHandler( this.toolStripMeasure_Move );
            toolStripPosition.ParentChanged += new System.EventHandler( this.toolStripPosition_ParentChanged );
            toolStripPosition.Move += new System.EventHandler( this.toolStripPosition_Move );
            toolStripFile.ParentChanged += new EventHandler( toolStripFile_ParentChanged );
            toolStripFile.Move += new EventHandler( toolStripFile_Move );
#endif

            // inputTextBoxの初期化
#if JAVA
            AppManager.inputTextBox = new TextBoxEx( this );
            AppManager.inputTextBox.setVisible( false );
            AppManager.inputTextBox.setSize( 80, 22 );
            AppManager.inputTextBox.setBackground( Color.white );
            AppManager.inputTextBox.setFont( new Font( AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, 9 ) );
            AppManager.inputTextBox.setEnabled( false );
            AppManager.inputTextBox.keyPressedEvent.add( new BKeyEventHandler( this, "m_input_textbox_KeyPress" ) );
#else
            AppManager.inputTextBox = new TextBoxEx();
            AppManager.inputTextBox.setVisible( false );
            AppManager.inputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            AppManager.inputTextBox.Width = 80;
            AppManager.inputTextBox.AcceptsReturn = true;
            AppManager.inputTextBox.setBackground( Color.white );
            AppManager.inputTextBox.setFont( new Font( AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, 9 ) );
            AppManager.inputTextBox.setEnabled( false );
            AppManager.inputTextBox.KeyPress += m_input_textbox_KeyPress;
            AppManager.inputTextBox.Parent = pictPianoRoll;
            panel1.Controls.Add( AppManager.inputTextBox );
#endif

            int fps = 1000 / AppManager.editorConfig.MaximumFrameRate;
            timer.setDelay( (fps <= 0) ? 1 : fps );
            menuTrackManager.setVisible( false );
#if DEBUG
            menuHelpDebug.setVisible( true );
#endif

#if !JAVA
            String _HAND = "AAACAAEAICAAABAAEADoAgAAFgAAACgAAAAgAAAAQAAAAAEABAAAAAAAgAIAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAgAAAAACAAACAgAAAAACAAIAAgAAAgIAAwMDAAICAgAD/AAAAAP8AAP//AAAAAP8A/wD/AAD//wD///8AAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/wAAAAAAAAAAAAAAAAAAD" +
                "//wAAAAAAAAAAAAAAAAAA//8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP8AAAAA/wAAAAAAAAAAA" +
                "A//AAAAAP/wAAAAAAAAAAAP/wAAAAD/8AAAAAAAAAAAAP8AAAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAA//8AAAAAAAAAAAAAAAAAAP//AAAAAAAAAAAAAAAAAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD////////////////////////////////////////////+f////" +
                "D////gf///4H////D///8/z//+H4f//B+D//wfg//+H4f//z/P///w////4H///+B////w////+f//////////////////////////" +
                "//////////////////w==";
            using ( System.IO.MemoryStream ms = new System.IO.MemoryStream( Base64.decode( _HAND ) ) ) {
                HAND = new System.Windows.Forms.Cursor( ms );
            }
#endif

#if !ENABLE_MIDI
            menuSettingMidi.setVisible( false );
#endif
            initResource();
            applyShortcut();
        }

        private void initResource() {
#if ENABLE_MIDI
            m_strip_ddbtn_metronome.setIcon( new ImageIcon( Resources.get_alarm_clock() ) );
#endif
        }

#if ENABLE_MIDI
        private void m_strip_ddbtn_metronome_CheckedChanged( Object sender, BEventArgs e ) {
            AppManager.editorConfig.MetronomeEnabled = m_strip_ddbtn_metronome.isSelected();
            if ( AppManager.editorConfig.MetronomeEnabled && AppManager.getEditMode() == EditMode.REALTIME ) {
                MidiPlayer.RestartMetronome();
            }
        }
#endif

        private void commonStripPaletteTool_Clicked( Object sender, BEventArgs e ) {
            String id = "";  //選択されたツールのID
#if ENABLE_SCRIPT
            if ( sender is BToolStripButton ) {
                BToolStripButton tsb = (BToolStripButton)sender;
                if ( tsb.getTag() != null && tsb.getTag() is String ) {
                    id = (String)tsb.getTag();
                    AppManager.selectedPaletteTool = id;
                    AppManager.setSelectedTool( EditTool.PALETTE_TOOL );
                    tsb.setSelected( true );
                }
            } else if ( sender is BMenuItem ) {
                BMenuItem tsmi = (BMenuItem)sender;
                if ( tsmi.getTag() != null && tsmi.getTag() is String ) {
                    id = (String)tsmi.getTag();
                    AppManager.selectedPaletteTool = id;
                    AppManager.setSelectedTool( EditTool.PALETTE_TOOL );
                    tsmi.setSelected( true );
                }
            }
#endif

            int count = toolStripTool.getComponentCount();
            for ( int i = 0; i < count; i++ ) {
                Object item = toolStripTool.getComponentAtIndex( i );
                if ( item is BToolStripButton ) {
                    BToolStripButton button = (BToolStripButton)item;
                    if ( button.getTag() != null && button.getTag() is String ) {
                        if ( ((String)button.getTag()).Equals( id ) ) {
                            button.setSelected( true );
                        } else {
                            button.setSelected( false );
                        }
                    }
                }
            }

            MenuElement[] sub_cmenu_piano_palette_tool = cMenuPianoPaletteTool.getSubElements();
            for ( int i = 0; i < sub_cmenu_piano_palette_tool.Length; i++ ) {
                MenuElement item = sub_cmenu_piano_palette_tool[i];
                if ( item is BMenuItem ) {
                    BMenuItem menu = (BMenuItem)item;
                    if ( menu.getTag() != null && menu.getTag() is String ) {
                        if ( ((String)menu.getTag()).Equals( id ) ) {
                            menu.setSelected( true );
                        } else {
                            menu.setSelected( false );
                        }
                    }
                }
            }

            MenuElement[] sub_cmenu_track_selectro_palette_tool = cMenuTrackSelectorPaletteTool.getSubElements();
            for ( int i = 0; i < sub_cmenu_track_selectro_palette_tool.Length; i++ ) {
                MenuElement item = sub_cmenu_track_selectro_palette_tool[i];
                if ( item is BMenuItem ) {
                    BMenuItem menu = (BMenuItem)item;
                    if ( menu.getTag() != null && menu.getTag() is String ) {
                        if ( ((String)menu.getTag()).Equals( id ) ) {
                            menu.setSelected( true );
                        } else {
                            menu.setSelected( false );
                        }
                    }
                }
            }
        }

        #region AppManager.inputTextBox
        private void m_input_textbox_KeyDown( Object sender, BKeyEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "m_input_textbox_KeyDown" );
            AppManager.debugWriteLine( "    e.KeyCode=" + e.KeyCode );
#endif
#if JAVA
            int keycode = e.getKeyCode();
            int modifiers = e.getModifiers();
            if ( keycode == KeyEvent.VK_TAB || keycode == KeyEvent.VK_ENTER )
#else
            if ( e.KeyCode == System.Windows.Forms.Keys.Tab || e.KeyCode == System.Windows.Forms.Keys.Return )
#endif
            {
                executeLyricChangeCommand();
                int selected = AppManager.getSelected();
                int index = -1;
                VsqTrack track = AppManager.getVsqFile().Track.get( selected );
                track.sortEvent();
#if JAVA
                if( keycode == KeyEvent.VK_TAB )
#else
                if ( e.KeyCode == System.Windows.Forms.Keys.Tab ) 
#endif
                {
                    int clock = 0;
                    for ( int i = 0; i < track.getEventCount(); i++ ) {
                        VsqEvent item = track.getEvent( i );
                        if ( item.InternalID == AppManager.getLastSelectedEvent().original.InternalID ) {
                            index = i;
                            clock = item.Clock;
                            break;
                        }
                    }
#if JAVA
                    if( (modifiers & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK )
#else
                    if ( (e.Modifiers & System.Windows.Forms.Keys.Shift) == System.Windows.Forms.Keys.Shift )
#endif
                    {
                        // 1個前の音符イベントを検索
                        int tindex = -1;
                        for ( int i = track.getEventCount() - 1; i >= 0; i-- ) {
                            VsqEvent ve = track.getEvent( i );
                            if ( ve.ID.type == VsqIDType.Anote && i != index && ve.Clock <= clock ) {
                                tindex = i;
                                break;
                            }
                        }
                        index = tindex;
                    } else {
                        // 1個後の音符イベントを検索
                        int tindex = -1;
                        for ( int i = 0; i < track.getEventCount(); i++ ) {
                            VsqEvent ve = track.getEvent( i );
                            if ( ve.ID.type == VsqIDType.Anote && i != index && ve.Clock >= clock ) {
                                tindex = i;
                                break;
                            }
                        }
                        index = tindex;
                    }
                }
                if ( 0 <= index && index < track.getEventCount() ) {
                    AppManager.clearSelectedEvent();
                    VsqEvent item = track.getEvent( index );
                    AppManager.addSelectedEvent( item.InternalID );
                    int x = AppManager.xCoordFromClocks( item.Clock );
                    int y = yCoordFromNote( item.ID.Note );
                    boolean phonetic_symbol_edit_mode = ((TagLyricTextBox)AppManager.inputTextBox.getTag()).isPhoneticSymbolEditMode();
                    showInputTextBox( item.ID.LyricHandle.L0.Phrase,
                                            item.ID.LyricHandle.L0.getPhoneticSymbol(),
                                            new Point( x, y ),
                                            phonetic_symbol_edit_mode );
                    int clWidth = (int)(AppManager.inputTextBox.getWidth() / AppManager.scaleX);
#if DEBUG
                    System.Diagnostics.Debug.WriteLine( "    clWidth=" + clWidth );
#endif
                    // 画面上にAppManager.inputTextBoxが見えるように，移動
                    int SPACE = 20;
                    if ( x < AppManager.keyWidth || pictPianoRoll.getWidth() < x + AppManager.inputTextBox.getWidth() ) {
                        int clock, clock_x;
                        if ( x < AppManager.keyWidth ) {
                            clock = item.Clock;
                            clock_x = AppManager.keyWidth + SPACE;
                        } else {
                            clock = item.Clock + clWidth;
                            clock_x = pictPianoRoll.getWidth() - SPACE;
                        }
                        double draft_d = (73 - clock_x) / AppManager.scaleX + clock;
                        if ( draft_d < 0.0 ) {
                            draft_d = 0.0;
                        }
                        int draft = (int)draft_d;
                        if ( draft < hScroll.getMinimum() ) {
                            draft = hScroll.getMinimum();
                        } else if ( hScroll.getMaximum() < draft ) {
                            draft = hScroll.getMaximum();
                        }
                        hScroll.setValue( draft );
                    } else {
                        refreshScreen();
                    }
                } else {
                    int id = AppManager.getLastSelectedEvent().original.InternalID;
                    AppManager.clearSelectedEvent();
                    AppManager.addSelectedEvent( id );
                    hideInputTextBox();
                }
            }
        }

        private void m_input_textbox_KeyUp( Object sender, BKeyEventArgs e ) {
#if JAVA
            boolean flip = (e.getKeyCode() == KeyEvent.VK_UP || e.getKeyCode() == KeyEvent.VK_DOWN) && ((e.getModifiers() & InputEvent.ALT_MASK) == InputEvent.ALT_MASK);
            boolean hide = (e.getKeyCode() == KeyEvent.VK_ESCAPE);
#else
            bool flip = (e.KeyCode == System.Windows.Forms.Keys.Up || e.KeyCode == System.Windows.Forms.Keys.Down) && (PortUtil.getCurrentModifierKey() == InputEvent.ALT_MASK);
            bool hide = e.KeyCode == System.Windows.Forms.Keys.Escape;
#endif

            if ( flip ) {
                if ( AppManager.inputTextBox.isVisible() ) {
                    flipInputTextBoxMode();
                }
            } else if ( hide ) {
                hideInputTextBox();
            }
        }

        private void m_input_textbox_ImeModeChanged( Object sender, BEventArgs e ) {
#if DEBUG
            Console.WriteLine( "FormMain#m_input_textbox_ImeModeChanged; imemode=" + AppManager.inputTextBox.ImeMode );
#endif
            m_last_is_imemode_on = AppManager.inputTextBox.isImeModeOn();
        }

        private void m_input_textbox_KeyPress( Object sender, BKeyPressEventArgs e ) {
#if !JAVA
            //           Enter                                  Tab
            e.Handled = (e.KeyChar == Convert.ToChar( 13 )) || (e.KeyChar == Convert.ToChar( 09 ));
#endif
        }
        #endregion

        private void m_toolbar_edit_SelectedToolChanged( EditTool tool ) {
            AppManager.setSelectedTool( tool );
        }

        private void m_toolbar_measure_EndMarkerClick( Object sender, BEventArgs e ) {
            AppManager.endMarkerEnabled = !AppManager.endMarkerEnabled;
#if DEBUG
            AppManager.debugWriteLine( "m_toolbar_measure_EndMarkerClick" );
            AppManager.debugWriteLine( "    m_config.EndMarkerEnabled=" + AppManager.endMarkerEnabled );
#endif
            refreshScreen();
        }

        private void m_toolbar_measure_StartMarkerClick( Object sender, BEventArgs e ) {
            AppManager.startMarkerEnabled = !AppManager.startMarkerEnabled;
#if DEBUG
            AppManager.debugWriteLine( "m_toolbar_measure_StartMarkerClick" );
            AppManager.debugWriteLine( "    m_config.StartMarkerEnabled=" + AppManager.startMarkerEnabled );
#endif
            refreshScreen();
        }

        private void handleRecentFileMenuItem_Click( Object sender, BEventArgs e ) {
            if ( sender is BMenuItem ) {
                BMenuItem item = (BMenuItem)sender;
                if ( item.getTag() is String ) {
                    String filename = (String)item.getTag();
                    openVsqCor( filename );
                    refreshScreen();
                }
            }
        }

        private void handleRecentFileMenuItem_MouseEnter( Object sender, BEventArgs e ) {
            if ( sender is BMenuItem ) {
                BMenuItem item = (BMenuItem)sender;
#if JAVA
                statusLabel.setText( item.getToolTipText() );
#else
                statusLabel.setText( item.ToolTipText );
#endif
            }
        }

        #region AppManager
        private void AppManager_CurrentClockChanged( Object sender, BEventArgs e ) {
#if JAVA
            stripLblBeat.setText( AppManager.getPlayPosition().numerator + "/" + AppManager.getPlayPosition().denominator );
            stripLblTempo.setText( PortUtil.formatDecimal( "#.00#", 60e6 / (float)AppManager.getPlayPosition().tempo ) );
            stripLblCursor.setText( AppManager.getPlayPosition().barCount + " : " + AppManager.getPlayPosition().beat + " : " + PortUtil.formatDecimal( "000", AppManager.getPlayPosition().clock ) );
#else
            stripLblBeat.setText( AppManager.getPlayPosition().numerator + "/" + AppManager.getPlayPosition().denominator );
            stripLblTempo.setText( PortUtil.formatDecimal( "#.00#", 60e6 / (float)AppManager.getPlayPosition().tempo ) );
            stripLblCursor.setText( AppManager.getPlayPosition().barCount + " : " + AppManager.getPlayPosition().beat + " : " + PortUtil.formatDecimal( "000", AppManager.getPlayPosition().clock ) );
#endif
        }

        private void AppManager_GridVisibleChanged( Object sender, BEventArgs e ) {
            menuVisualGridline.setSelected( AppManager.isGridVisible() );
            stripBtnGrid.setSelected( AppManager.isGridVisible() );
            cMenuPianoGrid.setSelected( AppManager.isGridVisible() );
        }

        private void AppManager_PreviewAborted( Object sender, BEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "AppManager_PreviewAborted" );
#endif
            if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                menuJobRealTime.setText( _( "Start Realtime Input" ) );
                AppManager.setEditMode( EditMode.NONE );
            }
#if DEBUG
            PortUtil.println( "  calling VSTiProxy.abortRendering..." );
#endif
            VSTiProxy.abortRendering();
#if DEBUG
            PortUtil.println( "  done" );
#endif
            AppManager.firstBufferWritten = false;
#if ENABLE_MIDI
            if ( m_midi_in != null ) {
                m_midi_in.Stop();
            }
#endif

            PlaySound.reset();
            for ( int i = 0; i < AppManager.drawStartIndex.Length; i++ ) {
                AppManager.drawStartIndex[i] = 0;
            }
#if ENABLE_MIDI
            MidiPlayer.Stop();
#endif
            timer.stop();
        }

        private void AppManager_PreviewStarted( Object sender, BEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "m_config_PreviewStarted" );
#endif
            PlaySound.reset();
            VsqFileEx vsq = AppManager.getVsqFile();
            String renderer = vsq.Track.get( AppManager.getSelected() ).getCommon().Version;
            int clock = AppManager.getCurrentClock();
            m_direct_play_shift = (float)vsq.getSecFromClock( clock );
            if ( AppManager.getEditMode() != EditMode.REALTIME ) {
                int selected = AppManager.getSelected();
                String tmppath = AppManager.getTempWaveDir();

                double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( vsq.Mixer.MasterFeder );
                double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( vsq.Mixer.MasterPanpot );
                double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( vsq.Mixer.MasterPanpot );

                // 選択されたトラック以外のレンダリングを行う
                Vector<Integer> render_all = new Vector<Integer>();
                int track_count = vsq.Track.size();
                for ( int track = 1; track < track_count; track++ ) {
                    if ( track == selected || !(vsq.Track.get( track ).getCommon().PlayMode >= 0) ) {
                        continue;
                    }
                    String file = PortUtil.combinePath( tmppath, track + ".wav" );
                    if ( !PortUtil.isFileExists( file ) ) {
                        render_all.add( track );
                    }
                }
                if ( render_all.size() > 0 ) {
                    render( PortUtil.convertIntArray( render_all.toArray( new Integer[] { } ) ) );
                }

                Vector<WaveReader> sounds = new Vector<WaveReader>();
                track_count = vsq.Track.size();
                for ( int track = 1; track < track_count; track++ ) {
                    VsqTrack vsq_track = vsq.Track.get( track );
                    if ( track == selected || !(vsq_track.getCommon().PlayMode >= 0) ) {
                        continue;
                    }

                    String file = PortUtil.combinePath( tmppath, track + ".wav" );
                    String tmpfile = PortUtil.combinePath( tmppath, "temp.wav" );
                    int t_start = vsq_track.getEditedStart();
                    int t_end = vsq_track.getEditedEnd();
                    int start = t_start;
                    int end = t_end;

                    // 編集が施された範囲に存在している音符を探し、（音符の末尾と次の音符の先頭の接続を無視した場合の）最長一致範囲を決める
                    int index_start = -1; //startから始まっている音符のインデックス
                    int event_count = vsq_track.getEventCount();
                    for ( int i = 0; i < event_count; i++ ) {
                        VsqEvent item = vsq_track.getEvent( i );
                        if ( item.Clock <= t_start && t_start <= item.Clock + item.ID.getLength() ) {
                            start = item.Clock;
                            index_start = i;
                            break;
                        }
                    }
                    int index_end = -1;
                    for ( int i = event_count - 1; i >= 0; i-- ) {
                        VsqEvent item = vsq_track.getEvent( i );
                        if ( item.Clock <= t_end && t_end <= item.Clock + item.ID.getLength() ) {
                            end = item.Clock + item.ID.getLength();
                            index_end = i;
                            break;
                        }
                    }

                    // 音符の末尾と次の音符の先頭がつながっている場合、レンダリング範囲を広げる
                    if ( index_start >= 0 ) {
                        for ( int i = index_start - 1; i >= 0; i-- ) {
                            VsqEvent ve = vsq_track.getEvent( i );
                            if ( ve.ID.type == VsqIDType.Anote ) {
                                int endpoint = ve.Clock + ve.ID.getLength();
                                if ( endpoint == start ) {
                                    start = ve.Clock;
                                    index_start = i;
                                } else if ( endpoint < start ) {
                                    break;
                                }
                            }
                        }
                    }
                    if ( index_end >= 0 ) {
                        for ( int i = index_end + 1; i < event_count; i++ ) {
                            VsqEvent ve = vsq_track.getEvent( i );
                            if ( ve.ID.type == VsqIDType.Anote ) {
                                int startpoint = ve.Clock;
                                if ( end == ve.Clock ) {
                                    end = ve.Clock + ve.ID.getLength();
                                    index_end = i;
                                } else if ( end < startpoint ) {
                                    break;
                                }
                            }
                        }
                    }

                    if ( start < end ) {
#if DEBUG
                        AppManager.debugWriteLine( "    partial rendering!" );
#endif
                        int temp_premeasure = AppManager.getVsqFile().getPresendClockAt( start, AppManager.editorConfig.PreSendTime ) * 2;
                        boolean successed = false;
                        FormSynthesize dlg = null;
                        try {
                            dlg = new FormSynthesize( vsq,
                                                      AppManager.editorConfig.PreSendTime,
                                                      track,
                                                      tmpfile,
                                                      start,
                                                      end,
                                                      temp_premeasure,
                                                      false );
                            if ( dlg.showDialog() == BDialogResult.OK ) {
                                successed = true;
                            }
                        } catch ( Exception ex ) {
                        } finally {
                            if ( dlg != null ) {
                                try {
                                    dlg.close();
                                } catch ( Exception ex2 ) {
                                }
                            }
                        }
                        if ( successed ) {
                            vsq_track.resetEditedArea();
                            Wave main = null;
                            Wave temp = null;
                            try {
                                main = new Wave( file );
                                temp = new Wave( tmpfile );
                                double sec_start = vsq.getSecFromClock( start );
                                double sec_end = vsq.getSecFromClock( end );

                                main.replace( temp,
                                              0,
                                              (int)(sec_start * main.getSampleRate()),
                                              (int)((sec_end - sec_start) * temp.getSampleRate()) );
                                main.write( file );
                            } catch ( Exception ex ) {
                            } finally {
                                if ( main != null ) {
                                    try {
#if !JAVA
                                        main.Dispose();
#endif
                                    } catch ( Exception ex2 ) {
                                    }
                                }
                                if ( temp != null ) {
                                    try {
#if !JAVA
                                        temp.Dispose();
#endif
                                    } catch ( Exception ex2 ) {
                                    }
                                }
                            }
                        }
                    }

                    WaveReader wr = new WaveReader( file );
                    wr.setTag( track );
                    sounds.add( wr );
                }

                // リアルタイム再生用のデータを準備
                m_preview_ending_time = vsq.getSecFromClock( AppManager.getVsqFile().TotalClocks ) + 1.0;

                // clock以降に音符があるかどうかを調べる
                int count = 0;
                for ( Iterator itr = vsq.Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = (VsqEvent)itr.next();
                    if ( ve.Clock >= clock ) {
                        count++;
                        break;
                    }
                }

                int bgm_count = AppManager.getBgmCount();
                double pre_measure_sec = vsq.getSecFromClock( vsq.getPreMeasureClocks() );
                for ( int i = 0; i < bgm_count; i++ ) {
                    BgmFile bgm = AppManager.getBgm( i );
                    WaveReader wr = new WaveReader( bgm.file );
                    wr.setTag( (int)(-i - 1) );
                    double offset = bgm.readOffsetSeconds;
                    if ( bgm.startAfterPremeasure ) {
                        offset -= pre_measure_sec;
                    }
                    wr.setOffsetSeconds( offset );
                    sounds.add( wr );
                }

                if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().PlayMode >= 0 && count > 0 ) {
                    int ms_presend = AppManager.editorConfig.PreSendTime;
                    if ( renderer.StartsWith( VSTiProxy.RENDERER_UTU0 ) ) {
                        ms_presend = 0;
                    }
#if DEBUG
                    AppManager.debugWriteLine( "m_preview_ending_time=" + m_preview_ending_time );
#endif
                    VSTiProxy.render( AppManager.getVsqFile(),
                                      selected,
                                      null,
                                      m_direct_play_shift,
                                      m_preview_ending_time,
                                      ms_presend,
                                      true,
                                      sounds.toArray( new WaveReader[] { } ),
                                      m_direct_play_shift,
                                      true,
                                      AppManager.getTempWaveDir(),
                                      false );

                    for ( int i = 0; i < AppManager.drawStartIndex.Length; i++ ) {
                        AppManager.drawStartIndex[i] = 0;
                    }
                    int clock_now = AppManager.getCurrentClock();
                    double sec_now = AppManager.getVsqFile().getSecFromClock( clock_now );
                } else {
                    VSTiProxy.render( new VsqFileEx( "Miku", AppManager.getVsqFile().getPreMeasure(), 4, 4, 500000 ),
                                      1,
                                      null,
                                      0,
                                      m_preview_ending_time,
                                      AppManager.editorConfig.PreSendTime,
                                      true,
                                      sounds.toArray( new WaveReader[] { } ),
                                      m_direct_play_shift,
                                      true,
                                      AppManager.getTempWaveDir(),
                                      false );
                }
            }

            m_last_ignitted = PortUtil.getCurrentTime();
            if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                menuJobRealTime.setText( _( "Stop Realtime Input" ) );
                AppManager.rendererAvailable = false;
#if ENABLE_MIDI
                if ( m_midi_in != null ) {
                    m_midi_in.Start();
                }
                MidiPlayer.SetSpeed( AppManager.editorConfig.RealtimeInputSpeed, m_last_ignitted );
                MidiPlayer.Start( AppManager.getVsqFile(), clock, m_last_ignitted );
#endif
            } else {
                AppManager.rendererAvailable = VSTiProxy.isRendererAvailable( renderer );
            }
            AppManager.firstBufferWritten = true;
            AppManager.previewStartedTime = m_last_ignitted;
#if DEBUG
            AppManager.debugWriteLine( "    m_config.VsqFile.TotalClocks=" + AppManager.getVsqFile().TotalClocks );
            AppManager.debugWriteLine( "    total seconds=" + AppManager.getVsqFile().getSecFromClock( (int)AppManager.getVsqFile().TotalClocks ) );
#endif
            timer.start();
        }

        private void AppManager_SelectedToolChanged( Object sender, BEventArgs e ) {
            applySelectedTool();
        }

        private void AppManager_SelectedEventChanged( Object sender, boolean selected_event_is_null ) {
            menuEditCut.setEnabled( !selected_event_is_null );
            menuEditPaste.setEnabled( !selected_event_is_null );
            menuEditDelete.setEnabled( !selected_event_is_null );
            cMenuPianoCut.setEnabled( !selected_event_is_null );
            cMenuPianoCopy.setEnabled( !selected_event_is_null );
            cMenuPianoDelete.setEnabled( !selected_event_is_null );
            cMenuPianoExpressionProperty.setEnabled( !selected_event_is_null );
            menuLyricVibratoProperty.setEnabled( !selected_event_is_null );
            menuLyricExpressionProperty.setEnabled( !selected_event_is_null );
            stripBtnCut.setEnabled( !selected_event_is_null );
            stripBtnCopy.setEnabled( !selected_event_is_null );
        }
        #endregion

        #region pictPianoRoll
        private void pictPianoRoll_MouseClick( Object sender, BMouseEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "pictPianoRoll_MouseClick" );
#endif
            int modefiers = PortUtil.getCurrentModifierKey();
            EditMode edit_mode = AppManager.getEditMode();

            boolean is_button_left = e.Button == BMouseButtons.Left;

            if ( e.Button == BMouseButtons.Left ) {
#if ENABLE_MOUSEHOVER
                if ( m_mouse_hover_thread != null ) {
                    m_mouse_hover_thread.Abort();
                }
#endif

                // クリック位置にIDが無いかどうかを検査
                ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>( new Rectangle() );
                VsqEvent item = getItemAtClickedPosition( new Point( e.X, e.Y ), out_id_rect );
                Rectangle id_rect = out_id_rect.value;
#if DEBUG
                AppManager.debugWriteLine( "    (item==null)=" + (item == null) );
#endif
                if ( item != null &&
                     edit_mode != EditMode.MOVE_ENTRY_WAIT_MOVE &&
                     edit_mode != EditMode.MOVE_ENTRY &&
                     edit_mode != EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE &&
                     edit_mode != EditMode.MOVE_ENTRY_WHOLE &&
                     edit_mode != EditMode.EDIT_LEFT_EDGE &&
                     edit_mode != EditMode.EDIT_RIGHT_EDGE &&
                     edit_mode != EditMode.MIDDLE_DRAG ) {
                    if ( (modefiers & InputEvent.SHIFT_MASK) != InputEvent.SHIFT_MASK && (modefiers & s_modifier_key) != s_modifier_key ) {
                        AppManager.clearSelectedEvent();
                    }
                    AppManager.addSelectedEvent( item.InternalID );
                    int selected = AppManager.getSelected();
                    int internal_id = item.InternalID;
                    hideInputTextBox();
                    if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventDelete( selected, internal_id ) );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                        setEdited( true );
                        AppManager.clearSelectedEvent();
                        return;
#if ENABLE_SCRIPT
                    } else if ( AppManager.getSelectedTool() == EditTool.PALETTE_TOOL ) {
                        Vector<Integer> internal_ids = new Vector<Integer>();
                        for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                            SelectedEventEntry see = (SelectedEventEntry)itr.next();
                            internal_ids.add( see.original.InternalID );
                        }
                        BMouseButtons btn = e.Button;
                        if ( AppManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier && m_spacekey_downed && e.Button == System.Windows.Forms.MouseButtons.Left ) {
                            btn = BMouseButtons.Middle;
                        }
                        boolean result = PaletteToolServer.InvokePaletteTool( AppManager.selectedPaletteTool,
                                                                           AppManager.getSelected(),
                                                                           internal_ids.toArray( new Integer[] { } ),
                                                                           btn );
                        if ( result ) {
                            setEdited( true );
                            AppManager.clearSelectedEvent();
                            return;
                        }
#endif
                    }
                } else {
                    if ( edit_mode != EditMode.MOVE_ENTRY_WAIT_MOVE &&
                         edit_mode != EditMode.MOVE_ENTRY &&
                         edit_mode != EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE &&
                         edit_mode != EditMode.MOVE_ENTRY_WHOLE &&
                         edit_mode != EditMode.EDIT_LEFT_EDGE &&
                         edit_mode != EditMode.EDIT_RIGHT_EDGE ) {
                        if ( !AppManager.isPointerDowned ) {
                            AppManager.clearSelectedEvent();
                        }
                        hideInputTextBox();
                    }
                    if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                        // マウス位置にビブラートの波波があったら削除する
                        int stdx = AppManager.startToDrawX;
                        int stdy = getStartToDrawY();
#if USE_DOBJ
                        for ( int i = 0; i < AppManager.drawObjects.get( AppManager.getSelected() - 1 ).size(); i++ ) {
                            DrawObject dobj = AppManager.drawObjects.get( AppManager.getSelected() - 1 ).get( i );
                            if ( dobj.pxRectangle.x + AppManager.startToDrawX + dobj.pxRectangle.width - stdx < 0 ) {
                                continue;
                            } else if ( pictPianoRoll.getWidth() < dobj.pxRectangle.x + AppManager.keyWidth - stdx ) {
                                break;
                            }
                            Rectangle rc = new Rectangle( dobj.pxRectangle.x + AppManager.keyWidth + dobj.pxVibratoDelay - stdx,
                                                          dobj.pxRectangle.y + AppManager.editorConfig.PxTrackHeight - stdy,
                                                          dobj.pxRectangle.width - dobj.pxVibratoDelay,
                                                          AppManager.editorConfig.PxTrackHeight );
#else
                        VsqTrack vsq_track = AppManager.VsqFile.getTrack( AppManager.Selected );
                        float scalex = AppManager.ScaleX;
                        for( Iterator itr0 = vsq_track.getNoteEventIterator(); itr0.hasNext();){
                            VsqEvent evnt = (VsqEvent)itr0.next();
                            if ( evnt.ID.VibratoHandle == null ){
                                continue;
                            }
                            int event_sx = XCoordFromClocks( evnt.Clock );
                            int event_ex = XCoordFromClocks( evnt.Clock + evnt.ID.Length);
                            int vib_sx = XCoordFromClocks( evnt.Clock + evnt.ID.VibratoDelay);
                            Rectangle rc = new Rectangle( vib_sx,
                                                          YCoordFromNote( evnt.ID.Note, stdy ),
                                                          event_ex - vib_sx,
                                                          AppManager.EditorConfig.PxTrackHeight );
#endif
                            if ( isInRect( new Point( e.X, e.Y ), rc ) ) {
                                //ビブラートの範囲なのでビブラートを消す
                                VsqID item2 = null;
                                int internal_id = -1;
#if USE_DOBJ
                                internal_id = dobj.internalID;
                                for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = (VsqEvent)itr.next();
                                    if ( ve.InternalID == dobj.internalID ) {
                                        item2 = (VsqID)ve.ID.clone();
                                        break;
                                    }
                                }
#else
                                item2 = evnt.ID;
                                internal_id = evnt.InternalID;
#endif
                                if ( item2 != null ) {
                                    item2.VibratoHandle = null;
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(),
                                                                                          internal_id,
                                                                                          item2 ) );
                                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                    setEdited( true );
                                }
                                break;
                            }
                        }
                    }
                }
            } else if ( e.Button == BMouseButtons.Right ) {
                boolean show_context_menu = (e.X > AppManager.keyWidth);
#if ENABLE_MOUSEHOVER
                if ( m_mouse_hover_thread != null ) {
                    if ( !m_mouse_hover_thread.IsAlive && AppManager.editorConfig.PlayPreviewWhenRightClick ) {
                        show_context_menu = false;
                    }
                } else {
#endif
                    if ( AppManager.editorConfig.PlayPreviewWhenRightClick ) {
                        show_context_menu = false;
                    }
#if ENABLE_MOUSEHOVER
                }
#endif
                show_context_menu = show_context_menu && !m_mouse_moved;
                if ( show_context_menu ) {
#if ENABLE_MOUSEHOVER
                    if ( m_mouse_hover_thread != null ) {
                        m_mouse_hover_thread.Abort();
                    }
#endif
                    ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>();
                    VsqEvent item = getItemAtClickedPosition( new Point( e.X, e.Y ), out_id_rect );
                    Rectangle id_rect = out_id_rect.value;
                    if ( item != null ) {
                        if ( !AppManager.isSelectedEventContains( AppManager.getSelected(), item.InternalID ) ) {
                            AppManager.clearSelectedEvent();
                        }
                        AppManager.addSelectedEvent( item.InternalID );
                    }
                    boolean item_is_null = (item == null);
                    cMenuPianoCopy.setEnabled( !item_is_null );
                    cMenuPianoCut.setEnabled( !item_is_null );
                    cMenuPianoDelete.setEnabled( !item_is_null );
                    cMenuPianoImportLyric.setEnabled( !item_is_null );
                    cMenuPianoExpressionProperty.setEnabled( !item_is_null );

                    int clock = AppManager.clockFromXCoord( e.X );
                    cMenuPianoPaste.setEnabled( ((AppManager.getCopiedItems().events.size() != 0) && (clock >= AppManager.getVsqFile().getPreMeasureClocks())) );
                    refreshScreen();

                    m_cMenuOpenedPosition = new Point( e.X, e.Y );
                    cMenuPiano.show( pictPianoRoll, e.X, e.Y );
                } else {
                    ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>();
                    VsqEvent item = getItemAtClickedPosition( m_button_initial, out_id_rect );
                    Rectangle id_rect = out_id_rect.value;
#if DEBUG
                    AppManager.debugWriteLine( "pitcPianoRoll_MouseClick; button is right; (item==null)=" + (item == null) );
#endif
                    if ( item != null ) {
                        int itemx = AppManager.xCoordFromClocks( item.Clock );
                        int itemy = yCoordFromNote( item.ID.Note );
                    }
                }
            } else if ( e.Button == BMouseButtons.Middle ) {
#if ENABLE_SCRIPT
                if ( AppManager.getSelectedTool() == EditTool.PALETTE_TOOL ) {
                    ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>();
                    VsqEvent item = getItemAtClickedPosition( new Point( e.X, e.Y ), out_id_rect );
                    Rectangle id_rect = out_id_rect.value;
                    if ( item != null ) {
                        AppManager.clearSelectedEvent();
                        AppManager.addSelectedEvent( item.InternalID );
                        Vector<Integer> internal_ids = new Vector<Integer>();
                        for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                            SelectedEventEntry see = (SelectedEventEntry)itr.next();
                            internal_ids.add( see.original.InternalID );
                        }
                        boolean result = PaletteToolServer.InvokePaletteTool( AppManager.selectedPaletteTool,
                                                                           AppManager.getSelected(),
                                                                           internal_ids.toArray( new Integer[] { } ),
                                                                           e.Button );
                        if ( result ) {
                            setEdited( true );
                            AppManager.clearSelectedEvent();
                            return;
                        }
                    }
                }
#endif
            }
        }

        private void pictPianoRoll_MouseDoubleClick( Object sender, BMouseEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "pictureBox1_MouseDoubleClick" );
#endif
            ByRef<Rectangle> out_rect = new ByRef<Rectangle>();
            VsqEvent item = getItemAtClickedPosition( new Point( e.X, e.Y ), out_rect );
            Rectangle rect = out_rect.value;
            if ( item != null ) {
#if ENABLE_SCRIPT
                if ( AppManager.getSelectedTool() != EditTool.PALETTE_TOOL )
#endif
                {
                    AppManager.clearSelectedEvent();
                    AppManager.addSelectedEvent( item.InternalID );
#if ENABLE_MOUSEHOVER
                    m_mouse_hover_thread.Abort();
#endif
                    if ( !AppManager.editorConfig.KeepLyricInputMode ) {
                        m_last_symbol_edit_mode = false;
                    }
                    showInputTextBox( item.ID.LyricHandle.L0.Phrase, item.ID.LyricHandle.L0.getPhoneticSymbol(), new Point( rect.x, rect.y ), m_last_symbol_edit_mode );
                    refreshScreen();
                    return;
                }
            } else {
                AppManager.clearSelectedEvent();
                hideInputTextBox();
                if ( AppManager.editorConfig.ShowExpLine && AppManager.keyWidth <= e.X ) {
                    int stdx = AppManager.startToDrawX;
                    int stdy = getStartToDrawY();
#if USE_DOBJ
                    for ( Iterator itr = AppManager.drawObjects.get( AppManager.getSelected() - 1 ).iterator(); itr.hasNext(); ) {
                        DrawObject dobj = (DrawObject)itr.next();
                        // 表情コントロールプロパティを表示するかどうかを決める
                        rect = new Rectangle(
                            dobj.pxRectangle.x + AppManager.keyWidth - stdx,
                            dobj.pxRectangle.y - stdy + AppManager.editorConfig.PxTrackHeight,
                            21,
                            AppManager.editorConfig.PxTrackHeight );
#else
                    for( Iterator itr = AppManager.VsqFile.getTrack( AppManager.Selected ).getNoteEventIterator(); itr.hasNext(); ){
                        VsqEvent evnt = (VsqEvent)itr.next();
                        int event_ex = XCoordFromClocks( evnt.Clock + evnt.ID.Length );
                        if ( event_ex < 0 ) {
                            continue;
                        }
                        int event_sx = XCoordFromClocks( evnt.Clock );
                        if ( pictPianoRoll.getWidth() < event_sx ) {
                            break;
                        }
                        int vib_sx = XCoordFromClocks( evnt.Clock + evnt.ID.VibratoDelay);
                        int event_sy = YCoordFromNote( evnt.ID.Note, stdy );
                        rect = new Rectangle( event_sx, event_sy, 21, AppManager.EditorConfig.PxTrackHeight );
#endif
                        if ( isInRect( new Point( e.X, e.Y ), rect ) ) {
                            VsqEvent selected = null;
#if USE_DOBJ
                            for ( Iterator itr2 = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr2.hasNext(); ) {
                                VsqEvent ev = (VsqEvent)itr2.next();
                                if ( ev.InternalID == dobj.internalID ) {
                                    selected = ev;
                                    break;
                                }
                            }
#else
                            selected = evnt;
#endif
                            if ( selected != null ) {
#if ENABLE_MOUSEHOVER
                                if ( m_mouse_hover_thread != null ) {
                                    m_mouse_hover_thread.Abort();
                                }
#endif
                                SynthesizerType type = SynthesizerType.VOCALOID2;
                                if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                                    type = SynthesizerType.VOCALOID1;
                                }
                                FormNoteExpressionConfig dlg = null;
                                try {
                                    dlg = new FormNoteExpressionConfig( type, selected.ID.NoteHeadHandle );
                                    dlg.setPMBendDepth( selected.ID.PMBendDepth );
                                    dlg.setPMBendLength( selected.ID.PMBendLength );
                                    dlg.setPMbPortamentoUse( selected.ID.PMbPortamentoUse );
                                    dlg.setDEMdecGainRate( selected.ID.DEMdecGainRate );
                                    dlg.setDEMaccent( selected.ID.DEMaccent );
                                    dlg.setLocation( getFormPreferedLocation( dlg ) );
                                    if ( dlg.showDialog() == BDialogResult.OK ) {
                                        VsqID id = (VsqID)selected.ID.clone();
                                        id.PMBendDepth = dlg.getPMBendDepth();
                                        id.PMBendLength = dlg.getPMBendLength();
                                        id.PMbPortamentoUse = dlg.getPMbPortamentoUse();
                                        id.DEMdecGainRate = dlg.getDEMdecGainRate();
                                        id.DEMaccent = dlg.getDEMaccent();
                                        id.NoteHeadHandle = dlg.getEditedNoteHeadHandle();
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(), selected.InternalID, id ) );
                                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                        setEdited( true );
                                        refreshScreen();
                                    }
                                } catch ( Exception ex ) {
                                } finally {
                                    if ( dlg != null ) {
                                        try {
                                            dlg.close();
                                        } catch ( Exception ex2 ) {
                                        }
                                    }
                                }
                                return;
                            }
                            break;
                        }

                        // ビブラートプロパティダイアログを表示するかどうかを決める
#if USE_DOBJ
                        rect = new Rectangle( dobj.pxRectangle.x + AppManager.keyWidth - stdx + 21,
                                              dobj.pxRectangle.y - stdy + AppManager.editorConfig.PxTrackHeight,
                                              dobj.pxRectangle.width - 21,
                                              AppManager.editorConfig.PxTrackHeight );
#else
                        if ( evnt.ID.VibratoHandle == null ){
                            continue;
                        }
                        rect = new Rectangle( event_sx + 21, 
                                              event_sy + AppManager.EditorConfig.PxTrackHeight,
                                              event_ex - event_sx - 21,
                                              AppManager.EditorConfig.PxTrackHeight );
#endif
                        if ( isInRect( new Point( e.X, e.Y ), rect ) ) {
                            VsqEvent selected = null;
#if USE_DOBJ
                            for ( Iterator itr2 = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr2.hasNext(); ) {
                                VsqEvent ev = (VsqEvent)itr2.next();
                                if ( ev.InternalID == dobj.internalID ) {
                                    selected = ev;
                                    break;
                                }
                            }
#else
                            selected = evnt;
#endif
                            if ( selected != null ) {
#if ENABLE_MOUSEHOVER
                                if ( m_mouse_hover_thread != null ) {
                                    m_mouse_hover_thread.Abort();
                                }
#endif
                                SynthesizerType type = SynthesizerType.VOCALOID2;
#if DEBUG
                                PortUtil.println( "FormMain#pictPianoRoll_MouseDoubleClick; version=" + AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version );
#endif
                                if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                                    type = SynthesizerType.VOCALOID1;
                                }
                                FormVibratoConfig dlg = null;
                                try {
                                    dlg = new FormVibratoConfig( selected.ID.VibratoHandle, selected.ID.getLength(), AppManager.editorConfig.DefaultVibratoLength, type );
                                    dlg.setLocation( getFormPreferedLocation( dlg ) );
                                    if ( dlg.showDialog() == BDialogResult.OK ) {
                                        VsqID t = (VsqID)selected.ID.clone();
                                        t.VibratoHandle = dlg.getVibratoHandle();
                                        if ( t.VibratoHandle != null ) {
                                            int vibrato_length = t.VibratoHandle.getLength();
                                            int note_length = selected.ID.getLength();
                                            t.VibratoDelay = note_length - vibrato_length;
                                        }
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(),
                                                                                    selected.InternalID,
                                                                                    t ) );
                                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                        setEdited( true );
                                        refreshScreen();
                                    }
                                } catch ( Exception ex ) {
                                } finally {
                                    if ( dlg != null ) {
                                        try {
                                            dlg.close();
                                        } catch ( Exception ex2 ) {
                                        }
                                    }
                                }
                                return;
                            }
                            break;
                        }

                    }
                }
            }

            // 必要な操作が何も無ければ，クリック位置にソングポジションを移動
            if ( e.Button == BMouseButtons.Left && AppManager.keyWidth < e.X ) {
                int unit = AppManager.getPositionQuantizeClock();
                int clock = AppManager.clockFromXCoord( e.X );
                int odd = clock % unit;
                clock -= odd;
                if ( odd > unit / 2 ) {
                    clock += unit;
                }
                AppManager.setCurrentClock( clock );
            }
        }

        private void pictPianoRoll_MouseDown( Object sender, BMouseEventArgs e0 ) {
#if DEBUG
            AppManager.debugWriteLine( "pictPianoRoll_MouseDown" );
#endif
            if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                return;
            }

            BMouseButtons btn0 = e0.Button;
            if ( AppManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier && m_spacekey_downed && e0.Button == BMouseButtons.Left ) {
                btn0 = BMouseButtons.Middle;
            }
            BMouseEventArgs e = new BMouseEventArgs( btn0, e0.Clicks, e0.X, e0.Y, e0.Delta );

            m_mouse_moved = false;
            if ( !AppManager.isPlaying() && 0 <= e.X && e.X <= AppManager.keyWidth ) {
                int note = noteFromYCoord( e.Y );
                if ( 0 <= note && note <= 126 ) {
                    if ( e.Button == BMouseButtons.Left ) {
                        KeySoundPlayer.Play( note );
                    }
                    return;
                }
            }

            AppManager.clearSelectedTempo();
            AppManager.clearSelectedTimesig();
            AppManager.clearSelectedPoint();
            /*if ( e.Button == BMouseButtons.Left ) {
                AppManager.selectedRegionEnabled = false;
            }*/

            m_mouse_downed = true;
            m_button_initial = new Point( e.X, e.Y );
            int modefier = PortUtil.getCurrentModifierKey();
            if ( m_txtbox_track_name != null ) {
#if JAVA
                m_txtbox_track_name.setVisible( false );
#else
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
#endif
                m_txtbox_track_name = null;
            }

            EditTool selected_tool = AppManager.getSelectedTool();
#if ENABLE_SCRIPT
            if ( selected_tool != EditTool.PALETTE_TOOL && e.Button == BMouseButtons.Middle )
#else
            if ( e.Button == BMouseButtons.Middle )
#endif
            {
                AppManager.setEditMode( EditMode.MIDDLE_DRAG );
                m_middle_button_vscroll = vScroll.getValue();
                m_middle_button_hscroll = hScroll.getValue();
                return;
            }

            ByRef<Rectangle> out_rect = new ByRef<Rectangle>();
            VsqEvent item = getItemAtClickedPosition( new Point( e.X, e.Y ), out_rect );
            Rectangle rect = out_rect.value;

#if ENABLE_SCRIPT
            if ( selected_tool == EditTool.PALETTE_TOOL && item == null && e.Button == BMouseButtons.Middle ) {
                AppManager.setEditMode( EditMode.MIDDLE_DRAG );
                m_middle_button_vscroll = vScroll.getValue();
                m_middle_button_hscroll = hScroll.getValue();
                return;
            }
#endif

            int selected_track = AppManager.getSelected();

            // マウス位置にある音符を検索
            if ( item == null ) {
                if ( e.Button == BMouseButtons.Left ) {
                    AppManager.setWholeSelectedIntervalEnabled( false );
                }
                #region 音符がなかった時
#if DEBUG
                AppManager.debugWriteLine( "    No Event" );
#endif
                if ( AppManager.getLastSelectedEvent() != null ) {
                    executeLyricChangeCommand();
                }
                boolean start_mouse_hover_generator = true;

                // CTRLキーを押しながら範囲選択
                if ( (modefier & s_modifier_key) == s_modifier_key ) {
                    AppManager.setWholeSelectedIntervalEnabled( true );
                    AppManager.setCurveSelectedIntervalEnabled( false );
                    AppManager.clearSelectedPoint();
                    int stdx = AppManager.startToDrawX;
                    int x = e.X + stdx;
                    if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                        int clock = AppManager.clockFromXCoord( e.X );
                        int unit = AppManager.getPositionQuantizeClock();
                        int odd = clock % unit;
                        int nclock = clock;
                        nclock -= odd;
                        if ( odd > unit / 2 ) {
                            nclock += unit;
                        }
                        x = AppManager.xCoordFromClocks( nclock ) + stdx;
                    }
                    int clock_at_x = AppManager.clockFromXCoord( x - stdx );
                    AppManager.wholeSelectedInterval = new SelectedRegion( clock_at_x );
                    AppManager.wholeSelectedInterval.setEnd( clock_at_x );
                    AppManager.isPointerDowned = true;
                } else {
                    boolean vibrato_found = false;
                    if ( selected_tool == EditTool.LINE || selected_tool == EditTool.PENCIL ) {
                        // ビブラート範囲の編集
                        int px_vibrato_length = 0;
                        int stdx = AppManager.startToDrawX;
                        int stdy = getStartToDrawY();
                        m_vibrato_editing_id = -1;
#if USE_DOBJ
                        Rectangle pxFound = new Rectangle();
                        Vector<DrawObject> target_list = AppManager.drawObjects.get( selected_track - 1 );
                        int count = target_list.size();
                        for ( int i = 0; i < count; i++ ) {
                            DrawObject dobj = target_list.get( i );
                            if ( dobj.pxRectangle.width <= dobj.pxVibratoDelay ) {
                                continue;
                            }
                            if ( dobj.pxRectangle.x + AppManager.keyWidth + dobj.pxRectangle.width - stdx < 0 ) {
                                continue;
                            } else if ( pictPianoRoll.getWidth() < dobj.pxRectangle.x + AppManager.keyWidth - stdx ) {
                                break;
                            }
                            Rectangle rc = new Rectangle( dobj.pxRectangle.x + AppManager.keyWidth + dobj.pxVibratoDelay - stdx - _EDIT_HANDLE_WIDTH / 2,
                                                dobj.pxRectangle.y + AppManager.editorConfig.PxTrackHeight - stdy,
                                                _EDIT_HANDLE_WIDTH,
                                                AppManager.editorConfig.PxTrackHeight );
#else
                        int clock = 0;
                        int note = 0;
                        int length = 0;
                        for ( Iterator itr = AppManager.VsqFile.getTrack( AppManager.Selected ).getNoteEventIterator(); itr.hasNext(); ){
                            VsqEvent evnt = (VsqEvent)itr.next();
                            if ( evnt.ID.VibratoHandle == null ){
                                continue;
                            }
                            int event_ex = XCoordFromClocks( evnt.Clock + evnt.ID.Length );
                            if ( event_ex < 0 ) {
                                continue;
                            }
                            int event_sx = XCoordFromClocks( evnt.Clock );
                            if ( pictPianoRoll.getWidth() < event_sx ) {
                                break;
                            }
                            int vib_sx = XCoordFromClocks( evnt.Clock + evnt.ID.VibratoDelay );
                            Rectangle rc = new Rectangle( event_sx - _EDIT_HANDLE_WIDTH / 2,
                                                          YCoordFromNote( evnt.ID.Note ) + AppManager.EditorConfig.PxTrackHeight,
                                                          _EDIT_HANDLE_WIDTH,
                                                          AppManager.EditorConfig.PxTrackHeight );
#endif
                            if ( isInRect( new Point( e.X, e.Y ), rc ) ) {
                                vibrato_found = true;
#if USE_DOBJ
                                m_vibrato_editing_id = dobj.internalID;
                                pxFound = dobj.pxRectangle;
                                pxFound.x += AppManager.keyWidth;
                                px_vibrato_length = dobj.pxRectangle.width - dobj.pxVibratoDelay;
#else
                                m_vibrato_editing_id = evnt.InternalID;
                                clock = evnt.Clock + evnt.ID.VibratoDelay;
                                note = evnt.ID.Note - 1;
                                length = evnt.ID.Length;
                                px_vibrato_length = event_ex - vib_sx;
#endif
                                break;
                            }
                        }
                        if ( vibrato_found ) {
#if USE_DOBJ
                            int clock = AppManager.clockFromXCoord( pxFound.x + pxFound.width - px_vibrato_length - stdx );
                            int note = noteFromYCoord( pxFound.y + AppManager.editorConfig.PxTrackHeight - stdy );
                            int length = (int)(pxFound.width / AppManager.scaleX);
#endif
                            AppManager.addingEvent = new VsqEvent( clock, new VsqID( 0 ) );
                            AppManager.addingEvent.ID.type = VsqIDType.Anote;
                            AppManager.addingEvent.ID.Note = note;
                            AppManager.addingEvent.ID.setLength( (int)(px_vibrato_length / AppManager.scaleX) );
                            AppManager.addingEventLength = length;
                            AppManager.addingEvent.ID.VibratoDelay = length - (int)(px_vibrato_length / AppManager.scaleX);
                            AppManager.setEditMode( EditMode.EDIT_VIBRATO_DELAY );
                            start_mouse_hover_generator = false;
                        }
                    }
                    if ( !vibrato_found ) {
                        if ( (selected_tool == EditTool.PENCIL || selected_tool == EditTool.LINE) &&
                            e.Button == BMouseButtons.Left &&
                            e.X >= AppManager.keyWidth ) {
                            int clock = AppManager.clockFromXCoord( e.X );
                            if ( AppManager.getVsqFile().getPreMeasureClocks() - AppManager.editorConfig.PxTolerance / AppManager.scaleX <= clock ) {
                                //10ピクセルまでは許容範囲
                                if ( AppManager.getVsqFile().getPreMeasureClocks() > clock ) { //だけど矯正するよ。
                                    clock = AppManager.getVsqFile().getPreMeasureClocks();
                                }
                                int note = noteFromYCoord( e.Y );
                                AppManager.clearSelectedEvent();
                                int unit = AppManager.getPositionQuantizeClock();
                                int odd = clock % unit;
                                int new_clock = clock - odd;
                                if ( odd > unit / 2 ) {
                                    new_clock += unit;
                                }
                                AppManager.addingEvent = new VsqEvent( new_clock, new VsqID( 0 ) );
                                AppManager.addingEvent.ID.PMBendDepth = AppManager.editorConfig.DefaultPMBendDepth;
                                AppManager.addingEvent.ID.PMBendLength = AppManager.editorConfig.DefaultPMBendLength;
                                AppManager.addingEvent.ID.PMbPortamentoUse = AppManager.editorConfig.DefaultPMbPortamentoUse;
                                AppManager.addingEvent.ID.DEMdecGainRate = AppManager.editorConfig.DefaultDEMdecGainRate;
                                AppManager.addingEvent.ID.DEMaccent = AppManager.editorConfig.DefaultDEMaccent;
                                if ( m_pencil_mode.getMode() == PencilModeEnum.Off ) {
                                    AppManager.setEditMode( EditMode.ADD_ENTRY );
                                    m_button_initial = new Point( e.X, e.Y );
                                    AppManager.addingEvent.ID.setLength( 0 );
                                    AppManager.addingEvent.ID.Note = note;
                                    setCursor( new Cursor( java.awt.Cursor.DEFAULT_CURSOR ) );
#if DEBUG
                                    AppManager.debugWriteLine( "    EditMode=" + AppManager.getEditMode() );
#endif
                                } else {
                                    AppManager.setEditMode( EditMode.ADD_FIXED_LENGTH_ENTRY );
                                    AppManager.addingEvent.ID.setLength( m_pencil_mode.getUnitLength() );
                                    AppManager.addingEvent.ID.Note = note;
                                    setCursor( new Cursor( java.awt.Cursor.DEFAULT_CURSOR ) );
                                }
                            } else {
#if !JAVA
                                SystemSounds.Asterisk.Play();
#endif
                            }
#if ENABLE_SCRIPT
                        } else if ( (selected_tool == EditTool.ARROW || selected_tool == EditTool.PALETTE_TOOL) && e.Button == BMouseButtons.Left ) {
#else
                        } else if ( (selected_tool == EditTool.ARROW) && e.Button == BMouseButtons.Left ) {
#endif
                            AppManager.setWholeSelectedIntervalEnabled( false );
                            AppManager.clearSelectedEvent();
                            AppManager.mouseDownLocation = new Point( e.X + AppManager.startToDrawX, e.Y + getStartToDrawY() );
                            AppManager.isPointerDowned = true;
#if DEBUG
                            AppManager.debugWriteLine( "    EditMode=" + AppManager.getEditMode() );
#endif
                        }
                    }
                }
                if ( e.Button == BMouseButtons.Right && !AppManager.editorConfig.PlayPreviewWhenRightClick ) {
                    start_mouse_hover_generator = false;
                }
#if ENABLE_MOUSEHOVER
                if ( start_mouse_hover_generator ) {
                    m_mouse_hover_thread = new Thread( new ParameterizedThreadStart( MouseHoverEventGenerator ) );
                    m_mouse_hover_thread.Start( noteFromYCoord( e.Y ) );
                }
#endif
                #endregion
            } else {
                #region 音符があった時
#if DEBUG
                AppManager.debugWriteLine( "    Event Found" );
#endif
                if ( AppManager.isSelectedEventContains( AppManager.getSelected(), item.InternalID ) ) {
                    executeLyricChangeCommand();
                }
                hideInputTextBox();
                if ( selected_tool != EditTool.ERASER ) {
#if ENABLE_MOUSEHOVER
                    m_mouse_hover_thread = new Thread( new ParameterizedThreadStart( MouseHoverEventGenerator ) );
                    m_mouse_hover_thread.Start( item.ID.Note );
#endif
                }
                // まず、両端の編集モードに移行可能かどうか調べる
#if ENABLE_SCRIPT
                if ( selected_tool != EditTool.ERASER && selected_tool != EditTool.PALETTE_TOOL && e.Button == BMouseButtons.Left ) {
#else
                if ( selected_tool != EditTool.ERASER && e.Button == BMouseButtons.Left ) {
#endif
                    int stdx = AppManager.startToDrawX;
                    int stdy = getStartToDrawY();
#if USE_DOBJ
                    for ( Iterator itr = AppManager.drawObjects.get( selected_track - 1 ).iterator(); itr.hasNext(); ) {
                        DrawObject dobj = (DrawObject)itr.next();
                        Rectangle rc = new Rectangle( dobj.pxRectangle.x + AppManager.keyWidth - stdx, dobj.pxRectangle.y - stdy, _EDIT_HANDLE_WIDTH, dobj.pxRectangle.height );
#else
                    for( Iterator itr = AppManager.VsqFile.getTrack( AppManager.Selected ).getNoteEventIterator(); itr.hasNext(); ){
                        VsqEvent evnt = (VsqEvent)itr.next();
                        int event_ex = XCoordFromClocks( evnt.Clock + evnt.ID.Length );
                        if ( event_ex < 0 ) {
                            continue;
                        }
                        int event_sx = XCoordFromClocks( evnt.Clock );
                        if ( pictPianoRoll.getWidth() < event_sx ) {
                            break;
                        }
                        int event_sy = YCoordFromNote( evnt.ID.Note, stdy );

                        // 左端
                        Rectangle rc = new Rectangle( event_sx - _EDIT_HANDLE_WIDTH / 2, 
                                                      event_sy,
                                                      _EDIT_HANDLE_WIDTH,
                                                      AppManager.EditorConfig.PxTrackHeight );
#endif
                        if ( isInRect( new Point( e.X, e.Y ), rc ) ) {
                            AppManager.setWholeSelectedIntervalEnabled( false );
                            AppManager.setEditMode( EditMode.EDIT_LEFT_EDGE );
                            if ( !AppManager.isSelectedEventContains( selected_track, item.InternalID ) ) {
                                AppManager.clearSelectedEvent();
                            }
                            AppManager.addSelectedEvent( item.InternalID );
#if JAVA
                            setCursor( new Cursor( Cursor.S_RESIZE_CURSOR ) );
#else
                            this.Cursor = System.Windows.Forms.Cursors.VSplit;
#endif
                            refreshScreen();
#if DEBUG
                            AppManager.debugWriteLine( "    EditMode=" + AppManager.getEditMode() );
#endif
                            return;
                        }
#if USE_DOBJ
                        rc = new Rectangle( dobj.pxRectangle.x + AppManager.keyWidth + dobj.pxRectangle.width - stdx - _EDIT_HANDLE_WIDTH,
                                            dobj.pxRectangle.y - stdy,
                                            _EDIT_HANDLE_WIDTH,
                                            dobj.pxRectangle.height );
#else
                        rect = new Rectangle( event_ex - _EDIT_HANDLE_WIDTH / 2,
                                              event_sy,
                                              _EDIT_HANDLE_WIDTH,
                                              AppManager.EditorConfig.PxTrackHeight );
#endif
                        if ( isInRect( new Point( e.X, e.Y ), rc ) ) {
                            AppManager.setWholeSelectedIntervalEnabled( false );
                            AppManager.setEditMode( EditMode.EDIT_RIGHT_EDGE );
                            if ( !AppManager.isSelectedEventContains( selected_track, item.InternalID ) ) {
                                AppManager.clearSelectedEvent();
                            }
                            AppManager.addSelectedEvent( item.InternalID );
#if JAVA
                            setCursor( new Cursor( Cursor.S_RESIZE_CURSOR ) );
#else
                            this.Cursor = System.Windows.Forms.Cursors.VSplit;
#endif
                            refreshScreen();
#if DEBUG
                            AppManager.debugWriteLine( "    EditMode=" + AppManager.getEditMode() );
#endif
                            return;
                        }
                    }
                }
                if ( e.Button == BMouseButtons.Left || e.Button == BMouseButtons.Middle ) {
#if ENABLE_SCRIPT
                    if ( selected_tool == EditTool.PALETTE_TOOL ) {
                        AppManager.setWholeSelectedIntervalEnabled( false );
                        AppManager.setEditMode( EditMode.NONE );
                        AppManager.clearSelectedEvent();
                        AppManager.addSelectedEvent( item.InternalID );
                    } else
#endif
                        if ( selected_tool != EditTool.ERASER ) {
                            m_mouse_move_init = new Point( e.X + AppManager.startToDrawX, e.Y + getStartToDrawY() );
                            int head_x = AppManager.xCoordFromClocks( item.Clock );
                            m_mouse_move_offset = e.X - head_x;
                            if ( (modefier & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK ) {
                                // 範囲選択
                                int last_id = AppManager.getLastSelectedEvent().original.InternalID;
                                int last_clock = 0;
                                int this_clock = 0;
                                boolean this_found = false, last_found = false;
                                for ( Iterator itr = AppManager.getVsqFile().Track.get( selected_track ).getEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ev = (VsqEvent)itr.next();
                                    if ( ev.InternalID == last_id ) {
                                        last_clock = ev.Clock;
                                        last_found = true;
                                    } else if ( ev.InternalID == item.InternalID ) {
                                        this_clock = ev.Clock;
                                        this_found = true;
                                    }
                                    if ( last_found && this_found ) {
                                        break;
                                    }
                                }
                                int start = Math.Min( last_clock, this_clock );
                                int end = Math.Max( last_clock, this_clock );
                                Vector<Integer> add_required = new Vector<Integer>();
                                for ( Iterator itr = AppManager.getVsqFile().Track.get( selected_track ).getEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ev = (VsqEvent)itr.next();
                                    if ( start <= ev.Clock && ev.Clock <= end ) {
                                        if ( !add_required.contains( ev.InternalID ) ) {
                                            add_required.add( ev.InternalID );
                                        }
                                    }
                                }
                                if ( !add_required.contains( item.InternalID ) ) {
                                    add_required.add( item.InternalID );
                                }
                                AppManager.addSelectedEventAll( add_required );
                            } else if ( (modefier & s_modifier_key) == s_modifier_key ) {
                                // CTRLキーを押しながら選択／選択解除
                                if ( AppManager.isSelectedEventContains( selected_track, item.InternalID ) ) {
                                    AppManager.removeSelectedEvent( item.InternalID );
                                } else {
                                    AppManager.addSelectedEvent( item.InternalID );
                                }
                            } else {
                                if ( !AppManager.isSelectedEventContains( selected_track, item.InternalID ) ) {
                                    // MouseDownしたアイテムが、まだ選択されていなかった場合。当該アイテム単独に選択しなおす
                                    AppManager.clearSelectedEvent();
                                }
                                AppManager.addSelectedEvent( item.InternalID );
                            }

                            // 範囲選択モードで、かつマウス位置の音符がその範囲に入っていた場合にのみ、MOVE_ENTRY_WHOLE_WAIT_MOVEに移行
                            if ( AppManager.isWholeSelectedIntervalEnabled() &&
                                 AppManager.wholeSelectedInterval.getStart() <= item.Clock && item.Clock <= AppManager.wholeSelectedInterval.getEnd() ) {
                                AppManager.setEditMode( EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE );
                                AppManager.wholeSelectedIntervalStartForMoving = AppManager.wholeSelectedInterval.getStart();
                            } else {
                                AppManager.setWholeSelectedIntervalEnabled( false );
                                AppManager.setEditMode( EditMode.MOVE_ENTRY_WAIT_MOVE );
                            }

                            setCursor( new Cursor( java.awt.Cursor.HAND_CURSOR ) );
#if DEBUG
                            AppManager.debugWriteLine( "    EditMode=" + AppManager.getEditMode() );
                            AppManager.debugWriteLine( "    m_config.SelectedEvent.Count=" + AppManager.getSelectedEventCount() );
#endif
                        }
                }
                #endregion
            }
            refreshScreen();
        }

        private void pictPianoRoll_MouseMove( Object sender, BMouseEventArgs e ) {
            if ( m_form_activated ) {
#if ENABLE_PROPERTY
#if JAVA
                if ( AppManager.inputTextBox != null && !AppManager.inputTextBox.isVisible() && !AppManager.propertyPanel.isEditing() ) {
#else
                if ( AppManager.inputTextBox != null && !AppManager.inputTextBox.IsDisposed && !AppManager.inputTextBox.isVisible() && !AppManager.propertyPanel.isEditing() ) {
#endif
#else
#if JAVA
                if ( AppManager.inputTextBox != null && !AppManager.inputTextBox.isVisible() ) {
#else
                if ( AppManager.inputTextBox != null && !AppManager.inputTextBox.IsDisposed && !AppManager.inputTextBox.isVisible() ) {
#endif
#endif
                    pictPianoRoll.requestFocus();
                }
            }
            EditMode edit_mode = AppManager.getEditMode();
            if ( !m_mouse_moved && edit_mode == EditMode.MIDDLE_DRAG ) {
#if JAVA
                setCursor( new Cursor( java.awt.Cursor.MOVE_CURSOR ) );
#else
                this.Cursor = HAND;
#endif
            }

            if ( e.X != m_button_initial.x || e.Y != m_button_initial.y ) {
                m_mouse_moved = true;
            }
            if ( !(edit_mode == EditMode.MIDDLE_DRAG) && AppManager.isPlaying() ) {
                return;
            }

            if ( edit_mode == EditMode.MOVE_ENTRY_WAIT_MOVE ||
                 edit_mode == EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE ) {
                int x = e.X + AppManager.startToDrawX;
                int y = e.Y + getStartToDrawY();
                if ( m_mouse_move_init.x != x || m_mouse_move_init.y != y ) {
                    if ( edit_mode == EditMode.MOVE_ENTRY_WAIT_MOVE ) {
                        AppManager.setEditMode( EditMode.MOVE_ENTRY );
                        edit_mode = EditMode.MOVE_ENTRY;
                    } else {
                        AppManager.setEditMode( EditMode.MOVE_ENTRY_WHOLE );
                        edit_mode = EditMode.MOVE_ENTRY_WHOLE;
                    }
                }
            }

#if ENABLE_MOUSEHOVER
            if ( m_mouse_moved && m_mouse_hover_thread != null ) {
                m_mouse_hover_thread.Abort();
            }
#endif

            int clock = AppManager.clockFromXCoord( e.X );
            if ( m_mouse_downed ) {
                if ( m_ext_dragx == ExtDragXMode.NONE ) {
                    if ( AppManager.keyWidth > e.X ) {
                        m_ext_dragx = ExtDragXMode.LEFT;
                    } else if ( pictPianoRoll.getWidth() < e.X ) {
                        m_ext_dragx = ExtDragXMode.RIGHT;
                    }
                } else {
                    if ( AppManager.keyWidth <= e.X && e.X <= pictPianoRoll.getWidth() ) {
                        m_ext_dragx = ExtDragXMode.NONE;
                    }
                }

                if ( m_ext_dragy == ExtDragYMode.NONE ) {
                    if ( 0 > e.Y ) {
                        m_ext_dragy = ExtDragYMode.UP;
                    } else if ( pictPianoRoll.getHeight() < e.Y ) {
                        m_ext_dragy = ExtDragYMode.DOWN;
                    }
                } else {
                    if ( 0 <= e.Y && e.Y <= pictPianoRoll.getHeight() ) {
                        m_ext_dragy = ExtDragYMode.NONE;
                    }
                }
            } else {
                m_ext_dragx = ExtDragXMode.NONE;
                m_ext_dragy = ExtDragYMode.NONE;
            }

            if ( m_ext_dragx == ExtDragXMode.RIGHT || m_ext_dragx == ExtDragXMode.LEFT ) {
                double now = PortUtil.getCurrentTime();
                double dt = now - m_timer_drag_last_ignitted;
                m_timer_drag_last_ignitted = now;
                int px_move = AppManager.editorConfig.MouseDragIncrement;
                if ( px_move / dt > AppManager.editorConfig.MouseDragMaximumRate ) {
                    px_move = (int)(dt * AppManager.editorConfig.MouseDragMaximumRate);
                }
                double d_draft;
                if ( m_ext_dragx == ExtDragXMode.RIGHT ) {
                    int right_clock = AppManager.clockFromXCoord( pictPianoRoll.getWidth() );
                    int dclock = (int)(px_move / AppManager.scaleX);
                    d_draft = (73 - pictPianoRoll.getWidth()) / AppManager.scaleX + right_clock + dclock;
                } else {
                    px_move *= -1;
                    int left_clock = AppManager.clockFromXCoord( AppManager.keyWidth );
                    int dclock = (int)(px_move / AppManager.scaleX);
                    d_draft = (73 - AppManager.keyWidth) / AppManager.scaleX + left_clock + dclock;
                }
                if ( d_draft < 0.0 ) {
                    d_draft = 0.0;
                }
                int draft = (int)d_draft;
                if ( hScroll.getMaximum() < draft ) {
                    if ( edit_mode == EditMode.ADD_ENTRY || edit_mode == EditMode.MOVE_ENTRY || edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY ) {
                        hScroll.setMaximum( draft );
                    } else {
                        draft = hScroll.getMaximum();
                    }
                }
                if ( draft < hScroll.getMinimum() ) {
                    draft = hScroll.getMinimum();
                }
                hScroll.setValue( draft );
            }
            if ( m_ext_dragy == ExtDragYMode.UP || m_ext_dragy == ExtDragYMode.DOWN ) {
                double now = PortUtil.getCurrentTime();
                double dt = now - m_timer_drag_last_ignitted;
                m_timer_drag_last_ignitted = now;
                int px_move = AppManager.editorConfig.MouseDragIncrement;
                if ( px_move / dt > AppManager.editorConfig.MouseDragMaximumRate ) {
                    px_move = (int)(dt * AppManager.editorConfig.MouseDragMaximumRate);
                }
                if ( m_ext_dragy == ExtDragYMode.UP ) {
                    px_move *= -1;
                }
                int draft_stdy = getStartToDrawY() + px_move;
                int draft = (int)((draft_stdy * (double)vScroll.getMaximum()) / (128.0 * AppManager.editorConfig.PxTrackHeight - vScroll.getHeight()));
                if ( draft < 0 ) {
                    draft = 0;
                }
                int df = (int)draft;
                if ( df < vScroll.getMinimum() ) {
                    df = vScroll.getMinimum();
                } else if ( vScroll.getMaximum() < df ) {
                    df = vScroll.getMaximum();
                }
                vScroll.setValue( df );
            }

            // 選択範囲にあるイベントを選択．
            int stdy = getStartToDrawY();
            if ( AppManager.isPointerDowned ) {
                if ( AppManager.isWholeSelectedIntervalEnabled() ) {
                    int stdx = AppManager.startToDrawX;
                    int x = e.X + stdx;
                    if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                        int clock1 = AppManager.clockFromXCoord( e.X );
                        int unit = AppManager.getPositionQuantizeClock();
                        int odd = clock1 % unit;
                        int nclock = clock1;
                        nclock -= odd;
                        if ( odd > unit / 2 ) {
                            nclock += unit;
                        }
                        x = AppManager.xCoordFromClocks( nclock ) + stdx;
                    }
                    int clock_at_x = AppManager.clockFromXCoord( x - stdx );
                    AppManager.wholeSelectedInterval.setEnd( clock_at_x );
                } else {
                    Point mouse = new Point( e.X + AppManager.startToDrawX, e.Y + getStartToDrawY() );
                    int tx, ty, twidth, theight;
                    int lx = AppManager.mouseDownLocation.x;
                    if ( lx < mouse.x ) {
                        tx = lx;
                        twidth = mouse.x - lx;
                    } else {
                        tx = mouse.x;
                        twidth = lx - mouse.x;
                    }
                    int ly = AppManager.mouseDownLocation.y;
                    if ( ly < mouse.y ) {
                        ty = ly;
                        theight = mouse.y - ly;
                    } else {
                        ty = mouse.y;
                        theight = ly - mouse.y;
                    }

                    Rectangle rect = new Rectangle( tx, ty, twidth, theight );
                    Vector<Integer> add_required = new Vector<Integer>();
                    int internal_id = -1;
#if USE_DOBJ
                    for ( Iterator itr = AppManager.drawObjects.get( AppManager.getSelected() - 1 ).iterator(); itr.hasNext(); ) {
                        DrawObject dobj = (DrawObject)itr.next();
                        int x0 = dobj.pxRectangle.x + AppManager.keyWidth;
                        int x1 = dobj.pxRectangle.x + AppManager.keyWidth + dobj.pxRectangle.width;
                        int y0 = dobj.pxRectangle.y;
                        int y1 = dobj.pxRectangle.y + dobj.pxRectangle.height;
                        internal_id = dobj.internalID;
#else
                    //int stdy = StartToDrawY;
                    for ( Iterator itr = AppManager.VsqFile.getTrack( AppManager.Selected ).getNoteEventIterator(); itr.hasNext(); ){
                        VsqEvent evnt = (VsqEvent)itr.next();
                        int x0 = XCoordFromClocks( evnt.Clock );
                        int x1 = XCoordFromClocks( evnt.Clock + evnt.ID.Length );
                        int y0 = YCoordFromNote( evnt.ID.Note, stdy );
                        int y1 = y0 + AppManager.EditorConfig.PxTrackHeight;
                        internal_id = evnt.InternalID;
#endif
                        if ( x1 < tx ) {
                            continue;
                        }
                        if ( tx + twidth < x0 ) {
                            break;
                        }
                        boolean found = isInRect( new Point( x0, y0 ), rect ) | isInRect( new Point( x0, y1 ), rect ) | isInRect( new Point( x1, y0 ), rect ) | isInRect( new Point( x1, y1 ), rect );
                        if ( found ) {
                            add_required.add( internal_id );
                        } else {
                            if ( x0 <= tx && tx + twidth <= x1 ) {
                                if ( ty < y0 ) {
                                    if ( y0 <= ty + theight ) {
                                        add_required.add( internal_id );
                                    }
                                } else if ( y0 <= ty && ty < y1 ) {
                                    add_required.add( internal_id );
                                }
                            } else if ( y0 <= ty && ty + theight <= y1 ) {
                                if ( tx < x0 ) {
                                    if ( x0 <= tx + twidth ) {
                                        add_required.add( internal_id );
                                    }
                                } else if ( x0 <= tx && tx < x1 ) {
                                    add_required.add( internal_id );
                                }
                            }
                        }
                    }
                    Vector<Integer> remove_required = new Vector<Integer>();
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                        SelectedEventEntry selected = (SelectedEventEntry)itr.next();
                        if ( !add_required.contains( selected.original.InternalID ) ) {
                            remove_required.add( selected.original.InternalID );
                        }
                    }
                    if ( remove_required.size() > 0 ) {
                        AppManager.removeSelectedEventRange( PortUtil.convertIntArray( remove_required.toArray( new Integer[] { } ) ) );
                    }
                    for ( Iterator itr = add_required.iterator(); itr.hasNext(); ) {
                        int id = (Integer)itr.next();
                        if ( AppManager.isSelectedEventContains( AppManager.getSelected(), id ) ) {
                            itr.remove();
                        }
                    }
                    AppManager.addSelectedEventAll( add_required );
                }
            }

            if ( edit_mode == EditMode.MIDDLE_DRAG ) {
                #region MiddleDrag
                int dx = e.X - m_button_initial.x;
                int dy = e.Y - m_button_initial.y;
                double new_vscroll_value = (double)m_middle_button_vscroll - dy * (double)vScroll.getMaximum() / (128.0 * AppManager.editorConfig.PxTrackHeight - (double)vScroll.getHeight());
                double new_hscroll_value = (double)m_middle_button_hscroll - (double)dx / AppManager.scaleX;
                if ( new_vscroll_value < vScroll.getMinimum() ) {
                    vScroll.setValue( vScroll.getMinimum() );
                } else if ( vScroll.getMaximum() < new_vscroll_value ) {
                    vScroll.setValue( vScroll.getMaximum() );
                } else {
                    vScroll.setValue( (int)new_vscroll_value );
                }
                if ( new_hscroll_value < hScroll.getMinimum() ) {
                    hScroll.setValue( hScroll.getMinimum() );
                } else if ( hScroll.getMaximum() < new_hscroll_value ) {
                    hScroll.setValue( hScroll.getMaximum() );
                } else {
                    hScroll.setValue( (int)new_hscroll_value );
                }
                if ( AppManager.isPlaying() ) {
                    return;
                }
                #endregion
                return;
            } else if ( edit_mode == EditMode.ADD_ENTRY ) {
                #region AddEntry
                int unit = AppManager.getLengthQuantizeClock();
                int length = clock - AppManager.addingEvent.Clock;
                int odd = length % unit;
                int new_length = length - odd;

                if ( unit * AppManager.scaleX > 10 ) { //これをしないと、グリッド2個分増えることがある
                    int next_clock = AppManager.clockFromXCoord( e.X + 10 );
                    int next_length = next_clock - AppManager.addingEvent.Clock;
                    int next_new_length = next_length - (next_length % unit);
                    if ( next_new_length == new_length + unit ) {
                        new_length = next_new_length;
                    }
                }

                if ( new_length <= 0 ) {
                    new_length = 0;
                }
                AppManager.addingEvent.ID.setLength( new_length );
                #endregion
            } else if ( edit_mode == EditMode.MOVE_ENTRY || edit_mode == EditMode.MOVE_ENTRY_WHOLE ) {
                #region MOVE_ENTRY, MOVE_ENTRY_WHOLE
                if ( AppManager.getSelectedEventCount() > 0 ) {
                    VsqEvent original = AppManager.getLastSelectedEvent().original;
                    int note = noteFromYCoord( e.Y );                           // 現在のマウス位置でのnote
                    int note_init = original.ID.Note;
                    int dnote = (edit_mode == EditMode.MOVE_ENTRY) ? note - note_init : 0;

                    int tclock = AppManager.clockFromXCoord( e.X - m_mouse_move_offset );
                    int clock_init = original.Clock;

                    int dclock = tclock - clock_init;

                    if ( AppManager.editorConfig.getPositionQuantize() != QuantizeMode.off ) {
                        int unit = AppManager.getPositionQuantizeClock();
                        int new_clock = original.Clock + dclock;
                        int odd = new_clock % unit;
                        new_clock -= odd;
                        if ( odd > unit / 2 ) {
                            new_clock += unit;
                        }
                        dclock = new_clock - clock_init;
                    }

                    AppManager.wholeSelectedIntervalStartForMoving = AppManager.wholeSelectedInterval.getStart() + dclock;

                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                        SelectedEventEntry item = (SelectedEventEntry)itr.next();
                        int new_clock = item.original.Clock + dclock;
                        int new_note = item.original.ID.Note + dnote;
                        item.editing.Clock = new_clock;
                        item.editing.ID.Note = new_note;
                    }
                }
                #endregion
            } else if ( edit_mode == EditMode.EDIT_LEFT_EDGE ) {
                #region EditLeftEdge
                int unit = AppManager.getLengthQuantizeClock();
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                int clock_init = original.Clock;
                int dclock = clock - clock_init;
                for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                    SelectedEventEntry item = (SelectedEventEntry)itr.next();
                    int end_clock = item.original.Clock + item.original.ID.getLength();
                    int new_clock = item.original.Clock + dclock;
                    int length = end_clock - new_clock;
                    int odd = length % unit;
                    int new_length = length - odd;
                    if ( odd > unit / 2 ) {
                        new_length += unit;
                    }
                    if ( new_length <= 0 ) {
                        new_length = unit;
                    }
                    item.editing.Clock = end_clock - new_length;
                    item.editing.ID.setLength( new_length );
                }
                #endregion
            } else if ( edit_mode == EditMode.EDIT_RIGHT_EDGE ) {
                #region EditRightEdge
                int unit = AppManager.getLengthQuantizeClock();

                VsqEvent original = AppManager.getLastSelectedEvent().original;
                int dlength = clock - (original.Clock + original.ID.getLength());
                for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                    SelectedEventEntry item = (SelectedEventEntry)itr.next();
                    int length = item.original.ID.getLength() + dlength;
                    int odd = length % unit;
                    int new_length = length - odd;
                    if ( odd > unit / 2 ) {
                        new_length += unit;
                    }
                    if ( new_length <= 0 ) {
                        new_length = unit;
                    }
                    item.editing.ID.setLength( new_length );
                }
                #endregion
            } else if ( edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY ) {
                #region AddFixedLengthEntry
                int note = noteFromYCoord( e.Y );
                int unit = AppManager.getPositionQuantizeClock();
                int new_clock = AppManager.clockFromXCoord( e.X );
                int odd = new_clock % unit;
                new_clock -= odd;
                if ( odd > unit / 2 ) {
                    new_clock += unit;
                }
                AppManager.addingEvent.ID.Note = note;
                AppManager.addingEvent.Clock = new_clock;
                #endregion
            } else if ( edit_mode == EditMode.EDIT_VIBRATO_DELAY ) {
                #region EditVibratoDelay
                int new_vibrato_start = clock;
                int old_vibrato_end = AppManager.addingEvent.Clock + AppManager.addingEvent.ID.getLength();
                int new_vibrato_length = old_vibrato_end - new_vibrato_start;
                int max_length = (int)(AppManager.addingEventLength - _PX_ACCENT_HEADER / AppManager.scaleX);
                if ( max_length < 0 ) {
                    max_length = 0;
                }
                if ( new_vibrato_length > max_length ) {
                    new_vibrato_start = old_vibrato_end - max_length;
                    new_vibrato_length = max_length;
                }
                if ( new_vibrato_length < 0 ) {
                    new_vibrato_start = old_vibrato_end;
                    new_vibrato_length = 0;
                }
                AppManager.addingEvent.Clock = new_vibrato_start;
                AppManager.addingEvent.ID.setLength( new_vibrato_length );
                updatePositionViewFromMousePosition( clock );
                if ( !timer.isRunning() ) {
                    refreshScreen();
                }
                #endregion
                return;
            }
            updatePositionViewFromMousePosition( clock );

            // カーソルの形を決める
            if ( !m_mouse_downed ) {
                boolean split_cursor = false;
                boolean hand_cursor = false;
                int stdx = AppManager.startToDrawX;
#if USE_DOBJ
                for ( Iterator itr = AppManager.drawObjects.get( AppManager.getSelected() - 1 ).iterator(); itr.hasNext(); ) {
                    DrawObject dobj = (DrawObject)itr.next();
                    // 音符左側の編集領域
#else
                for ( Iterator itr = AppManager.VsqFile.getTrack( AppManager.Selected ).getNoteEventIterator(); itr.hasNext(); ){

#endif
                    Rectangle rc = new Rectangle(
                                        dobj.pxRectangle.x + AppManager.keyWidth - stdx,
                                        dobj.pxRectangle.y - stdy,
                                        _EDIT_HANDLE_WIDTH,
                                        AppManager.editorConfig.PxTrackHeight );
                    if ( isInRect( new Point( e.X, e.Y ), rc ) ) {
                        split_cursor = true;
                        break;
                    }

                    // 音符右側の編集領域
                    rc = new Rectangle( dobj.pxRectangle.x + AppManager.keyWidth + dobj.pxRectangle.width - stdx - _EDIT_HANDLE_WIDTH,
                                        dobj.pxRectangle.y - stdy,
                                        _EDIT_HANDLE_WIDTH,
                                        AppManager.editorConfig.PxTrackHeight );
                    if ( isInRect( new Point( e.X, e.Y ), rc ) ) {
                        split_cursor = true;
                        break;
                    }

                    // 音符本体
                    rc = new Rectangle( dobj.pxRectangle.x + AppManager.keyWidth - stdx,
                                        dobj.pxRectangle.y - stdy,
                                        dobj.pxRectangle.width,
                                        dobj.pxRectangle.height );
                    if ( AppManager.editorConfig.ShowExpLine && !dobj.overlappe ) {
                        rc.height *= 2;
                        if ( isInRect( new Point( e.X, e.Y ), rc ) ) {
                            // ビブラートの開始位置
                            rc = new Rectangle( dobj.pxRectangle.x + AppManager.keyWidth + dobj.pxVibratoDelay - stdx - _EDIT_HANDLE_WIDTH / 2,
                                                dobj.pxRectangle.y + AppManager.editorConfig.PxTrackHeight - stdy,
                                                _EDIT_HANDLE_WIDTH,
                                                AppManager.editorConfig.PxTrackHeight );
                            if ( isInRect( new Point( e.X, e.Y ), rc ) ) {
                                split_cursor = true;
                                break;
                            } else {
                                hand_cursor = true;
                                break;
                            }
                        }
                    } else {
                        if ( isInRect( new Point( e.X, e.Y ), rc ) ) {
                            hand_cursor = true;
                            break;
                        }
                    }
                }

                if ( split_cursor ) {
                    setCursor( new Cursor( java.awt.Cursor.E_RESIZE_CURSOR ) );
                } else if ( hand_cursor ) {
                    setCursor( new Cursor( java.awt.Cursor.HAND_CURSOR ) );
                } else {
                    setCursor( new Cursor( java.awt.Cursor.DEFAULT_CURSOR ) );
                }
            }
            if ( !timer.isRunning() ) {
                refreshScreen();
            }
        }

        private void pictPianoRoll_MouseUp( Object sender, BMouseEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "pictureBox1_MouseUp" );
            AppManager.debugWriteLine( "    m_config.EditMode=" + AppManager.getEditMode() );
#endif
            AppManager.isPointerDowned = false;
            m_mouse_downed = false;

            int modefiers = PortUtil.getCurrentModifierKey();

            EditMode edit_mode = AppManager.getEditMode();
            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track.get( AppManager.getSelected() );
            CurveType selected_curve = trackSelector.getSelectedCurve();

            if ( edit_mode == EditMode.MIDDLE_DRAG ) {
                setCursor( new Cursor( java.awt.Cursor.DEFAULT_CURSOR ) );
            } else if ( edit_mode == EditMode.ADD_ENTRY || edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY ) {
                #region AddEntry || AddFixedLengthEntry
                if ( AppManager.getSelected() >= 0 ) {
                    if ( (AppManager.getEditMode() == EditMode.ADD_FIXED_LENGTH_ENTRY) ||
                         (AppManager.getEditMode() == EditMode.ADD_ENTRY && (m_button_initial.x != e.X || m_button_initial.y != e.Y) && AppManager.addingEvent.ID.getLength() > 0) ) {
                        LyricHandle lyric = new LyricHandle( "a", "a" );
                        VibratoHandle vibrato = null;
                        int vibrato_delay = 0;
                        if ( AppManager.editorConfig.EnableAutoVibrato ) {
                            int note_length = AppManager.addingEvent.ID.getLength();
                            // 音符位置での拍子を調べる
                            //int denom, numer;
                            Timesig timesig = AppManager.getVsqFile().getTimesigAt( AppManager.addingEvent.Clock );

                            // ビブラートを自動追加するかどうかを決める閾値
                            int autovib = AutoVibratoMinLengthUtil.getValue( AppManager.editorConfig.AutoVibratoMinimumLength );
                            int threshold = 480 * 4 / timesig.denominator * autovib;
                            if ( note_length >= threshold ) {
                                int vibrato_clocks = 0;
                                if ( AppManager.editorConfig.DefaultVibratoLength == DefaultVibratoLengthEnum.L100 ) {
                                    vibrato_clocks = note_length;
                                } else if ( AppManager.editorConfig.DefaultVibratoLength == DefaultVibratoLengthEnum.L50 ) {
                                    vibrato_clocks = note_length / 2;
                                } else if ( AppManager.editorConfig.DefaultVibratoLength == DefaultVibratoLengthEnum.L66 ) {
                                    vibrato_clocks = note_length * 2 / 3;
                                } else if ( AppManager.editorConfig.DefaultVibratoLength == DefaultVibratoLengthEnum.L75 ) {
                                    vibrato_clocks = note_length * 3 / 4;
                                }
                                SynthesizerType type = SynthesizerType.VOCALOID2;
                                String default_icon_id = AppManager.editorConfig.AutoVibratoType2;
                                if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version.Equals( VSTiProxy.RENDERER_DSB2 ) ) {
                                    type = SynthesizerType.VOCALOID1;
                                    default_icon_id = AppManager.editorConfig.AutoVibratoType1;
                                }
                                vibrato = VocaloSysUtil.getDefaultVibratoHandle( default_icon_id, vibrato_clocks, type );
                                vibrato_delay = note_length - vibrato_clocks;
                            }
                        }

                        // 自動ノーマライズのモードで、処理を分岐
                        if ( AppManager.autoNormalize ) {
                            VsqTrack work = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).clone();
                            AppManager.addingEvent.ID.type = VsqIDType.Anote;
                            AppManager.addingEvent.ID.Dynamics = 64;
                            AppManager.addingEvent.ID.VibratoHandle = vibrato;
                            AppManager.addingEvent.ID.LyricHandle = lyric;
                            AppManager.addingEvent.ID.VibratoDelay = vibrato_delay;
                            //AppManager.addingEvent.InternalID = work.GetNextId( 0 );

                            boolean changed = true;
                            while ( changed ) {
                                changed = false;
                                for ( int i = 0; i < work.getEventCount(); i++ ) {
                                    int start_clock = work.getEvent( i ).Clock;
                                    int end_clock = work.getEvent( i ).ID.getLength() + start_clock;
                                    if ( start_clock < AppManager.addingEvent.Clock && AppManager.addingEvent.Clock < end_clock ) {
                                        work.getEvent( i ).ID.setLength( AppManager.addingEvent.Clock - start_clock );
                                        changed = true;
                                    } else if ( start_clock == AppManager.addingEvent.Clock ) {
                                        work.removeEvent( i );
                                        changed = true;
                                        break;
                                    } else if ( AppManager.addingEvent.Clock < start_clock && start_clock < AppManager.addingEvent.Clock + AppManager.addingEvent.ID.getLength() ) {
                                        AppManager.addingEvent.ID.setLength( start_clock - AppManager.addingEvent.Clock );
                                        changed = true;
                                    }
                                }
                            }
                            work.addEvent( (VsqEvent)AppManager.addingEvent.clone() );
                            CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(),
                                                                                         work,
                                                                                         AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                            setEdited( true );
                        } else {
                            VsqEvent[] items = new VsqEvent[1];
                            AppManager.addingEvent.ID.type = VsqIDType.Anote;
                            AppManager.addingEvent.ID.Dynamics = 64;
                            items[0] = new VsqEvent( 0, AppManager.addingEvent.ID );
                            items[0].Clock = AppManager.addingEvent.Clock;
                            items[0].ID.LyricHandle = lyric;
                            items[0].ID.VibratoDelay = vibrato_delay;
                            items[0].ID.VibratoHandle = vibrato;
#if DEBUG
                            AppManager.debugWriteLine( "        items[0].ID.ToString()=" + items[0].ID.ToString() );
#endif
                            CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventAddRange( AppManager.getSelected(), items ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                            setEdited( true );
                        }
                    }
                }
                #endregion
            } else if ( edit_mode == EditMode.MOVE_ENTRY ) {
                #region MoveEntry
#if DEBUG
                AppManager.debugWriteLine( "    m_config.SelectedEvent.Count=" + AppManager.getSelectedEventCount() );
#endif
                if ( AppManager.getSelectedEventCount() > 0 ) {
                    VsqEvent original = AppManager.getLastSelectedEvent().original;
                    if ( original.Clock != AppManager.getLastSelectedEvent().editing.Clock || original.ID.Note != AppManager.getLastSelectedEvent().editing.ID.Note ) {
                        int count = AppManager.getSelectedEventCount();
                        int[] ids = new int[count];
                        int[] clocks = new int[count];
                        VsqID[] values = new VsqID[count];
                        int i = -1;
                        boolean out_of_range = false;
                        for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                            SelectedEventEntry ev = (SelectedEventEntry)itr.next();
                            i++;
                            ids[i] = ev.original.InternalID;
                            clocks[i] = ev.editing.Clock;
                            if ( clocks[i] < AppManager.getVsqFile().getPreMeasureClocks() ) {
                                out_of_range = true;
                            }
                            values[i] = ev.editing.ID;
                            if ( values[i].Note < 0 || 128 < values[i].Note ) {
                                out_of_range = true;
                            }
                        }
                        if ( !out_of_range ) {
                            CadenciiCommand run = new CadenciiCommand(
                                VsqCommand.generateCommandEventChangeClockAndIDContaintsRange( AppManager.getSelected(),
                                                                                               ids,
                                                                                               clocks,
                                                                                               values ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                            setEdited( true );
                        } else {
#if !JAVA
                            SystemSounds.Asterisk.Play();
#endif
                        }
                    } else {
                        /*if ( (modefier & Keys.Shift) == Keys.Shift || (modefier & Keys.Control) == Keys.Control ) {
                            Rectangle rc;
                            VsqEvent select = IdOfClickedPosition( e.Location, out rc );
                            if ( select != null ) {
                                m_config.addSelectedEvent( item.InternalID );
                            }
                        }*/
                    }
                    lock ( AppManager.drawObjects ) {
                        Collections.sort( AppManager.drawObjects.get( AppManager.getSelected() - 1 ) );
                    }
                }
                #endregion
            } else if ( edit_mode == EditMode.EDIT_LEFT_EDGE ) {
                #region EditLeftEdge
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                if ( original.Clock != AppManager.getLastSelectedEvent().editing.Clock ||
                    original.ID.getLength() != original.ID.getLength() ) {
                    int count = AppManager.getSelectedEventCount();
                    int[] ids = new int[count];
                    int[] clocks = new int[count];
                    VsqID[] values = new VsqID[count];
                    int i = -1;
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                        SelectedEventEntry ev = (SelectedEventEntry)itr.next();
                        i++;
                        if ( ev.editing.ID.VibratoHandle == null ) {
                            ids[i] = ev.original.InternalID;
                            clocks[i] = ev.editing.Clock;
                            values[i] = ev.editing.ID;
                        } else {
                            int draft_vibrato_length = ev.editing.ID.getLength() - ev.editing.ID.VibratoDelay;
                            if ( draft_vibrato_length <= 0 ) {
                                // ビブラートを削除
                                ev.editing.ID.VibratoHandle = null;
                                ev.editing.ID.VibratoDelay = 0;
                            } else {
                                // ビブラートは温存
                                ev.editing.ID.VibratoHandle.setLength( draft_vibrato_length );
                            }
                            ids[i] = ev.original.InternalID;
                            clocks[i] = ev.editing.Clock;
                            values[i] = ev.editing.ID;
                        }
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeClockAndIDContaintsRange( AppManager.getSelected(),
                                                                             ids,
                                                                             clocks,
                                                                             values ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    setEdited( true );
                }
                #endregion
            } else if ( edit_mode == EditMode.EDIT_RIGHT_EDGE ) {
                #region EditRightEdge
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                if ( original.ID.getLength() != AppManager.getLastSelectedEvent().editing.ID.getLength() ) {
                    int count = AppManager.getSelectedEventCount();
                    int[] ids = new int[count];
                    int[] clocks = new int[count];
                    VsqID[] values = new VsqID[count];
                    int i = -1;
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                        SelectedEventEntry ev = (SelectedEventEntry)itr.next();
                        i++;
                        if ( ev.editing.ID.VibratoHandle == null ) {
                            ids[i] = ev.original.InternalID;
                            clocks[i] = ev.editing.Clock;
                            values[i] = ev.editing.ID;
                        } else {
                            int draft_vibrato_length = ev.editing.ID.getLength() - ev.editing.ID.VibratoDelay;
                            if ( draft_vibrato_length <= 0 ) {
                                // ビブラートを削除
                                ev.editing.ID.VibratoHandle = null;
                                ev.editing.ID.VibratoDelay = 0;
                            } else {
                                // ビブラートは温存
                                ev.editing.ID.VibratoHandle.setLength( draft_vibrato_length );
                            }
                            ids[i] = ev.original.InternalID;
                            clocks[i] = ev.editing.Clock;
                            values[i] = ev.editing.ID;
                        }
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeClockAndIDContaintsRange( AppManager.getSelected(),
                                                                             ids,
                                                                             clocks,
                                                                             values ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    setEdited( true );
                }
                #endregion
            } else if ( edit_mode == EditMode.EDIT_VIBRATO_DELAY ) {
                #region EditVibratoDelay
                if ( m_mouse_moved ) {
                    double max_length = AppManager.addingEventLength - _PX_ACCENT_HEADER / AppManager.scaleX;
                    double rate = AppManager.addingEvent.ID.getLength() / max_length;
                    if ( rate > 0.99 ) {
                        rate = 1.0;
                    }
                    int vibrato_length = (int)(AppManager.addingEventLength * rate);
                    VsqEvent item = null;
                    for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                        VsqEvent ve = (VsqEvent)itr.next();
                        if ( ve.InternalID == m_vibrato_editing_id ) {
                            item = (VsqEvent)ve.clone();
                            break;
                        }
                    }
                    if ( item != null ) {
                        if ( vibrato_length <= 0 ) {
                            item.ID.VibratoHandle = null;
                            item.ID.VibratoDelay = item.ID.getLength();
                        } else {
                            item.ID.VibratoHandle.setLength( vibrato_length );
                            item.ID.VibratoDelay = item.ID.getLength() - vibrato_length;
                        }
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(), m_vibrato_editing_id, item.ID ) );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                        setEdited( true );
                    }
                }
                #endregion
            } else if ( edit_mode == EditMode.MOVE_ENTRY_WHOLE ) {
#if DEBUG
                PortUtil.println( "FormMain#pictPianoRoll_MouseUp; EditMode.MOVE_ENTRY_WHOLE" );
#endif
                #region MOVE_ENTRY_WHOLE
                int src_clock_start = AppManager.wholeSelectedInterval.getStart();
                int src_clock_end = AppManager.wholeSelectedInterval.getEnd();
                int dst_clock_start = AppManager.wholeSelectedIntervalStartForMoving;
                int dst_clock_end = dst_clock_start + (src_clock_end - src_clock_start);
                int dclock = dst_clock_start - src_clock_start;

                int num = AppManager.getSelectedEventCount();
                int[] selected_ids = new int[num]; // 後段での再選択用のInternalIDのリスト
                int last_selected_id = AppManager.getLastSelectedEvent().original.InternalID;

                // 音符イベントを移動
                VsqTrack work = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).clone();
                int k = 0;
                for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                    SelectedEventEntry item = (SelectedEventEntry)itr.next();
                    int internal_id = item.original.InternalID;
                    selected_ids[k] = internal_id;
                    k++;
#if DEBUG
                    PortUtil.println( "FormMain#pictPianoRoll_MouseUp; internal_id=" + internal_id );
#endif
                    for ( Iterator itr2 = work.getNoteEventIterator(); itr2.hasNext(); ) {
                        VsqEvent vsq_event = (VsqEvent)itr2.next();
                        if ( internal_id == vsq_event.InternalID ) {
#if DEBUG
                            PortUtil.println( "FormMain#pictPianoRoll_MouseUp; before: clock=" + vsq_event.Clock + "; after: clock=" + item.editing.Clock );
#endif
                            vsq_event.Clock = item.editing.Clock;
                            break;
                        }
                    }
                }

                // 全てのコントロールカーブのデータ点を移動
                for ( int i = 0; i < AppManager.CURVE_USAGE.Length; i++ ) {
                    CurveType curve_type = AppManager.CURVE_USAGE[i];
                    VsqBPList bplist = work.getCurve( curve_type.getName() );
                    if ( bplist == null ) {
                        continue;
                    }

                    // src_clock_startからsrc_clock_endの範囲にあるデータ点をコピー＆削除
                    VsqBPList copied = new VsqBPList( bplist.getDefault(), bplist.getMinimum(), bplist.getMaximum() );
                    int size = bplist.size();
                    for ( int j = size - 1; j >= 0; j-- ) {
                        int clock = bplist.getKeyClock( j );
                        if ( src_clock_start <= clock && clock <= src_clock_end ) {
                            VsqBPPair bppair = bplist.getElementB( j );
                            copied.add( clock, bppair.value );
                            bplist.removeElementAt( j );
                        }
                    }

                    // dst_clock_startからdst_clock_endの範囲にあるコントロールカーブのデータ点をすべて削除
                    size = bplist.size();
                    for ( int j = size - 1; j >= 0; j-- ) {
                        int clock = bplist.getKeyClock( j );
                        if ( dst_clock_start <= clock && clock <= dst_clock_end ) {
                            bplist.removeElementAt( j );
                        }
                    }

                    // コピーしたデータを、クロックをずらしながら追加
                    size = copied.size();
                    for ( int j = 0; j < size; j++ ) {
                        int clock = copied.getKeyClock( j );
                        VsqBPPair bppair = copied.getElementB( j );
                        bplist.add( clock + dclock, bppair.value );
                    }
                }

                // コマンドを作成＆実行
                int track_num = AppManager.getSelected();
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( track_num,
                                                                             work,
                                                                             AppManager.getVsqFile().AttachedCurves.get( track_num - 1 ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );

                // 選択範囲を更新
                AppManager.wholeSelectedInterval = new SelectedRegion( dst_clock_start );
                AppManager.wholeSelectedInterval.setEnd( dst_clock_end );
                AppManager.wholeSelectedIntervalStartForMoving = dst_clock_start;

                // 音符の再選択
                AppManager.clearSelectedEvent();
                Vector<Integer> list_selected_ids = new Vector<Integer>();
                for ( int i = 0; i < num; i++ ) {
                    list_selected_ids.add( selected_ids[i] );
                }
                AppManager.addSelectedEventAll( list_selected_ids );
                AppManager.addSelectedEvent( last_selected_id );

                setEdited( true );
                #endregion
            } else if ( AppManager.isWholeSelectedIntervalEnabled() ) {
                int start = AppManager.wholeSelectedInterval.getStart();
                int end = AppManager.wholeSelectedInterval.getEnd();
                AppManager.clearSelectedEvent();

                // 音符の選択状態を更新
                Vector<Integer> add_required_event = new Vector<Integer>();
                for ( Iterator itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = (VsqEvent)itr.next();
                    if ( start <= ve.Clock && ve.Clock + ve.ID.getLength() <= end ) {
                        add_required_event.add( ve.InternalID );
                    }
                }
                AppManager.addSelectedEventAll( add_required_event );

                // コントロールカーブ点の選択状態を更新
                Vector<Long> add_required_point = new Vector<Long>();
                VsqBPList list = vsq_track.getCurve( selected_curve.getName() );
                if ( list != null ) {
                    int count = list.size();
                    for ( int i = 0; i < count; i++ ) {
                        int clock = list.getKeyClock( i );
                        if ( clock < start ) {
                            continue;
                        } else if ( end < clock ) {
                            break;
                        } else {
                            VsqBPPair v = list.getElementB( i );
                            add_required_point.add( v.id );
                        }
                    }
                }
                if ( add_required_point.size() > 0 ) {
                    AppManager.addSelectedPointAll( selected_curve,
                                                    PortUtil.convertLongArray( add_required_point.toArray( new Long[] { } ) ) );
                }
            }
            refreshScreen();
            if ( AppManager.getEditMode() != EditMode.REALTIME ) {
                AppManager.setEditMode( EditMode.NONE );
            }
        }

        private void pictPianoRoll_MouseWheel( Object sender, BMouseEventArgs e ) {
            boolean horizontal = (PortUtil.getCurrentModifierKey() & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK;
            if ( AppManager.editorConfig.ScrollHorizontalOnWheel ) {
                horizontal = !horizontal;
            }
            if ( horizontal ) {
                hScroll.setValue( computeScrollValueFromWheelDelta( e.Delta ) );
            } else {
                double new_val = (double)vScroll.getValue() - e.Delta;
                if ( new_val > vScroll.getMaximum() ) {
                    vScroll.setValue( vScroll.getMaximum() );
                } else if ( new_val < vScroll.getMinimum() ) {
                    vScroll.setValue( vScroll.getMinimum() );
                } else {
                    vScroll.setValue( (int)new_val );
                }
            }
            refreshScreen();
        }

        private void pictPianoRoll_PreviewKeyDown( Object sender, BPreviewKeyDownEventArgs e ) {
#if DEBUG
            System.Diagnostics.Debug.WriteLine( "pictureBox1_PreviewKeyDown" );
            System.Diagnostics.Debug.WriteLine( "    e.KeyCode=" + e.KeyCode );
#endif

#if JAVA
            if ( e.KeyValue == KeyEvent.VK_TAB && AppManager.getSelectedEventCount() > 0 ) {
#else
            if ( e.KeyCode == System.Windows.Forms.Keys.Tab && AppManager.getSelectedEventCount() > 0 ) {
#endif
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                if ( original == null ) {
                    return;
                }
                int x = AppManager.xCoordFromClocks( original.Clock );
                int y = yCoordFromNote( original.ID.Note );
                if ( !AppManager.editorConfig.KeepLyricInputMode ) {
                    m_last_symbol_edit_mode = false;
                }
                showInputTextBox( original.ID.LyricHandle.L0.Phrase,
                                  original.ID.LyricHandle.L0.getPhoneticSymbol(),
                                  new Point( x, y ),
                                  m_last_symbol_edit_mode );
#if !JAVA
                e.IsInputKey = true;
#endif
                refreshScreen();
            }
            ProcessSpecialShortcutKey( e );
        }
        #endregion

        #region menuVisual*
        private void menuVisualMixer_Click( Object sender, BEventArgs e ) {
            menuVisualMixer.setSelected( !menuVisualMixer.isSelected() );
            AppManager.editorConfig.MixerVisible = menuVisualMixer.isSelected();
            AppManager.mixerWindow.setVisible( AppManager.editorConfig.MixerVisible );
            requestFocus();
        }

        private void menuVisualGridline_CheckedChanged( Object sender, BEventArgs e ) {
            AppManager.setGridVisible( menuVisualGridline.isSelected() );
            refreshScreen();
        }

        private void menuVisualLyrics_CheckedChanged( Object sender, BEventArgs e ) {
            AppManager.editorConfig.ShowLyric = menuVisualLyrics.isSelected();
        }

        private void menuVisualNoteProperty_CheckedChanged( Object sender, BEventArgs e ) {
            AppManager.editorConfig.ShowExpLine = menuVisualNoteProperty.isSelected();
            refreshScreen();
        }

        private void menuVisualPitchLine_CheckedChanged( Object sender, BEventArgs e ) {
            AppManager.editorConfig.ViewAtcualPitch = menuVisualPitchLine.isSelected();
        }

        private void menuVisualControlTrack_CheckedChanged( Object sender, BEventArgs e ) {
            trackSelector.setCurveVisible( menuVisualControlTrack.isSelected() );
            if ( menuVisualControlTrack.isSelected() ) {
                splitContainer1.setSplitterFixed( false );
                splitContainer1.setDividerLocation( splitContainer1.Height - AppManager.lastTrackSelectorHeight - splitContainer1.getDividerSize() );
                splitContainer1.Panel2MinSize = trackSelector.getPreferredMinSize();
            } else {
                AppManager.lastTrackSelectorHeight = splitContainer1.Height - splitContainer1.getDividerLocation() - splitContainer1.getDividerSize();
                splitContainer1.setSplitterFixed( true );
                splitContainer1.setDividerLocation( splitContainer1.Height - _SPL1_PANEL2_MIN_HEIGHT - splitContainer1.getDividerSize() );
                splitContainer1.Panel2MinSize = _SPL1_PANEL2_MIN_HEIGHT;
            }
            refreshScreen();
        }

        private void menuHiddenVisualForwardParameter_Click( Object sender, BEventArgs e ) {
            trackSelector.SelectNextCurve();
        }

        private void menuHiddenVisualBackwardParameter_Click( Object sender, BEventArgs e ) {
            trackSelector.SelectPreviousCurve();
        }

        private void menuVisualWaveform_CheckedChanged( Object sender, BEventArgs e ) {
            AppManager.editorConfig.ViewWaveform = menuVisualWaveform.isSelected();
            updateSplitContainer2Size();
        }

        private void menuVisualControlTrack_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Show/Hide control curves." ) );
        }

        private void menuVisualMixer_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Show/Hide mixer window." ) );
        }

        private void menuVisualWaveform_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Show/Hide waveform." ) );
        }

        private void menuVisualProperty_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Show/Hide property window." ) );
        }

        private void menuVisualGridline_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Show/Hide grid line." ) );
        }

        private void menuVisualStartMarker_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Enable/Disable start marker." ) );
        }

        private void menuVisualEndMarker_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Enable/Disable end marker." ) );
        }

        private void menuVisualLyrics_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Show/Hide lyrics." ) );
        }

        private void menuVisualNoteProperty_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Show/Hide expression lines." ) );
        }

        private void menuVisualPitchLine_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Show/Hide pitch bend lines." ) );
        }
        #endregion

        #region m_mixer_dlg
        private void m_mixer_dlg_SoloChanged( int track, boolean solo ) {
#if DEBUG
            AppManager.debugWriteLine( "m_mixer_dlg_SoloChanged" );
            AppManager.debugWriteLine( "    track=" + track );
            AppManager.debugWriteLine( "    solo=" + solo );
#endif
            if ( track == 0 ) {
                // ここはなし
            } else if ( track > 0 ) {
                AppManager.getVsqFile().Mixer.Slave.get( track - 1 ).Solo = solo ? 1 : 0;
            } else {
                // ここもなし
            }
        }

        private void m_mixer_dlg_MuteChanged( int track, boolean mute ) {
#if DEBUG
            AppManager.debugWriteLine( "m_mixer_dlg_MuteChanged" );
            AppManager.debugWriteLine( "    track=" + track );
            AppManager.debugWriteLine( "    mute=" + mute );
#endif
            if ( track == 0 ) {
                AppManager.getVsqFile().Mixer.MasterMute = mute ? 1 : 0;
            } else if ( track > 0 ) {
                AppManager.getVsqFile().Mixer.Slave.get( track - 1 ).Mute = mute ? 1 : 0;
            } else {
                AppManager.getBgm( -track - 1 ).mute = mute ? 1 : 0;
            }
        }

        private void m_mixer_dlg_PanpotChanged( int track, int panpot ) {
            if ( track == 0 ) {
                // master
                AppManager.getVsqFile().Mixer.MasterPanpot = panpot;
            } else if ( track > 0 ) {
                // slave
                AppManager.getVsqFile().Mixer.Slave.get( track - 1 ).Panpot = panpot;
            } else {
                AppManager.getBgm( -track - 1 ).panpot = panpot;
            }
        }

        private void m_mixer_dlg_FederChanged( int track, int feder ) {
#if DEBUG
            PortUtil.println( "FormMain#m_mixer_dlg_FederChanged; track=" + track + "; feder=" + feder );
#endif
            if ( track == 0 ) {
                AppManager.getVsqFile().Mixer.MasterFeder = feder;
            } else if ( track > 0 ) {
                AppManager.getVsqFile().Mixer.Slave.get( track - 1 ).Feder = feder;
            } else {
                AppManager.getBgm( -track - 1 ).feder = feder;
            }
        }

        private void m_mixer_dlg_TopMostChanged( Object sender, boolean arg ) {
            AppManager.editorConfig.MixerTopMost = arg;
        }
        #endregion

        #region FormMain
        private void FormMain_FormClosed( Object sender, BFormClosedEventArgs e ) {
            clearTempWave();
            String tempdir = AppManager.getTempWaveDir();
            String log = PortUtil.combinePath( tempdir, "run.log" );
            bocoree.debug.close();
            try {
                if ( PortUtil.isFileExists( log ) ) {
                    PortUtil.deleteFile( log );
                }
                PortUtil.deleteDirectory( tempdir, true );
            } catch ( Exception ex ) {
            }
            VSTiProxy.abortRendering();
            VSTiProxy.terminate();
#if ENABLE_MIDI
            MidiPlayer.Stop();
#endif
        }

        private void FormMain_FormClosing( Object sender, BFormClosingEventArgs e ) {
            if ( isEdited() ) {
                String file = AppManager.getFileName();
                if ( file.Equals( "" ) ) {
                    file = "Untitled";
                } else {
                    file = PortUtil.getFileName( file );
                }
                BDialogResult ret = AppManager.showMessageBox( _( "Save this sequence?" ),
                                                               _( "Affirmation" ),
                                                               AppManager.MSGBOX_YES_NO_CANCEL_OPTION,
                                                               AppManager.MSGBOX_QUESTION_MESSAGE );
                if ( ret == BDialogResult.YES ) {
                    if ( AppManager.getFileName().Equals( "" ) ) {
                        int dr = saveXmlVsqDialog.showSaveDialog( this );
                        if ( dr == BFileChooser.APPROVE_OPTION ) {
                            AppManager.saveTo( saveXmlVsqDialog.getSelectedFile() );
                        } else {
                            e.Cancel = true;
                            return;
                        }
                    } else {
                        AppManager.saveTo( AppManager.getFileName() );
                    }

                } else if ( ret == BDialogResult.CANCEL ) {
                    e.Cancel = true;
                    return;
                }
            }
            AppManager.editorConfig.WindowMaximized = (getExtendedState() == BForm.MAXIMIZED_BOTH);
            AppManager.saveConfig();
            UtauRenderingRunner.clearCache();
            StraightRenderingRunner.clearCache();
#if ENABLE_MIDI
            if ( m_midi_in != null ) {
                m_midi_in.Dispose();
            }
#endif
            bgWorkScreen.Dispose();
            e.Cancel = false;
        }

        private void FormMain_Load( Object sender, BEventArgs e ) {
            applyLanguage();
            trackBar.setValue( AppManager.editorConfig.DefaultXScale );
            AppManager.setCurrentClock( 0 );
            setEdited( false );

            AppManager.PreviewStarted += new EventHandler( AppManager_PreviewStarted );
            AppManager.PreviewAborted += new EventHandler( AppManager_PreviewAborted );
            AppManager.GridVisibleChanged += new EventHandler( AppManager_GridVisibleChanged );
            AppManager.SelectedEventChanged += new SelectedEventChangedEventHandler( AppManager_SelectedEventChanged );
            AppManager.CurrentClockChanged += new EventHandler( AppManager_CurrentClockChanged );
            AppManager.SelectedToolChanged += new EventHandler( AppManager_SelectedToolChanged );
            EditorConfig.QuantizeModeChanged += new EventHandler( EditorConfig_QuantizeModeChanged );
#if ENABLE_PROPERTY
            m_property_panel_container.StateChangeRequired += new StateChangeRequiredEventHandler( m_property_panel_container_StateChangeRequired );
#endif

            updateRecentFileMenu();

            // C3が画面中央に来るように調整
            int draft_start_to_draw_y = 68 * AppManager.editorConfig.PxTrackHeight - pictPianoRoll.getHeight() / 2;
            int draft_vscroll_value = (int)((draft_start_to_draw_y * (double)vScroll.getMaximum()) / (128 * AppManager.editorConfig.PxTrackHeight - vScroll.getHeight()));
            try {
                vScroll.setValue( draft_vscroll_value );
            } catch ( Exception ex ) {
            }

            // x=97がプリメジャークロックになるように調整
            int cp = AppManager.getVsqFile().getPreMeasureClocks();
            int draft_hscroll_value = (int)(cp - 24.0 / AppManager.scaleX);
            try {
                hScroll.setValue( draft_hscroll_value );
            } catch ( Exception ex ) {
            }

            //s_pen_dashed_171_171_171.DashPattern = new float[] { 3, 3 };
            //s_pen_dashed_209_204_172.DashPattern = new float[] { 3, 3 };

            menuVisualNoteProperty.setSelected( AppManager.editorConfig.ShowExpLine );
            menuVisualLyrics.setSelected( AppManager.editorConfig.ShowLyric );
            menuVisualMixer.setSelected( AppManager.editorConfig.MixerVisible );
            menuVisualPitchLine.setSelected( AppManager.editorConfig.ViewAtcualPitch );

            AppManager.mixerWindow = new FormMixer( this );

            updateMenuFonts();

#if JAVA
            AppManager.mixerWindow.federChangedEvent.add( new FederChangedEventHandler( this, "m_mixer_dlg_FederChanged" ) );
            AppManager.mixerWindow.panpotChangedEvent.add( new PanpotChangedEventHandler( this, "m_mixer_dlg_PanpotChanged" ) );
            AppManager.mixerWindow.muteChangedEvent.add( new MuteChangedEventHandler( this, "m_mixer_dlg_MuteChanged" ) );
            AppManager.mixerWindow.soloChangedEvent( new SoloChangedEventHandler( this, "m_mixer_dlg_SoloChanged" ) );
            AppManager.mixerWindow.topMostChangedEvent( new TopMostChangedEventHandler( this, "m_mixer_dlg_TopMostChanged" ) );
#else
            AppManager.mixerWindow.FederChanged += new FederChangedEventHandler( m_mixer_dlg_FederChanged );
            AppManager.mixerWindow.PanpotChanged += new PanpotChangedEventHandler( m_mixer_dlg_PanpotChanged );
            AppManager.mixerWindow.MuteChanged += new MuteChangedEventHandler( m_mixer_dlg_MuteChanged );
            AppManager.mixerWindow.SoloChanged += new SoloChangedEventHandler( m_mixer_dlg_SoloChanged );
            AppManager.mixerWindow.TopMostChanged += new TopMostChangedEventHandler( m_mixer_dlg_TopMostChanged );
#endif
            AppManager.mixerWindow.setShowTopMost( AppManager.editorConfig.MixerTopMost );
            AppManager.mixerWindow.updateStatus();
            if ( AppManager.editorConfig.MixerVisible ) {
                AppManager.mixerWindow.setVisible( true );
            }

            trackSelector.CommandExecuted += new EventHandler( trackSelector_CommandExecuted );

#if ENABLE_SCRIPT
            updateScriptShortcut();
#endif

            clearTempWave();
            setHScrollRange( AppManager.getVsqFile().TotalClocks );
            setVScrollRange( vScroll.getMaximum() );
            m_pencil_mode.setMode( PencilModeEnum.Off );
            updateCMenuPianoFixed();
            loadGameControler();
#if ENABLE_MIDI
            reloadMidiIn();
#endif
            menuVisualWaveform.setSelected( AppManager.editorConfig.ViewWaveform );
            updateSplitContainer2Size();

            updateRendererMenu();

            if ( AppManager.editorConfig.WindowMaximized ) {
                setExtendedState( BForm.MAXIMIZED_BOTH );
            } else {
                setExtendedState( BForm.NORMAL );
            }
            this.setBounds( AppManager.editorConfig.WindowRect );
            updateLayout();

            // プロパティウィンドウの位置を復元
            Rectangle rc1 = PortUtil.getScreenBounds( this );
            Rectangle rcScreen = new Rectangle( rc1.x, rc1.y, rc1.width, rc1.height );
            Point p = this.getLocation();
            XmlRectangle xr = AppManager.editorConfig.PropertyWindowStatus.Bounds;
            Point p0 = new Point( xr.x, xr.y );
            Point a = new Point( p.x + p0.x, p.y + p0.y );
            Rectangle rc = new Rectangle( a.x,
                                          a.y,
                                          AppManager.editorConfig.PropertyWindowStatus.Bounds.Width,
                                          AppManager.editorConfig.PropertyWindowStatus.Bounds.Height );

            if ( a.y > rcScreen.y + rcScreen.height ) {
                a = new Point( a.x, rcScreen.y + rcScreen.height - rc.height );
            }
            if ( a.y < rcScreen.y ) {
                a = new Point( a.x, rcScreen.y );
            }
            if ( a.x > rcScreen.x + rcScreen.width ) {
                a = new Point( rcScreen.x + rcScreen.width - rc.width, a.y );
            }
            if ( a.x < rcScreen.x ) {
                a = new Point( rcScreen.x, a.y );
            }
#if DEBUG
            AppManager.debugWriteLine( "FormMain_Load; a=" + a );
#endif

#if ENABLE_PROPERTY
            AppManager.propertyWindow.setBounds( a.x, a.y, rc.width, rc.height );
            AppManager.propertyWindow.LocationChanged += new EventHandler( m_note_proerty_dlg_LocationOrSizeChanged );
            AppManager.propertyWindow.SizeChanged += new EventHandler( m_note_proerty_dlg_LocationOrSizeChanged );
            AppManager.propertyWindow.FormClosing += new System.Windows.Forms.FormClosingEventHandler( m_note_proerty_dlg_FormClosing );
            AppManager.propertyPanel.CommandExecuteRequired += new CommandExecuteRequiredEventHandler( m_note_proerty_dlg_CommandExecuteRequired );
            AppManager.propertyWindow.setFormCloseShortcutKey( AppManager.editorConfig.getShortcutKeyFor( menuVisualProperty ) );
            updatePropertyPanelState( AppManager.editorConfig.PropertyWindowStatus.State );
#endif
            updateBgmMenuState();

#if JAVA
            sizeChangedEvent.add( new BEventHandler( this, "FormMain_SizeChanged" ) );
            locationChangedEvent.add( new BEventHandler( this, "FormMain_LocationChanged" ) );
#else
            this.SizeChanged += new System.EventHandler( this.FormMain_SizeChanged );
            this.LocationChanged += new System.EventHandler( this.FormMain_LocationChanged );
#endif
            repaint();
#if DEBUG
            //VocaloSysUtil_DRAFT.getLanguageFromName( "" );
            /*ExpressionConfigSys exp_config_sys = new ExpressionConfigSys( @"C:\Program Files\VOCALOID2\expdbdir" );
            PortUtil.println( "vibrato:" );
            for ( Iterator itr = exp_config_sys.vibratoConfigIterator(); itr.hasNext(); ) {
                VibratoConfig vc = (VibratoConfig)itr.next();
                PortUtil.println( "file=" + vc.file );
            }
            PortUtil.println( "attack:" );
            for ( Iterator itr = exp_config_sys.attackConfigIterator(); itr.hasNext(); ) {
                AttackConfig ac = (AttackConfig)itr.next();
                PortUtil.println( "file=" + ac.file );
            }

            byte[] dat = BitConverter.GetBytes( 4 );
            using ( StreamWriter sw = new StreamWriter( @"C:\get_bytes.txt" ) ) {
                for ( int i = 0; i < dat.Length; i++ ) {
                    sw.WriteLine( dat[i] );
                }
            }
            using ( TextMemoryStream tms = new TextMemoryStream( @"C:\a.txt", Encoding.ASCII ) ) {
                tms.rewind();
                while ( tms.peek() >= 0 ) {
                    PortUtil.println( tms.readLine() );
                }
            }
            WaveDrawContext wdc = new WaveDrawContext( @"C:\ぴょ.wav" );
            using ( Bitmap b = new Bitmap( 500, 200 ) ) {
                using ( Graphics g = Graphics.FromImage( b ) ) {
                    wdc.Draw( g, Pens.Black, new Rectangle( 0, 0, 500, 200 ), 0.0f, 0.5f );
                }
                b.Save( @"C:\ぴょ.wav.png", System.Drawing.Imaging.ImageFormat.Png );
            }
            try {
                UtauFreq uf = UtauFreq.FromFrq( @"C:\あ_wav.frq" );
                uf.Write( new FileStream( @"C:\regenerated.frq", FileMode.Create, FileAccess.Write ) );
            } catch {
            }*/
            menuHidden.setVisible( true );
            /*using ( StreamWriter sw = new StreamWriter( PortUtil.combinePath( Application.StartupPath, "Keys.txt" ) ) ) {
                foreach ( Keys key in Enum.GetValues( typeof( Keys ) ) ) {
                    sw.WriteLine( (int)key + "\t" + key.ToString() );
                }
            }*/
            /*OpenFileDialog ofd = new OpenFileDialog();
            XmlSerializer xs = new XmlSerializer( typeof( VsqFileEx ) );
            while ( ofd.ShowDialog() == DialogResult.OK ) {
                VsqFileEx vsq = new VsqFileEx( ofd.FileName );
                vsq.Track.get( 1 ).getEvent( 1 ).UstEvent = new UstEvent();
                using ( FileStream fs = new FileStream( ofd.FileName + "_regen.xml", FileMode.Create ) ) {
                    xs.Serialize( fs, vsq );
                }
            }*/
            /*Cursor c = SynthCursor( Properties.Resources.arrow_135 );
            if ( c != null ) {
                HAND = c;
            }*/
            /*MessageBody mb = new MessageBody( "ja", PortUtil.combinePath( Application.StartupPath, "ja.po" ) );
            mb.Write( PortUtil.combinePath( Application.StartupPath, "foo.po" ) );*/
            /*OpenFileDialog ofd = new OpenFileDialog();
            Wave.TestEnabled = true;
            while ( ofd.ShowDialog() == DialogResult.OK ) {
                String file = PortUtil.combinePath( Path.GetDirectoryName( ofd.FileName ), PortUtil.getFileNameWithoutExtension( ofd.FileName ) + ".txt" );
                using ( StreamWriter sw = new StreamWriter( file ) )
                using ( Wave w = new Wave( ofd.FileName ) ) {
                    w.TrimSilence();
                    int WID = 2048;
                    double[] wind = new double[WID];
                    for ( int j = 0; j < WID; j++ ) {
                        wind[j] = bocoree.math.window_func( bocoree.math.WindowFunctionType.Hamming, (double)j / (double)WID );
                    }
                    uint i = w.SampleRate;
                    //for ( uint i = 0; i < w.TotalSamples; i+=10 ) {
                        double f0 = w.TEST_GetF0( i, wind );
                        double n = 12.0 * Math.Log( f0 / 440.0, 2.0 ) + 69.0;
                        sw.WriteLine( i / (double)w.SampleRate + "\t" + n + "\t" + f0 );
                    //}
                }
            }*/
            /*bocoree.debug.push_log( "installed singers 1" );
            SingerConfig[] s1 = VocaloSysUtil.getInstalledSingers1();
            foreach ( SingerConfig sc in s1 ) {
                bocoree.debug.push_log( "    " + sc );
            }
            bocoree.debug.push_log( "installed singers 2" );
            SingerConfig[] s2 = VocaloSysUtil.getInstalledSingers2();
            foreach ( SingerConfig sc in s2 ) {
                bocoree.debug.push_log( "    " + sc );
            }
            if ( AppManager.EditorConfig.PathUtauVSTi != "" ) {
                bocoree.debug.push_log( "installed singers utau" );
                UtauSingerConfigSys uscs = new UtauSingerConfigSys( Path.GetDirectoryName( AppManager.EditorConfig.PathUtauVSTi ) );
                s2 = uscs.getInstalledSingers();
                foreach ( SingerConfig sc in s2 ) {
                    bocoree.debug.push_log( "    " + sc );
                }
            }
            PortUtil.println( VocaloSysUtil.getLanguage2( 0 ) );*/

            /*OpenFileDialog ofd = new OpenFileDialog();
                const String format = "    {0,8} 0x{1:X4} {2,-32} 0x{3:X2} 0x{4:X2}";
                const String format0 = "    {0,8} 0x{1:X4} {2,-32} 0x{3:X2}";
            while ( ofd.ShowDialog() == DialogResult.OK ) {
                VsqFile vf = new VsqFile( ofd.FileName );
                vf.getTrack( 1 ).getCommon().Version = "UTU000";
                VsqNrpn[] nrpns = VsqFile.generateNRPN( vf, 1, 500 );
                String file = PortUtil.combinePath( Path.GetDirectoryName( ofd.FileName ), PortUtil.getFileNameWithoutExtension( ofd.FileName ) + "_regen.txt" );
                using ( StreamWriter sw = new StreamWriter( file ) ) {
                    for ( int i = 0; i < nrpns.Length; i++ ) {
                        VsqNrpn vn = nrpns[i];
                        if ( vn.DataLsbSpecified ) {
                            sw.WriteLine( String.Format( format, vn.Clock, vn.Nrpn, NRPN.getName( vn.Nrpn ), vn.DataMsb, vn.DataLsb ) );
                        } else {
                            sw.WriteLine( String.Format( format0, vn.Clock, vn.Nrpn, NRPN.getName( vn.Nrpn ), vn.DataMsb ) );
                        }
                    }
                }
            }*/
            /*unsafe {
                WavePlay w = new WavePlay( 44100, 44100 );
                w.on_your_mark( new String[] { }, 0 );
                float* left = (float*)System.Runtime.InteropServices.Marshal.AllocHGlobal( sizeof( float ) * 10000 );
                float* right = (float*)System.Runtime.InteropServices.Marshal.AllocHGlobal( sizeof( float ) * 10000 );
                float** buf = (float**)System.Runtime.InteropServices.Marshal.AllocHGlobal( sizeof( float* ) * 2 );
                buf[0] = left;
                buf[1] = right;
                float wv = 0.0f;
                for ( int i = 0; i < 10000; i++ ) {
                    wv += 0.002f;
                    if ( wv > 0.2f ) {
                        wv = -0.2f;
                    }
                    left[i] = wv;
                    right[i] = -wv;
                }
                for ( int i = 0; i < 100; i++ ) {
                    w.append( buf, 10000, 0.2, 0.2 );
                }
                w.flush_and_exit( 0.2, 0.2 );
                while ( w.is_alive() ) {
                }
            }*/
#endif
        }

#if ENABLE_PROPERTY
        private void m_property_panel_container_StateChangeRequired( Object sender, PanelState arg ) {
            updatePropertyPanelState( arg );
        }
#endif

#if ENABLE_PROPERTY
        private void m_note_proerty_dlg_CommandExecuteRequired( CadenciiCommand command ) {
#if DEBUG
            AppManager.debugWriteLine( "m_note_property_dlg_CommandExecuteRequired" );
#endif
            AppManager.register( AppManager.getVsqFile().executeCommand( command ) );
            updateDrawObjectList();
            refreshScreen();
            setEdited( true );
        }
#endif

#if ENABLE_PROPERTY
        private void m_note_proerty_dlg_FormClosing( Object sender, BFormClosingEventArgs e ) {
            if ( e.CloseReason == System.Windows.Forms.CloseReason.UserClosing ) {
                e.Cancel = true;
                updatePropertyPanelState( PanelState.Hidden );
            }
        }
#endif

#if ENABLE_PROPERTY
        private void m_note_proerty_dlg_LocationOrSizeChanged( Object sender, BEventArgs e ) {
#if DEBUG
            PortUtil.println( "m_note_proeprty_dlg_LocationOrSizeChanged; WindowState=" + AppManager.propertyWindow.WindowState );
#endif
            if ( AppManager.editorConfig.PropertyWindowStatus.State == PanelState.Window ) {
                if ( AppManager.propertyWindow.getExtendedState() == BForm.ICONIFIED ) {
                    updatePropertyPanelState( PanelState.Docked );
                } else {
                    Point parent = this.getLocation();
                    Point proeprty = AppManager.propertyWindow.getLocation();
                    AppManager.editorConfig.PropertyWindowStatus.Bounds = new XmlRectangle( proeprty.x - parent.x,
                                                                                            proeprty.y - parent.y,
                                                                                            AppManager.propertyWindow.getWidth(),
                                                                                            AppManager.propertyWindow.getHeight() );
                }
            }
        }
#endif

        private void FormMain_LocationChanged( Object sender, BEventArgs e ) {
            if ( getExtendedState() == BForm.NORMAL ) {
                AppManager.editorConfig.WindowRect = this.getBounds();
            }
        }

        private void FormMain_SizeChanged( Object sender, BEventArgs e ) {
            if ( getExtendedState() == BForm.NORMAL ) {
                AppManager.editorConfig.WindowRect = this.getBounds();
#if ENABLE_PROPERTY
                AppManager.propertyWindow.setExtendedState( BForm.NORMAL );
                AppManager.propertyWindow.setVisible( AppManager.editorConfig.PropertyWindowStatus.State == PanelState.Window );
#endif
                AppManager.mixerWindow.setVisible( AppManager.editorConfig.MixerVisible );
                updateLayout();
            } else if ( getExtendedState() == BForm.ICONIFIED ) {
#if ENABLE_PROPERTY
                AppManager.propertyWindow.setVisible( false );
#endif
                AppManager.mixerWindow.setVisible( false );
            } else if ( getExtendedState() == BForm.MAXIMIZED_BOTH ) {
#if ENABLE_PROPERTY
                AppManager.propertyWindow.setExtendedState( BForm.NORMAL );
                AppManager.propertyWindow.setVisible( AppManager.editorConfig.PropertyWindowStatus.State == PanelState.Window );
#endif
                AppManager.mixerWindow.setVisible( AppManager.editorConfig.MixerVisible );
            }
        }

        private void FormMain_MouseWheel( Object sender, BMouseEventArgs e ) {
            if ( (PortUtil.getCurrentModifierKey() & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK ) {
                hScroll.setValue( computeScrollValueFromWheelDelta( e.Delta ) );
            } else {
                double new_val = (double)vScroll.getValue() - e.Delta;
                if ( new_val > vScroll.getMaximum() ) {
                    vScroll.setValue( vScroll.getMaximum() );
                } else if ( new_val < vScroll.getMinimum() ) {
                    vScroll.setValue( vScroll.getMinimum() );
                } else {
                    vScroll.setValue( (int)new_val );
                }
            }
            refreshScreen();
        }

        private void FormMain_PreviewKeyDown( Object sender, BPreviewKeyDownEventArgs e ) {
            ProcessSpecialShortcutKey( e );
        }

        private void FormMain_Deactivate( Object sender, BEventArgs e ) {
            m_form_activated = false;
        }

        private void FormMain_Activated( Object sender, BEventArgs e ) {
            m_form_activated = true;
        }
        #endregion

#if !JAVA
        private void m_timer_Tick( Object sender, BEventArgs e ) {
            if ( !m_form_activated ) {
                return;
            }
            try {
                double now = PortUtil.getCurrentTime();
                byte[] buttons;
                int pov0;
#if !JAVA
                winmmhelp.JoyGetStatus( 0, out buttons, out pov0 );
#endif
                boolean event_processed = false;
                double dt_ms = (now - m_last_event_processed) * 1000.0;

                EditorConfig m = AppManager.editorConfig;
                boolean btn_x = (0 <= m.GameControlerCross && m.GameControlerCross < buttons.Length && buttons[m.GameControlerCross] > 0x00);
                boolean btn_o = (0 <= m.GameControlerCircle && m.GameControlerCircle < buttons.Length && buttons[m.GameControlerCircle] > 0x00);
                boolean btn_tr = (0 <= m.GameControlerTriangle && m.GameControlerTriangle < buttons.Length && buttons[m.GameControlerTriangle] > 0x00);
                boolean btn_re = (0 <= m.GameControlerRectangle && m.GameControlerRectangle < buttons.Length && buttons[m.GameControlerRectangle] > 0x00);
                boolean pov_r = pov0 == m.GameControlPovRight;
                boolean pov_l = pov0 == m.GameControlPovLeft;
                boolean pov_u = pov0 == m.GameControlPovUp;
                boolean pov_d = pov0 == m.GameControlPovDown;
                boolean L1 = (0 <= m.GameControlL1 && m.GameControlL1 < buttons.Length && buttons[m.GameControlL1] > 0x00);
                boolean R1 = (0 <= m.GameControlL2 && m.GameControlL2 < buttons.Length && buttons[m.GameControlR1] > 0x00);
                boolean L2 = (0 <= m.GameControlR1 && m.GameControlR1 < buttons.Length && buttons[m.GameControlL2] > 0x00);
                boolean R2 = (0 <= m.GameControlR2 && m.GameControlR2 < buttons.Length && buttons[m.GameControlR2] > 0x00);
                boolean SELECT = (0 <= m.GameControlSelect && m.GameControlSelect <= buttons.Length && buttons[m.GameControlSelect] > 0x00);
                if ( m_game_mode == GameControlMode.NORMAL ) {
                    m_last_btn_x = btn_x;

                    if ( !event_processed && !btn_o && m_last_btn_o ) {
                        if ( AppManager.isPlaying() ) {
                            timer.stop();
                        }
                        AppManager.setPlaying( !AppManager.isPlaying() );
                        m_last_event_processed = now;
                        event_processed = true;
                    }
                    m_last_btn_o = btn_o;

                    if ( !event_processed && pov_r && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval ) {
                        forward();
                        m_last_event_processed = now;
                        event_processed = true;
                    }
                    m_last_pov_r = pov_r;

                    if ( !event_processed && pov_l && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval ) {
                        rewind();
                        m_last_event_processed = now;
                        event_processed = true;
                    }
                    m_last_pov_l = pov_l;

                    if ( !event_processed && pov_u && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval ) {
                        int draft_vscroll = vScroll.getValue() - AppManager.editorConfig.PxTrackHeight * 3;
                        if ( draft_vscroll < vScroll.getMinimum() ) {
                            draft_vscroll = vScroll.getMinimum();
                        }
                        vScroll.setValue( draft_vscroll );
                        refreshScreen();
                        m_last_event_processed = now;
                        event_processed = true;
                    }

                    if ( !event_processed && pov_d && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval ) {
                        int draft_vscroll = vScroll.getValue() + AppManager.editorConfig.PxTrackHeight * 3;
                        if ( draft_vscroll > vScroll.getMaximum() ) {
                            draft_vscroll = vScroll.getMaximum();
                        }
                        vScroll.setValue( draft_vscroll );
                        refreshScreen();
                        m_last_event_processed = now;
                        event_processed = true;
                    }

                    if ( !event_processed && !SELECT && m_last_select ) {
                        event_processed = true;
                        m_game_mode = GameControlMode.KEYBOARD;
                        stripLblGameCtrlMode.setText( m_game_mode.ToString() );
                        stripLblGameCtrlMode.setIcon( new ImageIcon( Resources.get_piano() ) );
                    }
                    m_last_select = SELECT;
                } else if ( m_game_mode == GameControlMode.KEYBOARD ) {
                    if ( !event_processed && !SELECT && m_last_select ) {
                        event_processed = true;
                        m_game_mode = GameControlMode.NORMAL;
                        updateGameControlerStatus( null, null );
                        m_last_select = SELECT;
                        return;
                    }
                    m_last_select = SELECT;
                    if ( L1 && R1 && L2 && R2 && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval ) {
                        if ( AppManager.isPlaying() ) {
                            AppManager.setEditMode( EditMode.NONE );
                            AppManager.setPlaying( false );
                            timer.stop();
                        } else {
                            m_timer.stop();
                            FormRealtimeConfig frc = null;
                            try {
                                frc = new FormRealtimeConfig();
                                if ( frc.showDialog() == BDialogResult.OK ) {
                                    AppManager.addingEvent = null;
                                    AppManager.setEditMode( EditMode.REALTIME );
                                    AppManager.editorConfig.RealtimeInputSpeed = frc.getSpeed();
                                    AppManager.setPlaying( true );
                                }
                            } catch ( Exception ex ) {
                            } finally {
                                try {
                                    frc.close();
                                } catch ( Exception ex2 ) {
                                }
                            }
                            m_timer.start();
                        }
                        m_last_btn_o = btn_o;
                        m_last_btn_x = btn_x;
                        m_last_btn_re = btn_re;
                        m_last_btn_tr = btn_tr;
                        m_last_pov_l = pov_l;
                        m_last_pov_d = pov_d;
                        m_last_pov_r = pov_r;
                        m_last_pov_u = pov_u;
                        return;
                    }

                    int note = -1;
                    if ( pov_r && !m_last_pov_r ) {
                        note = 60;
                    } else if ( btn_re && !m_last_btn_re ) {
                        note = 62;
                    } else if ( btn_tr && !m_last_btn_tr ) {
                        note = 64;
                    } else if ( btn_o && !m_last_btn_o ) {
                        note = 65;
                    } else if ( btn_x && !m_last_btn_x ) {
                        note = 67;
                    } else if ( pov_u && !m_last_pov_u ) {
                        note = 59;
                    } else if ( pov_l && !m_last_pov_l ) {
                        note = 57;
                    } else if ( pov_d && !m_last_pov_d ) {
                        note = 55;
                    }
                    if ( note >= 0 ) {
                        if ( L1 ) {
                            note += 12;
                        } else if ( L2 ) {
                            note -= 12;
                        }
                        if ( R1 ) {
                            note += 1;
                        } else if ( R2 ) {
                            note -= 1;
                        }
                    }
                    m_last_btn_o = btn_o;
                    m_last_btn_x = btn_x;
                    m_last_btn_re = btn_re;
                    m_last_btn_tr = btn_tr;
                    m_last_pov_l = pov_l;
                    m_last_pov_d = pov_d;
                    m_last_pov_r = pov_r;
                    m_last_pov_u = pov_u;
                    if ( note >= 0 ) {
#if DEBUG
                        AppManager.debugWriteLine( "FormMain+m_timer_Tick" );
                        AppManager.debugWriteLine( "    note=" + note );
#endif
                        if ( AppManager.isPlaying() ) {
                            int clock = AppManager.getCurrentClock();
                            if ( AppManager.addingEvent != null ) {
                                AppManager.addingEvent.ID.setLength( clock - AppManager.addingEvent.Clock );
                                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventAdd( AppManager.getSelected(),
                                                                                                        AppManager.addingEvent ) );
                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                if ( !isEdited() ) {
                                    setEdited( true );
                                }
#if USE_DOBJ
                                updateDrawObjectList();
#endif
                            }
                            AppManager.addingEvent = new VsqEvent( clock, new VsqID( 0 ) );
                            AppManager.addingEvent.ID.type = VsqIDType.Anote;
                            AppManager.addingEvent.ID.Dynamics = 64;
                            AppManager.addingEvent.ID.VibratoHandle = null;
                            AppManager.addingEvent.ID.LyricHandle = new LyricHandle( "a", "a" );
                            AppManager.addingEvent.ID.Note = note;
                        }
                        if ( AppManager.getEditMode() == EditMode.REALTIME ) {
#if ENABLE_MIDI
                            MidiPlayer.PlayImmediate( (byte)note );
#endif
                        } else {
                            KeySoundPlayer.Play( note );
                        }
                    } else {
                        if ( AppManager.isPlaying() && AppManager.addingEvent != null ) {
                            AppManager.addingEvent.ID.setLength( AppManager.getCurrentClock() - AppManager.addingEvent.Clock );
                        }
                    }
                }
            } catch ( Exception ex ) {
#if DEBUG
                AppManager.debugWriteLine( "    ex=" + ex );
#endif
                m_game_mode = GameControlMode.DISABLED;
                updateGameControlerStatus( null, null );
                if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                    AppManager.setPlaying( false );
                    AppManager.setEditMode( EditMode.NONE );
                    AppManager.addingEvent = null;
                }
                m_timer.stop();
            }
        }
#endif

        private void EditorConfig_QuantizeModeChanged( Object sender, BEventArgs e ) {
            applyQuantizeMode();
        }

        #region menuFile*
        private void menuFileSaveNamed_Click( Object sender, BEventArgs e ) {
            for ( int track = 1; track < AppManager.getVsqFile().Track.size(); track++ ) {
                if ( AppManager.getVsqFile().Track.get( track ).getEventCount() == 0 ) {
                    AppManager.showMessageBox(
                        String.Format(
                            _( "Invalid note data.\nTrack {0} : {1}\n\n-> Piano roll : Blank sequence." ), track, AppManager.getVsqFile().Track.get( track ).getName()
                        ),
                        _APP_NAME,
                        AppManager.MSGBOX_DEFAULT_OPTION,
                        AppManager.MSGBOX_WARNING_MESSAGE );
                    return;
                }
            }

            int dr = saveXmlVsqDialog.showSaveDialog( this );
            if ( dr == BFileChooser.APPROVE_OPTION ) {
                String file = saveXmlVsqDialog.getSelectedFile();
                AppManager.saveTo( file );
                updateRecentFileMenu();
                setEdited( false );
            }
        }

        private void commonFileSave_Click( Object sender, BEventArgs e ) {
            for ( int track = 1; track < AppManager.getVsqFile().Track.size(); track++ ) {
                if ( AppManager.getVsqFile().Track.get( track ).getEventCount() == 0 ) {
                    AppManager.showMessageBox(
                        String.Format(
                            _( "Invalid note data.\nTrack {0} : {1}\n\n-> Piano roll : Blank sequence." ), track, AppManager.getVsqFile().Track.get( track ).getName()
                        ),
                        _APP_NAME,
                        AppManager.MSGBOX_DEFAULT_OPTION,
                        AppManager.MSGBOX_WARNING_MESSAGE );
                    return;
                }
            }
            String file = AppManager.getFileName();
            if ( AppManager.getFileName().Equals( "" ) ) {
                int dr = saveXmlVsqDialog.showSaveDialog( this );
                if ( dr == BFileChooser.APPROVE_OPTION ) {
                    file = saveXmlVsqDialog.getSelectedFile();
                }
            }
            if ( file != "" ) {
                AppManager.saveTo( file );
                updateRecentFileMenu();
                setEdited( false );
            }
        }

        private void menuFileQuit_Click( Object sender, BEventArgs e ) {
            close();
        }

        private void menuFileExportWave_Click( Object sender, BEventArgs e ) {
            int dialog_result = BFileChooser.CANCEL_OPTION;
            String filename = "";
            BFileChooser sfd = null;
            try {
                sfd = new BFileChooser( "" );
                sfd.setDialogTitle( _( "Wave Export" ) );
                sfd.addFileFilter( _( "Wave File(*.wav)|*.wav" ) );
                sfd.addFileFilter( _( "All Files(*.*)|*.*" ) );
                dialog_result = sfd.showSaveDialog( this );
                filename = sfd.getSelectedFile();
            } catch ( Exception ex ) {
            } finally {
                if ( sfd != null ) {
                    try {
#if !JAVA
                        sfd.Dispose();
#endif
                    } catch ( Exception ex2 ) {
                    }
                }
            }

            if ( dialog_result == BFileChooser.APPROVE_OPTION ) {
                FormSynthesize fs = null;
                try {
                    fs = new FormSynthesize(
                        AppManager.getVsqFile(),
                        AppManager.editorConfig.PreSendTime,
                        new int[] { AppManager.getSelected() },
                        new String[] { filename },
                        AppManager.getVsqFile().TotalClocks + 240,
                        true );

                    double started = PortUtil.getCurrentTime();
                    fs.showDialog();
#if DEBUG
                    bocoree.debug.push_log( "elapsed time=" + (PortUtil.getCurrentTime() - started) + "sec" );
#endif
                } catch ( Exception ex ) {
                } finally {
                    if ( fs != null ) {
                        try {
                            fs.close();
                        } catch ( Exception ex2 ) {
                        }
                    }
                }
            }
        }

        private void menuFileExport_DropDownOpening( Object sender, BEventArgs e ) {
            menuFileExportWave.setEnabled( (AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventCount() > 0) && VSTiProxy.CurrentUser.Equals( "" ) );
        }

        private void menuFileImportMidi_Click( Object sender, BEventArgs e ) {
            if ( m_midi_imexport_dialog == null ) {
                m_midi_imexport_dialog = new FormMidiImExport();
            }
            m_midi_imexport_dialog.listTrack.clear();
            m_midi_imexport_dialog.setMode( FormMidiImExport.FormMidiMode.IMPORT );

            int dialog_result = openMidiDialog.showOpenDialog( this );

            if ( dialog_result != BFileChooser.APPROVE_OPTION ) {
                return;
            }
            m_midi_imexport_dialog.setLocation( getFormPreferedLocation( m_midi_imexport_dialog ) );
            MidiFile mf = null;
            try {
                mf = new MidiFile( openMidiDialog.getSelectedFile() );
            } catch ( Exception ex ) {
                AppManager.showMessageBox( _( "Invalid MIDI file." ), _( "Error" ), AppManager.MSGBOX_DEFAULT_OPTION, AppManager.MSGBOX_WARNING_MESSAGE );
                return;
            }
            if ( mf == null ) {
                AppManager.showMessageBox( _( "Invalid MIDI file." ), _( "Error" ), AppManager.MSGBOX_DEFAULT_OPTION, AppManager.MSGBOX_WARNING_MESSAGE );
                return;
            }
            int count = mf.getTrackCount();
            //Encoding def_enc = Encoding.GetEncoding( 0 );
            for ( int i = 0; i < count; i++ ) {
                String track_name = "";
                int notes = 0;
                Vector<MidiEvent> events = mf.getMidiEventList( i );
                int events_count = events.size();

                // トラック名を取得
                for ( int j = 0; j < events_count; j++ ) {
                    MidiEvent item = events.get( j );
                    if ( item.firstByte == 0xff && item.data.Length >= 2 && item.data[0] == 0x03 ) {
                        track_name = PortUtil.getDecodedString( "Shift_JIS", item.data, 1, item.data.Length - 1 );
                        break;
                    }
                }

                // イベント数を数える
                for ( int j = 0; j < events_count; j++ ) {
                    MidiEvent item = events.get( j );
                    if ( (item.firstByte & 0xf0) == 0x90 && item.data.Length > 1 && item.data[1] > 0x00 ) {
                        notes++;
                    }
                }
                m_midi_imexport_dialog.listTrack.addItem( "", new BListViewItem( new String[] { i.ToString(), track_name, notes.ToString() } ) );
                m_midi_imexport_dialog.listTrack.setItemCheckedAt( "", i, true );
            }

            if ( m_midi_imexport_dialog.showDialog() != BDialogResult.OK ) {
                return;
            }

            // インポートするしないにかかわらずテンポと拍子を取得
            VsqFileEx tempo = new VsqFileEx( "Miku", 2, 4, 4, 500000 ); //テンポリスト用のVsqFile。テンポの部分のみ使用
            tempo.executeCommand( VsqCommand.generateCommandChangePreMeasure( 0 ) );
            boolean tempo_added = false;
            boolean timesig_added = false;
            tempo.TempoTable.clear();
            tempo.TimesigTable.clear();
            int mf_getTrackCount = mf.getTrackCount();
            for ( int i = 0; i < mf_getTrackCount; i++ ) {
                Vector<MidiEvent> events = mf.getMidiEventList( i );
                boolean t_tempo_added = false;   //第iトラックからテンポをインポートしたかどうか
                boolean t_timesig_added = false; //第iトラックから拍子をインポートしたかどうか
                int events_Count = events.size();
                for ( int j = 0; j < events_Count; j++ ) {
                    MidiEvent itemj = events.get( j );
                    if ( !tempo_added && itemj.firstByte == 0xff && itemj.data.Length >= 4 && itemj.data[0] == 0x51 ) {
                        int vtempo = itemj.data[1] << 16 | itemj.data[2] << 8 | itemj.data[3];
                        tempo.TempoTable.add( new TempoTableEntry( (int)itemj.clock, vtempo, 0.0 ) );
                        t_tempo_added = true;
                    }
                    if ( !timesig_added && itemj.firstByte == 0xff && itemj.data.Length >= 5 && itemj.data[0] == 0x58 ) {
                        int num = itemj.data[1];
                        int den = 1;
                        for ( int k = 0; k < itemj.data[2]; k++ ) {
                            den = den * 2;
                        }
                        tempo.TimesigTable.add( new TimeSigTableEntry( (int)itemj.clock, num, den, 0 ) );
                        t_timesig_added = true;
                    }
                }
                if ( t_tempo_added ) {
                    tempo_added = true;
                }
                if ( t_timesig_added ) {
                    timesig_added = true;
                }
                if ( timesig_added && tempo_added ) {
                    // 両方ともインポート済みならexit。2個以上のトラックから、重複してテンポや拍子をインポートするのはNG（たぶん）
                    break;
                }
            }
            boolean contains_zero = false;
            int c = tempo.TempoTable.size();
            for ( int i = 0; i < c; i++ ) {
                if ( tempo.TempoTable.get( i ).Clock == 0 ) {
                    contains_zero = true;
                    break;
                }
            }
            if ( !contains_zero ) {
                tempo.TempoTable.add( new TempoTableEntry( 0, 500000, 0.0 ) );
            }
            contains_zero = false;
            c = tempo.TempoTable.size();
            for ( int i = 0; i < c; i++ ) {
                if ( tempo.TimesigTable.get( i ).Clock == 0 ) {
                    contains_zero = true;
                    break;
                }
            }
            if ( !contains_zero ) {
                tempo.TimesigTable.add( new TimeSigTableEntry( 0, 4, 4, 0 ) );
            }
            VsqFileEx work = (VsqFileEx)AppManager.getVsqFile().clone(); //後でReplaceコマンドを発行するための作業用
            double sec_at_premeasure = work.getSecFromClock( work.getPreMeasureClocks() );
            if ( !m_midi_imexport_dialog.isPreMeasure() ) {
                sec_at_premeasure = 0.0;
            }
            VsqFileEx copy_src = (VsqFileEx)tempo.clone();
            if ( sec_at_premeasure != 0.0 ) {
                int t = work.TempoTable.get( 0 ).Tempo;
                VsqFileEx.shift( copy_src, sec_at_premeasure, t );
            }
            tempo.updateTempoInfo();
            tempo.updateTimesigInfo();

            // tempoをインポート
            boolean import_tempo = m_midi_imexport_dialog.isTempo();
            if ( import_tempo ) {
#if DEBUG
                PortUtil.println( "FormMain#menuFileImportMidi_Click; sec_at_premeasure=" + sec_at_premeasure );
#endif
                // 最初に、workにある全てのイベント・コントロールカーブ・ベジエ曲線をtempoのテンポテーブルに合うように、シフトする
                //ShiftClockToMatchWith( work, copy_src, work.getSecFromClock( work.getPreMeasureClocks() ) );
                //ShiftClockToMatchWith( work, copy_src, copy_src.getSecFromClock( copy_src.getPreMeasureClocks() ) );
                shiftClockToMatchWith( work, copy_src, sec_at_premeasure );

                work.TempoTable.clear();
                Vector<TempoTableEntry> list = copy_src.TempoTable;
                int list_count = list.size();
                for ( int i = 0; i < list_count; i++ ) {
                    TempoTableEntry item = list.get( i );
                    work.TempoTable.add( new TempoTableEntry( item.Clock, item.Tempo, item.Time ) );
                }
                work.updateTempoInfo();
            }

            // timesig
            if ( m_midi_imexport_dialog.isTimesig() ) {
                work.TimesigTable.clear();
                Vector<TimeSigTableEntry> list = tempo.TimesigTable;
                int list_count = list.size();
                for ( int i = 0; i < list_count; i++ ) {
                    TimeSigTableEntry item = list.get( i );
                    work.TimesigTable.add( new TimeSigTableEntry( item.Clock,
                                                                  item.Numerator,
                                                                  item.Denominator,
                                                                  item.BarCount ) );
                }
                Collections.sort( work.TimesigTable );
                work.updateTimesigInfo();
            }

            for ( int i = 0; i < m_midi_imexport_dialog.listTrack.getItemCount( "" ); i++ ) {
                if ( !m_midi_imexport_dialog.listTrack.isItemCheckedAt( "", i ) ) {
                    continue;
                }
                if ( work.Track.size() + 1 > 16 ) {
                    break;
                }
                VsqTrack work_track = new VsqTrack( m_midi_imexport_dialog.listTrack.getItemAt( "", i ).getSubItemAt( 1 ), "Miku" );
                Vector<MidiEvent> events = mf.getMidiEventList( i );
                Collections.sort( events );
                int events_count = events.size();

                // note
                if ( m_midi_imexport_dialog.isNotes() ) {
                    int[] onclock_each_note = new int[128];
                    int[] velocity_each_note = new int[128];
                    for ( int j = 0; j < 128; j++ ) {
                        onclock_each_note[j] = -1;
                        velocity_each_note[j] = 64;
                    }
                    int last_note = -1;
                    for ( int j = 0; j < events_count; j++ ) {
                        MidiEvent itemj = events.get( j );
                        int not_closed_note = -1;
                        if ( (itemj.firstByte & 0xf0) == 0x90 && itemj.data.Length >= 2 && itemj.data[1] > 0 ) {
                            for ( int m = 0; m < 128; m++ ) {
                                if ( onclock_each_note[m] >= 0 ) {
                                    not_closed_note = m;
                                    break;
                                }
                            }
                        }
#if DEBUG
                        Console.WriteLine( "FormMain#menuFileImprotMidi_Click; not_closed_note=" + not_closed_note );
#endif
                        if ( ((itemj.firstByte & 0xf0) == 0x90 && itemj.data.Length >= 2 && itemj.data[1] == 0) ||
                             ((itemj.firstByte & 0xf0) == 0x80 && itemj.data.Length >= 2) ||
                             not_closed_note >= 0 ) {
                            int clock_off = (int)itemj.clock;
                            int note = (int)itemj.data[0];
                            if ( not_closed_note >= 0 ) {
                                note = not_closed_note;
                            }
                            if ( onclock_each_note[note] >= 0 ) {
                                double time_clock_on = tempo.getSecFromClock( onclock_each_note[note] ) + sec_at_premeasure;
                                double time_clock_off = tempo.getSecFromClock( clock_off ) + sec_at_premeasure;
                                int add_clock_on = (int)work.getClockFromSec( time_clock_on );
                                int add_clock_off = (int)work.getClockFromSec( time_clock_off );
                                VsqID vid = new VsqID( 0 );
                                vid.type = VsqIDType.Anote;
                                vid.setLength( add_clock_off - add_clock_on );
#if DEBUG
                                Console.WriteLine( "FormMain#menuFileImportMidi_Click; vid.Length=" + vid.getLength() );
#endif
                                String phrase = "a";
                                if ( m_midi_imexport_dialog.isLyric() ) {
                                    for ( int k = 0; k < events_count; k++ ) {
                                        MidiEvent itemk = events.get( k );
                                        if ( onclock_each_note[note] <= (int)itemk.clock && (int)itemk.clock <= clock_off ) {
                                            if ( itemk.firstByte == 0xff && itemk.data.Length >= 2 && itemk.data[0] == 0x05 ) {
                                                phrase = PortUtil.getDecodedString( "Shift_JIS", itemk.data, 1, itemk.data.Length - 1 );
                                                break;
                                            }
                                        }
                                    }
                                }
                                vid.LyricHandle = new LyricHandle( phrase, "a" );
                                vid.Note = note;
                                vid.Dynamics = velocity_each_note[note];
                                vid.DEMaccent = 50;
                                vid.DEMdecGainRate = 50;

                                // ビブラート
                                if ( AppManager.editorConfig.EnableAutoVibrato ) {
                                    int note_length = vid.getLength();
                                    // 音符位置での拍子を調べる
                                    Timesig timesig = work.getTimesigAt( add_clock_on );

                                    // ビブラートを自動追加するかどうかを決める閾値
                                    int autovib = AutoVibratoMinLengthUtil.getValue( AppManager.editorConfig.AutoVibratoMinimumLength );
                                    int threshold = 480 * 4 / timesig.denominator * autovib;
                                    if ( note_length >= threshold ) {
                                        int vibrato_clocks = 0;
                                        DefaultVibratoLengthEnum vib_length = AppManager.editorConfig.DefaultVibratoLength;
                                        if ( vib_length == DefaultVibratoLengthEnum.L100 ) {
                                            vibrato_clocks = note_length;
                                        } else if ( vib_length == DefaultVibratoLengthEnum.L50 ) {
                                            vibrato_clocks = note_length / 2;
                                        } else if ( vib_length == DefaultVibratoLengthEnum.L66 ) {
                                            vibrato_clocks = note_length * 2 / 3;
                                        } else if ( vib_length == DefaultVibratoLengthEnum.L75 ) {
                                            vibrato_clocks = note_length * 3 / 4;
                                        }
                                        // とりあえずVOCALOID2のデフォルトビブラートの設定を使用
                                        vid.VibratoHandle = VocaloSysUtil.getDefaultVibratoHandle( AppManager.editorConfig.AutoVibratoType2,
                                                                                                   vibrato_clocks,
                                                                                                   SynthesizerType.VOCALOID2 );
                                        vid.VibratoDelay = note_length - vibrato_clocks;
                                    }
                                }

                                VsqEvent ve = new VsqEvent( add_clock_on, vid );
                                work_track.addEvent( ve );
                                onclock_each_note[note] = -1;
                            }
                        }
                        if ( (itemj.firstByte & 0xf0) == 0x90 && itemj.data.Length >= 2 && itemj.data[1] > 0 ) {
                            int note = itemj.data[0];
                            onclock_each_note[note] = (int)itemj.clock;
                            int vel = itemj.data[1];
                            velocity_each_note[note] = vel;
                            last_note = note;
                        }
                    }

                    int track = work.Track.size();
                    CadenciiCommand run_add = VsqFileEx.generateCommandAddTrack( work_track,
                                                                                 new VsqMixerEntry( 0, 0, 0, 0 ),
                                                                                 track,
                                                                                 new BezierCurves() );
                    work.executeCommand( run_add );
                }
            }

            CadenciiCommand lastrun = VsqFileEx.generateCommandReplace( work );
            AppManager.register( AppManager.getVsqFile().executeCommand( lastrun ) );
            setEdited( true );
            refreshScreen();
        }

        private void menuFileExportMidi_Click( Object sender, BEventArgs e ) {
            if ( m_midi_imexport_dialog == null ) {
                m_midi_imexport_dialog = new FormMidiImExport();
            }
            m_midi_imexport_dialog.listTrack.clear();
            VsqFileEx vsq = (VsqFileEx)AppManager.getVsqFile().Clone();

            for ( int i = 0; i < vsq.Track.size(); i++ ) {
                VsqTrack track = vsq.Track.get( i );
                int notes = 0;
                for ( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                    Object obj = itr.next();
                    notes++;
                }
                m_midi_imexport_dialog.listTrack.addItem( "", new BListViewItem( new String[] { i.ToString(), track.getName(), notes.ToString() } ) );
                m_midi_imexport_dialog.listTrack.setItemCheckedAt( "", i, true );
            }
            m_midi_imexport_dialog.setMode( FormMidiImExport.FormMidiMode.EXPORT );
            m_midi_imexport_dialog.setLocation( getFormPreferedLocation( m_midi_imexport_dialog ) );
            if ( m_midi_imexport_dialog.showDialog() == BDialogResult.OK ) {
                if ( !m_midi_imexport_dialog.isPreMeasure() ) {
                    vsq.removePart( 0, vsq.getPreMeasureClocks() );
                }
                int track_count = 0;
                for ( int i = 0; i < m_midi_imexport_dialog.listTrack.getItemCount( "" ); i++ ) {
                    if ( m_midi_imexport_dialog.listTrack.isItemCheckedAt( "", i ) ) {
                        track_count++;
                    }
                }
                if ( track_count == 0 ) {
                    return;
                }

                int dialog_result = saveMidiDialog.showSaveDialog( this );

                if ( dialog_result == BFileChooser.APPROVE_OPTION ) {
                    RandomAccessFile fs = null;
                    try {
                        fs = new RandomAccessFile( saveMidiDialog.getSelectedFile(), "rw" );
                        // ヘッダー
                        fs.write( new byte[] { 0x4d, 0x54, 0x68, 0x64 }, 0, 4 );
                        //データ長
                        fs.write( (byte)0x00 );
                        fs.write( (byte)0x00 );
                        fs.write( (byte)0x00 );
                        fs.write( (byte)0x06 );
                        //フォーマット
                        fs.write( (byte)0x00 );
                        fs.write( (byte)0x01 );
                        //トラック数
                        VsqFile.writeUnsignedShort( fs, (ushort)track_count );
                        //時間単位
                        fs.write( (byte)0x01 );
                        fs.write( (byte)0xe0 );
                        int count = -1;
                        for ( int i = 0; i < m_midi_imexport_dialog.listTrack.getItemCount( "" ); i++ ) {
                            if ( !m_midi_imexport_dialog.listTrack.isItemCheckedAt( "", i ) ) {
                                continue;
                            }
                            VsqTrack track = vsq.Track.get( i );
                            count++;
                            fs.write( new byte[] { 0x4d, 0x54, 0x72, 0x6b }, 0, 4 );
                            //データ長。とりあえず0を入れておく
                            fs.write( new byte[] { 0x00, 0x00, 0x00, 0x00 }, 0, 4 );
                            long first_position = fs.getFilePointer();
                            //トラック名
                            VsqFile.writeFlexibleLengthUnsignedLong( fs, 0 );//デルタタイム
                            fs.write( (byte)0xff );//ステータスタイプ
                            fs.write( (byte)0x03 );//イベントタイプSequence/Track Name
                            byte[] track_name = PortUtil.getEncodedByte( "Shift_JIS", track.getName() );
                            fs.write( (byte)track_name.Length );
                            fs.write( track_name, 0, track_name.Length );

                            Vector<MidiEvent> events = new Vector<MidiEvent>();

                            // tempo
                            boolean print_tempo = m_midi_imexport_dialog.isTempo();
                            if ( print_tempo && count == 0 ) {
                                Vector<MidiEvent> tempo_events = vsq.generateTempoChange();
                                for ( int j = 0; j < tempo_events.size(); j++ ) {
                                    events.add( tempo_events.get( j ) );
                                }
                            }

                            // timesig
                            if ( m_midi_imexport_dialog.isTimesig() && count == 0 ) {
                                Vector<MidiEvent> timesig_events = vsq.generateTimeSig();
                                for ( int j = 0; j < timesig_events.size(); j++ ) {
                                    events.add( timesig_events.get( j ) );
                                }
                            }

                            // Notes
                            if ( m_midi_imexport_dialog.isNotes() ) {
                                for ( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = (VsqEvent)itr.next();
                                    int clock_on = ve.Clock;
                                    int clock_off = ve.Clock + ve.ID.getLength();
                                    if ( !print_tempo ) {
                                        // テンポを出力しない場合、テンポを500000（120）と見なしてクロックを再計算
                                        double time_on = vsq.getSecFromClock( clock_on );
                                        double time_off = vsq.getSecFromClock( clock_off );
                                        clock_on = (int)(960.0 * time_on);
                                        clock_off = (int)(960.0 * time_off);
                                    }
                                    MidiEvent noteon = new MidiEvent();
                                    noteon.clock = clock_on;
                                    noteon.firstByte = 0x90;
                                    noteon.data = new byte[2];
                                    noteon.data[0] = (byte)ve.ID.Note;
                                    noteon.data[1] = (byte)ve.ID.Dynamics;
                                    events.add( noteon );
                                    MidiEvent noteoff = new MidiEvent();
                                    noteoff.clock = clock_off;
                                    noteoff.firstByte = 0x80;
                                    noteoff.data = new byte[2];
                                    noteoff.data[0] = (byte)ve.ID.Note;
                                    noteoff.data[1] = 0x7f;
                                    events.add( noteoff );
                                }
                            }

                            // lyric
                            if ( m_midi_imexport_dialog.isLyric() ) {
                                for ( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = (VsqEvent)itr.next();
                                    int clock_on = ve.Clock;
                                    if ( !print_tempo ) {
                                        double time_on = vsq.getSecFromClock( clock_on );
                                        clock_on = (int)(960.0 * time_on);
                                    }
                                    MidiEvent add = new MidiEvent();
                                    add.clock = clock_on;
                                    add.firstByte = 0xff;
                                    byte[] lyric = PortUtil.getEncodedByte( "Shift_JIS", ve.ID.LyricHandle.L0.Phrase );
                                    add.data = new byte[lyric.Length + 1];
                                    add.data[0] = 0x05;
                                    for ( int j = 0; j < lyric.Length; j++ ) {
                                        add.data[j + 1] = lyric[j];
                                    }
                                    events.add( add );
                                }
                            }

                            // vocaloid metatext
                            Vector<MidiEvent> meta;
                            if ( m_midi_imexport_dialog.isVocaloidMetatext() && i > 0 ) {
                                meta = vsq.generateMetaTextEvent( i, "Shift_JIS" );
                            } else {
                                meta = new Vector<MidiEvent>();
                            }

                            // vocaloid nrpn
                            Vector<MidiEvent> vocaloid_nrpn_midievent;
                            if ( m_midi_imexport_dialog.isVocaloidNrpn() && i > 0 ) {
                                VsqNrpn[] vsqnrpn = VsqFileEx.generateNRPN( (VsqFile)vsq, i, AppManager.editorConfig.PreSendTime );
                                NrpnData[] nrpn = VsqNrpn.convert( vsqnrpn );

                                vocaloid_nrpn_midievent = new Vector<MidiEvent>();
                                for ( int j = 0; j < nrpn.Length; j++ ) {
                                    MidiEvent me = new MidiEvent();
                                    me.clock = nrpn[j].getClock();
                                    me.firstByte = 0xb0;
                                    me.data = new byte[2];
                                    me.data[0] = nrpn[j].getParameter();
                                    me.data[1] = nrpn[j].Value;
                                    vocaloid_nrpn_midievent.add( me );
                                }
                            } else {
                                vocaloid_nrpn_midievent = new Vector<MidiEvent>();
                            }
#if DEBUG
                            PortUtil.println( "menuFileExportMidi_Click" );
                            PortUtil.println( "    vocaloid_nrpn_midievent.size()=" + vocaloid_nrpn_midievent.size() );
#endif

                            // midi eventを出力
                            Collections.sort( events );
                            long last_clock = 0;
                            int events_count = events.size();
                            if ( events_count > 0 ) {
                                for ( int j = 0; j < events_count; j++ ) {
                                    if ( events.get( j ).clock > 0 && meta.size() > 0 ) {
                                        for ( int k = 0; k < meta.size(); k++ ) {
                                            VsqFile.writeFlexibleLengthUnsignedLong( fs, 0 );
                                            meta.get( k ).writeData( fs );
                                        }
                                        meta.clear();
                                        last_clock = 0;
                                    }
                                    long clock = events.get( j ).clock;
                                    while ( vocaloid_nrpn_midievent.size() > 0 && vocaloid_nrpn_midievent.get( 0 ).clock < clock ) {
                                        VsqFile.writeFlexibleLengthUnsignedLong( fs, (long)(vocaloid_nrpn_midievent.get( 0 ).clock - last_clock) );
                                        last_clock = vocaloid_nrpn_midievent.get( 0 ).clock;
                                        vocaloid_nrpn_midievent.get( 0 ).writeData( fs );
                                        vocaloid_nrpn_midievent.removeElementAt( 0 );
                                    }
                                    VsqFile.writeFlexibleLengthUnsignedLong( fs, (long)(events.get( j ).clock - last_clock) );
                                    events.get( j ).writeData( fs );
                                    last_clock = events.get( j ).clock;
                                }
                            } else {
                                int c = vocaloid_nrpn_midievent.size();
                                for ( int k = 0; k < meta.size(); k++ ) {
                                    VsqFile.writeFlexibleLengthUnsignedLong( fs, 0 );
                                    meta.get( k ).writeData( fs );
                                }
                                meta.clear();
                                last_clock = 0;
                                for ( int j = 0; j < c; j++ ) {
                                    MidiEvent item = vocaloid_nrpn_midievent.get( j );
                                    long clock = item.clock;
                                    VsqFile.writeFlexibleLengthUnsignedLong( fs, (long)(clock - last_clock) );
                                    item.writeData( fs );
                                    last_clock = clock;
                                }
                            }

                            // トラックエンドを記入し、
                            VsqFile.writeFlexibleLengthUnsignedLong( fs, (long)0 );
                            fs.write( (byte)0xff );
                            fs.write( (byte)0x2f );
                            fs.write( (byte)0x00 );
                            // チャンクの先頭に戻ってチャンクのサイズを記入
                            long pos = fs.getFilePointer();
                            fs.seek( first_position - 4 );
                            VsqFile.writeUnsignedInt( fs, (uint)(pos - first_position) );
                            // ファイルを元の位置にseek
                            fs.seek( pos );
                        }
                    } catch ( Exception ex ) {
                    } finally {
                        if ( fs != null ) {
                            try {
                                fs.close();
                            } catch ( Exception ex2 ) {
                            }
                        }
                    }
                }
            }
        }

        private void menuFileOpenVsq_Click( Object sender, BEventArgs e ) {
            if ( !dirtyCheck() ) {
                return;
            }

            String[] filters = openMidiDialog.getChoosableFileFilter();
            String filter = "";
            foreach ( String f in filters ) {
                if ( f.EndsWith( AppManager.editorConfig.LastUsedExtension ) ) {
                    filter = f;
                    break;
                }
            }

            openMidiDialog.setFileFilter( filter );
            int dialog_result = openMidiDialog.showOpenDialog( this );
            if ( dialog_result == BFileChooser.APPROVE_OPTION ) {
#if DEBUG
                AppManager.debugWriteLine( "openMidiDialog.FilterIndex=" + openMidiDialog.getFileFilter() );
#endif
                if ( openMidiDialog.getFileFilter().EndsWith( ".mid" ) ) {
                    AppManager.editorConfig.LastUsedExtension = ".mid";
                } else if ( openMidiDialog.getFileFilter().EndsWith( ".vsq" ) ) {
                    AppManager.editorConfig.LastUsedExtension = ".vsq";
                }
            } else {
                return;
            }
            try {
                VsqFileEx vsq = new VsqFileEx( openMidiDialog.getSelectedFile(), "Shift_JIS" );
                AppManager.setVsqFile( vsq );
            } catch ( Exception ex ) {
#if DEBUG
                PortUtil.println( "FormMain#menuFileOpenVsq_Click; ex=" + ex );
#endif
                AppManager.showMessageBox( _( "Invalid VSQ/VOCALOID MIDI file" ), _( "Error" ), AppManager.MSGBOX_DEFAULT_OPTION, AppManager.MSGBOX_WARNING_MESSAGE );
                return;
            }
            AppManager.setSelected( 1 );
            clearExistingData();
            setEdited( false );
            AppManager.mixerWindow.updateStatus();
            clearTempWave();
#if USE_DOBJ
            updateDrawObjectList();
#endif
            refreshScreen();
        }

        private void menuFileOpenUst_Click( Object sender, BEventArgs e ) {
            if ( !dirtyCheck() ) {
                return;
            }
            int dialog_result = openUstDialog.showOpenDialog( this );

            if ( dialog_result != BFileChooser.APPROVE_OPTION ) {
                return;
            }

            try {
                UstFile ust = new UstFile( openUstDialog.getSelectedFile() );
                VsqFileEx vsq = new VsqFileEx( ust );
                clearExistingData();
                AppManager.setVsqFile( vsq );
                setEdited( false );
                AppManager.mixerWindow.updateStatus();
                clearTempWave();
#if USE_DOBJ
                updateDrawObjectList();
#endif
                refreshScreen();
            } catch ( Exception ex ) {
#if DEBUG
                PortUtil.println( "FormMain#menuFileOpenUst_Click; ex=" + ex );
#endif
            }
        }

        private void menuFileImportVsq_Click( Object sender, BEventArgs e ) {
            int dialog_result = openMidiDialog.showOpenDialog( this );

            if ( dialog_result != BFileChooser.APPROVE_OPTION ) {
                return;
            }
            VsqFileEx vsq = null;
            try {
                vsq = new VsqFileEx( openMidiDialog.getSelectedFile(), "Shift_JIS" );
            } catch ( Exception ex ) {
                AppManager.showMessageBox( _( "Invalid VSQ/VOCALOID MIDI file" ), _( "Error" ), AppManager.MSGBOX_DEFAULT_OPTION, AppManager.MSGBOX_WARNING_MESSAGE );
                return;
            }
            if ( m_midi_imexport_dialog == null ) {
                m_midi_imexport_dialog = new FormMidiImExport();
            }
            m_midi_imexport_dialog.listTrack.clear();
            for ( int track = 1; track < vsq.Track.size(); track++ ) {
                m_midi_imexport_dialog.listTrack.addItem( "", new BListViewItem( new String[] { track.ToString(), 
                                                                                                vsq.Track.get( track ).getName(),
                                                                                                vsq.Track.get( track ).getEventCount().ToString() } ) );
                m_midi_imexport_dialog.listTrack.setItemCheckedAt( "", track - 1, true );
            }
            m_midi_imexport_dialog.setMode( FormMidiImExport.FormMidiMode.IMPORT_VSQ );
            m_midi_imexport_dialog.setTempo( false );
            m_midi_imexport_dialog.setTimesig( false );
            m_midi_imexport_dialog.setLocation( getFormPreferedLocation( m_midi_imexport_dialog ) );
            if ( m_midi_imexport_dialog.showDialog() != BDialogResult.OK ) {
                return;
            }

            Vector<Integer> add_track = new Vector<Integer>();
            for ( int i = 0; i < m_midi_imexport_dialog.listTrack.getItemCount( "" ); i++ ) {
                if ( m_midi_imexport_dialog.listTrack.isItemCheckedAt( "", i ) ) {
                    add_track.add( i + 1 );
                }
            }
            if ( add_track.size() <= 0 ) {
                return;
            }

            VsqFileEx replace = (VsqFileEx)AppManager.getVsqFile().clone();
            double premeasure_sec_replace = replace.getSecFromClock( replace.getPreMeasureClocks() );
            double premeasure_sec_vsq = vsq.getSecFromClock( vsq.getPreMeasureClocks() );

            if ( m_midi_imexport_dialog.isTempo() ) {
                shiftClockToMatchWith( replace, vsq, premeasure_sec_replace - premeasure_sec_vsq );
                // テンポテーブルを置き換え
                replace.TempoTable.clear();
                for ( int i = 0; i < vsq.TempoTable.size(); i++ ) {
                    replace.TempoTable.add( (TempoTableEntry)vsq.TempoTable.get( i ).clone() );
                }
                replace.updateTempoInfo();
                replace.updateTotalClocks();
            }

            if ( m_midi_imexport_dialog.isTimesig() ) {
                // 拍子をリプレースする場合
                replace.TimesigTable.clear();
                for ( int i = 0; i < vsq.TimesigTable.size(); i++ ) {
                    replace.TimesigTable.add( (TimeSigTableEntry)vsq.TimesigTable.get( i ).clone() );
                }
                replace.updateTimesigInfo();
            }

            for ( Iterator itr = add_track.iterator(); itr.hasNext(); ) {
                int track = (Integer)itr.next();
                if ( replace.Track.size() + 1 >= 16 ) {
                    break;
                }
                if ( !m_midi_imexport_dialog.isTempo() ) {
                    // テンポをリプレースしない場合。インポートするトラックのクロックを調節する
                    for ( Iterator itr2 = vsq.Track.get( track ).getEventIterator(); itr2.hasNext(); ) {
                        VsqEvent item = (VsqEvent)itr2.next();
                        if ( item.ID.type == VsqIDType.Singer && item.Clock == 0 ) {
                            continue;
                        }
                        int clock = item.Clock;
                        double sec_start = vsq.getSecFromClock( clock ) - premeasure_sec_vsq + premeasure_sec_replace;
                        double sec_end = vsq.getSecFromClock( clock + item.ID.getLength() ) - premeasure_sec_vsq + premeasure_sec_replace;
                        int clock_start = (int)replace.getClockFromSec( sec_start );
                        int clock_end = (int)replace.getClockFromSec( sec_end );
                        item.Clock = clock_start;
                        item.ID.setLength( clock_end - clock_start );
                        if ( item.ID.VibratoHandle != null ) {
                            double sec_vib_start = vsq.getSecFromClock( clock + item.ID.VibratoDelay ) - premeasure_sec_vsq + premeasure_sec_replace;
                            int clock_vib_start = (int)replace.getClockFromSec( sec_vib_start );
                            item.ID.VibratoDelay = clock_vib_start - clock_start;
                            item.ID.VibratoHandle.setLength( clock_end - clock_vib_start );
                        }
                    }

                    // コントロールカーブをシフト
                    foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                        VsqBPList item = vsq.Track.get( track ).getCurve( ct.getName() );
                        if ( item == null ) {
                            continue;
                        }
                        VsqBPList repl = new VsqBPList( item.getDefault(), item.getMinimum(), item.getMaximum() );
                        for ( int i = 0; i < item.size(); i++ ) {
                            int clock = item.getKeyClock( i );
                            int value = item.getElement( i );
                            double sec = vsq.getSecFromClock( clock ) - premeasure_sec_vsq + premeasure_sec_replace;
                            if ( sec >= premeasure_sec_replace ) {
                                int clock_new = (int)replace.getClockFromSec( sec );
                                repl.add( clock_new, value );
                            }
                        }
                        vsq.Track.get( track ).setCurve( ct.getName(), repl );
                    }

                    // ベジエカーブをシフト
                    foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                        Vector<BezierChain> list = vsq.AttachedCurves.get( track - 1 ).get( ct );
                        if ( list == null ) {
                            continue;
                        }
                        for ( Iterator itr2 = list.iterator(); itr2.hasNext(); ) {
                            BezierChain chain = (BezierChain)itr2.next();
                            for ( Iterator itr3 = chain.points.iterator(); itr3.hasNext(); ) {
                                BezierPoint point = (BezierPoint)itr3.next();
                                PointD bse = new PointD( replace.getClockFromSec( vsq.getSecFromClock( point.getBase().getX() ) - premeasure_sec_vsq + premeasure_sec_replace ),
                                                         point.getBase().getY() );
                                PointD ctrl_r = new PointD( replace.getClockFromSec( vsq.getSecFromClock( point.controlLeft.getX() ) - premeasure_sec_vsq + premeasure_sec_replace ),
                                                            point.controlLeft.getY() );
                                PointD ctrl_l = new PointD( replace.getClockFromSec( vsq.getSecFromClock( point.controlRight.getX() ) - premeasure_sec_vsq + premeasure_sec_replace ),
                                                            point.controlRight.getY() );
                                point.setBase( bse );
                                point.controlLeft = ctrl_l;
                                point.controlRight = ctrl_r;
                            }
                        }
                    }
                }
                replace.Mixer.Slave.add( new VsqMixerEntry() );
                replace.Track.add( vsq.Track.get( track ) );
                replace.AttachedCurves.add( vsq.AttachedCurves.get( track - 1 ) );
            }

            // コマンドを発行し、実行
            CadenciiCommand run = VsqFileEx.generateCommandReplace( replace );
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
            setEdited( true );
        }

        private void commonFileOpen_Click( Object sender, BEventArgs e ) {
            if ( !dirtyCheck() ) {
                return;
            }
            int dialog_result = openXmlVsqDialog.showOpenDialog( this );
            if ( dialog_result == BFileChooser.APPROVE_OPTION ) {
                if ( AppManager.isPlaying() ) {
                    AppManager.setPlaying( false );
                }
                openVsqCor( openXmlVsqDialog.getSelectedFile() );
                clearExistingData();
                setEdited( false );
                AppManager.mixerWindow.updateStatus();
                clearTempWave();
#if USE_DOBJ
                updateDrawObjectList();
#endif
                refreshScreen();
            }
        }

        private void commonFileNew_Click( Object sender, BEventArgs e ) {
            if ( !dirtyCheck() ) {
                return;
            }
            AppManager.setSelected( 1 );
            AppManager.setVsqFile( new VsqFileEx( AppManager.editorConfig.DefaultSingerName, AppManager.editorConfig.DefaultPreMeasure, 4, 4, 500000 ) );
            clearExistingData();
            setEdited( false );
            AppManager.mixerWindow.updateStatus();
            clearTempWave();
#if USE_DOBJ
            updateDrawObjectList();
#endif
            refreshScreen();
        }

        private void menuFileNew_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Create new project." ) );
        }

        private void menuFileOpen_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Open Cadencii project." ) );
        }

        private void menuFileSave_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Save current project." ) );
        }

        private void menuFileSaveNamed_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Save current project with new name." ) );
        }

        private void menuFileOpenVsq_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Open VSQ / VOCALOID MIDI and create new project." ) );
        }

        private void menuFileOpenUst_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Open UTAU project and create new project." ) );
        }

        private void menuFileImport_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Import." ) );
        }

        private void menuFileImportVsq_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Import VSQ / VOCALOID MIDI." ) );
        }

        private void menuFileImportMidi_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Import Standard MIDI." ) );
        }

        private void menuFileExportWave_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Export to WAVE file." ) );
        }

        private void menuFileExportMidi_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Export to Standard MIDI." ) );
        }

        private void menuFileRecent_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Recent projects." ) );
        }

        private void menuFileQuit_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Close this window." ) );
        }
        #endregion

        #region menuSetting*
        private void menuSettingDefaultSingerStyle_Click( Object sender, BEventArgs e ) {
            FormSingerStyleConfig dlg = null;
            try {
                dlg = new FormSingerStyleConfig();
                dlg.setPMBendDepth( AppManager.editorConfig.DefaultPMBendDepth );
                dlg.setPMBendLength( AppManager.editorConfig.DefaultPMBendLength );
                dlg.setPMbPortamentoUse( AppManager.editorConfig.DefaultPMbPortamentoUse );
                dlg.setDEMdecGainRate( AppManager.editorConfig.DefaultDEMdecGainRate );
                dlg.setDEMaccent( AppManager.editorConfig.DefaultDEMaccent );

                dlg.setLocation( getFormPreferedLocation( dlg ) );
                if ( dlg.showDialog() == BDialogResult.OK ) {
                    if ( dlg.getApplyCurrentTrack() ) {
                        VsqTrack copy = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).clone();
                        boolean changed = false;
                        for ( int i = 0; i < copy.getEventCount(); i++ ) {
                            if ( copy.getEvent( i ).ID.type == VsqIDType.Anote ) {
                                copy.getEvent( i ).ID.PMBendDepth = dlg.getPMBendDepth();
                                copy.getEvent( i ).ID.PMBendLength = dlg.getPMBendLength();
                                copy.getEvent( i ).ID.PMbPortamentoUse = dlg.getPMbPortamentoUse();
                                copy.getEvent( i ).ID.DEMdecGainRate = dlg.getDEMdecGainRate();
                                copy.getEvent( i ).ID.DEMaccent = dlg.getDEMaccent();
                                changed = true;
                            }
                        }
                        if ( changed ) {
                            CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(),
                                                                                         copy,
                                                                                         AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
#if USE_DOBJ
                            updateDrawObjectList();
#endif
                            refreshScreen();
                        }
                    }
                    AppManager.editorConfig.DefaultPMBendDepth = dlg.getPMBendDepth();
                    AppManager.editorConfig.DefaultPMBendLength = dlg.getPMBendLength();
                    AppManager.editorConfig.DefaultPMbPortamentoUse = dlg.getPMbPortamentoUse();
                    AppManager.editorConfig.DefaultDEMdecGainRate = dlg.getDEMdecGainRate();
                    AppManager.editorConfig.DefaultDEMaccent = dlg.getDEMaccent();
                }
            } catch ( Exception ex ) {
            } finally {
                if ( dlg != null ) {
                    try {
                        dlg.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

#if ENABLE_MIDI
        private void menuSettingMidi_Click( Object sender, BEventArgs e ) {
            FormMidiConfig form = null;
            try {
                form = new FormMidiConfig();
                form.setLocation( getFormPreferedLocation( form ) );
                if ( form.showDialog() == BDialogResult.OK ) {

                }
            } catch ( Exception ex ) {
            } finally {
                if ( form != null ) {
                    try {
                        form.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }
#endif

        private void menuSettingPreference_Click( Object sender, BEventArgs e ) {
            if ( m_preference_dlg == null ) {
                m_preference_dlg = new Preference();
            }
            m_preference_dlg.setBaseFont( new Font( AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, 9 ) );
            m_preference_dlg.setScreenFont( new Font( AppManager.editorConfig.ScreenFontName, java.awt.Font.PLAIN, 9 ) );
            m_preference_dlg.setWheelOrder( AppManager.editorConfig.WheelOrder );
            m_preference_dlg.setCursorFixed( AppManager.editorConfig.CursorFixed );
            m_preference_dlg.setDefaultVibratoLength( AppManager.editorConfig.DefaultVibratoLength );
            m_preference_dlg.setAutoVibratoMinimumLength( AppManager.editorConfig.AutoVibratoMinimumLength );
            m_preference_dlg.setAutoVibratoType1( AppManager.editorConfig.AutoVibratoType1 );
            m_preference_dlg.setAutoVibratoType2( AppManager.editorConfig.AutoVibratoType2 );
            m_preference_dlg.setEnableAutoVibrato( AppManager.editorConfig.EnableAutoVibrato );
            m_preference_dlg.setPreMeasure( AppManager.getVsqFile().getPreMeasure() );
            m_preference_dlg.setPreSendTime( AppManager.editorConfig.PreSendTime );
            m_preference_dlg.setControlCurveResolution( AppManager.editorConfig.ControlCurveResolution );
            m_preference_dlg.setDefaultSingerName( AppManager.editorConfig.DefaultSingerName );
            m_preference_dlg.setScrollHorizontalOnWheel( AppManager.editorConfig.ScrollHorizontalOnWheel );
            m_preference_dlg.setMaximumFrameRate( AppManager.editorConfig.MaximumFrameRate );
            m_preference_dlg.setPlatform( AppManager.editorConfig.Platform );
            m_preference_dlg.setKeepLyricInputMode( AppManager.editorConfig.KeepLyricInputMode );
            m_preference_dlg.setPxTrackHeight( AppManager.editorConfig.PxTrackHeight );
            m_preference_dlg.setMouseHoverTime( AppManager.editorConfig.MouseHoverTime );
            m_preference_dlg.setPlayPreviewWhenRightClick( AppManager.editorConfig.PlayPreviewWhenRightClick );
            m_preference_dlg.setCurveSelectingQuantized( AppManager.editorConfig.CurveSelectingQuantized );
            m_preference_dlg.setCurveVisibleAccent( AppManager.editorConfig.CurveVisibleAccent );
            m_preference_dlg.setCurveVisibleBre( AppManager.editorConfig.CurveVisibleBreathiness );
            m_preference_dlg.setCurveVisibleBri( AppManager.editorConfig.CurveVisibleBrightness );
            m_preference_dlg.setCurveVisibleCle( AppManager.editorConfig.CurveVisibleClearness );
            m_preference_dlg.setCurveVisibleDecay( AppManager.editorConfig.CurveVisibleDecay );
            m_preference_dlg.setCurveVisibleDyn( AppManager.editorConfig.CurveVisibleDynamics );
            m_preference_dlg.setCurveVisibleGen( AppManager.editorConfig.CurveVisibleGendorfactor );
            m_preference_dlg.setCurveVisibleOpe( AppManager.editorConfig.CurveVisibleOpening );
            m_preference_dlg.setCurveVisiblePit( AppManager.editorConfig.CurveVisiblePit );
            m_preference_dlg.setCurveVisiblePbs( AppManager.editorConfig.CurveVisiblePbs );
            m_preference_dlg.setCurveVisiblePor( AppManager.editorConfig.CurveVisiblePortamento );
            m_preference_dlg.setCurveVisibleVel( AppManager.editorConfig.CurveVisibleVelocity );
            m_preference_dlg.setCurveVisibleVibratoDepth( AppManager.editorConfig.CurveVisibleVibratoDepth );
            m_preference_dlg.setCurveVisibleVibratoRate( AppManager.editorConfig.CurveVisibleVibratoRate );
            m_preference_dlg.setCurveVisibleFx2Depth( AppManager.editorConfig.CurveVisibleFx2Depth );
            m_preference_dlg.setCurveVisibleHarmonics( AppManager.editorConfig.CurveVisibleHarmonics );
            m_preference_dlg.setCurveVisibleReso1( AppManager.editorConfig.CurveVisibleReso1 );
            m_preference_dlg.setCurveVisibleReso2( AppManager.editorConfig.CurveVisibleReso2 );
            m_preference_dlg.setCurveVisibleReso3( AppManager.editorConfig.CurveVisibleReso3 );
            m_preference_dlg.setCurveVisibleReso4( AppManager.editorConfig.CurveVisibleReso4 );
            m_preference_dlg.setCurveVisibleEnvelope( AppManager.editorConfig.CurveVisibleEnvelope );
#if ENABLE_MIDI
            m_preference_dlg.setMidiInPort( AppManager.editorConfig.MidiInPort.PortNumber );
#endif
            m_preference_dlg.setInvokeWithWine( AppManager.editorConfig.InvokeUtauCoreWithWine );
            m_preference_dlg.setPathResampler( AppManager.editorConfig.PathResampler );
            m_preference_dlg.setPathWavtool( AppManager.editorConfig.PathWavtool );
            m_preference_dlg.setUtauSingers( AppManager.editorConfig.UtauSingers );
            m_preference_dlg.setSelfDeRomantization( AppManager.editorConfig.SelfDeRomanization );
            m_preference_dlg.setAutoBackupIntervalMinutes( AppManager.editorConfig.AutoBackupIntervalMinutes );
            m_preference_dlg.setUseSpaceKeyAsMiddleButtonModifier( AppManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier );

            m_preference_dlg.setLocation( getFormPreferedLocation( m_preference_dlg ) );

            if ( m_preference_dlg.showDialog() == BDialogResult.OK ) {
                AppManager.editorConfig.BaseFontName = m_preference_dlg.getBaseFont().getName();
                AppManager.editorConfig.BaseFontSize = m_preference_dlg.getBaseFont().getSize2D();
                updateMenuFonts();

                AppManager.editorConfig.ScreenFontName = m_preference_dlg.getScreenFont().getName();
                //AppManager.EditorConfig.CounterFontName = m_preference_dlg.CounterFont.Name;
                AppManager.editorConfig.WheelOrder = m_preference_dlg.getWheelOrder();
                AppManager.editorConfig.CursorFixed = m_preference_dlg.isCursorFixed();

                AppManager.editorConfig.DefaultVibratoLength = m_preference_dlg.getDefaultVibratoLength();
                AppManager.editorConfig.AutoVibratoMinimumLength = m_preference_dlg.getAutoVibratoMinimumLength();
                AppManager.editorConfig.AutoVibratoType1 = m_preference_dlg.getAutoVibratoType1();
                AppManager.editorConfig.AutoVibratoType2 = m_preference_dlg.getAutoVibratoType2();

                AppManager.editorConfig.EnableAutoVibrato = m_preference_dlg.isEnableAutoVibrato();
                AppManager.editorConfig.PreSendTime = m_preference_dlg.getPreSendTime();
                AppManager.editorConfig.DefaultPreMeasure = m_preference_dlg.getPreMeasure();
                if ( m_preference_dlg.getPreMeasure() != AppManager.getVsqFile().getPreMeasure() ) {
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandChangePreMeasure( m_preference_dlg.getPreMeasure() ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    setEdited( true );
                }
                AppManager.editorConfig.Language = m_preference_dlg.getLanguage();
                if ( !Messaging.getLanguage().Equals( AppManager.editorConfig.Language ) ) {
                    Messaging.setLanguage( AppManager.editorConfig.Language );
                    applyLanguage();
                    m_preference_dlg.ApplyLanguage();
                    AppManager.mixerWindow.applyLanguage();
                    if ( m_versioninfo != null && !m_versioninfo.IsDisposed ) {
                        m_versioninfo.ApplyLanguage();
                    }
#if ENABLE_PROPERTY
                    AppManager.propertyWindow.ApplyLanguage();
                    AppManager.propertyPanel.UpdateValue( AppManager.getSelected() );
#endif
                }

                AppManager.editorConfig.ControlCurveResolution = m_preference_dlg.getControlCurveResolution();
                AppManager.editorConfig.DefaultSingerName = m_preference_dlg.getDefaultSingerName();
                AppManager.editorConfig.ScrollHorizontalOnWheel = m_preference_dlg.isScrollHorizontalOnWheel();
                AppManager.editorConfig.MaximumFrameRate = m_preference_dlg.getMaximumFrameRate();
                int fps = 1000 / AppManager.editorConfig.MaximumFrameRate;
                timer.setDelay( (fps <= 0) ? 1 : fps );
                AppManager.editorConfig.Platform = m_preference_dlg.getPlatform();
                s_modifier_key = ((AppManager.editorConfig.Platform == PlatformEnum.Macintosh) ? InputEvent.META_MASK : InputEvent.CTRL_MASK);
                applyShortcut();
                AppManager.editorConfig.KeepLyricInputMode = m_preference_dlg.isKeepLyricInputMode();
                if ( AppManager.editorConfig.PxTrackHeight != m_preference_dlg.getPxTrackHeight() ) {
                    AppManager.editorConfig.PxTrackHeight = m_preference_dlg.getPxTrackHeight();
#if USE_DOBJ
                    updateDrawObjectList();
#endif
                }
                AppManager.editorConfig.MouseHoverTime = m_preference_dlg.getMouseHoverTime();
                AppManager.editorConfig.PlayPreviewWhenRightClick = m_preference_dlg.isPlayPreviewWhenRightClick();
                AppManager.editorConfig.CurveSelectingQuantized = m_preference_dlg.isCurveSelectingQuantized();

                AppManager.editorConfig.CurveVisibleAccent = m_preference_dlg.isCurveVisibleAccent();
                AppManager.editorConfig.CurveVisibleBreathiness = m_preference_dlg.isCurveVisibleBre();
                AppManager.editorConfig.CurveVisibleBrightness = m_preference_dlg.isCurveVisibleBri();
                AppManager.editorConfig.CurveVisibleClearness = m_preference_dlg.isCurveVisibleCle();
                AppManager.editorConfig.CurveVisibleDecay = m_preference_dlg.isCurveVisibleDecay();
                AppManager.editorConfig.CurveVisibleDynamics = m_preference_dlg.isCurveVisibleDyn();
                AppManager.editorConfig.CurveVisibleGendorfactor = m_preference_dlg.isCurveVisibleGen();
                AppManager.editorConfig.CurveVisibleOpening = m_preference_dlg.isCurveVisibleOpe();
                AppManager.editorConfig.CurveVisiblePit = m_preference_dlg.isCurveVisiblePit();
                AppManager.editorConfig.CurveVisiblePbs = m_preference_dlg.isCurveVisiblePbs();
                AppManager.editorConfig.CurveVisiblePortamento = m_preference_dlg.isCurveVisiblePor();
                AppManager.editorConfig.CurveVisibleVelocity = m_preference_dlg.isCurveVisibleVel();
                AppManager.editorConfig.CurveVisibleVibratoDepth = m_preference_dlg.isCurveVisibleVibratoDepth();
                AppManager.editorConfig.CurveVisibleVibratoRate = m_preference_dlg.isCurveVisibleVibratoRate();
                AppManager.editorConfig.CurveVisibleFx2Depth = m_preference_dlg.isCurveVisibleFx2Depth();
                AppManager.editorConfig.CurveVisibleHarmonics = m_preference_dlg.isCurveVisibleHarmonics();
                AppManager.editorConfig.CurveVisibleReso1 = m_preference_dlg.isCurveVisibleReso1();
                AppManager.editorConfig.CurveVisibleReso2 = m_preference_dlg.isCurveVisibleReso2();
                AppManager.editorConfig.CurveVisibleReso3 = m_preference_dlg.isCurveVisibleReso3();
                AppManager.editorConfig.CurveVisibleReso4 = m_preference_dlg.isCurveVisibleReso4();
                AppManager.editorConfig.CurveVisibleEnvelope = m_preference_dlg.isCurveVisibleEnvelope();

#if ENABLE_MIDI
                AppManager.editorConfig.MidiInPort.PortNumber = m_preference_dlg.getMidiInPort();
                updateMidiInStatus();
                reloadMidiIn();
#endif

                AppManager.editorConfig.InvokeUtauCoreWithWine = m_preference_dlg.isInvokeWithWine();
                AppManager.editorConfig.PathResampler = m_preference_dlg.getPathResampler();
                AppManager.editorConfig.PathWavtool = m_preference_dlg.getPathWavtool();
                AppManager.editorConfig.UtauSingers.clear();
                for ( Iterator itr = m_preference_dlg.getUtauSingers().iterator(); itr.hasNext(); ) {
                    SingerConfig sc = (SingerConfig)itr.next();
                    AppManager.editorConfig.UtauSingers.add( (SingerConfig)sc.clone() );
                }
                AppManager.editorConfig.SelfDeRomanization = m_preference_dlg.isSelfDeRomantization();
                AppManager.editorConfig.AutoBackupIntervalMinutes = m_preference_dlg.getAutoBackupIntervalMinutes();
                AppManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier = m_preference_dlg.isUseSpaceKeyAsMiddleButtonModifier();

                Vector<CurveType> visible_curves = new Vector<CurveType>();
                trackSelector.clearViewingCurve();
                trackSelector.prepareSingerMenu( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version );

                updateTrackSelectorVisibleCurve();
                updateRendererMenu();
                AppManager.updateAutoBackupTimerStatus();

                if ( menuVisualControlTrack.isSelected() ) {
                    splitContainer1.Panel2MinSize = trackSelector.getPreferredMinSize();
                }

                AppManager.saveConfig();
                applyLanguage();
#if ENABLE_SCRIPT
                updateScriptShortcut();
#endif

#if USE_DOBJ
                updateDrawObjectList();
#endif
                refreshScreen();
            }
        }

        private void menuSettingShortcut_Click( Object sender, BEventArgs e ) {
            TreeMap<String, ValuePair<String, BKeys[]>> dict = new TreeMap<String, ValuePair<String, BKeys[]>>();
            TreeMap<String, BKeys[]> configured = AppManager.editorConfig.getShortcutKeysDictionary();

            // スクリプトのToolStripMenuITemを蒐集
            Vector<String> script_shortcut = new Vector<String>();
            MenuElement[] sub_menu_script = menuScript.getSubElements();
            for ( int i = 0; i < sub_menu_script.Length; i++ ) {
                MenuElement tsi = sub_menu_script[i];
                if ( tsi is BMenuItem ) {
                    BMenuItem tsmi = (BMenuItem)tsi;
                    MenuElement[] sub_tsmi = tsmi.getSubElements();
                    if ( sub_tsmi.Length == 1 && sub_tsmi[0] is BMenuItem ) {
                        BMenuItem item = (BMenuItem)sub_tsmi[0];
                        script_shortcut.add( item.getName() );
                        if ( !configured.containsKey( item.getName() ) ) {
                            configured.put( item.getName(), new BKeys[] { } );
                        }
                    }
                }
            }

            for ( Iterator itr = configured.keySet().iterator(); itr.hasNext(); ) {
                String name = (String)itr.next();
                MenuElement menu = searchMenuItemFromName( name );
                if ( menu != null ) {
                    String parent = "";
                    MenuElement owner_item = null;
                    Object pa = menu.getParent();
                    if ( pa != null && pa is MenuElement ) {
                        owner_item = (MenuElement)pa;
                    }
                    if ( owner_item != null && !owner_item.getName().Equals( menuHidden.getName() ) ) {
                        String s = owner_item.getText();
                        int i = s.IndexOf( "(&" );
                        if ( i > 0 ) {
                            s = s.Substring( 0, i );
                        }
                        parent = s + " -> ";
                    }
                    String s1 = menu.getText();
                    int i1 = s1.IndexOf( "(&" );
                    if ( i1 > 0 ) {
                        s1 = s1.Substring( 0, i1 );
                    }
                    if ( script_shortcut.contains( name ) ) {
                        String s2 = menuScript.getText();
                        int i2 = s2.IndexOf( "(&" );
                        if ( i2 > 0 ) {
                            s2 = s2.Substring( 0, i2 );
                        }
                        parent = s2 + " -> " + parent;
                    }
                    dict.put( parent + s1, new ValuePair<String, BKeys[]>( name, configured.get( name ) ) );
                }
            }

            FormShortcutKeys form = null;
            try {
                form = new FormShortcutKeys( dict );
                form.setLocation( getFormPreferedLocation( form ) );
                if ( form.showDialog() == BDialogResult.OK ) {
                    TreeMap<String, ValuePair<String, BKeys[]>> res = form.getResult();
                    for ( Iterator itr = res.keySet().iterator(); itr.hasNext(); ) {
                        String display = (String)itr.next();
                        String name = res.get( display ).getKey();
                        BKeys[] keys = res.get( display ).getValue();
                        boolean found = false;
                        for ( int i = 0; i < AppManager.editorConfig.ShortcutKeys.size(); i++ ) {
                            if ( AppManager.editorConfig.ShortcutKeys.get( i ).Key.Equals( name ) ) {
                                AppManager.editorConfig.ShortcutKeys.get( i ).Value = keys;
                                found = true;
                                break;
                            }
                        }
                        if ( !found ) {
                            AppManager.editorConfig.ShortcutKeys.add( new ValuePairOfStringArrayOfKeys( name, keys ) );
                        }
                    }
                    applyShortcut();
#if ENABLE_PROPERTY
                    AppManager.propertyWindow.setFormCloseShortcutKey( AppManager.editorConfig.getShortcutKeyFor( menuVisualProperty ) );
#endif
                }
            } catch ( Exception ex ) {
            } finally {
                if ( form != null ) {
                    try {
                        form.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        private void menuSettingGameControlerLoad_Click( Object sender, BEventArgs e ) {
            loadGameControler();
        }

        private void menuSettingGameControlerRemove_Click( Object sender, BEventArgs e ) {
            removeGameControler();
        }

        private void menuSettingGameControlerSetting_Click( Object sender, BEventArgs e ) {
            FormGameControlerConfig dlg = null;
            try {
                dlg = new FormGameControlerConfig();
                dlg.setLocation( getFormPreferedLocation( dlg ) );
                if ( dlg.showDialog() == BDialogResult.OK ) {
                    AppManager.editorConfig.GameControlerRectangle = dlg.getRectangle();
                    AppManager.editorConfig.GameControlerTriangle = dlg.getTriangle();
                    AppManager.editorConfig.GameControlerCircle = dlg.getCircle();
                    AppManager.editorConfig.GameControlerCross = dlg.getCross();
                    AppManager.editorConfig.GameControlL1 = dlg.getL1();
                    AppManager.editorConfig.GameControlL2 = dlg.getL2();
                    AppManager.editorConfig.GameControlR1 = dlg.getR1();
                    AppManager.editorConfig.GameControlR2 = dlg.getR2();
                    AppManager.editorConfig.GameControlSelect = dlg.getSelect();
                    AppManager.editorConfig.GameControlStart = dlg.getStart();
                    AppManager.editorConfig.GameControlPovDown = dlg.getPovDown();
                    AppManager.editorConfig.GameControlPovLeft = dlg.getPovLeft();
                    AppManager.editorConfig.GameControlPovUp = dlg.getPovUp();
                    AppManager.editorConfig.GameControlPovRight = dlg.getPovRight();
                }
            } catch ( Exception ex ) {
            } finally {
                if ( dlg != null ) {
                    try {
                        dlg.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }
        #endregion

        #region menuEdit*
        private void menuEditDelete_Click( Object sender, BEventArgs e ) {
            deleteEvent();
        }

        private void commonEditPaste_Click( Object sender, BEventArgs e ) {
            pasteEvent();
        }

        private void commonEditCopy_Click( Object sender, BEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "commonEditCopy_Click" );
#endif
            copyEvent();
        }

        private void commonEditCut_Click( Object sender, BEventArgs e ) {
            cutEvent();
        }

        private void menuEdit_DropDownOpening( Object sender, BEventArgs e ) {
            updateCopyAndPasteButtonStatus();
        }

        private void commonEditUndo_Click( Object sender, BEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "menuEditUndo_Click" );
#endif
            undo();
            refreshScreen();
        }


        private void commonEditRedo_Click( Object sender, BEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "menuEditRedo_Click" );
#endif
            redo();
            refreshScreen();
        }

        private void menuEditSelectAllEvents_Click( Object sender, BEventArgs e ) {
            selectAllEvent();
        }

        private void menuEditSelectAll_Click( Object sender, BEventArgs e ) {
            selectAll();
        }

        private void menuEditAutoNormalizeMode_Click( Object sender, BEventArgs e ) {
            AppManager.autoNormalize = !AppManager.autoNormalize;
            menuEditAutoNormalizeMode.setSelected( AppManager.autoNormalize );
        }

        private void menuEditUndo_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Undo." ) );
        }

        private void menuEditRedo_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Redo." ) );
        }

        private void menuEditCut_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Cut selected items." ) );
        }

        private void menuEditCopy_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Copy selected items." ) );
        }

        private void menuEditPaste_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Paste copied items to current song position." ) );
        }

        private void menuEditDelete_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Delete selected items." ) );
        }

        private void menuEditAutoNormalizeMode_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Avoid automaticaly polyphonic editing." ) );
        }

        private void menuEditSelectAll_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Select all items and control curves of current track." ) );
        }

        private void menuEditSelectAllEvents_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Select all items of current track." ) );
        }
        #endregion

        #region menuLyric*
        private void menuLyricExpressionProperty_Click( Object sender, BEventArgs e ) {
            editNoteExpressionProperty();
        }

        private void menuLyricDictionary_Click( Object sender, BEventArgs e ) {
            FormWordDictionary dlg = null;
            try {
                dlg = new FormWordDictionary();
                dlg.setLocation( getFormPreferedLocation( dlg ) );
                if ( dlg.showDialog() == BDialogResult.OK ) {
                    Vector<ValuePair<String, Boolean>> result = dlg.getResult();
                    SymbolTable.changeOrder( result );
                }
            } catch ( Exception ex ) {
            } finally {
                if ( dlg != null ) {
                    try {
                        dlg.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        private void menuLyricVibratoProperty_Click( Object sender, BEventArgs e ) {
            editNoteVibratoProperty();
        }
        #endregion

        #region menuJob
        private void menuJobRealTime_Click( Object sender, BEventArgs e ) {
            if ( !AppManager.isPlaying() ) {
                AppManager.addingEvent = null;
                AppManager.setEditMode( EditMode.REALTIME );
                AppManager.setPlaying( true );
                //menuJobRealTime.setText( _( "Stop Realtime Input" );
            } else {
                timer.stop();
                AppManager.setPlaying( false );
                AppManager.setEditMode( EditMode.NONE );
                //menuJobRealTime.setText( _( "Start Realtime Input" );
            }
        }

        private void menuJobReloadVsti_Click( Object sender, BEventArgs e ) {
            //VSTiProxy.ReloadPlugin(); //todo: FormMain+menuJobReloadVsti_Click
        }

        private void menuJob_DropDownOpening( Object sender, BEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "menuJob_DropDownOpening" );
            AppManager.debugWriteLine( "    menuJob.Bounds=" + menuJob.Bounds );
#endif
            if ( AppManager.getSelectedEventCount() <= 1 ) {
                menuJobConnect.setEnabled( false );
            } else {
                // menuJobConnect(音符の結合)がEnableされるかどうかは、選択されている音符がピアノロール上で連続かどうかで決まる
                int[] list = new int[AppManager.getSelectedEventCount()];
                for ( int i = 0; i < AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventCount(); i++ ) {
                    int count = -1;
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                        SelectedEventEntry item = (SelectedEventEntry)itr.next();
                        int key = item.original.InternalID;
                        count++;
                        if ( key == AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEvent( i ).InternalID ) {
                            list[count] = i;
                            break;
                        }
                    }
                }
                boolean changed = true;
                while ( changed ) {
                    changed = false;
                    for ( int i = 0; i < list.Length - 1; i++ ) {
                        if ( list[i] > list[i + 1] ) {
                            int b = list[i];
                            list[i] = list[i + 1];
                            list[i + 1] = b;
                            changed = true;
                        }
                    }
                }
                boolean continued = true;
                for ( int i = 0; i < list.Length - 1; i++ ) {
                    if ( list[i] + 1 != list[i + 1] ) {
                        continued = false;
                        break;
                    }
                }
                menuJobConnect.setEnabled( continued );
            }

            menuJobLyric.setEnabled( AppManager.getLastSelectedEvent() != null );
        }

        private void menuJobLyric_Click( Object sender, BEventArgs e ) {
            importLyric();
        }

        private void menuJobConnect_Click( Object sender, BEventArgs e ) {
            int count = AppManager.getSelectedEventCount();
            int[] clocks = new int[count];
            VsqID[] ids = new VsqID[count];
            int[] internalids = new int[count];
            int i = -1;
            for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                SelectedEventEntry item = (SelectedEventEntry)itr.next();
                i++;
                clocks[i] = item.original.Clock;
                ids[i] = (VsqID)item.original.ID.clone();
                internalids[i] = item.original.InternalID;
            }
            boolean changed = true;
            while ( changed ) {
                changed = false;
                for ( int j = 0; j < clocks.Length - 1; j++ ) {
                    if ( clocks[j] > clocks[j + 1] ) {
                        int b = clocks[j];
                        clocks[j] = clocks[j + 1];
                        clocks[j + 1] = b;
                        VsqID a = ids[j];
                        ids[j] = ids[j + 1];
                        ids[j + 1] = a;
                        changed = true;
                        b = internalids[j];
                        internalids[j] = internalids[j + 1];
                        internalids[j + 1] = b;
                    }
                }
            }

            for ( int j = 0; j < ids.Length - 1; j++ ) {
                ids[j].Length = clocks[j + 1] - clocks[j];
            }
            CadenciiCommand run = new CadenciiCommand(
                VsqCommand.generateCommandEventChangeIDContaintsRange( AppManager.getSelected(), internalids, ids ) );
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
            setEdited( true );
            repaint();
        }

        private void menuJobInsertBar_Click( Object sender, BEventArgs e ) {
            int total_clock = AppManager.getVsqFile().TotalClocks;
            int total_barcount = AppManager.getVsqFile().getBarCountFromClock( total_clock ) + 1;
            FormInsertBar dlg = null;
            try {
                dlg = new FormInsertBar( total_barcount );
                int current_clock = AppManager.getCurrentClock();
                int barcount = AppManager.getVsqFile().getBarCountFromClock( current_clock );
                int draft = barcount - AppManager.getVsqFile().getPreMeasure() + 1;
                if ( draft <= 0 ) {
                    draft = 1;
                }
                dlg.setPosition( draft );

                dlg.setLocation( getFormPreferedLocation( dlg ) );
                if ( dlg.showDialog() == BDialogResult.OK ) {
                    int pos = dlg.getPosition() + AppManager.getVsqFile().getPreMeasure() - 1;
                    int length = dlg.getLength();

                    int clock_start = AppManager.getVsqFile().getClockFromBarCount( pos );
                    int clock_end = AppManager.getVsqFile().getClockFromBarCount( pos + length );
                    int dclock = clock_end - clock_start;
                    VsqFileEx temp = (VsqFileEx)AppManager.getVsqFile().Clone();

                    for ( int track = 1; track < temp.Track.size(); track++ ) {
                        BezierCurves newbc = new BezierCurves();
                        foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                            int index = ct.getIndex();
                            if ( index < 0 ) {
                                continue;
                            }

                            Vector<BezierChain> list = new Vector<BezierChain>();
                            for ( Iterator itr = temp.AttachedCurves.get( track - 1 ).get( ct ).iterator(); itr.hasNext(); ) {
                                BezierChain bc = (BezierChain)itr.next();
                                if ( bc.size() < 2 ) {
                                    continue;
                                }
                                int chain_start = (int)bc.points.get( 0 ).getBase().getX();
                                int chain_end = (int)bc.points.get( bc.points.size() - 1 ).getBase().getX();

                                if ( clock_start <= chain_start ) {
                                    for ( int i = 0; i < bc.points.size(); i++ ) {
                                        PointD t = bc.points.get( i ).getBase();
                                        bc.points.get( i ).setBase( new PointD( t.getX() + dclock, t.getY() ) );
                                    }
                                    list.add( bc );
                                } else if ( chain_start < clock_start && clock_start < chain_end ) {
                                    BezierChain adding1 = bc.extractPartialBezier( chain_start, clock_start );
                                    BezierChain adding2 = bc.extractPartialBezier( clock_start, chain_end );
                                    for ( int i = 0; i < adding2.points.size(); i++ ) {
                                        PointD t = adding2.points.get( i ).getBase();
                                        adding2.points.get( i ).setBase( new PointD( t.getX() + dclock, t.getY() ) );
                                    }
                                    //PointD t2 = adding1.points[adding1.points.Count - 1].Base;
                                    adding1.points.get( adding1.points.size() - 1 ).setControlRightType( BezierControlType.None );
                                    /*BezierPoint bp = new BezierPoint( t2.X + dclock, t2.Y );
                                    bp.ControlLeftType = BezierControlType.None;
                                    bp.ControlRightType = BezierControlType.None;
                                    adding1.points.Add( bp );*/
                                    adding2.points.get( 0 ).setControlLeftType( BezierControlType.None );
                                    for ( int i = 0; i < adding2.points.size(); i++ ) {
                                        adding1.points.add( adding2.points.get( i ) );
                                    }
                                    adding1.id = bc.id;
                                    list.add( adding1 );
                                } else {
                                    list.add( (BezierChain)bc.Clone() );
                                }
                            }

                            newbc.set( ct, list );
                        }
                        temp.AttachedCurves.set( track - 1, newbc );
                    }

                    for ( int track = 1; track < AppManager.getVsqFile().Track.size(); track++ ) {
                        for ( int i = 0; i < temp.Track.get( track ).getEventCount(); i++ ) {
                            if ( temp.Track.get( track ).getEvent( i ).Clock >= clock_start ) {
                                temp.Track.get( track ).getEvent( i ).Clock += dclock;
                            }
                        }
                        foreach ( CurveType curve in AppManager.CURVE_USAGE ) {
                            if ( curve.isScalar() || curve.isAttachNote() ) {
                                continue;
                            }
                            VsqBPList target = temp.Track.get( track ).getCurve( curve.getName() );
                            VsqBPList src = AppManager.getVsqFile().Track.get( track ).getCurve( curve.getName() );
                            target.clear();
                            for ( Iterator itr = src.keyClockIterator(); itr.hasNext(); ) {
                                int key = (int)itr.next();
                                if ( key >= clock_start ) {
                                    target.add( key + dclock, src.getValue( key ) );
                                } else {
                                    target.add( key, src.getValue( key ) );
                                }
                            }
                        }
                    }
                    for ( int i = 0; i < temp.TempoTable.size(); i++ ) {
                        if ( temp.TempoTable.get( i ).Clock >= clock_start ) {
                            temp.TempoTable.get( i ).Clock = temp.TempoTable.get( i ).Clock + dclock;
                        }
                    }
                    for ( int i = 0; i < temp.TimesigTable.size(); i++ ) {
                        if ( temp.TimesigTable.get( i ).Clock >= clock_start ) {
                            temp.TimesigTable.get( i ).Clock = temp.TimesigTable.get( i ).Clock + dclock;
                        }
                    }
                    temp.updateTempoInfo();
                    temp.updateTimesigInfo();

                    CadenciiCommand run = VsqFileEx.generateCommandReplace( temp );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    setEdited( true );
                    repaint();
                }
            } catch ( Exception ex ) {
            } finally {
                if ( dlg != null ) {
                    try {
                        dlg.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        private void menuJobDeleteBar_Click( Object sender, BEventArgs e ) {
            int total_clock = AppManager.getVsqFile().TotalClocks;
            int total_barcount = AppManager.getVsqFile().getBarCountFromClock( total_clock ) + 1;
            int clock = AppManager.getCurrentClock();
            int barcount = AppManager.getVsqFile().getBarCountFromClock( clock );
            FormDeleteBar dlg = null;
            try {
                dlg = new FormDeleteBar( total_barcount );
                int draft = barcount - AppManager.getVsqFile().getPreMeasure() + 1;
                if ( draft <= 0 ) {
                    draft = 1;
                }
                dlg.setStart( draft );
                dlg.setEnd( draft + 1 );

                dlg.setLocation( getFormPreferedLocation( dlg ) );
                if ( dlg.showDialog() == BDialogResult.OK ) {
                    VsqFileEx temp = (VsqFileEx)AppManager.getVsqFile().Clone();
                    int start = dlg.getStart() + AppManager.getVsqFile().getPreMeasure() - 1;
                    int end = dlg.getEnd() + AppManager.getVsqFile().getPreMeasure() - 1;
#if DEBUG
                    AppManager.debugWriteLine( "FormMain+menuJobDeleteBar_Click" );
                    AppManager.debugWriteLine( "    start,end=" + start + "," + end );
#endif
                    int clock_start = temp.getClockFromBarCount( start );
                    int clock_end = temp.getClockFromBarCount( end );
                    int dclock = clock_end - clock_start;
                    for ( int track = 1; track < temp.Track.size(); track++ ) {
                        BezierCurves newbc = new BezierCurves();
                        for ( int j = 0; j < AppManager.CURVE_USAGE.Length; j++ ) {
                            CurveType ct = AppManager.CURVE_USAGE[j];
                            int index = ct.getIndex();
                            if ( index < 0 ) {
                                continue;
                            }

                            Vector<BezierChain> list = new Vector<BezierChain>();
                            for ( Iterator itr = temp.AttachedCurves.get( track - 1 ).get( ct ).iterator(); itr.hasNext(); ) {
                                BezierChain bc = (BezierChain)itr.next();
                                if ( bc.size() < 2 ) {
                                    continue;
                                }
                                int chain_start = (int)bc.points.get( 0 ).getBase().getX();
                                int chain_end = (int)bc.points.get( bc.points.size() - 1 ).getBase().getX();

                                if ( clock_start < chain_start && chain_start < clock_end && clock_end < chain_end ) {
                                    BezierChain adding = bc.extractPartialBezier( clock_end, chain_end );
                                    adding.id = bc.id;
                                    for ( int i = 0; i < adding.points.size(); i++ ) {
                                        PointD t = adding.points.get( i ).getBase();
                                        adding.points.get( i ).setBase( new PointD( t.getX() - dclock, t.getY() ) );
                                    }
                                    list.add( adding );
                                } else if ( chain_start < clock_start && clock_end < chain_end ) {
                                    BezierChain adding1 = bc.extractPartialBezier( chain_start, clock_start );
                                    adding1.id = bc.id;
                                    adding1.points.get( adding1.points.size() - 1 ).setControlRightType( BezierControlType.None );
                                    BezierChain adding2 = bc.extractPartialBezier( clock_end, chain_end );
                                    adding2.points.get( 0 ).setControlLeftType( BezierControlType.None );
                                    PointD t = adding2.points.get( 0 ).getBase();
                                    adding2.points.get( 0 ).setBase( new PointD( t.getX() - dclock, t.getY() ) );
                                    adding1.points.add( adding2.points.get( 0 ) );
                                    for ( int i = 1; i < adding2.points.size(); i++ ) {
                                        t = adding2.points.get( i ).getBase();
                                        adding2.points.get( i ).setBase( new PointD( t.getX() - dclock, t.getY() ) );
                                        adding1.points.add( adding2.points.get( i ) );
                                    }
                                    list.add( adding1 );
                                } else if ( chain_start < clock_start && clock_start < chain_end && chain_end < clock_end ) {
                                    BezierChain adding = bc.extractPartialBezier( chain_start, clock_start );
                                    adding.id = bc.id;
                                    list.add( adding );
                                } else if ( clock_end <= chain_start || chain_end <= clock_start ) {
                                    if ( clock_end <= chain_start ) {
                                        for ( int i = 0; i < bc.points.size(); i++ ) {
                                            PointD t = bc.points.get( i ).getBase();
                                            bc.points.get( i ).setBase( new PointD( t.getX() - dclock, t.getY() ) );
                                        }
                                    }
                                    list.add( (BezierChain)bc.Clone() );
                                }
                            }

                            newbc.set( ct, list );
                        }
                        temp.AttachedCurves.set( track - 1, newbc );
                    }

                    temp.removePart( clock_start, clock_end );
                    CadenciiCommand run = VsqFileEx.generateCommandReplace( temp );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    setEdited( true );
                    repaint();
                }
            } catch ( Exception ex ) {
            } finally {
                if ( dlg != null ) {
                    try {
                        dlg.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        private void menuJobNormalize_Click( Object sender, BEventArgs e ) {
            VsqFile work = (VsqFile)AppManager.getVsqFile().Clone();
            int track = AppManager.getSelected();
            boolean changed = true;
            boolean total_changed = false;

            // 最初、開始時刻が同じになっている奴を削除
            while ( changed ) {
                changed = false;
                for ( int i = 0; i < work.Track.get( track ).getEventCount() - 1; i++ ) {
                    int clock = work.Track.get( track ).getEvent( i ).Clock;
                    int id = work.Track.get( track ).getEvent( i ).InternalID;
                    for ( int j = i + 1; j < work.Track.get( track ).getEventCount(); j++ ) {
                        if ( clock == work.Track.get( track ).getEvent( j ).Clock ) {
                            if ( id < work.Track.get( track ).getEvent( j ).InternalID ) { //内部IDが小さい＝より高年齢（音符追加時刻が古い）
                                work.Track.get( track ).removeEvent( i );
                            } else {
                                work.Track.get( track ).removeEvent( j );
                            }
                            changed = true;
                            total_changed = true;
                            break;
                        }
                    }
                    if ( changed ) {
                        break;
                    }
                }
            }

            changed = true;
            while ( changed ) {
                changed = false;
                for ( int i = 0; i < work.Track.get( track ).getEventCount() - 1; i++ ) {
                    int start_clock = work.Track.get( track ).getEvent( i ).Clock;
                    int end_clock = work.Track.get( track ).getEvent( i ).ID.getLength() + start_clock;
                    for ( int j = i + 1; j < work.Track.get( track ).getEventCount(); j++ ) {
                        int this_start_clock = work.Track.get( track ).getEvent( j ).Clock;
                        if ( this_start_clock < end_clock ) {
                            work.Track.get( track ).getEvent( i ).ID.setLength( this_start_clock - start_clock );
                            changed = true;
                            total_changed = true;
                        }
                    }
                }
            }

            if ( total_changed ) {
                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandReplace( work ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                setEdited( true );
                refreshScreen();
            }
        }

        private void menuJobNormalize_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Correct overlapped item." ) );
        }

        private void menuJobInsertBar_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Insert bar." ) );
        }

        private void menuJobDeleteBar_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Delete bar." ) );
        }

        private void menuJobRandomize_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Randomize items." ) + _( "(not implemented)" ) );
        }

        private void menuJobConnect_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Lengthen note end to neighboring note." ) );
        }

        private void menuJobLyric_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Import lyric." ) );
        }

        private void menuJobRewire_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Import tempo from ReWire host." ) + _( "(not implemented)" ) );
        }

        private void menuJobRealTime_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Start realtime input." ) );
        }

        private void menuJobReloadVsti_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Reload VSTi dll." ) + _( "(not implemented)" ) );
        }
        #endregion

        #region menuScript
        private void menuScriptUpdate_Click( Object sender, BEventArgs e ) {
#if ENABLE_SCRIPT
            updateScriptShortcut();
            applyShortcut();
#endif
        }
        #endregion

        #region vScroll
        private void vScroll_Enter( Object sender, BEventArgs e ) {
            pictPianoRoll.Select();
        }

        private void vScroll_Resize( Object sender, BEventArgs e ) {
            setVScrollRange( vScroll.getMaximum() );
        }

        private void vScroll_ValueChanged( Object sender, BEventArgs e ) {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
            refreshScreen();
        }
        #endregion

        #region hScroll
        private void hScroll_Enter( Object sender, BEventArgs e ) {
            pictPianoRoll.Select();
        }

        private void hScroll_Resize( Object sender, BEventArgs e ) {
#if DEBUG
            PortUtil.println( "FormMain#hScroll_Resize" );
#endif
            if ( getExtendedState() != BForm.ICONIFIED ) {
                setHScrollRange( AppManager.getVsqFile().TotalClocks );
            }
        }

        private void hScroll_ValueChanged( Object sender, BEventArgs e ) {
#if DEBUG
            //PortUtil.println( "hScroll_ValueChanged" );
            //PortUtil.println( "    Value/Maximum=" + hScroll.getValue() + "/" + hScroll.getMaximum() );
            //PortUtil.println( "    LargeChange=" + hScroll.LargeChange );
#endif
            AppManager.startToDrawX = (int)(hScroll.getValue() * AppManager.scaleX);
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
            refreshScreen();
        }
        #endregion

        #region picturePositionIndicator
        private void picturePositionIndicator_MouseWheel( Object sender, BMouseEventArgs e ) {
            hScroll.setValue( computeScrollValueFromWheelDelta( e.Delta ) );
        }

        private void picturePositionIndicator_MouseDoubleClick( Object sender, BMouseEventArgs e ) {
            if ( e.X < AppManager.keyWidth || Width - 3 < e.X ) {
                return;
            }
            if ( e.Button == BMouseButtons.Left ) {
                if ( 18 < e.Y && e.Y <= 32 ) {
                    #region テンポの変更
                    int index = -1;
                    for ( int i = 0; i < AppManager.getVsqFile().TempoTable.size(); i++ ) {
                        int clock = AppManager.getVsqFile().TempoTable.get( i ).Clock;
                        int x = AppManager.xCoordFromClocks( clock );
                        if ( x < 0 ) {
                            continue;
                        } else if ( Width < x ) {
                            break;
                        }
                        String s = (60e6 / (float)AppManager.getVsqFile().TempoTable.get( i ).Tempo).ToString( "#.00" );
                        Dimension size = Util.measureString( s, new Font( AppManager.editorConfig.ScreenFontName, java.awt.Font.PLAIN, 8 ) );
                        if ( isInRect( new Point( e.X, e.Y ), new Rectangle( x, 14, (int)size.width, 14 ) ) ) {
                            index = i;
                            break;
                        }
                    }

                    if ( index < 0 ) {
                        return;
                    }

                    TempoTableEntry tte = AppManager.getVsqFile().TempoTable.get( index );
                    AppManager.clearSelectedTempo();
                    AppManager.addSelectedTempo( tte.Clock );
                    int bar_count = AppManager.getVsqFile().getBarCountFromClock( tte.Clock );
                    int bar_top_clock = AppManager.getVsqFile().getClockFromBarCount( bar_count );
                    //int local_denominator, local_numerator;
                    Timesig timesig = AppManager.getVsqFile().getTimesigAt( tte.Clock );
                    int clock_per_beat = 480 * 4 / timesig.denominator;
                    int clocks_in_bar = tte.Clock - bar_top_clock;
                    int beat_in_bar = clocks_in_bar / clock_per_beat + 1;
                    int clocks_in_beat = clocks_in_bar - (beat_in_bar - 1) * clock_per_beat;
                    FormTempoConfig dlg = null;
                    try {
                        dlg = new FormTempoConfig( bar_count, beat_in_bar, timesig.numerator, clocks_in_beat, clock_per_beat, (float)(6e7 / tte.Tempo), AppManager.getVsqFile().getPreMeasure() );
                        dlg.setLocation( getFormPreferedLocation( dlg ) );
                        if ( dlg.showDialog() == BDialogResult.OK ) {
                            int new_beat = dlg.getBeatCount();
                            int new_clocks_in_beat = dlg.getClock();
                            int new_clock = bar_top_clock + (new_beat - 1) * clock_per_beat + new_clocks_in_beat;
                            CadenciiCommand run = new CadenciiCommand(
                                VsqCommand.generateCommandUpdateTempo( new_clock, new_clock, (int)(6e7 / (double)dlg.getTempo()) ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                            setEdited( true );
                            refreshScreen();
                        }
                    } catch ( Exception ex ) {
                    } finally {
                        if ( dlg != null ) {
                            try {
                                dlg.close();
                            } catch ( Exception ex2 ) {
                            }
                        }
                    }

                    #endregion
                } else if ( 32 < e.Y && e.Y <= picturePositionIndicator.getHeight() - 1 ) {
                    #region 拍子の変更
                    int index = -1;
                    // クリック位置に拍子が表示されているかどうか検査
                    for ( int i = 0; i < AppManager.getVsqFile().TimesigTable.size(); i++ ) {
                        String s = AppManager.getVsqFile().TimesigTable.get( i ).Numerator + "/" + AppManager.getVsqFile().TimesigTable.get( i ).Denominator;
                        int x = AppManager.xCoordFromClocks( AppManager.getVsqFile().TimesigTable.get( i ).Clock );
                        Dimension size = Util.measureString( s, new Font( AppManager.editorConfig.ScreenFontName, java.awt.Font.PLAIN, 8 ) );
                        if ( isInRect( new Point( e.X, e.Y ), new Rectangle( x, 28, (int)size.width, 14 ) ) ) {
                            index = i;
                            break;
                        }
                    }

                    if ( index < 0 ) {
                        return;
                    }

                    int pre_measure = AppManager.getVsqFile().getPreMeasure();
                    int clock = AppManager.clockFromXCoord( e.X );
                    int bar_count = AppManager.getVsqFile().getBarCountFromClock( clock );
                    //int numerator, denominator;
                    int total_clock = AppManager.getVsqFile().TotalClocks;
                    //int max_barcount = AppManager.VsqFile.getBarCountFromClock( total_clock );
                    //int min_barcount = 1 - pre_measure;
                    Timesig timesig = AppManager.getVsqFile().getTimesigAt( clock );
                    boolean num_enabled = !(bar_count == 0);
                    FormBeatConfig dlg = null;
                    try {
                        dlg = new FormBeatConfig( bar_count - pre_measure + 1, timesig.numerator, timesig.denominator, num_enabled, pre_measure );
                        dlg.setLocation( getFormPreferedLocation( dlg ) );
                        if ( dlg.showDialog() == BDialogResult.OK ) {
                            if ( dlg.isEndSpecified() ) {
                                int[] new_barcounts = new int[2];
                                int[] numerators = new int[2];
                                int[] denominators = new int[2];
                                int[] barcounts = new int[2];
                                new_barcounts[0] = dlg.getStart() + pre_measure - 1;
                                new_barcounts[1] = dlg.getEnd() + pre_measure - 1;
                                numerators[0] = dlg.getNumerator();
                                denominators[0] = dlg.getDenominator();
                                numerators[1] = timesig.numerator;
                                denominators[1] = timesig.denominator;
                                barcounts[0] = bar_count;
                                barcounts[1] = dlg.getEnd() + pre_measure - 1;
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandUpdateTimesigRange( barcounts, new_barcounts, numerators, denominators ) );
                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                setEdited( true );
                            } else {
#if DEBUG
                                PortUtil.println( "picturePositionIndicator_MouseDoubleClick" );
                                PortUtil.println( "    bar_count=" + bar_count );
                                PortUtil.println( "    dlg.Start+pre_measure-1=" + (dlg.getStart() + pre_measure - 1) );
#endif
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandUpdateTimesig( bar_count, dlg.getStart() + pre_measure - 1, dlg.getNumerator(), dlg.getDenominator() ) );
                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                setEdited( true );
                            }
                        }
                    } catch ( Exception ex ) {
                    } finally {
                        if ( dlg != null ) {
                            try {
                                dlg.close();
                            } catch ( Exception ex2 ) {
                            }
                        }
                    }
                    #endregion
                }
                picturePositionIndicator.repaint();
                pictPianoRoll.repaint();
            }
        }

        private void picturePositionIndicator_MouseDown( Object sender, BMouseEventArgs e ) {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }

            if ( e.X < AppManager.keyWidth || Width - 3 < e.X ) {
                return;
            }

            m_startmark_dragging = false;
            m_endmark_dragging = false;
            int modifiers = PortUtil.getCurrentModifierKey();
            if ( e.Button == BMouseButtons.Left ) {
                if ( 0 <= e.Y && e.Y <= 18 ) {
                    #region スタート/エンドマーク
                    if ( AppManager.startMarkerEnabled ) {
                        int startx = AppManager.xCoordFromClocks( AppManager.startMarker ) - AppManager.editorConfig.PxTolerance;
                        int marker_width = Resources.get_start_marker().getWidth( this );
                        if ( startx <= e.X && e.X <= startx + AppManager.editorConfig.PxTolerance * 2 + marker_width ) {
                            m_startmark_dragging = true;
                        }
                    }
                    if ( AppManager.endMarkerEnabled && !m_startmark_dragging ) {
                        int marker_width = Resources.get_end_marker().getWidth( this );
                        int endx = AppManager.xCoordFromClocks( AppManager.endMarker ) - marker_width - AppManager.editorConfig.PxTolerance;
                        if ( endx <= e.X && e.X <= endx + AppManager.editorConfig.PxTolerance * 2 + marker_width ) {
                            m_endmark_dragging = true;
                        }
                    }
                    #endregion
                } else if ( 18 < e.Y && e.Y <= 32 ) {
                    #region テンポ
                    int index = -1;
                    int count = AppManager.getVsqFile().TempoTable.size();
                    for ( int i = 0; i < count; i++ ) {
                        int clock = AppManager.getVsqFile().TempoTable.get( i ).Clock;
                        int x = AppManager.xCoordFromClocks( clock );
                        if ( x < 0 ) {
                            continue;
                        } else if ( Width < x ) {
                            break;
                        }
                        String s = (60e6 / (float)AppManager.getVsqFile().TempoTable.get( i ).Tempo).ToString( "#.00" );
                        Dimension size = Util.measureString( s, new Font( AppManager.editorConfig.ScreenFontName, java.awt.Font.PLAIN, 8 ) );
                        if ( isInRect( new Point( e.X, e.Y ), new Rectangle( x, 14, (int)size.width, 14 ) ) ) {
                            index = i;
                            break;
                        }
                    }

                    if ( index >= 0 ) {
                        int clock = AppManager.getVsqFile().TempoTable.get( index ).Clock;
                        if ( AppManager.getSelectedTool() != EditTool.ERASER ) {
                            int mouse_clock = AppManager.clockFromXCoord( e.X );
                            m_tempo_dragging_deltaclock = mouse_clock - clock;
                            m_tempo_dragging = true;
                        }
                        if ( (modifiers & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK ) {
                            if ( AppManager.getSelectedTempoCount() > 0 ) {
                                int last_clock = AppManager.getLastSelectedTempoClock();
                                int start = Math.Min( last_clock, clock );
                                int end = Math.Max( last_clock, clock );
                                for ( int i = 0; i < AppManager.getVsqFile().TempoTable.size(); i++ ) {
                                    int tclock = AppManager.getVsqFile().TempoTable.get( i ).Clock;
                                    if ( tclock < start ) {
                                        continue;
                                    } else if ( end < tclock ) {
                                        break;
                                    }
                                    if ( start <= tclock && tclock <= end ) {
                                        AppManager.addSelectedTempo( tclock );
                                    }
                                }
                            } else {
                                AppManager.addSelectedTempo( clock );
                            }
                        } else if ( (modifiers & s_modifier_key) == s_modifier_key ) {
                            if ( AppManager.isSelectedTempoContains( clock ) ) {
                                AppManager.removeSelectedTempo( clock );
                            } else {
                                AppManager.addSelectedTempo( clock );
                            }
                        } else {
                            if ( !AppManager.isSelectedTempoContains( clock ) ) {
                                AppManager.clearSelectedTempo();
                            }
                            AppManager.addSelectedTempo( clock );
                        }
                    } else {
                        AppManager.clearSelectedEvent();
                        AppManager.clearSelectedTempo();
                        AppManager.clearSelectedTimesig();
                    }
                    #endregion
                } else if ( 32 < e.Y && e.Y <= picturePositionIndicator.getHeight() - 1 ) {
                    #region 拍子
                    // クリック位置に拍子が表示されているかどうか検査
                    int index = -1;
                    for ( int i = 0; i < AppManager.getVsqFile().TimesigTable.size(); i++ ) {
                        String s = AppManager.getVsqFile().TimesigTable.get( i ).Numerator + "/" + AppManager.getVsqFile().TimesigTable.get( i ).Denominator;
                        int x = AppManager.xCoordFromClocks( AppManager.getVsqFile().TimesigTable.get( i ).Clock );
                        Dimension size = Util.measureString( s, new Font( AppManager.editorConfig.ScreenFontName, java.awt.Font.PLAIN, 8 ) );
                        if ( isInRect( new Point( e.X, e.Y ), new Rectangle( x, 28, (int)size.width, 14 ) ) ) {
                            index = i;
                            break;
                        }
                    }

                    if ( index >= 0 ) {
                        int barcount = AppManager.getVsqFile().TimesigTable.get( index ).BarCount;
                        if ( AppManager.getSelectedTool() != EditTool.ERASER ) {
                            int barcount_clock = AppManager.getVsqFile().getClockFromBarCount( barcount );
                            int mouse_clock = AppManager.clockFromXCoord( e.X );
                            m_timesig_dragging_deltaclock = mouse_clock - barcount_clock;
                            m_timesig_dragging = true;
                        }
                        if ( (modifiers & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK ) {
                            if ( AppManager.getSelectedTimesigCount() > 0 ) {
                                int last_barcount = AppManager.getLastSelectedTimesigBarcount();
                                int start = Math.Min( last_barcount, barcount );
                                int end = Math.Max( last_barcount, barcount );
                                for ( int i = 0; i < AppManager.getVsqFile().TimesigTable.size(); i++ ) {
                                    int tbarcount = AppManager.getVsqFile().TimesigTable.get( i ).BarCount;
                                    if ( tbarcount < start ) {
                                        continue;
                                    } else if ( end < tbarcount ) {
                                        break;
                                    }
                                    if ( start <= tbarcount && tbarcount <= end ) {
                                        AppManager.addSelectedTimesig( AppManager.getVsqFile().TimesigTable.get( i ).BarCount );
                                    }
                                }
                            } else {
                                AppManager.addSelectedTimesig( barcount );
                            }
                        } else if ( (modifiers & s_modifier_key) == s_modifier_key ) {
                            if ( AppManager.isSelectedTimesigContains( barcount ) ) {
                                AppManager.removeSelectedTimesig( barcount );
                            } else {
                                AppManager.addSelectedTimesig( barcount );
                            }
                        } else {
                            if ( !AppManager.isSelectedTimesigContains( barcount ) ) {
                                AppManager.clearSelectedTimesig();
                            }
                            AppManager.addSelectedTimesig( barcount );
                        }
                    } else {
                        AppManager.clearSelectedEvent();
                        AppManager.clearSelectedTempo();
                        AppManager.clearSelectedTimesig();
                    }
                    #endregion
                }
            }
            refreshScreen();
        }

        private void picturePositionIndicator_MouseClick( Object sender, BMouseEventArgs e ) {
            if ( e.X < AppManager.keyWidth || Width - 3 < e.X ) {
                return;
            }

            int modifiers = PortUtil.getCurrentModifierKey();
#if DEBUG
            AppManager.debugWriteLine( "picturePositionIndicator_MouseClick" );
#endif
            if ( e.Button == BMouseButtons.Left ) {
                if ( 4 <= e.Y && e.Y <= 18 ) {
                    #region マーカー位置の変更
                    if ( !m_startmark_dragging && !m_endmark_dragging ) {

                        int clock = AppManager.clockFromXCoord( e.X );
                        if ( AppManager.editorConfig.getPositionQuantize() != QuantizeMode.off ) {
                            int unit = AppManager.getPositionQuantizeClock();
                            int odd = clock % unit;
                            clock -= odd;
                            if ( odd > unit / 2 ) {
                                clock += unit;
                            }
                        }
                        AppManager.setCurrentClock( clock );
                        refreshScreen();
                    } else {
                        m_startmark_dragging = false;
                        m_endmark_dragging = false;
                    }
                    #endregion
                } else if ( 18 < e.Y && e.Y <= 32 ) {
                    if ( m_tempo_dragging ) {
                        int count = AppManager.getSelectedTempoCount();
                        int[] clocks = new int[count];
                        int[] new_clocks = new int[count];
                        int[] tempos = new int[count];
                        int i = -1;
                        boolean contains_first_tempo = false;
                        for ( Iterator itr = AppManager.getSelectedTempoIterator(); itr.hasNext(); ) {
                            ValuePair<Integer, SelectedTempoEntry> item = (ValuePair<Integer, SelectedTempoEntry>)itr.next();
                            int clock = item.getKey();
                            i++;
                            clocks[i] = clock;
                            if ( clock == 0 ) {
                                contains_first_tempo = true;
                                break;
                            }
                            TempoTableEntry editing = AppManager.getSelectedTempo( clock ).editing;
                            new_clocks[i] = editing.Clock;
                            tempos[i] = editing.Tempo;
                        }
                        if ( contains_first_tempo ) {
#if !JAVA
                            SystemSounds.Asterisk.Play();
#endif
                        } else {
                            CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandUpdateTempoRange( clocks, new_clocks, tempos ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                            setEdited( true );
                        }
                        m_tempo_dragging = false;
                    } else if ( !m_startmark_dragging && !m_endmark_dragging ) {
                        #region テンポの変更
#if DEBUG
                        AppManager.debugWriteLine( "TempoChange" );
#endif
                        AppManager.clearSelectedEvent();
                        AppManager.clearSelectedTimesig();
                        if ( AppManager.getSelectedTempoCount() > 0 ) {
                            #region テンポ変更があった場合
                            int index = -1;
                            int clock = AppManager.getLastSelectedTempoClock();
                            for ( int i = 0; i < AppManager.getVsqFile().TempoTable.size(); i++ ) {
                                if ( clock == AppManager.getVsqFile().TempoTable.get( i ).Clock ) {
                                    index = i;
                                    break;
                                }
                            }
                            if ( index >= 0 && AppManager.getSelectedTool() == EditTool.ERASER ) {
                                #region ツールがEraser
                                if ( AppManager.getVsqFile().TempoTable.get( index ).Clock == 0 ) {
                                    statusLabel.setText( _( "Cannot remove first symbol of track!" ) );
#if !JAVA
                                    SystemSounds.Asterisk.Play();
#endif
                                    return;
                                }
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandUpdateTempo( AppManager.getVsqFile().TempoTable.get( index ).Clock,
                                                                 AppManager.getVsqFile().TempoTable.get( index ).Clock,
                                                                 -1 ) );
                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                setEdited( true );
                                #endregion
                            }
                            #endregion
                        } else {
                            #region テンポ変更がなかった場合
                            AppManager.clearSelectedEvent();
                            AppManager.clearSelectedTempo();
                            AppManager.clearSelectedTimesig();
                            switch ( AppManager.getSelectedTool() ) {
                                case EditTool.ARROW:
                                case EditTool.ERASER:
                                    break;
                                case EditTool.PENCIL:
                                case EditTool.LINE:
                                    int changing_clock = AppManager.clockFromXCoord( e.X );
                                    int changing_tempo = AppManager.getVsqFile().getTempoAt( changing_clock );
                                    int bar_count;
                                    int bar_top_clock;
                                    int local_denominator, local_numerator;
                                    bar_count = AppManager.getVsqFile().getBarCountFromClock( changing_clock );
                                    bar_top_clock = AppManager.getVsqFile().getClockFromBarCount( bar_count );
                                    int index2 = -1;
                                    for ( int i = 0; i < AppManager.getVsqFile().TimesigTable.size(); i++ ) {
                                        if ( AppManager.getVsqFile().TimesigTable.get( i ).BarCount > bar_count ) {
                                            index2 = i;
                                            break;
                                        }
                                    }
                                    if ( index2 >= 1 ) {
                                        local_denominator = AppManager.getVsqFile().TimesigTable.get( index2 - 1 ).Denominator;
                                        local_numerator = AppManager.getVsqFile().TimesigTable.get( index2 - 1 ).Numerator;
                                    } else {
                                        local_denominator = AppManager.getVsqFile().TimesigTable.get( 0 ).Denominator;
                                        local_numerator = AppManager.getVsqFile().TimesigTable.get( 0 ).Numerator;
                                    }
                                    int clock_per_beat = 480 * 4 / local_denominator;
                                    int clocks_in_bar = changing_clock - bar_top_clock;
                                    int beat_in_bar = clocks_in_bar / clock_per_beat + 1;
                                    int clocks_in_beat = clocks_in_bar - (beat_in_bar - 1) * clock_per_beat;
                                    FormTempoConfig dlg = null;
                                    try {
                                        dlg = new FormTempoConfig( bar_count - AppManager.getVsqFile().getPreMeasure() + 1,
                                                                   beat_in_bar,
                                                                   local_numerator,
                                                                   clocks_in_beat,
                                                                   clock_per_beat,
                                                                   (float)(6e7 / changing_tempo),
                                                                   AppManager.getVsqFile().getPreMeasure() );
                                        dlg.setLocation( getFormPreferedLocation( dlg ) );
                                        if ( dlg.showDialog() == BDialogResult.OK ) {
                                            int new_beat = dlg.getBeatCount();
                                            int new_clocks_in_beat = dlg.getClock();
                                            int new_clock = bar_top_clock + (new_beat - 1) * clock_per_beat + new_clocks_in_beat;
#if DEBUG
                                            AppManager.debugWriteLine( "    new_beat=" + new_beat );
                                            AppManager.debugWriteLine( "    new_clocks_in_beat=" + new_clocks_in_beat );
                                            AppManager.debugWriteLine( "    changing_clock=" + changing_clock );
                                            AppManager.debugWriteLine( "    new_clock=" + new_clock );
#endif
                                            CadenciiCommand run = new CadenciiCommand(
                                                VsqCommand.generateCommandUpdateTempo( new_clock, new_clock, (int)(6e7 / (double)dlg.getTempo()) ) );
                                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                            setEdited( true );
                                            refreshScreen();
                                        }
                                    } catch ( Exception ex ) {
                                    } finally {
                                        if ( dlg != null ) {
                                            try {
                                                dlg.close();
                                            } catch ( Exception ex2 ) {
                                            }
                                        }
                                    }
                                    break;
                            }
                            #endregion
                        }
                        m_startmark_dragging = false;
                        m_endmark_dragging = false;
                        #endregion
                    }
                } else if ( 32 < e.Y && e.Y <= picturePositionIndicator.getHeight() - 1 ) {
                    if ( m_timesig_dragging ) {
                        int count = AppManager.getSelectedTimesigCount();
                        int[] barcounts = new int[count];
                        int[] new_barcounts = new int[count];
                        int[] numerators = new int[count];
                        int[] denominators = new int[count];
                        int i = -1;
                        boolean contains_first_bar = false;
                        for ( Iterator itr = AppManager.getSelectedTimesigIterator(); itr.hasNext(); ) {
                            ValuePair<Integer, SelectedTimesigEntry> item = (ValuePair<Integer, SelectedTimesigEntry>)itr.next();
                            int bar = item.getKey();
                            i++;
                            barcounts[i] = bar;
                            if ( bar == 0 ) {
                                contains_first_bar = true;
                                break;
                            }
                            TimeSigTableEntry editing = AppManager.getSelectedTimesig( bar ).editing;
                            new_barcounts[i] = editing.BarCount;
                            numerators[i] = editing.Numerator;
                            denominators[i] = editing.Denominator;
                        }
                        if ( contains_first_bar ) {
#if !JAVA
                            SystemSounds.Asterisk.Play();
#endif
                        } else {
                            CadenciiCommand run = new CadenciiCommand(
                                VsqCommand.generateCommandUpdateTimesigRange( barcounts, new_barcounts, numerators, denominators ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                            setEdited( true );
                        }
                        m_timesig_dragging = false;
                    } else if ( !m_startmark_dragging && !m_endmark_dragging ) {
                        #region 拍子の変更
                        AppManager.clearSelectedEvent();
                        AppManager.clearSelectedTempo();
                        if ( AppManager.getSelectedTimesigCount() > 0 ) {
                            #region 拍子変更があった場合
                            int index = 0;
                            int last_barcount = AppManager.getLastSelectedTimesigBarcount();
                            for ( int i = 0; i < AppManager.getVsqFile().TimesigTable.size(); i++ ) {
                                if ( AppManager.getVsqFile().TimesigTable.get( i ).BarCount == last_barcount ) {
                                    index = i;
                                    break;
                                }
                            }
                            if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                                #region ツールがEraser
                                if ( AppManager.getVsqFile().TimesigTable.get( index ).Clock == 0 ) {
                                    statusLabel.setText( _( "Cannot remove first symbol of track!" ) );
#if !JAVA
                                    SystemSounds.Asterisk.Play();
#endif
                                    return;
                                }
                                int barcount = AppManager.getVsqFile().TimesigTable.get( index ).BarCount;
                                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandUpdateTimesig( barcount, barcount, -1, -1 ) );
                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                setEdited( true );
                                #endregion
                            }
                            #endregion
                        } else {
                            #region 拍子変更がなかった場合
                            AppManager.clearSelectedEvent();
                            AppManager.clearSelectedTempo();
                            AppManager.clearSelectedTimesig();
                            switch ( AppManager.getSelectedTool() ) {
                                case EditTool.ERASER:
                                case EditTool.ARROW:
                                    break;
                                case EditTool.PENCIL:
                                case EditTool.LINE:
                                    int pre_measure = AppManager.getVsqFile().getPreMeasure();
                                    int clock = AppManager.clockFromXCoord( e.X );
                                    int bar_count = AppManager.getVsqFile().getBarCountFromClock( clock );
                                    int numerator, denominator;
                                    Timesig timesig = AppManager.getVsqFile().getTimesigAt( clock );
                                    int total_clock = AppManager.getVsqFile().TotalClocks;
                                    //int max_barcount = AppManager.VsqFile.getBarCountFromClock( total_clock ) - pre_measure + 1;
                                    //int min_barcount = 1;
#if DEBUG
                                    AppManager.debugWriteLine( "FormMain.picturePositionIndicator_MouseClick; bar_count=" + (bar_count - pre_measure + 1) );
#endif
                                    FormBeatConfig dlg = null;
                                    try {
                                        dlg = new FormBeatConfig( bar_count - pre_measure + 1, timesig.numerator, timesig.denominator, true, pre_measure );
                                        dlg.setLocation( getFormPreferedLocation( dlg ) );
                                        if ( dlg.showDialog() == BDialogResult.OK ) {
                                            if ( dlg.isEndSpecified() ) {
                                                int[] new_barcounts = new int[2];
                                                int[] numerators = new int[2];
                                                int[] denominators = new int[2];
                                                int[] barcounts = new int[2];
                                                new_barcounts[0] = dlg.getStart() + pre_measure - 1;
                                                new_barcounts[1] = dlg.getEnd() + pre_measure - 1 + 1;
                                                numerators[0] = dlg.getNumerator();
                                                numerators[1] = timesig.numerator;

                                                denominators[0] = dlg.getDenominator();
                                                denominators[1] = timesig.denominator;

                                                barcounts[0] = dlg.getStart() + pre_measure - 1;
                                                barcounts[1] = dlg.getEnd() + pre_measure - 1 + 1;
                                                CadenciiCommand run = new CadenciiCommand(
                                                    VsqCommand.generateCommandUpdateTimesigRange( barcounts, new_barcounts, numerators, denominators ) );
                                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                                setEdited( true );
                                            } else {
                                                CadenciiCommand run = new CadenciiCommand(
                                                    VsqCommand.generateCommandUpdateTimesig( bar_count,
                                                                                   dlg.getStart() + pre_measure - 1,
                                                                                   dlg.getNumerator(),
                                                                                   dlg.getDenominator() ) );
                                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                                setEdited( true );
                                            }
                                        }
                                    } catch ( Exception ex ) {
                                    } finally {
                                        if ( dlg != null ) {
                                            try {
                                                dlg.close();
                                            } catch ( Exception ex2 ) {
                                            }
                                        }
                                    }
                                    break;
                            }
                            #endregion
                        }
                        m_startmark_dragging = false;
                        m_endmark_dragging = false;
                        #endregion
                    }
                }
            }
            pictPianoRoll.repaint();
            picturePositionIndicator.repaint();
        }

        private void picturePositionIndicator_MouseMove( Object sender, BMouseEventArgs e ) {
            if ( m_tempo_dragging ) {
                int clock = AppManager.clockFromXCoord( e.X ) - m_tempo_dragging_deltaclock;
                int step = AppManager.getPositionQuantizeClock();
                int odd = clock % step;
                clock -= odd;
                if ( odd > step / 2 ) {
                    clock += step;
                }
                int last_clock = AppManager.getLastSelectedTempoClock();
                int dclock = clock - last_clock;
                for ( Iterator itr = AppManager.getSelectedTempoIterator(); itr.hasNext(); ) {
                    ValuePair<Integer, SelectedTempoEntry> item = (ValuePair<Integer, SelectedTempoEntry>)itr.next();
                    int key = item.getKey();
                    AppManager.getSelectedTempo( key ).editing.Clock = AppManager.getSelectedTempo( key ).original.Clock + dclock;
                }
                picturePositionIndicator.repaint();
            } else if ( m_timesig_dragging ) {
                int clock = AppManager.clockFromXCoord( e.X ) - m_timesig_dragging_deltaclock;
                int barcount = AppManager.getVsqFile().getBarCountFromClock( clock );
                int last_barcount = AppManager.getLastSelectedTimesigBarcount();
                int dbarcount = barcount - last_barcount;
                for ( Iterator itr = AppManager.getSelectedTimesigIterator(); itr.hasNext(); ) {
                    ValuePair<Integer, SelectedTimesigEntry> item = (ValuePair<Integer, SelectedTimesigEntry>)itr.next();
                    int bar = item.getKey();
                    AppManager.getSelectedTimesig( bar ).editing.BarCount = AppManager.getSelectedTimesig( bar ).original.BarCount + dbarcount;
                }
                picturePositionIndicator.repaint();
            } else if ( m_startmark_dragging ) {
                int clock = AppManager.clockFromXCoord( e.X );
                int unit = AppManager.getPositionQuantizeClock();
                int odd = clock % unit;
                clock -= odd;
                if ( odd > unit / 2 ) {
                    clock += unit;
                }
                int draft_start = Math.Min( clock, AppManager.endMarker );
                int draft_end = Math.Max( clock, AppManager.endMarker );
                if ( draft_start != AppManager.startMarker ) {
                    AppManager.startMarker = draft_start;
                }
                if ( draft_end != AppManager.endMarker ) {
                    AppManager.endMarker = draft_end;
                }
                refreshScreen();
            } else if ( m_endmark_dragging ) {
                int clock = AppManager.clockFromXCoord( e.X );
                int unit = AppManager.getPositionQuantizeClock();
                int odd = clock % unit;
                clock -= odd;
                if ( odd > unit / 2 ) {
                    clock += unit;
                }
                int draft_start = Math.Min( clock, AppManager.startMarker );
                int draft_end = Math.Max( clock, AppManager.startMarker );
                if ( draft_start != AppManager.startMarker ) {
                    AppManager.startMarker = draft_start;
                }
                if ( draft_end != AppManager.endMarker ) {
                    AppManager.endMarker = draft_end;
                }
                refreshScreen();
            }
        }

        private void picturePositionIndicator_MouseLeave( Object sender, BEventArgs e ) {
            m_startmark_dragging = false;
            m_endmark_dragging = false;
            m_tempo_dragging = false;
            m_timesig_dragging = false;
        }

        private void picturePositionIndicator_Paint( Object sender, BPaintEventArgs e ) {
            picturePositionIndicatorDrawTo( new Graphics2D( e.Graphics ) );
        }

        private void picturePositionIndicator_PreviewKeyDown( Object sender, BPreviewKeyDownEventArgs e ) {
            ProcessSpecialShortcutKey( e );
        }
        #endregion

        #region trackBar
        private void trackBar_Enter( Object sender, BEventArgs e ) {
            pictPianoRoll.Select();
        }

        private void trackBar_MouseDown( Object sender, BMouseEventArgs e ) {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
        }

        private void trackBar_ValueChanged( Object sender, BEventArgs e ) {
            AppManager.scaleX = trackBar.getValue() / 480f;
            AppManager.startToDrawX = (int)(hScroll.getValue() * AppManager.scaleX);
#if USE_DOBJ
            updateDrawObjectList();
#endif
            repaint();
        }
        #endregion

        #region menuHelp
        private void menuHelpAbout_Click( Object sender, BEventArgs e ) {
            String version_str = AppManager.getVersion() + "\n\n" +
                                 AppManager.getAssemblyNameAndFileVersion( typeof( Boare.Lib.AppUtil.Util ) ) + "\n" +
                                 AppManager.getAssemblyNameAndFileVersion( typeof( Boare.Lib.Media.Wave ) ) + "\n" +
                                 AppManager.getAssemblyNameAndFileVersion( typeof( Boare.Lib.Vsq.VsqFile ) ) + "\n" +
                                 AppManager.getAssemblyNameAndFileVersion( typeof( bocoree.math ) /*) + "\n" +
                                 AppManager.GetAssemblyNameAndFileVersion( typeof( Boare.Cadencii.vstidrv )*/
                                                                                                              );
            if ( m_versioninfo == null ) {
                m_versioninfo = new Boare.Cadencii.VersionInfo( _APP_NAME, version_str );
                //m_versioninfo.Credit = Boare.Cadencii.Properties.Resources.author_list;
                m_versioninfo.setAuthorList( _CREDIT );
#if DEBUG
#if AUTHOR_LIST_SAVE_BUTTON_VISIBLE
                m_versioninfo.setSaveAuthorListVisible( true );
#else
                m_versioninfo.setSaveAuthorListVisible( false );
#endif
#else
                m_versioninfo.setSaveAuthorListVisible( false );
#endif
                m_versioninfo.setVisible( true );
            } else {
#if !JAVA
                if ( m_versioninfo.IsDisposed ) 
#endif
                {
                    m_versioninfo = new Boare.Cadencii.VersionInfo( _APP_NAME, version_str );
                    //m_versioninfo.Credit = Boare.Cadencii.Properties.Resources.author_list;
                    m_versioninfo.setAuthorList( _CREDIT );
#if DEBUG
#if AUTHOR_LIST_SAVE_BUTTON_VISIBLE
                    m_versioninfo.setSaveAuthorListVisible( true );
#else
                    m_versioninfo.setSaveAuthorListVisible( false );
#endif
#else
                    m_versioninfo.setSaveAuthorListVisible( false );
#endif
                }
                m_versioninfo.setVisible( true );
            }
        }

        private void menuHelpDebug_Click( Object sender, BEventArgs e ) {
            PortUtil.println( "menuHelpDebug_Click" );
#if DEBUG
            /*InputBox ib = new InputBox( "input shift seconds" );
            if ( ib.ShowDialog() == DialogResult.OK ) {
                VsqFileEx vsq = (VsqFileEx)AppManager.getVsqFile().Clone();
                VsqFileEx.shift( vsq, double.Parse( ib.Result ) );
                CadenciiCommand run = VsqFileEx.generateCommandReplace( vsq );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                Edited = true;
            }*/
            /*DialogResult dr = DialogResult.OK;
            while ( dr == DialogResult.OK ) {
                FileDialog fd = new FileDialog( FileDialog.DialogMode.Open );
                fd.Filter = "All Files(*.*)|*.*|VSQ Format(*.vsq)|*.vsq";
                dr = fd.ShowDialog();
                AppManager.DebugWriteLine( "fd.FileName=" + fd.FileName );
            }*/
            /*using ( OpenFileDialog ofd = new OpenFileDialog() ) {
                if ( ofd.ShowDialog() == DialogResult.OK ) {
                    using ( SaveFileDialog sfd = new SaveFileDialog() ) {
                        sfd.InitialDirectory = Path.GetDirectoryName( ofd.FileName );
                        sfd.FileName = PortUtil.getFileNameWithoutExtension( ofd.FileName ) + ".txt";
                        if ( sfd.ShowDialog() == DialogResult.OK ) {
                            using ( Wave w = new Wave( ofd.FileName ) ) {
                                w.PrintToText( sfd.FileName );
                            }
                        }
                    }
                }
            }*/

            /*using ( OpenFileDialog ofd = new OpenFileDialog() ) {
                if ( ofd.ShowDialog() == DialogResult.OK ) {
                    String file = ofd.FileName;
                    using ( Wave wv = new Wave( file ) ) {
                        const double sec_dt = 0.0025;
                        uint samples = wv.TotalSamples;
                        const int _WINDOW_WIDTH = 2048;
                        uint spl_dt = (uint)(sec_dt * wv.SampleRate);
                        double[] window_func = new double[_WINDOW_WIDTH];
                        for ( int i = 0; i < _WINDOW_WIDTH; i++ ) {
                            window_func[i] = bocoree.math.window_func( bocoree.math.WindowFunctionType.Hamming, i / (double)_WINDOW_WIDTH );
                        }
                        Wave.TestEnabled = false;

                        InputBox ib = new InputBox( "input clock count" );
                        if ( ib.ShowDialog() == DialogResult.OK ) {
                            int count = 0;
                            if ( int.TryParse( ib.Result, out count ) ) {
                                Wave.TestEnabled = true;
                                double f0 = wv.GetF0( (uint)count, window_func );
                                AppManager.showMessageBox( "f0=" + f0 + " at sample=" + count );
                                Wave.TestEnabled = false;
                            }
                        }
                        if ( AppManager.showMessageBox( "calculate all formant profile?", "Cadencii", MessageBoxButtons.YesNo ) == DialogResult.Yes ) {
                            using ( StreamWriter sw = new StreamWriter( ofd.FileName + "_formanto.txt" ) ) {
                                FormantoDetectionArguments fda = new FormantoDetectionArguments();
                                fda.PeakDetectionThreshold = 0.05;
                                for ( uint spl_i = 0; spl_i < samples; spl_i += spl_dt ) {
                                    double f0 = wv.GetF0( spl_i, window_func, fda );
                                    double note;
                                    if ( f0 > 0.0 ) {
                                        note = 12.0 * Math.Log( f0 / 440.0, 2.0 ) + 69;
                                    } else {
                                        note = 0.0;
                                    }
                                    sw.WriteLine( spl_i + "\t" + (spl_i / (double)wv.SampleRate) + "\t" + f0 + "\t" + note + "\t" + Math.Round( note, 0, MidpointRounding.AwayFromZero ) );
                                }
                            }
                        }
                        if ( AppManager.showMessageBox( "calculate volume profile?", "Cadencii", MessageBoxButtons.YesNo ) == DialogResult.Yes ) {
                            using ( StreamWriter sw = new StreamWriter( ofd.FileName + "_volume.txt" ) ) {
                                for ( uint spl_i = 0; spl_i < samples; spl_i += spl_dt ) {
                                    double volume = wv.GetVolume( (int)spl_i, window_func );
                                    sw.WriteLine( spl_i + "\t" + (spl_i / (double)wv.SampleRate) + "\t" + volume );
                                }
                            }
                        }
                    }
                }
            }*/
#endif
#if FOO
            using ( OpenFileDialog ofd = new OpenFileDialog() ) {
                if ( ofd.ShowDialog() == DialogResult.OK ) {
                    using ( Wave wv = new Wave( ofd.FileName ) ) {
                        wv.TrimSilence();
                        const int _WIN_LEN = 441;
                        double[] window = new double[_WIN_LEN];
                        for ( int i = 0; i < _WIN_LEN; i++ ) {
                            window[i] = bocoree.math.window_func( bocoree.math.WindowFunctionType.Hamming, (double)i / (double)_WIN_LEN );
                        }
                        using ( StreamWriter sw = new StreamWriter( ofd.FileName + ".txt" ) ) {
                            for ( int i = 0; i < wv.TotalSamples - _WIN_LEN / 10; i += _WIN_LEN / 10 ) {
                                sw.WriteLine( i / (double)wv.SampleRate + "\t" + wv.GetVolume( i, window ) );
                            }
                        }
                        /*using ( StreamWriter sw = new StreamWriter( ofd.FileName + ".txt" ) ) {
                            double to = 7000.0 * 2.0 / (double)wv.SampleRate;
                            int jmax = (int)(to * _WIN_LEN);
                            double resolution = 1e-3;
                            uint di = (uint)(resolution * wv.SampleRate);
                            for ( uint i = 0; i < wv.TotalSamples; i += di ) {
                                double[] formanto = wv.GetFormanto( i, window );
                                for ( int j = 1; j < _WIN_LEN && j < jmax; j++ ) {
                                    //double f = (double)j / (double)_WIN_LEN * (double)wv.SampleRate / 2.0;
                                    sw.Write( "{0:f4}\t", Math.Abs( formanto[j] ) );
                                }
                                sw.WriteLine();
                            }
                        }*/
                    }
                }
            }
#endif
        }
        #endregion

        #region trackSelector
        private void trackSelector_CommandExecuted( Object sender, BEventArgs e ) {
            setEdited( true );
            refreshScreen();
        }

        private void trackSelector_MouseClick( Object sender, BMouseEventArgs e ) {
            if ( e.Button == BMouseButtons.Right ) {
                if ( AppManager.keyWidth < e.X && e.X < trackSelector.getWidth() - AppManager.keyWidth ) {
                    if ( trackSelector.getHeight() - TrackSelector.OFFSET_TRACK_TAB <= e.Y && e.Y <= trackSelector.getHeight() ) {
                        cMenuTrackTab.show( trackSelector, e.X, e.Y );
                    } else {
                        cMenuTrackSelector.show( trackSelector, e.X, e.Y );
                    }
                }
            }
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
        }

        private void trackSelector_MouseDown( Object sender, BMouseEventArgs e ) {
            if ( AppManager.keyWidth < e.X ) {
                m_mouse_downed_trackselector = true;
                BMouseButtons btn = e.Button;
                if ( AppManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier && m_spacekey_downed && e.Button == BMouseButtons.Left ) {
                    btn = BMouseButtons.Middle;
                }
                if ( btn == BMouseButtons.Middle ) {
                    m_edit_curve_mode = CurveEditMode.MIDDLE_DRAG;
                    m_button_initial = new Point( e.X, e.Y );
                    m_middle_button_hscroll = hScroll.getValue();
#if !JAVA
                    this.Cursor = HAND;
#endif
                }
            }
        }

        private void trackSelector_MouseMove( Object sender, BMouseEventArgs e ) {
            if ( m_form_activated ) {
#if ENABLE_PROPERTY
                if ( AppManager.inputTextBox != null && !AppManager.inputTextBox.IsDisposed && !AppManager.inputTextBox.isVisible() && !AppManager.propertyPanel.isEditing() ) {
#else
                if ( AppManager.inputTextBox != null && !AppManager.inputTextBox.IsDisposed && !AppManager.inputTextBox.isVisible() ) {
#endif
                    trackSelector.requestFocus();
                }
            }
            if ( e.Button == BMouseButtons.None ) {
                int cl = AppManager.clockFromXCoord( e.X );
                updatePositionViewFromMousePosition( cl );
                refreshScreen();
                return;
            }
            int parent_width = ((TrackSelector)sender).getWidth();
            if ( m_edit_curve_mode == CurveEditMode.MIDDLE_DRAG ) {
                int dx = e.X - m_button_initial.x;
                int dy = e.Y - m_button_initial.y;
                double new_hscroll_value = (double)m_middle_button_hscroll - (double)dx / AppManager.scaleX;
                int draft;
                if ( new_hscroll_value < hScroll.getMinimum() ) {
                    draft = hScroll.getMinimum();
                } else if ( hScroll.getMaximum() < new_hscroll_value ) {
                    draft = hScroll.getMaximum();
                } else {
                    draft = (int)new_hscroll_value;
                }
                if ( AppManager.isPlaying() ) {
                    return;
                }
                if ( hScroll.getValue() != draft ) {
                    hScroll.setValue( draft );
                }
            } else {
                if ( m_mouse_downed_trackselector ) {
                    if ( m_ext_dragx_trackselector == ExtDragXMode.NONE ) {
                        if ( AppManager.keyWidth > e.X ) {
                            m_ext_dragx_trackselector = ExtDragXMode.LEFT;
                        } else if ( parent_width < e.X ) {
                            m_ext_dragx_trackselector = ExtDragXMode.RIGHT;
                        }
                    } else {
                        if ( AppManager.keyWidth <= e.X && e.X <= parent_width ) {
                            m_ext_dragx_trackselector = ExtDragXMode.NONE;
                        }
                    }
                } else {
                    m_ext_dragx_trackselector = ExtDragXMode.NONE;
                }

                if ( m_ext_dragx_trackselector != ExtDragXMode.NONE ) {
                    double now = PortUtil.getCurrentTime();
                    double dt = now - m_timer_drag_last_ignitted;
                    m_timer_drag_last_ignitted = now;
                    int px_move = AppManager.editorConfig.MouseDragIncrement;
                    if ( px_move / dt > AppManager.editorConfig.MouseDragMaximumRate ) {
                        px_move = (int)(dt * AppManager.editorConfig.MouseDragMaximumRate);
                    }
                    double d_draft;
                    if ( m_ext_dragx_trackselector == ExtDragXMode.RIGHT ) {
                        int right_clock = AppManager.clockFromXCoord( parent_width + 5 );
                        int dclock = (int)(px_move / AppManager.scaleX);
                        d_draft = (73 - trackSelector.getWidth()) / AppManager.scaleX + right_clock + dclock;
                    } else {
                        px_move *= -1;
                        int left_clock = AppManager.clockFromXCoord( AppManager.keyWidth );
                        int dclock = (int)(px_move / AppManager.scaleX);
                        d_draft = (73 - AppManager.keyWidth) / AppManager.scaleX + left_clock + dclock;
                    }
                    if ( d_draft < 0.0 ) {
                        d_draft = 0.0;
                    }
                    int draft = (int)d_draft;
                    if ( hScroll.getMaximum() < draft ) {
                        hScroll.setMaximum( draft );
                    }
                    if ( draft < hScroll.getMinimum() ) {
                        draft = hScroll.getMinimum();
                    }
                    hScroll.setValue( draft );
                }
            }
            int clock = AppManager.clockFromXCoord( e.X );
            updatePositionViewFromMousePosition( clock );
            refreshScreen();
        }

        private void trackSelector_MouseUp( Object sender, BMouseEventArgs e ) {
            m_mouse_downed_trackselector = false;
            if ( m_edit_curve_mode == CurveEditMode.MIDDLE_DRAG ) {
                m_edit_curve_mode = CurveEditMode.NONE;
                setCursor( new Cursor( java.awt.Cursor.DEFAULT_CURSOR ) );
            }
        }

        private void trackSelector_MouseWheel( Object sender, BMouseEventArgs e ) {
            if ( (PortUtil.getCurrentModifierKey() & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK ) {
                double new_val = (double)vScroll.getValue() - e.Delta;
                if ( new_val > vScroll.getMaximum() ) {
                    vScroll.setValue( vScroll.getMaximum() );
                } else if ( new_val < vScroll.getMinimum() ) {
                    vScroll.setValue( vScroll.getMinimum() );
                } else {
                    vScroll.setValue( (int)new_val );
                }
            } else {
                hScroll.setValue( computeScrollValueFromWheelDelta( e.Delta ) );
            }
            refreshScreen();
        }

        private void trackSelector_PreferredMinHeightChanged( Object sender, BEventArgs e ) {
            if ( menuVisualControlTrack.isSelected() ) {
                splitContainer1.Panel2MinSize = trackSelector.getPreferredMinSize();
#if DEBUG
                PortUtil.println( "FormMain#trackSelector_PreferredMinHeightChanged; splitContainer1.Panel2MinSize changed" );
#endif
            }
        }

        private void trackSelector_PreviewKeyDown( Object sender, BPreviewKeyDownEventArgs e ) {
            ProcessSpecialShortcutKey( e );
        }

        private void trackSelector_RenderRequired( Object sender, int[] tracks ) {
            render( tracks );
            Vector<Integer> t = new Vector<Integer>( tracks );
            if ( t.contains( AppManager.getSelected() ) ) {
                String file = PortUtil.combinePath( AppManager.getTempWaveDir(), AppManager.getSelected() + ".wav" );
                if ( PortUtil.isFileExists( file ) ) {
                    Thread loadwave_thread = new Thread( new ParameterizedThreadStart( this.LoadWaveThreadProc ) );
                    loadwave_thread.IsBackground = true;
                    loadwave_thread.Start( file );
                }
            }
        }

        private void trackSelector_SelectedCurveChanged( Object sender, CurveType type ) {
            refreshScreen();
        }

        private void trackSelector_SelectedTrackChanged( Object sender, int selected ) {
            if ( menuVisualWaveform.isSelected() ) {
                waveView.clear();
                String file = PortUtil.combinePath( AppManager.getTempWaveDir(), selected + ".wav" );
                if ( PortUtil.isFileExists( file ) ) {
                    Thread load_wave = new Thread( new ParameterizedThreadStart( this.LoadWaveThreadProc ) );
                    load_wave.IsBackground = true;
                    load_wave.Start( (Object)file );
                }
            }
            AppManager.clearSelectedBezier();
            AppManager.clearSelectedEvent();
            AppManager.clearSelectedPoint();
#if USE_DOBJ
            updateDrawObjectList();
#endif
            refreshScreen();
        }
        #endregion

        #region cMenuPiano*
        private void cMenuPianoDelete_Click( Object sender, BEventArgs e ) {
            deleteEvent();
        }

        private void cMenuPianoVibratoProperty_Click( Object sender, BEventArgs e ) {
            editNoteVibratoProperty();
        }

        private void cMenuPianoPaste_Click( Object sender, BEventArgs e ) {
            pasteEvent();
        }

        private void cMenuPianoCopy_Click( Object sender, BEventArgs e ) {
            copyEvent();
        }

        private void cMenuPianoCut_Click( Object sender, BEventArgs e ) {
            cutEvent();
        }

        private void cMenuPianoExpression_Click( Object sender, BEventArgs e ) {
            if ( AppManager.getSelectedEventCount() > 0 ) {
                SynthesizerType type = SynthesizerType.VOCALOID2;
                if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                    type = SynthesizerType.VOCALOID1;
                }
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                FormNoteExpressionConfig dlg = null;
                try {
                    dlg = new FormNoteExpressionConfig( type, original.ID.NoteHeadHandle );
                    int id = AppManager.getLastSelectedEvent().original.InternalID;
                    dlg.setPMBendDepth( original.ID.PMBendDepth );
                    dlg.setPMBendLength( original.ID.PMBendLength );
                    dlg.setPMbPortamentoUse( original.ID.PMbPortamentoUse );
                    dlg.setDEMdecGainRate( original.ID.DEMdecGainRate );
                    dlg.setDEMaccent( original.ID.DEMaccent );
                    if ( dlg.showDialog() == BDialogResult.OK ) {
                        VsqID copy = (VsqID)original.ID.clone();
                        copy.PMBendDepth = dlg.getPMBendDepth();
                        copy.PMBendLength = dlg.getPMBendLength();
                        copy.PMbPortamentoUse = dlg.getPMbPortamentoUse();
                        copy.DEMdecGainRate = dlg.getDEMdecGainRate();
                        copy.DEMaccent = dlg.getDEMaccent();
                        copy.NoteHeadHandle = dlg.getEditedNoteHeadHandle();
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(), id, copy ) );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                        setEdited( true );
                    }
                } catch ( Exception ex ) {
                } finally {
                    if ( dlg != null ) {
                        try {
                            dlg.close();
                        } catch ( Exception ex2 ) {
                        }
                    }
                }
            }
        }

        private void cMenuPianoPointer_Click( Object sender, BEventArgs e ) {
            AppManager.setSelectedTool( EditTool.ARROW );
        }

        private void cMenuPianoPencil_Click( Object sender, BEventArgs e ) {
            AppManager.setSelectedTool( EditTool.PENCIL );
        }

        private void cMenuPianoEraser_Click( Object sender, BEventArgs e ) {
            AppManager.setSelectedTool( EditTool.ERASER );
        }

        private void handlePositionQuantize( Object sender, BEventArgs e ) {
            QuantizeMode qm = AppManager.editorConfig.getPositionQuantize();
            if ( sender == stripDDBtnQuantize04 || sender == cMenuPianoQuantize04 || sender == menuSettingPositionQuantize04 ) {
                qm = QuantizeMode.p4;
            } else if ( sender == stripDDBtnQuantize08 || sender == cMenuPianoQuantize08 || sender == menuSettingPositionQuantize08 ) {
                qm = QuantizeMode.p8;
            } else if ( sender == stripDDBtnQuantize16 || sender == cMenuPianoQuantize16 || sender == menuSettingPositionQuantize16 ) {
                qm = QuantizeMode.p16;
            } else if ( sender == stripDDBtnQuantize32 || sender == cMenuPianoQuantize32 || sender == menuSettingPositionQuantize32 ) {
                qm = QuantizeMode.p32;
            } else if ( sender == stripDDBtnQuantize64 || sender == cMenuPianoQuantize64 || sender == menuSettingPositionQuantize64 ) {
                qm = QuantizeMode.p64;
            } else if ( sender == stripDDBtnQuantize128 || sender == cMenuPianoQuantize128 || sender == menuSettingPositionQuantize128 ) {
                qm = QuantizeMode.p128;
            } else if ( sender == stripDDBtnQuantizeOff || sender == cMenuPianoQuantizeOff || sender == menuSettingPositionQuantizeOff ) {
                qm = QuantizeMode.off;
            }
            AppManager.editorConfig.setPositionQuantize( qm );
            refreshScreen();
        }

        private void h_positionQuantizeTriplet( Object sender, BEventArgs e ) {
            AppManager.editorConfig.setPositionQuantizeTriplet( !AppManager.editorConfig.isPositionQuantizeTriplet() );
            refreshScreen();
        }

        private void h_lengthQuantize04( Object sender, BEventArgs e ) {
            AppManager.editorConfig.setLengthQuantize(QuantizeMode.p4 );
        }

        private void h_lengthQuantize08( Object sender, BEventArgs e ) {
            AppManager.editorConfig.setLengthQuantize( QuantizeMode.p8 );
        }

        private void h_lengthQuantize16( Object sender, BEventArgs e ) {
            AppManager.editorConfig.setLengthQuantize( QuantizeMode.p16 );
        }

        private void h_lengthQuantize32( Object sender, BEventArgs e ) {
            AppManager.editorConfig.setLengthQuantize( QuantizeMode.p32 );
        }

        private void h_lengthQuantize64( Object sender, BEventArgs e ) {
            AppManager.editorConfig.setLengthQuantize( QuantizeMode.p64 );
        }

        private void h_lengthQuantize128( Object sender, BEventArgs e ) {
            AppManager.editorConfig.setLengthQuantize( QuantizeMode.p128 );
        }

        private void h_lengthQuantizeOff( Object sender, BEventArgs e ) {
            AppManager.editorConfig.setLengthQuantize( QuantizeMode.off );
        }

        private void h_lengthQuantizeTriplet( Object sender, BEventArgs e ) {
            AppManager.editorConfig.setLengthQuantizeTriplet( !AppManager.editorConfig.isLengthQuantizeTriplet() );
        }

        private void cMenuPianoGrid_Click( Object sender, BEventArgs e ) {
            cMenuPianoGrid.setSelected( !cMenuPianoGrid.isSelected() );
            AppManager.setGridVisible( cMenuPianoGrid.isSelected() );
        }

        private void cMenuPianoUndo_Click( Object sender, BEventArgs e ) {
            undo();
        }

        private void cMenuPianoRedo_Click( Object sender, BEventArgs e ) {
            redo();
        }

        private void cMenuPianoSelectAllEvents_Click( Object sender, BEventArgs e ) {
            selectAllEvent();
        }

        private void cMenuPianoProperty_Click( Object sender, BEventArgs e ) {
            editNoteExpressionProperty();
        }

        private void cMenuPianoImportLyric_Click( Object sender, BEventArgs e ) {
            importLyric();
        }

        private void cMenuPiano_Opening( Object sender, BCancelEventArgs e ) {
            updateCopyAndPasteButtonStatus();
            cMenuPianoImportLyric.setEnabled( AppManager.getLastSelectedEvent() != null );
        }

        private void cMenuPianoSelectAll_Click( Object sender, BEventArgs e ) {
            selectAll();
        }

        private void cMenuPianoFixed01_Click( Object sender, BEventArgs e ) {
            m_pencil_mode.setMode( PencilModeEnum.L1 );
            updateCMenuPianoFixed();
        }

        private void cMenuPianoFixed02_Click( Object sender, BEventArgs e ) {
            m_pencil_mode.setMode( PencilModeEnum.L2 );
            updateCMenuPianoFixed();
        }

        private void cMenuPianoFixed04_Click( Object sender, BEventArgs e ) {
            m_pencil_mode.setMode( PencilModeEnum.L4 );
            updateCMenuPianoFixed();
        }

        private void cMenuPianoFixed08_Click( Object sender, BEventArgs e ) {
            m_pencil_mode.setMode( PencilModeEnum.L8 );
            updateCMenuPianoFixed();
        }

        private void cMenuPianoFixed16_Click( Object sender, BEventArgs e ) {
            m_pencil_mode.setMode( PencilModeEnum.L16 );
            updateCMenuPianoFixed();
        }

        private void cMenuPianoFixed32_Click( Object sender, BEventArgs e ) {
            m_pencil_mode.setMode( PencilModeEnum.L32 );
            updateCMenuPianoFixed();
        }

        private void cMenuPianoFixed64_Click( Object sender, BEventArgs e ) {
            m_pencil_mode.setMode( PencilModeEnum.L64 );
            updateCMenuPianoFixed();
        }

        private void cMenuPianoFixed128_Click( Object sender, BEventArgs e ) {
            m_pencil_mode.setMode( PencilModeEnum.L128 );
            updateCMenuPianoFixed();
        }

        private void cMenuPianoFixedOff_Click( Object sender, BEventArgs e ) {
            m_pencil_mode.setMode( PencilModeEnum.Off );
            updateCMenuPianoFixed();
        }

        private void cMenuPianoFixedTriplet_Click( Object sender, BEventArgs e ) {
            m_pencil_mode.setTriplet( !m_pencil_mode.isTriplet() );
            updateCMenuPianoFixed();
        }

        private void cMenuPianoFixedDotted_Click( Object sender, BEventArgs e ) {
            m_pencil_mode.setDot( !m_pencil_mode.isDot() );
            updateCMenuPianoFixed();
        }

        private void cMenuPianoCurve_Click( Object sender, BEventArgs e ) {
            AppManager.setCurveMode( !AppManager.isCurveMode() );
            applySelectedTool();
        }
        #endregion

        #region menuTrack*
        private void menuTrack_DropDownOpening( Object sender, BEventArgs e ) {
            updateTrackMenuStatus();
        }

        private void menuTrackCopy_Click( Object sender, BEventArgs e ) {
            copyTrackCore();
        }

        private void menuTrackChangeName_Click( Object sender, BEventArgs e ) {
            changeTrackNameCore();
        }

        private void menuTrackDelete_Click( Object sender, BEventArgs e ) {
            deleteTrackCore();
        }

        private void menuTrackOn_Click( Object sender, BEventArgs e ) {
            menuTrackOn.setSelected( !menuTrackOn.isSelected() );
            CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandTrackChangePlayMode( AppManager.getSelected(),
                                                                                                      menuTrackOn.isSelected() ? 1 : -1 ) );
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
            setEdited( true );
            refreshScreen();
        }

        private void menuTrackAdd_Click( Object sender, BEventArgs e ) {
            addTrackCore();
        }

        private void menuTrackOverlay_Click( Object sender, BEventArgs e ) {
            AppManager.setOverlay( !AppManager.isOverlay() );
            refreshScreen();
        }

        private void menuTrackRenderCurrent_Click( Object sender, BEventArgs e ) {
            render( new int[] { AppManager.getSelected() } );
        }

        private void commonTrackRenderAll_Click( Object sender, BEventArgs e ) {
            Vector<Integer> list = new Vector<int>();
            int c = AppManager.getVsqFile().Track.size();
            for ( int i = 1; i < c; i++ ) {
                if ( AppManager.getRenderRequired( i ) ) {
                    list.add( i );
                }
            }
            if ( list.size() <= 0 ) {
                return;
            }
            render( list.toArray( new int[] { } ) );
        }

        private void menuTrackRenderer_DropDownOpening( Object sender, BEventArgs e ) {
            updateRendererMenu();
        }

        private void menuTrackOn_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Enable current track." ) );
        }

        private void menuTrackAdd_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Add new track." ) );
        }

        private void menuTrackCopy_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Copy current track." ) );
        }

        private void menuTrackChangeName_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Change track name." ) );
        }

        private void menuTrackDelete_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Delete current track." ) );
        }

        private void menuTrackRenderCurrent_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Render current track." ) );
        }

        private void menuTrackRenderAll_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Render all tracks." ) );
        }

        private void menuTrackOverlay_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Show background items." ) );
        }

        private void menuTrackRenderer_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Select voice synthesis engine." ) );
        }

        private void menuTrackRendererVOCALOID1_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "VOCALOID1" ) );
        }

        private void menuTrackRendererVOCALOID2_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "VOCALOID2" ) );
        }

        private void menuTrackRendererUtau_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "UTAU" ) );
        }

        private void menuTrackMasterTuning_MouseEnter( Object sender, BEventArgs e ) {
            statusLabel.setText( _( "Set global pitch shift." ) );
        }
        #endregion

        #region menuHidden*
        private void menuHiddenTrackNext_Click( Object sender, BEventArgs e ) {
            if ( AppManager.getSelected() == AppManager.getVsqFile().Track.size() - 1 ) {
                AppManager.setSelected( 1 );
            } else {
                AppManager.setSelected( AppManager.getSelected() + 1 );
            }
            refreshScreen();
        }

        private void menuHiddenTrackBack_Click( Object sender, BEventArgs e ) {
            if ( AppManager.getSelected() == 1 ) {
                AppManager.setSelected( AppManager.getVsqFile().Track.size() - 1 );
            } else {
                AppManager.setSelected( AppManager.getSelected() - 1 );
            }
            refreshScreen();
        }

        private void menuHiddenEditPaste_Click( Object sender, BEventArgs e ) {
            pasteEvent();
        }

        private void menuHiddenEditFlipToolPointerPencil_Click( Object sender, BEventArgs e ) {
            if ( AppManager.getSelectedTool() == EditTool.ARROW ) {
                AppManager.setSelectedTool( EditTool.PENCIL );
            } else {
                AppManager.setSelectedTool( EditTool.ARROW );
            }
            refreshScreen();
        }

        private void menuHiddenEditFlipToolPointerEraser_Click( Object sender, BEventArgs e ) {
            if ( AppManager.getSelectedTool() == EditTool.ARROW ) {
                AppManager.setSelectedTool( EditTool.ERASER );
            } else {
                AppManager.setSelectedTool( EditTool.ARROW );
            }
            refreshScreen();
        }

        private void menuHiddenEditLyric_Click( Object sender, BEventArgs e ) {
#if JAVA
            boolean input_enabled = AppManager.inputTextBox.isVisible();
#else
            boolean input_enabled = AppManager.inputTextBox.Enabled;
#endif
            if ( !input_enabled && AppManager.getSelectedEventCount() > 0 ) {
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                int clock = original.Clock;
                int note = original.ID.Note;
                Point pos = new Point( AppManager.xCoordFromClocks( clock ), yCoordFromNote( note ) );
                if ( !AppManager.editorConfig.KeepLyricInputMode ) {
                    m_last_symbol_edit_mode = false;
                }
                showInputTextBox( original.ID.LyricHandle.L0.Phrase,
                                  original.ID.LyricHandle.L0.getPhoneticSymbol(),
                                  pos, m_last_symbol_edit_mode );
            } else if ( input_enabled ) {
                TagLyricTextBox tltb = (TagLyricTextBox)AppManager.inputTextBox.getTag();
                if ( tltb.isPhoneticSymbolEditMode() ) {
                    flipInputTextBoxMode();
                }
            }
        }
        #endregion

        #region cMenuTrackTab
        private void cMenuTrackTabCopy_Click( Object sender, BEventArgs e ) {
            copyTrackCore();
        }

        private void cMenuTrackTabChangeName_Click( Object sender, BEventArgs e ) {
            changeTrackNameCore();
        }

        private void cMenuTrackTabTrackOn_Click( Object sender, BEventArgs e ) {
            cMenuTrackTabTrackOn.setSelected( !cMenuTrackTabTrackOn.isSelected() );
            CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandTrackChangePlayMode( AppManager.getSelected(),
                                                                                                      cMenuTrackTabTrackOn.isSelected() ? 1 : -1 ) );
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
            setEdited( true );
            refreshScreen();
        }

        private void cMenuTrackTabDelete_Click( Object sender, BEventArgs e ) {
            deleteTrackCore();
        }

        private void cMenuTrackTabAdd_Click( Object sender, BEventArgs e ) {
            addTrackCore();
        }

        private void cMenuTrackTab_Opening( Object sender, BCancelEventArgs e ) {
            updateTrackMenuStatus();
        }

        private void updateTrackMenuStatus() {
            int tracks = AppManager.getVsqFile().Track.size();
            cMenuTrackTabDelete.setEnabled( tracks >= 3 );
            menuTrackDelete.setEnabled( tracks >= 3 );
            cMenuTrackTabAdd.setEnabled( tracks <= 16 );
            menuTrackAdd.setEnabled( tracks <= 16 );
            cMenuTrackTabCopy.setEnabled( tracks <= 16 );
            menuTrackCopy.setEnabled( tracks <= 16 );
            boolean on = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().PlayMode >= 0;
            cMenuTrackTabTrackOn.setSelected( on );
            menuTrackOn.setSelected( on );
            if ( AppManager.getVsqFile().Track.size() > 2 ) {
                cMenuTrackTabOverlay.setEnabled( true );
                menuTrackOverlay.setEnabled( true );
                cMenuTrackTabOverlay.setSelected( AppManager.isOverlay() );
                menuTrackOverlay.setSelected( AppManager.isOverlay() );
            } else {
                cMenuTrackTabOverlay.setEnabled( false );
                menuTrackOverlay.setEnabled( false );
                cMenuTrackTabOverlay.setSelected( false );
                menuTrackOverlay.setSelected( false );
            }
            cMenuTrackTabRenderCurrent.setEnabled( !AppManager.isPlaying() );
            menuTrackRenderCurrent.setEnabled( !AppManager.isPlaying() );
            cMenuTrackTabRenderAll.setEnabled( !AppManager.isPlaying() );
            menuTrackRenderAll.setEnabled( !AppManager.isPlaying() );
            cMenuTrackTabRendererVOCALOID1.setSelected( false );
            menuTrackRendererVOCALOID1.setSelected( false );
            cMenuTrackTabRendererVOCALOID2.setSelected( false );
            menuTrackRendererVOCALOID2.setSelected( false );
            cMenuTrackTabRendererUtau.setSelected( false );
            menuTrackRendererUtau.setSelected( false );
            cMenuTrackTabRendererStraight.setSelected( false );
            menuTrackRendererStraight.setSelected( false );

            String version = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
            if ( version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                cMenuTrackTabRendererVOCALOID1.setSelected( true );
                menuTrackRendererVOCALOID1.setSelected( true );
            } else if ( version.StartsWith( VSTiProxy.RENDERER_DSB3 ) ) {
                cMenuTrackTabRendererVOCALOID2.setSelected( true );
                menuTrackRendererVOCALOID2.setSelected( true );
            } else if ( version.StartsWith( VSTiProxy.RENDERER_UTU0 ) ) {
                cMenuTrackTabRendererUtau.setSelected( true );
                menuTrackRendererUtau.setSelected( true );
            } else if ( version.StartsWith( VSTiProxy.RENDERER_STR0 ) ) {
                cMenuTrackTabRendererStraight.setSelected( true );
                menuTrackRendererStraight.setSelected( true );
            }
        }

        private void cMenuTrackTabOverlay_Click( Object sender, BEventArgs e ) {
            AppManager.setOverlay( !AppManager.isOverlay() );
            refreshScreen();
        }

        private void cMenuTrackTabRenderCurrent_Click( Object sender, BEventArgs e ) {
            render( new int[] { AppManager.getSelected() } );
        }

        private void cMenuTrackTabRenderer_DropDownOpening( Object sender, BEventArgs e ) {
            updateRendererMenu();
        }
        #endregion

        #region m_txtbox_track_name
        private void m_txtbox_track_name_KeyUp( Object sender, BKeyEventArgs e ) {
#if JAVA
            if ( e.KeyValue == KeyEvent.VK_ENTER ){
#else
            if ( e.KeyCode == System.Windows.Forms.Keys.Enter ) {
#endif
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandTrackChangeName( AppManager.getSelected(), m_txtbox_track_name.getText() ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                setEdited( true );
                m_txtbox_track_name.Dispose();
                m_txtbox_track_name = null;
                refreshScreen();
#if JAVA
            } else if ( e.KeyValue == KeyEvent.VK_ESCAPE ) {
#else
            } else if ( e.KeyCode == System.Windows.Forms.Keys.Escape ) {
#endif
                m_txtbox_track_name.Dispose();
                m_txtbox_track_name = null;
            }
        }
        #endregion

        #region cMenuTrackSelector
        private void cMenuTrackSelector_Opening( Object sender, BCancelEventArgs e ) {
            updateCopyAndPasteButtonStatus();

            // 選択ツールの状態に合わせて表示を更新
            cMenuTrackSelectorPointer.setSelected( false );
            cMenuTrackSelectorPencil.setSelected( false );
            cMenuTrackSelectorLine.setSelected( false );
            cMenuTrackSelectorEraser.setSelected( false );
            EditTool tool = AppManager.getSelectedTool();
            if ( tool == EditTool.ARROW ) {
                cMenuTrackSelectorPointer.setSelected( true );
            } else if ( tool == EditTool.PENCIL ) {
                cMenuTrackSelectorPencil.setSelected( true );
            } else if ( tool == EditTool.LINE ) {
                cMenuTrackSelectorLine.setSelected( true );
            } else if ( tool == EditTool.ERASER ) {
                cMenuTrackSelectorEraser.setSelected( true );
            }
            cMenuTrackSelectorCurve.setSelected( AppManager.isCurveMode() );
        }

        private void cMenuTrackSelectorPointer_Click( Object sender, BEventArgs e ) {
            AppManager.setSelectedTool( EditTool.ARROW );
            refreshScreen();
        }

        private void cMenuTrackSelectorPencil_Click( Object sender, BEventArgs e ) {
            AppManager.setSelectedTool( EditTool.PENCIL );
            refreshScreen();
        }

        private void cMenuTrackSelectorLine_Click( Object sender, BEventArgs e ) {
            AppManager.setSelectedTool( EditTool.LINE );
        }

        private void cMenuTrackSelectorEraser_Click( Object sender, BEventArgs e ) {
            AppManager.setSelectedTool( EditTool.ERASER );
        }

        private void cMenuTrackSelectorCurve_Click( Object sender, BEventArgs e ) {
            AppManager.setCurveMode( !AppManager.isCurveMode() );
        }

        private void cMenuTrackSelectorSelectAll_Click( Object sender, BEventArgs e ) {
            selectAllEvent();
        }

        private void cMenuTrackSelectorCut_Click( Object sender, BEventArgs e ) {
            cutEvent();
        }

        private void cMenuTrackSelectorCopy_Click( Object sender, BEventArgs e ) {
            copyEvent();
        }

        private void cMenuTrackSelectorDelete_Click( Object sender, BEventArgs e ) {
            deleteEvent();
        }

        private void cMenuTrackSelectorDeleteBezier_Click( Object sender, BEventArgs e ) {
            for ( Iterator itr = AppManager.getSelectedBezierEnumerator(); itr.hasNext(); ) {
                SelectedBezierPoint sbp = (SelectedBezierPoint)itr.next();
                int chain_id = sbp.chainID;
                int point_id = sbp.pointID;
                BezierChain chain = (BezierChain)AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).getBezierChain( trackSelector.getSelectedCurve(), chain_id ).Clone();
                int index = -1;
                for ( int i = 0; i < chain.points.size(); i++ ) {
                    if ( chain.points.get( i ).getID() == point_id ) {
                        index = i;
                        break;
                    }
                }
                if ( index >= 0 ) {
                    chain.points.removeElementAt( index );
                    if ( chain.points.size() == 0 ) {
                        CadenciiCommand run = VsqFileEx.generateCommandDeleteBezierChain( AppManager.getSelected(),
                                                                                   trackSelector.getSelectedCurve(),
                                                                                   chain_id,
                                                                                   AppManager.editorConfig.ControlCurveResolution.getValue() );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    } else {
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( AppManager.getSelected(),
                                                                                    trackSelector.getSelectedCurve(),
                                                                                    chain_id,
                                                                                    chain,
                                                                                    AppManager.editorConfig.ControlCurveResolution.getValue() );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    }
                    setEdited( true );
                    refreshScreen();
                    break;
                }
            }
        }

        private void cMenuTrackSelectorPaste_Click( Object sender, BEventArgs e ) {
            pasteEvent();
        }

        private void cMenuTrackSelectorUndo_Click( Object sender, BEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "cMenuTrackSelectorUndo_Click" );
#endif
            undo();
            refreshScreen();
        }

        private void cMenuTrackSelectorRedo_Click( Object sender, BEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "cMenuTrackSelectorRedo_Click" );
#endif
            redo();
            refreshScreen();
        }
        #endregion

        private void pictureBox3_MouseDown( Object sender, BMouseEventArgs e ) {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
        }

        private void pictureBox2_MouseDown( Object sender, BMouseEventArgs e ) {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
        }

        private void menuStrip1_MouseDown( Object sender, BMouseEventArgs e ) {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
        }

        private void timer_Tick( Object sender, BEventArgs e ) {
            float play_time = -1.0f;
            if ( AppManager.rendererAvailable ) {
                // レンダリング用VSTiが利用可能な状態でAppManager_PreviewStartedした場合
                if ( !AppManager.firstBufferWritten ) {
                    return;
                }
                play_time = VSTiProxy.getPlayTime();
            } else {
                play_time = (float)(PortUtil.getCurrentTime() - AppManager.previewStartedTime);
            }
            if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                play_time = play_time * AppManager.editorConfig.RealtimeInputSpeed;
            }
            float now = (float)(play_time + m_direct_play_shift);

            if ( (play_time < 0.0 || m_preview_ending_time < now) && AppManager.getEditMode() != EditMode.REALTIME ) {
                AppManager.setPlaying( false );
                timer.stop();
                if ( AppManager.startMarkerEnabled ) {
                    AppManager.setCurrentClock( AppManager.startMarker );
                }
                ensureCursorVisible();
                return;
            }
            int clock = (int)AppManager.getVsqFile().getClockFromSec( now );
            if ( clock > hScroll.getMaximum() ) {
                if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                    hScroll.setMaximum( clock + (int)((pictPianoRoll.getWidth() - AppManager.keyWidth) / 2.0f / AppManager.scaleX) );
                } else {
                    //AppManager.CurrentClock = 0;
                    //EnsureCursorVisible();
                    if ( !AppManager.isRepeatMode() ) {
                        timer.stop();
                        AppManager.setPlaying( false );
                    }
                }
            } else if ( AppManager.endMarkerEnabled && clock > (int)AppManager.endMarker && AppManager.getEditMode() != EditMode.REALTIME ) {
                AppManager.setCurrentClock( (AppManager.startMarkerEnabled) ? AppManager.startMarker : 0 );
                ensureCursorVisible();
                AppManager.setPlaying( false );
                if ( AppManager.isRepeatMode() ) {
                    AppManager.setPlaying( true );
                } else {
                    timer.stop();
                }
            } else {
                AppManager.setCurrentClock( (int)clock );
                if ( AppManager.autoScroll ) {
                    if ( AppManager.editorConfig.CursorFixed ) {
                        float f_draft = clock - (pictPianoRoll.getWidth() / 2 + 34 - 70) / AppManager.scaleX;
                        if ( f_draft < 0f ) {
                            f_draft = 0;
                        }
                        int draft = (int)(f_draft);
                        if ( draft < hScroll.getMinimum() ) {
                            draft = hScroll.getMinimum();
                        } else if ( hScroll.getMaximum() < draft ) {
                            draft = hScroll.getMaximum();
                        }
                        if ( hScroll.getValue() != draft ) {
                            hScroll.setValue( draft );
                        }
                    } else {
                        ensureCursorVisible();
                    }
                }
            }
            refreshScreen();
        }

        private void bgWorkScreen_DoWork( Object sender, BDoWorkEventArgs e ) {
            try {
                this.Invoke( new EventHandler( this.refreshScreenCore ) );
            } catch ( Exception ex ) {
            }
        }

        private void toolStripEdit_Move( Object sender, BEventArgs e ) {
            AppManager.editorConfig.ToolEditTool.Location = new XmlPoint( toolStripTool.Location.X, toolStripTool.Location.Y );
        }

        private void toolStripEdit_ParentChanged( Object sender, BEventArgs e ) {
            if ( toolStripTool.Parent != null ) {
                if ( toolStripTool.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    AppManager.editorConfig.ToolEditTool.Parent = ToolStripLocation.ParentPanel.Top;
                } else {
                    AppManager.editorConfig.ToolEditTool.Parent = ToolStripLocation.ParentPanel.Bottom;
                }
            }
        }

        private void toolStripPosition_Move( Object sender, BEventArgs e ) {
            AppManager.editorConfig.ToolPositionLocation.Location = new XmlPoint( toolStripPosition.Location.X, toolStripPosition.Location.Y );
        }

        private void toolStripPosition_ParentChanged( Object sender, BEventArgs e ) {
            if ( toolStripPosition.Parent != null ) {
                if ( toolStripPosition.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    AppManager.editorConfig.ToolPositionLocation.Parent = ToolStripLocation.ParentPanel.Top;
                } else {
                    AppManager.editorConfig.ToolPositionLocation.Parent = ToolStripLocation.ParentPanel.Bottom;
                }
            }
        }

        private void toolStripMeasure_Move( Object sender, BEventArgs e ) {
            AppManager.editorConfig.ToolMeasureLocation.Location = new XmlPoint( toolStripMeasure.Location.X, toolStripMeasure.Location.Y );
        }

        private void toolStripMeasure_ParentChanged( Object sender, BEventArgs e ) {
            if ( toolStripMeasure.Parent != null ) {
                if ( toolStripMeasure.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    AppManager.editorConfig.ToolMeasureLocation.Parent = ToolStripLocation.ParentPanel.Top;
                } else {
                    AppManager.editorConfig.ToolMeasureLocation.Parent = ToolStripLocation.ParentPanel.Bottom;
                }
            }
        }

        void toolStripFile_Move( Object sender, BEventArgs e ) {
            AppManager.editorConfig.ToolFileLocation.Location = new XmlPoint( toolStripFile.Location.X, toolStripFile.Location.Y );
        }

        void toolStripFile_ParentChanged( Object sender, BEventArgs e ) {
            if ( toolStripFile.Parent != null ) {
                if ( toolStripFile.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    AppManager.editorConfig.ToolFileLocation.Parent = ToolStripLocation.ParentPanel.Top;
                } else {
                    AppManager.editorConfig.ToolFileLocation.Parent = ToolStripLocation.ParentPanel.Bottom;
                }
            }
        }

        #region stripBtn*
        private void stripBtnGrid_CheckedChanged( Object sender, BEventArgs e ) {
            AppManager.setGridVisible( stripBtnGrid.isSelected() );
        }

        private void stripBtnArrow_Click( Object sender, BEventArgs e ) {
            AppManager.setSelectedTool( EditTool.ARROW );
        }

        private void stripBtnPencil_Click( Object sender, BEventArgs e ) {
            AppManager.setSelectedTool( EditTool.PENCIL );
        }

        private void stripBtnLine_Click( Object sender, BEventArgs e ) {
            AppManager.setSelectedTool( EditTool.LINE );
        }

        private void stripBtnEraser_Click( Object sender, BEventArgs e ) {
            AppManager.setSelectedTool( EditTool.ERASER );
        }

        private void stripBtnCurve_Click( Object sender, BEventArgs e ) {
            AppManager.setCurveMode( !AppManager.isCurveMode() );
        }

        private void stripBtnPlay_Click( Object sender, BEventArgs e ) {
            if ( !AppManager.isPlaying() ) {
                AppManager.setPlaying( true );
            }
            pictPianoRoll.requestFocus();
        }

        private void stripBtnScroll_Click( Object sender, BEventArgs e ) {
            stripBtnScroll.setSelected(  !stripBtnScroll.isSelected() );
            AppManager.autoScroll = stripBtnScroll.isSelected();
            pictPianoRoll.requestFocus();
        }

        private void stripBtnLoop_Click( Object sender, BEventArgs e ) {
            stripBtnLoop.setSelected( !stripBtnLoop.isSelected() );
            AppManager.setRepeatMode( stripBtnLoop.isSelected() );
            pictPianoRoll.requestFocus();
        }

        private void stripBtnStop_Click( Object sender, BEventArgs e ) {
            AppManager.setPlaying( false );
            timer.stop();
            pictPianoRoll.requestFocus();
        }

        private void handleStartMarker_Click( Object sender, BEventArgs e ) {
            AppManager.startMarkerEnabled = !AppManager.startMarkerEnabled;
            stripBtnStartMarker.setSelected( AppManager.startMarkerEnabled );
            menuVisualStartMarker.setSelected( AppManager.startMarkerEnabled );
            pictPianoRoll.requestFocus();
            refreshScreen();
        }

        private void handleEndMarker_Click( Object sender, BEventArgs e ) {
            AppManager.endMarkerEnabled = !AppManager.endMarkerEnabled;
            stripBtnEndMarker.setSelected( AppManager.endMarkerEnabled );
            menuVisualEndMarker.setSelected( AppManager.endMarkerEnabled );
            pictPianoRoll.requestFocus();
            refreshScreen();
        }

        private void stripBtnMoveEnd_Click( Object sender, BEventArgs e ) {
            if ( AppManager.isPlaying() ) {
                AppManager.setPlaying( false );
            }
            AppManager.setCurrentClock( AppManager.getVsqFile().TotalClocks );
            ensureCursorVisible();
            refreshScreen();
        }

        private void stripBtnMoveTop_Click( Object sender, BEventArgs e ) {
            if ( AppManager.isPlaying() ) {
                AppManager.setPlaying( false );
            }
            AppManager.setCurrentClock( 0 );
            ensureCursorVisible();
            refreshScreen();
        }

        private void stripBtnRewind_Click( Object sender, BEventArgs e ) {
            rewind();
        }

        private void stripBtnForward_Click( Object sender, BEventArgs e ) {
            forward();
        }
        #endregion

        private void commonCaptureSpaceKeyDown( Object sender, BKeyEventArgs e ) {
#if JAVA
            if ( (e.KeyValue & KeyEvent.VK_SPACE) == KeyEvent.VK_SPACE ) {
#else
            if ( (e.KeyCode & System.Windows.Forms.Keys.Space) == System.Windows.Forms.Keys.Space ) {
#endif
                m_spacekey_downed = true;
            }
        }

        private void commonCaptureSpaceKeyUp( Object sender, BKeyEventArgs e ) {
#if JAVA
            if ( (e.KeyValue & KeyEvent.VK_SPACE) == KeyEvent.VK_SPACE ) {
#else
            if ( (e.KeyCode & System.Windows.Forms.Keys.Space) == System.Windows.Forms.Keys.Space ) {
#endif
                m_spacekey_downed = false;
            }
        }

        private void commonRendererVOCALOID1_Click( Object sender, BEventArgs e ) {
            String old = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
            if ( !old.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                VsqTrack item = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).clone();
                Vector<VsqID> singers = new Vector<VsqID>();
                SingerConfig[] configs = VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID1 );
                for ( int i = 0; i < configs.Length; i++ ) {
                    SingerConfig sc = configs[i];
                    singers.add( VocaloSysUtil.getSingerID( sc.VOICENAME, SynthesizerType.VOCALOID1 ) );
                }
                item.changeRenderer( "DSB202", singers );
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(),
                                                                             item,
                                                                             AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedStart( 0 );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedEnd( AppManager.getVsqFile().TotalClocks );
                cMenuTrackTabRendererVOCALOID1.setSelected( true );
                cMenuTrackTabRendererVOCALOID2.setSelected( false );
                cMenuTrackTabRendererUtau.setSelected( false );
                cMenuTrackTabRendererStraight.setSelected( false );
                menuTrackRendererVOCALOID1.setSelected( true );
                menuTrackRendererVOCALOID2.setSelected( false );
                menuTrackRendererUtau.setSelected( false );
                menuTrackRendererStraight.setSelected( false );
                setEdited( true );
                refreshScreen();
            }
        }

        private void commonRendererVOCALOID2_Click( Object sender, BEventArgs e ) {
            String old = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
            if ( !old.StartsWith( VSTiProxy.RENDERER_DSB3 ) ) {
                VsqTrack item = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).clone();
                Vector<VsqID> singers = new Vector<VsqID>();
                SingerConfig[] configs = VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID2 );
                for ( int i = 0; i < configs.Length; i++ ) {
                    SingerConfig sc = configs[i];
                    singers.add( VocaloSysUtil.getSingerID( sc.VOICENAME, SynthesizerType.VOCALOID2 ) );
                }
                item.changeRenderer( "DSB301", singers );
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(),
                                                                             item,
                                                                             AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedStart( 0 );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedEnd( AppManager.getVsqFile().TotalClocks );
                cMenuTrackTabRendererVOCALOID1.setSelected( false );
                cMenuTrackTabRendererVOCALOID2.setSelected( true );
                cMenuTrackTabRendererUtau.setSelected( false );
                cMenuTrackTabRendererStraight.setSelected( false );
                menuTrackRendererVOCALOID1.setSelected( false );
                menuTrackRendererVOCALOID2.setSelected( true );
                menuTrackRendererUtau.setSelected( false );
                menuTrackRendererStraight.setSelected( false );
                setEdited( true );
                refreshScreen();
            }
        }

        private void commonRendererUtau_Click( Object sender, BEventArgs e ) {
            String old = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
            if ( !old.StartsWith( VSTiProxy.RENDERER_UTU0 ) ) {
                VsqTrack item = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).clone();
                Vector<SingerConfig> list = AppManager.editorConfig.UtauSingers;
                Vector<VsqID> singers = new Vector<VsqID>();
                for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                    SingerConfig sc = (SingerConfig)itr.next();
                    singers.add( AppManager.getSingerIDUtau( sc.VOICENAME ) );
                }
                item.changeRenderer( "UTU000", singers );
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(),
                                                                             item,
                                                                             AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedStart( 0 );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedEnd( AppManager.getVsqFile().TotalClocks );
                cMenuTrackTabRendererVOCALOID1.setSelected( false );
                cMenuTrackTabRendererVOCALOID2.setSelected( false );
                cMenuTrackTabRendererUtau.setSelected( true );
                cMenuTrackTabRendererStraight.setSelected( false );
                menuTrackRendererVOCALOID1.setSelected( false );
                menuTrackRendererVOCALOID2.setSelected( false );
                menuTrackRendererUtau.setSelected( true );
                menuTrackRendererStraight.setSelected( false );
                setEdited( true );
                refreshScreen();
            }
        }

        private void commonRendererStraight_Click( Object sender, BEventArgs e ) {
            String old = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
            if ( !old.StartsWith( VSTiProxy.RENDERER_STR0 ) ) {
                VsqTrack item = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).clone();
                Vector<SingerConfig> list = AppManager.editorConfig.UtauSingers;
                Vector<VsqID> singers = new Vector<VsqID>();
                for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                    SingerConfig sc = (SingerConfig)itr.next();
                    singers.add( AppManager.getSingerIDUtau( sc.VOICENAME ) );
                }
                item.changeRenderer( "STR000", singers );
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(),
                                                                             item,
                                                                             AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedStart( 0 );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedEnd( AppManager.getVsqFile().TotalClocks );
                cMenuTrackTabRendererVOCALOID1.setSelected( false );
                cMenuTrackTabRendererVOCALOID2.setSelected( false );
                cMenuTrackTabRendererUtau.setSelected( false );
                cMenuTrackTabRendererStraight.setSelected( true );
                menuTrackRendererVOCALOID1.setSelected( false );
                menuTrackRendererVOCALOID2.setSelected( false );
                menuTrackRendererUtau.setSelected( false );
                menuTrackRendererStraight.setSelected( true );
                setEdited( true );
                refreshScreen();
            }
        }

        private void toolStripContainer_TopToolStripPanel_SizeChanged( Object sender, BEventArgs e ) {
            if ( getExtendedState() == BForm.ICONIFIED ) {
                return;
            }
            Dimension minsize = getWindowMinimumSize();
            int wid = getWidth();
            int hei = getHeight();
            boolean change_size_required = false;
            if ( minsize.width > wid ) {
                wid = minsize.width;
                change_size_required = true;
            }
            if ( minsize.height > hei ) {
                hei = minsize.height;
                change_size_required = true;
            }
            setMinimumSize( getWindowMinimumSize() );
            if ( change_size_required ) {
                setSize( wid, hei );
            }
        }

        private void stripDDBtnSpeed_DropDownOpening( Object sender, BEventArgs e ) {
            if ( AppManager.editorConfig.RealtimeInputSpeed == 1.0f ) {
                stripDDBtnSpeed100.setSelected( true );
                stripDDBtnSpeed050.setSelected( false );
                stripDDBtnSpeed033.setSelected( false );
                stripDDBtnSpeedTextbox.setText( "100" );
            } else if ( AppManager.editorConfig.RealtimeInputSpeed == 0.5f ) {
                stripDDBtnSpeed100.setSelected( false );
                stripDDBtnSpeed050.setSelected( true );
                stripDDBtnSpeed033.setSelected( false );
                stripDDBtnSpeedTextbox.setText( "50" );
            } else if ( AppManager.editorConfig.RealtimeInputSpeed == 1.0f / 3.0f ) {
                stripDDBtnSpeed100.setSelected( false );
                stripDDBtnSpeed050.setSelected( false );
                stripDDBtnSpeed033.setSelected( true );
                stripDDBtnSpeedTextbox.setText( "33.333" );
            } else {
                stripDDBtnSpeed100.setSelected( false );
                stripDDBtnSpeed050.setSelected( false );
                stripDDBtnSpeed033.setSelected( false );
                stripDDBtnSpeedTextbox.setText( (AppManager.editorConfig.RealtimeInputSpeed * 100.0f).ToString() );
            }
        }

        private void stripDDBtnSpeed100_Click( Object sender, BEventArgs e ) {
            changeRealtimeInputSpeed( 1.0f );
            AppManager.editorConfig.RealtimeInputSpeed = 1.0f;
            updateStripDDBtnSpeed();
        }

        private void stripDDBtnSpeed050_Click( Object sender, BEventArgs e ) {
            changeRealtimeInputSpeed( 0.5f );
            AppManager.editorConfig.RealtimeInputSpeed = 0.5f;
            updateStripDDBtnSpeed();
        }

        private void stripDDBtnSpeed033_Click( Object sender, BEventArgs e ) {
            changeRealtimeInputSpeed( 1.0f / 3.0f );
            AppManager.editorConfig.RealtimeInputSpeed = 1.0f / 3.0f;
            updateStripDDBtnSpeed();
        }

        private void stripDDBtnSpeedTextbox_KeyDown( Object sender, BKeyEventArgs e ) {
#if JAVA
            if ( e.KeyValue == KeyEvent.VK_ENTER ) {
#else
            if ( e.KeyCode == System.Windows.Forms.Keys.Enter ) {
#endif
                float v;
                try {
                    v = PortUtil.parseFloat( stripDDBtnSpeedTextbox.getText() );
                    changeRealtimeInputSpeed( v / 100.0f );
                    AppManager.editorConfig.RealtimeInputSpeed = v / 100.0f;
#if JAVA
                    //TODO: FormMain#stripDDBtnSpeedTextBox_KeyDown
#else
                    stripDDBtnSpeed.HideDropDown();
#endif
                    updateStripDDBtnSpeed();
                } catch ( Exception ex ) {
                }
            }
        }

        public void changeRealtimeInputSpeed( float newv ) {
            float old = AppManager.editorConfig.RealtimeInputSpeed;
            double now = PortUtil.getCurrentTime();
            float play_time = (float)(now - AppManager.previewStartedTime) * old / newv;
            int sec = (int)(Math.Floor( play_time ) + 0.1);
            int millisec = (int)((play_time - sec) * 1000);
            AppManager.previewStartedTime = now - (sec + millisec / 1000.0);
#if ENABLE_MIDI
            MidiPlayer.SetSpeed( newv, AppManager.previewStartedTime );
#endif
        }

        /// <summary>
        /// stripDDBtnSpeedの表示状態を更新します
        /// </summary>
        private void updateStripDDBtnSpeed() {
            stripDDBtnSpeed.setText( _( "Speed" ) + " " + (AppManager.editorConfig.RealtimeInputSpeed * 100) + "%" );
        }

        private void menuSetting_DropDownOpening( Object sender, BEventArgs e ) {
            menuSettingMidi.setEnabled( AppManager.getEditMode() != EditMode.REALTIME );
        }

        private void menuVisualProperty_Click( Object sender, BEventArgs e ) {
#if ENABLE_PROPERTY
            if ( menuVisualProperty.isSelected() ) {
                if ( AppManager.editorConfig.PropertyWindowStatus.WindowState == BFormWindowState.Minimized ) {
                    updatePropertyPanelState( PanelState.Docked );
                } else {
                    updatePropertyPanelState( PanelState.Window );
                }
            } else {
                updatePropertyPanelState( PanelState.Hidden );
            }
#endif
        }

        private void menuSettingUtauVoiceDB_Click( Object sender, BEventArgs e ) {
            String edit_oto_ini = PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "EditOtoIni.exe" );
            if ( !PortUtil.isFileExists( edit_oto_ini ) ) {
                return;
            }

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Environment.GetEnvironmentVariable( "ComSpec" );
            psi.Arguments = "/c \"" + edit_oto_ini + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start( psi );
        }

        private void menuVisualOverview_CheckedChanged( Object sender, BEventArgs e ) {
            AppManager.editorConfig.OverviewEnabled = menuVisualOverview.isSelected();
            updateLayout();
        }

        private void pictOverview_MouseMove( Object sender, BMouseEventArgs e ) {
            if ( m_overview_mouse_down_mode == OverviewMouseDownMode.LEFT ) {
                int draft = getOverviewStartToDrawX( e.X );
                if ( draft < 0 ) {
                    draft = 0;
                }
                AppManager.startToDrawX = draft;
                refreshScreen();
            } else if ( m_overview_mouse_down_mode == OverviewMouseDownMode.MIDDLE ) {
                int dx = e.X - m_overview_mouse_downed_locationx;
                int draft = m_overview_start_to_draw_clock_initial_value - (int)(dx / m_overview_px_per_clock);
                int clock = getOverviewClockFromXCoord( pictOverview.getWidth(), draft );
                if ( AppManager.getVsqFile().TotalClocks < clock ) {
                    draft = AppManager.getVsqFile().TotalClocks - (int)(pictOverview.getWidth() / m_overview_px_per_clock);
                }
                if ( draft < 0 ) {
                    draft = 0;
                }
                m_overview_start_to_draw_clock = draft;
                refreshScreen();
            }
        }

        private int getOverviewStartToDrawX( int mouse_x ) {
            float clock = mouse_x / m_overview_px_per_clock + m_overview_start_to_draw_clock;
            int clock_at_left = (int)(clock - (pictPianoRoll.getWidth() - AppManager.keyWidth) / AppManager.scaleX / 2);
            return (int)(clock_at_left * AppManager.scaleX);
        }

        private void pictOverview_MouseDown( Object sender, BMouseEventArgs e ) {
            BMouseButtons btn = e.Button;
            if ( AppManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier && m_spacekey_downed && e.Button == BMouseButtons.Left ) {
                btn = BMouseButtons.Middle;
            }
            if ( btn == BMouseButtons.Middle ) {
                m_overview_mouse_down_mode = OverviewMouseDownMode.MIDDLE;
                m_overview_mouse_downed_locationx = e.X;
                m_overview_start_to_draw_clock_initial_value = m_overview_start_to_draw_clock;
            } else if ( e.Button == BMouseButtons.Left ) {
                if ( e.Clicks == 1 ) {
                    m_overview_mouse_down_mode = OverviewMouseDownMode.LEFT;
                    int draft = getOverviewStartToDrawX( e.X );
                    if ( draft < 0 ) {
                        draft = 0;
                    }
                    AppManager.startToDrawX = draft;
                    refreshScreen();
                }
            }
        }

        private void pictOverview_MouseUp( Object sender, BMouseEventArgs e ) {
            if ( m_overview_mouse_down_mode == OverviewMouseDownMode.LEFT ) {
                AppManager.startToDrawX = (int)(hScroll.getValue() * AppManager.scaleX);
            }
            m_overview_mouse_down_mode = OverviewMouseDownMode.NONE;
            refreshScreen();
        }

        private void pictOverview_Paint( Object sender, BPaintEventArgs e ) {
            Graphics2D g = new bocoree.java.awt.Graphics2D( e.Graphics );
            int count = 0;
            int sum = 0;
            int height = pictOverview.getHeight();
            BasicStroke pen = new java.awt.BasicStroke( _OVERVIEW_DOT_DIAM );
            g.setStroke( pen );
            g.setColor( s_note_fill );
            //using ( Pen pen = new Pen( s_note_fill, _OVERVIEW_DOT_DIAM ) ) {
            for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                int x = getOverviewXCoordFromClock( item.Clock );
                if ( x < 0 ) {
                    continue;
                }
                if ( pictOverview.getWidth() < x ) {
                    break;
                }
                count++;
                sum += item.ID.Note;
                int y = height - (height / 2 + (int)((item.ID.Note - m_overview_average_note) * _OVERVIEW_DOT_DIAM));
                int length = (int)(item.ID.getLength() * m_overview_px_per_clock);
                if ( length < _OVERVIEW_DOT_DIAM ) {
                    length = _OVERVIEW_DOT_DIAM;
                }
                g.drawLine( x, y, x + length, y );
            }
            g.setStroke( new BasicStroke() );
            //}
            int current_start = AppManager.clockFromXCoord( AppManager.keyWidth );
            int current_end = AppManager.clockFromXCoord( pictPianoRoll.getWidth() );
            int x_start = getOverviewXCoordFromClock( current_start );
            int x_end = getOverviewXCoordFromClock( current_end );

            // 小節ごとの線
            int clock_start = getOverviewClockFromXCoord( 0 );
            int clock_end = getOverviewClockFromXCoord( pictOverview.getWidth() );
            int premeasure = AppManager.getVsqFile().getPreMeasure();
            g.setClip( null );
            BasicStroke pen_bold = new bocoree.java.awt.BasicStroke( 2 );
            Color pen_color = new bocoree.java.awt.Color( 0, 0, 0, 130 );

            int barcountx = 0;
            String barcountstr = "";
            for ( Iterator itr = AppManager.getVsqFile().getBarLineIterator( clock_end * 3 / 2 ); itr.hasNext(); ) {
                VsqBarLineType bar = (VsqBarLineType)itr.next();
                if ( bar.clock() < clock_start ) {
                    continue;
                }
                if ( pictOverview.getWidth() < barcountx ) {
                    break;
                }
                if ( bar.isSeparator() ) {
                    int barcount = bar.getBarCount() - premeasure + 1;
                    int x = getOverviewXCoordFromClock( bar.clock() );
                    if ( (barcount % 5 == 0 && barcount > 0) || barcount == 1 ) {
                        g.setColor( pen_color );
                        g.setStroke( pen_bold );
                        g.drawLine( x, 0, x, pictOverview.getHeight() );

                        g.setStroke( new BasicStroke() );
                        if ( !barcountstr.Equals( "" ) ) {
                            g.setColor( Color.white );
                            g.setFont( AppManager.baseFont9 );
                            g.drawString( barcountstr, barcountx + 1, 1 );
                        }
                        barcountstr = barcount + "";
                        barcountx = x;
                    } else {
                        g.setColor( pen_color );
                        g.drawLine( x, 0, x, pictOverview.getHeight() );
                    }
                }
            }
            g.setClip( null );

            // 移動中している最中に，移動開始直前の部分を影付で表示する
            int act_start_to_draw_x = (int)(hScroll.getValue() * AppManager.scaleX);
            if ( act_start_to_draw_x != AppManager.startToDrawX ) {
                int act_start_clock = AppManager.clockFromXCoord( AppManager.keyWidth - AppManager.startToDrawX + act_start_to_draw_x );
                int act_end_clock = AppManager.clockFromXCoord( pictPianoRoll.getWidth() - AppManager.startToDrawX + act_start_to_draw_x );
                int act_start_x = getOverviewXCoordFromClock( act_start_clock );
                int act_end_x = getOverviewXCoordFromClock( act_end_clock );
                Rectangle rcm = new Rectangle( act_start_x, 0, act_end_x - act_start_x, height );
                g.setColor( new Color( 0, 0, 0, 100 ) );
                g.fillRect( rcm.x, rcm.y, rcm.width, rcm.height );
            }

            // 現在の表示範囲
            Rectangle rc = new Rectangle( x_start, 0, x_end - x_start, height - 1 );
            g.setColor( new Color( 255, 255, 255, 50 ) );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( AppManager.getHilightColor() );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            if ( count > 0 ) {
                m_overview_average_note = sum / (float)count;
            }

            // ソングポジション
            int px_current_clock = (int)((AppManager.getCurrentClock() - m_overview_start_to_draw_clock) * m_overview_px_per_clock);
            g.setStroke( new BasicStroke( 2 ) );
            g.setColor( Color.white );
            g.drawLine( px_current_clock, 0, px_current_clock, pictOverview.getHeight() );
            g.setStroke( new BasicStroke() );
        }

        private int getOverviewXCoordFromClock( int clock ) {
            return (int)((clock - m_overview_start_to_draw_clock) * m_overview_px_per_clock);
        }

        private int getOverviewClockFromXCoord( int x, int start_to_draw_clock ) {
            return (int)(x / m_overview_px_per_clock) + start_to_draw_clock;
        }

        private int getOverviewClockFromXCoord( int x ) {
            return getOverviewClockFromXCoord( x, m_overview_start_to_draw_clock );
        }

        private void pictOverview_MouseDoubleClick( Object sender, BMouseEventArgs e ) {
            m_overview_mouse_down_mode = OverviewMouseDownMode.NONE;
            int draft_stdx = getOverviewStartToDrawX( e.X );
            int draft = (int)(draft_stdx / AppManager.scaleX);
            if ( draft < hScroll.getMinimum() ) {
                draft = hScroll.getMinimum();
            } else if ( hScroll.getMaximum() < draft ) {
                draft = hScroll.getMaximum();
            }
            hScroll.setValue( draft );
            refreshScreen();
        }

        private void btnLeft_MouseDown( Object sender, BMouseEventArgs e ) {
            m_overview_btn_downed = PortUtil.getCurrentTime();
            m_overview_start_to_draw_clock_initial_value = m_overview_start_to_draw_clock;
            if ( m_overview_update_thread != null ) {
                try {
                    m_overview_update_thread.Abort();
                    while ( m_overview_update_thread.IsAlive ) {
#if JAVA
                        Thread.sleep( 0 );
#else
                        System.Windows.Forms.Application.DoEvents();
#endif
                    }
                } catch ( Exception ex ) {
                }
                m_overview_update_thread = null;
            }
            m_overview_direction = -1;
            m_overview_update_thread = new Thread( new ThreadStart( updateOverview ) );
            m_overview_update_thread.Start();
        }

        private void btnLeft_MouseUp( Object sender, BMouseEventArgs e ) {
            overviewStopThread();
        }

        private void btnRight_MouseDown( Object sender, BMouseEventArgs e ) {
            m_overview_btn_downed = PortUtil.getCurrentTime();
            m_overview_start_to_draw_clock_initial_value = m_overview_start_to_draw_clock;
            if ( m_overview_update_thread != null ) {
                try {
                    while ( m_overview_update_thread.IsAlive ) {
#if JAVA
                        Thread.sleep( 0 );
#else
                        System.Windows.Forms.Application.DoEvents();
#endif
                    }
                } catch ( Exception ex ) {
                }
                m_overview_update_thread = null;
            }
            m_overview_direction = 1;
            m_overview_update_thread = new Thread( new ThreadStart( updateOverview ) );
            m_overview_update_thread.Start();
        }

        private void btnRight_MouseUp( Object sender, BMouseEventArgs e ) {
            overviewStopThread();
        }

        private void updateOverview() {
            boolean д = true;
            for (;д;) {
#if DEBUG
                PortUtil.println( "updateOverview" );
#endif
                Thread.Sleep( 100 );
                double dt = PortUtil.getCurrentTime() - m_overview_btn_downed;
                int draft = (int)(m_overview_start_to_draw_clock_initial_value + m_overview_direction * dt * _OVERVIEW_SCROLL_SPEED / m_overview_px_per_clock);
                int clock = getOverviewClockFromXCoord( pictOverview.getWidth(), draft );
                if ( AppManager.getVsqFile().TotalClocks < clock ) {
                    draft = AppManager.getVsqFile().TotalClocks - (int)(pictOverview.getWidth() / m_overview_px_per_clock);
                }
                if ( draft < 0 ) {
                    draft = 0;
                }
                m_overview_start_to_draw_clock = draft;
#if JAVA
                if ( this == null ) {
#else
                if ( this == null || (this != null && this.IsDisposed) ) {
#endif
                    break;
                }
                this.Invoke( new BEventHandler( invalidatePictOverview ) );
            }
        }

        private void invalidatePictOverview( Object sender, BEventArgs e ) {
            pictOverview.invalidate();
        }

        private void btnMooz_Click( Object sender, BEventArgs e ) {
            int draft = m_overview_scale_count - 1;
            if ( draft < _OVERVIEW_SCALE_COUNT_MIN ) {
                draft = _OVERVIEW_SCALE_COUNT_MIN;
            }
            m_overview_scale_count = draft;
            m_overview_px_per_clock = getOverviewScaleX( m_overview_scale_count );
            AppManager.editorConfig.OverviewScaleCount = m_overview_scale_count;
            refreshScreen();
        }

        private void btnZoom_Click( Object sender, BEventArgs e ) {
            int draft = m_overview_scale_count + 1;
            if ( _OVERVIEW_SCALE_COUNT_MAX < draft ) {
                draft = _OVERVIEW_SCALE_COUNT_MAX;
            }
            m_overview_scale_count = draft;
            m_overview_px_per_clock = getOverviewScaleX( m_overview_scale_count );
            AppManager.editorConfig.OverviewScaleCount = m_overview_scale_count;
            refreshScreen();
        }

        private float getOverviewScaleX( int scale_count ) {
            return (float)Math.Pow( 10.0, 0.2 * scale_count - 3.0 );
        }

        public void updateBgmMenuState() {
            menuTrackBgm.removeAll();
            int count = AppManager.getBgmCount();
            if ( count > 0 ) {
                for ( int i = 0; i < count; i++ ) {
                    BgmFile item = AppManager.getBgm( i );
                    BMenuItem menu = new BMenuItem();
                    menu.setText( PortUtil.getFileName( item.file ) );
                    menu.setToolTipText( item.file );

                    BMenuItem menu_remove = new BMenuItem();
                    menu_remove.setText( _( "Remove" ) );
                    menu_remove.setToolTipText( item.file );
                    menu_remove.setTag( (int)i );
#if JAVA
                    menu_remove.clickEvent.add( new BEventHandler( this, "handleBgmRemove_Click" ) );
#else
                    menu_remove.Click += new EventHandler( handleBgmRemove_Click );
#endif
                    menu.add( menu_remove );

                    BMenuItem menu_start_after_premeasure = new BMenuItem();
                    menu_start_after_premeasure.setText( _( "Start After Premeasure" ) );
                    menu_start_after_premeasure.setName( "menu_start_after_premeasure" + i );
                    menu_start_after_premeasure.setTag( (int)i );
                    menu_start_after_premeasure.setCheckOnClick( true );
                    menu_start_after_premeasure.setSelected( item.startAfterPremeasure );
                    menu_start_after_premeasure.CheckedChanged += new EventHandler( handleBgmStartAfterPremeasure_CheckedChanged );
                    menu.add( menu_start_after_premeasure );

                    BMenuItem menu_offset_second = new BMenuItem();
                    menu_offset_second.setText( _( "Set Offset Seconds" ) );
                    menu_offset_second.setTag( (int)i );
                    menu_offset_second.setToolTipText( item.readOffsetSeconds + " " + _( "seconds" ) );
#if JAVA
                    menu_offset_second.clickEvent.add( new BEventHandler( this, "handleBgmOffsetSeconds_Click" ) );
#else
                    menu_offset_second.Click += new EventHandler( handleBgmOffsetSeconds_Click );
#endif
                    menu.add( menu_offset_second );

                    menuTrackBgm.add( menu );
                }
                menuTrackBgm.addSeparator();
            }
            BMenuItem menu_add = new BMenuItem();
            menu_add.setText( _( "Add" ) );
#if JAVA
            menu_add.clickEvent.add( new BEventHandler( this, "handleBgmAdd_Click" ) );
#else
            menu_add.Click += new EventHandler( handleBgmAdd_Click );
#endif
            menuTrackBgm.add( menu_add );
        }

        private void handleBgmOffsetSeconds_Click( Object sender, BEventArgs e ) {
            if ( !(sender is BMenuItem) ) {
                return;
            }
            BMenuItem parent = (BMenuItem)sender;
            if ( parent.getTag() == null ) {
                return;
            }
            if ( !(parent.getTag() is int) ) {
                return;
            }
            int index = (int)parent.getTag();
            InputBox ib = null;
            try {
                ib = new InputBox( _( "Input Offset Seconds" ) );
                ib.setLocation( getFormPreferedLocation( ib ) );
                ib.setResult( AppManager.getBgm( index ).readOffsetSeconds + "" );
                if ( ib.showDialog() != BDialogResult.OK ) {
                    return;
                }
                Vector<BgmFile> list = new Vector<BgmFile>();
                int count = AppManager.getBgmCount();
                BgmFile item = null;
                for ( int i = 0; i < count; i++ ) {
                    if ( i == index ) {
                        item = (BgmFile)AppManager.getBgm( i ).clone();
                        list.add( item );
                    } else {
                        list.add( AppManager.getBgm( i ) );
                    }
                }
                double draft;
                try {
                    draft = PortUtil.parseDouble( ib.getResult() );
                    item.readOffsetSeconds = draft;
                    parent.setToolTipText( draft + " " + _( "seconds" ) );
                } catch ( Exception ex3 ) {
                }
                CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate( list );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                setEdited( true );
            } catch ( Exception ex ) {
            } finally {
                if ( ib != null ) {
                    try {
#if !JAVA
                        ib.Dispose();
#endif
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        private void handleBgmStartAfterPremeasure_CheckedChanged( Object sender, BEventArgs e ) {
            if ( !(sender is BMenuItem) ) {
                return;
            }
            BMenuItem parent = (BMenuItem)sender;
            if ( parent.getTag() == null ) {
                return;
            }
            if ( !(parent.getTag() is int) ) {
                return;
            }
            int index = (int)parent.getTag();
            Vector<BgmFile> list = new Vector<BgmFile>();
            int count = AppManager.getBgmCount();
            for ( int i = 0; i < count; i++ ) {
                if ( i == index ) {
                    BgmFile item = (BgmFile)AppManager.getBgm( i ).clone();
                    item.startAfterPremeasure = parent.isSelected();
                    list.add( item );
                } else {
                    list.add( AppManager.getBgm( i ) );
                }
            }
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate( list );
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
            setEdited( true );
        }

        private void handleBgmAdd_Click( Object sender, BEventArgs e ) {
            if ( openWaveDialog.showOpenDialog( this ) != BFileChooser.APPROVE_OPTION ) {
                return;
            }

            String file = openWaveDialog.getSelectedFile();

            // 既に開かれていたらキャンセル
            int count = AppManager.getBgmCount();
            boolean found = false;
            for ( int i = 0; i < count; i++ ) {
                BgmFile item = AppManager.getBgm( i );
                if ( file.Equals( item.file ) ) {
                    found = true;
                    break;
                }
            }
            if ( found ) {
                AppManager.showMessageBox( string.Format( _( "file '{0}' is already registered as BGM." ), file ),
                                 _( "Error" ),
                                 AppManager.MSGBOX_DEFAULT_OPTION,
                                 AppManager.MSGBOX_WARNING_MESSAGE );
                return;
            }

            // 登録
            AppManager.addBgm( file );
            setEdited( true );
            updateBgmMenuState();
        }

        private void handleBgmRemove_Click( Object sender, BEventArgs e ) {
            if ( !(sender is BMenuItem) ) {
                return;
            }
            BMenuItem parent = (BMenuItem)sender;
            if ( parent.getTag() == null ) {
                return;
            }
            if ( !(parent.getTag() is int) ) {
                return;
            }
            int index = (int)parent.getTag();
            BgmFile bgm = AppManager.getBgm( index );
            if ( AppManager.showMessageBox( string.Format( _( "remove '{0}'?" ), bgm.file ),
                                  "Cadencii",
                                  AppManager.MSGBOX_YES_NO_OPTION,
                                  AppManager.MSGBOX_QUESTION_MESSAGE ) != BDialogResult.YES ) {
                return;
            }
            AppManager.removeBgm( index );
            setEdited( true );
            updateBgmMenuState();
        }

#if ENABLE_PROPERTY
        private void updatePropertyPanelState( PanelState state ) {
            if ( state == PanelState.Docked ) {
                m_property_panel_container.Add( AppManager.propertyPanel );
                AppManager.propertyWindow.setVisible( false );
                menuVisualProperty.setSelected( true );
                AppManager.editorConfig.PropertyWindowStatus.State = PanelState.Docked;
                splitContainerProperty.setSplitterFixed( false );
                splitContainerProperty.Panel1MinSize = _PROPERTY_DOCK_MIN_WIDTH;
                splitContainerProperty.setDividerLocation( AppManager.editorConfig.PropertyWindowStatus.DockWidth );
                AppManager.editorConfig.PropertyWindowStatus.WindowState = BFormWindowState.Minimized;
                AppManager.propertyWindow.setExtendedState( BForm.ICONIFIED );
            } else if ( state == PanelState.Hidden ) {
                AppManager.propertyWindow.setVisible( false );
                menuVisualProperty.setSelected( false );
                if ( AppManager.editorConfig.PropertyWindowStatus.State == PanelState.Docked ) {
                    AppManager.editorConfig.PropertyWindowStatus.DockWidth = splitContainerProperty.getDividerLocation();
                }
                AppManager.editorConfig.PropertyWindowStatus.State = PanelState.Hidden;
                splitContainerProperty.Panel1MinSize = 0;
                splitContainerProperty.setDividerLocation( 0 );
                splitContainerProperty.setSplitterFixed( true );
            } else if ( state == PanelState.Window ) {
                AppManager.propertyWindow.setVisible( true );
                if ( AppManager.propertyWindow.getExtendedState() != BForm.NORMAL ) {
                    AppManager.propertyWindow.setExtendedState( BForm.NORMAL );
                }
                AppManager.propertyWindow.Controls.Add( AppManager.propertyPanel );
                Point parent = this.getLocation();
                XmlRectangle rc = AppManager.editorConfig.PropertyWindowStatus.Bounds;
                Point property = new Point( rc.x, rc.y );
                AppManager.propertyWindow.setBounds( new Rectangle( parent.x + property.x, parent.y + property.y, rc.Width, rc.Height ) );
                normalizeFormLocation( AppManager.propertyWindow );
                menuVisualProperty.setSelected( true );
                if ( AppManager.editorConfig.PropertyWindowStatus.State == PanelState.Docked ) {
                    AppManager.editorConfig.PropertyWindowStatus.DockWidth = splitContainerProperty.getDividerLocation();
                }
                AppManager.editorConfig.PropertyWindowStatus.State = PanelState.Window;
                splitContainerProperty.Panel1MinSize = 0;
                splitContainerProperty.setDividerLocation( 0 );
                splitContainerProperty.setSplitterFixed( true );
                AppManager.editorConfig.PropertyWindowStatus.WindowState = BFormWindowState.Normal;
            }
        }
#endif

        /// <summary>
        /// VsqEvent, VsqBPList, BezierCurvesの全てのクロックを、tempoに格納されているテンポテーブルに
        /// 合致するようにシフトします．ただし，このメソッド内ではtargetのテンポテーブルは変更せず，クロック値だけが変更される．
        /// </summary>
        /// <param name="work"></param>
        /// <param name="tempo"></param>
        private static void shiftClockToMatchWith( VsqFileEx target, VsqFile tempo, double shift_seconds ) {
            // テンポをリプレースする場合。
            // まずクロック値を、リプレース後のモノに置き換え
            for ( int track = 1; track < target.Track.size(); track++ ) {
                // ノート・歌手イベントをシフト
                for ( Iterator itr = target.Track.get( track ).getEventIterator(); itr.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr.next();
                    if ( item.ID.type == VsqIDType.Singer && item.Clock == 0 ) {
                        continue;
                    }
                    int clock = item.Clock;
                    double sec_start = target.getSecFromClock( clock ) + shift_seconds;
                    double sec_end = target.getSecFromClock( clock + item.ID.getLength() ) + shift_seconds;
                    int clock_start = (int)tempo.getClockFromSec( sec_start );
                    int clock_end = (int)tempo.getClockFromSec( sec_end );
                    item.Clock = clock_start;
                    item.ID.setLength( clock_end - clock_start );
                    if ( item.ID.VibratoHandle != null ) {
                        double sec_vib_start = target.getSecFromClock( clock + item.ID.VibratoDelay ) + shift_seconds;
                        int clock_vib_start = (int)tempo.getClockFromSec( sec_vib_start );
                        item.ID.VibratoDelay = clock_vib_start - clock_start;
                        item.ID.VibratoHandle.setLength( clock_end - clock_vib_start );
                    }
                }

                // コントロールカーブをシフト
                for ( int j = 0; j < AppManager.CURVE_USAGE.Length; j++ ) {
                    CurveType ct = AppManager.CURVE_USAGE[j];
                    VsqBPList item = target.Track.get( track ).getCurve( ct.getName() );
                    if ( item == null ) {
                        continue;
                    }
                    VsqBPList repl = new VsqBPList( item.getDefault(), item.getMinimum(), item.getMaximum() );
                    for ( int i = 0; i < item.size(); i++ ) {
                        int clock = item.getKeyClock( i );
                        int value = item.getElement( i );
                        double sec = target.getSecFromClock( clock ) + shift_seconds;
                        if ( sec >= 0 ) {
                            int clock_new = (int)tempo.getClockFromSec( sec );
                            repl.add( clock_new, value );
                        }
                    }
                    target.Track.get( track ).setCurve( ct.getName(), repl );
                }

                // ベジエカーブをシフト
                for ( int j = 0; j < AppManager.CURVE_USAGE.Length; j++ ) {
                    CurveType ct = AppManager.CURVE_USAGE[j];
                    Vector<BezierChain> list = target.AttachedCurves.get( track - 1 ).get( ct );
                    if ( list == null ) {
                        continue;
                    }
                    for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                        BezierChain chain = (BezierChain)itr.next();
                        for ( Iterator itr2 = chain.points.iterator(); itr2.hasNext(); ) {
                            BezierPoint point = (BezierPoint)itr2.next();
                            PointD bse = new PointD( tempo.getClockFromSec( target.getSecFromClock( point.getBase().getX() ) + shift_seconds ),
                                                     point.getBase().getY() );
                            double rx = point.getBase().getX() + point.controlRight.getX();
                            double new_rx = tempo.getClockFromSec( target.getSecFromClock( rx ) + shift_seconds );
                            PointD ctrl_r = new PointD( new_rx - bse.getX(), point.controlRight.getY() );

                            double lx = point.getBase().getX() + point.controlLeft.getX();
                            double new_lx = tempo.getClockFromSec( target.getSecFromClock( lx ) + shift_seconds );
                            PointD ctrl_l = new PointD( new_lx - bse.getX(), point.controlLeft.getY() );
                            point.setBase( bse );
                            point.controlLeft = ctrl_l;
                            point.controlRight = ctrl_r;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// メインメニュー項目の中から，Nameプロパティがnameであるものを検索します．見つからなければnullを返す．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private MenuElement searchMenuItemFromName( String name ) {
            MenuElement[] subitems = menuStripMain.getSubElements();
            for ( int i = 0; i < subitems.Length; i++ ) {
                MenuElement tsi = subitems[i];
                MenuElement ret = searchMenuItemRecurse( name, tsi );
                if ( ret != null ) {
                    return ret;
                }
            }
            return null;
        }

        /// <summary>
        /// 指定されたメニューアイテムから，Nameプロパティがnameであるものを再帰的に検索します．見つからなければnullを返す
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        private MenuElement searchMenuItemRecurse( String name, MenuElement tree ) {
            if ( tree.getName().Equals( name ) ) {
                return tree;
            } else {
                MenuElement[] subitems = tree.getSubElements();
                for ( int i = 0; i < subitems.Length; i++ ) {
                    MenuElement tsi = subitems[i];
                    if ( tsi.getName().Equals( name ) ) {
                        return tsi;
                    }
                    MenuElement ret = searchMenuItemRecurse( name, tsi );
                    if ( ret != null ) {
                        return ret;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// フォームのタイトルバーが画面内に入るよう、Locationを正規化します
        /// </summary>
        /// <param name="form"></param>
        public static void normalizeFormLocation( BForm dlg ) {
            Rectangle rcScreen = PortUtil.getWorkingArea( dlg );
            int top = dlg.getY();
            if ( top + dlg.getHeight() > rcScreen.y + rcScreen.height ) {
                // ダイアログの下端が隠れる場合、位置をずらす
                top = rcScreen.y + rcScreen.height - dlg.getHeight();
            }
            if ( top < rcScreen.y ) {
                // ダイアログの上端が隠れる場合、位置をずらす
                top = rcScreen.y;
            }
            int left = dlg.getX();
            if ( left + dlg.getWidth() > rcScreen.x + rcScreen.width ) {
                left = rcScreen.x + rcScreen.width - dlg.getWidth();
            }
            if ( left < rcScreen.x ) {
                left = rcScreen.x;
            }
            dlg.setLocation( left, top );
        }

        /// <summary>
        /// フォームをマウス位置に出す場合に推奨されるフォーム位置を計算します
        /// </summary>
        /// <param name="dlg"></param>
        /// <returns></returns>
        private Point getFormPreferedLocation( BForm dlg ) {
            Point mouse = PortUtil.getMousePosition();
            Rectangle rcScreen = PortUtil.getWorkingArea( this );
            int top = mouse.y - dlg.Height / 2;
            if ( top + dlg.getHeight() > rcScreen.y + rcScreen.height ) {
                // ダイアログの下端が隠れる場合、位置をずらす
                top = rcScreen.y + rcScreen.height - dlg.getHeight();
            }
            if ( top < rcScreen.y ) {
                // ダイアログの上端が隠れる場合、位置をずらす
                top = rcScreen.y;
            }
            int left = mouse.x - dlg.getWidth() / 2;
            if ( left + dlg.getWidth() > rcScreen.x + rcScreen.width ) {
                left = rcScreen.x + rcScreen.width - dlg.getWidth();
            }
            return new Point( left, top );
        }

        private void updateLayout() {
#if !JAVA
            int width = panel1.Width;
            int height = panel1.Height;

            // splitContainter1.Panel1->splitContainer2.Panel1
            if ( AppManager.editorConfig.OverviewEnabled ) {
                panel3.Height = _OVERVIEW_HEIGHT;
            } else {
                panel3.Height = 0;
            }
            panel3.Width = width;
            pictOverview.Left = AppManager.keyWidth;
            pictOverview.Width = panel3.Width - AppManager.keyWidth;
            pictOverview.Top = 0;
            pictOverview.Height = panel3.Height;

            picturePositionIndicator.Width = width;
            picturePositionIndicator.Height = _PICT_POSITION_INDICATOR_HEIGHT;

            hScroll.Width = width - pictKeyLengthSplitter.Width - pictureBox2.Width - pictureBox3.Width - trackBar.Width;
            hScroll.Height = _SCROLL_WIDTH;

            vScroll.Width = _SCROLL_WIDTH;
            vScroll.Height = height - _PICT_POSITION_INDICATOR_HEIGHT - _SCROLL_WIDTH - panel3.Height;

            pictPianoRoll.Width = width - _SCROLL_WIDTH;
            pictPianoRoll.Height = height - _PICT_POSITION_INDICATOR_HEIGHT - _SCROLL_WIDTH - panel3.Height;

            pictureBox3.Width = AppManager.keyWidth - _SCROLL_WIDTH;
            pictKeyLengthSplitter.Width = _SCROLL_WIDTH;
            pictureBox3.Height = _SCROLL_WIDTH;
            pictureBox2.Height = _SCROLL_WIDTH;
            trackBar.Height = _SCROLL_WIDTH;

            panel3.Top = 0;
            panel3.Left = 0;

            picturePositionIndicator.Top = panel3.Height;
            picturePositionIndicator.Left = 0;

            pictPianoRoll.Top = _PICT_POSITION_INDICATOR_HEIGHT + panel3.Height;
            pictPianoRoll.Left = 0;

            vScroll.Top = _PICT_POSITION_INDICATOR_HEIGHT + panel3.Height;
            vScroll.Left = width - _SCROLL_WIDTH;

            pictureBox3.Top = height - _SCROLL_WIDTH;
            pictureBox3.Left = 0;
            pictKeyLengthSplitter.Top = height - _SCROLL_WIDTH;
            pictKeyLengthSplitter.Left = pictureBox3.Width;

            hScroll.Top = height - _SCROLL_WIDTH;
            hScroll.Left = pictureBox3.Width + pictKeyLengthSplitter.Width;

            trackBar.Top = height - _SCROLL_WIDTH;
            trackBar.Left = width - _SCROLL_WIDTH - trackBar.Width;

            pictureBox2.Top = height - _SCROLL_WIDTH;
            pictureBox2.Left = width - _SCROLL_WIDTH;

            // splitContainer1.Panel2
            //trackSelector.Width = splitContainer1.Panel2.Width - _SCROLL_WIDTH;
            //trackSelector.Height = splitContainer1.Panel2.Height;
#endif
        }

        public void updateRendererMenu() {
            if ( !VSTiProxy.isRendererAvailable( VSTiProxy.RENDERER_DSB2 ) ) {
                cMenuTrackTabRendererVOCALOID1.setIcon( new ImageIcon( Resources.get_slash() ) );
                menuTrackRendererVOCALOID1.setIcon( new ImageIcon( Resources.get_slash() ) );
            } else {
                cMenuTrackTabRendererVOCALOID1.setIcon( null );
                menuTrackRendererVOCALOID1.setIcon( null );
            }
            if ( !VSTiProxy.isRendererAvailable( VSTiProxy.RENDERER_DSB3 ) ) {
                cMenuTrackTabRendererVOCALOID2.setIcon( new ImageIcon( Resources.get_slash() ) );
                menuTrackRendererVOCALOID2.setIcon( new ImageIcon( Resources.get_slash() ) );
            } else {
                cMenuTrackTabRendererVOCALOID2.setIcon( null );
                menuTrackRendererVOCALOID2.setIcon( null );
            }
            if ( !VSTiProxy.isRendererAvailable( VSTiProxy.RENDERER_UTU0 ) ) {
                cMenuTrackTabRendererUtau.setIcon( new ImageIcon( Resources.get_slash() ) );
                menuTrackRendererUtau.setIcon( new ImageIcon( Resources.get_slash() ) );
            } else {
                cMenuTrackTabRendererUtau.setIcon( null );
                menuTrackRendererUtau.setIcon( null );
            }
            if ( !VSTiProxy.isRendererAvailable( VSTiProxy.RENDERER_STR0 ) ) {
                cMenuTrackTabRendererStraight.setIcon( new ImageIcon( Resources.get_slash() ) );
                menuTrackRendererStraight.setIcon( new ImageIcon( Resources.get_slash() ) );
            } else {
                cMenuTrackTabRendererStraight.setIcon( null );
                menuTrackRendererStraight.setIcon( null );
            }
        }

        private void drawUtauVibrato( Graphics2D g, UstVibrato vibrato, int note, int clock_start, int clock_width ) {
            //SmoothingMode old = g.SmoothingMode;
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            // 魚雷を描いてみる
            int y0 = yCoordFromNote( note - 0.5f );
            int x0 = AppManager.xCoordFromClocks( clock_start );
            int px_width = AppManager.xCoordFromClocks( clock_start + clock_width ) - x0;
            int boxheight = (int)(vibrato.Depth * 2 / 100.0f * AppManager.editorConfig.PxTrackHeight);
            int px_shift = (int)(vibrato.Shift / 100.0 * vibrato.Depth / 100.0 * AppManager.editorConfig.PxTrackHeight);

            // vibrato in
            int cl_vibin_end = clock_start + (int)(clock_width * vibrato.In / 100.0);
            int x_vibin_end = AppManager.xCoordFromClocks( cl_vibin_end );
            Point ul = new Point( x_vibin_end, y0 - boxheight / 2 - px_shift );
            Point dl = new Point( x_vibin_end, y0 + boxheight / 2 - px_shift );
            g.setColor( Color.black );
            g.drawPolygon( new int[] { x0, ul.x, dl.x },
                           new int[] { y0, ul.y, dl.y },
                           3 );

            // vibrato out
            int cl_vibout_start = clock_start + clock_width - (int)(clock_width * vibrato.Out / 100.0);
            int x_vibout_start = AppManager.xCoordFromClocks( cl_vibout_start );
            Point ur = new Point( x_vibout_start, y0 - boxheight / 2 - px_shift );
            Point dr = new Point( x_vibout_start, y0 + boxheight / 2 - px_shift );
            g.drawPolygon( new int[] { x0 + px_width, ur.x, dr.x },
                           new int[] { y0, ur.y, dr.y },
                           3 );

            // box
            int boxwidth = x_vibout_start - x_vibin_end;
            if ( boxwidth > 0 ) {
                g.drawPolygon( new int[] { ul.x, dl.x, dr.x, ur.x },
                               new int[] { ul.y, dl.y, dr.y, ur.y },
                               4 );
            }

#if DEBUG
            BufferedWriter sw = new BufferedWriter( new FileWriter( "list.txt" ) );
#endif
            // buf1に、vibrato in/outによる増幅率を代入
            float[] buf1 = new float[clock_width + 1];
            for ( int clock = clock_start; clock <= clock_start + clock_width; clock++ ) {
                buf1[clock - clock_start] = 1.0f;
            }
            // vibin
            if ( cl_vibin_end - clock_start > 0 ) {
                for ( int clock = clock_start; clock <= cl_vibin_end; clock++ ) {
                    int i = clock - clock_start;
                    buf1[i] *= i / (float)(cl_vibin_end - clock_start);
#if DEBUG
                    sw.write( "vibin: " + i + "\t" + buf1[i] );
                    sw.newLine();
#endif
                }
            }
            if ( clock_start + clock_width - cl_vibout_start > 0 ) {
                for ( int clock = clock_start + clock_width; clock >= cl_vibout_start; clock-- ) {
                    int i = clock - clock_start;
                    float v = (clock_start + clock_width - clock) / (float)(clock_start + clock_width - cl_vibout_start);
                    buf1[i] = buf1[i] * v;
#if DEBUG
                    sw.write( "vibout: " + i + "\t" + buf1[i] );
                    sw.newLine();
#endif
                }
            }

            // buf2に、shiftによるy座標のシフト量を代入
            float[] buf2 = new float[clock_width + 1];
            for ( int i = 0; i < clock_width; i++ ) {
                buf2[i] = px_shift * buf1[i];
            }
            try {
                double phase = 2.0 * Math.PI * vibrato.Phase / 100.0;
                double omega = 2.0 * Math.PI / vibrato.Period;   //角速度(rad/msec)
                double msec = AppManager.getVsqFile().getSecFromClock( clock_start - 1 ) * 1000.0;
                float px_track_height = AppManager.editorConfig.PxTrackHeight;
                phase -= (AppManager.getVsqFile().getSecFromClock( clock_start ) * 1000.0 - msec) * omega;
                for ( int clock = clock_start; clock <= clock_start + clock_width; clock++ ) {
                    int i = clock - clock_start;
                    double t_msec = AppManager.getVsqFile().getSecFromClock( clock ) * 1000.0;
                    phase += (t_msec - msec) * omega;
                    msec = t_msec;
                    buf2[i] += (float)(vibrato.Depth * 0.01f * px_track_height * buf1[i] * Math.Sin( phase ));
                }
                int[] listx = new int[clock_width + 1];
                int[] listy = new int[clock_width + 1];
                for ( int clock = clock_start; clock <= clock_start + clock_width; clock++ ) {
                    int i = clock - clock_start;
                    listx[i] = AppManager.xCoordFromClocks( clock );
                    listy[i] = (int)(y0 + buf2[i]);
                }
#if DEBUG
                AppManager.debugWriteLine( "DrawUtauVibrato" );
                for ( int i = 0; i < listx.Length; i++ ) {
                    sw.write( listx[i] + "\t" + listy[i] );
                    sw.newLine();
                }
                sw.close();
                sw = null;
#endif
                if ( listx.Length >= 2 ) {
                    g.setColor( Color.red );
                    g.drawPolygon( listx, listy, listx.Length );
                }
                //g.SmoothingMode = old;
            } catch ( OverflowException oex ) {
#if DEBUG
                AppManager.debugWriteLine( "DrawUtauVibrato; oex=" + oex );
#endif
            }
        }

        /// <summary>
        /// ビブラート用のデータ点のリストを取得します。返却されるリストは、{秒, ビブラートの振幅(ノートナンバー単位)}の値ペアとなっています
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="start_rate"></param>
        /// <param name="depth"></param>
        /// <param name="start_depth"></param>
        /// <param name="clock_start"></param>
        /// <param name="clock_width"></param>
        /// <returns></returns>
        public static Vector<PointD> getVibratoPoints( VsqFileEx vsq,
                                                       VibratoBPList rate,
                                                       int start_rate,
                                                       VibratoBPList depth,
                                                       int start_depth,
                                                       int clock_start,
                                                       int clock_width,
                                                       float sec_resolution ) {
            Vector<PointD> ret = new Vector<PointD>();
            double sec0 = vsq.getSecFromClock( clock_start );
            double sec1 = vsq.getSecFromClock( clock_start + clock_width );
            int count = (int)((sec1 - sec0) / sec_resolution);
            double phase = 0;
            start_rate = rate.getValue( 0.0f, start_rate );
            start_depth = depth.getValue( 0.0f, start_depth );
            float amplitude = start_depth * 2.5f / 127.0f / 2.0f; // ビブラートの振幅。
            float period = (float)Math.Exp( 5.24 - 1.07e-2 * start_rate ) * 2.0f / 1000.0f; //ビブラートの周期、秒
            float omega = (float)(2.0 * Math.PI / period); // 角速度(rad/sec)
            ret.add( new PointD( sec0, 0 ) );
            double sec = sec0;
            float fadewidth = (float)(sec1 - sec0) * 0.2f;
            for ( int i = 1; i < count; i++ ) {
                double t_sec = sec0 + sec_resolution * i;
                double clock = vsq.getClockFromSec( t_sec );
                if ( sec0 <= t_sec && t_sec <= sec0 + fadewidth ) {
                    amplitude *= (float)(t_sec - sec0) / fadewidth;
                }
                if ( sec1 - fadewidth <= t_sec && t_sec <= sec1 ) {
                    amplitude *= (float)(sec1 - t_sec) / fadewidth;
                }
                phase += omega * (t_sec - sec);
                ret.add( new PointD( t_sec, amplitude * Math.Sin( phase ) ) );
                float v = (float)(clock - clock_start) / (float)clock_width;
                int r = rate.getValue( v, start_rate );
                int d = depth.getValue( v, start_depth );
                amplitude = d * 2.5f / 127.0f / 2.0f;
                period = (float)Math.Exp( 5.24 - 1.07e-2 * r ) * 2.0f / 1000.0f;
                omega = (float)(2.0 * Math.PI / period);
                sec = t_sec;
            }
            return ret;
        }

#if !JAVA
        /// <summary>
        /// listに登録されているToolStripを，座標の若い順にcontainerに追加します
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="list"></param>
        private void addToolStripInPositionOrder( System.Windows.Forms.ToolStripPanel panel, Vector<BToolBar> list ) {
            boolean[] reg = new boolean[list.size()];
            for ( int i = 0; i < reg.Length; i++ ) {
                reg[i] = false;
            }
            for ( int i = 0; i < list.size(); i++ ) {
                Point p = new Point( int.MaxValue, int.MaxValue );
                int index = -1;

                // x座標の小さいやつを探す
                for ( int j = 0; j < list.size(); j++ ) {
                    if ( !reg[j] ) {
                        BToolBar ts = list.get( j );
                        if ( p.y > ts.Location.Y ) {
                            index = j;
                            p = new Point( ts.Location.X, ts.Location.Y );
                        }
                        if ( p.y >= ts.Location.Y && p.x > ts.Location.X ) {
                            index = j;
                            p = new Point( ts.Location.X, ts.Location.Y );
                        }
                    }
                }

                // コントロールを登録
                panel.Join( list.get( index ), list.get( index ).Location );
                reg[index] = true;
            }
        }
#endif

#if ENABLE_SCRIPT
        /// <summary>
        /// Palette Toolの表示を更新します
        /// </summary>
        public void updatePaletteTool() {
            int count = 0;
            int num_has_dialog = 0;
            for ( Iterator itr = m_palette_tools.iterator(); itr.hasNext(); ) {
                BToolStripButton item = (BToolStripButton)itr.next();
                toolStripTool.add( item );
            }
            String lang = Messaging.getLanguage();
            boolean first = true;
            for ( Iterator itr = PaletteToolServer.LoadedTools.keySet().iterator(); itr.hasNext(); ) {
                String id = (String)itr.next();
                count++;
                IPaletteTool ipt = (IPaletteTool)PaletteToolServer.LoadedTools.get( id );
#if !JAVA
                System.Drawing.Bitmap icon = ipt.getIcon();
#endif
                String name = ipt.getName( lang );
                String desc = ipt.getDescription( lang );

                // toolStripPaletteTools
                BToolStripButton tsb = new BToolStripButton();
#if !JAVA
                if ( icon != null ) {
                    tsb.setIcon( new ImageIcon( icon ) );
                }
#endif
                tsb.setText( name );
                tsb.setToolTipText( desc );
                tsb.setTag( id );
                tsb.setCheckOnClick( false );
#if JAVA
                tsb.clickEvent.add( new BEventHandler( this, "commonStripPaletteTool_Clicked" ) );
#else
                tsb.Click += new EventHandler( commonStripPaletteTool_Clicked );
                tsb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
#endif
                if ( first ) {
                    toolStripTool.addSeparator();
                    first = false;
                }
                m_palette_tools.add( tsb );
                toolStripTool.add( tsb );

                // cMenuTrackSelector
                BMenuItem tsmi = new BMenuItem();
                tsmi.setText( name );
                tsmi.setToolTipText( desc );
                tsmi.setTag( id );
#if JAVA
                tsmi.clickEvent.add( new BEventHandler( this, "commonStripPaletteTool_Clicked" ) );
#else
                tsmi.Click += new BEventHandler( commonStripPaletteTool_Clicked );
#endif
                cMenuTrackSelectorPaletteTool.add( tsmi );

                // cMenuPiano
                BMenuItem tsmi2 = new BMenuItem();
                tsmi2.setText( name );
                tsmi2.setToolTipText( desc );
                tsmi2.setTag( id );
#if JAVA
                tsmi2.clickEvent.add( new BEventHandler( this, "commonStripPaletteTool_Clicked" ) );
#else
                tsmi2.Click += new EventHandler( commonStripPaletteTool_Clicked );
#endif
                cMenuPianoPaletteTool.add( tsmi2 );

                // menuSettingPaletteTool
                if ( ipt.hasDialog() ) {
                    BMenuItem tsmi3 = new BMenuItem();
                    tsmi3.setText( name );
                    tsmi3.setTag( id );
#if JAVA
                    tsmi3.clickEvent.add( new BEventHandler( this, "commonSettingPaletteTool" ) );
#else
                    tsmi3.Click += new EventHandler( commonSettingPaletteTool );
#endif
                    menuSettingPaletteTool.add( tsmi3 );
                    num_has_dialog++;
                }
            }
            if ( count == 0 ) {
                cMenuTrackSelectorPaletteTool.setVisible( false );
                cMenuPianoPaletteTool.setVisible( false );
            }
            if ( num_has_dialog == 0 ) {
                menuSettingPaletteTool.setVisible( false );
            }
        }
#endif

        private void commonSettingPaletteTool( Object sender, BEventArgs e ) {
#if ENABLE_SCRIPT
            if ( sender is BMenuItem ) {
                BMenuItem tsmi = (BMenuItem)sender;
                if ( tsmi.getTag() != null && tsmi.getTag() is String ) {
                    String id = (String)tsmi.getTag();
                    if ( PaletteToolServer.LoadedTools.containsKey( id ) ) {
                        Object instance = PaletteToolServer.LoadedTools.get( id );
                        IPaletteTool ipt = (IPaletteTool)instance;
                        if ( ipt.openDialog() == System.Windows.Forms.DialogResult.OK ) {
                            XmlSerializer xsms = new XmlSerializer( instance.GetType(), true );
                            String dir = PortUtil.combinePath( AppManager.getApplicationDataPath(), "tool" );
                            if ( !PortUtil.isDirectoryExists( dir ) ) {
                                PortUtil.createDirectory( dir );
                            }
                            String cfg = id + ".config";
                            String config = PortUtil.combinePath( dir, cfg );
                            FileOutputStream fs = null;
                            try {
                                fs = new FileOutputStream( config );
                                xsms.serialize( fs, null );
                            } catch ( Exception ex ) {
                            } finally {
                                if ( fs != null ) {
                                    try {
                                        fs.close();
                                    } catch ( Exception ex2 ) {
                                    }
                                }
                            }
                        }
                    }
                }
            }
#endif
        }

        public void updateCopyAndPasteButtonStatus() {
            // copy cut deleteの表示状態更新
            boolean selected_is_null = (AppManager.getSelectedEventCount() == 0) &&
                                       (AppManager.getSelectedTempoCount() == 0) &&
                                       (AppManager.getSelectedTimesigCount() == 0) &&
                                       (AppManager.getSelectedPointIDCount() == 0);

            cMenuTrackSelectorCopy.setEnabled( AppManager.getSelectedPointIDCount() > 0 );
            cMenuTrackSelectorCut.setEnabled( AppManager.getSelectedPointIDCount() > 0 );
            cMenuTrackSelectorDeleteBezier.setEnabled( (AppManager.isCurveMode() && AppManager.getLastSelectedBezier() != null) );
            cMenuTrackSelectorDelete.setEnabled( AppManager.getSelectedPointIDCount() > 0 ); //todo: このへん。右クリック位置にベジエ制御点などがあった場合eneble=trueにする

            cMenuPianoCopy.setEnabled( !selected_is_null );
            cMenuPianoCut.setEnabled( !selected_is_null );
            cMenuPianoDelete.setEnabled( !selected_is_null );

            menuEditCopy.setEnabled( !selected_is_null );
            menuEditCut.setEnabled( !selected_is_null );
            menuEditDelete.setEnabled( !selected_is_null );

            ClipboardEntry ce = AppManager.getCopiedItems();
            int copy_started_clock = ce.copyStartedClock;
            TreeMap<CurveType, VsqBPList> copied_curve = ce.points;
            TreeMap<CurveType, Vector<BezierChain>> copied_bezier = ce.beziers;
            boolean copied_is_null = (ce.events.size() == 0) &&
                                  (ce.tempo.size() == 0) &&
                                  (ce.timesig.size() == 0) &&
                                  (copied_curve.size() == 0) &&
                                  (copied_bezier.size() == 0);
            boolean enabled = !copied_is_null;
            if ( copied_curve.size() == 1 ) {
                // 1種類のカーブがコピーされている場合→コピーされているカーブの種類と、現在選択されているカーブの種類とで、最大値と最小値が一致している場合のみ、ペースト可能
                CurveType ct = CurveType.Empty;
                for ( Iterator itr = copied_curve.keySet().iterator(); itr.hasNext(); ) {
                    CurveType c = (CurveType)itr.next();
                    ct = c;
                }
                CurveType selected = trackSelector.getSelectedCurve();
                if ( ct.getMaximum() == selected.getMaximum() &&
                     ct.getMinimum() == selected.getMinimum() &&
                     !selected.isScalar() && !selected.isAttachNote() ) {
                    enabled = true;
                } else {
                    enabled = false;
                }
            } else if ( copied_curve.size() >= 2 ) {
                // 複数種類のカーブがコピーされている場合→そのままペーストすればOK
                enabled = true;
            }
            cMenuTrackSelectorPaste.setEnabled( enabled );
            cMenuPianoPaste.setEnabled( enabled );
            menuEditPaste.setEnabled( enabled );

            /*int copy_started_clock;
            boolean copied_is_null = (AppManager.GetCopiedEvent().Count == 0) &&
                                  (AppManager.GetCopiedTempo( out copy_started_clock ).Count == 0) &&
                                  (AppManager.GetCopiedTimesig( out copy_started_clock ).Count == 0) &&
                                  (AppManager.GetCopiedCurve( out copy_started_clock ).Count == 0) &&
                                  (AppManager.GetCopiedBezier( out copy_started_clock ).Count == 0);
            menuEditCut.isEnabled() = !selected_is_null;
            menuEditCopy.isEnabled() = !selected_is_null;
            menuEditDelete.isEnabled() = !selected_is_null;
            //stripBtnCopy.isEnabled() = !selected_is_null;
            //stripBtnCut.isEnabled() = !selected_is_null;

            if ( AppManager.GetCopiedEvent().Count != 0 ) {
                menuEditPaste.isEnabled() = (AppManager.CurrentClock >= AppManager.VsqFile.getPreMeasureClocks());
                //stripBtnPaste.isEnabled() = (AppManager.CurrentClock >= AppManager.VsqFile.getPreMeasureClocks());
            } else {
                menuEditPaste.isEnabled() = !copied_is_null;
                //stripBtnPaste.isEnabled() = !copied_is_null;
            }*/
        }

        /// <summary>
        /// 現在の編集データを全て破棄する。DirtyCheckは行われない。
        /// </summary>
        private void clearExistingData() {
            AppManager.clearCommandBuffer();
            AppManager.clearSelectedBezier();
            AppManager.clearSelectedEvent();
            AppManager.clearSelectedTempo();
            AppManager.clearSelectedTimesig();
            if ( AppManager.isPlaying() ) {
                AppManager.setPlaying( false );
            }
        }

        /// <summary>
        /// 保存されていない編集内容があるかどうかチェックし、必要なら確認ダイアログを出す。
        /// </summary>
        /// <returns>保存されていない保存内容などない場合、または、保存する必要がある場合で（保存しなくてよいと指定された場合または保存が行われた場合）にtrueを返す</returns>
        private boolean dirtyCheck() {
            if ( m_edited ) {
                String file = AppManager.getFileName();
                if ( file.Equals( "" ) ) {
                    file = "Untitled";
                } else {
                    file = PortUtil.getFileName( file );
                }
                BDialogResult dr = AppManager.showMessageBox( _( "Save this sequence?" ),
                                                              _( "Affirmation" ),
                                                              AppManager.MSGBOX_YES_NO_CANCEL_OPTION,
                                                              AppManager.MSGBOX_QUESTION_MESSAGE );
                if ( dr == BDialogResult.YES ) {
                    if ( AppManager.getFileName().Equals( "" ) ) {
                        int dr2 = saveXmlVsqDialog.showSaveDialog( this );
                        if ( dr2 == BFileChooser.APPROVE_OPTION ) {
                            AppManager.saveTo( saveXmlVsqDialog.getSelectedFile() );
                            return true;
                        } else {
                            return false;
                        }
                    } else {
                        AppManager.saveTo( AppManager.getFileName() );
                        return true;
                    }
                } else if ( dr == BDialogResult.NO ) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }

        /// <summary>
        /// waveView用のwaveファイルを読込むスレッドで使用する
        /// </summary>
        /// <param name="arg"></param>
        private void LoadWaveThreadProc( Object arg ) {
            String file = (String)arg;
            waveView.loadWave( file );
        }

        /// <summary>
        /// menuVisualWaveform.isSelected()の値をもとに、splitterContainer2の表示状態を更新します
        /// </summary>
        private void updateSplitContainer2Size() {
            if ( menuVisualWaveform.isSelected() ) {
                splitContainer2.Panel2MinSize = _SPL2_PANEL2_MIN_HEIGHT;
                splitContainer2.setSplitterFixed( false );
                splitContainer2.SplitterWidth = _SPL_SPLITTER_WIDTH;
                if ( m_last_splitcontainer2_split_distance <= 0 || m_last_splitcontainer2_split_distance > splitContainer2.Height ) {
                    splitContainer2.setDividerLocation( (int)(splitContainer2.Height * 0.9) );
                } else {
                    splitContainer2.setDividerLocation( m_last_splitcontainer2_split_distance );
                }
            } else {
                m_last_splitcontainer2_split_distance = splitContainer2.getDividerLocation();
                splitContainer2.Panel2MinSize = 0;
                splitContainer2.SplitterWidth = 0;
                splitContainer2.setDividerLocation( splitContainer2.Height );
                splitContainer2.setSplitterFixed( true );
            }
        }

        /// <summary>
        /// trackSelectorに表示するコントロールのカーブの種類を、AppManager.EditorConfigの設定に応じて更新します
        /// </summary>
        private void updateTrackSelectorVisibleCurve() {
            if ( AppManager.editorConfig.CurveVisibleVelocity ) {
                trackSelector.addViewingCurve( CurveType.VEL );
            }
            if ( AppManager.editorConfig.CurveVisibleAccent ) {
                trackSelector.addViewingCurve( CurveType.Accent );
            }
            if ( AppManager.editorConfig.CurveVisibleDecay ) {
                trackSelector.addViewingCurve( CurveType.Decay );
            }
            if ( AppManager.editorConfig.CurveVisibleVibratoRate ) {
                trackSelector.addViewingCurve( CurveType.VibratoRate );
            }
            if ( AppManager.editorConfig.CurveVisibleVibratoDepth ) {
                trackSelector.addViewingCurve( CurveType.VibratoDepth );
            }
            if ( AppManager.editorConfig.CurveVisibleDynamics ) {
                trackSelector.addViewingCurve( CurveType.DYN );
            }
            if ( AppManager.editorConfig.CurveVisibleBreathiness ) {
                trackSelector.addViewingCurve( CurveType.BRE );
            }
            if ( AppManager.editorConfig.CurveVisibleBrightness ) {
                trackSelector.addViewingCurve( CurveType.BRI );
            }
            if ( AppManager.editorConfig.CurveVisibleClearness ) {
                trackSelector.addViewingCurve( CurveType.CLE );
            }
            if ( AppManager.editorConfig.CurveVisibleOpening ) {
                trackSelector.addViewingCurve( CurveType.OPE );
            }
            if ( AppManager.editorConfig.CurveVisibleGendorfactor ) {
                trackSelector.addViewingCurve( CurveType.GEN );
            }
            if ( AppManager.editorConfig.CurveVisiblePortamento ) {
                trackSelector.addViewingCurve( CurveType.POR );
            }
            if ( AppManager.editorConfig.CurveVisiblePit ) {
                trackSelector.addViewingCurve( CurveType.PIT );
            }
            if ( AppManager.editorConfig.CurveVisiblePbs ) {
                trackSelector.addViewingCurve( CurveType.PBS );
            }
            if ( AppManager.editorConfig.CurveVisibleHarmonics ) {
                trackSelector.addViewingCurve( CurveType.harmonics );
            }
            if ( AppManager.editorConfig.CurveVisibleFx2Depth ) {
                trackSelector.addViewingCurve( CurveType.fx2depth );
            }
            if ( AppManager.editorConfig.CurveVisibleReso1 ) {
                trackSelector.addViewingCurve( CurveType.reso1freq );
                trackSelector.addViewingCurve( CurveType.reso1bw );
                trackSelector.addViewingCurve( CurveType.reso1amp );
            }
            if ( AppManager.editorConfig.CurveVisibleReso2 ) {
                trackSelector.addViewingCurve( CurveType.reso2freq );
                trackSelector.addViewingCurve( CurveType.reso2bw );
                trackSelector.addViewingCurve( CurveType.reso2amp );
            }
            if ( AppManager.editorConfig.CurveVisibleReso3 ) {
                trackSelector.addViewingCurve( CurveType.reso3freq );
                trackSelector.addViewingCurve( CurveType.reso3bw );
                trackSelector.addViewingCurve( CurveType.reso3amp );
            }
            if ( AppManager.editorConfig.CurveVisibleReso4 ) {
                trackSelector.addViewingCurve( CurveType.reso4freq );
                trackSelector.addViewingCurve( CurveType.reso4bw );
                trackSelector.addViewingCurve( CurveType.reso4amp );
            }
            if ( AppManager.editorConfig.CurveVisibleEnvelope ) {
                trackSelector.addViewingCurve( CurveType.Env );
            }
        }

        /// <summary>
        /// ウィンドウの表示内容に応じて、ウィンドウサイズの最小値を計算します
        /// </summary>
        /// <returns></returns>
        private Dimension getWindowMinimumSize() {
            Dimension current_minsize = new Dimension( getMinimumSize().width, getMinimumSize().height );
#if JAVA
            Dimension client = getContentPane().getSize();
            Dimension current = getSize();
#else
            Dimension client = new Dimension( this.ClientSize.Width, this.ClientSize.Height );
            Dimension current = new Dimension( this.Size.Width, this.Size.Height );
#endif
            return new Dimension( current_minsize.width,
                                  splitContainer1.Panel2MinSize +
                                     _SCROLL_WIDTH + _PICT_POSITION_INDICATOR_HEIGHT + pictPianoRoll.MinimumSize.Height +
                                     toolStripContainer.TopToolStripPanel.Height +
                                     menuStripMain.getHeight() + statusStrip1.Height +
                                     (current.height - client.height) +
                                     20 );
        }

        /// <summary>
        /// 現在のAppManager.inputTextBoxの状態を元に、歌詞の変更を反映させるコマンドを実行します
        /// </summary>
        private void executeLyricChangeCommand() {
#if JAVA
            if ( !AppManager.inputTextBox.isVisible() ) {
#else
            if ( !AppManager.inputTextBox.Enabled ) {
#endif
                return;
            }
            if ( AppManager.inputTextBox.IsDisposed ) {
                return;
            }
            int selected = AppManager.getSelected();
            SelectedEventEntry last_selected_event = AppManager.getLastSelectedEvent();
            String original_phrase = last_selected_event.original.ID.LyricHandle.L0.Phrase;
            String original_symbol = last_selected_event.original.ID.LyricHandle.L0.getPhoneticSymbol();
            boolean symbol_protected = last_selected_event.original.ID.LyricHandle.L0.PhoneticSymbolProtected;
            boolean phonetic_symbol_edit_mode = ((TagLyricTextBox)AppManager.inputTextBox.getTag()).isPhoneticSymbolEditMode();
#if DEBUG
            AppManager.debugWriteLine( "    original_phase,symbol=" + original_phrase + "," + original_symbol );
            AppManager.debugWriteLine( "    phonetic_symbol_edit_mode=" + phonetic_symbol_edit_mode );
            AppManager.debugWriteLine( "    AppManager.inputTextBox.setText(=" + AppManager.inputTextBox.getText() );
#endif
            String phrase;
            ByRef<String> phonetic_symbol = new ByRef<String>( "" );
            phrase = AppManager.inputTextBox.getText();
            if ( !phonetic_symbol_edit_mode ) {
                if ( AppManager.editorConfig.SelfDeRomanization ) {
                    phrase = KanaDeRomanization.Attach( phrase );
                }
            }
            if ( (phonetic_symbol_edit_mode && AppManager.inputTextBox.getText() != original_symbol) ||
                 (!phonetic_symbol_edit_mode && phrase != original_phrase) ) {
                TagLyricTextBox kvp = (TagLyricTextBox)AppManager.inputTextBox.getTag();
                if ( phonetic_symbol_edit_mode ) {
                    phrase = kvp.getBufferText();
                    phonetic_symbol.value = AppManager.inputTextBox.getText();
                    String[] spl = PortUtil.splitString( phonetic_symbol.value, new char[] { ' ' }, true );
                    Vector<String> list = new Vector<String>();
                    for ( int i = 0; i < spl.Length; i++ ) {
                        String s = spl[i];
                        if ( VsqPhoneticSymbol.isValidSymbol( s ) ) {
                            list.add( s );
                        }
                    }
                    phonetic_symbol.value = "";
                    boolean first = true;
                    for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                        String s = (String)itr.next();
                        if ( first ) {
                            phonetic_symbol.value += s;
                            first = false;
                        } else {
                            phonetic_symbol.value += " " + s;
                        }
                    }
                    symbol_protected = true;
                } else {
                    if ( !symbol_protected ) {
                        SymbolTable.attatch( phrase, phonetic_symbol );
                    } else {
                        phonetic_symbol.value = original_symbol;
                    }
                }
#if DEBUG
                AppManager.debugWriteLine( "    phrase,phonetic_symbol=" + phrase + "," + phonetic_symbol );
#endif
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandEventChangeLyric( selected,
                                                      AppManager.getLastSelectedEvent().original.InternalID,
                                                      phrase,
                                                      phonetic_symbol.value,
                                                      symbol_protected ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                setEdited( true );
            }
        }

        private void removeGameControler() {
#if !JAVA
            if ( m_timer != null ) {
                m_timer.stop();
                m_timer.Dispose();
                m_timer = null;
            }
            m_game_mode = GameControlMode.DISABLED;
            updateGameControlerStatus( null, null );
#endif
        }

        private void loadGameControler() {
#if !JAVA
            try {
                boolean init_success = false;
                int num_joydev = winmmhelp.JoyInit();
                if ( num_joydev <= 0 ) {
                    init_success = false;
                } else {
                    init_success = true;
                }
                if ( init_success ) {
                    m_game_mode = GameControlMode.NORMAL;
                    stripLblGameCtrlMode.setIcon( null );
                    stripLblGameCtrlMode.setText( m_game_mode.ToString() );
                    m_timer = new BTimer();
                    m_timer.setDelay( 10 );
                    m_timer.Tick += new EventHandler( m_timer_Tick );
                    m_timer.start();
                } else {
                    m_game_mode = GameControlMode.DISABLED;
                }
            } catch ( Exception ex ) {
                m_game_mode = GameControlMode.DISABLED;
#if DEBUG
                AppManager.debugWriteLine( "FormMain+ReloadGameControler" );
                AppManager.debugWriteLine( "    ex=" + ex );
#endif
            }
            updateGameControlerStatus( null, null );
#endif
        }

#if ENABLE_MIDI
        private void reloadMidiIn() {
#if DEBUG
            AppManager.debugWriteLine( "FormMain.ReloadMidiIn" );
#endif
            if ( m_midi_in != null ) {
                m_midi_in.MidiReceived -= m_midi_in_MidiReceived;
                m_midi_in.Dispose();
            }
            try {
                m_midi_in = new MidiInDevice( AppManager.editorConfig.MidiInPort.PortNumber );
                m_midi_in.MidiReceived += m_midi_in_MidiReceived;
            } catch ( Exception ex ) {
#if DEBUG
                AppManager.debugWriteLine( "    ex=" + ex );
#endif
            }
            updateMidiInStatus();
        }
#endif

#if ENABLE_MIDI
        private void m_midi_in_MidiReceived( double time, byte[] data ) {
#if !JAVA
            if ( data.Length <= 2 ) {
                return;
            }
            if ( !AppManager.isPlaying() ) {
                return;
            }
            int code = data[0] & 0xf0;
#if DEBUG
            AppManager.debugWriteLine( "m_midi_in_MidiReceived" );
            AppManager.debugWriteLine( "    code=0x" + Convert.ToString( code, 16 ) );
#endif
            if ( code != 0x80 && code != 0x90 ) {
                return;
            }
            if ( code == 0x90 && data[2] == 0x00 ) {
                code = 0x80;//ベロシティ0のNoteOnはNoteOff
            }

            byte note = data[1];
            if ( code == 0x90 && AppManager.getEditMode() == EditMode.REALTIME ) {
                MidiPlayer.PlayImmediate( note );
            }

            int clock = AppManager.getCurrentClock();
            int unit = AppManager.getPositionQuantizeClock();
            if ( unit > 1 ) {
                int odd = clock % unit;
                int nclock = clock;
                nclock -= odd;
                if ( odd > unit / 2 ) {
                    nclock += unit;
                }
                clock = nclock;
            }

            if ( code == 0x80 ) {
                if ( AppManager.addingEvent != null ) {
                    int len = clock - AppManager.addingEvent.Clock;
                    if ( len <= 0 ) {
                        len = unit;
                    }
                    AppManager.addingEvent.ID.Length = len;
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventAdd( AppManager.getSelected(),
                                                                                                   AppManager.addingEvent ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    if ( !isEdited() ) {
                        setEdited( true );
                    }
#if USE_DOBJ
                    updateDrawObjectList();
#endif
                }
            } else if ( code == 0x90 ) {
                AppManager.addingEvent = new VsqEvent( clock, new VsqID( 0 ) );
                AppManager.addingEvent.ID.type = VsqIDType.Anote;
                AppManager.addingEvent.ID.Dynamics = 64;
                AppManager.addingEvent.ID.VibratoHandle = null;
                AppManager.addingEvent.ID.LyricHandle = new LyricHandle( "a", "a" );
                AppManager.addingEvent.ID.Note = note;
                if ( AppManager.getEditMode() != EditMode.REALTIME ) {
                    KeySoundPlayer.Play( note );
                }
            }
#endif
        }
#endif

        /// <summary>
        /// 現在のゲームコントローラのモードに応じてstripLblGameCtrlModeの表示状態を更新します。
        /// </summary>
        private void updateGameControlerStatus( Object sender, BEventArgs e ) {
#if !JAVA
            if ( m_game_mode == GameControlMode.DISABLED ) {
                stripLblGameCtrlMode.setText( _( "Disabled" ) );
                stripLblGameCtrlMode.setIcon( new ImageIcon( Resources.get_slash() ) );
            } else if ( m_game_mode == GameControlMode.CURSOR ) {
                stripLblGameCtrlMode.setText( _( "Cursor" ) );
                stripLblGameCtrlMode.setIcon( null );
            } else if ( m_game_mode == GameControlMode.KEYBOARD ) {
                stripLblGameCtrlMode.setText( _( "Keyboard" ) );
                stripLblGameCtrlMode.setIcon( new ImageIcon( Resources.get_piano() ) );
            } else if ( m_game_mode == GameControlMode.NORMAL ) {
                stripLblGameCtrlMode.setText( _( "Normal" ) );
                stripLblGameCtrlMode.setIcon( null );
            }
#endif
        }

#if ENABLE_MIDI
        private void updateMidiInStatus() {
            int midiport = AppManager.editorConfig.MidiInPort.PortNumber;
            bocoree.MIDIINCAPS[] devices = MidiInDevice.GetMidiInDevices();
            if ( midiport < 0 || devices.Length <= 0 ) {
                stripLblMidiIn.setText( _( "Disabled" ) );
                stripLblMidiIn.setIcon( new ImageIcon( Resources.get_slash() ) );
            } else {
                if ( midiport >= devices.Length ) {
                    midiport = 0;
                    AppManager.editorConfig.MidiInPort.PortNumber = midiport;
                }
                stripLblMidiIn.setText( devices[midiport].szPname );
                stripLblMidiIn.setIcon( new ImageIcon( Resources.get_piano() ) );
            }
        }
#endif

#if ENABLE_SCRIPT
        /// <summary>
        /// スクリプトフォルダ中のスクリプトへのショートカットを作成する
        /// </summary>
        private void updateScriptShortcut() {
            TreeMap<String, ScriptInvoker> old = new TreeMap<String, ScriptInvoker>();
            MenuElement[] sub_menu_script = menuScript.getSubElements();
            for ( int i = 0; i < sub_menu_script.Length; i++ ) {
                MenuElement item = sub_menu_script[i];
                if ( !(item is BMenuItem) ) {
                    continue;
                }
                BMenuItem tsmi = (BMenuItem)item;
                MenuElement[] sub_tsmi = tsmi.getSubElements();
                if ( sub_tsmi.Length <= 0 ) {
                    continue;
                }
                if ( !(sub_tsmi[0] is BMenuItem) ) {
                    continue;
                }
                BMenuItem sub_tsmi0 = (BMenuItem)sub_tsmi[0];
                if ( sub_tsmi0.getTag() != null && sub_tsmi0.getTag() is ScriptInvoker ) {
                    ScriptInvoker si = (ScriptInvoker)sub_tsmi0.getTag();
                    old.put( si.ScriptFile, si );
                }
            }
            menuScript.removeAll();
            String script_path = PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "script" );
            if ( !PortUtil.isDirectoryExists( script_path ) ) {
                System.IO.Directory.CreateDirectory( script_path );
            }

            System.IO.DirectoryInfo current = new System.IO.DirectoryInfo( script_path );
            int count = 0;
            Vector<System.IO.FileInfo> files = new Vector<System.IO.FileInfo>( current.GetFiles( "*.txt" ) );
            files.addAll( Arrays.asList( current.GetFiles( "*.cs" ) ) );
            for ( Iterator itr = files.iterator(); itr.hasNext(); ) {
                System.IO.FileInfo fi = (System.IO.FileInfo)itr.next();
                count++;
                String fname = fi.FullName;
                String scriptname = PortUtil.getFileNameWithoutExtension( fname );
                BMenuItem item = new BMenuItem();
                item.setText( scriptname );
                BMenuItem dd_run = new BMenuItem();
                dd_run.setText( _( "Run" ) + "(&R)" );
                dd_run.setName( "menuScript" + scriptname + "Run" );
                if ( old.containsKey( fname ) && old.get( fname ) != null ) {
                    dd_run.setTag( old.get( fname ) );
                } else {
                    ScriptInvoker si2 = new ScriptInvoker();
                    si2.FileTimestamp = DateTime.MinValue;
                    si2.ScriptFile = fname;
                    dd_run.setTag( si2 );
                }
                dd_run.Click += new EventHandler( dd_run_Click );
                item.add( dd_run );
                menuScript.add( item );
            }
            old.clear();
            if ( count > 0 ) {
                menuScript.addSeparator();
            }
            menuScript.add( menuScriptUpdate );
            Util.applyToolStripFontRecurse( menuScript, AppManager.editorConfig.getBaseFont() );
            applyShortcut();
        }
#endif

#if ENABLE_SCRIPT
        private void dd_run_Click( Object sender, BEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "dd_run_Click" );
#endif
            try {
                ScriptInvoker si = (ScriptInvoker)((BMenuItem)sender).getTag();
                String script_file = si.ScriptFile;
#if DEBUG
                AppManager.debugWriteLine( "    si.FileTimestamp=" + si.FileTimestamp );
                AppManager.debugWriteLine( "    File.GetLastWriteTimeUtc( script_file )=" + System.IO.File.GetLastWriteTimeUtc( script_file ) );
#endif
                if ( si.FileTimestamp != System.IO.File.GetLastWriteTimeUtc( script_file ) ||
                     si.ScriptType == null ||
                     si.Serializer == null ||
                     si.scriptDelegate == null ) {

                    si = AppManager.loadScript( script_file );
                    ((BMenuItem)sender).setTag( si );
#if DEBUG
                    AppManager.debugWriteLine( "    err_msg=" + si.ErrorMessage );
#endif
                }
                if ( si.scriptDelegate != null ) {
                    if ( AppManager.invokeScript( si ) ) {
                        setEdited( true );
#if USE_DOBJ
                        updateDrawObjectList();
#endif
                        refreshScreen();
                    }
                } else {
                    FormCompileResult dlg = null;
                    try {
                        dlg = new FormCompileResult( _( "Failed loading script." ), si.ErrorMessage );
                        dlg.showDialog();
                    } catch ( Exception ex ) {
                    } finally {
                        if ( dlg != null ) {
                            try {
                                dlg.close();
                            } catch ( Exception ex2 ) {
                            }
                        }
                    }
                }
            } catch ( Exception ex3 ) {
#if DEBUG
                PortUtil.println( "AppManager#dd_run_Click; ex3=" + ex3 );
#endif
            }
        }
#endif

        public void ensureVisible( int clock ) {
            // カーソルが画面内にあるかどうか検査
            int clock_left = AppManager.clockFromXCoord( AppManager.keyWidth );
            int clock_right = AppManager.clockFromXCoord( pictPianoRoll.getWidth() );
            int uwidth = clock_right - clock_left;
            if ( clock < clock_left || clock_right < clock ) {
                int cl_new_center = (clock / uwidth) * uwidth + uwidth / 2;
                float f_draft = cl_new_center - (pictPianoRoll.getWidth() / 2 + 34 - 70) / AppManager.scaleX;
                if ( f_draft < 0f ) {
                    f_draft = 0;
                }
                int draft = (int)(f_draft);
                if ( draft < hScroll.getMinimum() ) {
                    draft = hScroll.getMinimum();
                } else if ( hScroll.getMaximum() < draft ) {
                    draft = hScroll.getMaximum();
                }
                if ( hScroll.getValue() != draft ) {
                    AppManager.drawStartIndex[AppManager.getSelected() - 1] = 0;
                    hScroll.setValue( draft );
                }
            }
        }

        /// <summary>
        /// プレイカーソルが見えるようスクロールする
        /// </summary>
        public void ensureCursorVisible() {
            ensureVisible( AppManager.getCurrentClock() );
        }

        private void ProcessSpecialShortcutKey( BPreviewKeyDownEventArgs e ) {
#if JAVA
            if ( !AppManager.inputTextBox.isVisible() ) {
#else
            if ( !AppManager.inputTextBox.Enabled ) {
#endif

#if JAVA
                if ( e.KeyValue == KeyEvent.VK_RETURN ) {
#else
                if ( e.KeyCode == System.Windows.Forms.Keys.Return ) {
#endif
                    if ( AppManager.isPlaying() ) {
                        timer.stop();
                    }
                    AppManager.setPlaying( !AppManager.isPlaying() );
#if JAVA
                } else if ( e.KeyValue == KeyEvent.VK_SPACE ) {
#else
                } else if ( e.KeyCode == System.Windows.Forms.Keys.Space ) {
#endif
                    if ( !AppManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier ) {
                        if ( AppManager.isPlaying() ) {
                            timer.stop();
                        }
                        AppManager.setPlaying( !AppManager.isPlaying() );
                    }
#if JAVA
                } else if ( e.KeyValue == KeyEvent.VK_PERIOD ) {
#else
                } else if ( e.KeyCode == System.Windows.Forms.Keys.OemPeriod ) {
#endif
                    if ( AppManager.isPlaying() ) {
                        AppManager.setPlaying( false );
                    } else {
                        if ( !AppManager.startMarkerEnabled ) {
                            AppManager.setCurrentClock( 0 );
                        } else {
                            AppManager.setCurrentClock( AppManager.startMarker );
                        }
                        refreshScreen();
                    }
#if JAVA
                } else if( e.KeyValue == KeyEvent.VK_ADD || e.KeyValue == KeyEvent.VK_PLUS || e.KeyValue == KeyEvent.VK_RIGHT ) {
#else
                } else if ( e.KeyCode == System.Windows.Forms.Keys.Add || e.KeyCode == System.Windows.Forms.Keys.Oemplus || e.KeyCode == System.Windows.Forms.Keys.Right ) {
#endif
                    forward();
#if JAVA
                } else if ( e.KeyValue == KeyEvent.VK_MINUS || e.KeyValue == KeyEvent.VK_LEFT ) {
#else
                } else if ( e.KeyCode == System.Windows.Forms.Keys.Subtract || e.KeyCode == System.Windows.Forms.Keys.OemMinus || e.KeyCode == System.Windows.Forms.Keys.Left ) {
#endif
                    rewind();
                }
            }
            return;
            #region OLD CODES DO NOT REMOVE
            /*if ( AppManager.EditorConfig.Platform == Platform.Macintosh ) {
                if ( AppManager.EditorConfig.CommandKeyAsControl ) {
                    #region menuStripMain
                    if ( e.Alt && e.KeyCode == Keys.N && menuFileNew.isEnabled() ) {
                        this.menuFileNew_Click( menuFileNew, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.O && menuFileOpen.isEnabled() ) {
                        this.menuFileOpen_Click( menuFileOpen, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.S && menuFileSave.isEnabled() ) {
                        this.menuFileSave_Click( menuFileSave, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.Q && menuFileQuit.isEnabled() ) {
                        this.menuFileQuit_Click( menuFileQuit, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.Z && menuEditUndo.isEnabled() ) {
                        this.menuEditUndo_Click( menuEditUndo, null );
                        return;
                    } else if ( e.Alt && e.Shift && e.KeyCode == Keys.Z && menuEditRedo.isEnabled() ) {
                        this.menuEditRedo_Click( this.menuEditRedo, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.X && this.menuEditCut.isEnabled() ) {
                        this.menuEditCut_Click( this.menuEditCut, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.C && this.menuEditCopy.isEnabled() ) {
                        this.menuEditCopy_Click( this.menuEditCopy, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.V && this.menuEditPaste.isEnabled() ) {
                        this.menuEditPaste_Click( this.menuEditPaste, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.A && this.menuEditSelectAll.isEnabled() ) {
                        this.menuEditSelectAll_Click( this.menuEditSelectAll, null );
                        return;
                    } else if ( e.Alt && e.Shift && this.menuEditSelectAllEvents.isEnabled() ) {
                        this.menuEditSelectAllEvents_Click( this.menuEditSelectAllEvents, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.V && this.menuHiddenEditPaste.isEnabled() ) {
                        this.menuHiddenEditPaste_Click( this.menuHiddenEditPaste, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.W && this.menuHiddenEditFlipToolPointerPencil.isEnabled() ) {
                        this.menuHiddenEditFlipToolPointerPencil_Click( this.menuHiddenEditFlipToolPointerPencil, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.E && this.menuHiddenEditFlipToolPointerEraser.isEnabled() ) {
                        this.menuHiddenEditFlipToolPointerEraser_Click( this.menuHiddenEditFlipToolPointerEraser, null );
                        return;
                    } else if ( (e.KeyCode & Keys.Clear) == Keys.Clear && e.Alt && e.Shift && this.menuHiddenVisualForwardParameter.isEnabled() ) {
                        this.menuHiddenVisualForwardParameter_Click( this.menuHiddenVisualForwardParameter, null );
                        return;
                    } else if ( (e.KeyCode & Keys.LButton) == Keys.LButton && (e.KeyCode & Keys.LineFeed) == Keys.LineFeed && e.Alt && e.Shift && this.menuHiddenVisualBackwardParameter.isEnabled() ) {
                        this.menuHiddenVisualBackwardParameter_Click( this.menuHiddenVisualBackwardParameter, null );
                        return;
                    } else if ( (e.KeyCode & Keys.Clear) == Keys.Clear && e.Alt && this.menuHiddenTrackNext.isEnabled() ) {
                        this.menuHiddenTrackNext_Click( this.menuHiddenTrackNext, null );
                        return;
                    } else if ( (e.KeyCode & Keys.LButton) == Keys.LButton && (e.KeyCode & Keys.LineFeed) == Keys.LineFeed && e.Alt && this.menuHiddenTrackBack.isEnabled() ) {
                        this.menuHiddenTrackBack_Click( this.menuHiddenTrackBack, null );
                        return;
                    }
                    #endregion

                    #region cMenuPiano
                    if ( e.Alt && e.KeyCode == Keys.Z && cMenuPianoUndo.isEnabled() ) {
                        this.cMenuPianoUndo_Click( this.cMenuPianoUndo, null );
                        return;
                    } else if ( e.Alt && e.Shift && e.KeyCode == Keys.Z && this.cMenuPianoRedo.isEnabled() ) {
                        this.cMenuPianoRedo_Click( this.cMenuPianoRedo, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.X && this.cMenuPianoCut.isEnabled() ) {
                        this.cMenuPianoCut_Click( this.cMenuPianoCut, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.C && this.cMenuPianoCopy.isEnabled() ) {
                        this.cMenuPianoCopy_Click( this.cMenuPianoCopy, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.A && cMenuPianoSelectAll.isEnabled() ) {
                        this.cMenuPianoSelectAll_Click( this.cMenuPianoSelectAll, null );
                        return;
                    } else if ( e.Alt && e.Shift && e.KeyCode == Keys.A && cMenuPianoSelectAllEvents.isEnabled() ) {
                        this.cMenuPianoSelectAllEvents_Click( this.cMenuPianoSelectAllEvents, null );
                        return;
                    }
                    #endregion

                    #region cMenuTrackSelector
                    if ( e.Alt && e.KeyCode == Keys.Z && cMenuTrackSelectorUndo.isEnabled() ) {
                        this.cMenuTrackSelectorUndo_Click( this.cMenuTrackSelectorUndo, null );
                        return;
                    } else if ( e.Alt && e.Shift && e.KeyCode == Keys.Z && this.cMenuTrackSelectorRedo.isEnabled() ) {
                        this.cMenuTrackSelectorRedo_Click( this.cMenuTrackSelectorRedo, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.X && this.cMenuTrackSelectorCut.isEnabled() ) {
                        this.cMenuTrackSelectorCut_Click( this.cMenuTrackSelectorCut, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.C && this.cMenuTrackSelectorCopy.isEnabled() ) {
                        this.cMenuTrackSelectorCopy_Click( this.cMenuTrackSelectorCopy, null );
                        return;
                    } else if ( e.Alt && e.KeyCode == Keys.V && this.cMenuTrackSelectorPaste.isEnabled() ) {
                        this.cMenuTrackSelectorPaste_Click( this.cMenuTrackSelectorPaste, null );
                        return;
                    } else if ( e.Alt && e.Shift && e.KeyCode == Keys.A && this.cMenuTrackSelectorSelectAll.isEnabled() ) {
                        this.cMenuTrackSelectorSelectAll_Click( this.cMenuTrackSelectorSelectAll, null );
                        return;
                    }
                    #endregion
                } else {
                    boolean RButton = (e.KeyCode & Keys.RButton) == Keys.RButton;
                    boolean Clear = (e.KeyCode & Keys.Clear) == Keys.Clear;
                    boolean Return = (e.KeyCode & Keys.Return) == Keys.Return;
                    boolean Pause = (e.KeyCode & Keys.Pause) == Keys.Pause;
                    boolean FinalMode = (e.KeyCode & Keys.FinalMode) == Keys.FinalMode;
                    boolean Cancel = (e.KeyCode & Keys.Cancel) == Keys.Cancel;
                    boolean CapsLock = (e.KeyCode & Keys.CapsLock) == Keys.CapsLock;
                    boolean LButton = (e.KeyCode & Keys.LButton) == Keys.LButton;
                    boolean JunjaMode = (e.KeyCode & Keys.JunjaMode) == Keys.JunjaMode;
                    boolean LineFeed = (e.KeyCode & Keys.LineFeed) == Keys.LineFeed;
                    boolean ControlKey = (e.KeyCode & Keys.ControlKey) == Keys.ControlKey;
                    boolean XButton1 = (e.KeyCode & Keys.XButton1) == Keys.XButton1;

                    #region menuStripMain
                    if ( RButton && Clear && (e.KeyCode & Keys.N) == Keys.N && menuFileNew.isEnabled() ) {
                        this.menuFileNew_Click( menuFileNew, null );
                        return;
                    } else if ( RButton && Return && (e.KeyCode & Keys.O) == Keys.O && menuFileOpen.isEnabled() ) {
                        this.menuFileOpen_Click( menuFileOpen, null );
                        return;
                    } else if ( Pause && (e.KeyCode & Keys.S) == Keys.S && menuFileSave.isEnabled() ) {
                        this.menuFileSave_Click( menuFileSave, null );
                        return;
                    } else if ( ControlKey && (e.KeyCode & Keys.Q) == Keys.Q && menuFileQuit.isEnabled() ) {
                        this.menuFileQuit_Click( menuFileQuit, null );
                        return;
                    } else if ( RButton && FinalMode && (e.KeyCode & Keys.Z) == Keys.Z && menuEditUndo.isEnabled() ) {
                        this.menuEditUndo_Click( menuEditUndo, null );
                        return;
                    } else if ( RButton && FinalMode && e.Shift && (e.KeyCode & Keys.Z) == Keys.Z && menuEditRedo.isEnabled() ) {
                        this.menuEditRedo_Click( this.menuEditRedo, null );
                        return;
                    } else if ( FinalMode && (e.KeyCode & Keys.X) == Keys.X && this.menuEditCut.isEnabled() ) {
                        this.menuEditCut_Click( this.menuEditCut, null );
                        return;
                    } else if ( Cancel && (e.KeyCode & Keys.C) == Keys.C && this.menuEditCopy.isEnabled() ) {
                        this.menuEditCopy_Click( this.menuEditCopy, null );
                        return;
                    } else if ( RButton && CapsLock && (e.KeyCode & Keys.V) == Keys.V && this.menuEditPaste.isEnabled() ) {
                        this.menuEditPaste_Click( this.menuEditPaste, null );
                        return;
                    } else if ( LButton && (e.KeyCode & Keys.A) == Keys.A && this.menuEditSelectAll.isEnabled() ) {
                        this.menuEditSelectAll_Click( this.menuEditSelectAll, null );
                        return;
                    } else if ( LButton && e.Shift && (e.KeyCode & Keys.A) == Keys.A && this.menuEditSelectAllEvents.isEnabled() ) {
                        this.menuEditSelectAllEvents_Click( this.menuEditSelectAllEvents, null );
                        return;
                    } else if ( RButton && CapsLock && (e.KeyCode & Keys.V) == Keys.V && this.menuHiddenEditPaste.isEnabled() ) {
                        this.menuHiddenEditPaste_Click( this.menuHiddenEditPaste, null );
                        return;
                    } else if ( JunjaMode && (e.KeyCode & Keys.W) == Keys.W && this.menuHiddenEditFlipToolPointerPencil.isEnabled() ) {
                        this.menuHiddenEditFlipToolPointerPencil_Click( this.menuHiddenEditFlipToolPointerPencil, null );
                        return;
                    } else if ( XButton1 && (e.KeyCode & Keys.E) == Keys.E && this.menuHiddenEditFlipToolPointerEraser.isEnabled() ) {
                        this.menuHiddenEditFlipToolPointerEraser_Click( this.menuHiddenEditFlipToolPointerEraser, null );
                        return;
                    } else if ( Clear && e.Control && e.Shift && this.menuHiddenVisualForwardParameter.isEnabled() ) {
                        this.menuHiddenVisualForwardParameter_Click( this.menuHiddenVisualForwardParameter, null );
                        return;
                    } else if ( LButton && LineFeed && e.Control && e.Shift && this.menuHiddenVisualBackwardParameter.isEnabled() ) {
                        this.menuHiddenVisualBackwardParameter_Click( this.menuHiddenVisualBackwardParameter, null );
                        return;
                    } else if ( Clear && e.Control && this.menuHiddenTrackNext.isEnabled() ) {
                        this.menuHiddenTrackNext_Click( this.menuHiddenTrackNext, null );
                        return;
                    } else if ( LButton && LineFeed && e.Control && this.menuHiddenTrackBack.isEnabled() ) {
                        this.menuHiddenTrackBack_Click( this.menuHiddenTrackBack, null );
                        return;
                    }
                    #endregion

                    #region cMenuPiano
                    if ( RButton && FinalMode && (e.KeyCode & Keys.Z) == Keys.Z && cMenuPianoUndo.isEnabled() ) {
                        this.cMenuPianoUndo_Click( this.cMenuPianoUndo, null );
                        return;
                    } else if ( RButton && FinalMode && e.Shift && (e.KeyCode & Keys.Z) == Keys.Z && this.cMenuPianoRedo.isEnabled() ) {
                        this.cMenuPianoRedo_Click( this.cMenuPianoRedo, null );
                        return;
                    } else if ( FinalMode && (e.KeyCode & Keys.X) == Keys.X && this.cMenuPianoCut.isEnabled() ) {
                        this.cMenuPianoCut_Click( this.cMenuPianoCut, null );
                        return;
                    } else if ( Cancel && (e.KeyCode & Keys.C) == Keys.C && this.cMenuPianoCopy.isEnabled() ) {
                        this.cMenuPianoCopy_Click( this.cMenuPianoCopy, null );
                        return;
                    } else if ( LButton && (e.KeyCode & Keys.A) == Keys.A && cMenuPianoSelectAll.isEnabled() ) {
                        this.cMenuPianoSelectAll_Click( this.cMenuPianoSelectAll, null );
                        return;
                    } else if ( LButton && e.Shift && (e.KeyCode & Keys.A) == Keys.A && cMenuPianoSelectAllEvents.isEnabled() ) {
                        this.cMenuPianoSelectAllEvents_Click( this.cMenuPianoSelectAllEvents, null );
                        return;
                    }
                    #endregion

                    #region cMenuTrackSelector
                    if ( RButton && FinalMode && (e.KeyCode & Keys.Z) == Keys.Z && cMenuTrackSelectorUndo.isEnabled() ) {
                        this.cMenuTrackSelectorUndo_Click( this.cMenuTrackSelectorUndo, null );
                        return;
                    } else if ( RButton && FinalMode && e.Shift && (e.KeyCode & Keys.Z) == Keys.Z && this.cMenuTrackSelectorRedo.isEnabled() ) {
                        this.cMenuTrackSelectorRedo_Click( this.cMenuTrackSelectorRedo, null );
                        return;
                    } else if ( FinalMode && (e.KeyCode & Keys.X) == Keys.X && this.cMenuTrackSelectorCut.isEnabled() ) {
                        this.cMenuTrackSelectorCut_Click( this.cMenuTrackSelectorCut, null );
                        return;
                    } else if ( Cancel && (e.KeyCode & Keys.C) == Keys.C && this.cMenuTrackSelectorCopy.isEnabled() ) {
                        this.cMenuTrackSelectorCopy_Click( this.cMenuTrackSelectorCopy, null );
                        return;
                    } else if ( RButton && CapsLock && (e.KeyCode & Keys.V) == Keys.V && this.cMenuTrackSelectorPaste.isEnabled() ) {
                        this.cMenuTrackSelectorPaste_Click( this.cMenuTrackSelectorPaste, null );
                        return;
                    } else if ( LButton && e.Shift && (e.KeyCode & Keys.A) == Keys.A && this.cMenuTrackSelectorSelectAll.isEnabled() ) {
                        this.cMenuTrackSelectorSelectAll_Click( this.cMenuTrackSelectorSelectAll, null );
                        return;
                    }
                    #endregion
                }
            } else {
                #region menuStripMain
                if ( e.Control && e.KeyCode == Keys.N && menuFileNew.isEnabled() ) {
                    this.menuFileNew_Click( menuFileNew, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.O && menuFileOpen.isEnabled() ) {
                    this.menuFileOpen_Click( menuFileOpen, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.S && menuFileSave.isEnabled() ) {
                    this.menuFileSave_Click( menuFileSave, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.Q && menuFileQuit.isEnabled() ) {
                    this.menuFileQuit_Click( menuFileQuit, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.Z && menuEditUndo.isEnabled() ) {
                    this.menuEditUndo_Click( menuEditUndo, null );
                    return;
                } else if ( e.Control && e.Shift && e.KeyCode == Keys.Z && menuEditRedo.isEnabled() ) {
                    this.menuEditRedo_Click( this.menuEditRedo, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.X && this.menuEditCut.isEnabled() ) {
                    this.menuEditCut_Click( this.menuEditCut, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.C && this.menuEditCopy.isEnabled() ) {
                    this.menuEditCopy_Click( this.menuEditCopy, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.V && this.menuEditPaste.isEnabled() ) {
                    this.menuEditPaste_Click( this.menuEditPaste, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.A && this.menuEditSelectAll.isEnabled() ) {
                    this.menuEditSelectAll_Click( this.menuEditSelectAll, null );
                    return;
                } else if ( e.Control && e.Shift && this.menuEditSelectAllEvents.isEnabled() ) {
                    this.menuEditSelectAllEvents_Click( this.menuEditSelectAllEvents, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.V && this.menuHiddenEditPaste.isEnabled() ) {
                    this.menuHiddenEditPaste_Click( this.menuHiddenEditPaste, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.W && this.menuHiddenEditFlipToolPointerPencil.isEnabled() ) {
                    this.menuHiddenEditFlipToolPointerPencil_Click( this.menuHiddenEditFlipToolPointerPencil, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.E && this.menuHiddenEditFlipToolPointerEraser.isEnabled() ) {
                    this.menuHiddenEditFlipToolPointerEraser_Click( this.menuHiddenEditFlipToolPointerEraser, null );
                    return;
                } else if ( e.Control && e.Alt && (e.KeyCode & Keys.PageDown) == Keys.PageDown && this.menuHiddenVisualForwardParameter.isEnabled() ) {
                    this.menuHiddenVisualForwardParameter_Click( this.menuHiddenVisualForwardParameter, null );
                    return;
                } else if ( e.Control && e.Alt && (e.KeyCode & Keys.PageUp) == Keys.PageUp && this.menuHiddenVisualBackwardParameter.isEnabled() ) {
                    this.menuHiddenVisualBackwardParameter_Click( this.menuHiddenVisualBackwardParameter, null );
                    return;
                } else if ( e.Control && (e.KeyCode & Keys.PageDown) == Keys.PageDown && this.menuHiddenTrackNext.isEnabled() ) {
                    this.menuHiddenTrackNext_Click( this.menuHiddenTrackNext, null );
                    return;
                } else if ( e.Control && (e.KeyCode & Keys.PageUp) == Keys.PageUp && this.menuHiddenTrackBack.isEnabled() ) {
                    this.menuHiddenTrackBack_Click( this.menuHiddenTrackBack, null );
                    return;
                }
                #endregion

                #region cMenuPiano
                if ( e.Control && e.KeyCode == Keys.Z && cMenuPianoUndo.isEnabled() ) {
                    this.cMenuPianoUndo_Click( this.cMenuPianoUndo, null );
                    return;
                } else if ( e.Control && e.Shift && e.KeyCode == Keys.Z && this.cMenuPianoRedo.isEnabled() ) {
                    this.cMenuPianoRedo_Click( this.cMenuPianoRedo, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.X && this.cMenuPianoCut.isEnabled() ) {
                    this.cMenuPianoCut_Click( this.cMenuPianoCut, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.C && this.cMenuPianoCopy.isEnabled() ) {
                    this.cMenuPianoCopy_Click( this.cMenuPianoCopy, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.A && cMenuPianoSelectAll.isEnabled() ) {
                    this.cMenuPianoSelectAll_Click( this.cMenuPianoSelectAll, null );
                    return;
                } else if ( e.Control && e.Shift && e.KeyCode == Keys.A && cMenuPianoSelectAllEvents.isEnabled() ) {
                    this.cMenuPianoSelectAllEvents_Click( this.cMenuPianoSelectAllEvents, null );
                    return;
                }
                #endregion

                #region cMenuTrackSelector
                if ( e.Control && e.KeyCode == Keys.Z && cMenuTrackSelectorUndo.isEnabled() ) {
                    this.cMenuTrackSelectorUndo_Click( this.cMenuTrackSelectorUndo, null );
                    return;
                } else if ( e.Control && e.Shift && e.KeyCode == Keys.Z && this.cMenuTrackSelectorRedo.isEnabled() ) {
                    this.cMenuTrackSelectorRedo_Click( this.cMenuTrackSelectorRedo, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.X && this.cMenuTrackSelectorCut.isEnabled() ) {
                    this.cMenuTrackSelectorCut_Click( this.cMenuTrackSelectorCut, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.C && this.cMenuTrackSelectorCopy.isEnabled() ) {
                    this.cMenuTrackSelectorCopy_Click( this.cMenuTrackSelectorCopy, null );
                    return;
                } else if ( e.Control && e.KeyCode == Keys.V && this.cMenuTrackSelectorPaste.isEnabled() ) {
                    this.cMenuTrackSelectorPaste_Click( this.cMenuTrackSelectorPaste, null );
                    return;
                } else if ( e.Control && e.Shift && e.KeyCode == Keys.A && this.cMenuTrackSelectorSelectAll.isEnabled() ) {
                    this.cMenuTrackSelectorSelectAll_Click( this.cMenuTrackSelectorSelectAll, null );
                    return;
                }
                #endregion
            }*/
            #endregion
        }

        private void setHScrollRange( int draft_length ) {
            int _ARROWS = 40; // 両端の矢印の表示幅px（おおよその値）
            draft_length += 240;
            if ( draft_length > hScroll.getMaximum() ) {
                hScroll.setMaximum( draft_length );
            }
            if ( pictPianoRoll.getWidth() <= AppManager.keyWidth || hScroll.getWidth() <= _ARROWS ) {
                return;
            }
            int large_change = (int)((pictPianoRoll.getWidth() - AppManager.keyWidth) / (float)AppManager.scaleX);
            int box_width = (int)((hScroll.getWidth() - _ARROWS) * (float)large_change / (float)(hScroll.getMaximum() + large_change));
            if ( box_width < AppManager.editorConfig.MinimumScrollHandleWidth ) {
                box_width = AppManager.editorConfig.MinimumScrollHandleWidth;
                large_change = (int)((float)hScroll.getMaximum() * (float)box_width / (float)(hScroll.getWidth() - _ARROWS - box_width));
            }
            if ( large_change > 0 ) {
                hScroll.setVisibleAmount( large_change );
            }
        }

        private void setVScrollRange( int draft_length ) {
            int _ARROWS = 40; // 両端の矢印の表示幅px（おおよその値）
            if ( draft_length > vScroll.getMaximum() ) {
                vScroll.setMaximum( draft_length );
            }
            int large_change = (int)pictPianoRoll.getHeight();
            int box_width = (int)((vScroll.getHeight() - _ARROWS) * (float)large_change / (float)(vScroll.getMaximum() + large_change));
            if ( box_width < AppManager.editorConfig.MinimumScrollHandleWidth ) {
                box_width = AppManager.editorConfig.MinimumScrollHandleWidth;
                large_change = (int)((float)vScroll.getMaximum() * (float)box_width / (float)(vScroll.getWidth() - _ARROWS - box_width));
            }
            if ( large_change > 0 ) {
                vScroll.setVisibleAmount( large_change );
            }
        }

        private void refreshScreenCore( Object sender, BEventArgs e ) {
            pictPianoRoll.repaint();
            picturePositionIndicator.repaint();
            trackSelector.repaint();
            if ( menuVisualWaveform.isSelected() ) {
                waveView.draw();
                waveView.Refresh();
            }
            if ( AppManager.editorConfig.OverviewEnabled ) {
                pictOverview.repaint();
            }
        }

        public void refreshScreen() {
            //#if DEBUG
            //            RefreshScreenCore();
            //#else
            if ( !bgWorkScreen.IsBusy ) {
                bgWorkScreen.RunWorkerAsync();
            }
            //#endif
        }

        public void flipMixerDialogVisible( boolean visible ) {
            AppManager.mixerWindow.setVisible( visible );
            AppManager.editorConfig.MixerVisible = visible;
            menuVisualMixer.setSelected( visible );
        }

        /// <summary>
        /// メニューのショートカットキーを、AppManager.EditorConfig.ShorcutKeysの内容に応じて変更します
        /// </summary>
        private void applyShortcut() {
            if ( AppManager.editorConfig.Platform == PlatformEnum.Macintosh ) {
                #region Platform.Macintosh
                String _CO = "";
                //if ( AppManager.EditorConfig.CommandKeyAsControl ) {
#if JAVA
                char[] arr = new char[]{ 0x2318 };
                _CO = new String( arr );
#else
                _CO = new String( '\x2318', 1 );
#endif
                //} else {
                //_CO = "^";
                //}
                String _SHIFT = "⇧";
                //if ( AppManager.EditorConfig.CommandKeyAsControl ) {
                #region menuStripMain
                menuFileNew.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_N, InputEvent.META_MASK ) );
                menuFileOpen.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_O, InputEvent.META_MASK ) );
                menuFileSave.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_S, InputEvent.META_MASK ) );
                menuFileQuit.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_Q, InputEvent.META_MASK ) );

                menuEditUndo.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_Z, InputEvent.META_MASK ) );
                menuEditRedo.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_Z, InputEvent.META_MASK | InputEvent.SHIFT_MASK ) );
                menuEditCut.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_X, InputEvent.META_MASK ) );
                menuEditCopy.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_C, InputEvent.META_MASK ) );
                menuEditPaste.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_V, InputEvent.META_MASK ) );
                menuEditSelectAll.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_A, InputEvent.META_MASK ) );
                menuEditSelectAllEvents.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_A, InputEvent.META_MASK | InputEvent.SHIFT_MASK ) );

                menuHiddenEditFlipToolPointerPencil.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_W, InputEvent.META_MASK ) );
                menuHiddenEditFlipToolPointerEraser.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_E, InputEvent.META_MASK ) );
                menuHiddenVisualForwardParameter.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_RIGHT, 0 ) );
                menuHiddenVisualBackwardParameter.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_LEFT, 0 ) );
                menuHiddenTrackNext.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_PAGE_DOWN, 0 ) );
                menuHiddenTrackBack.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_PAGE_UP, 0 ) );
                #endregion

                #region cMenuPiano
                cMenuPianoUndo.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_Z, InputEvent.META_MASK ) );
                cMenuPianoRedo.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_Z, InputEvent.META_MASK | InputEvent.SHIFT_MASK ) );
                cMenuPianoCut.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_X, InputEvent.META_MASK ) );
                cMenuPianoCopy.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_C, InputEvent.META_MASK ) );
                cMenuPianoSelectAll.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_A, InputEvent.META_MASK ) );
                cMenuPianoSelectAllEvents.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_A, InputEvent.META_MASK | InputEvent.SHIFT_MASK ) );
                #endregion

                #region cMenuTrackSelector
                cMenuTrackSelectorUndo.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_Z, InputEvent.META_MASK ) );
                cMenuTrackSelectorRedo.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_Z, InputEvent.META_MASK | InputEvent.SHIFT_MASK ) );
                cMenuTrackSelectorCut.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_X, InputEvent.META_MASK ) );
                cMenuTrackSelectorCopy.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_C, InputEvent.META_MASK ) );
                cMenuTrackSelectorPaste.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_V, InputEvent.META_MASK ) );
                cMenuTrackSelectorSelectAll.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_A, InputEvent.META_MASK | InputEvent.SHIFT_MASK ) );
                #endregion
                #endregion
            } else {
                TreeMap<String, BKeys[]> dict = AppManager.editorConfig.getShortcutKeysDictionary();
                #region menuStripMain
                for ( Iterator itr = dict.keySet().iterator(); itr.hasNext(); ) {
                    String key = (String)itr.next();
                    if ( key.Equals( "menuEditCopy" ) || key.Equals( "menuEditCut" ) || key.Equals( "menuEditPaste" ) ) {
                        continue;
                    }
                    MenuElement menu = searchMenuItemFromName( key );
                    if ( menu != null ) {
                        applyMenuItemShortcut( dict, menu, menu.getName() );
                    }
                }
                if ( dict.containsKey( "menuEditCopy" ) ) {
                    applyMenuItemShortcut( dict, menuHiddenCopy, "menuEditCopy" );
                }
                if ( dict.containsKey( "menuEditCut" ) ) {
                    applyMenuItemShortcut( dict, menuHiddenCut, "menuEditCut" );
                }
                if ( dict.containsKey( "menuEditCopy" ) ) {
                    applyMenuItemShortcut( dict, menuHiddenPaste, "menuEditPaste" );
                }
                #endregion

                ValuePair<String, BMenuItem[]>[] work = new ValuePair<String, BMenuItem[]>[]{
                    new ValuePair<String, BMenuItem[]>( "menuEditUndo", new BMenuItem[]{ cMenuPianoUndo, cMenuTrackSelectorUndo } ),
                    new ValuePair<String, BMenuItem[]>( "menuEditRedo", new BMenuItem[]{ cMenuPianoRedo, cMenuTrackSelectorRedo } ),
                    new ValuePair<String, BMenuItem[]>( "menuEditCut", new BMenuItem[]{ cMenuPianoCut, cMenuTrackSelectorCut, menuEditCut } ),
                    new ValuePair<String, BMenuItem[]>( "menuEditCopy", new BMenuItem[]{ cMenuPianoCopy, cMenuTrackSelectorCopy, menuEditCopy } ),
                    new ValuePair<String, BMenuItem[]>( "menuEditPaste", new BMenuItem[]{ cMenuPianoPaste, cMenuTrackSelectorPaste, menuEditPaste } ),
                    new ValuePair<String, BMenuItem[]>( "menuEditSelectAll", new BMenuItem[]{ cMenuPianoSelectAll, cMenuTrackSelectorSelectAll } ),
                    new ValuePair<String, BMenuItem[]>( "menuEditSelectAllEvents", new BMenuItem[]{ cMenuPianoSelectAllEvents } ),
                    new ValuePair<String, BMenuItem[]>( "menuEditDelete", new BMenuItem[]{ menuEditDelete } ),
                    new ValuePair<String, BMenuItem[]>( "menuVisualGridline", new BMenuItem[]{ cMenuPianoGrid } ),
                    new ValuePair<String, BMenuItem[]>( "menuJobLyric", new BMenuItem[]{ cMenuPianoImportLyric } ),
                    new ValuePair<String, BMenuItem[]>( "menuLyricExpressionProperty", new BMenuItem[]{ cMenuPianoExpressionProperty } ),
                    new ValuePair<String, BMenuItem[]>( "menuLyricVibratoProperty", new BMenuItem[]{ cMenuPianoVibratoProperty } ),
                    new ValuePair<String, BMenuItem[]>( "menuTrackOn", new BMenuItem[]{ cMenuTrackTabTrackOn } ),
                    new ValuePair<String, BMenuItem[]>( "menuTrackAdd", new BMenuItem[]{ cMenuTrackTabAdd } ),
                    new ValuePair<String, BMenuItem[]>( "menuTrackCopy", new BMenuItem[]{ cMenuTrackTabCopy } ),
                    new ValuePair<String, BMenuItem[]>( "menuTrackDelete", new BMenuItem[]{ cMenuTrackTabDelete } ),
                    new ValuePair<String, BMenuItem[]>( "menuTrackRenderCurrent", new BMenuItem[]{ cMenuTrackTabRenderCurrent } ),
                    new ValuePair<String, BMenuItem[]>( "menuTrackRenderAll", new BMenuItem[]{ cMenuTrackTabRenderAll } ),
                    new ValuePair<String, BMenuItem[]>( "menuTrackOverlay", new BMenuItem[]{ cMenuTrackTabOverlay } ),
                    new ValuePair<String, BMenuItem[]>( "menuTrackRendererVOCALOID1", new BMenuItem[]{ cMenuTrackTabRendererVOCALOID1 } ),
                    new ValuePair<String, BMenuItem[]>( "menuTrackRendererVOCALOID2", new BMenuItem[]{ cMenuTrackTabRendererVOCALOID2 } ),
                    new ValuePair<String, BMenuItem[]>( "menuTrackRendererUtau", new BMenuItem[]{ cMenuTrackTabRendererUtau } ),
                };
                for ( int j = 0; j < work.Length; j++ ) {
                    ValuePair<String, BMenuItem[]> item = work[j];
                    if ( dict.containsKey( item.getKey() ) ) {
                        BKeys[] k = dict.get( item.getKey() );
                        String s = AppManager.getShortcutDisplayString( k );
                        if ( s != "" ) {
                            for ( int i = 0; i < item.getValue().Length; i++ ) {
                                item.getValue()[i].ShortcutKeyDisplayString = s;
                            }
                        }
                    }
                }

                // スクリプトにショートカットを適用
#if DEBUG
                AppManager.debugWriteLine( "ApplyShortCut" );
#endif
                MenuElement[] sub_menu_script = menuScript.getSubElements();
                for ( int i = 0; i < sub_menu_script.Length; i++ ) {
                    MenuElement tsi = sub_menu_script[i];
                    MenuElement[] sub_tsi = tsi.getSubElements();
                    if ( sub_tsi.Length == 1 ) {
                        MenuElement dd_run = sub_tsi[0];
#if DEBUG
                        AppManager.debugWriteLine( "    dd_run.name=" + dd_run.getName() );
#endif
                        if ( dict.containsKey( dd_run.getName() ) ) {
                            applyMenuItemShortcut( dict, tsi, tsi.getName() );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// dictの中から
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="item"></param>
        /// <param name="item_name"></param>
        /// <param name="default_shortcut"></param>
        private void applyMenuItemShortcut( TreeMap<String, BKeys[]> dict, MenuElement item, String item_name ) {
            try {
                if ( dict.containsKey( item_name ) ) {
                    item.setAccelerator( PortUtil.getKeyStrokeFromBKeys( dict.get( item_name ) ) );
                } else {
                    item.setAccelerator( PortUtil.getKeyStrokeFromBKeys( new BKeys[] { BKeys.None } ) );
                }
            } catch ( Exception ex ) {
            }
        }

        /// <summary>
        /// ソングポジションを1小節進めます
        /// </summary>
        private void forward() {
            boolean playing = AppManager.isPlaying();
            if ( playing ) {
                return;
            }
            int current = AppManager.getVsqFile().getBarCountFromClock( AppManager.getCurrentClock() ) + 1;
            int new_clock = AppManager.getVsqFile().getClockFromBarCount( current );
            if ( new_clock <= hScroll.getMaximum() + (pictPianoRoll.getWidth() - AppManager.keyWidth) / AppManager.scaleX ) {
                AppManager.setCurrentClock( new_clock );
                ensureCursorVisible();
                AppManager.setPlaying( playing );
                refreshScreen();
            }
        }

        /// <summary>
        /// ソングポジションを1小節戻します
        /// </summary>
        private void rewind() {
            boolean playing = AppManager.isPlaying();
            if ( playing ) {
                return;
            }
            int current = AppManager.getVsqFile().getBarCountFromClock( AppManager.getCurrentClock() );
            if ( current > 0 ) {
                current--;
            }
            int new_clock = AppManager.getVsqFile().getClockFromBarCount( current );
            AppManager.setCurrentClock( new_clock );
            ensureCursorVisible();
            AppManager.setPlaying( playing );
            refreshScreen();
        }

        /// <summary>
        /// cMenuPianoの固定長音符入力の各メニューのチェック状態をm_pencil_modeを元に更新します
        /// </summary>
        private void updateCMenuPianoFixed() {
            cMenuPianoFixed01.setSelected( false );
            cMenuPianoFixed02.setSelected( false );
            cMenuPianoFixed04.setSelected( false );
            cMenuPianoFixed08.setSelected( false );
            cMenuPianoFixed16.setSelected( false );
            cMenuPianoFixed32.setSelected( false );
            cMenuPianoFixed64.setSelected( false );
            cMenuPianoFixed128.setSelected( false );
            cMenuPianoFixedOff.setSelected( false );
            cMenuPianoFixedTriplet.setSelected( false );
            cMenuPianoFixedDotted.setSelected( false );
            switch ( m_pencil_mode.getMode() ) {
                case PencilModeEnum.L1:
                    cMenuPianoFixed01.setSelected( true );
                    break;
                case PencilModeEnum.L2:
                    cMenuPianoFixed02.setSelected( true );
                    break;
                case PencilModeEnum.L4:
                    cMenuPianoFixed04.setSelected( true );
                    break;
                case PencilModeEnum.L8:
                    cMenuPianoFixed08.setSelected( true );
                    break;
                case PencilModeEnum.L16:
                    cMenuPianoFixed16.setSelected( true );
                    break;
                case PencilModeEnum.L32:
                    cMenuPianoFixed32.setSelected( true );
                    break;
                case PencilModeEnum.L64:
                    cMenuPianoFixed64.setSelected( true );
                    break;
                case PencilModeEnum.L128:
                    cMenuPianoFixed128.setSelected( true );
                    break;
                case PencilModeEnum.Off:
                    cMenuPianoFixedOff.setSelected( true );
                    break;
            }
            cMenuPianoFixedTriplet.setSelected( m_pencil_mode.isTriplet() );
            cMenuPianoFixedDotted.setSelected( m_pencil_mode.isDot() );
        }

        private void clearTempWave() {
            String tmppath = AppManager.getTempWaveDir();

            for ( int i = 1; i <= 16; i++ ) {
                String file = PortUtil.combinePath( tmppath, i + ".wav" );
                if ( PortUtil.isFileExists( file ) ) {
                    for ( int error = 0; error < 100; error++ ) {
                        try {
                            PortUtil.deleteFile( file );
                            break;
                        } catch ( Exception ex ) {
#if DEBUG
                            bocoree.debug.push_log( "FormMain+ClearTempWave()" );
                            bocoree.debug.push_log( "    ex=" + ex.ToString() );
                            bocoree.debug.push_log( "    error_count=" + error );
#endif
                            System.Threading.Thread.Sleep( 100 );
                        }
                    }
                }
            }
            String whd = PortUtil.combinePath( tmppath, UtauRenderingRunner.FILEBASE + ".whd" );
            if ( PortUtil.isFileExists( whd ) ) {
                try {
                    PortUtil.deleteFile( whd );
                } catch ( Exception ex ) {
                }
            }
            String dat = PortUtil.combinePath( tmppath, UtauRenderingRunner.FILEBASE + ".dat" );
            if ( PortUtil.isFileExists( dat ) ) {
                try {
                    PortUtil.deleteFile( dat );
                } catch ( Exception ex ) {
                }
            }
        }

        private void render( int[] tracks ) {
            String tmppath = AppManager.getTempWaveDir();
            if ( !PortUtil.isDirectoryExists( tmppath ) ) {
                System.IO.Directory.CreateDirectory( tmppath );
            }
            String[] files = new String[tracks.Length];
            for ( int i = 0; i < tracks.Length; i++ ) {
                files[i] = PortUtil.combinePath( tmppath, tracks[i] + ".wav" );
            }
            FormSynthesize dlg = null;
            try {
                dlg = new FormSynthesize( AppManager.getVsqFile(), AppManager.editorConfig.PreSendTime, tracks, files, AppManager.getVsqFile().TotalClocks + 240, false );
                if ( dlg.showDialog() == BDialogResult.OK ) {
                    AppManager.getVsqFile().Track.get( AppManager.getSelected() ).resetEditedArea();
                }
                int[] finished = dlg.getFinished();
                for ( int i = 0; i < finished.Length; i++ ) {
                    AppManager.setRenderRequired( finished[i], false );
                }
            } catch ( Exception ex ) {
            } finally {
                if ( dlg != null ) {
                    try {
                        dlg.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        private void playPreviewSound( int note ) {
            KeySoundPlayer.Play( note );
        }

#if ENABLE_MOUSEHOVER
        private void MouseHoverEventGenerator( Object arg ) {
            int note = (int)arg;
            if ( AppManager.editorConfig.MouseHoverTime > 0 ) {
                Thread.Sleep( AppManager.editorConfig.MouseHoverTime );
            }
            KeySoundPlayer.Play( note );
        }
#endif

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public void applyLanguage() {
            openXmlVsqDialog.clearChoosableFileFilter();
            try {
                openXmlVsqDialog.addFileFilter( _( "XML-VSQ Format(*.xvsq)|*.xvsq" ) );
                openXmlVsqDialog.addFileFilter( _( "All Files(*.*)|*.*" ) );
            } catch ( Exception ex ) {
                openXmlVsqDialog.addFileFilter( "XML-VSQ Format(*.xvsq)|*.xvsq" );
                openXmlVsqDialog.addFileFilter( "All Files(*.*)|*.*" );
            }

            saveXmlVsqDialog.clearChoosableFileFilter();
            try {
                saveXmlVsqDialog.addFileFilter( _( "XML-VSQ Format(*.xvsq)|*.xvsq" ) );
                saveXmlVsqDialog.addFileFilter( _( "All Files(*.*)|*.*" ) );
            } catch ( Exception ex ) {
                saveXmlVsqDialog.addFileFilter( "XML-VSQ Format(*.xvsq)|*.xvsq" );
                saveXmlVsqDialog.addFileFilter( "All Files(*.*)|*.*" );
            }

            openUstDialog.clearChoosableFileFilter();
            try {
                openUstDialog.addFileFilter( _( "UTAU Script Format(*.ust)|*.ust" ) );
                openUstDialog.addFileFilter( _( "All Files(*.*)|*.*" ) );
            } catch ( Exception ex ) {
                openUstDialog.addFileFilter( "UTAU Script Format(*.ust)|*.ust" );
                openUstDialog.addFileFilter( "All Files(*.*)|*.*" );
            }

            openMidiDialog.clearChoosableFileFilter();
            try {
                openMidiDialog.addFileFilter( _( "MIDI Format(*.mid)|*.mid" ) );
                openMidiDialog.addFileFilter( _( "VSQ Format(*.vsq)|*.vsq" ) );
                openMidiDialog.addFileFilter( _( "All Files(*.*)|*.*" ) );
            } catch ( Exception ex ) {
                openMidiDialog.addFileFilter( "MIDI Format(*.mid)|*.mid" );
                openMidiDialog.addFileFilter( "VSQ Format(*.vsq)|*.vsq" );
                openMidiDialog.addFileFilter( "All Files(*.*)|*.*" );
            }

            saveMidiDialog.clearChoosableFileFilter();
            try {
                saveMidiDialog.addFileFilter( _( "MIDI Format(*.mid)|*.mid" ) );
                saveMidiDialog.addFileFilter( _( "VSQ Format(*.vsq)|*.vsq" ) );
                saveMidiDialog.addFileFilter( _( "All Files(*.*)|*.*" ) );
            } catch ( Exception ex ) {
                saveMidiDialog.addFileFilter( "MIDI Format(*.mid)|*.mid" );
                saveMidiDialog.addFileFilter( "VSQ Format(*.vsq)|*.vsq" );
                saveMidiDialog.addFileFilter( "All Files(*.*)|*.*" );
            }

            openWaveDialog.clearChoosableFileFilter();
            try {
                openWaveDialog.addFileFilter( _( "Wave File(*.wav)|*.wav" ) );
                openWaveDialog.addFileFilter( _( "All Files(*.*)|*.*" ) );
            } catch ( Exception ex ) {
                openWaveDialog.addFileFilter( "Wave File(*.wav)|*.wav" );
                openWaveDialog.addFileFilter( "All Files(*.*)|*.*" );
            }

            stripLblGameCtrlMode.setToolTipText( _( "Game Controler" ) );
            this.Invoke( new BEventHandler( updateGameControlerStatus ) );

            stripBtnPointer.setText( _( "Pointer" ) );
            stripBtnPointer.setToolTipText( _( "Pointer" ) );
            stripBtnPencil.setText( _( "Pencil" ) );
            stripBtnPencil.setToolTipText( _( "Pencil" ) );
            stripBtnLine.setText( _( "Line" ) );
            stripBtnLine.setToolTipText( _( "Line" ) );
            stripBtnEraser.setText( _( "Eraser" ) );
            stripBtnEraser.setToolTipText( _( "Eraser" ) );
            stripBtnCurve.setText( _( "Curve" ) );
            stripBtnCurve.setToolTipText( _( "Curve" ) );
            stripBtnGrid.setText( _( "Grid" ) );
            stripBtnGrid.setToolTipText( _( "Grid" ) );

            #region main menu
            menuFile.setText( _( "File" ) + "(&F)" );
            menuFileNew.setText( _( "New" ) + "(&N)" );
            menuFileOpen.setText( _( "Open" ) + "(&O)" );
            menuFileOpenVsq.setText( _( "Open VSQ/Vocaloid Midi" ) + "(&V)" );
            menuFileOpenUst.setText( _( "Open UTAU Project File" ) + "(&U)" );
            menuFileSave.setText( _( "Save" ) + "(&S)" );
            menuFileSaveNamed.setText( _( "Save As" ) + "(&A)" );
            menuFileImport.setText( _( "Import" ) + "(&I)" );
            menuFileImportVsq.setText( _( "VSQ / Vocaloid Midi" ) );
            menuFileExport.setText( _( "Export" ) + "(&E)" );
            menuFileRecent.setText( _( "Recent Files" ) + "(&R)" );
            menuFileQuit.setText( _( "Quit" ) + "(&Q)" );

            menuEdit.setText( _( "Edit" ) + "(&E)" );
            menuEditUndo.setText( _( "Undo" ) + "(&U)" );
            menuEditRedo.setText( _( "Redo" ) + "(&R)" );
            menuEditCut.setText( _( "Cut" ) + "(&T)" );
            menuEditCopy.setText( _( "Copy" ) + "(&C)" );
            menuEditPaste.setText( _( "Paste" ) + "(&P)" );
            menuEditDelete.setText( _( "Delete" ) + "(&D)" );
            menuEditAutoNormalizeMode.setText( _( "Auto Normalize Mode" ) + "(&N)" );
            menuEditSelectAll.setText( _( "Select All" ) + "(&A)" );
            menuEditSelectAllEvents.setText( _( "Select All Events" ) + "(&E)" );

            menuVisual.setText( _( "View" ) + "(&V)" );
            menuVisualControlTrack.setText( _( "Control Track" ) + "(&C)" );
            menuVisualMixer.setText( _( "Mixer" ) + "(&X)" );
            menuVisualWaveform.setText( _( "Waveform" ) + "(&W)" );
            menuVisualProperty.setText( _( "Property Window" ) );
            menuVisualOverview.setText( _( "Navigation" ) + "(&V)" );
            menuVisualGridline.setText( _( "Grid Line" ) + "(&G)" );
            menuVisualStartMarker.setText( _( "Start Marker" ) + "(&S)" );
            menuVisualEndMarker.setText( _( "End Marker" ) + "(&E)" );
            menuVisualLyrics.setText( _( "Lyrics/Phoneme" ) + "(&L)" );
            menuVisualNoteProperty.setText( _( "Note Expression/Vibrato" ) + "(&N)" );
            menuVisualPitchLine.setText( _( "Pitch Line" ) + "(&P)" );

            menuJob.setText( _( "Job" ) + "(&J)" );
            menuJobNormalize.setText( _( "Normalize Notes" ) + "(&N)" );
            menuJobInsertBar.setText( _( "Insert Bars" ) + "(&I)" );
            menuJobDeleteBar.setText( _( "Delete Bars" ) + "(&D)" );
            menuJobRandomize.setText( _( "Randomize" ) + "(&R)" );
            menuJobConnect.setText( _( "Connect Notes" ) + "(&C)" );
            menuJobLyric.setText( _( "Insert Lyrics" ) + "(&L)" );
            menuJobRewire.setText( _( "Import ReWire Host Tempo" ) + "(&T)" );
            menuJobRealTime.setText( _( "Start Realtime Input" ) );
            menuJobReloadVsti.setText( _( "Reload VSTi" ) + "(&R)" );

            menuTrack.setText( _( "Track" ) + "(&T)" );
            menuTrackOn.setText( _( "Track On" ) + "(&K)" );
            menuTrackAdd.setText( _( "Add Track" ) + "(&A)" );
            menuTrackCopy.setText( _( "Copy Track" ) + "(&C)" );
            menuTrackChangeName.setText( _( "Rename Track" ) + "(&R)" );
            menuTrackDelete.setText( _( "Delete Track" ) + "(&D)" );
            menuTrackRenderCurrent.setText( _( "Render Current Track" ) + "(&T)" );
            menuTrackRenderAll.setText( _( "Render All Tracks" ) + "(&S)" );
            menuTrackOverlay.setText( _( "Overlay" ) + "(&O)" );
            menuTrackRenderer.setText( _( "Renderer" ) );

            menuLyric.setText( _( "Lyrics" ) + "(&L)" );
            menuLyricExpressionProperty.setText( _( "Note Expression Property" ) + "(&E)" );
            menuLyricVibratoProperty.setText( _( "Note Vibrato Property" ) + "(&V)" );
            menuLyricSymbol.setText( _( "Phoneme Transformation" ) + "(&T)" );
            menuLyricDictionary.setText( _( "User Word Dictionary" ) + "(&C)" );

            menuScript.setText( _( "Script" ) + "(&C)" );
            menuScriptUpdate.setText( _( "Update Script List" ) + "(&U)" );

            menuSetting.setText( _( "Setting" ) + "(&S)" );
            menuSettingPreference.setText( _( "Preference" ) + "(&P)" );
            menuSettingGameControler.setText( _( "Game Controler" ) + "(&G)" );
            menuSettingGameControlerLoad.setText( _( "Load" ) + "(&L)" );
            menuSettingGameControlerRemove.setText( _( "Remove" ) + "(&R)" );
            menuSettingGameControlerSetting.setText( _( "Setting" ) + "(&S)" );
            menuSettingShortcut.setText( _( "Shortcut Key" ) + "(&S)" );
            menuSettingUtauVoiceDB.setText( _( "UTAU Voice DB" ) + "(&U)" );
            menuSettingDefaultSingerStyle.setText( _( "Singing Style Defaults" ) + "(&D)" );
            menuSettingPositionQuantize.setText( _( "Quantize" ) + "(&Q)" );
            menuSettingPositionQuantizeOff.setText( _( "Off" ) );
            menuSettingPositionQuantizeTriplet.setText( _( "Triplet" ) );
            menuSettingLengthQuantize.setText( _( "Length" ) + "(&L)" );
            menuSettingLengthQuantizeOff.setText( _( "Off" ) );
            menuSettingLengthQuantizeTriplet.setText( _( "Triplet" ) );
            menuSettingSingerProperty.setText( _( "Singer Properties" ) + "(&S)" );
            menuSettingPaletteTool.setText( _( "Palette Tool" ) + "(&T)" );

            menuHelp.setText( _( "Help" ) + "(&H)" );
            menuHelpAbout.setText( _( "About Cadencii" ) + "(&A)" );

            menuHiddenEditLyric.setText( _( "Start Lyric Input" ) );
            menuHiddenEditFlipToolPointerEraser.setText( _( "Chagne Tool Pointer / Eraser" ) );
            menuHiddenEditFlipToolPointerPencil.setText( _( "Change Tool Pointer / Pencil" ) );
            menuHiddenTrackBack.setText( _( "Previous Track" ) );
            menuHiddenTrackNext.setText( _( "Next Track" ) );
            menuHiddenVisualBackwardParameter.setText( _( "Previous Control Curve" ) );
            menuHiddenVisualForwardParameter.setText( _( "Next Control Curve" ) );
            #endregion

            #region cMenuPiano
            cMenuPianoPointer.setText( _( "Arrow" ) + "(&A)" );
            cMenuPianoPencil.setText( _( "Pencil" ) + "(&W)" );
            cMenuPianoEraser.setText( _( "Eraser" ) + "(&E)" );
            cMenuPianoPaletteTool.setText( _( "Palette Tool" ) );

            cMenuPianoCurve.setText( _( "Curve" ) + "(&V)" );

            cMenuPianoFixed.setText( _( "Note Fixed Length" ) + "(&N)" );
            cMenuPianoFixedTriplet.setText( _( "Triplet" ) );
            cMenuPianoFixedOff.setText( _( "Off" ) );
            cMenuPianoFixedDotted.setText( _( "Dot" ) );
            cMenuPianoQuantize.setText( _( "Quantize" ) + "(&Q)" );
            cMenuPianoQuantizeTriplet.setText( _( "Triplet" ) );
            cMenuPianoQuantizeOff.setText( _( "Off" ) );
            cMenuPianoLength.setText( _( "Length" ) + "(&L)" );
            cMenuPianoLengthTriplet.setText( _( "Triplet" ) );
            cMenuPianoLengthOff.setText( _( "Off" ) );
            cMenuPianoGrid.setText( _( "Show/Hide Grid Line" ) + "(&S)" );

            cMenuPianoUndo.setText( _( "Undo" ) + "(&U)" );
            cMenuPianoRedo.setText( _( "Redo" ) + "(&R)" );

            cMenuPianoCut.setText( _( "Cut" ) + "(&T)" );
            cMenuPianoPaste.setText( _( "Paste" ) + "(&P)" );
            cMenuPianoCopy.setText( _( "Copy" ) + "(&C)" );
            cMenuPianoDelete.setText( _( "Delete" ) + "(&D)" );

            cMenuPianoSelectAll.setText( _( "Select All" ) + "(&A)" );
            cMenuPianoSelectAllEvents.setText( _( "Select All Events" ) + "(&E)" );

            cMenuPianoExpressionProperty.setText( _( "Note Expression Property" ) + "(&P)" );
            cMenuPianoVibratoProperty.setText( _( "Note Vibrato Property" ) );
            cMenuPianoImportLyric.setText( _( "Insert Lyrics" ) + "(&P)" );
            #endregion

            #region cMenuTrackTab
            cMenuTrackTabTrackOn.setText( _( "Track On" ) + "(&K)" );
            cMenuTrackTabAdd.setText( _( "Add Track" ) + "(&A)" );
            cMenuTrackTabCopy.setText( _( "Copy Track" ) + "(&C)" );
            cMenuTrackTabChangeName.setText( _( "Rename Track" ) + "(&R)" );
            cMenuTrackTabDelete.setText( _( "Delete Track" ) + "(&D)" );

            cMenuTrackTabRenderCurrent.setText( _( "Render Current Track" ) + "(&T)" );
            cMenuTrackTabRenderAll.setText( _( "Render All Tracks" ) + "(&S)" );
            cMenuTrackTabOverlay.setText( _( "Overlay" ) + "(&O)" );
            cMenuTrackTabRenderer.setText( _( "Renderer" ) );
            #endregion

            #region cMenuTrackSelector
            cMenuTrackSelectorPointer.setText( _( "Arrow" ) + "(&A)" );
            cMenuTrackSelectorPencil.setText( _( "Pencil" ) + "(&W)" );
            cMenuTrackSelectorLine.setText( _( "Line" ) + "(&L)" );
            cMenuTrackSelectorEraser.setText( _( "Eraser" ) + "(&E)" );
            cMenuTrackSelectorPaletteTool.setText( _( "Palette Tool" ) );

            cMenuTrackSelectorCurve.setText( _( "Curve" ) + "(&V)" );

            cMenuTrackSelectorUndo.setText( _( "Undo" ) + "(&U)" );
            cMenuTrackSelectorRedo.setText( _( "Redo" ) + "(&R)" );

            cMenuTrackSelectorCut.setText( _( "Cut" ) + "(&T)" );
            cMenuTrackSelectorCopy.setText( _( "Copy" ) + "(&C)" );
            cMenuTrackSelectorPaste.setText( _( "Paste" ) + "(&P)" );
            cMenuTrackSelectorDelete.setText( _( "Delete" ) + "(&D)" );
            cMenuTrackSelectorDeleteBezier.setText( _( "Delete Bezier Point" ) + "(&B)" );

            cMenuTrackSelectorSelectAll.setText( _( "Select All Events" ) + "(&E)" );
            #endregion

            stripLblGameCtrlMode.setToolTipText( _( "Game Controler" ) );

            // Palette Tool
#if DEBUG
            AppManager.debugWriteLine( "FormMain.ApplyLanguage; Messaging.Language=" + Messaging.getLanguage() );
#endif
#if ENABLE_SCRIPT
            int count = toolStripTool.getComponentCount();
            for ( int i = 0; i < count; i++ ){
                Object tsi = toolStripTool.getComponentAtIndex( i );
                if ( tsi is BToolStripButton ) {
                    BToolStripButton tsb = (BToolStripButton)tsi;
                    if ( tsb.getTag() != null && tsb.getTag() is String ) {
                        String id = (String)tsb.getTag();
                        if ( PaletteToolServer.LoadedTools.containsKey( id ) ) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.LoadedTools.get( id );
                            tsb.setText( ipt.getName( Messaging.getLanguage() ) );
                            tsb.setToolTipText( ipt.getDescription( Messaging.getLanguage() ) );
                        }
                    }
                }
            }

            foreach ( MenuElement tsi in cMenuPianoPaletteTool.getSubElements() ) {
                if ( tsi is BMenuItem ) {
                    BMenuItem tsmi = (BMenuItem)tsi;
                    if ( tsmi.getTag() != null && tsmi.getTag() is String ) {
                        String id = (String)tsmi.getTag();
                        if ( PaletteToolServer.LoadedTools.containsKey( id ) ) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.LoadedTools.get( id );
                            tsmi.setText( ipt.getName( Messaging.getLanguage() ) );
                            tsmi.setToolTipText( ipt.getDescription( Messaging.getLanguage() ) );
                        }
                    }
                }
            }

            foreach ( MenuElement tsi in cMenuTrackSelectorPaletteTool.getSubElements() ) {
                if ( tsi is BMenuItem ) {
                    BMenuItem tsmi = (BMenuItem)tsi;
                    if ( tsmi.getTag() != null && tsmi.getTag() is String ) {
                        String id = (String)tsmi.getTag();
                        if ( PaletteToolServer.LoadedTools.containsKey( id ) ) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.LoadedTools.get( id );
                            tsmi.setText( ipt.getName( Messaging.getLanguage() ) );
                            tsmi.setToolTipText( ipt.getDescription( Messaging.getLanguage() ) );
                        }
                    }
                }
            }

            foreach ( MenuElement tsi in menuSettingPaletteTool.getSubElements() ) {
                if ( tsi is BMenuItem ) {
                    BMenuItem tsmi = (BMenuItem)tsi;
                    if ( tsmi.getTag() != null && tsmi.getTag() is String ) {
                        String id = (String)tsmi.getTag();
                        if ( PaletteToolServer.LoadedTools.containsKey( id ) ) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.LoadedTools.get( id );
                            tsmi.setText( ipt.getName( Messaging.getLanguage() ) );
                        }
                    }
                }
            }

            for ( Iterator itr = PaletteToolServer.LoadedTools.keySet().iterator(); itr.hasNext(); ) {
                String id = (String)itr.next();
                IPaletteTool ipt = (IPaletteTool)PaletteToolServer.LoadedTools.get( id );
                ipt.applyLanguage( Messaging.getLanguage() );
            }
#endif

            updateStripDDBtnSpeed();
        }

        private void importLyric() {
#if DEBUG
            AppManager.debugWriteLine( "ImportLyric" );
#endif
            int start = 0;
            int selectedid = AppManager.getLastSelectedEvent().original.InternalID;
            for ( int i = 0; i < AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventCount(); i++ ) {
                if ( selectedid == AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEvent( i ).InternalID ) {
                    start = i;
                    break;
                }
            }
            int count = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventCount() - 1 - start + 1;
#if DEBUG
            AppManager.debugWriteLine( "    count=" + count );
#endif
            FormImportLyric dlg = null;
            try {
                dlg = new FormImportLyric( count );
                dlg.setLocation( getFormPreferedLocation( dlg ) );
                if ( dlg.showDialog() == BDialogResult.OK ) {
                    String[] phrases = dlg.GetLetters();
#if DEBUG
                    for ( int i = 0; i < phrases.Length; i++ ) {
                        AppManager.debugWriteLine( "    " + phrases[i] );
                    }
#endif
                    int min = Math.Min( count, phrases.Length );
                    String[] new_phrases = new String[min];
                    String[] new_symbols = new String[min];
                    for ( int i = 0; i < min; i++ ) {
                        new_phrases[i] = phrases[i];
                        ByRef<String> symb = new ByRef<String>( "" );
                        SymbolTable.attatch( phrases[i], symb );
                        new_symbols[i] = symb.value;
                    }
                    VsqID[] new_ids = new VsqID[min];
                    int[] ids = new int[min];
                    for ( int i = start; i < start + min; i++ ) {
                        new_ids[i - start] = (VsqID)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEvent( i ).ID.clone();
                        new_ids[i - start].LyricHandle.L0.Phrase = new_phrases[i - start];
                        new_ids[i - start].LyricHandle.L0.setPhoneticSymbol( new_symbols[i - start] );
                        ids[i - start] = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEvent( i ).InternalID;
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeIDContaintsRange( AppManager.getSelected(), ids, new_ids ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    setEdited( true );
                    repaint();
                }
            } catch ( Exception ex ) {
            } finally {
                if ( dlg != null ) {
                    try {
                        dlg.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        private void editNoteVibratoProperty() {
            SelectedEventEntry item = AppManager.getLastSelectedEvent();
            if ( item == null ) {
                return;
            }

            VsqEvent ev = item.original;
            SynthesizerType type = SynthesizerType.VOCALOID2;
            if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                type = SynthesizerType.VOCALOID1;
            }
            FormVibratoConfig dlg = null;
            try {
                dlg = new FormVibratoConfig( ev.ID.VibratoHandle, ev.ID.getLength(), AppManager.editorConfig.DefaultVibratoLength, type );
                dlg.setLocation( getFormPreferedLocation( dlg ) );
                if ( dlg.showDialog() == BDialogResult.OK ) {
                    VsqEvent edited = (VsqEvent)ev.clone();
                    if ( dlg.getVibratoHandle() != null ) {
                        edited.ID.VibratoHandle = (VibratoHandle)dlg.getVibratoHandle().clone();
                        edited.ID.VibratoDelay = ev.ID.getLength() - dlg.getVibratoHandle().getLength();
                    } else {
                        edited.ID.VibratoHandle = null;
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(), ev.InternalID, (VsqID)edited.ID.clone() ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    setEdited( true );
                    refreshScreen();
                }
            } catch ( Exception ex ) {
            } finally {
                if ( dlg != null ) {
                    try {
                        dlg.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        private void editNoteExpressionProperty() {
            SelectedEventEntry item = AppManager.getLastSelectedEvent();
            if ( item == null ) {
                return;
            }

            VsqEvent ev = item.original;
            SynthesizerType type = SynthesizerType.VOCALOID2;
            if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                type = SynthesizerType.VOCALOID1;
            }
            FormNoteExpressionConfig dlg = null;
            try {
                dlg = new FormNoteExpressionConfig( type, ev.ID.NoteHeadHandle );
                dlg.setPMBendDepth( ev.ID.PMBendDepth );
                dlg.setPMBendLength( ev.ID.PMBendLength );
                dlg.setPMbPortamentoUse( ev.ID.PMbPortamentoUse );
                dlg.setDEMdecGainRate( ev.ID.DEMdecGainRate );
                dlg.setDEMaccent( ev.ID.DEMaccent );

                dlg.setLocation( getFormPreferedLocation( dlg ) );

                if ( dlg.showDialog() == BDialogResult.OK ) {
                    VsqEvent edited = (VsqEvent)ev.clone();
                    edited.ID.PMBendDepth = dlg.getPMBendDepth();
                    edited.ID.PMBendLength = dlg.getPMBendLength();
                    edited.ID.PMbPortamentoUse = dlg.getPMbPortamentoUse();
                    edited.ID.DEMdecGainRate = dlg.getDEMdecGainRate();
                    edited.ID.DEMaccent = dlg.getDEMaccent();
                    edited.ID.NoteHeadHandle = dlg.getEditedNoteHeadHandle();
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(), ev.InternalID, (VsqID)edited.ID.clone() ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    setEdited( true );
                    refreshScreen();
                }
            } catch ( Exception ex ) {
            } finally {
                if ( dlg != null ) {
                    try {
                        dlg.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        private int computeScrollValueFromWheelDelta( int delta ) {
            double new_val = (double)hScroll.getValue() - delta * AppManager.editorConfig.WheelOrder / (5.0 * AppManager.scaleX);
            if ( new_val < 0.0 ) {
                new_val = 0;
            }
            int draft = (int)new_val;
            if ( draft > hScroll.getMaximum() ) {
                draft = hScroll.getMaximum();
            } else if ( draft < hScroll.getMinimum() ) {
                draft = hScroll.getMinimum();
            }
            return draft;
        }

        #region 音符の編集関連
        public void selectAll() {
            AppManager.clearSelectedEvent();
            AppManager.clearSelectedTempo();
            AppManager.clearSelectedTimesig();
            AppManager.clearSelectedPoint();
            int min = int.MaxValue;
            int max = int.MinValue;
            int premeasure = AppManager.getVsqFile().getPreMeasureClocks();
            Vector<Integer> add_required = new Vector<Integer>();
            for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = (VsqEvent)itr.next();
                if ( premeasure <= ve.Clock ) {
                    add_required.add( ve.InternalID );
                    min = Math.Min( min, ve.Clock );
                    max = Math.Max( max, ve.Clock + ve.ID.getLength() );
                }
            }
            if ( add_required.size() > 0 ) {
                AppManager.addSelectedEventAll( add_required );
            }
            foreach ( CurveType vct in AppManager.CURVE_USAGE ) {
                if ( vct.isScalar() || vct.isAttachNote() ) {
                    continue;
                }
                VsqBPList target = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( vct.getName() );
                int count = target.size();
                if ( count >= 1 ) {
                    //int[] keys = target.getKeys();
                    int max_key = target.getKeyClock( count - 1 );
                    max = Math.Max( max, target.getValue( max_key ) );
                    for ( int i = 0; i < count; i++ ) {
                        int key = target.getKeyClock( i );
                        if ( premeasure <= key ) {
                            min = Math.Min( min, key );
                            break;
                        }
                    }
                }
            }
            if ( min < premeasure ) {
                min = premeasure;
            }
            if ( min < max ) {
                //int stdx = AppManager.startToDrawX;
                //min = xCoordFromClocks( min ) + stdx;
                //max = xCoordFromClocks( max ) + stdx;
                AppManager.wholeSelectedInterval = new SelectedRegion( min );
                AppManager.wholeSelectedInterval.setEnd( max );
                AppManager.setWholeSelectedIntervalEnabled( true );
            }
        }

        public void selectAllEvent() {
            AppManager.clearSelectedTempo();
            AppManager.clearSelectedTimesig();
            AppManager.clearSelectedEvent();
            AppManager.clearSelectedPoint();
            int premeasureclock = AppManager.getVsqFile().getPreMeasureClocks();
            Vector<Integer> add_required = new Vector<Integer>();
            for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent ev = (VsqEvent)itr.next();
                if ( ev.ID.type == VsqIDType.Anote && ev.Clock >= premeasureclock ) {
                    add_required.add( ev.InternalID );
                }
            }
            if ( add_required.size() > 0 ) {
                AppManager.addSelectedEventAll( add_required );
            }
            refreshScreen();
        }

        private void deleteEvent() {
#if DEBUG
            AppManager.debugWriteLine( "DeleteEvent()" );
            AppManager.debugWriteLine( "    AppManager.inputTextBox.isEnabled()=" + AppManager.inputTextBox.Enabled );
#endif

            if ( !AppManager.inputTextBox.isVisible() ) {
                if ( AppManager.getSelectedEventCount() > 0 ) {
                    Vector<Integer> ids = new Vector<Integer>();
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                        SelectedEventEntry ev = (SelectedEventEntry)itr.next();
                        ids.add( ev.original.InternalID );
                    }
                    VsqCommand run = VsqCommand.generateCommandEventDeleteRange( AppManager.getSelected(), ids );
                    if ( AppManager.isWholeSelectedIntervalEnabled() ) {
                        VsqFileEx work = (VsqFileEx)AppManager.getVsqFile().Clone();
                        work.executeCommand( run );
                        int stdx = AppManager.startToDrawX;
                        int start_clock = AppManager.wholeSelectedInterval.getStart();
                        int end_clock = AppManager.wholeSelectedInterval.getEnd();
                        Vector<Vector<BPPair>> curves = new Vector<Vector<BPPair>>();
                        Vector<CurveType> types = new Vector<CurveType>();
                        foreach ( CurveType vct in AppManager.CURVE_USAGE ) {
                            if ( vct.isScalar() || vct.isAttachNote() ) {
                                continue;
                            }
                            Vector<BPPair> t = new Vector<BPPair>();
                            t.add( new BPPair( start_clock, work.Track.get( AppManager.getSelected() ).getCurve( vct.getName() ).getValue( start_clock ) ) );
                            t.add( new BPPair( end_clock, work.Track.get( AppManager.getSelected() ).getCurve( vct.getName() ).getValue( end_clock ) ) );
                            curves.add( t );
                            types.add( vct );
                        }
                        Vector<String> strs = new Vector<String>();
                        for ( int i = 0; i < types.size(); i++ ) {
                            strs.add( types.get( i ).getName() );
                        }
                        CadenciiCommand delete_curve = new CadenciiCommand( VsqCommand.generateCommandTrackCurveEditRange( AppManager.getSelected(),
                                                                                                                           strs,
                                                                                                                           curves ) );
                        work.executeCommand( delete_curve );
                        CadenciiCommand run2 = new CadenciiCommand( VsqCommand.generateCommandReplace( work ) );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run2 ) );
                        setEdited( true );
                    } else {
                        CadenciiCommand run2 = new CadenciiCommand( run );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run2 ) );
                        setEdited( true );
                        AppManager.clearSelectedEvent();
                    }
                    repaint();
                } else if ( AppManager.getSelectedTempoCount() > 0 ) {
                    Vector<Integer> clocks = new Vector<Integer>();
                    for ( Iterator itr = AppManager.getSelectedTempoIterator(); itr.hasNext(); ) {
                        ValuePair<Integer, SelectedTempoEntry> item = (ValuePair<Integer, SelectedTempoEntry>)itr.next();
                        //SelectedTempoEntry value = AppManager.getSelectedTempo().get( key );
                        if ( item.getKey() <= 0 ) {
                            statusLabel.setText( _( "Cannot remove first symbol of track!" ) );
#if !JAVA
                            SystemSounds.Asterisk.Play();
#endif
                            return;
                        }
                        clocks.add( item.getKey() );
                    }
                    int[] dum = new int[clocks.size()];
                    for ( int i = 0; i < dum.Length; i++ ) {
                        dum[i] = -1;
                    }
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandUpdateTempoRange( clocks.toArray( new Integer[] { } ), clocks.toArray( new Integer[] { } ), dum ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    setEdited( true );
                    AppManager.clearSelectedTempo();
                    repaint();
                } else if ( AppManager.getSelectedTimesigCount() > 0 ) {
#if DEBUG
                    AppManager.debugWriteLine( "    Timesig" );
#endif
                    int[] barcounts = new int[AppManager.getSelectedTimesigCount()];
                    int[] numerators = new int[AppManager.getSelectedTimesigCount()];
                    int[] denominators = new int[AppManager.getSelectedTimesigCount()];
                    int count = -1;
                    for ( Iterator itr = AppManager.getSelectedTimesigIterator(); itr.hasNext(); ) {
                        ValuePair<Integer, SelectedTimesigEntry> item = (ValuePair<Integer, SelectedTimesigEntry>)itr.next();
                        int key = item.getKey();
                        SelectedTimesigEntry value = item.getValue();
                        count++;
                        barcounts[count] = key;
                        if ( key <= 0 ) {
                            statusLabel.setText( _( "Cannot remove first symbol of track!" ) );
#if !JAVA
                            SystemSounds.Asterisk.Play();
#endif
                            return;
                        }
                        numerators[count] = -1;
                        denominators[count] = -1;
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandUpdateTimesigRange( barcounts, barcounts, numerators, denominators ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    setEdited( true );
                    AppManager.clearSelectedTimesig();
                    repaint();
                }
                if ( AppManager.getSelectedPointIDCount() > 0 ) {
#if DEBUG
                    AppManager.debugWriteLine( "    Curve" );
#endif
                    String curve;
                    if ( !trackSelector.getSelectedCurve().isAttachNote() ) {
                        curve = trackSelector.getSelectedCurve().getName();
                        VsqBPList list = (VsqBPList)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( curve ).clone();
                        Vector<Integer> remove_clock_queue = new Vector<Integer>();
                        int count = list.size();
                        for ( int i = 0; i < count; i++ ) {
                            VsqBPPair item = list.getElementB( i );
                            if ( AppManager.isSelectedPointContains( item.id ) ) {
                                remove_clock_queue.add( list.getKeyClock( i ) );
                            }
                        }
                        count = remove_clock_queue.size();
                        for ( int i = 0; i < count; i++ ) {
                            list.remove( remove_clock_queue.get( i ) );
                        }
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandTrackCurveReplace( AppManager.getSelected(),
                                                                                                                trackSelector.getSelectedCurve().getName(),
                                                                                                                list ) );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                        setEdited( true );
                    } else {
                        //todo: FormMain+DeleteEvent; VibratoDepth, VibratoRateの場合
                    }
                    AppManager.clearSelectedPoint();
                    refreshScreen();
                }
            }
        }

        private void pasteEvent() {
            int clock = AppManager.getCurrentClock();
            int unit = AppManager.getPositionQuantizeClock();
            int odd = clock % unit;
            clock -= odd;
            if ( odd > unit / 2 ) {
                clock += unit;
            }

            VsqCommand add_event = null; // VsqEventを追加するコマンド

            ClipboardEntry ce = AppManager.getCopiedItems();
            int copy_started_clock = ce.copyStartedClock;
            Vector<VsqEvent> copied_events = ce.events;
#if DEBUG
            PortUtil.println( "FormMain#pasteEvent; copy_started_clock=" + copy_started_clock );
            PortUtil.println( "FormMain#pasteEvent; copied_events.size()=" + copied_events.size() );
#endif
            if ( copied_events.size() != 0 ) {
                // VsqEventのペーストを行うコマンドを発行
                int dclock = clock - copy_started_clock;
                if ( clock >= AppManager.getVsqFile().getPreMeasureClocks() ) {
                    Vector<VsqEvent> paste = new Vector<VsqEvent>();
                    int count = copied_events.size();
                    for ( int i = 0; i < count; i++ ) {
                        VsqEvent item = (VsqEvent)copied_events.get( i ).clone();
                        item.Clock = copied_events.get( i ).Clock + dclock;
                        paste.add( item );
                    }
                    add_event = VsqCommand.generateCommandEventAddRange( AppManager.getSelected(), paste.toArray( new VsqEvent[] { } ) );
                }
            }
            Vector<TempoTableEntry> copied_tempo = ce.tempo;
            if ( copied_tempo.size() != 0 ) {
                // テンポ変更の貼付けを実行
                int dclock = clock - copy_started_clock;
                int count = copied_tempo.size();
                int[] clocks = new int[count];
                int[] tempos = new int[count];
                for ( int i = 0; i < count; i++ ) {
                    TempoTableEntry item = copied_tempo.get( i );
                    clocks[i] = item.Clock + dclock;
                    tempos[i] = item.Tempo;
                }
                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandUpdateTempoRange( clocks, clocks, tempos ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                setEdited( true );
                refreshScreen();
                return;
            }
            Vector<TimeSigTableEntry> copied_timesig = ce.timesig;
            if ( copied_timesig.size() > 0 ) {
                // 拍子変更の貼付けを実行
                int bar_count = AppManager.getVsqFile().getBarCountFromClock( clock );
                int min_barcount = copied_timesig.get( 0 ).BarCount;
                for ( Iterator itr = copied_timesig.iterator(); itr.hasNext(); ) {
                    TimeSigTableEntry tste = (TimeSigTableEntry)itr.next();
                    min_barcount = Math.Min( min_barcount, tste.BarCount );
                }
                int dbarcount = bar_count - min_barcount;
                int count = copied_timesig.size();
                int[] barcounts = new int[count];
                int[] numerators = new int[count];
                int[] denominators = new int[count];
                for ( int i = 0; i < count; i++ ) {
                    TimeSigTableEntry item = copied_timesig.get( i );
                    barcounts[i] = item.BarCount + dbarcount;
                    numerators[i] = item.Numerator;
                    denominators[i] = item.Denominator;
                }
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandUpdateTimesigRange( barcounts, barcounts, numerators, denominators ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                setEdited( true );
                refreshScreen();
                return;
            }

            // BPPairの貼付け
            VsqCommand edit_bpcurve = null; // BPListを変更するコマンド
            TreeMap<CurveType, VsqBPList> copied_curve = ce.points;
#if DEBUG
            PortUtil.println( "FormMain#pasteEvent; copied_curve.size()=" + copied_curve.size() );
#endif
            if ( copied_curve.size() > 0 ) {
                int dclock = clock - copy_started_clock;

                TreeMap<String, VsqBPList> work = new TreeMap<String, VsqBPList>();
                for ( Iterator itr = copied_curve.keySet().iterator(); itr.hasNext(); ) {
                    CurveType curve = (CurveType)itr.next();
                    VsqBPList list = copied_curve.get( curve );
#if DEBUG
                    AppManager.debugWriteLine( "FormMain#pasteEvent; curve=" + curve );
#endif
                    if ( curve.isScalar() ) {
                        continue;
                    }
                    if ( list.size() <= 0 ) {
                        continue;
                    }
                    if ( curve.isAttachNote() ) {
                        //todo: FormMain+PasteEvent; VibratoRate, VibratoDepthカーブのペースト処理
                    } else {
                        VsqBPList target = (VsqBPList)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( curve.getName() ).clone();
                        int count = list.size();
#if DEBUG
                        PortUtil.println( "FormMain#pasteEvent; list.getCount()=" + count );
#endif
                        int min = list.getKeyClock( 0 ) + dclock;
                        int max = list.getKeyClock( count - 1 ) + dclock;
                        for ( int i = 0; i < target.size(); i++ ) {
                            int cl = target.getKeyClock( i );
                            if ( min <= cl && cl <= max ) {
                                target.removeElementAt( i );
                                i--;
                            }
                        }
                        for ( int i = 0; i < count; i++ ) {
                            target.add( list.getKeyClock( i ) + dclock, list.getElementA( i ) );
                        }
                        if ( copied_curve.size() == 1 ) {
                            work.put( trackSelector.getSelectedCurve().getName(), target );
                        } else {
                            work.put( curve.getName(), target );
                        }
                    }
                }
#if DEBUG
                PortUtil.println( "FormMain#pasteEvent; work.size()=" + work.size() );
#endif
                if ( work.size() > 0 ) {
                    String[] curves = new String[work.size()];
                    VsqBPList[] bplists = new VsqBPList[work.size()];
                    int count = -1;
                    for ( Iterator itr = work.keySet().iterator(); itr.hasNext(); ) {
                        String s = (String)itr.next();
                        count++;
                        curves[count] = s;
                        bplists[count] = work.get( s );
                    }
                    edit_bpcurve = VsqCommand.generateCommandTrackCurveReplaceRange( AppManager.getSelected(), curves, bplists );
                }
                AppManager.clearSelectedPoint();
            }

            // ベジエ曲線の貼付け
            CadenciiCommand edit_bezier = null;
            TreeMap<CurveType, Vector<BezierChain>> copied_bezier = ce.beziers;
#if DEBUG
            PortUtil.println( "FormMain#pasteEvent; copied_bezier.size()=" + copied_bezier.size() );
#endif
            if ( copied_bezier.size() > 0 ) {
                int dclock = clock - copy_started_clock;
                BezierCurves attached_curve = (BezierCurves)AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).clone();
                TreeMap<CurveType, Vector<BezierChain>> command_arg = new TreeMap<CurveType, Vector<BezierChain>>();
                for ( Iterator itr = copied_bezier.keySet().iterator(); itr.hasNext(); ) {
                    CurveType curve = (CurveType)itr.next();
                    if ( curve.isScalar() ) {
                        continue;
                    }
                    for ( Iterator itr2 = copied_bezier.get( curve ).iterator(); itr2.hasNext(); ) {
                        BezierChain bc = (BezierChain)itr2.next();
                        BezierChain bc_copy = (BezierChain)bc.Clone();
                        for ( Iterator itr3 = bc_copy.points.iterator(); itr3.hasNext(); ) {
                            BezierPoint bp = (BezierPoint)itr3.next();
                            bp.setBase( new PointD( bp.getBase().getX() + dclock, bp.getBase().getY() ) );
                        }
                        attached_curve.mergeBezierChain( curve, bc_copy );
                    }
                    Vector<BezierChain> arg = new Vector<BezierChain>();
                    for ( Iterator itr2 = attached_curve.get( curve ).iterator(); itr2.hasNext(); ) {
                        BezierChain bc = (BezierChain)itr2.next();
                        arg.add( bc );
                    }
                    command_arg.put( curve, arg );
                }
                edit_bezier = VsqFileEx.generateCommandReplaceAttachedCurveRange( AppManager.getSelected(), command_arg );
            }

            int commands = 0;
            commands += (add_event != null) ? 1 : 0;
            commands += (edit_bpcurve != null) ? 1 : 0;
            commands += (edit_bezier != null) ? 1 : 0;

#if DEBUG
            AppManager.debugWriteLine( "FormMain#pasteEvent; commands=" + commands );
            AppManager.debugWriteLine( "FormMain#pasteEvent; (add_event != null)=" + (add_event != null) );
            AppManager.debugWriteLine( "FormMain#pasteEvent; (edit_bpcurve != null)=" + (edit_bpcurve != null) );
            AppManager.debugWriteLine( "FormMain#pasteEvent; (edit_bezier != null)=" + (edit_bezier != null) );
#endif
            if ( commands == 1 ) {
                if ( add_event != null ) {
                    CadenciiCommand run = new CadenciiCommand( add_event );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                } else if ( edit_bpcurve != null ) {
                    CadenciiCommand run = new CadenciiCommand( edit_bpcurve );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                } else if ( edit_bezier != null ) {
                    AppManager.register( AppManager.getVsqFile().executeCommand( edit_bezier ) );
                }
                setEdited( true );
                refreshScreen();
            } else if ( commands > 1 ) {
                VsqFileEx work = (VsqFileEx)AppManager.getVsqFile().Clone();
                if ( add_event != null ) {
                    work.executeCommand( add_event );
                }
                if ( edit_bezier != null ) {
                    work.executeCommand( edit_bezier );
                }
                if ( edit_bpcurve != null ) {
                    // edit_bpcurveのVsqCommandTypeはTrackEditCurveRangeしかありえない
                    work.executeCommand( edit_bpcurve );
                }
                CadenciiCommand run = VsqFileEx.generateCommandReplace( work );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                setEdited( true );
                refreshScreen();
            }
        }

        /// <summary>
        /// アイテムのコピーを行います
        /// </summary>
        private void copyEvent() {
#if DEBUG
            AppManager.debugWriteLine( "FormMain#copyEvent" );
#endif
            AppManager.clearClipBoard();
            int min = int.MaxValue; // コピーされたアイテムの中で、最小の開始クロック

            if ( AppManager.isWholeSelectedIntervalEnabled() ) {
#if DEBUG
                PortUtil.println( "FormMain#copyEvent; selected with CTRL key" );
#endif
                int stdx = AppManager.startToDrawX;
                int start_clock = AppManager.wholeSelectedInterval.getStart();
                int end_clock = AppManager.wholeSelectedInterval.getEnd();
                ClipboardEntry ce = new ClipboardEntry();
                ce.copyStartedClock = start_clock;
                ce.points = new TreeMap<CurveType, VsqBPList>();
                ce.beziers = new TreeMap<CurveType, Vector<BezierChain>>();
                for ( int i = 0; i < AppManager.CURVE_USAGE.Length; i++ ) {
                    CurveType vct = AppManager.CURVE_USAGE[i];
                    VsqBPList list = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( vct.getName() );
                    if ( list == null ) {
                        continue;
                    }
                    Vector<BezierChain> tmp_bezier = new Vector<BezierChain>();
                    copyCurveCor( AppManager.getSelected(),
                                  vct,
                                  start_clock,
                                  end_clock,
                                  tmp_bezier );
                    VsqBPList tmp_bplist = new VsqBPList( list.getDefault(), list.getMinimum(), list.getMaximum() );
                    int c = list.size();
                    for ( int j = 0; j < c; j++ ) {
                        int clock = list.getKeyClock( j );
                        if ( start_clock <= clock && clock <= end_clock ) {
                            tmp_bplist.add( clock, list.getElement( j ) );
                        } else if ( end_clock < clock ) {
                            break;
                        }
                    }
                    ce.beziers.put( vct, tmp_bezier );
                    ce.points.put( vct, tmp_bplist );
                }

                if ( AppManager.getSelectedEventCount() > 0 ) {
                    Vector<VsqEvent> list = new Vector<VsqEvent>();
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                        SelectedEventEntry item = (SelectedEventEntry)itr.next();
                        if ( item.original.ID.type == VsqIDType.Anote ) {
                            min = Math.Min( item.original.Clock, min );
                            list.add( (VsqEvent)item.original.clone() );
                        }
                    }
                    ce.events = list;
                }
                AppManager.setClipboard( ce );
            } else if ( AppManager.getSelectedEventCount() > 0 ) {
                Vector<VsqEvent> list = new Vector<VsqEvent>();
                for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                    SelectedEventEntry item = (SelectedEventEntry)itr.next();
                    min = Math.Min( item.original.Clock, min );
                    list.add( (VsqEvent)item.original.clone() );
                }
                AppManager.setCopiedEvent( list, min );
            } else if ( AppManager.getSelectedTempoCount() > 0 ) {
                Vector<TempoTableEntry> list = new Vector<TempoTableEntry>();
                for ( Iterator itr = AppManager.getSelectedTempoIterator(); itr.hasNext(); ) {
                    ValuePair<Integer, SelectedTempoEntry> item = (ValuePair<Integer, SelectedTempoEntry>)itr.next();
                    int key = item.getKey();
                    SelectedTempoEntry value = item.getValue();
                    min = Math.Min( value.original.Clock, min );
                    list.add( (TempoTableEntry)value.original.clone() );
                }
                AppManager.setCopiedTempo( list, min );
            } else if ( AppManager.getSelectedTimesigCount() > 0 ) {
                Vector<TimeSigTableEntry> list = new Vector<TimeSigTableEntry>();
                for ( Iterator itr = AppManager.getSelectedTimesigIterator(); itr.hasNext(); ) {
                    ValuePair<Integer, SelectedTimesigEntry> item = (ValuePair<Integer, SelectedTimesigEntry>)itr.next();
                    int key = item.getKey();
                    SelectedTimesigEntry value = item.getValue();
                    min = Math.Min( value.original.Clock, min );
                    list.add( (TimeSigTableEntry)value.original.clone() );
                }
                AppManager.setCopiedTimesig( list, min );
            } else if ( AppManager.getSelectedPointIDCount() > 0 ) {
                ClipboardEntry ce = new ClipboardEntry();
                ce.points = new TreeMap<CurveType, VsqBPList>();
                ce.beziers = new TreeMap<CurveType, Vector<BezierChain>>();

                ValuePair<Integer, Integer> t = trackSelector.getSelectedRegion();
                int start = t.getKey();
                int end = t.getValue();
                ce.copyStartedClock = start;
                Vector<BezierChain> tmp_bezier = new Vector<BezierChain>();
                copyCurveCor( AppManager.getSelected(),
                              trackSelector.getSelectedCurve(),
                              start,
                              end,
                              tmp_bezier );
                if ( tmp_bezier.size() > 0 ) {
                    // ベジエ曲線が1個以上コピーされた場合
                    // 範囲内のデータ点を追加する
                    ce.beziers.put( trackSelector.getSelectedCurve(), tmp_bezier );
                    CurveType curve = trackSelector.getSelectedCurve();
                    VsqBPList list = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( curve.getName() );
                    if ( list != null ) {
                        VsqBPList tmp_bplist = new VsqBPList( list.getDefault(), list.getMinimum(), list.getMaximum() );
                        int c = list.size();
                        for ( int i = 0; i < c; i++ ) {
                            int clock = list.getKeyClock( i );
                            if ( start <= clock && clock <= end ) {
                                tmp_bplist.add( clock, list.getElement( i ) );
                            } else if ( end < clock ) {
                                break;
                            }
                        }
                        ce.points.put( curve, tmp_bplist );
                    }
                } else {
                    // ベジエ曲線がコピーされなかった場合
                    // AppManager.selectedPointIDIteratorの中身のみを選択
                    CurveType curve = trackSelector.getSelectedCurve();
                    VsqBPList list = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( curve.getName() );
                    if ( list != null ) {
                        VsqBPList tmp_bplist = new VsqBPList( curve.getDefault(), curve.getMinimum(), curve.getMaximum() );
                        for ( Iterator itr = AppManager.getSelectedPointIDIterator(); itr.hasNext(); ) {
                            long id = (Long)itr.next();
                            VsqBPPairSearchContext cxt = list.findElement( id );
                            if ( cxt.index >= 0 ) {
                                tmp_bplist.add( cxt.clock, cxt.point.value );
                            }
                        }
                        if ( tmp_bplist.size() > 0 ) {
                            ce.copyStartedClock = tmp_bplist.getKeyClock( 0 );
                            ce.points.put( curve, tmp_bplist );
                        }
                    }
                }
                AppManager.setClipboard( ce );
            }
        }

        private void cutEvent() {
            // まずコピー
            copyEvent();

            int track = AppManager.getSelected();

            // 選択されたノートイベントがあれば、まず、削除を行うコマンドを発行
            VsqCommand delete_event = null;
            boolean other_command_executed = false;
            if ( AppManager.getSelectedEventCount() > 0 ) {
                Vector<Integer> ids = new Vector<Integer>();
                for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                    SelectedEventEntry item = (SelectedEventEntry)itr.next();
                    ids.add( item.original.InternalID );
                }
                delete_event = VsqCommand.generateCommandEventDeleteRange( AppManager.getSelected(), ids );
            }

            // Ctrlキーを押しながらドラッグしたか、そうでないかで分岐
            if ( AppManager.isWholeSelectedIntervalEnabled() || AppManager.getSelectedPointIDCount() > 0 ) {
                int stdx = AppManager.startToDrawX;
                int start_clock, end_clock;
                if ( AppManager.isWholeSelectedIntervalEnabled() ) {
                    start_clock = AppManager.wholeSelectedInterval.getStart();
                    end_clock = AppManager.wholeSelectedInterval.getEnd();
                } else {
                    start_clock = trackSelector.getSelectedRegion().getKey();
                    end_clock = trackSelector.getSelectedRegion().getValue();
                }

                // クローンを作成
                VsqFileEx work = (VsqFileEx)AppManager.getVsqFile().Clone();
                if ( delete_event != null ) {
                    // 選択されたノートイベントがあれば、クローンに対して削除を実行
                    work.executeCommand( delete_event );
                }

                // BPListに削除処理を施す
                for ( int i = 0; i < AppManager.CURVE_USAGE.Length; i++ ) {
                    CurveType curve = AppManager.CURVE_USAGE[i];
                    VsqBPList list = work.Track.get( track ).getCurve( curve.getName() );
                    if ( list == null ) {
                        continue;
                    }
                    int c = list.size();
                    Vector<Long> delete = new Vector<Long>();
                    if ( AppManager.isWholeSelectedIntervalEnabled() ) {
                        // 一括選択モード
                        for ( int j = 0; j < c; j++ ) {
                            int clock = list.getKeyClock( j );
                            if ( start_clock <= clock && clock <= end_clock ) {
                                delete.add( list.getElementB( j ).id );
                            } else if ( end_clock < clock ) {
                                break;
                            }
                        }
                    } else {
                        // 普通の範囲選択
                        for ( Iterator itr = AppManager.getSelectedPointIDIterator(); itr.hasNext(); ) {
                            long id = (Long)itr.next();
                            delete.add( id );
                        }
                    }
                    VsqCommand tmp = VsqCommand.generateCommandTrackCurveEdit2( track, curve.getName(), delete, new TreeMap<Integer, VsqBPPair>() );
                    work.executeCommand( tmp );
                }

                // ベジエ曲線に削除処理を施す
                Vector<CurveType> target_curve = new Vector<CurveType>();
                if ( AppManager.isWholeSelectedIntervalEnabled() ) {
                    // ctrlによる全選択モード
                    for ( int i = 0; i < AppManager.CURVE_USAGE.Length; i++ ) {
                        CurveType ct = AppManager.CURVE_USAGE[i];
                        if ( ct.isScalar() || ct.isAttachNote() ) {
                            continue;
                        }
                        target_curve.add( ct );
                    }
                } else {
                    // 普通の選択モード
                    target_curve.add( trackSelector.getSelectedCurve() );
                }
                work.AttachedCurves.get( AppManager.getSelected() - 1 ).deleteBeziers( target_curve, start_clock, end_clock );

                // コマンドを発行し、実行
                CadenciiCommand run = VsqFileEx.generateCommandReplace( work );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                this.setEdited( true );

                other_command_executed = true;
            } else if ( AppManager.getSelectedTempoCount() > 0 ) {
                // テンポ変更のカット
                int count = -1;
                int[] dum = new int[AppManager.getSelectedTempoCount()];
                int[] clocks = new int[AppManager.getSelectedTempoCount()];
                for ( Iterator itr = AppManager.getSelectedTempoIterator(); itr.hasNext(); ) {
                    ValuePair<Integer, SelectedTempoEntry> item = (ValuePair<Integer, SelectedTempoEntry>)itr.next();
                    int key = item.getKey();
                    SelectedTempoEntry value = item.getValue();
                    count++;
                    dum[count] = -1;
                    clocks[count] = value.original.Clock;
                }
                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandUpdateTempoRange( clocks, clocks, dum ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                setEdited( true );
                other_command_executed = true;
            } else if ( AppManager.getSelectedTimesigCount() > 0 ) {
                // 拍子変更のカット
                int[] barcounts = new int[AppManager.getSelectedTimesigCount()];
                int[] numerators = new int[AppManager.getSelectedTimesigCount()];
                int[] denominators = new int[AppManager.getSelectedTimesigCount()];
                int count = -1;
                for ( Iterator itr = AppManager.getSelectedTimesigIterator(); itr.hasNext(); ) {
                    ValuePair<Integer, SelectedTimesigEntry> item = (ValuePair<Integer, SelectedTimesigEntry>)itr.next();
                    int key = item.getKey();
                    SelectedTimesigEntry value = item.getValue();
                    count++;
                    barcounts[count] = value.original.BarCount;
                    numerators[count] = -1;
                    denominators[count] = -1;
                }
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandUpdateTimesigRange( barcounts, barcounts, numerators, denominators ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                setEdited( true );
                other_command_executed = true;
            }

            // 冒頭で作成した音符イベント削除以外に、コマンドが実行されなかった場合
            if ( delete_event != null && !other_command_executed ) {
                CadenciiCommand run = new CadenciiCommand( delete_event );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                setEdited( true );
            }

            refreshScreen();
        }

        private void copyCurveCor(
            int track,
            CurveType curve_type,
            int start,
            int end,
            Vector<BezierChain> copied_chain
        ) {
            for ( Iterator itr = AppManager.getVsqFile().AttachedCurves.get( track - 1 ).get( curve_type ).iterator(); itr.hasNext(); ) {
                BezierChain bc = (BezierChain)itr.next();
                int len = bc.points.size();
                if ( len < 2 ) {
                    continue;
                }
                int chain_start = (int)bc.points.get( 0 ).getBase().getX();
                int chain_end = (int)bc.points.get( len - 1 ).getBase().getX();
                if ( start < chain_start && chain_start < end && end < chain_end ) {
                    // (1) chain_start ~ end をコピー
                    copied_chain.add( bc.extractPartialBezier( chain_start, end ) );
                } else if ( chain_start <= start && end <= chain_end ) {
                    // (2) start ~ endをコピー
                    copied_chain.add( bc.extractPartialBezier( start, end ) );
                } else if ( chain_start < start && start < chain_end && chain_end <= end ) {
                    // (3) start ~ chain_endをコピー
                    copied_chain.add( bc.extractPartialBezier( start, chain_end ) );
                } else if ( start <= chain_start && chain_end <= end ) {
                    // (4) 全部コピーでOK
                    copied_chain.add( (BezierChain)bc.Clone() );
                }
            }
        }
        #endregion

        #region トラックの編集関連
        private void copyTrackCore() {
            VsqTrack track = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).clone();
            track.setName( track.getName() + " (1)" );
            CadenciiCommand run = VsqFileEx.generateCommandAddTrack( track,
                                                                     AppManager.getVsqFile().Mixer.Slave.get( AppManager.getSelected() - 1 ),
                                                                     AppManager.getVsqFile().Track.size(),
                                                                     AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) ); ;
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
            setEdited( true );
            AppManager.mixerWindow.updateStatus();
            refreshScreen();
        }

        private void changeTrackNameCore() {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
            m_txtbox_track_name = new TextBoxEx();
            m_txtbox_track_name.setVisible( false );
            int selector_width = trackSelector.getSelectorWidth();
            int x = AppManager.keyWidth + (AppManager.getSelected() - 1) * selector_width;
            m_txtbox_track_name.setLocation( x, trackSelector.getHeight() - TrackSelector.OFFSET_TRACK_TAB + 1 );
            m_txtbox_track_name.BorderStyle = System.Windows.Forms.BorderStyle.None;
            m_txtbox_track_name.setText( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getName() );
#if JAVA
            m_txtbox_track_name.keyUpEvent.add( new BEventHandler( this, "m_txtbox_track_name_KeyUp" ) );
#else
            m_txtbox_track_name.KeyUp += new System.Windows.Forms.KeyEventHandler( m_txtbox_track_name_KeyUp );
#endif
            m_txtbox_track_name.setSize( selector_width, TrackSelector.OFFSET_TRACK_TAB );
            m_txtbox_track_name.Parent = trackSelector;
            m_txtbox_track_name.setVisible( true );
            m_txtbox_track_name.requestFocus();
            m_txtbox_track_name.selectAll();
        }

        private void deleteTrackCore() {
            int selected = AppManager.getSelected();
            if ( AppManager.showMessageBox(
                    String.Format( _( "Do you wish to remove track? {0} : '{1}'" ), selected, AppManager.getVsqFile().Track.get( selected ).getName() ),
                    _APP_NAME,
                    AppManager.MSGBOX_YES_NO_OPTION,
                    AppManager.MSGBOX_QUESTION_MESSAGE ) == BDialogResult.YES ) {
                //VsqFileEx temp = (VsqFileEx)AppManager.getVsqFile().Clone();
                CadenciiCommand run = VsqFileEx.generateCommandDeleteTrack( AppManager.getSelected() );
                //temp.executeCommand( run );
                //CadenciiCommand run2 = VsqFileEx.generateCommandReplace( temp );
                if ( AppManager.getSelected() >= 2 ) {
                    AppManager.setSelected( AppManager.getSelected() - 1 );
                }
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
#if USE_DOBJ
                updateDrawObjectList();
#endif
                setEdited( true );
                AppManager.mixerWindow.updateStatus();
                refreshScreen();
            }
        }

        private void addTrackCore() {
            int i = AppManager.getVsqFile().Track.size();
            String name = "Voice" + i;
            String singer = "Miku";
            //VsqFileEx temp = (VsqFileEx)AppManager.getVsqFile().Clone();
            CadenciiCommand run = VsqFileEx.generateCommandAddTrack( new VsqTrack( name, singer ),
                                                                     new VsqMixerEntry( 0, 0, 0, 0 ),
                                                                     i,
                                                                     new BezierCurves() );
            //temp.executeCommand( run );
            //CadenciiCommand run2 = VsqFileEx.generateCommandReplace( temp );
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
#if USE_DOBJ
            updateDrawObjectList();
#endif
            setEdited( true );
            AppManager.setSelected( i );
            AppManager.mixerWindow.updateStatus();
            refreshScreen();
        }
        #endregion

        /// <summary>
        /// length, positionの各Quantizeモードに応じて、表示状態を更新します
        /// </summary>
        private void applyQuantizeMode() {
            cMenuPianoQuantize04.setSelected( false );
            cMenuPianoQuantize08.setSelected( false );
            cMenuPianoQuantize16.setSelected( false );
            cMenuPianoQuantize32.setSelected( false );
            cMenuPianoQuantize64.setSelected( false );
            cMenuPianoQuantize128.setSelected( false );
            cMenuPianoQuantizeOff.setSelected( false );

            stripDDBtnQuantize04.setSelected( false );
            stripDDBtnQuantize08.setSelected( false );
            stripDDBtnQuantize16.setSelected( false );
            stripDDBtnQuantize32.setSelected( false );
            stripDDBtnQuantize64.setSelected( false );
            stripDDBtnQuantize128.setSelected( false );
            stripDDBtnQuantizeOff.setSelected( false );

            menuSettingPositionQuantize04.setSelected( false );
            menuSettingPositionQuantize08.setSelected( false );
            menuSettingPositionQuantize16.setSelected( false );
            menuSettingPositionQuantize32.setSelected( false );
            menuSettingPositionQuantize64.setSelected( false );
            menuSettingPositionQuantize128.setSelected( false );
            menuSettingPositionQuantizeOff.setSelected( false );

#if !JAVA
            stripDDBtnQuantize.setText( "QUANTIZE " + QuantizeModeUtil.getString( AppManager.editorConfig.getPositionQuantize() ) );
#endif
            if ( AppManager.editorConfig.getPositionQuantize() == QuantizeMode.p4 ) {
                cMenuPianoQuantize04.setSelected( true );
                stripDDBtnQuantize04.setSelected( true );
                menuSettingPositionQuantize04.setSelected( true );
            } else if ( AppManager.editorConfig.getPositionQuantize() == QuantizeMode.p8 ) {
                cMenuPianoQuantize08.setSelected( true );
                stripDDBtnQuantize08.setSelected( true );
                menuSettingPositionQuantize08.setSelected( true );
            } else if ( AppManager.editorConfig.getPositionQuantize() == QuantizeMode.p16 ) {
                cMenuPianoQuantize16.setSelected( true );
                stripDDBtnQuantize16.setSelected( true );
                menuSettingPositionQuantize16.setSelected( true );
            } else if ( AppManager.editorConfig.getPositionQuantize() == QuantizeMode.p32 ) {
                cMenuPianoQuantize32.setSelected( true );
                stripDDBtnQuantize32.setSelected( true );
                menuSettingPositionQuantize32.setSelected( true );
            } else if ( AppManager.editorConfig.getPositionQuantize() == QuantizeMode.p64 ) {
                cMenuPianoQuantize64.setSelected( true );
                stripDDBtnQuantize64.setSelected( true );
                menuSettingPositionQuantize64.setSelected( true );
            } else if ( AppManager.editorConfig.getPositionQuantize() == QuantizeMode.p128 ) {
                cMenuPianoQuantize128.setSelected( true );
                stripDDBtnQuantize128.setSelected( true );
                menuSettingPositionQuantize128.setSelected( true );
            } else if ( AppManager.editorConfig.getPositionQuantize() == QuantizeMode.off ) {
                cMenuPianoQuantizeOff.setSelected( true );
                stripDDBtnQuantizeOff.setSelected( true );
                menuSettingPositionQuantizeOff.setSelected( true );
            }
            cMenuPianoQuantizeTriplet.setSelected( AppManager.editorConfig.isPositionQuantizeTriplet() );
            stripDDBtnQuantizeTriplet.setSelected( AppManager.editorConfig.isPositionQuantizeTriplet() );
            menuSettingPositionQuantizeTriplet.setSelected( AppManager.editorConfig.isPositionQuantizeTriplet() );

            cMenuPianoLength04.setSelected( false );
            cMenuPianoLength08.setSelected( false );
            cMenuPianoLength16.setSelected( false );
            cMenuPianoLength32.setSelected( false );
            cMenuPianoLength64.setSelected( false );
            cMenuPianoLength128.setSelected( false );
            cMenuPianoLengthOff.setSelected( false );

            stripDDBtnLength04.setSelected( false );
            stripDDBtnLength08.setSelected( false );
            stripDDBtnLength16.setSelected( false );
            stripDDBtnLength32.setSelected( false );
            stripDDBtnLength64.setSelected( false );
            stripDDBtnLength128.setSelected( false );
            stripDDBtnLengthOff.setSelected( false );

            menuSettingLengthQuantize04.setSelected( false );
            menuSettingLengthQuantize08.setSelected( false );
            menuSettingLengthQuantize16.setSelected( false );
            menuSettingLengthQuantize32.setSelected( false );
            menuSettingLengthQuantize64.setSelected( false );
            menuSettingLengthQuantize128.setSelected( false );
            menuSettingLengthQuantizeOff.setSelected( false );

#if !JAVA
            stripDDBtnLength.setText( "LENGTH " + QuantizeModeUtil.getString( AppManager.editorConfig.getLengthQuantize() ) );
#endif
            if ( AppManager.editorConfig.getLengthQuantize() == QuantizeMode.p4 ) {
                cMenuPianoLength04.setSelected( true );
                stripDDBtnLength04.setSelected( true );
                menuSettingLengthQuantize04.setSelected( true );
            } else if ( AppManager.editorConfig.getLengthQuantize() == QuantizeMode.p8 ) {
                cMenuPianoLength08.setSelected( true );
                stripDDBtnLength08.setSelected( true );
                menuSettingLengthQuantize08.setSelected( true );
            } else if ( AppManager.editorConfig.getLengthQuantize() == QuantizeMode.p16 ) {
                cMenuPianoLength16.setSelected( true );
                stripDDBtnLength16.setSelected( true );
                menuSettingLengthQuantize16.setSelected( true );
            } else if ( AppManager.editorConfig.getLengthQuantize() == QuantizeMode.p32 ) {
                cMenuPianoLength32.setSelected( true );
                stripDDBtnLength32.setSelected( true );
                menuSettingLengthQuantize32.setSelected( true );
            } else if ( AppManager.editorConfig.getLengthQuantize() == QuantizeMode.p64 ) {
                cMenuPianoLength64.setSelected( true );
                stripDDBtnLength64.setSelected( true );
                menuSettingLengthQuantize64.setSelected( true );
            } else if ( AppManager.editorConfig.getLengthQuantize() == QuantizeMode.p128 ) {
                cMenuPianoLength128.setSelected( true );
                stripDDBtnLength128.setSelected( true );
                menuSettingLengthQuantize128.setSelected( true );
            } else if ( AppManager.editorConfig.getLengthQuantize() == QuantizeMode.off ) {
                cMenuPianoLengthOff.setSelected( true );
                stripDDBtnLengthOff.setSelected( true );
                menuSettingLengthQuantizeOff.setSelected( true );
            }
            cMenuPianoLengthTriplet.setSelected( AppManager.editorConfig.isLengthQuantizeTriplet() );
            stripDDBtnLengthTriplet.setSelected( AppManager.editorConfig.isLengthQuantizeTriplet() );
            menuSettingLengthQuantizeTriplet.setSelected( AppManager.editorConfig.isLengthQuantizeTriplet() );
        }

        /// <summary>
        /// 現在選択されている編集ツールに応じて、メニューのチェック状態を更新します
        /// </summary>
        private void applySelectedTool() {
            EditTool tool = AppManager.getSelectedTool();

            int count = toolStripTool.getComponentCount();
            for ( int i = 0; i < count; i++ ) {
                Object tsi = toolStripTool.getComponentAtIndex( i );
                if ( tsi is BToolStripButton ) {
                    BToolStripButton tsb = (BToolStripButton)tsi;
                    Object tag = tsb.getTag();
                    if ( tag != null && tag is String ) {
#if ENABLE_SCRIPT
                        if ( tool == EditTool.PALETTE_TOOL ) {
                            String id = (String)tag;
                            tsb.setSelected( (AppManager.selectedPaletteTool.Equals( id )) );
                        } else
#endif
 {
                            tsb.setSelected( false );
                        }
                    }
                }
            }
            MenuElement[] items = cMenuTrackSelectorPaletteTool.getSubElements();
            foreach ( MenuElement tsi in items ) {
                if ( tsi is BMenuItem ) {
                    BMenuItem tsmi = (BMenuItem)tsi;
                    Object tag = tsmi.getTag();
                    if ( tag != null && tag is String ) {
#if ENABLE_SCRIPT
                        if ( tool == EditTool.PALETTE_TOOL ) {
                            String id = (String)tsmi.getTag();
                            tsmi.setSelected( (AppManager.selectedPaletteTool.Equals( id )) );
                        } else
#endif
 {
                            tsmi.setSelected( false );
                        }
                    }
                }
            }

            items = cMenuPianoPaletteTool.getSubElements();
            foreach ( MenuElement tsi in items ) {
                if ( tsi is BMenuItem ) {
                    BMenuItem tsmi = (BMenuItem)tsi;
                    Object tag = tsmi.getTag();
                    if ( tag != null && tag is String ) {
#if ENABLE_SCRIPT
                        if ( tool == EditTool.PALETTE_TOOL ) {
                            String id = (String)tsmi.getTag();
                            tsmi.setSelected( (AppManager.selectedPaletteTool.Equals( id )) );
                        } else
#endif
 {
                            tsmi.setSelected( false );
                        }
                    }
                }
            }

            EditTool selected_tool = AppManager.getSelectedTool();
            if ( selected_tool == EditTool.ARROW ) {
                cMenuPianoPointer.setSelected( true );
                cMenuPianoPencil.setSelected( false );
                cMenuPianoEraser.setSelected( false );

                cMenuTrackSelectorPointer.setSelected( true );
                cMenuTrackSelectorPencil.setSelected( false );
                cMenuTrackSelectorLine.setSelected( false );
                cMenuTrackSelectorEraser.setSelected( false );

                stripBtnPointer.setSelected( true );
                stripBtnPencil.setSelected( false );
                stripBtnLine.setSelected( false );
                stripBtnEraser.setSelected( false );
            } else if ( selected_tool == EditTool.PENCIL ) {
                cMenuPianoPointer.setSelected( false );
                cMenuPianoPencil.setSelected( true );
                cMenuPianoEraser.setSelected( false );

                cMenuTrackSelectorPointer.setSelected( false );
                cMenuTrackSelectorPencil.setSelected( true );
                cMenuTrackSelectorLine.setSelected( false );
                cMenuTrackSelectorEraser.setSelected( false );

                stripBtnPointer.setSelected( false );
                stripBtnPencil.setSelected( true );
                stripBtnLine.setSelected( false );
                stripBtnEraser.setSelected( false );
            } else if ( selected_tool == EditTool.ERASER ) {
                cMenuPianoPointer.setSelected( false );
                cMenuPianoPencil.setSelected( false );
                cMenuPianoEraser.setSelected( true );

                cMenuTrackSelectorPointer.setSelected( false );
                cMenuTrackSelectorPencil.setSelected( false );
                cMenuTrackSelectorLine.setSelected( false );
                cMenuTrackSelectorEraser.setSelected( true );

                stripBtnPointer.setSelected( false );
                stripBtnPencil.setSelected( false );
                stripBtnLine.setSelected( false );
                stripBtnEraser.setSelected( true );
            } else if ( selected_tool == EditTool.LINE ) {
                cMenuPianoPointer.setSelected( false );
                cMenuPianoPencil.setSelected( false );
                cMenuPianoEraser.setSelected( false );

                cMenuTrackSelectorPointer.setSelected( false );
                cMenuTrackSelectorPencil.setSelected( false );
                cMenuTrackSelectorLine.setSelected( true );
                cMenuTrackSelectorEraser.setSelected( false );

                stripBtnPointer.setSelected( false );
                stripBtnPencil.setSelected( false );
                stripBtnLine.setSelected( true );
                stripBtnEraser.setSelected( false );
#if ENABLE_SCRIPT
            } else if ( selected_tool == EditTool.PALETTE_TOOL ) {
                cMenuPianoPointer.setSelected( false );
                cMenuPianoPencil.setSelected( false );
                cMenuPianoEraser.setSelected( false );

                cMenuTrackSelectorPointer.setSelected( false );
                cMenuTrackSelectorPencil.setSelected( false );
                cMenuTrackSelectorLine.setSelected( false );
                cMenuTrackSelectorEraser.setSelected( false );

                stripBtnPointer.setSelected( false );
                stripBtnPencil.setSelected( false );
                stripBtnLine.setSelected( false );
                stripBtnEraser.setSelected( false );
#endif
            }
            cMenuPianoCurve.setSelected( AppManager.isCurveMode() );
            cMenuTrackSelectorCurve.setSelected( AppManager.isCurveMode() );
            stripBtnCurve.setSelected( AppManager.isCurveMode() );
        }

        /// <summary>
        /// 画面上のマウス位置におけるクロック値を元に，_toolbar_measureの場所表示文字列を更新します．
        /// </summary>
        /// <param name="mouse_pos_x"></param>
        private void updatePositionViewFromMousePosition( int clock ) {
            int barcount = AppManager.getVsqFile().getBarCountFromClock( clock );
            //int numerator, denominator;
            Timesig timesig = AppManager.getVsqFile().getTimesigAt( clock );
            int clock_per_beat = 480 / 4 * timesig.denominator;
            int barcount_clock = AppManager.getVsqFile().getClockFromBarCount( barcount );
            int beat = (clock - barcount_clock) / clock_per_beat;
            int odd = clock - barcount_clock - beat * clock_per_beat;
#if OBSOLUTE
            m_toolbar_measure.Measure = (barcount - AppManager.VsqFile.PreMeasure + 1) + " : " + (beat + 1) + " : " + odd.ToString( "000" );
#else
            stripLblMeasure.setText( (barcount - AppManager.getVsqFile().getPreMeasure() + 1) + " : " + (beat + 1) + " : " + odd.ToString( "000" ) );
#endif
        }

        /// <summary>
        /// 描画すべきオブジェクトのリスト，AppManager.drawObjectsを更新します
        /// </summary>
        public void updateDrawObjectList() {
            // AppManager.m_draw_objects
            if ( AppManager.drawObjects == null ) {
                AppManager.drawObjects = new Vector<Vector<DrawObject>>();
            }
            lock ( AppManager.drawObjects ) {
                if ( AppManager.getVsqFile() == null ) {
                    return;
                }
                for ( int i = 0; i < AppManager.drawStartIndex.Length; i++ ) {
                    AppManager.drawStartIndex[i] = 0;
                }
                if ( AppManager.drawObjects != null ) {
                    for ( Iterator itr = AppManager.drawObjects.iterator(); itr.hasNext(); ) {
                        Vector<DrawObject> list = (Vector<DrawObject>)itr.next();
                        list.clear();
                    }
                    AppManager.drawObjects.clear();
                }

                int xoffset = 6;// 6 + AppManager.keyWidth;
                int yoffset = 127 * AppManager.editorConfig.PxTrackHeight;
                float scalex = AppManager.scaleX;
                Font SMALL_FONT = null;
                try {
                    SMALL_FONT = new Font( AppManager.editorConfig.ScreenFontName, java.awt.Font.PLAIN, 8 );
                    for ( int track = 1; track < AppManager.getVsqFile().Track.size(); track++ ) {
                        Vector<DrawObject> tmp = new Vector<DrawObject>();
                        for ( Iterator itr = AppManager.getVsqFile().Track.get( track ).getEventIterator(); itr.hasNext(); ) {
                            VsqEvent ev = (VsqEvent)itr.next();
                            int timesig = ev.Clock;
                            if ( ev.ID.LyricHandle != null ) {
                                int length = ev.ID.getLength();
                                int note = ev.ID.Note;
                                int x = (int)(timesig * scalex + xoffset);
                                int y = -note * AppManager.editorConfig.PxTrackHeight + yoffset;
                                int lyric_width = (int)(length * scalex);
                                String lyric_jp = ev.ID.LyricHandle.L0.Phrase;
                                String lyric_en = ev.ID.LyricHandle.L0.getPhoneticSymbol();
                                String title = AppManager.trimString( lyric_jp + " [" + lyric_en + "]", SMALL_FONT, lyric_width );
                                int accent = ev.ID.DEMaccent;
                                int vibrato_start = x + lyric_width;
                                int vibrato_end = x;
                                int vibrato_delay = lyric_width * 2;
                                if ( ev.ID.VibratoHandle != null ) {
                                    double rate = (double)ev.ID.VibratoDelay / (double)length;
                                    vibrato_delay = _PX_ACCENT_HEADER + (int)((lyric_width - _PX_ACCENT_HEADER) * rate);
                                }
                                VibratoBPList rate_bp = null;
                                VibratoBPList depth_bp = null;
                                int rate_start = 0;
                                int depth_start = 0;
                                if ( ev.ID.VibratoHandle != null ) {
                                    rate_bp = ev.ID.VibratoHandle.RateBP;
                                    depth_bp = ev.ID.VibratoHandle.DepthBP;
                                    rate_start = ev.ID.VibratoHandle.StartRate;
                                    depth_start = ev.ID.VibratoHandle.StartDepth;
                                }
                                tmp.add( new DrawObject( new Rectangle( x, y, lyric_width, AppManager.editorConfig.PxTrackHeight ),
                                                         title,
                                                         accent,
                                                         ev.InternalID,
                                                         vibrato_delay,
                                                         false,
                                                         ev.ID.LyricHandle.L0.PhoneticSymbolProtected,
                                                         rate_bp,
                                                         depth_bp,
                                                         rate_start,
                                                         depth_start,
                                                         ev.ID.Note,
                                                         ev.UstEvent.Envelope,
                                                         ev.ID.getLength() ) );
                            }
                        }

                        // 重複部分があるかどうかを判定
                        for ( int i = 0; i < tmp.size() - 1; i++ ) {
                            boolean overwrapped = false;
                            for ( int j = i + 1; j < tmp.size(); j++ ) {
                                int startx = tmp.get( j ).pxRectangle.x;
                                int endx = tmp.get( j ).pxRectangle.x + tmp.get( j ).pxRectangle.width;
                                if ( startx < tmp.get( i ).pxRectangle.x ) {
                                    if ( tmp.get( i ).pxRectangle.x < endx ) {
                                        overwrapped = true;
                                        tmp.get( j ).overlappe = true;
                                        // breakできない．2個以上の重複を検出する必要があるので．
                                    }
                                } else if ( tmp.get( i ).pxRectangle.x <= startx && startx < tmp.get( i ).pxRectangle.x + tmp.get( i ).pxRectangle.width ) {
                                    overwrapped = true;
                                    tmp.get( j ).overlappe = true;
                                }
                            }
                            if ( overwrapped ) {
                                tmp.get( i ).overlappe = true;
                            }
                        }
                        AppManager.drawObjects.add( tmp );
                    }
                } catch ( Exception ex ) {
                } finally {
#if !JAVA
                    if ( SMALL_FONT != null ) {
                        SMALL_FONT.font.Dispose();
                    }
#endif
                }
            }
        }

        /// <summary>
        /// _editor_configのRecentFilesを元に，menuFileRecentのドロップダウンアイテムを更新します
        /// </summary>
        private void updateRecentFileMenu() {
            int added = 0;
            menuFileRecent.removeAll();
            if ( AppManager.editorConfig.RecentFiles != null ) {
                for ( int i = 0; i < AppManager.editorConfig.RecentFiles.size(); i++ ) {
                    String item = AppManager.editorConfig.RecentFiles.get( i );
                    if ( item == null ) {
                        continue;
                    }
                    if ( item != "" ) {
                        String short_name = PortUtil.getFileName( item );
                        boolean available = PortUtil.isFileExists( item );
                        BMenuItem itm = new BMenuItem();
                        itm.setText( short_name );
                        if ( !available ) {
                            itm.setToolTipText( _( "[file not found]" ) + " " );
                        }
                        itm.setToolTipText( itm.getToolTipText() + item );
                        itm.setTag( item );
                        itm.setEnabled( available );
#if JAVA
                        itm.clickEvent.add( new BEventHandler( this, "handleRecentFileMenuItem_Click" ) );
                        itm.mouseEnterEvent.add( new BEventHandler( this, "handleRecentFileMenuItem_MouseEnter" ) );
#else
                        itm.Click += new EventHandler( handleRecentFileMenuItem_Click );
                        itm.MouseEnter += new EventHandler( handleRecentFileMenuItem_MouseEnter );
#endif
                        menuFileRecent.add( itm );
                        added++;
                    }
                }
            } else {
                AppManager.editorConfig.pushRecentFiles( "" );
            }
            if ( added == 0 ) {
                menuFileRecent.setEnabled( false );
            } else {
                menuFileRecent.setEnabled( true );
            }
        }

        /// <summary>
        /// 最後に保存したときから変更されているかどうかを取得または設定します
        /// </summary>
        public boolean isEdited() {
            return m_edited;
        }

        public void setEdited( boolean value ) {
            m_edited = value;
            String file = AppManager.getFileName();
            if ( file.Equals( "" ) ) {
                file = "Untitled";
            } else {
                file = PortUtil.getFileNameWithoutExtension( file );
            }
            if ( m_edited ) {
                file += " *";
            }
            String title = file + " - " + _APP_NAME;
            if ( !getTitle().Equals( title ) ) {
                setTitle( title );
            }
            boolean redo = AppManager.isRedoAvailable();
            boolean undo = AppManager.isUndoAvailable();
            menuEditRedo.setEnabled( redo );
            menuEditUndo.setEnabled( undo );
            cMenuPianoRedo.setEnabled( redo );
            cMenuPianoUndo.setEnabled( undo );
            cMenuTrackSelectorRedo.setEnabled( redo );
            cMenuTrackSelectorUndo.setEnabled( undo );
            stripBtnUndo.setEnabled( undo );
            stripBtnRedo.setEnabled( redo );
            //AppManager.setRenderRequired( AppManager.getSelected(), true );
            if ( AppManager.getVsqFile() != null ) {
                int draft = AppManager.getVsqFile().TotalClocks;
                if ( draft > hScroll.getMaximum() ) {
                    setHScrollRange( draft );
                }
            }
#if USE_DOBJ
            updateDrawObjectList();
#endif

#if ENABLE_PROPERTY
            AppManager.propertyPanel.UpdateValue( AppManager.getSelected() );
#endif
        }

        /// <summary>
        /// 入力用のテキストボックスを初期化します
        /// </summary>
        private void showInputTextBox( String phrase, String phonetic_symbol, Point position, boolean phonetic_symbol_edit_mode ) {
#if DEBUG
            AppManager.debugWriteLine( "InitializeInputTextBox" );
#endif
            hideInputTextBox();
#if JAVA
            // TODO: FormMain#showInputTextBox
#else
            AppManager.inputTextBox.KeyUp += m_input_textbox_KeyUp;
            AppManager.inputTextBox.KeyDown += m_input_textbox_KeyDown;
            AppManager.inputTextBox.ImeModeChanged += m_input_textbox_ImeModeChanged;
#endif
            AppManager.inputTextBox.setImeModeOn( m_last_is_imemode_on );
            if ( phonetic_symbol_edit_mode ) {
                AppManager.inputTextBox.setTag( new TagLyricTextBox( phrase, true ) );
                AppManager.inputTextBox.setText( phonetic_symbol );
                AppManager.inputTextBox.setBackground( s_txtbox_backcolor );
            } else {
                AppManager.inputTextBox.setTag( new TagLyricTextBox( phonetic_symbol, false ) );
                AppManager.inputTextBox.setText( phrase );
                AppManager.inputTextBox.setBackground( Color.white );
            }
            AppManager.inputTextBox.setFont( new Font( AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, 9 ) );
            AppManager.inputTextBox.setLocation( position.x + 4, position.y + 2 );
#if !JAVA
            AppManager.inputTextBox.Parent = pictPianoRoll;
#endif
            AppManager.inputTextBox.setEnabled( true );
            AppManager.inputTextBox.setVisible( true );
            AppManager.inputTextBox.requestFocusInWindow();
            AppManager.inputTextBox.selectAll();

        }

        private void hideInputTextBox() {
#if JAVA
            // TODO: FormMain#hideInputTextBox
            /*AppManager.inputTextBox.KeyUp -= m_input_textbox_KeyUp;
            AppManager.inputTextBox.KeyDown -= m_input_textbox_KeyDown;
            AppManager.inputTextBox.ImeModeChanged -= m_input_textbox_ImeModeChanged;*/
#else
            AppManager.inputTextBox.KeyUp -= m_input_textbox_KeyUp;
            AppManager.inputTextBox.KeyDown -= m_input_textbox_KeyDown;
            AppManager.inputTextBox.ImeModeChanged -= m_input_textbox_ImeModeChanged;
#endif
            if ( AppManager.inputTextBox.getTag() != null && AppManager.inputTextBox.getTag() is TagLyricTextBox ) {
                TagLyricTextBox tltb = (TagLyricTextBox)AppManager.inputTextBox.getTag();
                m_last_symbol_edit_mode = tltb.isPhoneticSymbolEditMode();
            }
            AppManager.inputTextBox.setVisible( false );
#if !JAVA
            AppManager.inputTextBox.Parent = null;
#endif
            AppManager.inputTextBox.setEnabled( false );
            pictPianoRoll.requestFocus();
        }

        /// <summary>
        /// 歌詞入力用テキストボックスのモード（歌詞/発音記号）を切り替えます
        /// </summary>
        private void flipInputTextBoxMode() {
            TagLyricTextBox kvp = (TagLyricTextBox)AppManager.inputTextBox.getTag();
            String new_value = AppManager.inputTextBox.getText();
            if ( !kvp.isPhoneticSymbolEditMode() ) {
                AppManager.inputTextBox.setBackground( s_txtbox_backcolor );
            } else {
                AppManager.inputTextBox.setBackground( Color.white );
            }
            AppManager.inputTextBox.setText( kvp.getBufferText() );
            AppManager.inputTextBox.setTag( new TagLyricTextBox( new_value, !kvp.isPhoneticSymbolEditMode() ) );
        }

        /// <summary>
        /// 音の高さを表すnoteから、画面に描くべきy座標を計算します
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public int yCoordFromNote( float note ) {
            return yCoordFromNote( note, getStartToDrawY() );
        }

        public int yCoordFromNote( float note, int start_to_draw_y ) {
            return (int)(-1 * (note - 127.0f) * AppManager.editorConfig.PxTrackHeight) - start_to_draw_y;
        }

        /// <summary>
        /// ピアノロール画面のy座標から、その位置における音の高さを取得します
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int noteFromYCoord( int y ) {
            return 127 - (int)((double)(getStartToDrawY() + y) / (double)AppManager.editorConfig.PxTrackHeight);
        }

        /// <summary>
        /// 「選択されている」と登録されているオブジェクトのうち、Undo, Redoなどによって存在しなくなったものを登録解除する
        /// </summary>
        public void cleanupDeadSelection() {
            Vector<ValuePair<int, int>> list = new Vector<ValuePair<int, int>>();
            for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                SelectedEventEntry item = (SelectedEventEntry)itr.next();
                list.add( new ValuePair<int, int>( item.track, item.original.InternalID ) );
            }

            for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                ValuePair<Integer, Integer> specif = (ValuePair<Integer, Integer>)itr.next();
                boolean found = false;
                for ( Iterator itr2 = AppManager.getVsqFile().Track.get( specif.getKey() ).getNoteEventIterator(); itr2.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr2.next();
                    if ( item.InternalID == specif.getValue() ) {
                        found = true;
                        break;
                    }
                }
                if ( !found ) {
                    AppManager.removeSelectedEvent( specif.getKey() );
                }
            }
        }

        /// <summary>
        /// アンドゥ処理を行います
        /// </summary>
        public void undo() {
            if ( AppManager.isUndoAvailable() ) {
                AppManager.undo();
                menuEditRedo.setEnabled( AppManager.isRedoAvailable() );
                menuEditUndo.setEnabled( AppManager.isUndoAvailable() );
                cMenuPianoRedo.setEnabled( AppManager.isRedoAvailable() );
                cMenuPianoUndo.setEnabled( AppManager.isUndoAvailable() );
                cMenuTrackSelectorRedo.setEnabled( AppManager.isRedoAvailable() );
                cMenuTrackSelectorUndo.setEnabled( AppManager.isUndoAvailable() );
                AppManager.mixerWindow.updateStatus();
                setEdited( true );
                cleanupDeadSelection();
#if USE_DOBJ
                updateDrawObjectList();
#endif

#if ENABLE_PROPERTY
                if ( AppManager.propertyPanel != null ) {
                    AppManager.propertyPanel.UpdateValue( AppManager.getSelected() );
                }
#endif
            }
        }

        /// <summary>
        /// リドゥ処理を行います
        /// </summary>
        public void redo() {
            if ( AppManager.isRedoAvailable() ) {
                AppManager.redo();
                menuEditRedo.setEnabled( AppManager.isRedoAvailable() );
                menuEditUndo.setEnabled( AppManager.isUndoAvailable() );
                cMenuPianoRedo.setEnabled( AppManager.isRedoAvailable() );
                cMenuPianoUndo.setEnabled( AppManager.isUndoAvailable() );
                cMenuTrackSelectorRedo.setEnabled( AppManager.isRedoAvailable() );
                cMenuTrackSelectorUndo.setEnabled( AppManager.isUndoAvailable() );
                AppManager.mixerWindow.updateStatus();
                setEdited( true );
                cleanupDeadSelection();
#if USE_DOBJ
                updateDrawObjectList();
#endif

#if ENABLE_PROPERTY
                if ( AppManager.propertyPanel != null ) {
                    AppManager.propertyPanel.UpdateValue( AppManager.getSelected() );
                }
#endif
            }
        }

        public int getStartToDrawY() {
            return (int)((128 * AppManager.editorConfig.PxTrackHeight - vScroll.getHeight()) * (float)vScroll.getValue() / ((float)vScroll.getMaximum()));
        }

        /// <summary>
        /// pがrcの中にあるかどうかを判定します
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rc"></param>
        /// <returns></returns>
        static boolean isInRect( Point p, Rectangle rc ) {
            if ( rc.x <= p.x ) {
                if ( p.x <= rc.x + rc.width ) {
                    if ( rc.y <= p.y ) {
                        if ( p.y <= rc.y + rc.height ) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// マウス位置におけるIDを返します。該当するIDが無ければnullを返します
        /// rectには、該当するIDがあればその画面上での形状を、該当するIDがなければ、
        /// 画面上で最も近かったIDの画面上での形状を返します
        /// </summary>
        /// <param name="mouse_position"></param>
        /// <returns></returns>
        VsqEvent getItemAtClickedPosition( Point mouse_position, ByRef<Rectangle> rect ) {
            rect.value = new Rectangle();
            if ( AppManager.keyWidth <= mouse_position.x && mouse_position.x <= pictPianoRoll.getWidth() ) {
                if ( 0 <= mouse_position.y && mouse_position.y <= pictPianoRoll.getHeight() ) {
                    int selected = AppManager.getSelected();
                    if ( selected >= 1 ) {
                        for ( int j = 0; j < AppManager.getVsqFile().Track.get( selected ).getEventCount(); j++ ) {
                            int timesig = AppManager.getVsqFile().Track.get( selected ).getEvent( j ).Clock;
                            int internal_id = AppManager.getVsqFile().Track.get( selected ).getEvent( j ).InternalID;
                            // イベントで指定されたIDがLyricであった場合
                            if ( AppManager.getVsqFile().Track.get( selected ).getEvent( j ).ID.type == VsqIDType.Anote &&
                                AppManager.getVsqFile().Track.get( selected ).getEvent( j ).ID.LyricHandle != null ) {
                                // 発音長を取得
                                int length = AppManager.getVsqFile().Track.get( selected ).getEvent( j ).ID.getLength();
                                int note = AppManager.getVsqFile().Track.get( selected ).getEvent( j ).ID.Note;
                                int x = AppManager.xCoordFromClocks( timesig );
                                int y = yCoordFromNote( note );
                                int lyric_width = (int)(length * AppManager.scaleX);
                                if ( x + lyric_width < 0 ) {
                                    continue;
                                } else if ( pictPianoRoll.getWidth() < x ) {
                                    break;
                                }
                                if ( x <= mouse_position.x && mouse_position.x <= x + lyric_width ) {
                                    if ( y + 1 <= mouse_position.y && mouse_position.y <= y + AppManager.editorConfig.PxTrackHeight ) {
                                        rect.value = new Rectangle( x, y + 1, lyric_width, AppManager.editorConfig.PxTrackHeight );
                                        return AppManager.getVsqFile().Track.get( selected ).getEvent( j );
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void openVsqCor( String file ) {
            AppManager.readVsq( file );
            if ( AppManager.getVsqFile().Track.size() >= 2 ) {
                AppManager.setBaseTempo( AppManager.getVsqFile().getBaseTempo() );
                setHScrollRange( AppManager.getVsqFile().TotalClocks );
            }
            AppManager.editorConfig.pushRecentFiles( file );
            updateRecentFileMenu();
            setEdited( false );
            AppManager.clearCommandBuffer();
            AppManager.mixerWindow.updateStatus();
        }

        private void updateMenuFonts() {
            if ( AppManager.editorConfig.BaseFontName.Equals( "" ) ) {
                return;
            }
            Font font = AppManager.editorConfig.getBaseFont();
            Util.applyFontRecurse( this, font );
            Util.applyContextMenuFontRecurse( cMenuPiano, font );
            Util.applyContextMenuFontRecurse( cMenuTrackSelector, font );
            if ( AppManager.mixerWindow != null ) {
                Util.applyFontRecurse( AppManager.mixerWindow, font );
            }
            Util.applyContextMenuFontRecurse( cMenuTrackTab, font );
            trackSelector.applyFont( font );
            Util.applyToolStripFontRecurse( menuFile, font );
            Util.applyToolStripFontRecurse( menuEdit, font );
            Util.applyToolStripFontRecurse( menuVisual, font );
            Util.applyToolStripFontRecurse( menuJob, font );
            Util.applyToolStripFontRecurse( menuTrack, font );
            Util.applyToolStripFontRecurse( menuLyric, font );
            Util.applyToolStripFontRecurse( menuScript, font );
            Util.applyToolStripFontRecurse( menuSetting, font );
            Util.applyToolStripFontRecurse( menuHelp, font );
            Util.applyFontRecurse( toolStripFile, font );
            Util.applyFontRecurse( toolStripMeasure, font );
            Util.applyFontRecurse( toolStripPosition, font );
            Util.applyFontRecurse( toolStripTool, font );
            if ( m_preference_dlg != null ) {
                Util.applyFontRecurse( m_preference_dlg, font );
            }

            AppManager.baseFont10Bold = new Font( AppManager.editorConfig.BaseFontName, java.awt.Font.BOLD, 10 );
            AppManager.baseFont8 = new Font( AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, 8 );
            AppManager.baseFont10 = new Font( AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, 10 );
            AppManager.baseFont9 = new Font( AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, 9 );
            AppManager.baseFont10OffsetHeight = Util.getStringDrawOffset( AppManager.baseFont10 );
            AppManager.baseFont8OffsetHeight = Util.getStringDrawOffset( AppManager.baseFont8 );
            AppManager.baseFont9OffsetHeight = Util.getStringDrawOffset( AppManager.baseFont9 );
        }

        private void picturePositionIndicatorDrawTo( Graphics2D g ) {
            Font SMALL_FONT = null;
            try {
                SMALL_FONT = new Font( AppManager.editorConfig.ScreenFontName, java.awt.Font.PLAIN, 8 );
                int width = picturePositionIndicator.getWidth();
                int height = picturePositionIndicator.getHeight();

                #region 小節ごとの線
                int dashed_line_step = AppManager.getPositionQuantizeClock();
                for ( Iterator itr = AppManager.getVsqFile().getBarLineIterator( AppManager.clockFromXCoord( width ) ); itr.hasNext(); ) {
                    VsqBarLineType blt = (VsqBarLineType)itr.next();
                    int local_clock_step = 480 * 4 / blt.getLocalDenominator();
                    int x = AppManager.xCoordFromClocks( blt.clock() );
                    if ( blt.isSeparator() ) {
                        int current = blt.getBarCount() - AppManager.getVsqFile().getPreMeasure() + 1;
                        g.setColor( s_pen_105_105_105 );
                        g.drawLine( x, 3, x, 46 );
                        // 小節の数字
                        //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.setColor( Color.black );
                        g.setFont( SMALL_FONT );
                        g.drawString( current.ToString(), x + 4, 6 );
                        //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    } else {
                        g.setColor( s_pen_105_105_105 );
                        g.drawLine( x, 11, x, 16 );
                        g.drawLine( x, 26, x, 31 );
                        g.drawLine( x, 41, x, 46 );
                    }
                    if ( dashed_line_step > 1 && AppManager.isGridVisible() ) {
                        int numDashedLine = local_clock_step / dashed_line_step;
                        for ( int i = 1; i < numDashedLine; i++ ) {
                            int x2 = AppManager.xCoordFromClocks( blt.clock() + i * dashed_line_step );
                            g.setColor( s_pen_065_065_065 );
                            g.drawLine( x2, 9 + 5, x2, 14 + 3 );
                            g.drawLine( x2, 24 + 5, x2, 29 + 3 );
                            g.drawLine( x2, 39 + 5, x2, 44 + 3 );
                        }
                    }
                }
                #endregion

                if ( AppManager.getVsqFile() != null ) {
                    #region 拍子の変更
                    for ( int i = 0; i < AppManager.getVsqFile().TimesigTable.size(); i++ ) {
                        int clock = AppManager.getVsqFile().TimesigTable.get( i ).Clock;
                        int barcount = AppManager.getVsqFile().TimesigTable.get( i ).BarCount;
                        int x = AppManager.xCoordFromClocks( clock );
                        if ( width < x ) {
                            break;
                        }
                        String s = AppManager.getVsqFile().TimesigTable.get( i ).Numerator + "/" + AppManager.getVsqFile().TimesigTable.get( i ).Denominator;
                        g.setFont( SMALL_FONT );
                        if ( AppManager.isSelectedTimesigContains( barcount ) ) {
                            g.setColor( AppManager.getHilightColor() );
                            g.drawString( s, x + 4, 36 );
                        } else {
                            g.setColor( Color.black );
                            g.drawString( s, x + 4, 36 );
                        }

                        if ( m_timesig_dragging ) {
                            if ( AppManager.isSelectedTimesigContains( barcount ) ) {
                                int edit_clock_x = AppManager.xCoordFromClocks( AppManager.getVsqFile().getClockFromBarCount( AppManager.getSelectedTimesig( barcount ).editing.BarCount ) );
                                g.setColor( s_pen_187_187_255 );
                                g.drawLine( edit_clock_x - 1, 32,
                                            edit_clock_x - 1, picturePositionIndicator.getHeight() - 1 );
                                g.setColor( s_pen_007_007_151 );
                                g.drawLine( edit_clock_x, 32,
                                            edit_clock_x, picturePositionIndicator.getHeight() - 1 );
                            }
                        }
                    }
                    #endregion

                    #region テンポの変更
                    g.setFont( SMALL_FONT );
                    for ( int i = 0; i < AppManager.getVsqFile().TempoTable.size(); i++ ) {
                        int clock = AppManager.getVsqFile().TempoTable.get( i ).Clock;
                        int x = AppManager.xCoordFromClocks( clock );
                        if ( width < x ) {
                            break;
                        }
                        String s = (60e6 / (float)AppManager.getVsqFile().TempoTable.get( i ).Tempo).ToString( "#.00" );
                        if ( AppManager.isSelectedTempoContains( clock ) ) {
                            g.setColor( AppManager.getHilightColor() );
                            g.drawString( s, x + 4, 21 );
                        } else {
                            g.setColor( Color.black );
                            g.drawString( s, x + 4, 21 );
                        }

                        if ( m_tempo_dragging ) {
                            if ( AppManager.isSelectedTempoContains( clock ) ) {
                                int edit_clock_x = AppManager.xCoordFromClocks( AppManager.getSelectedTempo( clock ).editing.Clock );
                                g.setColor( s_pen_187_187_255 );
                                g.drawLine( edit_clock_x - 1, 18,
                                            edit_clock_x - 1, 32 );
                                g.setColor( s_pen_007_007_151 );
                                g.drawLine( edit_clock_x, 18,
                                            edit_clock_x, 32 );
                            }
                        }
                    }
                    #endregion
                }

                #region 外枠
                /* 左(外側) */
                g.setColor( new Color( 160, 160, 160 ) );
                g.drawLine( 0, 0, 0, height - 1 );
                /* 左(内側) */
                g.setColor( new Color( 105, 105, 105 ) );
                g.drawLine( 1, 1, 1, height - 2 );
                /* 中(上側) */
                g.setColor( new Color( 160, 160, 160 ) );
                g.drawLine( 1, 47, width - 2, 47 );
                /* 中(下側) */
                g.setColor( new Color( 105, 105, 105 ) );
                g.drawLine( 2, 48, width - 3, 48 );
                // 右(外側)
                g.setColor( Color.white );
                g.drawLine( width - 1, 0, width - 1, height - 1 );
                // 右(内側)
                g.setColor( new Color( 241, 239, 226 ) );
                g.drawLine( width - 2, 1, width - 2, height - 1 );
                #endregion

                #region 現在のマーカー
                float xoffset = AppManager.keyWidth + 6 - AppManager.startToDrawX;
                int marker_x = (int)(AppManager.getCurrentClock() * AppManager.scaleX + xoffset);
                if ( AppManager.keyWidth <= marker_x && marker_x <= width ) {
                    g.setStroke( new BasicStroke( 2.0f ) );
                    g.setColor( Color.white );
                    g.drawLine( marker_x, 2, marker_x, height );
                    g.setStroke( new BasicStroke() );
                }
                if ( AppManager.startMarkerEnabled ) {
                    int x = AppManager.xCoordFromClocks( AppManager.startMarker );
                    g.drawImage(
                        Resources.get_start_marker(), x, 3, this );
                }
                if ( AppManager.endMarkerEnabled ) {
                    int x = AppManager.xCoordFromClocks( AppManager.endMarker ) - 6;
                    g.drawImage(
                        Resources.get_end_marker(), x, 3, this );
                }
                #endregion

                #region TEMPO & BEAT
                // TEMPO BEATの文字の部分。小節数が被っている可能性があるので、塗り潰す
                g.setColor( picturePositionIndicator.getBackground() );
                g.fillRect( 2, 3, AppManager.keyWidth - 2, 45 );
                // 横ライン上
                g.setColor( new Color( 104, 104, 104 ) );
                g.drawLine( 2, 17, width - 3, 17 );
                // 横ライン中央
                g.drawLine( 2, 32, width - 3, 32 );
                // 横ライン下
                g.drawLine( 2, 47, width - 3, 47 );
                // 縦ライン
                g.drawLine( AppManager.keyWidth, 2, AppManager.keyWidth, 46 );
                /* TEMPO&BEATとピアノロールの境界 */
                g.drawLine( AppManager.keyWidth, 48, width - 18, 48 );
                g.setFont( SMALL_FONT );
                g.setColor( Color.black );
                g.drawString( "TEMPO", 11, 20 );
                g.drawString( "BEAT", 11, 35 );
                g.setColor( new Color( 172, 168, 153 ) );
                g.drawLine( 0, 0, width, 0 );
                g.setColor( new Color( 113, 111, 100 ) );
                g.drawLine( 1, 1, width - 1, 1 );

                #endregion
            } catch ( Exception ex ) {
            } finally {
#if !JAVA
                if ( SMALL_FONT != null && SMALL_FONT.font != null ) {
                    SMALL_FONT.font.Dispose();
                }
#endif
            }
        }

        private void menuTrackManager_Click( Object sender, BEventArgs e ) {

        }

        private void pictKeyLengthSplitter_MouseDown( Object sender, BMouseEventArgs e ) {
            m_key_length_splitter_mouse_downed = true;
            m_key_length_splitter_initial_mouse = PortUtil.getMousePosition();
            m_key_length_init_value = AppManager.keyWidth;
        }

        private void pictKeyLengthSplitter_MouseMove( Object sender, BMouseEventArgs e ) {
            if ( !m_key_length_splitter_mouse_downed ) {
                return;
            }
            int dx = PortUtil.getMousePosition().x - m_key_length_splitter_initial_mouse.x;
            int draft = m_key_length_init_value + dx;
            if ( draft < AppManager.MIN_KEY_WIDTH ) {
                draft = AppManager.MIN_KEY_WIDTH;
            } else if ( AppManager.MAX_KEY_WIDTH < draft ) {
                draft = AppManager.MAX_KEY_WIDTH;
            }
            AppManager.keyWidth = draft;
            updateLayout();
            refreshScreen();
        }

        private void pictKeyLengthSplitter_MouseUp( Object sender, BMouseEventArgs e ) {
            m_key_length_splitter_mouse_downed = false;
        }

        private void overviewCommon_MouseLeave( Object sender, BEventArgs e ) {
#if DEBUG
            PortUtil.println( "FormMain#overviewCommon_MouseLeave" );
#endif
            overviewStopThread();
        }

        private void overviewStopThread() {
            if ( m_overview_update_thread != null ) {
                try {
                    m_overview_update_thread.Abort();
                    while ( m_overview_update_thread != null && m_overview_update_thread.IsAlive ) {
#if JAVA
                        Thread.sleep( 0 );
#else
                        System.Windows.Forms.Application.DoEvents();
#endif
                    }
                } catch ( Exception ex ) {
                }
                m_overview_update_thread = null;
            }
        }

        private void registerEventHandlers() {
            this.menuStripMain.MouseDown += new System.Windows.Forms.MouseEventHandler( this.menuStrip1_MouseDown );
            this.menuFileNew.MouseEnter += new System.EventHandler( this.menuFileNew_MouseEnter );
            this.menuFileNew.Click += new System.EventHandler( this.commonFileNew_Click );
            this.menuFileOpen.MouseEnter += new System.EventHandler( this.menuFileOpen_MouseEnter );
            this.menuFileOpen.Click += new System.EventHandler( this.commonFileOpen_Click );
            this.menuFileSave.MouseEnter += new System.EventHandler( this.menuFileSave_MouseEnter );
            this.menuFileSave.Click += new System.EventHandler( this.commonFileSave_Click );
            this.menuFileSaveNamed.MouseEnter += new System.EventHandler( this.menuFileSaveNamed_MouseEnter );
            this.menuFileSaveNamed.Click += new System.EventHandler( this.menuFileSaveNamed_Click );
            this.menuFileOpenVsq.MouseEnter += new System.EventHandler( this.menuFileOpenVsq_MouseEnter );
            this.menuFileOpenVsq.Click += new System.EventHandler( this.menuFileOpenVsq_Click );
            this.menuFileOpenUst.MouseEnter += new System.EventHandler( this.menuFileOpenUst_MouseEnter );
            this.menuFileOpenUst.Click += new System.EventHandler( this.menuFileOpenUst_Click );
            this.menuFileImport.MouseEnter += new System.EventHandler( this.menuFileImport_MouseEnter );
            this.menuFileImportVsq.MouseEnter += new System.EventHandler( this.menuFileImportVsq_MouseEnter );
            this.menuFileImportVsq.Click += new System.EventHandler( this.menuFileImportVsq_Click );
            this.menuFileImportMidi.MouseEnter += new System.EventHandler( this.menuFileImportMidi_MouseEnter );
            this.menuFileImportMidi.Click += new System.EventHandler( this.menuFileImportMidi_Click );
            this.menuFileExport.DropDownOpening += new System.EventHandler( this.menuFileExport_DropDownOpening );
            this.menuFileExportWave.MouseEnter += new System.EventHandler( this.menuFileExportWave_MouseEnter );
            this.menuFileExportWave.Click += new System.EventHandler( this.menuFileExportWave_Click );
            this.menuFileExportMidi.MouseEnter += new System.EventHandler( this.menuFileExportMidi_MouseEnter );
            this.menuFileExportMidi.Click += new System.EventHandler( this.menuFileExportMidi_Click );
            this.menuFileRecent.MouseEnter += new System.EventHandler( this.menuFileRecent_MouseEnter );
            this.menuFileQuit.MouseEnter += new System.EventHandler( this.menuFileQuit_MouseEnter );
            this.menuFileQuit.Click += new System.EventHandler( this.menuFileQuit_Click );
            this.menuEdit.DropDownOpening += new System.EventHandler( this.menuEdit_DropDownOpening );
            this.menuEditUndo.MouseEnter += new System.EventHandler( this.menuEditUndo_MouseEnter );
            this.menuEditUndo.Click += new System.EventHandler( this.commonEditUndo_Click );
            this.menuEditRedo.MouseEnter += new System.EventHandler( this.menuEditRedo_MouseEnter );
            this.menuEditRedo.Click += new System.EventHandler( this.commonEditRedo_Click );
            this.menuEditCut.MouseEnter += new System.EventHandler( this.menuEditCut_MouseEnter );
            this.menuEditCut.Click += new System.EventHandler( this.commonEditCut_Click );
            this.menuEditCopy.MouseEnter += new System.EventHandler( this.menuEditCopy_MouseEnter );
            this.menuEditCopy.Click += new System.EventHandler( this.commonEditCopy_Click );
            this.menuEditPaste.MouseEnter += new System.EventHandler( this.menuEditPaste_MouseEnter );
            this.menuEditPaste.Click += new System.EventHandler( this.commonEditPaste_Click );
            this.menuEditDelete.MouseEnter += new System.EventHandler( this.menuEditDelete_MouseEnter );
            this.menuEditDelete.Click += new System.EventHandler( this.menuEditDelete_Click );
            this.menuEditAutoNormalizeMode.MouseEnter += new System.EventHandler( this.menuEditAutoNormalizeMode_MouseEnter );
            this.menuEditAutoNormalizeMode.Click += new System.EventHandler( this.menuEditAutoNormalizeMode_Click );
            this.menuEditSelectAll.MouseEnter += new System.EventHandler( this.menuEditSelectAll_MouseEnter );
            this.menuEditSelectAll.Click += new System.EventHandler( this.menuEditSelectAll_Click );
            this.menuEditSelectAllEvents.MouseEnter += new System.EventHandler( this.menuEditSelectAllEvents_MouseEnter );
            this.menuEditSelectAllEvents.Click += new System.EventHandler( this.menuEditSelectAllEvents_Click );
            this.menuVisualControlTrack.CheckedChanged += new System.EventHandler( this.menuVisualControlTrack_CheckedChanged );
            this.menuVisualControlTrack.MouseEnter += new System.EventHandler( this.menuVisualControlTrack_MouseEnter );
            this.menuVisualMixer.MouseEnter += new System.EventHandler( this.menuVisualMixer_MouseEnter );
            this.menuVisualMixer.Click += new System.EventHandler( this.menuVisualMixer_Click );
            this.menuVisualWaveform.CheckedChanged += new System.EventHandler( this.menuVisualWaveform_CheckedChanged );
            this.menuVisualWaveform.MouseEnter += new System.EventHandler( this.menuVisualWaveform_MouseEnter );
            this.menuVisualProperty.MouseEnter += new System.EventHandler( this.menuVisualProperty_MouseEnter );
            this.menuVisualProperty.Click += new System.EventHandler( this.menuVisualProperty_Click );
            this.menuVisualGridline.CheckedChanged += new System.EventHandler( this.menuVisualGridline_CheckedChanged );
            this.menuVisualGridline.MouseEnter += new System.EventHandler( this.menuVisualGridline_MouseEnter );
            this.menuVisualStartMarker.MouseEnter += new System.EventHandler( this.menuVisualStartMarker_MouseEnter );
            this.menuVisualStartMarker.Click += new System.EventHandler( this.handleStartMarker_Click );
            this.menuVisualEndMarker.MouseEnter += new System.EventHandler( this.menuVisualEndMarker_MouseEnter );
            this.menuVisualEndMarker.Click += new System.EventHandler( this.handleEndMarker_Click );
            this.menuVisualLyrics.CheckedChanged += new System.EventHandler( this.menuVisualLyrics_CheckedChanged );
            this.menuVisualLyrics.MouseEnter += new System.EventHandler( this.menuVisualLyrics_MouseEnter );
            this.menuVisualNoteProperty.CheckedChanged += new System.EventHandler( this.menuVisualNoteProperty_CheckedChanged );
            this.menuVisualNoteProperty.MouseEnter += new System.EventHandler( this.menuVisualNoteProperty_MouseEnter );
            this.menuVisualPitchLine.CheckedChanged += new System.EventHandler( this.menuVisualPitchLine_CheckedChanged );
            this.menuVisualPitchLine.MouseEnter += new System.EventHandler( this.menuVisualPitchLine_MouseEnter );
            this.menuJob.DropDownOpening += new System.EventHandler( this.menuJob_DropDownOpening );
            this.menuJobNormalize.MouseEnter += new System.EventHandler( this.menuJobNormalize_MouseEnter );
            this.menuJobNormalize.Click += new System.EventHandler( this.menuJobNormalize_Click );
            this.menuJobInsertBar.MouseEnter += new System.EventHandler( this.menuJobInsertBar_MouseEnter );
            this.menuJobInsertBar.Click += new System.EventHandler( this.menuJobInsertBar_Click );
            this.menuJobDeleteBar.MouseEnter += new System.EventHandler( this.menuJobDeleteBar_MouseEnter );
            this.menuJobDeleteBar.Click += new System.EventHandler( this.menuJobDeleteBar_Click );
            this.menuJobRandomize.MouseEnter += new System.EventHandler( this.menuJobRandomize_MouseEnter );
            this.menuJobConnect.MouseEnter += new System.EventHandler( this.menuJobConnect_MouseEnter );
            this.menuJobConnect.Click += new System.EventHandler( this.menuJobConnect_Click );
            this.menuJobLyric.MouseEnter += new System.EventHandler( this.menuJobLyric_MouseEnter );
            this.menuJobLyric.Click += new System.EventHandler( this.menuJobLyric_Click );
            this.menuJobRewire.MouseEnter += new System.EventHandler( this.menuJobRewire_MouseEnter );
            this.menuJobRealTime.MouseEnter += new System.EventHandler( this.menuJobRealTime_MouseEnter );
            this.menuJobRealTime.Click += new System.EventHandler( this.menuJobRealTime_Click );
            this.menuJobReloadVsti.MouseEnter += new System.EventHandler( this.menuJobReloadVsti_MouseEnter );
            this.menuJobReloadVsti.Click += new System.EventHandler( this.menuJobReloadVsti_Click );
            this.menuTrack.DropDownOpening += new System.EventHandler( this.menuTrack_DropDownOpening );
            this.menuTrackOn.MouseEnter += new System.EventHandler( this.menuTrackOn_MouseEnter );
            this.menuTrackOn.Click += new System.EventHandler( this.menuTrackOn_Click );
            this.menuTrackAdd.MouseEnter += new System.EventHandler( this.menuTrackAdd_MouseEnter );
            this.menuTrackAdd.Click += new System.EventHandler( this.menuTrackAdd_Click );
            this.menuTrackCopy.MouseEnter += new System.EventHandler( this.menuTrackCopy_MouseEnter );
            this.menuTrackCopy.Click += new System.EventHandler( this.menuTrackCopy_Click );
            this.menuTrackChangeName.MouseEnter += new System.EventHandler( this.menuTrackChangeName_MouseEnter );
            this.menuTrackChangeName.Click += new System.EventHandler( this.menuTrackChangeName_Click );
            this.menuTrackDelete.MouseEnter += new System.EventHandler( this.menuTrackDelete_MouseEnter );
            this.menuTrackDelete.Click += new System.EventHandler( this.menuTrackDelete_Click );
            this.menuTrackRenderCurrent.MouseEnter += new System.EventHandler( this.menuTrackRenderCurrent_MouseEnter );
            this.menuTrackRenderCurrent.Click += new System.EventHandler( this.menuTrackRenderCurrent_Click );
            this.menuTrackRenderAll.MouseEnter += new System.EventHandler( this.menuTrackRenderAll_MouseEnter );
            this.menuTrackRenderAll.Click += new System.EventHandler( this.commonTrackRenderAll_Click );
            this.menuTrackOverlay.MouseEnter += new System.EventHandler( this.menuTrackOverlay_MouseEnter );
            this.menuTrackOverlay.Click += new System.EventHandler( this.menuTrackOverlay_Click );
            this.menuTrackRenderer.MouseEnter += new System.EventHandler( this.menuTrackRenderer_MouseEnter );
            this.menuTrackRenderer.DropDownOpening += new System.EventHandler( this.menuTrackRenderer_DropDownOpening );
            this.menuTrackRendererVOCALOID1.MouseEnter += new System.EventHandler( this.menuTrackRendererVOCALOID1_MouseEnter );
            this.menuTrackRendererVOCALOID1.Click += new System.EventHandler( this.commonRendererVOCALOID1_Click );
            this.menuTrackRendererVOCALOID2.MouseEnter += new System.EventHandler( this.menuTrackRendererVOCALOID2_MouseEnter );
            this.menuTrackRendererVOCALOID2.Click += new System.EventHandler( this.commonRendererVOCALOID2_Click );
            this.menuTrackRendererUtau.MouseEnter += new System.EventHandler( this.menuTrackRendererUtau_MouseEnter );
            this.menuTrackRendererUtau.Click += new System.EventHandler( this.commonRendererUtau_Click );
            this.menuTrackRendererStraight.Click += new System.EventHandler( this.commonRendererStraight_Click );
            this.menuTrackManager.Click += new System.EventHandler( this.menuTrackManager_Click );
            this.menuLyricExpressionProperty.Click += new System.EventHandler( this.menuLyricExpressionProperty_Click );
            this.menuLyricVibratoProperty.Click += new System.EventHandler( this.menuLyricVibratoProperty_Click );
            this.menuLyricDictionary.Click += new System.EventHandler( this.menuLyricDictionary_Click );
            this.menuScriptUpdate.Click += new System.EventHandler( this.menuScriptUpdate_Click );
            this.menuSetting.DropDownOpening += new System.EventHandler( this.menuSetting_DropDownOpening );
            this.menuSettingPreference.Click += new System.EventHandler( this.menuSettingPreference_Click );
            this.menuSettingGameControlerSetting.Click += new System.EventHandler( this.menuSettingGameControlerSetting_Click );
            this.menuSettingGameControlerLoad.Click += new System.EventHandler( this.menuSettingGameControlerLoad_Click );
            this.menuSettingGameControlerRemove.Click += new System.EventHandler( this.menuSettingGameControlerRemove_Click );
            this.menuSettingShortcut.Click += new System.EventHandler( this.menuSettingShortcut_Click );
#if ENABLE_MIDI
            this.menuSettingMidi.Click += new System.EventHandler( this.menuSettingMidi_Click );
#endif
            this.menuSettingUtauVoiceDB.Click += new System.EventHandler( this.menuSettingUtauVoiceDB_Click );
            this.menuSettingDefaultSingerStyle.Click += new System.EventHandler( this.menuSettingDefaultSingerStyle_Click );
            this.menuSettingPositionQuantize04.Click += new System.EventHandler( this.handlePositionQuantize );
            this.menuSettingPositionQuantize08.Click += new System.EventHandler( this.handlePositionQuantize );
            this.menuSettingPositionQuantize16.Click += new System.EventHandler( this.handlePositionQuantize );
            this.menuSettingPositionQuantize32.Click += new System.EventHandler( this.handlePositionQuantize );
            this.menuSettingPositionQuantize64.Click += new System.EventHandler( this.handlePositionQuantize );
            this.menuSettingPositionQuantize128.Click += new System.EventHandler( this.handlePositionQuantize );
            this.menuSettingPositionQuantizeOff.Click += new System.EventHandler( this.handlePositionQuantize );
            this.menuSettingPositionQuantizeTriplet.Click += new System.EventHandler( this.h_positionQuantizeTriplet );
            this.menuSettingLengthQuantize04.Click += new System.EventHandler( this.h_lengthQuantize04 );
            this.menuSettingLengthQuantize08.Click += new System.EventHandler( this.h_lengthQuantize08 );
            this.menuSettingLengthQuantize16.Click += new System.EventHandler( this.h_lengthQuantize16 );
            this.menuSettingLengthQuantize32.Click += new System.EventHandler( this.h_lengthQuantize32 );
            this.menuSettingLengthQuantize64.Click += new System.EventHandler( this.h_lengthQuantize64 );
            this.menuSettingLengthQuantize128.Click += new System.EventHandler( this.h_lengthQuantize128 );
            this.menuSettingLengthQuantizeOff.Click += new System.EventHandler( this.h_lengthQuantizeOff );
            this.menuSettingLengthQuantizeTriplet.Click += new System.EventHandler( this.h_lengthQuantizeTriplet );
            this.menuHelpAbout.Click += new System.EventHandler( this.menuHelpAbout_Click );
            this.menuHelpDebug.Click += new System.EventHandler( this.menuHelpDebug_Click );
            this.menuHiddenEditLyric.Click += new System.EventHandler( this.menuHiddenEditLyric_Click );
            this.menuHiddenEditFlipToolPointerPencil.Click += new System.EventHandler( this.menuHiddenEditFlipToolPointerPencil_Click );
            this.menuHiddenEditFlipToolPointerEraser.Click += new System.EventHandler( this.menuHiddenEditFlipToolPointerEraser_Click );
            this.menuHiddenVisualForwardParameter.Click += new System.EventHandler( this.menuHiddenVisualForwardParameter_Click );
            this.menuHiddenVisualBackwardParameter.Click += new System.EventHandler( this.menuHiddenVisualBackwardParameter_Click );
            this.menuHiddenTrackNext.Click += new System.EventHandler( this.menuHiddenTrackNext_Click );
            this.menuHiddenTrackBack.Click += new System.EventHandler( this.menuHiddenTrackBack_Click );
            this.menuHiddenCopy.Click += new System.EventHandler( this.commonEditCopy_Click );
            this.menuHiddenPaste.Click += new System.EventHandler( this.commonEditPaste_Click );
            this.menuHiddenCut.Click += new System.EventHandler( this.commonEditCut_Click );
            this.cMenuPiano.Opening += new System.ComponentModel.CancelEventHandler( this.cMenuPiano_Opening );
            this.cMenuPianoPointer.Click += new System.EventHandler( this.cMenuPianoPointer_Click );
            this.cMenuPianoPencil.Click += new System.EventHandler( this.cMenuPianoPencil_Click );
            this.cMenuPianoEraser.Click += new System.EventHandler( this.cMenuPianoEraser_Click );
            this.cMenuPianoCurve.Click += new System.EventHandler( this.cMenuPianoCurve_Click );
            this.cMenuPianoFixed01.Click += new System.EventHandler( this.cMenuPianoFixed01_Click );
            this.cMenuPianoFixed02.Click += new System.EventHandler( this.cMenuPianoFixed02_Click );
            this.cMenuPianoFixed04.Click += new System.EventHandler( this.cMenuPianoFixed04_Click );
            this.cMenuPianoFixed08.Click += new System.EventHandler( this.cMenuPianoFixed08_Click );
            this.cMenuPianoFixed16.Click += new System.EventHandler( this.cMenuPianoFixed16_Click );
            this.cMenuPianoFixed32.Click += new System.EventHandler( this.cMenuPianoFixed32_Click );
            this.cMenuPianoFixed64.Click += new System.EventHandler( this.cMenuPianoFixed64_Click );
            this.cMenuPianoFixed128.Click += new System.EventHandler( this.cMenuPianoFixed128_Click );
            this.cMenuPianoFixedOff.Click += new System.EventHandler( this.cMenuPianoFixedOff_Click );
            this.cMenuPianoFixedTriplet.Click += new System.EventHandler( this.cMenuPianoFixedTriplet_Click );
            this.cMenuPianoFixedDotted.Click += new System.EventHandler( this.cMenuPianoFixedDotted_Click );
            this.cMenuPianoQuantize04.Click += new System.EventHandler( this.handlePositionQuantize );
            this.cMenuPianoQuantize08.Click += new System.EventHandler( this.handlePositionQuantize );
            this.cMenuPianoQuantize16.Click += new System.EventHandler( this.handlePositionQuantize );
            this.cMenuPianoQuantize32.Click += new System.EventHandler( this.handlePositionQuantize );
            this.cMenuPianoQuantize64.Click += new System.EventHandler( this.handlePositionQuantize );
            this.cMenuPianoQuantize128.Click += new System.EventHandler( this.handlePositionQuantize );
            this.cMenuPianoQuantizeOff.Click += new System.EventHandler( this.handlePositionQuantize );
            this.cMenuPianoQuantizeTriplet.Click += new System.EventHandler( this.h_positionQuantizeTriplet );
            this.cMenuPianoLength04.Click += new System.EventHandler( this.h_lengthQuantize04 );
            this.cMenuPianoLength08.Click += new System.EventHandler( this.h_lengthQuantize08 );
            this.cMenuPianoLength16.Click += new System.EventHandler( this.h_lengthQuantize16 );
            this.cMenuPianoLength32.Click += new System.EventHandler( this.h_lengthQuantize32 );
            this.cMenuPianoLength64.Click += new System.EventHandler( this.h_lengthQuantize64 );
            this.cMenuPianoLength128.Click += new System.EventHandler( this.h_lengthQuantize128 );
            this.cMenuPianoLengthOff.Click += new System.EventHandler( this.h_lengthQuantizeOff );
            this.cMenuPianoLengthTriplet.Click += new System.EventHandler( this.h_lengthQuantizeTriplet );
            this.cMenuPianoGrid.Click += new System.EventHandler( this.cMenuPianoGrid_Click );
            this.cMenuPianoUndo.Click += new System.EventHandler( this.cMenuPianoUndo_Click );
            this.cMenuPianoRedo.Click += new System.EventHandler( this.cMenuPianoRedo_Click );
            this.cMenuPianoCut.Click += new System.EventHandler( this.cMenuPianoCut_Click );
            this.cMenuPianoCopy.Click += new System.EventHandler( this.cMenuPianoCopy_Click );
            this.cMenuPianoPaste.Click += new System.EventHandler( this.cMenuPianoPaste_Click );
            this.cMenuPianoDelete.Click += new System.EventHandler( this.cMenuPianoDelete_Click );
            this.cMenuPianoSelectAll.Click += new System.EventHandler( this.cMenuPianoSelectAll_Click );
            this.cMenuPianoSelectAllEvents.Click += new System.EventHandler( this.cMenuPianoSelectAllEvents_Click );
            this.cMenuPianoImportLyric.Click += new System.EventHandler( this.cMenuPianoImportLyric_Click );
            this.cMenuPianoExpressionProperty.Click += new System.EventHandler( this.cMenuPianoProperty_Click );
            this.cMenuPianoVibratoProperty.Click += new System.EventHandler( this.cMenuPianoVibratoProperty_Click );
            this.cMenuTrackTab.Opening += new System.ComponentModel.CancelEventHandler( this.cMenuTrackTab_Opening );
            this.cMenuTrackTabTrackOn.Click += new System.EventHandler( this.cMenuTrackTabTrackOn_Click );
            this.cMenuTrackTabAdd.Click += new System.EventHandler( this.cMenuTrackTabAdd_Click );
            this.cMenuTrackTabCopy.Click += new System.EventHandler( this.cMenuTrackTabCopy_Click );
            this.cMenuTrackTabChangeName.Click += new System.EventHandler( this.cMenuTrackTabChangeName_Click );
            this.cMenuTrackTabDelete.Click += new System.EventHandler( this.cMenuTrackTabDelete_Click );
            this.cMenuTrackTabRenderCurrent.Click += new System.EventHandler( this.cMenuTrackTabRenderCurrent_Click );
            this.cMenuTrackTabRenderAll.Click += new System.EventHandler( this.commonTrackRenderAll_Click );
            this.cMenuTrackTabOverlay.Click += new System.EventHandler( this.cMenuTrackTabOverlay_Click );
            this.cMenuTrackTabRenderer.DropDownOpening += new System.EventHandler( this.cMenuTrackTabRenderer_DropDownOpening );
            this.cMenuTrackTabRendererVOCALOID1.Click += new System.EventHandler( this.commonRendererVOCALOID1_Click );
            this.cMenuTrackTabRendererVOCALOID2.Click += new System.EventHandler( this.commonRendererVOCALOID2_Click );
            this.cMenuTrackTabRendererUtau.Click += new System.EventHandler( this.commonRendererUtau_Click );
            this.cMenuTrackTabRendererStraight.Click += new System.EventHandler( this.commonRendererStraight_Click );
            this.cMenuTrackSelector.Opening += new System.ComponentModel.CancelEventHandler( this.cMenuTrackSelector_Opening );
            this.cMenuTrackSelectorPointer.Click += new System.EventHandler( this.cMenuTrackSelectorPointer_Click );
            this.cMenuTrackSelectorPencil.Click += new System.EventHandler( this.cMenuTrackSelectorPencil_Click );
            this.cMenuTrackSelectorLine.Click += new System.EventHandler( this.cMenuTrackSelectorLine_Click );
            this.cMenuTrackSelectorEraser.Click += new System.EventHandler( this.cMenuTrackSelectorEraser_Click );
            this.cMenuTrackSelectorCurve.Click += new System.EventHandler( this.cMenuTrackSelectorCurve_Click );
            this.cMenuTrackSelectorUndo.Click += new System.EventHandler( this.cMenuTrackSelectorUndo_Click );
            this.cMenuTrackSelectorRedo.Click += new System.EventHandler( this.cMenuTrackSelectorRedo_Click );
            this.cMenuTrackSelectorCut.Click += new System.EventHandler( this.cMenuTrackSelectorCut_Click );
            this.cMenuTrackSelectorCopy.Click += new System.EventHandler( this.cMenuTrackSelectorCopy_Click );
            this.cMenuTrackSelectorPaste.Click += new System.EventHandler( this.cMenuTrackSelectorPaste_Click );
            this.cMenuTrackSelectorDelete.Click += new System.EventHandler( this.cMenuTrackSelectorDelete_Click );
            this.cMenuTrackSelectorDeleteBezier.Click += new System.EventHandler( this.cMenuTrackSelectorDeleteBezier_Click );
            this.cMenuTrackSelectorSelectAll.Click += new System.EventHandler( this.cMenuTrackSelectorSelectAll_Click );
            this.trackBar.ValueChanged += new System.EventHandler( this.trackBar_ValueChanged );
            this.trackBar.MouseDown += new System.Windows.Forms.MouseEventHandler( this.trackBar_MouseDown );
            this.trackBar.Enter += new System.EventHandler( this.trackBar_Enter );
            this.bgWorkScreen.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWorkScreen_DoWork );
            this.timer.Tick += new System.EventHandler( this.timer_Tick );
            this.pictKeyLengthSplitter.MouseMove += new System.Windows.Forms.MouseEventHandler( this.pictKeyLengthSplitter_MouseMove );
            this.pictKeyLengthSplitter.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictKeyLengthSplitter_MouseDown );
            this.pictKeyLengthSplitter.MouseUp += new System.Windows.Forms.MouseEventHandler( this.pictKeyLengthSplitter_MouseUp );
            this.btnRight1.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnRight_MouseDown );
            this.btnRight1.MouseUp += new System.Windows.Forms.MouseEventHandler( this.btnRight_MouseUp );
            this.btnRight1.MouseLeave += new EventHandler( this.overviewCommon_MouseLeave );
            this.btnLeft2.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnLeft_MouseDown );
            this.btnLeft2.MouseUp += new System.Windows.Forms.MouseEventHandler( this.btnLeft_MouseUp );
            this.btnLeft2.MouseLeave += new System.EventHandler( this.overviewCommon_MouseLeave );
            this.btnZoom.Click += new System.EventHandler( this.btnZoom_Click );
            this.btnMooz.Click += new System.EventHandler( this.btnMooz_Click );
            this.btnLeft1.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnLeft_MouseDown );
            this.btnLeft1.MouseUp += new System.Windows.Forms.MouseEventHandler( this.btnLeft_MouseUp );
            this.btnLeft1.MouseLeave += new System.EventHandler( this.overviewCommon_MouseLeave );
            this.btnRight2.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnRight_MouseDown );
            this.btnRight2.MouseUp += new System.Windows.Forms.MouseEventHandler( this.btnRight_MouseUp );
            this.btnRight2.MouseLeave += new System.EventHandler( this.overviewCommon_MouseLeave );
            this.pictOverview.MouseMove += new System.Windows.Forms.MouseEventHandler( this.pictOverview_MouseMove );
            this.pictOverview.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.pictOverview_MouseDoubleClick );
            this.pictOverview.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictOverview_MouseDown );
            this.pictOverview.Paint += new System.Windows.Forms.PaintEventHandler( this.pictOverview_Paint );
            this.pictOverview.MouseUp += new System.Windows.Forms.MouseEventHandler( this.pictOverview_MouseUp );
            this.pictOverview.BKeyUp += new System.Windows.Forms.KeyEventHandler( this.commonCaptureSpaceKeyUp );
            this.pictOverview.BKeyDown += new System.Windows.Forms.KeyEventHandler( this.commonCaptureSpaceKeyDown );
            this.vScroll.ValueChanged += new System.EventHandler( this.vScroll_ValueChanged );
            this.vScroll.Resize += new System.EventHandler( this.vScroll_Resize );
            this.vScroll.Enter += new System.EventHandler( this.vScroll_Enter );
            this.hScroll.ValueChanged += new System.EventHandler( this.hScroll_ValueChanged );
            this.hScroll.Resize += new System.EventHandler( this.hScroll_Resize );
            this.hScroll.Enter += new System.EventHandler( this.hScroll_Enter );
            this.picturePositionIndicator.MouseLeave += new System.EventHandler( this.picturePositionIndicator_MouseLeave );
            this.picturePositionIndicator.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.picturePositionIndicator_PreviewKeyDown );
            this.picturePositionIndicator.MouseMove += new System.Windows.Forms.MouseEventHandler( this.picturePositionIndicator_MouseMove );
            this.picturePositionIndicator.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.picturePositionIndicator_MouseDoubleClick );
            this.picturePositionIndicator.MouseClick += new System.Windows.Forms.MouseEventHandler( this.picturePositionIndicator_MouseClick );
            this.picturePositionIndicator.MouseDown += new System.Windows.Forms.MouseEventHandler( this.picturePositionIndicator_MouseDown );
            this.picturePositionIndicator.Paint += new System.Windows.Forms.PaintEventHandler( this.picturePositionIndicator_Paint );
            this.pictPianoRoll.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.pictPianoRoll_PreviewKeyDown );
            this.pictPianoRoll.BKeyUp += new System.Windows.Forms.KeyEventHandler( this.commonCaptureSpaceKeyUp );
            this.pictPianoRoll.MouseMove += new System.Windows.Forms.MouseEventHandler( this.pictPianoRoll_MouseMove );
            this.pictPianoRoll.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.pictPianoRoll_MouseDoubleClick );
            this.pictPianoRoll.MouseClick += new System.Windows.Forms.MouseEventHandler( this.pictPianoRoll_MouseClick );
            this.pictPianoRoll.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictPianoRoll_MouseDown );
            this.pictPianoRoll.MouseUp += new System.Windows.Forms.MouseEventHandler( this.pictPianoRoll_MouseUp );
            this.pictPianoRoll.BKeyDown += new System.Windows.Forms.KeyEventHandler( this.commonCaptureSpaceKeyDown );
            this.pictureBox3.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictureBox3_MouseDown );
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictureBox2_MouseDown );
            this.stripBtnPointer.Click += new System.EventHandler( this.stripBtnArrow_Click );
            this.stripBtnPencil.Click += new System.EventHandler( this.stripBtnPencil_Click );
            this.stripBtnLine.Click += new System.EventHandler( this.stripBtnLine_Click );
            this.stripBtnEraser.Click += new System.EventHandler( this.stripBtnEraser_Click );
            this.stripBtnGrid.CheckedChanged += new System.EventHandler( this.stripBtnGrid_CheckedChanged );
            this.stripBtnCurve.Click += new System.EventHandler( this.stripBtnCurve_Click );
            this.toolStripContainer.TopToolStripPanel.SizeChanged += new System.EventHandler( this.toolStripContainer_TopToolStripPanel_SizeChanged );
            this.stripDDBtnSpeed.DropDownOpening += new System.EventHandler( this.stripDDBtnSpeed_DropDownOpening );
            this.stripDDBtnSpeedTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler( this.stripDDBtnSpeedTextbox_KeyDown );
            this.stripDDBtnSpeed033.Click += new System.EventHandler( this.stripDDBtnSpeed033_Click );
            this.stripDDBtnSpeed050.Click += new System.EventHandler( this.stripDDBtnSpeed050_Click );
            this.stripDDBtnSpeed100.Click += new System.EventHandler( this.stripDDBtnSpeed100_Click );
            this.stripBtnFileNew.Click += new System.EventHandler( this.commonFileNew_Click );
            this.stripBtnFileOpen.Click += new System.EventHandler( this.commonFileOpen_Click );
            this.stripBtnFileSave.Click += new System.EventHandler( this.commonFileSave_Click );
            this.stripBtnCut.Click += new System.EventHandler( this.commonEditCut_Click );
            this.stripBtnCopy.Click += new System.EventHandler( this.commonEditCopy_Click );
            this.stripBtnPaste.Click += new System.EventHandler( this.commonEditPaste_Click );
            this.stripBtnUndo.Click += new System.EventHandler( this.commonEditUndo_Click );
            this.stripBtnRedo.Click += new System.EventHandler( this.commonEditRedo_Click );
            this.stripBtnMoveTop.Click += new System.EventHandler( this.stripBtnMoveTop_Click );
            this.stripBtnRewind.Click += new System.EventHandler( this.stripBtnRewind_Click );
            this.stripBtnForward.Click += new System.EventHandler( this.stripBtnForward_Click );
            this.stripBtnMoveEnd.Click += new System.EventHandler( this.stripBtnMoveEnd_Click );
            this.stripBtnPlay.Click += new System.EventHandler( this.stripBtnPlay_Click );
            this.stripBtnStop.Click += new System.EventHandler( this.stripBtnStop_Click );
            this.stripBtnScroll.Click += new System.EventHandler( this.stripBtnScroll_Click );
            this.stripBtnLoop.Click += new System.EventHandler( this.stripBtnLoop_Click );
            this.stripDDBtnLength04.Click += new System.EventHandler( this.h_lengthQuantize04 );
            this.stripDDBtnLength08.Click += new System.EventHandler( this.h_lengthQuantize08 );
            this.stripDDBtnLength16.Click += new System.EventHandler( this.h_lengthQuantize16 );
            this.stripDDBtnLength32.Click += new System.EventHandler( this.h_lengthQuantize32 );
            this.stripDDBtnLength64.Click += new System.EventHandler( this.h_lengthQuantize64 );
            this.stripDDBtnLength128.Click += new System.EventHandler( this.h_lengthQuantize128 );
            this.stripDDBtnLengthOff.Click += new System.EventHandler( this.h_lengthQuantizeOff );
            this.stripDDBtnLengthTriplet.Click += new System.EventHandler( this.h_lengthQuantizeTriplet );
            this.stripDDBtnQuantize04.Click += new System.EventHandler( this.handlePositionQuantize );
            this.stripDDBtnQuantize08.Click += new System.EventHandler( this.handlePositionQuantize );
            this.stripDDBtnQuantize16.Click += new System.EventHandler( this.handlePositionQuantize );
            this.stripDDBtnQuantize32.Click += new System.EventHandler( this.handlePositionQuantize );
            this.stripDDBtnQuantize64.Click += new System.EventHandler( this.handlePositionQuantize );
            this.stripDDBtnQuantize128.Click += new System.EventHandler( this.handlePositionQuantize );
            this.stripDDBtnQuantizeOff.Click += new System.EventHandler( this.handlePositionQuantize );
            this.stripDDBtnQuantizeTriplet.Click += new System.EventHandler( this.h_positionQuantizeTriplet );
            this.stripBtnStartMarker.Click += new System.EventHandler( this.handleStartMarker_Click );
            this.stripBtnEndMarker.Click += new System.EventHandler( this.handleEndMarker_Click );
            this.Deactivate += new System.EventHandler( this.FormMain_Deactivate );
            this.Load += new System.EventHandler( this.FormMain_Load );
            this.Activated += new System.EventHandler( this.FormMain_Activated );
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler( this.FormMain_FormClosed );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormMain_FormClosing );
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.FormMain_PreviewKeyDown );
        }

        private void setResources() {
            this.stripBtnPointer.setIcon( new ImageIcon( Resources.get_arrow_135() ) );
            this.stripBtnPencil.setIcon( new ImageIcon( Resources.get_pencil() ) );
            this.stripBtnLine.setIcon( new ImageIcon( Resources.get_layer_shape_line() ) );
            this.stripBtnEraser.setIcon( new ImageIcon( Resources.get_eraser() ) );
            this.stripBtnGrid.setIcon( new ImageIcon( Resources.get_ruler_crop() ) );
            this.stripBtnCurve.setIcon( new ImageIcon( Resources.get_layer_shape_curve() ) );
            this.stripLblGameCtrlMode.setIcon( new ImageIcon( Resources.get_slash() ) );
            this.stripLblMidiIn.setIcon( new ImageIcon( Resources.get_slash() ) );
            this.stripBtnFileNew.setIcon( new ImageIcon( Resources.get_disk__plus() ) );
            this.stripBtnFileOpen.setIcon( new ImageIcon( Resources.get_folder_horizontal_open() ) );
            this.stripBtnFileSave.setIcon( new ImageIcon( Resources.get_disk() ) );
            this.stripBtnCut.setIcon( new ImageIcon( Resources.get_scissors() ) );
            this.stripBtnCopy.setIcon( new ImageIcon( Resources.get_documents() ) );
            this.stripBtnPaste.setIcon( new ImageIcon( Resources.get_clipboard_paste() ) );
            this.stripBtnUndo.setIcon( new ImageIcon( Resources.get_arrow_skip_180() ) );
            this.stripBtnRedo.setIcon( new ImageIcon( Resources.get_arrow_skip() ) );
            this.stripBtnMoveTop.setIcon( new ImageIcon( Resources.get_control_stop_180() ) );
            this.stripBtnRewind.setIcon( new ImageIcon( Resources.get_control_double_180() ) );
            this.stripBtnForward.setIcon( new ImageIcon( Resources.get_control_double() ) );
            this.stripBtnMoveEnd.setIcon( new ImageIcon( Resources.get_control_stop() ) );
            this.stripBtnPlay.setIcon( new ImageIcon( Resources.get_control() ) );
            this.stripBtnStop.setIcon( new ImageIcon( Resources.get_control_pause() ) );
            this.stripBtnScroll.setIcon( new ImageIcon( Resources.get_arrow_circle_double() ) );
            this.stripBtnLoop.setIcon( new ImageIcon( Resources.get_arrow_return() ) );
            this.stripBtnStartMarker.setIcon( new ImageIcon( Resources.get_pin__arrow() ) );
            this.stripBtnEndMarker.setIcon( new ImageIcon( Resources.get_pin__arrow_inv() ) );
            setIconImage( Resources.get_icon() );
        }

#if JAVA
    #region UI Impl for Java
    //INCLUDE-SECTION FIELD ..\BuildJavaUI\src\FormMain.java
    //INCLUDE-SECTION METHOD ..\BuildJavaUI\src\FormMain.java
    #endregion
#else
        #region UI Impl for C#
        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing ) {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.menuStripMain = new bocoree.windows.forms.BMenuBar();
            this.menuFile = new bocoree.windows.forms.BMenuItem();
            this.menuFileNew = new bocoree.windows.forms.BMenuItem();
            this.menuFileOpen = new bocoree.windows.forms.BMenuItem();
            this.menuFileSave = new bocoree.windows.forms.BMenuItem();
            this.menuFileSaveNamed = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileOpenVsq = new bocoree.windows.forms.BMenuItem();
            this.menuFileOpenUst = new bocoree.windows.forms.BMenuItem();
            this.menuFileImport = new bocoree.windows.forms.BMenuItem();
            this.menuFileImportVsq = new bocoree.windows.forms.BMenuItem();
            this.menuFileImportMidi = new bocoree.windows.forms.BMenuItem();
            this.menuFileExport = new bocoree.windows.forms.BMenuItem();
            this.menuFileExportWave = new bocoree.windows.forms.BMenuItem();
            this.menuFileExportMidi = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileRecent = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileQuit = new bocoree.windows.forms.BMenuItem();
            this.menuEdit = new bocoree.windows.forms.BMenuItem();
            this.menuEditUndo = new bocoree.windows.forms.BMenuItem();
            this.menuEditRedo = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEditCut = new bocoree.windows.forms.BMenuItem();
            this.menuEditCopy = new bocoree.windows.forms.BMenuItem();
            this.menuEditPaste = new bocoree.windows.forms.BMenuItem();
            this.menuEditDelete = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEditAutoNormalizeMode = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEditSelectAll = new bocoree.windows.forms.BMenuItem();
            this.menuEditSelectAllEvents = new bocoree.windows.forms.BMenuItem();
            this.menuVisual = new bocoree.windows.forms.BMenuItem();
            this.menuVisualControlTrack = new bocoree.windows.forms.BMenuItem();
            this.menuVisualMixer = new bocoree.windows.forms.BMenuItem();
            this.menuVisualWaveform = new bocoree.windows.forms.BMenuItem();
            this.menuVisualProperty = new bocoree.windows.forms.BMenuItem();
            this.menuVisualOverview = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuVisualGridline = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuVisualStartMarker = new bocoree.windows.forms.BMenuItem();
            this.menuVisualEndMarker = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuVisualLyrics = new bocoree.windows.forms.BMenuItem();
            this.menuVisualNoteProperty = new bocoree.windows.forms.BMenuItem();
            this.menuVisualPitchLine = new bocoree.windows.forms.BMenuItem();
            this.menuJob = new bocoree.windows.forms.BMenuItem();
            this.menuJobNormalize = new bocoree.windows.forms.BMenuItem();
            this.menuJobInsertBar = new bocoree.windows.forms.BMenuItem();
            this.menuJobDeleteBar = new bocoree.windows.forms.BMenuItem();
            this.menuJobRandomize = new bocoree.windows.forms.BMenuItem();
            this.menuJobConnect = new bocoree.windows.forms.BMenuItem();
            this.menuJobLyric = new bocoree.windows.forms.BMenuItem();
            this.menuJobRewire = new bocoree.windows.forms.BMenuItem();
            this.menuJobRealTime = new bocoree.windows.forms.BMenuItem();
            this.menuJobReloadVsti = new bocoree.windows.forms.BMenuItem();
            this.menuTrack = new bocoree.windows.forms.BMenuItem();
            this.menuTrackOn = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTrackAdd = new bocoree.windows.forms.BMenuItem();
            this.menuTrackCopy = new bocoree.windows.forms.BMenuItem();
            this.menuTrackChangeName = new bocoree.windows.forms.BMenuItem();
            this.menuTrackDelete = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTrackRenderCurrent = new bocoree.windows.forms.BMenuItem();
            this.menuTrackRenderAll = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTrackOverlay = new bocoree.windows.forms.BMenuItem();
            this.menuTrackRenderer = new bocoree.windows.forms.BMenuItem();
            this.menuTrackRendererVOCALOID1 = new bocoree.windows.forms.BMenuItem();
            this.menuTrackRendererVOCALOID2 = new bocoree.windows.forms.BMenuItem();
            this.menuTrackRendererUtau = new bocoree.windows.forms.BMenuItem();
            this.menuTrackRendererStraight = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTrackBgm = new bocoree.windows.forms.BMenuItem();
            this.menuTrackManager = new bocoree.windows.forms.BMenuItem();
            this.menuLyric = new bocoree.windows.forms.BMenuItem();
            this.menuLyricExpressionProperty = new bocoree.windows.forms.BMenuItem();
            this.menuLyricVibratoProperty = new bocoree.windows.forms.BMenuItem();
            this.menuLyricSymbol = new bocoree.windows.forms.BMenuItem();
            this.menuLyricDictionary = new bocoree.windows.forms.BMenuItem();
            this.menuScript = new bocoree.windows.forms.BMenuItem();
            this.menuScriptUpdate = new bocoree.windows.forms.BMenuItem();
            this.menuSetting = new bocoree.windows.forms.BMenuItem();
            this.menuSettingPreference = new bocoree.windows.forms.BMenuItem();
            this.menuSettingGameControler = new bocoree.windows.forms.BMenuItem();
            this.menuSettingGameControlerSetting = new bocoree.windows.forms.BMenuItem();
            this.menuSettingGameControlerLoad = new bocoree.windows.forms.BMenuItem();
            this.menuSettingGameControlerRemove = new bocoree.windows.forms.BMenuItem();
            this.menuSettingPaletteTool = new bocoree.windows.forms.BMenuItem();
            this.menuSettingShortcut = new bocoree.windows.forms.BMenuItem();
            this.menuSettingMidi = new bocoree.windows.forms.BMenuItem();
            this.menuSettingUtauVoiceDB = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSettingDefaultSingerStyle = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSettingPositionQuantize = new bocoree.windows.forms.BMenuItem();
            this.menuSettingPositionQuantize04 = new bocoree.windows.forms.BMenuItem();
            this.menuSettingPositionQuantize08 = new bocoree.windows.forms.BMenuItem();
            this.menuSettingPositionQuantize16 = new bocoree.windows.forms.BMenuItem();
            this.menuSettingPositionQuantize32 = new bocoree.windows.forms.BMenuItem();
            this.menuSettingPositionQuantize64 = new bocoree.windows.forms.BMenuItem();
            this.menuSettingPositionQuantize128 = new bocoree.windows.forms.BMenuItem();
            this.menuSettingPositionQuantizeOff = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSettingPositionQuantizeTriplet = new bocoree.windows.forms.BMenuItem();
            this.menuSettingLengthQuantize = new bocoree.windows.forms.BMenuItem();
            this.menuSettingLengthQuantize04 = new bocoree.windows.forms.BMenuItem();
            this.menuSettingLengthQuantize08 = new bocoree.windows.forms.BMenuItem();
            this.menuSettingLengthQuantize16 = new bocoree.windows.forms.BMenuItem();
            this.menuSettingLengthQuantize32 = new bocoree.windows.forms.BMenuItem();
            this.menuSettingLengthQuantize64 = new bocoree.windows.forms.BMenuItem();
            this.menuSettingLengthQuantize128 = new bocoree.windows.forms.BMenuItem();
            this.menuSettingLengthQuantizeOff = new bocoree.windows.forms.BMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSettingLengthQuantizeTriplet = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSettingSingerProperty = new bocoree.windows.forms.BMenuItem();
            this.menuHelp = new bocoree.windows.forms.BMenuItem();
            this.menuHelpAbout = new bocoree.windows.forms.BMenuItem();
            this.menuHelpDebug = new bocoree.windows.forms.BMenuItem();
            this.menuHidden = new bocoree.windows.forms.BMenuItem();
            this.menuHiddenEditLyric = new bocoree.windows.forms.BMenuItem();
            this.menuHiddenEditFlipToolPointerPencil = new bocoree.windows.forms.BMenuItem();
            this.menuHiddenEditFlipToolPointerEraser = new bocoree.windows.forms.BMenuItem();
            this.menuHiddenVisualForwardParameter = new bocoree.windows.forms.BMenuItem();
            this.menuHiddenVisualBackwardParameter = new bocoree.windows.forms.BMenuItem();
            this.menuHiddenTrackNext = new bocoree.windows.forms.BMenuItem();
            this.menuHiddenTrackBack = new bocoree.windows.forms.BMenuItem();
            this.menuHiddenCopy = new bocoree.windows.forms.BMenuItem();
            this.menuHiddenPaste = new bocoree.windows.forms.BMenuItem();
            this.menuHiddenCut = new bocoree.windows.forms.BMenuItem();
            this.cMenuPiano = new BPopupMenu( this.components );
            this.cMenuPianoPointer = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoPencil = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoEraser = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoPaletteTool = new bocoree.windows.forms.BMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoCurve = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoFixed = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoFixed01 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoFixed02 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoFixed04 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoFixed08 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoFixed16 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoFixed32 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoFixed64 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoFixed128 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoFixedOff = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoFixedTriplet = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoFixedDotted = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoQuantize = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoQuantize04 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoQuantize08 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoQuantize16 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoQuantize32 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoQuantize64 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoQuantize128 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoQuantizeOff = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem26 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoQuantizeTriplet = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoLength = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoLength04 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoLength08 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoLength16 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoLength32 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoLength64 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoLength128 = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoLengthOff = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem32 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoLengthTriplet = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoGrid = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoUndo = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoRedo = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoCut = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoCopy = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoPaste = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoDelete = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoSelectAll = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoSelectAllEvents = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoImportLyric = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoExpressionProperty = new bocoree.windows.forms.BMenuItem();
            this.cMenuPianoVibratoProperty = new bocoree.windows.forms.BMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip( this.components );
            this.cMenuTrackTab = new BPopupMenu( this.components );
            this.cMenuTrackTabTrackOn = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackTabAdd = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackTabCopy = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackTabChangeName = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackTabDelete = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackTabRenderCurrent = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackTabRenderAll = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem27 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackTabOverlay = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackTabRenderer = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackTabRendererVOCALOID1 = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackTabRendererVOCALOID2 = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackTabRendererUtau = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackTabRendererStraight = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackSelector = new BPopupMenu( this.components );
            this.cMenuTrackSelectorPointer = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackSelectorPencil = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackSelectorLine = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackSelectorEraser = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackSelectorPaletteTool = new bocoree.windows.forms.BMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackSelectorCurve = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem28 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackSelectorUndo = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackSelectorRedo = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem29 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackSelectorCut = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackSelectorCopy = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackSelectorPaste = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackSelectorDelete = new bocoree.windows.forms.BMenuItem();
            this.cMenuTrackSelectorDeleteBezier = new bocoree.windows.forms.BMenuItem();
            this.toolStripMenuItem31 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackSelectorSelectAll = new bocoree.windows.forms.BMenuItem();
            this.trackBar = new BSlider();
            this.panel1 = new BPanel();
            this.pictKeyLengthSplitter = new BPictureBox();
            this.panel3 = new BPanel();
            this.btnRight1 = new BButton();
            this.btnLeft2 = new BButton();
            this.btnZoom = new BButton();
            this.btnMooz = new BButton();
            this.btnLeft1 = new BButton();
            this.btnRight2 = new BButton();
            this.pictOverview = new BPictureBox();
            this.vScroll = new BVScrollBar();
            this.hScroll = new BHScrollBar();
            this.picturePositionIndicator = new BPictureBox();
            this.pictPianoRoll = new Boare.Cadencii.PictPianoRoll();
            this.pictureBox3 = new BPictureBox();
            this.pictureBox2 = new BPictureBox();
            this.toolStripTool = new bocoree.windows.forms.BToolBar();
            this.stripBtnPointer = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnPencil = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnLine = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnEraser = new bocoree.windows.forms.BToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.stripBtnGrid = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnCurve = new bocoree.windows.forms.BToolStripButton();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.toolStripBottom = new BToolBar();
            this.toolStripLabel6 = new BToolStripLabel();
            this.stripLblCursor = new BToolStripLabel();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel8 = new BToolStripLabel();
            this.stripLblTempo = new BToolStripLabel();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel10 = new BToolStripLabel();
            this.stripLblBeat = new BToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripStatusLabel1 = new BStatusLabel();
            this.stripLblGameCtrlMode = new BStatusLabel();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripStatusLabel2 = new BStatusLabel();
            this.stripLblMidiIn = new BStatusLabel();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.stripDDBtnSpeed = new BToolStripDropDownButton();
            this.stripDDBtnSpeedTextbox = new BToolStripTextBox();
            this.stripDDBtnSpeed033 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnSpeed050 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnSpeed100 = new bocoree.windows.forms.BMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new BStatusLabel();
            this.splitContainerProperty = new Boare.Lib.AppUtil.BSplitContainer();
            this.panel2 = new BPanel();
            this.waveView = new Boare.Cadencii.WaveView();
            this.splitContainer2 = new Boare.Lib.AppUtil.BSplitContainer();
            this.splitContainer1 = new Boare.Lib.AppUtil.BSplitContainer();
            this.toolStripFile = new BToolBar();
            this.stripBtnFileNew = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnFileOpen = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnFileSave = new bocoree.windows.forms.BToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.stripBtnCut = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnCopy = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnPaste = new bocoree.windows.forms.BToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.stripBtnUndo = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnRedo = new bocoree.windows.forms.BToolStripButton();
            this.toolStripPosition = new BToolBar();
            this.stripBtnMoveTop = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnRewind = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnForward = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnMoveEnd = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnPlay = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnStop = new bocoree.windows.forms.BToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.stripBtnScroll = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnLoop = new bocoree.windows.forms.BToolStripButton();
            this.toolStripMeasure = new BToolBar();
            this.toolStripLabel5 = new BToolStripLabel();
            this.stripLblMeasure = new BToolStripLabel();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.stripDDBtnLength = new BToolStripDropDownButton();
            this.stripDDBtnLength04 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnLength08 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnLength16 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnLength32 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnLength64 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnLength128 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnLengthOff = new bocoree.windows.forms.BMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.stripDDBtnLengthTriplet = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnQuantize = new BToolStripDropDownButton();
            this.stripDDBtnQuantize04 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnQuantize08 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnQuantize16 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnQuantize32 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnQuantize64 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnQuantize128 = new bocoree.windows.forms.BMenuItem();
            this.stripDDBtnQuantizeOff = new bocoree.windows.forms.BMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.stripDDBtnQuantizeTriplet = new bocoree.windows.forms.BMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.stripBtnStartMarker = new bocoree.windows.forms.BToolStripButton();
            this.stripBtnEndMarker = new bocoree.windows.forms.BToolStripButton();
            this.menuStripMain.SuspendLayout();
            this.cMenuPiano.SuspendLayout();
            this.cMenuTrackTab.SuspendLayout();
            this.cMenuTrackSelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictKeyLengthSplitter)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictOverview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePositionIndicator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictPianoRoll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.toolStripTool.SuspendLayout();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.toolStripBottom.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStripFile.SuspendLayout();
            this.toolStripPosition.SuspendLayout();
            this.toolStripMeasure.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuVisual,
            this.menuJob,
            this.menuTrack,
            this.menuLyric,
            this.menuScript,
            this.menuSetting,
            this.menuHelp,
            this.menuHidden} );
            this.menuStripMain.Location = new System.Drawing.Point( 0, 0 );
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size( 960, 24 );
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFileNew,
            this.menuFileOpen,
            this.menuFileSave,
            this.menuFileSaveNamed,
            this.toolStripMenuItem10,
            this.menuFileOpenVsq,
            this.menuFileOpenUst,
            this.menuFileImport,
            this.menuFileExport,
            this.toolStripMenuItem11,
            this.menuFileRecent,
            this.toolStripMenuItem12,
            this.menuFileQuit} );
            this.menuFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size( 51, 20 );
            this.menuFile.Text = "File(&F)";
            // 
            // menuFileNew
            // 
            this.menuFileNew.Name = "menuFileNew";
            this.menuFileNew.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileNew.Text = "New(N)";
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Name = "menuFileOpen";
            this.menuFileOpen.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileOpen.Text = "Open(&O)";
            // 
            // menuFileSave
            // 
            this.menuFileSave.Name = "menuFileSave";
            this.menuFileSave.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileSave.Text = "Save(&S)";
            // 
            // menuFileSaveNamed
            // 
            this.menuFileSaveNamed.Name = "menuFileSaveNamed";
            this.menuFileSaveNamed.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileSaveNamed.Text = "Save As(&A)";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size( 211, 6 );
            // 
            // menuFileOpenVsq
            // 
            this.menuFileOpenVsq.Name = "menuFileOpenVsq";
            this.menuFileOpenVsq.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileOpenVsq.Text = "Open VSQ/Vocaloid Midi(&V)";
            // 
            // menuFileOpenUst
            // 
            this.menuFileOpenUst.Name = "menuFileOpenUst";
            this.menuFileOpenUst.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileOpenUst.Text = "Open UTAU Project File(&U)";
            // 
            // menuFileImport
            // 
            this.menuFileImport.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFileImportVsq,
            this.menuFileImportMidi} );
            this.menuFileImport.Name = "menuFileImport";
            this.menuFileImport.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileImport.Text = "Import(&I)";
            // 
            // menuFileImportVsq
            // 
            this.menuFileImportVsq.Name = "menuFileImportVsq";
            this.menuFileImportVsq.Size = new System.Drawing.Size( 142, 22 );
            this.menuFileImportVsq.Text = "VSQ File";
            // 
            // menuFileImportMidi
            // 
            this.menuFileImportMidi.Name = "menuFileImportMidi";
            this.menuFileImportMidi.Size = new System.Drawing.Size( 142, 22 );
            this.menuFileImportMidi.Text = "Standard MIDI";
            // 
            // menuFileExport
            // 
            this.menuFileExport.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFileExportWave,
            this.menuFileExportMidi} );
            this.menuFileExport.Name = "menuFileExport";
            this.menuFileExport.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileExport.Text = "Export(&E)";
            // 
            // menuFileExportWave
            // 
            this.menuFileExportWave.Name = "menuFileExportWave";
            this.menuFileExportWave.Size = new System.Drawing.Size( 97, 22 );
            this.menuFileExportWave.Text = "Wave";
            // 
            // menuFileExportMidi
            // 
            this.menuFileExportMidi.Name = "menuFileExportMidi";
            this.menuFileExportMidi.Size = new System.Drawing.Size( 97, 22 );
            this.menuFileExportMidi.Text = "MIDI";
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size( 211, 6 );
            // 
            // menuFileRecent
            // 
            this.menuFileRecent.Name = "menuFileRecent";
            this.menuFileRecent.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileRecent.Text = "Recent Files(&R)";
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size( 211, 6 );
            // 
            // menuFileQuit
            // 
            this.menuFileQuit.Name = "menuFileQuit";
            this.menuFileQuit.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileQuit.Text = "Quit(&Q)";
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuEditUndo,
            this.menuEditRedo,
            this.toolStripMenuItem5,
            this.menuEditCut,
            this.menuEditCopy,
            this.menuEditPaste,
            this.menuEditDelete,
            this.toolStripMenuItem19,
            this.menuEditAutoNormalizeMode,
            this.toolStripMenuItem20,
            this.menuEditSelectAll,
            this.menuEditSelectAllEvents} );
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size( 52, 20 );
            this.menuEdit.Text = "Edit(&E)";
            // 
            // menuEditUndo
            // 
            this.menuEditUndo.Name = "menuEditUndo";
            this.menuEditUndo.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditUndo.Text = "Undo(&U)";
            // 
            // menuEditRedo
            // 
            this.menuEditRedo.Name = "menuEditRedo";
            this.menuEditRedo.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditRedo.Text = "Redo(&R)";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size( 192, 6 );
            // 
            // menuEditCut
            // 
            this.menuEditCut.Name = "menuEditCut";
            this.menuEditCut.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditCut.Text = "Cut(&T)";
            // 
            // menuEditCopy
            // 
            this.menuEditCopy.Name = "menuEditCopy";
            this.menuEditCopy.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditCopy.Text = "Copy(&C)";
            // 
            // menuEditPaste
            // 
            this.menuEditPaste.Name = "menuEditPaste";
            this.menuEditPaste.ShortcutKeyDisplayString = "";
            this.menuEditPaste.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditPaste.Text = "Paste(&P)";
            // 
            // menuEditDelete
            // 
            this.menuEditDelete.Name = "menuEditDelete";
            this.menuEditDelete.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditDelete.Text = "Delete(&D)";
            // 
            // toolStripMenuItem19
            // 
            this.toolStripMenuItem19.Name = "toolStripMenuItem19";
            this.toolStripMenuItem19.Size = new System.Drawing.Size( 192, 6 );
            // 
            // menuEditAutoNormalizeMode
            // 
            this.menuEditAutoNormalizeMode.Name = "menuEditAutoNormalizeMode";
            this.menuEditAutoNormalizeMode.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditAutoNormalizeMode.Text = "Auto Normalize Mode(&N)";
            // 
            // toolStripMenuItem20
            // 
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            this.toolStripMenuItem20.Size = new System.Drawing.Size( 192, 6 );
            // 
            // menuEditSelectAll
            // 
            this.menuEditSelectAll.Name = "menuEditSelectAll";
            this.menuEditSelectAll.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditSelectAll.Text = "Select All(&A)";
            // 
            // menuEditSelectAllEvents
            // 
            this.menuEditSelectAllEvents.Name = "menuEditSelectAllEvents";
            this.menuEditSelectAllEvents.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditSelectAllEvents.Text = "Select All Events(&E)";
            // 
            // menuVisual
            // 
            this.menuVisual.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisual.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuVisualControlTrack,
            this.menuVisualMixer,
            this.menuVisualWaveform,
            this.menuVisualProperty,
            this.menuVisualOverview,
            this.toolStripMenuItem1,
            this.menuVisualGridline,
            this.toolStripMenuItem2,
            this.menuVisualStartMarker,
            this.menuVisualEndMarker,
            this.toolStripMenuItem3,
            this.menuVisualLyrics,
            this.menuVisualNoteProperty,
            this.menuVisualPitchLine} );
            this.menuVisual.Name = "menuVisual";
            this.menuVisual.Size = new System.Drawing.Size( 58, 20 );
            this.menuVisual.Text = "View(&V)";
            // 
            // menuVisualControlTrack
            // 
            this.menuVisualControlTrack.setSelected( true );
            this.menuVisualControlTrack.CheckOnClick = true;
            this.menuVisualControlTrack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuVisualControlTrack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualControlTrack.Name = "menuVisualControlTrack";
            this.menuVisualControlTrack.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualControlTrack.Text = "Control Track(&C)";
            // 
            // menuVisualMixer
            // 
            this.menuVisualMixer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualMixer.Name = "menuVisualMixer";
            this.menuVisualMixer.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualMixer.Text = "Mixer(&X)";
            // 
            // menuVisualWaveform
            // 
            this.menuVisualWaveform.CheckOnClick = true;
            this.menuVisualWaveform.Name = "menuVisualWaveform";
            this.menuVisualWaveform.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualWaveform.Text = "Waveform(&W)";
            // 
            // menuVisualProperty
            // 
            this.menuVisualProperty.CheckOnClick = true;
            this.menuVisualProperty.Name = "menuVisualProperty";
            this.menuVisualProperty.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualProperty.Text = "Property Window(&C)";
            // 
            // menuVisualOverview
            // 
            this.menuVisualOverview.CheckOnClick = true;
            this.menuVisualOverview.Name = "menuVisualOverview";
            this.menuVisualOverview.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualOverview.Text = "Overview(&O)";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size( 210, 6 );
            // 
            // menuVisualGridline
            // 
            this.menuVisualGridline.CheckOnClick = true;
            this.menuVisualGridline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualGridline.Name = "menuVisualGridline";
            this.menuVisualGridline.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualGridline.Text = "Grid Line(&G)";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size( 210, 6 );
            // 
            // menuVisualStartMarker
            // 
            this.menuVisualStartMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualStartMarker.Name = "menuVisualStartMarker";
            this.menuVisualStartMarker.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualStartMarker.Text = "Start Marker(&S)";
            // 
            // menuVisualEndMarker
            // 
            this.menuVisualEndMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualEndMarker.Name = "menuVisualEndMarker";
            this.menuVisualEndMarker.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualEndMarker.Text = "End Marker(&E)";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size( 210, 6 );
            // 
            // menuVisualLyrics
            // 
            this.menuVisualLyrics.setSelected( true );
            this.menuVisualLyrics.CheckOnClick = true;
            this.menuVisualLyrics.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuVisualLyrics.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualLyrics.Name = "menuVisualLyrics";
            this.menuVisualLyrics.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualLyrics.Text = "Lyric/Phoneme(&L)";
            // 
            // menuVisualNoteProperty
            // 
            this.menuVisualNoteProperty.setSelected( true );
            this.menuVisualNoteProperty.CheckOnClick = true;
            this.menuVisualNoteProperty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuVisualNoteProperty.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualNoteProperty.Name = "menuVisualNoteProperty";
            this.menuVisualNoteProperty.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualNoteProperty.Text = "Note Expression/Vibrato(&N)";
            // 
            // menuVisualPitchLine
            // 
            this.menuVisualPitchLine.CheckOnClick = true;
            this.menuVisualPitchLine.Name = "menuVisualPitchLine";
            this.menuVisualPitchLine.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualPitchLine.Text = "Pitch Line(&P)";
            // 
            // menuJob
            // 
            this.menuJob.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuJobNormalize,
            this.menuJobInsertBar,
            this.menuJobDeleteBar,
            this.menuJobRandomize,
            this.menuJobConnect,
            this.menuJobLyric,
            this.menuJobRewire,
            this.menuJobRealTime,
            this.menuJobReloadVsti} );
            this.menuJob.Name = "menuJob";
            this.menuJob.Size = new System.Drawing.Size( 51, 20 );
            this.menuJob.Text = "Job(&J)";
            // 
            // menuJobNormalize
            // 
            this.menuJobNormalize.Name = "menuJobNormalize";
            this.menuJobNormalize.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobNormalize.Text = "Normalize Notes(&N)";
            // 
            // menuJobInsertBar
            // 
            this.menuJobInsertBar.Name = "menuJobInsertBar";
            this.menuJobInsertBar.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobInsertBar.Text = "Insert Bars(&I)";
            // 
            // menuJobDeleteBar
            // 
            this.menuJobDeleteBar.Name = "menuJobDeleteBar";
            this.menuJobDeleteBar.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobDeleteBar.Text = "Delete Bars(&D)";
            // 
            // menuJobRandomize
            // 
            this.menuJobRandomize.Enabled = false;
            this.menuJobRandomize.Name = "menuJobRandomize";
            this.menuJobRandomize.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobRandomize.Text = "Randomize(&R)";
            // 
            // menuJobConnect
            // 
            this.menuJobConnect.Name = "menuJobConnect";
            this.menuJobConnect.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobConnect.Text = "Connect Notes(&C)";
            // 
            // menuJobLyric
            // 
            this.menuJobLyric.Name = "menuJobLyric";
            this.menuJobLyric.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobLyric.Text = "Insert Lyrics(&L)";
            // 
            // menuJobRewire
            // 
            this.menuJobRewire.Enabled = false;
            this.menuJobRewire.Name = "menuJobRewire";
            this.menuJobRewire.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobRewire.Text = "Import ReWire Host Tempo(&T)";
            // 
            // menuJobRealTime
            // 
            this.menuJobRealTime.Name = "menuJobRealTime";
            this.menuJobRealTime.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobRealTime.Text = "Start Realtime Input";
            // 
            // menuJobReloadVsti
            // 
            this.menuJobReloadVsti.Name = "menuJobReloadVsti";
            this.menuJobReloadVsti.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobReloadVsti.Text = "Reload VSTi(&R)";
            this.menuJobReloadVsti.Visible = false;
            // 
            // menuTrack
            // 
            this.menuTrack.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuTrackOn,
            this.toolStripMenuItem21,
            this.menuTrackAdd,
            this.menuTrackCopy,
            this.menuTrackChangeName,
            this.menuTrackDelete,
            this.toolStripMenuItem22,
            this.menuTrackRenderCurrent,
            this.menuTrackRenderAll,
            this.toolStripMenuItem23,
            this.menuTrackOverlay,
            this.menuTrackRenderer,
            this.toolStripMenuItem4,
            this.menuTrackBgm,
            this.menuTrackManager} );
            this.menuTrack.Name = "menuTrack";
            this.menuTrack.Size = new System.Drawing.Size( 61, 20 );
            this.menuTrack.Text = "Track(&T)";
            // 
            // menuTrackOn
            // 
            this.menuTrackOn.Name = "menuTrackOn";
            this.menuTrackOn.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackOn.Text = "Track On(&K)";
            // 
            // toolStripMenuItem21
            // 
            this.toolStripMenuItem21.Name = "toolStripMenuItem21";
            this.toolStripMenuItem21.Size = new System.Drawing.Size( 193, 6 );
            // 
            // menuTrackAdd
            // 
            this.menuTrackAdd.Name = "menuTrackAdd";
            this.menuTrackAdd.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackAdd.Text = "Add Track(&A)";
            // 
            // menuTrackCopy
            // 
            this.menuTrackCopy.Name = "menuTrackCopy";
            this.menuTrackCopy.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackCopy.Text = "Copy Track(&C)";
            // 
            // menuTrackChangeName
            // 
            this.menuTrackChangeName.Name = "menuTrackChangeName";
            this.menuTrackChangeName.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackChangeName.Text = "Rename Track(&R)";
            // 
            // menuTrackDelete
            // 
            this.menuTrackDelete.Name = "menuTrackDelete";
            this.menuTrackDelete.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackDelete.Text = "Delete Track(&D)";
            // 
            // toolStripMenuItem22
            // 
            this.toolStripMenuItem22.Name = "toolStripMenuItem22";
            this.toolStripMenuItem22.Size = new System.Drawing.Size( 193, 6 );
            // 
            // menuTrackRenderCurrent
            // 
            this.menuTrackRenderCurrent.Name = "menuTrackRenderCurrent";
            this.menuTrackRenderCurrent.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackRenderCurrent.Text = "Render Current Track(&T)";
            // 
            // menuTrackRenderAll
            // 
            this.menuTrackRenderAll.Enabled = false;
            this.menuTrackRenderAll.Name = "menuTrackRenderAll";
            this.menuTrackRenderAll.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackRenderAll.Text = "Render All Tracks(&S)";
            // 
            // toolStripMenuItem23
            // 
            this.toolStripMenuItem23.Name = "toolStripMenuItem23";
            this.toolStripMenuItem23.Size = new System.Drawing.Size( 193, 6 );
            // 
            // menuTrackOverlay
            // 
            this.menuTrackOverlay.Name = "menuTrackOverlay";
            this.menuTrackOverlay.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackOverlay.Text = "Overlay(&O)";
            // 
            // menuTrackRenderer
            // 
            this.menuTrackRenderer.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuTrackRendererVOCALOID1,
            this.menuTrackRendererVOCALOID2,
            this.menuTrackRendererUtau,
            this.menuTrackRendererStraight} );
            this.menuTrackRenderer.Name = "menuTrackRenderer";
            this.menuTrackRenderer.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackRenderer.Text = "Renderer";
            // 
            // menuTrackRendererVOCALOID1
            // 
            this.menuTrackRendererVOCALOID1.Name = "menuTrackRendererVOCALOID1";
            this.menuTrackRendererVOCALOID1.Size = new System.Drawing.Size( 156, 22 );
            this.menuTrackRendererVOCALOID1.Text = "VOCALOID1";
            // 
            // menuTrackRendererVOCALOID2
            // 
            this.menuTrackRendererVOCALOID2.Name = "menuTrackRendererVOCALOID2";
            this.menuTrackRendererVOCALOID2.Size = new System.Drawing.Size( 156, 22 );
            this.menuTrackRendererVOCALOID2.Text = "VOCALOID2";
            // 
            // menuTrackRendererUtau
            // 
            this.menuTrackRendererUtau.Name = "menuTrackRendererUtau";
            this.menuTrackRendererUtau.Size = new System.Drawing.Size( 156, 22 );
            this.menuTrackRendererUtau.Text = "UTAU";
            // 
            // menuTrackRendererStraight
            // 
            this.menuTrackRendererStraight.Name = "menuTrackRendererStraight";
            this.menuTrackRendererStraight.Size = new System.Drawing.Size( 156, 22 );
            this.menuTrackRendererStraight.Text = "Straight X UTAU";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size( 193, 6 );
            // 
            // menuTrackBgm
            // 
            this.menuTrackBgm.Name = "menuTrackBgm";
            this.menuTrackBgm.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackBgm.Text = "BGM(&B)";
            // 
            // menuTrackManager
            // 
            this.menuTrackManager.Name = "menuTrackManager";
            this.menuTrackManager.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackManager.Text = "Track Manager(&M)";
            // 
            // menuLyric
            // 
            this.menuLyric.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuLyricExpressionProperty,
            this.menuLyricVibratoProperty,
            this.menuLyricSymbol,
            this.menuLyricDictionary} );
            this.menuLyric.Name = "menuLyric";
            this.menuLyric.Size = new System.Drawing.Size( 62, 20 );
            this.menuLyric.Text = "Lyrics(&L)";
            // 
            // menuLyricExpressionProperty
            // 
            this.menuLyricExpressionProperty.Name = "menuLyricExpressionProperty";
            this.menuLyricExpressionProperty.Size = new System.Drawing.Size( 216, 22 );
            this.menuLyricExpressionProperty.Text = "Note Expression Property(&E)";
            // 
            // menuLyricVibratoProperty
            // 
            this.menuLyricVibratoProperty.Name = "menuLyricVibratoProperty";
            this.menuLyricVibratoProperty.Size = new System.Drawing.Size( 216, 22 );
            this.menuLyricVibratoProperty.Text = "Note Vibrato Property(&V)";
            // 
            // menuLyricSymbol
            // 
            this.menuLyricSymbol.Enabled = false;
            this.menuLyricSymbol.Name = "menuLyricSymbol";
            this.menuLyricSymbol.Size = new System.Drawing.Size( 216, 22 );
            this.menuLyricSymbol.Text = "Phoneme Transformation(&T)";
            // 
            // menuLyricDictionary
            // 
            this.menuLyricDictionary.Name = "menuLyricDictionary";
            this.menuLyricDictionary.Size = new System.Drawing.Size( 216, 22 );
            this.menuLyricDictionary.Text = "User Word Dictionary(&C)";
            // 
            // menuScript
            // 
            this.menuScript.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuScriptUpdate} );
            this.menuScript.Name = "menuScript";
            this.menuScript.Size = new System.Drawing.Size( 63, 20 );
            this.menuScript.Text = "Script(&C)";
            // 
            // menuScriptUpdate
            // 
            this.menuScriptUpdate.Name = "menuScriptUpdate";
            this.menuScriptUpdate.Size = new System.Drawing.Size( 179, 22 );
            this.menuScriptUpdate.Text = "Update Script List(&U)";
            // 
            // menuSetting
            // 
            this.menuSetting.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuSettingPreference,
            this.menuSettingGameControler,
            this.menuSettingPaletteTool,
            this.menuSettingShortcut,
            this.menuSettingMidi,
            this.menuSettingUtauVoiceDB,
            this.toolStripMenuItem6,
            this.menuSettingDefaultSingerStyle,
            this.toolStripMenuItem7,
            this.menuSettingPositionQuantize,
            this.menuSettingLengthQuantize,
            this.toolStripMenuItem8,
            this.menuSettingSingerProperty} );
            this.menuSetting.Name = "menuSetting";
            this.menuSetting.Size = new System.Drawing.Size( 68, 20 );
            this.menuSetting.Text = "Setting(&S)";
            // 
            // menuSettingPreference
            // 
            this.menuSettingPreference.Name = "menuSettingPreference";
            this.menuSettingPreference.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingPreference.Text = "Preference(&P)";
            // 
            // menuSettingGameControler
            // 
            this.menuSettingGameControler.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuSettingGameControlerSetting,
            this.menuSettingGameControlerLoad,
            this.menuSettingGameControlerRemove} );
            this.menuSettingGameControler.Name = "menuSettingGameControler";
            this.menuSettingGameControler.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingGameControler.Text = "Game Controler(&G)";
            // 
            // menuSettingGameControlerSetting
            // 
            this.menuSettingGameControlerSetting.Name = "menuSettingGameControlerSetting";
            this.menuSettingGameControlerSetting.Size = new System.Drawing.Size( 127, 22 );
            this.menuSettingGameControlerSetting.Text = "Setting(&S)";
            // 
            // menuSettingGameControlerLoad
            // 
            this.menuSettingGameControlerLoad.Name = "menuSettingGameControlerLoad";
            this.menuSettingGameControlerLoad.Size = new System.Drawing.Size( 127, 22 );
            this.menuSettingGameControlerLoad.Text = "Load(&L)";
            // 
            // menuSettingGameControlerRemove
            // 
            this.menuSettingGameControlerRemove.Name = "menuSettingGameControlerRemove";
            this.menuSettingGameControlerRemove.Size = new System.Drawing.Size( 127, 22 );
            this.menuSettingGameControlerRemove.Text = "Remove(&R)";
            // 
            // menuSettingPaletteTool
            // 
            this.menuSettingPaletteTool.Name = "menuSettingPaletteTool";
            this.menuSettingPaletteTool.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingPaletteTool.Text = "Palette Tool(&T)";
            // 
            // menuSettingShortcut
            // 
            this.menuSettingShortcut.Name = "menuSettingShortcut";
            this.menuSettingShortcut.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingShortcut.Text = "Shortcut Key(&S)";
            // 
            // menuSettingMidi
            // 
            this.menuSettingMidi.Name = "menuSettingMidi";
            this.menuSettingMidi.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingMidi.Text = "MIDI(&M)";
            // 
            // menuSettingUtauVoiceDB
            // 
            this.menuSettingUtauVoiceDB.Name = "menuSettingUtauVoiceDB";
            this.menuSettingUtauVoiceDB.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingUtauVoiceDB.Text = "UTAU Voice DB(&U)";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size( 197, 6 );
            // 
            // menuSettingDefaultSingerStyle
            // 
            this.menuSettingDefaultSingerStyle.Name = "menuSettingDefaultSingerStyle";
            this.menuSettingDefaultSingerStyle.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingDefaultSingerStyle.Text = "Singing Style Defaults(&D)";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size( 197, 6 );
            // 
            // menuSettingPositionQuantize
            // 
            this.menuSettingPositionQuantize.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuSettingPositionQuantize04,
            this.menuSettingPositionQuantize08,
            this.menuSettingPositionQuantize16,
            this.menuSettingPositionQuantize32,
            this.menuSettingPositionQuantize64,
            this.menuSettingPositionQuantize128,
            this.menuSettingPositionQuantizeOff,
            this.toolStripMenuItem9,
            this.menuSettingPositionQuantizeTriplet} );
            this.menuSettingPositionQuantize.Name = "menuSettingPositionQuantize";
            this.menuSettingPositionQuantize.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingPositionQuantize.Text = "Quantize(&Q)";
            // 
            // menuSettingPositionQuantize04
            // 
            this.menuSettingPositionQuantize04.Name = "menuSettingPositionQuantize04";
            this.menuSettingPositionQuantize04.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantize04.Text = "1/4";
            // 
            // menuSettingPositionQuantize08
            // 
            this.menuSettingPositionQuantize08.Name = "menuSettingPositionQuantize08";
            this.menuSettingPositionQuantize08.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantize08.Text = "1/8";
            // 
            // menuSettingPositionQuantize16
            // 
            this.menuSettingPositionQuantize16.Name = "menuSettingPositionQuantize16";
            this.menuSettingPositionQuantize16.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantize16.Text = "1/16";
            // 
            // menuSettingPositionQuantize32
            // 
            this.menuSettingPositionQuantize32.Name = "menuSettingPositionQuantize32";
            this.menuSettingPositionQuantize32.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantize32.Text = "1/32";
            // 
            // menuSettingPositionQuantize64
            // 
            this.menuSettingPositionQuantize64.Name = "menuSettingPositionQuantize64";
            this.menuSettingPositionQuantize64.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantize64.Text = "1/64";
            // 
            // menuSettingPositionQuantize128
            // 
            this.menuSettingPositionQuantize128.Name = "menuSettingPositionQuantize128";
            this.menuSettingPositionQuantize128.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantize128.Text = "1/128";
            // 
            // menuSettingPositionQuantizeOff
            // 
            this.menuSettingPositionQuantizeOff.Name = "menuSettingPositionQuantizeOff";
            this.menuSettingPositionQuantizeOff.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantizeOff.Text = "Off";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size( 100, 6 );
            // 
            // menuSettingPositionQuantizeTriplet
            // 
            this.menuSettingPositionQuantizeTriplet.Name = "menuSettingPositionQuantizeTriplet";
            this.menuSettingPositionQuantizeTriplet.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantizeTriplet.Text = "Triplet";
            // 
            // menuSettingLengthQuantize
            // 
            this.menuSettingLengthQuantize.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuSettingLengthQuantize04,
            this.menuSettingLengthQuantize08,
            this.menuSettingLengthQuantize16,
            this.menuSettingLengthQuantize32,
            this.menuSettingLengthQuantize64,
            this.menuSettingLengthQuantize128,
            this.menuSettingLengthQuantizeOff,
            this.toolStripSeparator1,
            this.menuSettingLengthQuantizeTriplet} );
            this.menuSettingLengthQuantize.Name = "menuSettingLengthQuantize";
            this.menuSettingLengthQuantize.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingLengthQuantize.Text = "Length(&L)";
            // 
            // menuSettingLengthQuantize04
            // 
            this.menuSettingLengthQuantize04.Name = "menuSettingLengthQuantize04";
            this.menuSettingLengthQuantize04.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantize04.Text = "1/4";
            // 
            // menuSettingLengthQuantize08
            // 
            this.menuSettingLengthQuantize08.Name = "menuSettingLengthQuantize08";
            this.menuSettingLengthQuantize08.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantize08.Text = "1/8";
            // 
            // menuSettingLengthQuantize16
            // 
            this.menuSettingLengthQuantize16.Name = "menuSettingLengthQuantize16";
            this.menuSettingLengthQuantize16.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantize16.Text = "1/16";
            // 
            // menuSettingLengthQuantize32
            // 
            this.menuSettingLengthQuantize32.Name = "menuSettingLengthQuantize32";
            this.menuSettingLengthQuantize32.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantize32.Text = "1/32";
            // 
            // menuSettingLengthQuantize64
            // 
            this.menuSettingLengthQuantize64.Name = "menuSettingLengthQuantize64";
            this.menuSettingLengthQuantize64.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantize64.Text = "1/64";
            // 
            // menuSettingLengthQuantize128
            // 
            this.menuSettingLengthQuantize128.Name = "menuSettingLengthQuantize128";
            this.menuSettingLengthQuantize128.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantize128.Text = "1/128";
            // 
            // menuSettingLengthQuantizeOff
            // 
            this.menuSettingLengthQuantizeOff.Name = "menuSettingLengthQuantizeOff";
            this.menuSettingLengthQuantizeOff.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantizeOff.Text = "Off";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 100, 6 );
            // 
            // menuSettingLengthQuantizeTriplet
            // 
            this.menuSettingLengthQuantizeTriplet.Name = "menuSettingLengthQuantizeTriplet";
            this.menuSettingLengthQuantizeTriplet.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantizeTriplet.Text = "Triplet";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size( 197, 6 );
            // 
            // menuSettingSingerProperty
            // 
            this.menuSettingSingerProperty.Enabled = false;
            this.menuSettingSingerProperty.Name = "menuSettingSingerProperty";
            this.menuSettingSingerProperty.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingSingerProperty.Text = "Singer Properties(&S)";
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpAbout,
            this.menuHelpDebug} );
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size( 56, 20 );
            this.menuHelp.Text = "Help(&H)";
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size( 164, 22 );
            this.menuHelpAbout.Text = "About Cadencii(&A)";
            // 
            // menuHelpDebug
            // 
            this.menuHelpDebug.Name = "menuHelpDebug";
            this.menuHelpDebug.Size = new System.Drawing.Size( 164, 22 );
            this.menuHelpDebug.Text = "Debug";
            this.menuHelpDebug.Visible = false;
            // 
            // menuHidden
            // 
            this.menuHidden.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuHiddenEditLyric,
            this.menuHiddenEditFlipToolPointerPencil,
            this.menuHiddenEditFlipToolPointerEraser,
            this.menuHiddenVisualForwardParameter,
            this.menuHiddenVisualBackwardParameter,
            this.menuHiddenTrackNext,
            this.menuHiddenTrackBack,
            this.menuHiddenCopy,
            this.menuHiddenPaste,
            this.menuHiddenCut} );
            this.menuHidden.Name = "menuHidden";
            this.menuHidden.Size = new System.Drawing.Size( 79, 20 );
            this.menuHidden.Text = "MenuHidden";
            this.menuHidden.Visible = false;
            // 
            // menuHiddenEditLyric
            // 
            this.menuHiddenEditLyric.Name = "menuHiddenEditLyric";
            this.menuHiddenEditLyric.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.menuHiddenEditLyric.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenEditLyric.Text = "Start Lyric Input";
            this.menuHiddenEditLyric.Visible = false;
            // 
            // menuHiddenEditFlipToolPointerPencil
            // 
            this.menuHiddenEditFlipToolPointerPencil.Name = "menuHiddenEditFlipToolPointerPencil";
            this.menuHiddenEditFlipToolPointerPencil.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.menuHiddenEditFlipToolPointerPencil.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenEditFlipToolPointerPencil.Text = "Change Tool Pointer / Pencil";
            this.menuHiddenEditFlipToolPointerPencil.Visible = false;
            // 
            // menuHiddenEditFlipToolPointerEraser
            // 
            this.menuHiddenEditFlipToolPointerEraser.Name = "menuHiddenEditFlipToolPointerEraser";
            this.menuHiddenEditFlipToolPointerEraser.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.menuHiddenEditFlipToolPointerEraser.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenEditFlipToolPointerEraser.Text = "Change Tool Pointer/ Eraser";
            this.menuHiddenEditFlipToolPointerEraser.Visible = false;
            // 
            // menuHiddenVisualForwardParameter
            // 
            this.menuHiddenVisualForwardParameter.Name = "menuHiddenVisualForwardParameter";
            this.menuHiddenVisualForwardParameter.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.Next)));
            this.menuHiddenVisualForwardParameter.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenVisualForwardParameter.Text = "Next Control Curve";
            this.menuHiddenVisualForwardParameter.Visible = false;
            // 
            // menuHiddenVisualBackwardParameter
            // 
            this.menuHiddenVisualBackwardParameter.Name = "menuHiddenVisualBackwardParameter";
            this.menuHiddenVisualBackwardParameter.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.PageUp)));
            this.menuHiddenVisualBackwardParameter.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenVisualBackwardParameter.Text = "Previous Control Curve";
            this.menuHiddenVisualBackwardParameter.Visible = false;
            // 
            // menuHiddenTrackNext
            // 
            this.menuHiddenTrackNext.Name = "menuHiddenTrackNext";
            this.menuHiddenTrackNext.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Next)));
            this.menuHiddenTrackNext.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenTrackNext.Text = "Next Track";
            this.menuHiddenTrackNext.Visible = false;
            // 
            // menuHiddenTrackBack
            // 
            this.menuHiddenTrackBack.Name = "menuHiddenTrackBack";
            this.menuHiddenTrackBack.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.PageUp)));
            this.menuHiddenTrackBack.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenTrackBack.Text = "Previous Track";
            this.menuHiddenTrackBack.Visible = false;
            // 
            // menuHiddenCopy
            // 
            this.menuHiddenCopy.Name = "menuHiddenCopy";
            this.menuHiddenCopy.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenCopy.Text = "Copy";
            // 
            // menuHiddenPaste
            // 
            this.menuHiddenPaste.Name = "menuHiddenPaste";
            this.menuHiddenPaste.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenPaste.Text = "Paste";
            // 
            // menuHiddenCut
            // 
            this.menuHiddenCut.Name = "menuHiddenCut";
            this.menuHiddenCut.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenCut.Text = "Cut";
            // 
            // cMenuPiano
            // 
            this.cMenuPiano.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuPianoPointer,
            this.cMenuPianoPencil,
            this.cMenuPianoEraser,
            this.cMenuPianoPaletteTool,
            this.toolStripSeparator15,
            this.cMenuPianoCurve,
            this.toolStripMenuItem13,
            this.cMenuPianoFixed,
            this.cMenuPianoQuantize,
            this.cMenuPianoLength,
            this.cMenuPianoGrid,
            this.toolStripMenuItem14,
            this.cMenuPianoUndo,
            this.cMenuPianoRedo,
            this.toolStripMenuItem15,
            this.cMenuPianoCut,
            this.cMenuPianoCopy,
            this.cMenuPianoPaste,
            this.cMenuPianoDelete,
            this.toolStripMenuItem16,
            this.cMenuPianoSelectAll,
            this.cMenuPianoSelectAllEvents,
            this.toolStripMenuItem17,
            this.cMenuPianoImportLyric,
            this.cMenuPianoExpressionProperty,
            this.cMenuPianoVibratoProperty} );
            this.cMenuPiano.Name = "cMenuPiano";
            this.cMenuPiano.ShowCheckMargin = true;
            this.cMenuPiano.ShowImageMargin = false;
            this.cMenuPiano.Size = new System.Drawing.Size( 217, 480 );
            // 
            // cMenuPianoPointer
            // 
            this.cMenuPianoPointer.Name = "cMenuPianoPointer";
            this.cMenuPianoPointer.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoPointer.Text = "Arrow(&A)";
            // 
            // cMenuPianoPencil
            // 
            this.cMenuPianoPencil.Name = "cMenuPianoPencil";
            this.cMenuPianoPencil.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoPencil.Text = "Pencil(&W)";
            // 
            // cMenuPianoEraser
            // 
            this.cMenuPianoEraser.Name = "cMenuPianoEraser";
            this.cMenuPianoEraser.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoEraser.Text = "Eraser(&E)";
            // 
            // cMenuPianoPaletteTool
            // 
            this.cMenuPianoPaletteTool.Name = "cMenuPianoPaletteTool";
            this.cMenuPianoPaletteTool.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoPaletteTool.Text = "Palette Tool";
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size( 213, 6 );
            // 
            // cMenuPianoCurve
            // 
            this.cMenuPianoCurve.Name = "cMenuPianoCurve";
            this.cMenuPianoCurve.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoCurve.Text = "Curve(&V)";
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size( 213, 6 );
            // 
            // cMenuPianoFixed
            // 
            this.cMenuPianoFixed.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuPianoFixed01,
            this.cMenuPianoFixed02,
            this.cMenuPianoFixed04,
            this.cMenuPianoFixed08,
            this.cMenuPianoFixed16,
            this.cMenuPianoFixed32,
            this.cMenuPianoFixed64,
            this.cMenuPianoFixed128,
            this.cMenuPianoFixedOff,
            this.toolStripMenuItem18,
            this.cMenuPianoFixedTriplet,
            this.cMenuPianoFixedDotted} );
            this.cMenuPianoFixed.Name = "cMenuPianoFixed";
            this.cMenuPianoFixed.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoFixed.Text = "Note Fixed Length(&N)";
            // 
            // cMenuPianoFixed01
            // 
            this.cMenuPianoFixed01.Name = "cMenuPianoFixed01";
            this.cMenuPianoFixed01.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed01.Text = "1/ 1 [1920]";
            // 
            // cMenuPianoFixed02
            // 
            this.cMenuPianoFixed02.Name = "cMenuPianoFixed02";
            this.cMenuPianoFixed02.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed02.Text = "1/ 2 [960]";
            // 
            // cMenuPianoFixed04
            // 
            this.cMenuPianoFixed04.Name = "cMenuPianoFixed04";
            this.cMenuPianoFixed04.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed04.Text = "1/ 4 [480]";
            // 
            // cMenuPianoFixed08
            // 
            this.cMenuPianoFixed08.Name = "cMenuPianoFixed08";
            this.cMenuPianoFixed08.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed08.Text = "1/ 8 [240]";
            // 
            // cMenuPianoFixed16
            // 
            this.cMenuPianoFixed16.Name = "cMenuPianoFixed16";
            this.cMenuPianoFixed16.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed16.Text = "1/16 [120]";
            // 
            // cMenuPianoFixed32
            // 
            this.cMenuPianoFixed32.Name = "cMenuPianoFixed32";
            this.cMenuPianoFixed32.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed32.Text = "1/32 [60]";
            // 
            // cMenuPianoFixed64
            // 
            this.cMenuPianoFixed64.Name = "cMenuPianoFixed64";
            this.cMenuPianoFixed64.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed64.Text = "1/64 [30]";
            // 
            // cMenuPianoFixed128
            // 
            this.cMenuPianoFixed128.Name = "cMenuPianoFixed128";
            this.cMenuPianoFixed128.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed128.Text = "1/128[15]";
            // 
            // cMenuPianoFixedOff
            // 
            this.cMenuPianoFixedOff.Name = "cMenuPianoFixedOff";
            this.cMenuPianoFixedOff.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixedOff.Text = "オフ";
            // 
            // toolStripMenuItem18
            // 
            this.toolStripMenuItem18.Name = "toolStripMenuItem18";
            this.toolStripMenuItem18.Size = new System.Drawing.Size( 125, 6 );
            // 
            // cMenuPianoFixedTriplet
            // 
            this.cMenuPianoFixedTriplet.Name = "cMenuPianoFixedTriplet";
            this.cMenuPianoFixedTriplet.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixedTriplet.Text = "3連符";
            // 
            // cMenuPianoFixedDotted
            // 
            this.cMenuPianoFixedDotted.Name = "cMenuPianoFixedDotted";
            this.cMenuPianoFixedDotted.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixedDotted.Text = "付点";
            // 
            // cMenuPianoQuantize
            // 
            this.cMenuPianoQuantize.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuPianoQuantize04,
            this.cMenuPianoQuantize08,
            this.cMenuPianoQuantize16,
            this.cMenuPianoQuantize32,
            this.cMenuPianoQuantize64,
            this.cMenuPianoQuantize128,
            this.cMenuPianoQuantizeOff,
            this.toolStripMenuItem26,
            this.cMenuPianoQuantizeTriplet} );
            this.cMenuPianoQuantize.Name = "cMenuPianoQuantize";
            this.cMenuPianoQuantize.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoQuantize.Text = "Quantize(&Q)";
            // 
            // cMenuPianoQuantize04
            // 
            this.cMenuPianoQuantize04.Name = "cMenuPianoQuantize04";
            this.cMenuPianoQuantize04.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantize04.Text = "1/4";
            // 
            // cMenuPianoQuantize08
            // 
            this.cMenuPianoQuantize08.Name = "cMenuPianoQuantize08";
            this.cMenuPianoQuantize08.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantize08.Text = "1/8";
            // 
            // cMenuPianoQuantize16
            // 
            this.cMenuPianoQuantize16.Name = "cMenuPianoQuantize16";
            this.cMenuPianoQuantize16.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantize16.Text = "1/16";
            // 
            // cMenuPianoQuantize32
            // 
            this.cMenuPianoQuantize32.Name = "cMenuPianoQuantize32";
            this.cMenuPianoQuantize32.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantize32.Text = "1/32";
            // 
            // cMenuPianoQuantize64
            // 
            this.cMenuPianoQuantize64.Name = "cMenuPianoQuantize64";
            this.cMenuPianoQuantize64.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantize64.Text = "1/64";
            // 
            // cMenuPianoQuantize128
            // 
            this.cMenuPianoQuantize128.Name = "cMenuPianoQuantize128";
            this.cMenuPianoQuantize128.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantize128.Text = "1/128";
            // 
            // cMenuPianoQuantizeOff
            // 
            this.cMenuPianoQuantizeOff.Name = "cMenuPianoQuantizeOff";
            this.cMenuPianoQuantizeOff.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantizeOff.Text = "オフ";
            // 
            // toolStripMenuItem26
            // 
            this.toolStripMenuItem26.Name = "toolStripMenuItem26";
            this.toolStripMenuItem26.Size = new System.Drawing.Size( 97, 6 );
            // 
            // cMenuPianoQuantizeTriplet
            // 
            this.cMenuPianoQuantizeTriplet.Name = "cMenuPianoQuantizeTriplet";
            this.cMenuPianoQuantizeTriplet.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantizeTriplet.Text = "3連符";
            // 
            // cMenuPianoLength
            // 
            this.cMenuPianoLength.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuPianoLength04,
            this.cMenuPianoLength08,
            this.cMenuPianoLength16,
            this.cMenuPianoLength32,
            this.cMenuPianoLength64,
            this.cMenuPianoLength128,
            this.cMenuPianoLengthOff,
            this.toolStripMenuItem32,
            this.cMenuPianoLengthTriplet} );
            this.cMenuPianoLength.Name = "cMenuPianoLength";
            this.cMenuPianoLength.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoLength.Text = "Length(&L)";
            // 
            // cMenuPianoLength04
            // 
            this.cMenuPianoLength04.Name = "cMenuPianoLength04";
            this.cMenuPianoLength04.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLength04.Text = "1/4";
            // 
            // cMenuPianoLength08
            // 
            this.cMenuPianoLength08.Name = "cMenuPianoLength08";
            this.cMenuPianoLength08.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLength08.Text = "1/8";
            // 
            // cMenuPianoLength16
            // 
            this.cMenuPianoLength16.Name = "cMenuPianoLength16";
            this.cMenuPianoLength16.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLength16.Text = "1/16";
            // 
            // cMenuPianoLength32
            // 
            this.cMenuPianoLength32.Name = "cMenuPianoLength32";
            this.cMenuPianoLength32.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLength32.Text = "1/32";
            // 
            // cMenuPianoLength64
            // 
            this.cMenuPianoLength64.Name = "cMenuPianoLength64";
            this.cMenuPianoLength64.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLength64.Text = "1/64";
            // 
            // cMenuPianoLength128
            // 
            this.cMenuPianoLength128.Name = "cMenuPianoLength128";
            this.cMenuPianoLength128.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLength128.Text = "1/128";
            // 
            // cMenuPianoLengthOff
            // 
            this.cMenuPianoLengthOff.Name = "cMenuPianoLengthOff";
            this.cMenuPianoLengthOff.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLengthOff.Text = "オフ";
            // 
            // toolStripMenuItem32
            // 
            this.toolStripMenuItem32.Name = "toolStripMenuItem32";
            this.toolStripMenuItem32.Size = new System.Drawing.Size( 97, 6 );
            // 
            // cMenuPianoLengthTriplet
            // 
            this.cMenuPianoLengthTriplet.Name = "cMenuPianoLengthTriplet";
            this.cMenuPianoLengthTriplet.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLengthTriplet.Text = "3連符";
            // 
            // cMenuPianoGrid
            // 
            this.cMenuPianoGrid.Name = "cMenuPianoGrid";
            this.cMenuPianoGrid.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoGrid.Text = "Show/Hide Grid Line(&S)";
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size( 213, 6 );
            // 
            // cMenuPianoUndo
            // 
            this.cMenuPianoUndo.Name = "cMenuPianoUndo";
            this.cMenuPianoUndo.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoUndo.Text = "Undo(&U)";
            // 
            // cMenuPianoRedo
            // 
            this.cMenuPianoRedo.Name = "cMenuPianoRedo";
            this.cMenuPianoRedo.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoRedo.Text = "Redo(&R)";
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size( 213, 6 );
            // 
            // cMenuPianoCut
            // 
            this.cMenuPianoCut.Name = "cMenuPianoCut";
            this.cMenuPianoCut.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoCut.Text = "Cut(&T)";
            // 
            // cMenuPianoCopy
            // 
            this.cMenuPianoCopy.Name = "cMenuPianoCopy";
            this.cMenuPianoCopy.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoCopy.Text = "Copy(&C)";
            // 
            // cMenuPianoPaste
            // 
            this.cMenuPianoPaste.Name = "cMenuPianoPaste";
            this.cMenuPianoPaste.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoPaste.Text = "Paste(&P)";
            // 
            // cMenuPianoDelete
            // 
            this.cMenuPianoDelete.Name = "cMenuPianoDelete";
            this.cMenuPianoDelete.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoDelete.Text = "Delete(&D)";
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new System.Drawing.Size( 213, 6 );
            // 
            // cMenuPianoSelectAll
            // 
            this.cMenuPianoSelectAll.Name = "cMenuPianoSelectAll";
            this.cMenuPianoSelectAll.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoSelectAll.Text = "Select All(&A)";
            // 
            // cMenuPianoSelectAllEvents
            // 
            this.cMenuPianoSelectAllEvents.Name = "cMenuPianoSelectAllEvents";
            this.cMenuPianoSelectAllEvents.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoSelectAllEvents.Text = "Select All Events(&E)";
            // 
            // toolStripMenuItem17
            // 
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            this.toolStripMenuItem17.Size = new System.Drawing.Size( 213, 6 );
            // 
            // cMenuPianoImportLyric
            // 
            this.cMenuPianoImportLyric.Name = "cMenuPianoImportLyric";
            this.cMenuPianoImportLyric.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoImportLyric.Text = "Insert Lyrics(&L)";
            // 
            // cMenuPianoExpressionProperty
            // 
            this.cMenuPianoExpressionProperty.Name = "cMenuPianoExpressionProperty";
            this.cMenuPianoExpressionProperty.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoExpressionProperty.Text = "Note Expression Property(&P)";
            // 
            // cMenuPianoVibratoProperty
            // 
            this.cMenuPianoVibratoProperty.Name = "cMenuPianoVibratoProperty";
            this.cMenuPianoVibratoProperty.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoVibratoProperty.Text = "Note Vibrato Property";
            // 
            // cMenuTrackTab
            // 
            this.cMenuTrackTab.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuTrackTabTrackOn,
            this.toolStripMenuItem24,
            this.cMenuTrackTabAdd,
            this.cMenuTrackTabCopy,
            this.cMenuTrackTabChangeName,
            this.cMenuTrackTabDelete,
            this.toolStripMenuItem25,
            this.cMenuTrackTabRenderCurrent,
            this.cMenuTrackTabRenderAll,
            this.toolStripMenuItem27,
            this.cMenuTrackTabOverlay,
            this.cMenuTrackTabRenderer} );
            this.cMenuTrackTab.Name = "cMenuTrackTab";
            this.cMenuTrackTab.Size = new System.Drawing.Size( 197, 220 );
            // 
            // cMenuTrackTabTrackOn
            // 
            this.cMenuTrackTabTrackOn.Name = "cMenuTrackTabTrackOn";
            this.cMenuTrackTabTrackOn.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabTrackOn.Text = "Track On(&K)";
            // 
            // toolStripMenuItem24
            // 
            this.toolStripMenuItem24.Name = "toolStripMenuItem24";
            this.toolStripMenuItem24.Size = new System.Drawing.Size( 193, 6 );
            // 
            // cMenuTrackTabAdd
            // 
            this.cMenuTrackTabAdd.Name = "cMenuTrackTabAdd";
            this.cMenuTrackTabAdd.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabAdd.Text = "Add Track(&A)";
            // 
            // cMenuTrackTabCopy
            // 
            this.cMenuTrackTabCopy.Name = "cMenuTrackTabCopy";
            this.cMenuTrackTabCopy.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabCopy.Text = "Copy Track(&C)";
            // 
            // cMenuTrackTabChangeName
            // 
            this.cMenuTrackTabChangeName.Name = "cMenuTrackTabChangeName";
            this.cMenuTrackTabChangeName.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabChangeName.Text = "Rename Track(&R)";
            // 
            // cMenuTrackTabDelete
            // 
            this.cMenuTrackTabDelete.Name = "cMenuTrackTabDelete";
            this.cMenuTrackTabDelete.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabDelete.Text = "Delete Track(&D)";
            // 
            // toolStripMenuItem25
            // 
            this.toolStripMenuItem25.Name = "toolStripMenuItem25";
            this.toolStripMenuItem25.Size = new System.Drawing.Size( 193, 6 );
            // 
            // cMenuTrackTabRenderCurrent
            // 
            this.cMenuTrackTabRenderCurrent.Name = "cMenuTrackTabRenderCurrent";
            this.cMenuTrackTabRenderCurrent.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabRenderCurrent.Text = "Render Current Track(&T)";
            // 
            // cMenuTrackTabRenderAll
            // 
            this.cMenuTrackTabRenderAll.Name = "cMenuTrackTabRenderAll";
            this.cMenuTrackTabRenderAll.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabRenderAll.Text = "Render All Tracks(&S)";
            // 
            // toolStripMenuItem27
            // 
            this.toolStripMenuItem27.Name = "toolStripMenuItem27";
            this.toolStripMenuItem27.Size = new System.Drawing.Size( 193, 6 );
            // 
            // cMenuTrackTabOverlay
            // 
            this.cMenuTrackTabOverlay.Name = "cMenuTrackTabOverlay";
            this.cMenuTrackTabOverlay.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabOverlay.Text = "Overlay(&O)";
            // 
            // cMenuTrackTabRenderer
            // 
            this.cMenuTrackTabRenderer.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuTrackTabRendererVOCALOID1,
            this.cMenuTrackTabRendererVOCALOID2,
            this.cMenuTrackTabRendererUtau,
            this.cMenuTrackTabRendererStraight} );
            this.cMenuTrackTabRenderer.Name = "cMenuTrackTabRenderer";
            this.cMenuTrackTabRenderer.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabRenderer.Text = "Renderer";
            // 
            // cMenuTrackTabRendererVOCALOID1
            // 
            this.cMenuTrackTabRendererVOCALOID1.Name = "cMenuTrackTabRendererVOCALOID1";
            this.cMenuTrackTabRendererVOCALOID1.Size = new System.Drawing.Size( 160, 22 );
            this.cMenuTrackTabRendererVOCALOID1.Text = "VOCALOID1";
            // 
            // cMenuTrackTabRendererVOCALOID2
            // 
            this.cMenuTrackTabRendererVOCALOID2.Name = "cMenuTrackTabRendererVOCALOID2";
            this.cMenuTrackTabRendererVOCALOID2.Size = new System.Drawing.Size( 160, 22 );
            this.cMenuTrackTabRendererVOCALOID2.Text = "VOCALOID2";
            // 
            // cMenuTrackTabRendererUtau
            // 
            this.cMenuTrackTabRendererUtau.Name = "cMenuTrackTabRendererUtau";
            this.cMenuTrackTabRendererUtau.Size = new System.Drawing.Size( 160, 22 );
            this.cMenuTrackTabRendererUtau.Text = "UTAU";
            // 
            // cMenuTrackTabRendererStraight
            // 
            this.cMenuTrackTabRendererStraight.Name = "cMenuTrackTabRendererStraight";
            this.cMenuTrackTabRendererStraight.Size = new System.Drawing.Size( 160, 22 );
            this.cMenuTrackTabRendererStraight.Text = "Straight X UTAU ";
            // 
            // cMenuTrackSelector
            // 
            this.cMenuTrackSelector.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuTrackSelectorPointer,
            this.cMenuTrackSelectorPencil,
            this.cMenuTrackSelectorLine,
            this.cMenuTrackSelectorEraser,
            this.cMenuTrackSelectorPaletteTool,
            this.toolStripSeparator14,
            this.cMenuTrackSelectorCurve,
            this.toolStripMenuItem28,
            this.cMenuTrackSelectorUndo,
            this.cMenuTrackSelectorRedo,
            this.toolStripMenuItem29,
            this.cMenuTrackSelectorCut,
            this.cMenuTrackSelectorCopy,
            this.cMenuTrackSelectorPaste,
            this.cMenuTrackSelectorDelete,
            this.cMenuTrackSelectorDeleteBezier,
            this.toolStripMenuItem31,
            this.cMenuTrackSelectorSelectAll} );
            this.cMenuTrackSelector.Name = "cMenuTrackSelector";
            this.cMenuTrackSelector.Size = new System.Drawing.Size( 186, 358 );
            // 
            // cMenuTrackSelectorPointer
            // 
            this.cMenuTrackSelectorPointer.Name = "cMenuTrackSelectorPointer";
            this.cMenuTrackSelectorPointer.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorPointer.Text = "Arrow(&A)";
            // 
            // cMenuTrackSelectorPencil
            // 
            this.cMenuTrackSelectorPencil.Name = "cMenuTrackSelectorPencil";
            this.cMenuTrackSelectorPencil.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorPencil.Text = "Pencil(&W)";
            // 
            // cMenuTrackSelectorLine
            // 
            this.cMenuTrackSelectorLine.Name = "cMenuTrackSelectorLine";
            this.cMenuTrackSelectorLine.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorLine.Text = "Line(&L)";
            // 
            // cMenuTrackSelectorEraser
            // 
            this.cMenuTrackSelectorEraser.Name = "cMenuTrackSelectorEraser";
            this.cMenuTrackSelectorEraser.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorEraser.Text = "Eraser(&E)";
            // 
            // cMenuTrackSelectorPaletteTool
            // 
            this.cMenuTrackSelectorPaletteTool.Name = "cMenuTrackSelectorPaletteTool";
            this.cMenuTrackSelectorPaletteTool.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorPaletteTool.Text = "Palette Tool";
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size( 182, 6 );
            // 
            // cMenuTrackSelectorCurve
            // 
            this.cMenuTrackSelectorCurve.Name = "cMenuTrackSelectorCurve";
            this.cMenuTrackSelectorCurve.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorCurve.Text = "Curve(&V)";
            // 
            // toolStripMenuItem28
            // 
            this.toolStripMenuItem28.Name = "toolStripMenuItem28";
            this.toolStripMenuItem28.Size = new System.Drawing.Size( 182, 6 );
            // 
            // cMenuTrackSelectorUndo
            // 
            this.cMenuTrackSelectorUndo.Name = "cMenuTrackSelectorUndo";
            this.cMenuTrackSelectorUndo.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorUndo.Text = "Undo(&U)";
            // 
            // cMenuTrackSelectorRedo
            // 
            this.cMenuTrackSelectorRedo.Name = "cMenuTrackSelectorRedo";
            this.cMenuTrackSelectorRedo.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorRedo.Text = "Redo(&R)";
            // 
            // toolStripMenuItem29
            // 
            this.toolStripMenuItem29.Name = "toolStripMenuItem29";
            this.toolStripMenuItem29.Size = new System.Drawing.Size( 182, 6 );
            // 
            // cMenuTrackSelectorCut
            // 
            this.cMenuTrackSelectorCut.Name = "cMenuTrackSelectorCut";
            this.cMenuTrackSelectorCut.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorCut.Text = "Cut(&T)";
            // 
            // cMenuTrackSelectorCopy
            // 
            this.cMenuTrackSelectorCopy.Name = "cMenuTrackSelectorCopy";
            this.cMenuTrackSelectorCopy.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorCopy.Text = "Copy(&C)";
            // 
            // cMenuTrackSelectorPaste
            // 
            this.cMenuTrackSelectorPaste.Name = "cMenuTrackSelectorPaste";
            this.cMenuTrackSelectorPaste.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorPaste.Text = "Paste(&P)";
            // 
            // cMenuTrackSelectorDelete
            // 
            this.cMenuTrackSelectorDelete.Name = "cMenuTrackSelectorDelete";
            this.cMenuTrackSelectorDelete.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorDelete.Text = "Delete(&D)";
            // 
            // cMenuTrackSelectorDeleteBezier
            // 
            this.cMenuTrackSelectorDeleteBezier.Name = "cMenuTrackSelectorDeleteBezier";
            this.cMenuTrackSelectorDeleteBezier.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorDeleteBezier.Text = "Delete Bezier Point(&B)";
            // 
            // toolStripMenuItem31
            // 
            this.toolStripMenuItem31.Name = "toolStripMenuItem31";
            this.toolStripMenuItem31.Size = new System.Drawing.Size( 182, 6 );
            // 
            // cMenuTrackSelectorSelectAll
            // 
            this.cMenuTrackSelectorSelectAll.Name = "cMenuTrackSelectorSelectAll";
            this.cMenuTrackSelectorSelectAll.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorSelectAll.Text = "Select All Events(&E)";
            // 
            // trackBar
            // 
            this.trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar.AutoSize = false;
            this.trackBar.Location = new System.Drawing.Point( 322, 266 );
            this.trackBar.Margin = new System.Windows.Forms.Padding( 0 );
            this.trackBar.Maximum = 609;
            this.trackBar.Minimum = 17;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size( 83, 16 );
            this.trackBar.TabIndex = 15;
            this.trackBar.TabStop = false;
            this.trackBar.TickFrequency = 100;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar.Value = 17;
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.pictKeyLengthSplitter );
            this.panel1.Controls.Add( this.panel3 );
            this.panel1.Controls.Add( this.vScroll );
            this.panel1.Controls.Add( this.hScroll );
            this.panel1.Controls.Add( this.picturePositionIndicator );
            this.panel1.Controls.Add( this.pictPianoRoll );
            this.panel1.Controls.Add( this.pictureBox3 );
            this.panel1.Controls.Add( this.trackBar );
            this.panel1.Controls.Add( this.pictureBox2 );
            this.panel1.Location = new System.Drawing.Point( 3, 3 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 421, 282 );
            this.panel1.TabIndex = 16;
            // 
            // pictKeyLengthSplitter
            // 
            this.pictKeyLengthSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictKeyLengthSplitter.BackColor = System.Drawing.SystemColors.Control;
            this.pictKeyLengthSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictKeyLengthSplitter.Cursor = System.Windows.Forms.Cursors.NoMoveHoriz;
            this.pictKeyLengthSplitter.Location = new System.Drawing.Point( 50, 266 );
            this.pictKeyLengthSplitter.Margin = new System.Windows.Forms.Padding( 0 );
            this.pictKeyLengthSplitter.Name = "pictKeyLengthSplitter";
            this.pictKeyLengthSplitter.Size = new System.Drawing.Size( 16, 16 );
            this.pictKeyLengthSplitter.TabIndex = 20;
            this.pictKeyLengthSplitter.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add( this.btnRight1 );
            this.panel3.Controls.Add( this.btnLeft2 );
            this.panel3.Controls.Add( this.btnZoom );
            this.panel3.Controls.Add( this.btnMooz );
            this.panel3.Controls.Add( this.btnLeft1 );
            this.panel3.Controls.Add( this.btnRight2 );
            this.panel3.Controls.Add( this.pictOverview );
            this.panel3.Location = new System.Drawing.Point( 0, 0 );
            this.panel3.Margin = new System.Windows.Forms.Padding( 0 );
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size( 421, 45 );
            this.panel3.TabIndex = 19;
            // 
            // btnRight1
            // 
            this.btnRight1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRight1.Location = new System.Drawing.Point( 52, 23 );
            this.btnRight1.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnRight1.Name = "btnRight1";
            this.btnRight1.Size = new System.Drawing.Size( 16, 22 );
            this.btnRight1.TabIndex = 24;
            this.btnRight1.Text = ">";
            this.btnRight1.UseVisualStyleBackColor = true;
            // 
            // btnLeft2
            // 
            this.btnLeft2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLeft2.Location = new System.Drawing.Point( 405, 0 );
            this.btnLeft2.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnLeft2.Name = "btnLeft2";
            this.btnLeft2.Size = new System.Drawing.Size( 16, 22 );
            this.btnLeft2.TabIndex = 23;
            this.btnLeft2.Text = "<";
            this.btnLeft2.UseVisualStyleBackColor = true;
            // 
            // btnZoom
            // 
            this.btnZoom.Location = new System.Drawing.Point( 26, 12 );
            this.btnZoom.Name = "btnZoom";
            this.btnZoom.Size = new System.Drawing.Size( 23, 23 );
            this.btnZoom.TabIndex = 22;
            this.btnZoom.Text = "+";
            this.btnZoom.UseVisualStyleBackColor = true;
            // 
            // btnMooz
            // 
            this.btnMooz.Location = new System.Drawing.Point( 3, 12 );
            this.btnMooz.Name = "btnMooz";
            this.btnMooz.Size = new System.Drawing.Size( 23, 23 );
            this.btnMooz.TabIndex = 21;
            this.btnMooz.Text = "-";
            this.btnMooz.UseVisualStyleBackColor = true;
            // 
            // btnLeft1
            // 
            this.btnLeft1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLeft1.Location = new System.Drawing.Point( 52, 0 );
            this.btnLeft1.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnLeft1.Name = "btnLeft1";
            this.btnLeft1.Size = new System.Drawing.Size( 16, 23 );
            this.btnLeft1.TabIndex = 20;
            this.btnLeft1.Text = "<";
            this.btnLeft1.UseVisualStyleBackColor = true;
            // 
            // btnRight2
            // 
            this.btnRight2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRight2.Location = new System.Drawing.Point( 405, 22 );
            this.btnRight2.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnRight2.Name = "btnRight2";
            this.btnRight2.Size = new System.Drawing.Size( 16, 23 );
            this.btnRight2.TabIndex = 19;
            this.btnRight2.Text = ">";
            this.btnRight2.UseVisualStyleBackColor = true;
            // 
            // pictOverview
            // 
            this.pictOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictOverview.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(106)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))) );
            this.pictOverview.Location = new System.Drawing.Point( 68, 0 );
            this.pictOverview.Margin = new System.Windows.Forms.Padding( 0 );
            this.pictOverview.Name = "pictOverview";
            this.pictOverview.Size = new System.Drawing.Size( 337, 45 );
            this.pictOverview.TabIndex = 18;
            this.pictOverview.TabStop = false;
            // 
            // vScroll
            // 
            this.vScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScroll.LargeChange = 10;
            this.vScroll.Location = new System.Drawing.Point( 405, 93 );
            this.vScroll.Margin = new System.Windows.Forms.Padding( 0 );
            this.vScroll.Maximum = 100;
            this.vScroll.Minimum = 0;
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new System.Drawing.Size( 16, 173 );
            this.vScroll.SmallChange = 1;
            this.vScroll.TabIndex = 17;
            this.vScroll.Value = 0;
            // 
            // hScroll
            // 
            this.hScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScroll.LargeChange = 10;
            this.hScroll.Location = new System.Drawing.Point( 66, 266 );
            this.hScroll.Margin = new System.Windows.Forms.Padding( 0 );
            this.hScroll.Maximum = 100;
            this.hScroll.Minimum = 0;
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size( 256, 16 );
            this.hScroll.SmallChange = 1;
            this.hScroll.TabIndex = 16;
            this.hScroll.Value = 0;
            // 
            // picturePositionIndicator
            // 
            this.picturePositionIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picturePositionIndicator.BackColor = System.Drawing.Color.DarkGray;
            this.picturePositionIndicator.Location = new System.Drawing.Point( 0, 45 );
            this.picturePositionIndicator.Margin = new System.Windows.Forms.Padding( 0 );
            this.picturePositionIndicator.Name = "picturePositionIndicator";
            this.picturePositionIndicator.Size = new System.Drawing.Size( 421, 48 );
            this.picturePositionIndicator.TabIndex = 10;
            this.picturePositionIndicator.TabStop = false;
            // 
            // pictPianoRoll
            // 
            this.pictPianoRoll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictPianoRoll.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))) );
            this.pictPianoRoll.Location = new System.Drawing.Point( 0, 93 );
            this.pictPianoRoll.Margin = new System.Windows.Forms.Padding( 0 );
            this.pictPianoRoll.MinimumSize = new System.Drawing.Size( 0, 100 );
            this.pictPianoRoll.Name = "pictPianoRoll";
            this.pictPianoRoll.Size = new System.Drawing.Size( 405, 173 );
            this.pictPianoRoll.TabIndex = 12;
            this.pictPianoRoll.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox3.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox3.Location = new System.Drawing.Point( 0, 266 );
            this.pictureBox3.Margin = new System.Windows.Forms.Padding( 0 );
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size( 49, 16 );
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox2.Location = new System.Drawing.Point( 405, 266 );
            this.pictureBox2.Margin = new System.Windows.Forms.Padding( 0 );
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size( 16, 16 );
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // toolStripTool
            // 
            this.toolStripTool.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripTool.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stripBtnPointer,
            this.stripBtnPencil,
            this.stripBtnLine,
            this.stripBtnEraser,
            this.toolStripSeparator5,
            this.stripBtnGrid,
            this.stripBtnCurve} );
            this.toolStripTool.Location = new System.Drawing.Point( 3, 75 );
            this.toolStripTool.Name = "toolStripTool";
            this.toolStripTool.Size = new System.Drawing.Size( 244, 25 );
            this.toolStripTool.TabIndex = 17;
            this.toolStripTool.Text = "toolStrip1";
            // 
            // stripBtnPointer
            // 
            this.stripBtnPointer.setSelected( true );
            this.stripBtnPointer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stripBtnPointer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.stripBtnPointer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnPointer.Name = "stripBtnPointer";
            this.stripBtnPointer.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.stripBtnPointer.Size = new System.Drawing.Size( 45, 22 );
            this.stripBtnPointer.Text = "Pointer";
            // 
            // stripBtnPencil
            // 
            this.stripBtnPencil.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnPencil.Name = "stripBtnPencil";
            this.stripBtnPencil.Size = new System.Drawing.Size( 40, 22 );
            this.stripBtnPencil.Text = "Pencil";
            // 
            // stripBtnLine
            // 
            this.stripBtnLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnLine.Name = "stripBtnLine";
            this.stripBtnLine.Size = new System.Drawing.Size( 30, 22 );
            this.stripBtnLine.Text = "Line";
            // 
            // stripBtnEraser
            // 
            this.stripBtnEraser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnEraser.Name = "stripBtnEraser";
            this.stripBtnEraser.Size = new System.Drawing.Size( 42, 22 );
            this.stripBtnEraser.Text = "Eraser";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripBtnGrid
            // 
            this.stripBtnGrid.CheckOnClick = true;
            this.stripBtnGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnGrid.Name = "stripBtnGrid";
            this.stripBtnGrid.Size = new System.Drawing.Size( 30, 22 );
            this.stripBtnGrid.Text = "Grid";
            // 
            // stripBtnCurve
            // 
            this.stripBtnCurve.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnCurve.Name = "stripBtnCurve";
            this.stripBtnCurve.Size = new System.Drawing.Size( 39, 22 );
            this.stripBtnCurve.Text = "Curve";
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Controls.Add( this.toolStripBottom );
            this.toolStripContainer.BottomToolStripPanel.Controls.Add( this.statusStrip1 );
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.AutoScroll = true;
            this.toolStripContainer.ContentPanel.Controls.Add( this.splitContainerProperty );
            this.toolStripContainer.ContentPanel.Controls.Add( this.panel2 );
            this.toolStripContainer.ContentPanel.Controls.Add( this.splitContainer2 );
            this.toolStripContainer.ContentPanel.Controls.Add( this.splitContainer1 );
            this.toolStripContainer.ContentPanel.Controls.Add( this.panel1 );
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size( 960, 518 );
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.LeftToolStripPanelVisible = false;
            this.toolStripContainer.Location = new System.Drawing.Point( 0, 24 );
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.RightToolStripPanelVisible = false;
            this.toolStripContainer.Size = new System.Drawing.Size( 960, 665 );
            this.toolStripContainer.TabIndex = 18;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add( this.toolStripFile );
            this.toolStripContainer.TopToolStripPanel.Controls.Add( this.toolStripPosition );
            this.toolStripContainer.TopToolStripPanel.Controls.Add( this.toolStripMeasure );
            this.toolStripContainer.TopToolStripPanel.Controls.Add( this.toolStripTool );
            this.toolStripContainer.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // toolStripBottom
            // 
            this.toolStripBottom.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripBottom.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel6,
            this.stripLblCursor,
            this.toolStripSeparator8,
            this.toolStripLabel8,
            this.stripLblTempo,
            this.toolStripSeparator9,
            this.toolStripLabel10,
            this.stripLblBeat,
            this.toolStripSeparator4,
            this.toolStripStatusLabel1,
            this.stripLblGameCtrlMode,
            this.toolStripSeparator10,
            this.toolStripStatusLabel2,
            this.stripLblMidiIn,
            this.toolStripSeparator11,
            this.stripDDBtnSpeed} );
            this.toolStripBottom.Location = new System.Drawing.Point( 5, 0 );
            this.toolStripBottom.Name = "toolStripBottom";
            this.toolStripBottom.Size = new System.Drawing.Size( 664, 25 );
            this.toolStripBottom.TabIndex = 22;
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size( 52, 22 );
            this.toolStripLabel6.Text = "CURSOR";
            // 
            // stripLblCursor
            // 
            this.stripLblCursor.AutoSize = false;
            this.stripLblCursor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripLblCursor.Font = new System.Drawing.Font( "MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)) );
            this.stripLblCursor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripLblCursor.Name = "stripLblCursor";
            this.stripLblCursor.Size = new System.Drawing.Size( 90, 22 );
            this.stripLblCursor.Text = "0 : 0 : 000";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size( 43, 22 );
            this.toolStripLabel8.Text = "TEMPO";
            // 
            // stripLblTempo
            // 
            this.stripLblTempo.AutoSize = false;
            this.stripLblTempo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripLblTempo.Font = new System.Drawing.Font( "MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)) );
            this.stripLblTempo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripLblTempo.Name = "stripLblTempo";
            this.stripLblTempo.Size = new System.Drawing.Size( 60, 22 );
            this.stripLblTempo.Text = "120.00";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripLabel10
            // 
            this.toolStripLabel10.Name = "toolStripLabel10";
            this.toolStripLabel10.Size = new System.Drawing.Size( 35, 22 );
            this.toolStripLabel10.Text = "BEAT";
            // 
            // stripLblBeat
            // 
            this.stripLblBeat.AutoSize = false;
            this.stripLblBeat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripLblBeat.Font = new System.Drawing.Font( "MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)) );
            this.stripLblBeat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripLblBeat.Name = "stripLblBeat";
            this.stripLblBeat.Size = new System.Drawing.Size( 45, 22 );
            this.stripLblBeat.Text = "4/4";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size( 85, 20 );
            this.toolStripStatusLabel1.Text = "Game Controler";
            // 
            // stripLblGameCtrlMode
            // 
            this.stripLblGameCtrlMode.Name = "stripLblGameCtrlMode";
            this.stripLblGameCtrlMode.Size = new System.Drawing.Size( 49, 20 );
            this.stripLblGameCtrlMode.Text = "Disabled";
            this.stripLblGameCtrlMode.ToolTipText = "Game Controler";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size( 41, 20 );
            this.toolStripStatusLabel2.Text = "MIDI In";
            // 
            // stripLblMidiIn
            // 
            this.stripLblMidiIn.Name = "stripLblMidiIn";
            this.stripLblMidiIn.Size = new System.Drawing.Size( 49, 20 );
            this.stripLblMidiIn.Text = "Disabled";
            this.stripLblMidiIn.ToolTipText = "Midi In Device";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripDDBtnSpeed
            // 
            this.stripDDBtnSpeed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripDDBtnSpeed.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stripDDBtnSpeedTextbox,
            this.stripDDBtnSpeed033,
            this.stripDDBtnSpeed050,
            this.stripDDBtnSpeed100} );
            this.stripDDBtnSpeed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripDDBtnSpeed.Name = "stripDDBtnSpeed";
            this.stripDDBtnSpeed.Size = new System.Drawing.Size( 73, 22 );
            this.stripDDBtnSpeed.Text = "Speed 1.0x";
            // 
            // stripDDBtnSpeedTextbox
            // 
            this.stripDDBtnSpeedTextbox.Name = "stripDDBtnSpeedTextbox";
            this.stripDDBtnSpeedTextbox.Size = new System.Drawing.Size( 100, 19 );
            this.stripDDBtnSpeedTextbox.Text = "100";
            // 
            // stripDDBtnSpeed033
            // 
            this.stripDDBtnSpeed033.Name = "stripDDBtnSpeed033";
            this.stripDDBtnSpeed033.Size = new System.Drawing.Size( 160, 22 );
            this.stripDDBtnSpeed033.Text = "33.3%";
            // 
            // stripDDBtnSpeed050
            // 
            this.stripDDBtnSpeed050.Name = "stripDDBtnSpeed050";
            this.stripDDBtnSpeed050.Size = new System.Drawing.Size( 160, 22 );
            this.stripDDBtnSpeed050.Text = "50%";
            // 
            // stripDDBtnSpeed100
            // 
            this.stripDDBtnSpeed100.Name = "stripDDBtnSpeed100";
            this.stripDDBtnSpeed100.Size = new System.Drawing.Size( 160, 22 );
            this.stripDDBtnSpeed100.Text = "100%";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel} );
            this.statusStrip1.Location = new System.Drawing.Point( 0, 25 );
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size( 960, 22 );
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size( 0, 17 );
            // 
            // splitContainerProperty
            // 
            this.splitContainerProperty.FixedPanel = System.Windows.Forms.FixedPanel.None;
            this.splitContainerProperty.IsSplitterFixed = false;
            this.splitContainerProperty.Location = new System.Drawing.Point( 714, 17 );
            this.splitContainerProperty.Name = "splitContainerProperty";
            this.splitContainerProperty.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // 
            // 
            this.splitContainerProperty.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerProperty.Panel1.BorderColor = System.Drawing.Color.Black;
            this.splitContainerProperty.Panel1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainerProperty.Panel1.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 4 );
            this.splitContainerProperty.Panel1.Name = "m_panel1";
            this.splitContainerProperty.Panel1.Size = new System.Drawing.Size( 220, 348 );
            this.splitContainerProperty.Panel1.TabIndex = 0;
            this.splitContainerProperty.Panel1MinSize = 25;
            // 
            // 
            // 
            this.splitContainerProperty.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerProperty.Panel2.BorderColor = System.Drawing.Color.Black;
            this.splitContainerProperty.Panel2.Location = new System.Drawing.Point( 224, 0 );
            this.splitContainerProperty.Panel2.Margin = new System.Windows.Forms.Padding( 0 );
            this.splitContainerProperty.Panel2.Name = "m_panel2";
            this.splitContainerProperty.Panel2.Size = new System.Drawing.Size( 217, 348 );
            this.splitContainerProperty.Panel2.TabIndex = 1;
            this.splitContainerProperty.Panel2MinSize = 25;
            this.splitContainerProperty.Size = new System.Drawing.Size( 441, 348 );
            this.splitContainerProperty.SplitterDistance = 220;
            this.splitContainerProperty.SplitterWidth = 4;
            this.splitContainerProperty.TabIndex = 20;
            this.splitContainerProperty.Text = "bSplitContainer1";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkGray;
            this.panel2.Controls.Add( this.waveView );
            this.panel2.Location = new System.Drawing.Point( 3, 291 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 421, 59 );
            this.panel2.TabIndex = 19;
            // 
            // waveView
            // 
            this.waveView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.waveView.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))) );
            this.waveView.Location = new System.Drawing.Point( 66, 0 );
            this.waveView.Margin = new System.Windows.Forms.Padding( 0 );
            this.waveView.Name = "waveView";
            this.waveView.Size = new System.Drawing.Size( 355, 59 );
            this.waveView.TabIndex = 17;
            // 
            // splitContainer2
            // 
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = false;
            this.splitContainer2.Location = new System.Drawing.Point( 593, 17 );
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // 
            // 
            this.splitContainer2.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Panel1.BorderColor = System.Drawing.Color.Black;
            this.splitContainer2.Panel1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer2.Panel1.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 4 );
            this.splitContainer2.Panel1.Name = "m_panel1";
            this.splitContainer2.Panel1.Size = new System.Drawing.Size( 115, 53 );
            this.splitContainer2.Panel1.TabIndex = 0;
            this.splitContainer2.Panel1MinSize = 25;
            // 
            // 
            // 
            this.splitContainer2.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Panel2.BorderColor = System.Drawing.Color.Black;
            this.splitContainer2.Panel2.Location = new System.Drawing.Point( 0, 57 );
            this.splitContainer2.Panel2.Margin = new System.Windows.Forms.Padding( 0 );
            this.splitContainer2.Panel2.Name = "m_panel2";
            this.splitContainer2.Panel2.Size = new System.Drawing.Size( 115, 185 );
            this.splitContainer2.Panel2.TabIndex = 1;
            this.splitContainer2.Panel2MinSize = 25;
            this.splitContainer2.Size = new System.Drawing.Size( 115, 242 );
            this.splitContainer2.SplitterDistance = 53;
            this.splitContainer2.SplitterWidth = 4;
            this.splitContainer2.TabIndex = 18;
            this.splitContainer2.Text = "bSplitContainer1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = false;
            this.splitContainer1.Location = new System.Drawing.Point( 440, 17 );
            this.splitContainer1.MinimumSize = new System.Drawing.Size( 0, 54 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // 
            // 
            this.splitContainer1.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Panel1.BorderColor = System.Drawing.Color.Black;
            this.splitContainer1.Panel1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Panel1.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 4 );
            this.splitContainer1.Panel1.Name = "m_panel1";
            this.splitContainer1.Panel1.Size = new System.Drawing.Size( 138, 27 );
            this.splitContainer1.Panel1.TabIndex = 0;
            this.splitContainer1.Panel1MinSize = 25;
            // 
            // 
            // 
            this.splitContainer1.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Panel2.BorderColor = System.Drawing.Color.Black;
            this.splitContainer1.Panel2.Location = new System.Drawing.Point( 0, 31 );
            this.splitContainer1.Panel2.Margin = new System.Windows.Forms.Padding( 0 );
            this.splitContainer1.Panel2.Name = "m_panel2";
            this.splitContainer1.Panel2.Size = new System.Drawing.Size( 138, 211 );
            this.splitContainer1.Panel2.TabIndex = 1;
            this.splitContainer1.Panel2MinSize = 25;
            this.splitContainer1.Size = new System.Drawing.Size( 138, 242 );
            this.splitContainer1.SplitterDistance = 27;
            this.splitContainer1.SplitterWidth = 4;
            this.splitContainer1.TabIndex = 4;
            this.splitContainer1.Text = "splitContainerEx1";
            // 
            // toolStripFile
            // 
            this.toolStripFile.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripFile.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stripBtnFileNew,
            this.stripBtnFileOpen,
            this.stripBtnFileSave,
            this.toolStripSeparator12,
            this.stripBtnCut,
            this.stripBtnCopy,
            this.stripBtnPaste,
            this.toolStripSeparator13,
            this.stripBtnUndo,
            this.stripBtnRedo} );
            this.toolStripFile.Location = new System.Drawing.Point( 3, 0 );
            this.toolStripFile.Name = "toolStripFile";
            this.toolStripFile.Size = new System.Drawing.Size( 208, 25 );
            this.toolStripFile.TabIndex = 20;
            // 
            // stripBtnFileNew
            // 
            this.stripBtnFileNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnFileNew.Name = "stripBtnFileNew";
            this.stripBtnFileNew.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnFileNew.Text = "toolStripButton6";
            this.stripBtnFileNew.ToolTipText = "New";
            // 
            // stripBtnFileOpen
            // 
            this.stripBtnFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnFileOpen.Name = "stripBtnFileOpen";
            this.stripBtnFileOpen.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnFileOpen.Text = "toolStripButton3";
            this.stripBtnFileOpen.ToolTipText = "Open";
            // 
            // stripBtnFileSave
            // 
            this.stripBtnFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnFileSave.Name = "stripBtnFileSave";
            this.stripBtnFileSave.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnFileSave.Text = "toolStripButton2";
            this.stripBtnFileSave.ToolTipText = "Save";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripBtnCut
            // 
            this.stripBtnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnCut.Name = "stripBtnCut";
            this.stripBtnCut.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnCut.Text = "toolStripButton4";
            this.stripBtnCut.ToolTipText = "Cut";
            // 
            // stripBtnCopy
            // 
            this.stripBtnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnCopy.Name = "stripBtnCopy";
            this.stripBtnCopy.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnCopy.Text = "toolStripButton5";
            this.stripBtnCopy.ToolTipText = "Copy";
            // 
            // stripBtnPaste
            // 
            this.stripBtnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnPaste.Name = "stripBtnPaste";
            this.stripBtnPaste.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnPaste.Text = "toolStripLabel1";
            this.stripBtnPaste.ToolTipText = "Paste";
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripBtnUndo
            // 
            this.stripBtnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnUndo.Name = "stripBtnUndo";
            this.stripBtnUndo.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnUndo.Text = "toolStripButton7";
            this.stripBtnUndo.ToolTipText = "Undo";
            // 
            // stripBtnRedo
            // 
            this.stripBtnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnRedo.Name = "stripBtnRedo";
            this.stripBtnRedo.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnRedo.Text = "toolStripButton8";
            this.stripBtnRedo.ToolTipText = "Redo";
            // 
            // toolStripPosition
            // 
            this.toolStripPosition.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripPosition.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stripBtnMoveTop,
            this.stripBtnRewind,
            this.stripBtnForward,
            this.stripBtnMoveEnd,
            this.stripBtnPlay,
            this.stripBtnStop,
            this.toolStripSeparator7,
            this.stripBtnScroll,
            this.stripBtnLoop} );
            this.toolStripPosition.Location = new System.Drawing.Point( 3, 25 );
            this.toolStripPosition.Name = "toolStripPosition";
            this.toolStripPosition.Size = new System.Drawing.Size( 202, 25 );
            this.toolStripPosition.TabIndex = 18;
            // 
            // stripBtnMoveTop
            // 
            this.stripBtnMoveTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnMoveTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnMoveTop.Name = "stripBtnMoveTop";
            this.stripBtnMoveTop.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnMoveTop.Text = "  <=|  ";
            this.stripBtnMoveTop.ToolTipText = "MoveTop";
            // 
            // stripBtnRewind
            // 
            this.stripBtnRewind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnRewind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnRewind.Name = "stripBtnRewind";
            this.stripBtnRewind.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnRewind.Text = "  <<  ";
            this.stripBtnRewind.ToolTipText = "Rewind";
            // 
            // stripBtnForward
            // 
            this.stripBtnForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnForward.Name = "stripBtnForward";
            this.stripBtnForward.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnForward.Text = "  >>  ";
            this.stripBtnForward.ToolTipText = "Forward";
            // 
            // stripBtnMoveEnd
            // 
            this.stripBtnMoveEnd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnMoveEnd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnMoveEnd.Name = "stripBtnMoveEnd";
            this.stripBtnMoveEnd.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnMoveEnd.Text = "  |=>  ";
            this.stripBtnMoveEnd.ToolTipText = "MoveEnd";
            // 
            // stripBtnPlay
            // 
            this.stripBtnPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnPlay.Name = "stripBtnPlay";
            this.stripBtnPlay.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnPlay.Text = "  =>  ";
            this.stripBtnPlay.ToolTipText = "Play";
            // 
            // stripBtnStop
            // 
            this.stripBtnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnStop.Name = "stripBtnStop";
            this.stripBtnStop.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnStop.Text = "   ||   ";
            this.stripBtnStop.ToolTipText = "Stop";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripBtnScroll
            // 
            this.stripBtnScroll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnScroll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnScroll.Name = "stripBtnScroll";
            this.stripBtnScroll.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnScroll.Text = "Scroll";
            // 
            // stripBtnLoop
            // 
            this.stripBtnLoop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnLoop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnLoop.Name = "stripBtnLoop";
            this.stripBtnLoop.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnLoop.Text = "Loop";
            // 
            // toolStripMeasure
            // 
            this.toolStripMeasure.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripMeasure.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel5,
            this.stripLblMeasure,
            this.toolStripButton1,
            this.stripDDBtnLength,
            this.stripDDBtnQuantize,
            this.toolStripSeparator6,
            this.stripBtnStartMarker,
            this.stripBtnEndMarker} );
            this.toolStripMeasure.Location = new System.Drawing.Point( 3, 50 );
            this.toolStripMeasure.Name = "toolStripMeasure";
            this.toolStripMeasure.Size = new System.Drawing.Size( 409, 25 );
            this.toolStripMeasure.TabIndex = 19;
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size( 59, 22 );
            this.toolStripLabel5.Text = "MEASURE";
            // 
            // stripLblMeasure
            // 
            this.stripLblMeasure.Font = new System.Drawing.Font( "MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)) );
            this.stripLblMeasure.Name = "stripLblMeasure";
            this.stripLblMeasure.Size = new System.Drawing.Size( 75, 22 );
            this.stripLblMeasure.Text = "0 : 0 : 000";
            this.stripLblMeasure.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripDDBtnLength
            // 
            this.stripDDBtnLength.AutoSize = false;
            this.stripDDBtnLength.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripDDBtnLength.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stripDDBtnLength04,
            this.stripDDBtnLength08,
            this.stripDDBtnLength16,
            this.stripDDBtnLength32,
            this.stripDDBtnLength64,
            this.stripDDBtnLength128,
            this.stripDDBtnLengthOff,
            this.toolStripSeparator2,
            this.stripDDBtnLengthTriplet} );
            this.stripDDBtnLength.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripDDBtnLength.Name = "stripDDBtnLength";
            this.stripDDBtnLength.Size = new System.Drawing.Size( 95, 22 );
            this.stripDDBtnLength.Text = "LENGTH  1/64";
            this.stripDDBtnLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stripDDBtnLength04
            // 
            this.stripDDBtnLength04.Name = "stripDDBtnLength04";
            this.stripDDBtnLength04.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLength04.Text = "1/4";
            // 
            // stripDDBtnLength08
            // 
            this.stripDDBtnLength08.Name = "stripDDBtnLength08";
            this.stripDDBtnLength08.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLength08.Text = "1/8";
            // 
            // stripDDBtnLength16
            // 
            this.stripDDBtnLength16.Name = "stripDDBtnLength16";
            this.stripDDBtnLength16.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLength16.Text = "1/16";
            // 
            // stripDDBtnLength32
            // 
            this.stripDDBtnLength32.Name = "stripDDBtnLength32";
            this.stripDDBtnLength32.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLength32.Text = "1/32";
            // 
            // stripDDBtnLength64
            // 
            this.stripDDBtnLength64.Name = "stripDDBtnLength64";
            this.stripDDBtnLength64.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLength64.Text = "1/64";
            // 
            // stripDDBtnLength128
            // 
            this.stripDDBtnLength128.Name = "stripDDBtnLength128";
            this.stripDDBtnLength128.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLength128.Text = "1/128";
            // 
            // stripDDBtnLengthOff
            // 
            this.stripDDBtnLengthOff.Name = "stripDDBtnLengthOff";
            this.stripDDBtnLengthOff.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLengthOff.Text = "Off";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size( 100, 6 );
            // 
            // stripDDBtnLengthTriplet
            // 
            this.stripDDBtnLengthTriplet.Name = "stripDDBtnLengthTriplet";
            this.stripDDBtnLengthTriplet.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLengthTriplet.Text = "Triplet";
            // 
            // stripDDBtnQuantize
            // 
            this.stripDDBtnQuantize.AutoSize = false;
            this.stripDDBtnQuantize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripDDBtnQuantize.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stripDDBtnQuantize04,
            this.stripDDBtnQuantize08,
            this.stripDDBtnQuantize16,
            this.stripDDBtnQuantize32,
            this.stripDDBtnQuantize64,
            this.stripDDBtnQuantize128,
            this.stripDDBtnQuantizeOff,
            this.toolStripSeparator3,
            this.stripDDBtnQuantizeTriplet} );
            this.stripDDBtnQuantize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripDDBtnQuantize.Name = "stripDDBtnQuantize";
            this.stripDDBtnQuantize.Size = new System.Drawing.Size( 110, 22 );
            this.stripDDBtnQuantize.Text = "QUANTIZE  1/64";
            this.stripDDBtnQuantize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stripDDBtnQuantize04
            // 
            this.stripDDBtnQuantize04.Name = "stripDDBtnQuantize04";
            this.stripDDBtnQuantize04.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantize04.Text = "1/4";
            // 
            // stripDDBtnQuantize08
            // 
            this.stripDDBtnQuantize08.Name = "stripDDBtnQuantize08";
            this.stripDDBtnQuantize08.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantize08.Text = "1/8";
            // 
            // stripDDBtnQuantize16
            // 
            this.stripDDBtnQuantize16.Name = "stripDDBtnQuantize16";
            this.stripDDBtnQuantize16.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantize16.Text = "1/16";
            // 
            // stripDDBtnQuantize32
            // 
            this.stripDDBtnQuantize32.Name = "stripDDBtnQuantize32";
            this.stripDDBtnQuantize32.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantize32.Text = "1/32";
            // 
            // stripDDBtnQuantize64
            // 
            this.stripDDBtnQuantize64.Name = "stripDDBtnQuantize64";
            this.stripDDBtnQuantize64.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantize64.Text = "1/64";
            // 
            // stripDDBtnQuantize128
            // 
            this.stripDDBtnQuantize128.Name = "stripDDBtnQuantize128";
            this.stripDDBtnQuantize128.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantize128.Text = "1/128";
            // 
            // stripDDBtnQuantizeOff
            // 
            this.stripDDBtnQuantizeOff.Name = "stripDDBtnQuantizeOff";
            this.stripDDBtnQuantizeOff.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantizeOff.Text = "Off";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size( 100, 6 );
            // 
            // stripDDBtnQuantizeTriplet
            // 
            this.stripDDBtnQuantizeTriplet.Name = "stripDDBtnQuantizeTriplet";
            this.stripDDBtnQuantizeTriplet.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantizeTriplet.Text = "Triplet";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripBtnStartMarker
            // 
            this.stripBtnStartMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnStartMarker.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnStartMarker.Name = "stripBtnStartMarker";
            this.stripBtnStartMarker.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnStartMarker.Text = "StartMarker";
            // 
            // stripBtnEndMarker
            // 
            this.stripBtnEndMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnEndMarker.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnEndMarker.Name = "stripBtnEndMarker";
            this.stripBtnEndMarker.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnEndMarker.Text = "EndMarker";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size( 960, 689 );
            this.Controls.Add( this.toolStripContainer );
            this.Controls.Add( this.menuStripMain );
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FormMain";
            this.Text = "Cadencii";
            this.menuStripMain.ResumeLayout( false );
            this.menuStripMain.PerformLayout();
            this.cMenuPiano.ResumeLayout( false );
            this.cMenuTrackTab.ResumeLayout( false );
            this.cMenuTrackSelector.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.panel1.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.pictKeyLengthSplitter)).EndInit();
            this.panel3.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.pictOverview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePositionIndicator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictPianoRoll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.toolStripTool.ResumeLayout( false );
            this.toolStripTool.PerformLayout();
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout( false );
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout( false );
            this.toolStripContainer.TopToolStripPanel.ResumeLayout( false );
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout( false );
            this.toolStripContainer.PerformLayout();
            this.toolStripBottom.ResumeLayout( false );
            this.toolStripBottom.PerformLayout();
            this.statusStrip1.ResumeLayout( false );
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout( false );
            this.toolStripFile.ResumeLayout( false );
            this.toolStripFile.PerformLayout();
            this.toolStripPosition.ResumeLayout( false );
            this.toolStripPosition.PerformLayout();
            this.toolStripMeasure.ResumeLayout( false );
            this.toolStripMeasure.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private BMenuBar menuStripMain;
        private BMenuItem menuFile;
        private BMenuItem menuEdit;
        private BMenuItem menuVisual;
        private BMenuItem menuJob;
        private BMenuItem menuTrack;
        private BMenuItem menuLyric;
        private BMenuItem menuSetting;
        private BMenuItem menuHelp;
        private BMenuItem menuVisualControlTrack;
        private BMenuItem menuVisualMixer;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private BMenuItem menuVisualGridline;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private BMenuItem menuVisualStartMarker;
        private BMenuItem menuVisualEndMarker;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private BMenuItem menuVisualLyrics;
        private BMenuItem menuVisualNoteProperty;
        private BMenuItem menuSettingPreference;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private BMenuItem menuSettingDefaultSingerStyle;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private BMenuItem menuSettingPositionQuantize;
        private BMenuItem menuSettingPositionQuantize04;
        private BMenuItem menuSettingPositionQuantize08;
        private BMenuItem menuSettingPositionQuantize16;
        private BMenuItem menuSettingPositionQuantize32;
        private BMenuItem menuSettingPositionQuantize64;
        private BMenuItem menuSettingPositionQuantizeOff;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private BMenuItem menuSettingSingerProperty;
        private BMenuItem menuSettingPositionQuantizeTriplet;
        private BMenuItem menuSettingLengthQuantize;
        private BMenuItem menuSettingLengthQuantize04;
        private BMenuItem menuSettingLengthQuantize08;
        private BMenuItem menuSettingLengthQuantize16;
        private BMenuItem menuSettingLengthQuantize32;
        private BMenuItem menuSettingLengthQuantize64;
        private BMenuItem menuSettingLengthQuantizeOff;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private BMenuItem menuSettingLengthQuantizeTriplet;
        private BMenuItem menuFileNew;
        private BMenuItem menuFileOpen;
        private BMenuItem menuFileSave;
        private BMenuItem menuFileSaveNamed;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
        private BMenuItem menuFileImport;
        private BMenuItem menuFileExport;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
        private BMenuItem menuFileQuit;
        private BMenuItem menuEditUndo;
        private BMenuItem menuEditRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private BPictureBox pictureBox2;
        private BPictureBox pictureBox3;
        private BPictureBox picturePositionIndicator;
        private BPopupMenu cMenuPiano;
        private BMenuItem cMenuPianoPointer;
        private BMenuItem cMenuPianoPencil;
        private BMenuItem cMenuPianoEraser;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem13;
        private BMenuItem cMenuPianoFixed;
        private BMenuItem cMenuPianoQuantize;
        private BMenuItem cMenuPianoLength;
        private BMenuItem cMenuPianoGrid;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem14;
        private BMenuItem cMenuPianoUndo;
        private BMenuItem cMenuPianoRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem15;
        private BMenuItem cMenuPianoCut;
        private BMenuItem cMenuPianoFixed01;
        private BMenuItem cMenuPianoFixed02;
        private BMenuItem cMenuPianoFixed04;
        private BMenuItem cMenuPianoFixed08;
        private BMenuItem cMenuPianoFixed16;
        private BMenuItem cMenuPianoFixed32;
        private BMenuItem cMenuPianoFixed64;
        private BMenuItem cMenuPianoFixedOff;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem18;
        private BMenuItem cMenuPianoFixedTriplet;
        private BMenuItem cMenuPianoFixedDotted;
        private BMenuItem cMenuPianoCopy;
        private BMenuItem cMenuPianoPaste;
        private BMenuItem cMenuPianoDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem16;
        private BMenuItem cMenuPianoSelectAll;
        private BMenuItem cMenuPianoSelectAllEvents;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem17;
        private BMenuItem cMenuPianoImportLyric;
        private BMenuItem cMenuPianoExpressionProperty;
        private BMenuItem cMenuPianoQuantize04;
        private BMenuItem cMenuPianoQuantize08;
        private BMenuItem cMenuPianoQuantize16;
        private BMenuItem cMenuPianoQuantize32;
        private BMenuItem cMenuPianoQuantize64;
        private BMenuItem cMenuPianoQuantizeOff;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem26;
        private BMenuItem cMenuPianoQuantizeTriplet;
        private BMenuItem cMenuPianoLength04;
        private BMenuItem cMenuPianoLength08;
        private BMenuItem cMenuPianoLength16;
        private BMenuItem cMenuPianoLength32;
        private BMenuItem cMenuPianoLength64;
        private BMenuItem cMenuPianoLengthOff;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem32;
        private BMenuItem cMenuPianoLengthTriplet;
        private BMenuItem menuFileRecent;
        private System.Windows.Forms.ToolTip toolTip;
        private BMenuItem menuEditCut;
        private BMenuItem menuEditCopy;
        private BMenuItem menuEditPaste;
        private BMenuItem menuEditDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem19;
        private BMenuItem menuEditAutoNormalizeMode;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem20;
        private BMenuItem menuEditSelectAll;
        private BMenuItem menuEditSelectAllEvents;
        private BMenuItem menuTrackOn;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem21;
        private BMenuItem menuTrackAdd;
        private BMenuItem menuTrackCopy;
        private BMenuItem menuTrackChangeName;
        private BMenuItem menuTrackDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem22;
        private BMenuItem menuTrackRenderCurrent;
        private BMenuItem menuTrackRenderAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem23;
        private BMenuItem menuTrackOverlay;
        private BPopupMenu cMenuTrackTab;
        private BMenuItem cMenuTrackTabTrackOn;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem24;
        private BMenuItem cMenuTrackTabAdd;
        private BMenuItem cMenuTrackTabCopy;
        private BMenuItem cMenuTrackTabChangeName;
        private BMenuItem cMenuTrackTabDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem25;
        private BMenuItem cMenuTrackTabRenderCurrent;
        private BMenuItem cMenuTrackTabRenderAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem27;
        private BMenuItem cMenuTrackTabOverlay;
        private BPopupMenu cMenuTrackSelector;
        private BMenuItem cMenuTrackSelectorPointer;
        private BMenuItem cMenuTrackSelectorPencil;
        private BMenuItem cMenuTrackSelectorLine;
        private BMenuItem cMenuTrackSelectorEraser;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem28;
        private BMenuItem cMenuTrackSelectorUndo;
        private BMenuItem cMenuTrackSelectorRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem29;
        private BMenuItem cMenuTrackSelectorCut;
        private BMenuItem cMenuTrackSelectorCopy;
        private BMenuItem cMenuTrackSelectorPaste;
        private BMenuItem cMenuTrackSelectorDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem31;
        private BMenuItem cMenuTrackSelectorSelectAll;
        private BMenuItem menuJobNormalize;
        private BMenuItem menuJobInsertBar;
        private BMenuItem menuJobDeleteBar;
        private BMenuItem menuJobRandomize;
        private BMenuItem menuJobConnect;
        private BMenuItem menuJobLyric;
        private BMenuItem menuJobRewire;
        private BMenuItem menuLyricExpressionProperty;
        private BMenuItem menuLyricSymbol;
        private BMenuItem menuLyricDictionary;
        private BMenuItem menuHelpAbout;
        private BMenuItem menuHelpDebug;
        private BMenuItem menuFileExportWave;
        private BMenuItem menuFileExportMidi;
        private BMenuItem menuScript;
        private BMenuItem menuHidden;
        private BMenuItem menuHiddenEditLyric;
        private BMenuItem menuHiddenEditFlipToolPointerPencil;
        private BMenuItem menuHiddenEditFlipToolPointerEraser;
        private BMenuItem menuHiddenVisualForwardParameter;
        private BMenuItem menuHiddenVisualBackwardParameter;
        private BMenuItem menuHiddenTrackNext;
        private BMenuItem menuHiddenTrackBack;
        private BMenuItem menuJobReloadVsti;
        private BMenuItem cMenuPianoCurve;
        private BMenuItem cMenuTrackSelectorCurve;
        private BSlider trackBar;
        private BPanel panel1;
        private BToolBar toolStripTool;
        private BToolStripButton stripBtnPointer;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private BToolStripButton stripBtnLine;
        private BToolStripButton stripBtnPencil;
        private BToolStripButton stripBtnEraser;
        private BToolStripButton stripBtnGrid;
        private BToolBar toolStripPosition;
        private BToolStripButton stripBtnMoveTop;
        private BToolStripButton stripBtnRewind;
        private BToolStripButton stripBtnForward;
        private BToolStripButton stripBtnMoveEnd;
        private BToolStripButton stripBtnPlay;
        private BToolStripButton stripBtnStop;
        private BToolStripButton stripBtnScroll;
        private BToolStripButton stripBtnLoop;
        private BToolStripButton stripBtnCurve;
        private BToolBar toolStripMeasure;
        private BToolStripLabel stripLblMeasure;
        private System.Windows.Forms.ToolStripSeparator toolStripButton1;
        private BToolStripDropDownButton stripDDBtnLength;
        private BMenuItem stripDDBtnLength04;
        private BMenuItem stripDDBtnLength08;
        private BMenuItem stripDDBtnLength16;
        private BMenuItem stripDDBtnLength32;
        private BMenuItem stripDDBtnLength64;
        private BMenuItem stripDDBtnLengthOff;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private BMenuItem stripDDBtnLengthTriplet;
        private BToolStripDropDownButton stripDDBtnQuantize;
        private BMenuItem stripDDBtnQuantize04;
        private BMenuItem stripDDBtnQuantize08;
        private BMenuItem stripDDBtnQuantize16;
        private BMenuItem stripDDBtnQuantize32;
        private BMenuItem stripDDBtnQuantize64;
        private BMenuItem stripDDBtnQuantizeOff;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private BMenuItem stripDDBtnQuantizeTriplet;
        private BToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private BToolStripButton stripBtnStartMarker;
        private BToolStripButton stripBtnEndMarker;
        private BHScrollBar hScroll;
        private BVScrollBar vScroll;
        private BMenuItem menuLyricVibratoProperty;
        private BMenuItem cMenuPianoVibratoProperty;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private BToolStripLabel toolStripLabel6;
        private BToolStripLabel stripLblCursor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private BToolStripLabel toolStripLabel8;
        private BToolStripLabel stripLblTempo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private BToolStripLabel toolStripLabel10;
        private BToolStripLabel stripLblBeat;
        private BMenuItem menuScriptUpdate;
        private BMenuItem menuSettingGameControler;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private BStatusLabel stripLblGameCtrlMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private BToolStripDropDownButton stripDDBtnSpeed;
        private BMenuItem menuSettingGameControlerSetting;
        private BMenuItem menuSettingGameControlerLoad;
        private BMenuItem stripDDBtnLength128;
        private BMenuItem stripDDBtnQuantize128;
        private BMenuItem menuSettingPositionQuantize128;
        private BMenuItem menuSettingLengthQuantize128;
        private BMenuItem cMenuPianoQuantize128;
        private BMenuItem cMenuPianoLength128;
        private BMenuItem cMenuPianoFixed128;
        private WaveView waveView;
        private BMenuItem menuVisualWaveform;
        private Boare.Lib.AppUtil.BSplitContainer splitContainer2;
        private BPanel panel2;
        private BMenuItem cMenuTrackSelectorDeleteBezier;
        private BStatusLabel stripLblMidiIn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private BMenuItem menuJobRealTime;
        private BMenuItem cMenuTrackTabRenderer;
        private BMenuItem cMenuTrackTabRendererVOCALOID1;
        private BMenuItem cMenuTrackTabRendererVOCALOID2;
        private BMenuItem cMenuTrackTabRendererUtau;
        private BMenuItem menuVisualPitchLine;
        private BMenuItem menuFileImportMidi;
        private BToolBar toolStripFile;
        private BStatusLabel toolStripStatusLabel1;
        private BStatusLabel toolStripStatusLabel2;
        private BToolStripButton stripBtnFileSave;
        private BToolStripButton stripBtnFileOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private BToolStripButton stripBtnCut;
        private BToolStripButton stripBtnCopy;
        private BToolStripButton stripBtnPaste;
        private BToolStripButton stripBtnFileNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private BToolStripButton stripBtnUndo;
        private BToolStripButton stripBtnRedo;
        private BMenuItem cMenuTrackSelectorPaletteTool;
        private BMenuItem cMenuPianoPaletteTool;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private BMenuItem menuSettingPaletteTool;
        private BMenuItem menuTrackRenderer;
        private BMenuItem menuTrackRendererVOCALOID1;
        private BMenuItem menuTrackRendererVOCALOID2;
        private BMenuItem menuTrackRendererUtau;
        private BMenuItem menuFileImportVsq;
        private BMenuItem menuSettingShortcut;
        private BToolStripTextBox stripDDBtnSpeedTextbox;
        private BMenuItem stripDDBtnSpeed033;
        private BMenuItem stripDDBtnSpeed050;
        private BMenuItem stripDDBtnSpeed100;
        private BMenuItem menuSettingMidi;
        private BMenuItem menuVisualProperty;
        private BMenuItem menuFileOpenVsq;
        private BMenuItem menuFileOpenUst;
        private BMenuItem menuSettingGameControlerRemove;
        private BMenuItem menuHiddenCopy;
        private BMenuItem menuHiddenPaste;
        private BMenuItem menuHiddenCut;
        private BMenuItem menuSettingUtauVoiceDB;
        private BToolBar toolStripBottom;
        private BStatusLabel statusLabel;
        private Boare.Lib.AppUtil.BSplitContainer splitContainerProperty;
        private BPictureBox pictOverview;
        private BMenuItem menuVisualOverview;
        private BPanel panel3;
        private BButton btnLeft1;
        private BButton btnRight2;
        private BButton btnZoom;
        private BButton btnMooz;
        private BButton btnLeft2;
        private BButton btnRight1;
        private Boare.Lib.AppUtil.BSplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private BMenuItem menuTrackBgm;
        private BMenuItem menuTrackRendererStraight;
        private BMenuItem menuTrackManager;
        private BMenuItem cMenuTrackTabRendererStraight;
        public PictPianoRoll pictPianoRoll;
        private TrackSelector trackSelector;
        #endregion
#endif

    }

#if !JAVA
}
#endif
