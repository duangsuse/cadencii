/*
 * BTimer.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.windows.forms.
 *
 * cadencii.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ./BTimer.java
#else
using System;

namespace cadencii.windows.forms {

    public class BTimer : System.Windows.Forms.Timer {
        public void start() {
            base.Start();
        }

        public BTimer()
            : base() {
        }

        public BTimer( System.ComponentModel.IContainer container )
            : base( container ) {
        }

        public void stop() {
            base.Stop();
        }

        public int getDelay() {
            return base.Interval;
        }

        public void setDelay( int value ) {
            base.Interval = value;
        }

        public bool isRunning() {
            return base.Enabled;
        }
    }

}
#endif
