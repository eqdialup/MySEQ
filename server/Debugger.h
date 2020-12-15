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

	UINT offsets[OT_max];

	string ptrNames[OT_max];

	

	void init(IniReaderInterface* ir_intf);

	void displayCurrentOffsets();

	bool setOffset(offset_types ot, UINT value);

	void setOffset(offset_types ot, UINT value, string ptrName);

	void setOffset(bool primary, string userInput);

	void printMenu();

	void examineRawMemory(MemReaderInterface* mr_intf, offset_types ot);

	void processSpawn(MemReaderInterface* mr_intf, offset_types ot);

	void walkSpawnList(MemReaderInterface* mr_intf, offset_types ot, bool reverse);

	void scanForPtr(MemReaderInterface* mr_intf, UINT pSearch, UINT pStart, UINT size);

	void scanForString(MemReaderInterface* mr_intf, offset_types ot, UINT size, string searchStr);

	void scanForWorldFromDate(MemReaderInterface* mr_intf, offset_types ot, UINT size, string args);

	void scanForUINT(MemReaderInterface* mr_intf, UINT pStart, UINT size, string args);

	void showProcesses(MemReaderInterface* mr_intf, string processName);

	void scanForFloatFromTarget(MemReaderInterface* mr_intf, string args);

	void scanForFloatFromSelf(MemReaderInterface* mr_intf, string args);

	void scanForUINTFromSelf(MemReaderInterface* mr_intf, UINT size, string args);

	void scanForFloatFromAddress(MemReaderInterface* mr_intf, string args);

	void scanForFloat(MemReaderInterface* mr_intf, string args, UINT pStart, bool yankPstart);

	int  tokenizeString(string input, vector<string>& tokens);

	int  tokenizeDate(string input, vector<string>& tokens);

	

public:

	Debugger();

	void enterDebugLoop(MemReaderInterface* mr_intf, IniReaderInterface* ir_intf);

};

