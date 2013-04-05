/*
 * CadenciiHelper.cpp
 * Copyright © 2011 kbinani
 *
 * This file is part of CadenciiHelper.
 *
 * CadenciiHelper is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * CadenciiHelper is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#include "stdafx.h"
#include "CadenciiHelper.h"

#ifdef _DEBUG
int main()
{
//    org::kbinani::cadencii::helper::Utils::MountPointCreate( "E:\\temp\\link", "E:\\Configurações" );
    unsigned int ret = com::github::cadencii::helper::Utils::MountPointCreate( TEXT("E:\\temp\\link"), TEXT("E:\\msys") );
    printf( "main; ret=0x%X\n", ret );
    getchar();
}

#endif
