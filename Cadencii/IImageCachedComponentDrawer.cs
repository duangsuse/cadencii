﻿/*
 * IImageCachedComponentDrawer.cs
 * Copyright (C) 2010 kbinani
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
using org.kbinani.java.awt;

namespace org.kbinani.cadencii {

    public interface IImageCachedComponentDrawer {
        void draw( Graphics2D g, int width, int height );
    }

}