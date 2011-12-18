/*
 * FormBezierPointEditUiListener.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package com.github.cadencii;

//INCLUDE-SECTION IMPORT ./ui/java/FormBezierPointEdit.java

import java.awt.*;
import java.util.*;
import com.github.cadencii.*;
import com.github.cadencii.apputil.*;
import com.github.cadencii.windows.forms.*;
#else
using System;
using com.github.cadencii.apputil;
using com.github.cadencii;
using com.github.cadencii.java.awt;
using com.github.cadencii.java.util;
using com.github.cadencii.windows.forms;

namespace com.github.cadencii
{
#endif

#if JAVA
    public interface FormBezierPointEditUiListener
#else
    public interface FormBezierPointEditUiListener
#endif
    {
        [PureVirtualFunction]
        void buttonOkClick();

        [PureVirtualFunction]
        void buttonCancelClick();

        [PureVirtualFunction]
        void buttonBackwardClick();

        [PureVirtualFunction]
        void buttonForwardClick();

        [PureVirtualFunction]
        void checkboxEnableSmoothCheckedChanged();

        [PureVirtualFunction]
        void buttonsMouseDown( int buttonType );

        [PureVirtualFunction]
        void buttonsMouseUp();

        [PureVirtualFunction]
        void buttonsMouseMove();
    }

#if !JAVA
}
#endif