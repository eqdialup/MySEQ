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

#include "StdAfx.h"

#include "Debugger.h"

#include <cstdlib>

#define TO_LOWER(str) (transform(str.begin(), str.end(), str.begin(), (int(*)(int))tolower))

#define DISPLAY_SPAWN_ITEM(off, member) cout << "    " << spawnParser.ptrNames[spawnParser.off] << " -> " << spawnParser.tempNetBuffer.member << endl

#define DISPLAY_SPAWN_ITEMI(off, member) cout << "    " << spawnParser.ptrNames[spawnParser.off] << " -> " << (UINT) spawnParser.tempNetBuffer.member << endl



Debugger::Debugger(){}



bool Debugger::setOffset(offset_types ot, UINT value)

{

	if (ot >= OT_max)

		return false;

		

	offsets[ot] = value;

	return true;

}



void Debugger::setOffset(offset_types ot, UINT value, string ptrName)

{

	setOffset(ot, value);

	ptrNames[ot] = ptrName;

}



void Debugger::init(IniReaderInterface* ir_intf)

{

	spawnParser.init(ir_intf);

	itemParser.init(ir_intf);

	worldParser.init(ir_intf);

	

	setOffset(OT_spawnlist,	ir_intf->readIntegerEntry("Memory Offsets", "SpawnHeaderAddr"), "pSpawns");

	setOffset(OT_self,		ir_intf->readIntegerEntry("Memory Offsets", "CharInfo"), "pSelf");

	setOffset(OT_target,	ir_intf->readIntegerEntry("Memory Offsets", "TargetAddr"), "pTarget");

	setOffset(OT_zonename,	ir_intf->readIntegerEntry("Memory Offsets", "ZoneAddr"), "pZone");

	setOffset(OT_ground,	ir_intf->readIntegerEntry("Memory Offsets", "ItemsAddr"), "pItems");

	setOffset(OT_world,		ir_intf->readIntegerEntry("Memory Offsets", "WorldAddr"), "pWorld");

	

	cout << "Debugger: Memory offsets read in." << endl;

}



void Debugger::printMenu()

{

	cout << endl;

	cout << "   (d)isplay / (r)eload offsets" << endl;

//	cout << "    r) reload all offsets from INI file" << endl;

	cout << "  spo) set a primary offset   (index/name) (hex value)" << endl;

	cout << "  sso) set a secondary offset (index/name) (hex value)" << endl;

	cout << "   ez) examine data using pZone (et) pTarget (ew) pWorld" << endl;

	cout << "   es) examine raw data using pSelf" << endl;

//	cout << "   et) examine raw data using pTarget" << endl;

//	cout << "   ew) examine raw data using pWorld" << endl;

	cout << "   fz) find zonename using pZone (zonename)" << endl;

	cout << "   ft) find spawnname using pTarget (fs) pSelf (spawnname)" << endl;

//	cout << "   fs) find spawnname using pSelf (spawnname)" << endl;

	cout << "   ps) display spawn info using pSelf (pt) pTarget" << endl;

//	cout << "   pt) display spawn info using pTarget" << endl;

	cout << "   sp) scan process names (process name)" << endl;

	cout << "  sft) scan for float using pTarget (sfs) pSelf (X,Y,Z)" << endl;
//	cout << "  sfs) scan for floating point using pSelf (X,Y,Z)" << endl;

	cout << "  sfa) scan for floating point using Address (X,Y,Z,Address)" << endl;

	cout << "  sfu) scan for UINT using pSelf (int)" << endl;

	cout << "  sfw) scan for world using Game Date (mm/dd/yyyy) from /time" << endl;

	cout << "   sg) scan for ground items" << endl;

	cout << "   ws) walk the spawnlist (reverse) using pSelf (wt) pTarget" << endl;

//	cout << "   wt) walk the spawnlist (reverse) using pTarget" << endl;

	cout << "   vs) walk the spawnlist (forward) using pSelf (vt) pTarget" << endl;

//	cout << "   vt) walk the spawnlist (forward) using pTarget" << endl;
	
	cout << "    x) exit debugger" << endl;

	cout << endl;

}

