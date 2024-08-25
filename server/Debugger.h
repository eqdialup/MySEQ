/*==============================================================================

	Copyright (C) 2006-2024  All developers at https://www.showeq.net/forums/forum.php

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

#include "Common.h"
#include "MemReader.h"
#include "Spawn.h"
#include "Item.h"
#include "World.h"

class Debugger
{
public:
	enum offset_types { OT_zonename, OT_spawnlist, OT_self, OT_target, OT_ground, OT_world, OT_max };

private:
	Spawn spawnParser;

	Item itemParser;

	World worldParser;

	QWORD offsets[OT_max]{};

	string ptrNames[OT_max];

	void init(IniReaderInterface* ir_intf);

	void displayCurrentOffsets();

	bool setOffset(offset_types ot, QWORD value);

	void setOffset(offset_types ot, QWORD value, string ptrName);

	void setOffset(bool primary, string userInput);

	void printMenu();

	void examineRawMemory(MemReaderInterface* mr_intf, offset_types ot);

	void processSpawn(MemReaderInterface* mr_intf, offset_types ot);

	void walkSpawnList(MemReaderInterface* mr_intf, offset_types ot, bool reverse);

	void scanForPtr(MemReaderInterface* mr_intf, QWORD pSearch, QWORD pStart, QWORD size);

	void scanForString(MemReaderInterface* mr_intf, offset_types ot, QWORD size, string searchStr);

	void scanForWorldFromDate(MemReaderInterface* mr_intf, offset_types ot, QWORD size, string args);

	void scanForUINT(MemReaderInterface* mr_intf, QWORD pStart, QWORD size, UINT length, string args);

	void showProcesses(MemReaderInterface* mr_intf, string processName);

	void scanForFloatFromTarget(MemReaderInterface* mr_intf, string args);

	void scanForFloatFromSelf(MemReaderInterface* mr_intf, string args);

	void scanForUINTFromSelf(MemReaderInterface* mr_intf, QWORD size, string args);

	void scanForBYTEFromTarget(MemReaderInterface* mr_intf, QWORD size, string args);

	void scanForBYTEFromSelf(MemReaderInterface* mr_intf, QWORD size, string args);

	void scanForFloatFromAddress(MemReaderInterface* mr_intf, string args);

	void scanForFloat(MemReaderInterface* mr_intf, string args, QWORD pStart, bool yankPstart);

	int  tokenizeString(string input, vector<string>& tokens);

	int  tokenizeDate(string input, vector<string>& tokens);

public:
	Debugger();

	void enterDebugLoop(MemReaderInterface* mr_intf, IniReaderInterface* ir_intf);
	void displayOffsetsSection(const string& sectionTitle, const vector<string>& names, const vector<QWORD>& offsets);
};