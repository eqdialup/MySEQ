
/*==============================================================================

	Copyright (C) 2006-2013  All developers at http://sourceforge.net/projects/seq

	This program is free software; you can redistribute it and/or
	modify it under the terms of the GNU General Public License
	as published by the Free Software Foundation; either version 2
	of the License, or (at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

  ==============================================================================*/

#pragma once

#include "resource.h"

#include "Common.h"

#include "IniReader.h"

#include "MemReader.h"

#include "NetworkServer.h"

#include "EQGameScanner.h"

#include "Debugger.h"



using namespace std;


bool debug_mode;
bool console_mode;
bool services;

bool otherini;

IniReader iniReader;

char iniFile[_MAX_PATH+1];
char configIniFile[_MAX_PATH+1];

int server_status;
int check_delay;

void ReadArgs(int argc, char* argv[]);

Debugger debugger;

MemReader memReader;

NetworkServer netServer;

EQGameScanner scanner;

void Minimize();
void Restore();
void InitNotifyIconData();
void ToggleStartMinimized();