void Debugger::enterDebugLoop(MemReaderInterface* mr_intf, IniReaderInterface* ir_intf)

{

	string userInput;

	

	init(ir_intf);

	printMenu();

	while (1)

	{

		

		cout << " > ";

		getline( cin, userInput);

		if (userInput.compare(0, 1, "?") == 0)

			printMenu();

		else if (userInput.compare(0, 1, "d") == 0)

			displayCurrentOffsets();

		else if (userInput.compare(0, 1, "r") == 0)

			init(ir_intf);

		else if (userInput.compare(0, 3, "spo") == 0)

			setOffset(true, userInput.erase(0, 4));

		else if (userInput.compare(0, 3, "sso") == 0)

			setOffset(false, userInput.erase(0, 4));

		else if (userInput.compare(0, 2, "ez") == 0)

			examineRawMemory(mr_intf, OT_zonename);

		else if (userInput.compare(0, 2, "et") == 0)

			examineRawMemory(mr_intf, OT_target);
		
		else if (userInput.compare(0, 2, "es") == 0)

			examineRawMemory(mr_intf, OT_self);

		else if (userInput.compare(0, 2, "ew") == 0)

			examineRawMemory(mr_intf, OT_world);

		else if (userInput.compare(0, 2, "fz") == 0)

			scanForString(mr_intf, OT_zonename, 0x100000, userInput.erase(0,3));

		else if (userInput.compare(0, 2, "ft") == 0)

			scanForString(mr_intf, OT_target, 0x100000, userInput.erase(0,3));

		else if (userInput.compare(0, 2, "fs") == 0)

			scanForString(mr_intf, OT_self, 0x100000, userInput.erase(0,3));

		else if (userInput.compare(0, 2, "ps") == 0)

			processSpawn(mr_intf, OT_self);

		else if (userInput.compare(0, 2, "pt") == 0)

			processSpawn(mr_intf, OT_target);

		else if (userInput.compare(0, 2, "sp") == 0)

			showProcesses(mr_intf, userInput.erase(0,3));

		else if (userInput.compare(0, 3, "sfa") == 0)

			scanForFloatFromAddress(mr_intf, userInput.erase(0,4));

		else if (userInput.compare(0, 3, "sft") == 0)

			scanForFloatFromTarget(mr_intf, userInput.erase(0,4));

		else if (userInput.compare(0, 3, "sfs") == 0)

			scanForFloatFromSelf(mr_intf, userInput.erase(0,4));

		else if (userInput.compare(0, 3, "sfu") == 0)

			scanForUINTFromSelf(mr_intf, 0x20000, userInput.erase(0,4));

		else if (userInput.compare(0, 3, "sfw") == 0)

			scanForWorldFromDate(mr_intf, OT_world, 0x100000, userInput.erase(0,4));

		else if (userInput.compare(0, 2, "sg") == 0)

			scanForString(mr_intf, OT_ground, 0x100000, "IT");

		else if (userInput.compare(0, 2, "ws") == 0)

			walkSpawnList(mr_intf, OT_self, true);

		else if (userInput.compare(0, 2, "wt") == 0)

			walkSpawnList(mr_intf, OT_target, true);

		else if (userInput.compare(0, 2, "vs") == 0)

			walkSpawnList(mr_intf, OT_self, false);

		else if (userInput.compare(0, 2, "vt") == 0)

			walkSpawnList(mr_intf, OT_target, false);

		else if (userInput.compare(0, 1, "x") == 0)

			break;	

		else

			cout << " Invalid selection. Please try again." << endl;

		cout << "    ?) display main menu" << endl;

	}

}



void Debugger::displayCurrentOffsets()

{

	int i;

	

	cout << endl;

	cout << "     Primary Offsets" << endl;

	cout << "=========================" << endl;

	for (i=0; i<OT_max; i++)

	{

		cout.width(10);

		cout << setfill(' ') << right << i << ") " << ptrNames[i] << " = 0x" << hex << offsets[i] << endl;

	}

	

	cout << endl;

	cout << "    Secondary Spawn Offsets" << endl;

	cout << "===============================" << endl;

	for (i=0; i<spawnParser.OT_max ; i++)

	{

		cout.width(10);

		cout << setfill(' ') << right << i << ") " << spawnParser.ptrNames[i] << " = 0x" <<

			setw(3) << hex << setfill('0') << spawnParser.offsets[i] <<

			dec << " (" << spawnParser.offsets[i] << ")" << endl;

	}

	

	cout << endl;

	cout << "    Secondary Ground Offsets" << endl;

	cout << "===============================" << endl;

	for (i=0; i<itemParser.OT_max ; i++)

	{

		cout.width(10);

		cout << setfill(' ') << right << i << ") " << itemParser.offsetNames[i] << " = 0x" <<

			setw(3) << hex << setfill('0') << itemParser.offsets[i] <<

			dec << " (" << itemParser.offsets[i] << ")" << endl;

	}



	cout << endl;

	cout << "    Secondary World Offsets" << endl;

	cout << "===============================" << endl;

	for (i=0; i<worldParser.OT_max ; i++)

	{

		cout.width(10);

		cout << setfill(' ') << right << i << ") " << worldParser.offsetNames[i] << " = 0x" <<

			setw(3) << hex << setfill('0') << worldParser.offsets[i] <<

			dec << " (" << worldParser.offsets[i] << ")" << endl;

	}

	

	cout << endl;		

	

}



void Debugger::setOffset(bool primary, string args)

{

	int parm1Int, parm2Int, i, OT_max;

	string parm1String;

	stringstream element(args);

	bool useIndex = true;

	

	// Primary offsets are in this Debugger class, but Secondary offsets live in the Spawn class.

	if (primary)

		OT_max = this->OT_max;

	else

		OT_max = spawnParser.OT_max;

	

	// Process the input parameters as either (string + int) or (int + int)

	if (!(element >> parm1Int))

	{

		element.clear();

		if (!(element >> parm1String))

			return;

		useIndex = false;

	}

	if (!(element >> hex >> parm2Int))

		return;

		

	

	// If the user has input parameter 1 as a string, determine the appropriate offset index.

	if (!useIndex)

	{

		string ptrName;

		string parm1Str(parm1String);

		

		TO_LOWER(parm1Str);

		parm1Int = OT_max;

		

		for (i=0; i<OT_max; i++)

		{

			if (primary)

				ptrName = ptrNames[i];

			else

				ptrName = spawnParser.ptrNames[i];

			

			TO_LOWER(ptrName);

				

			if (parm1Str == ptrName)

			{

				parm1Int = i;

				break;

			}

		}

	}			

	

	if (primary)

	{

		if (setOffset((offset_types)parm1Int, parm2Int))

			cout << " Primary offset #" << parm1Int << " (" << ptrNames[parm1Int] << ") was set to 0x" << hex << parm2Int << endl;

		else

			cout << " Failed to set primary offset" << endl;

	}

	else

	{

		if (parm1Int >= spawnParser.OT_max)

		{

			cout << " Failed to set secondary offset" << endl;

			return;

		}

		

		spawnParser.offsets[parm1Int] = parm2Int;

		cout << " Secondary offset #" << parm1Int << " (" << spawnParser.ptrNames[parm1Int] << ") was set to 0x" << hex << parm2Int << endl;

	}

}



void Debugger::examineRawMemory(MemReaderInterface* mr_intf, offset_types ot)

{

	const UINT bufSize=2048;

	char buffer[bufSize], temp[65];

	UINT pMem, index;

	int r,c;

	

	memset(buffer, 0, bufSize);

	

	// pZone is a direct pointer. All others are indirect.

	if (ot == OT_zonename)

		pMem = offsets[ot] - 0x400000 + (UINT)mr_intf->getCurrentBaseAddress();

	else

		pMem = mr_intf->extractRAWPointer(offsets[ot]);

	

	cout << " Display Raw Memory from 0x" << (pMem + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << " to 0x" << (pMem + bufSize  + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << endl;

	

	// Read the raw memory into our local buffer

	if (pMem)

	{

		if (!(mr_intf->extractToBuffer(pMem, buffer, bufSize)))

		{

			cout << " Failed to read memory at address 0x" << hex << (pMem + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << endl;

			return;

		}

	}

	else

	{

		cout << " Failed to obtain valid memory pointer for offset " << ptrNames[ot] << endl;

		return;

	}

	

	// Display the data

	for (r=0; r<bufSize/16; r++)

	{

		cout << "0x" << hex << setfill('0') << setw(2) << r*16 << ") ";

		

		// Display data in byte format

		for (c=0; c<16; c++)

		{

			index = r*16 + c;

			_itoa_s((buffer[index] & 0xFF), temp, 65, 16);

			cout << " " << setw(2) << setfill('0') << hex << temp;

			if ((c % 4) == 3)

				cout << " ";

		}

		

		// Display data in string format

		cout << "  ";

		for (c=0; c<16; c++)

		{

			index = r*16 + c;

			if (isalnum(buffer[index] & 0xFF))

				cout << buffer[index];

			else

				cout << ".";

		}

		cout << endl;

	}

}



void Debugger::processSpawn(MemReaderInterface* mr_intf, offset_types ot)

{

	UINT pMem;

	

	// pZone is a direct pointer. All others are indirect.

	if (ot == OT_zonename)

		pMem = offsets[ot] - 0x400000 + (UINT)mr_intf->getCurrentBaseAddress();

	else

		pMem = mr_intf->extractRAWPointer(offsets[ot]);

	

	if (pMem)

	{

		if (!(mr_intf->extractToBuffer(pMem, spawnParser.rawBuffer, spawnParser.largestOffset)))

		{

			cout << " Failed to read memory at address 0x" << hex << (pMem + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << endl;

			return;

		}

	}

	else

	{

		cout << " Failed to obtain valid memory pointer for offset " << ptrNames[ot] << endl;

		return;

	}

	

	// Use the existing spawn parser to fill out the Spawn::tempNetBuffer structure, which we can examine later.

	spawnParser.packNetBufferRaw(0, pMem);

	

	// Display the parsed data

	cout << " " << ptrNames[ot] << " = 0x" << hex << (pMem + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << endl;

	DISPLAY_SPAWN_ITEM(OT_name, name);

	DISPLAY_SPAWN_ITEM(OT_lastname, lastName);

	DISPLAY_SPAWN_ITEMI(OT_id, id);

	DISPLAY_SPAWN_ITEMI(OT_owner, owner);

	DISPLAY_SPAWN_ITEMI(OT_level, level);

	DISPLAY_SPAWN_ITEM(OT_race, race);

	DISPLAY_SPAWN_ITEMI(OT_class, _class);

	DISPLAY_SPAWN_ITEM(OT_x, x);

	DISPLAY_SPAWN_ITEM(OT_y, y);

	DISPLAY_SPAWN_ITEM(OT_z, z);

	DISPLAY_SPAWN_ITEM(OT_heading, heading);

	DISPLAY_SPAWN_ITEM(OT_speed, speed);

	DISPLAY_SPAWN_ITEMI(OT_type, type);

	DISPLAY_SPAWN_ITEMI(OT_hidden, hidden);

	DISPLAY_SPAWN_ITEM(OT_primary, primary);

	DISPLAY_SPAWN_ITEM(OT_offhand, offhand);

}



void Debugger::walkSpawnList(MemReaderInterface* mr_intf, offset_types ot, bool reverse)

{

	UINT pMem, pPrev, pNext, spawnCount;

	

	pMem = mr_intf->extractRAWPointer(offsets[ot]);

	

	// First try and get to the initial spawn entity

	if (pMem)

	{

		if (!(mr_intf->extractToBuffer(pMem, spawnParser.rawBuffer, spawnParser.largestOffset)))

		{

			cout << " Failed to read memory at address 0x" << hex << (pMem + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << endl;

			return;

		}

	}

	else

	{

		cout << " Failed to obtain valid memory pointer for offset " << ptrNames[ot] << endl;

		return;

	}

	

	spawnCount = 0;

	

	if (reverse)

		cout << " Walking spawnlist in reverse." << endl;

	else

		cout << " Walking spawnlist forward." << endl;

		

	do

	{

		// At this point we appear to have located a possible valid spawn entity. We will attempt to walk the list in reverse.

		// Use the existing spawn parser to fill out the Spawn::tempNetBuffer structure. We need the name of the spawn.

		spawnParser.packNetBufferRaw(0, pMem);

		

		// We also need the Prev ptr values which are in Spawn::rawBuffer

		pPrev = spawnParser.extractPrevPointer();

		pNext = spawnParser.extractNextPointer();

		spawnCount++;

		

		// Display a small amount of information about this spawn

		cout << "    -----------------------------------" << endl;

		DISPLAY_SPAWN_ITEM(OT_name, name);

		DISPLAY_SPAWN_ITEMI(OT_id, id);

		cout << "    " << spawnParser.ptrNames[spawnParser.OT_prev] << " -> 0x" << hex << pPrev << endl;

		cout << "    " << spawnParser.ptrNames[spawnParser.OT_next] << " -> 0x" << hex << pNext << endl;

		

		// Walk up the list until we reach the beginning

		if (( reverse && pPrev) || (!reverse && pNext))

		{

			if (reverse)

				pMem = pPrev;

			else

				pMem = pNext;

			

			if (!(mr_intf->extractToBuffer(pMem, spawnParser.rawBuffer, spawnParser.largestOffset)))

			{

				if (spawnCount == 0)

					cout << " Failed to read memory at address 0x" << hex << (pMem + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << endl;

				pMem = 0;

			}

		}

	}

	while ( ((!reverse && pNext) || ( reverse && pPrev)) && (spawnCount < 1000));

	

	cout << " Discovered " << dec << spawnCount << " spawn entities during the walk." << endl;

	

	if (reverse)

		scanForPtr(mr_intf, pMem, 0x800000, 0x800000);

}



void Debugger::scanForPtr(MemReaderInterface* mr_intf, UINT pSearch, UINT pStart, UINT size)

{

	UINT pMem, pExtracted;

	

	if (!pStart)

		return;

		

	cout << " Scanning for 0x" << hex << pSearch << " from 0x" << (pStart + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << " to 0x" << (pStart+size + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << endl;

	

	for (pMem=pStart; pMem<(pStart+size); pMem+=1)

	{

		if (!(mr_intf->extractToBuffer(pMem, (char*) &pExtracted, sizeof(pExtracted))))

		{

			//cout << " Failed to read memory at address 0x" << hex << pMem << endl;

			//return;

			continue;

		}

		if (pSearch == pExtracted)

		{

			cout << " Pointer match found for 0x" << hex << pSearch << " at 0x" << (pMem + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << endl;

		}

	}

}



void Debugger::scanForString(MemReaderInterface* mr_intf, offset_types ot, UINT size, string searchStr)

{

	UINT pMem, pStart, pDeepMem, pDeepMem2, nameOffset;

	char buffer[100];

	string spawnName, itemName;

	if ( (offsets[ot] == 0) || (size == 0) || searchStr.length() == 0)

	{

		cout << " Error: '" << searchStr << "' appears to be an invalid search string." << endl;

		return;

	}

	

	pStart = offsets[ot] - 4*size - 0x400000 + (UINT)mr_intf->getCurrentBaseAddress();

	

	if (ot == OT_ground)

		nameOffset = itemParser.offsets[itemParser.OT_name];

	else

		nameOffset = spawnParser.offsets[spawnParser.OT_name];

	

	cout << " Scanning for '" << searchStr << "' from 0x" << hex << pStart << " to 0x" << (pStart+8*size) << endl;

	

	for (pMem=pStart; pMem<(pStart+size*8);)

	{

		// The zonename search is a little different

		if (ot == OT_zonename)

		{

			if (!(mr_intf->extractToBuffer(pMem, buffer, 1)))

			{

				//cout << " Failed to read memory at address 0x" << hex << pMem << endl;

				//return;

				pMem += 1;

				continue;

			}

			if (searchStr[0] == buffer[0])

			{

				mr_intf->extractToBuffer(pMem, buffer, 30);

				if (searchStr == buffer)

				{

					cout << " Pointer match found at 0x" << hex << (pMem + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << endl;

				}

			}

			pMem += 1;

		}

		else if ((ot == OT_target) || (ot == OT_self))

		{

			// When seaching for spawn names, we have an additional pointer to go thru first.

			if (!(pDeepMem = mr_intf->extractPointer(pMem)))

			{

				//cout << " Failed to read memory at address 0x" << hex << pMem << endl;

				//return;

				pMem += 4;

				continue;

			}

			if (pDeepMem > pMem)

			{

				spawnName = mr_intf->extractString(pDeepMem + nameOffset);

				if (searchStr == spawnName)

				{

					cout << " Pointer match found at 0x" << (pMem + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << endl;

				}

			}

			pMem += 4;

		}

		else if (ot == OT_ground)

		{

			// When seaching for ground items, we have an additional pointer to go thru first.

			if (!(pDeepMem2 = mr_intf->extractPointer(pMem)))

			{

				//cout << " Failed to read memory at address 0x" << hex << pMem << endl;

				//return;

				pMem += 4;

				continue;

			}
			if (pDeepMem2 > pMem)

			{

				itemName = mr_intf->extractString(pDeepMem2 + nameOffset);

				if ( itemName.compare(0,2,searchStr) == 0)

				{

					cout << " Pointer match found at 0x" << (pMem + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << ". Full string is " << itemName << endl;

				}

			}

			if (!(pDeepMem = mr_intf->extractPointer(pDeepMem2)))

			{

				//cout << " Failed to read memory at address 0x" << hex << pMem << endl;

				//return;

				pMem += 4;

				continue;

			}

			

			if (pDeepMem > pMem)

			{

				itemName = mr_intf->extractString(pDeepMem + nameOffset);

				if ( itemName.compare(0,2,searchStr) == 0)

				{

					cout << " Pointer match found at 0x" << (pMem + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << ". Full string is " << itemName << endl;

				}

			}

			pMem += 4;

		}

	}	

}



void Debugger::showProcesses(MemReaderInterface* mr_intf, string processName)

{

	if ( processName == "" )

		processName = "eqgame";

	

	// Show all possible matching process information

	mr_intf->openFirstProcess(processName, true);

	while (mr_intf->openNextProcess(processName, true)){};

}



void Debugger::scanForFloatFromTarget(MemReaderInterface* mr_intf, string args)

{

	UINT pStart;

	

	pStart = mr_intf->extractPointer(offsets[OT_target]);

	scanForFloat(mr_intf, args, pStart, false);

}

void Debugger::scanForFloatFromSelf(MemReaderInterface* mr_intf, string args)

{

	UINT pStart;


	pStart = mr_intf->extractPointer(offsets[OT_self]);

	scanForFloat(mr_intf, args, pStart, false);

}

void Debugger::scanForUINTFromSelf(MemReaderInterface* mr_intf, UINT size, string args)

{

	UINT pStart;

	pStart = mr_intf->extractPointer(offsets[OT_self]);

	scanForUINT(mr_intf, pStart, size, args);

}

void Debugger::scanForFloatFromAddress(MemReaderInterface* mr_intf, string args)

{

	scanForFloat(mr_intf, args, 0, true);

}



void Debugger::scanForFloat(MemReaderInterface* mr_intf, string args, UINT pStart, bool yankPstart)

{

	const int INVALID = -100000;

	float xFind, yFind, zFind;

	int xTemp, yTemp, zTemp;

	vector<string> tokens;

	UINT pFloat, pEnd;

	bool xMatch, yMatch, zMatch;

	bool sCheck, dCheck, tCheck;

	

	// First get our x/y/z values if given

	xFind = yFind = zFind = INVALID;

	sCheck = dCheck = tCheck = false;

	

	switch ( tokenizeString(args, tokens) )

	{

		case 4:

			if (yankPstart) {

				pStart = strtol(tokens[3].c_str(), NULL, 16);
				pStart = pStart - 0x400000 + (UINT)mr_intf->getCurrentBaseAddress();
				
			}
			xFind = (float) atof(tokens[0].c_str());

			yFind = (float) atof(tokens[1].c_str());

			zFind = (float) atof(tokens[2].c_str());

			tCheck = true;

			break;

		

		case 3:

			xFind = (float) atof(tokens[0].c_str());

			yFind = (float) atof(tokens[1].c_str());

			if (yankPstart)

			{

				pStart = atoi(tokens[2].c_str());
				pStart = pStart - 0x400000 + (UINT)mr_intf->getCurrentBaseAddress();
				

				dCheck = true;

			}

			else

			{

				zFind = (float) atof(tokens[2].c_str());

				tCheck = true;

			}

			break;

		

		case 2:

			xFind = (float) atof(tokens[0].c_str());

			if (yankPstart)

			{

				pStart = atoi(tokens[1].c_str());
				pStart = pStart - 0x400000 + (UINT)mr_intf->getCurrentBaseAddress();
				
				sCheck = true;

			}

			else

			{

				yFind = (float) atof(tokens[1].c_str());

				dCheck = true;

			}

			break;

		

		case 1:

			xFind = (float) atof(tokens[0].c_str());

			sCheck = true;

			break;

		default:

			break;

	}

	

	if (pStart == 0)

		return;

	

	// For target searches, limit search to 4K. Otherwise scan 64M

	if (yankPstart)

		pEnd = pStart + 0x4000000;

	else

		pEnd = pStart + 0x1000;

	

	// First try and get to the initial spawn entity

	for ( pFloat = pStart; pFloat < pEnd; pFloat += 4 )

	{

		xMatch = yMatch = zMatch = false;

		xTemp = yTemp = zTemp = INVALID;

		

		if (sCheck || dCheck || tCheck)

		{

			xTemp = (int) mr_intf->extractFloat(pFloat);

			if (fabs(xTemp - xFind) < 20)

				xMatch = true;

		}

		

		if (dCheck || tCheck)

		{

			yTemp = (int) mr_intf->extractFloat(pFloat + 4);

			if (fabs(yTemp - yFind) < 20)

				yMatch = true;

		}

		

		if (tCheck)

		{

			zTemp = (int) mr_intf->extractFloat(pFloat + 8);

			if (fabs(zTemp - zFind) < 20)

				zMatch = true;

		}

		

		if (sCheck)

		{

			if (xMatch)

				cout << hex << "  X match found at offset 0x" << (pFloat - pStart + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << endl;

		}

		

		if (dCheck)

		{

			if (xMatch && yMatch)

				cout << hex << "  X,Y match found at offset 0x" << (pFloat - pStart + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << ", 0x" << (pFloat - pStart + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) + 4

					 << dec << " (" << xTemp << "," << yTemp << ")" << endl;

		}

		

		if (tCheck)

		{

			if (xMatch && yMatch && zMatch)

				cout << hex << "  X,Y,Z match found at offset 0x" << (pFloat - pStart + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << ", 0x" << (pFloat - pStart + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) + 4

					 << ", 0x" << (pFloat - pStart + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) + 8 << dec << " (" << xTemp << "," << yTemp << "," << zTemp << ")" << endl;

		}

	}

}



void Debugger::scanForWorldFromDate(MemReaderInterface* mr_intf, offset_types ot, UINT size, string args)

{

	UINT yFind, yTemp;

	BYTE mFind;

	BYTE dFind;

	BYTE dTemp, mTemp;

	UINT pMem, pStart, pDeepMem;

	UINT dayOffset, monthOffset, yearOffset;

	vector<string> tokens;

	

	if ( (offsets[ot] == 0) || (size == 0) || args.length() == 0)

	{

		cout << "    Error: '" << args << "' appears to be an invalid date." << endl;
		cout << "    Proper usage - sfw mm/dd/yyyy" << endl;
		cout << "    Get date from Game Time using /time" << endl;
 
		return;

	}

	


	dayOffset = worldParser.offsets[worldParser.OT_day];

	monthOffset = worldParser.offsets[worldParser.OT_month];

	yearOffset = worldParser.offsets[worldParser.OT_year];

	

	// First get our month/day/year values if given

	mFind = dFind = yFind = 0;



	if ( tokenizeDate(args, tokens) != 3)

	{

		cout << "    Incomplete Date.  Proper usage - sfw mm/dd/yyyy" << endl;
		cout << "    Get date from Game Time using /time" << endl;

		return;

	}

	mFind = (BYTE) atol(tokens[0].c_str());

	dFind = (BYTE) atol(tokens[1].c_str());

	yFind = (UINT) atol(tokens[2].c_str());



	if (((int) mFind > 12) || ((int)dFind > 31))

	{

		cout << "    Bad Date (mm/dd/yyyy): Limit mm to max of 12 and dd to max of 31" << endl;

		return;

	}

	pStart = offsets[ot] - 2* size;


	if (pStart == 0)

		return;

	

	cout << " Scanning for '" << args << "' from 0x" << hex << pStart << " to 0x" << (pStart+4*size) << endl;

	
	pStart = pStart - 0x400000 + (UINT)mr_intf->getCurrentBaseAddress();

	for (pMem=pStart; pMem<(pStart+size*4);)

	{

		// We have an additional pointer to go thru first.

		if (!(pDeepMem = mr_intf->extractPointer(pMem)))

		{

			//cout << " Failed to read memory at address 0x" << hex << pMem << endl;

			//return;

			pMem += 4;

			continue;

		}

		if (pDeepMem > pMem)

		{

			dTemp = mr_intf->extractBYTE(pDeepMem + dayOffset);

			mTemp = mr_intf->extractBYTE(pDeepMem + monthOffset);

			yTemp = mr_intf->extractUINT(pDeepMem + yearOffset);



			if (dTemp == dFind && mTemp == mFind && yTemp == yFind)

			{

				cout << hex << "  Date match found at offset 0x" << (pMem + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) <<

				dec << " (" << (int) mTemp << "/" << (int) dTemp << "/" << (int) yTemp << ")" << endl;

			}

		}

		pMem += 4;

	}

}

void Debugger::scanForUINT(MemReaderInterface* mr_intf, UINT pStart, UINT size, string args)

{

	UINT Temp;

	UINT Find;

	UINT pMem;

	vector<string> tokens;

	if ( (pStart == 0) || args.length() == 0)

	{

		cout << "    Error: '" << args << "' appears to be an invalid value." << endl;
		cout << "    Proper usage - sfu number" << endl;
 
		return;

	}



	if ( tokenizeString(args, tokens) != 1)

	{

		cout << "    Error: '" << args << "' appears to be an invalid value." << endl;
		cout << "    Proper usage - sfu number" << endl;

		return;

	}

	Find = (UINT) atol(tokens[0].c_str());


	if (pStart == 0)

		return;

	

	cout << " Scanning for '" << args << "' from 0x" << hex << pStart << " to 0x" << (pStart+4*size) << endl;

	
	pStart = pStart - 0x400000 + (UINT)mr_intf->getCurrentBaseAddress();

	for (pMem=pStart; pMem<(pStart+size*2);)

	{
	

		Temp = mr_intf->extractUINT(pMem);

		if (Temp == Find)

		{
			cout << dec << Find << "  match found at offset 0x" << (pMem - pStart + 0x400000 - (UINT)mr_intf->getCurrentBaseAddress()) << endl;

		}

		pMem += 1;

	}

}




int Debugger::tokenizeString(string input, vector<string>& tokens)

{

	string::size_type from = 0;

	string::size_type to = input.find(",", from);

	

	while ( to != string::npos )

	{

		tokens.push_back(input.substr(from, to-from));

        from = to + 1;

        to = input.find(",", from );

	}

	tokens.push_back(input.substr(from, input.length()));

	return tokens.size();

}



int Debugger::tokenizeDate(string input, vector<string>& tokens)

{

	string::size_type from = 0;

	string::size_type to = input.find("/", from);

	

	while ( to != string::npos )

	{

		tokens.push_back(input.substr(from, to-from));

        from = to + 1;

        to = input.find("/", from );

	}

	tokens.push_back(input.substr(from, input.length()));

	return tokens.size();

}