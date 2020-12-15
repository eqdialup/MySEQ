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

#include "Spawn.h"



Spawn::Spawn(void)

{

	// reserve space for 700 spawns, which should cover most zones. 

	// note: vectors will grow as needed even if we exceed the 700

	spawnList.reserve(700);

	largestOffset = 0;

}



void Spawn::setOffset(offset_types ot, UINT value, string ptrName)

{

	offsets[ot] = value;

	ptrNames[ot] = ptrName;

}



void Spawn::init(IniReaderInterface* ir_intf)

{
	if (ir_intf->readIntegerEntry("SpawnInfo Offsets", "EightBitRace") == 0)
		race8 = false;
	else
		race8 = true;

	setOffset(OT_name,		ir_intf->readIntegerEntry("SpawnInfo Offsets", "NameOffset"), "First Name");

	setOffset(OT_lastname,	ir_intf->readIntegerEntry("SpawnInfo Offsets", "LastNameOffset"), "Last Name");

	setOffset(OT_x,			ir_intf->readIntegerEntry("SpawnInfo Offsets", "XOffset"), "X");

	setOffset(OT_y,			ir_intf->readIntegerEntry("SpawnInfo Offsets", "YOffset"), "Y");

	setOffset(OT_z,			ir_intf->readIntegerEntry("SpawnInfo Offsets", "ZOffset"), "Z");

	setOffset(OT_speed,		ir_intf->readIntegerEntry("SpawnInfo Offsets", "SpeedOffset"), "Speed");

	setOffset(OT_heading,	ir_intf->readIntegerEntry("SpawnInfo Offsets", "HeadingOffset"), "Heading");

	setOffset(OT_prev,		ir_intf->readIntegerEntry("SpawnInfo Offsets", "PrevOffset"), "Previous");

	setOffset(OT_next,		ir_intf->readIntegerEntry("SpawnInfo Offsets", "NextOffset"), "Next");

	setOffset(OT_type,		ir_intf->readIntegerEntry("SpawnInfo Offsets", "TypeOffset"), "Type");

	setOffset(OT_level,		ir_intf->readIntegerEntry("SpawnInfo Offsets", "LevelOffset"), "Level");

	setOffset(OT_hidden,	ir_intf->readIntegerEntry("SpawnInfo Offsets", "HideOffset"), "Hidden");

	setOffset(OT_class,		ir_intf->readIntegerEntry("SpawnInfo Offsets", "ClassOffset"), "Class");

	setOffset(OT_id,		ir_intf->readIntegerEntry("SpawnInfo Offsets", "SpawnIDOffset"), "Id");

	setOffset(OT_owner,		ir_intf->readIntegerEntry("SpawnInfo Offsets", "OwnerIDOffset"), "OwnerID");

	setOffset(OT_race,		ir_intf->readIntegerEntry("SpawnInfo Offsets", "RaceOffset"), "Race");

	setOffset(OT_primary,	ir_intf->readIntegerEntry("SpawnInfo Offsets", "PrimaryOffset"), "Primary");
	
	setOffset(OT_offhand,	ir_intf->readIntegerEntry("SpawnInfo Offsets", "OffhandOffset"), "Offhand");

	// Determine how many bytes we should read for each spawn

	largestOffset = 0;

	for (int i = 0; i < OT_max; i++)

	{

		if (offsets[i] > largestOffset)

			largestOffset = offsets[i];

	}

	largestOffset += 30;

	// The raw buffer is what where we dump raw data from the EQ process into

	rawBuffer = new char[largestOffset];	

	cout << "Spawn: Spawn Offsets read in." << endl;

}


/* the rest of the net buffer can be filled in directly, but this makes

   it easier to pack in the string information */

void Spawn::packNetBufferStrings(UINT flags, string firstName, string lastName)

{

	memset(tempNetBuffer.name, 0, 30);

	memset(tempNetBuffer.lastName, 0, 22);

	firstName._Copy_s(tempNetBuffer.name, 30, 30);

	lastName._Copy_s(tempNetBuffer.lastName, 22, 22);

	tempNetBuffer.flags = flags;

}



void Spawn::packNetBufferRaw(UINT flags, UINT _this)

{

	packNetBufferStrings(flags, extractRawString(OT_name), extractRawString(OT_lastname));

	tempNetBuffer.x			= extractRawFloat(OT_x);

	tempNetBuffer.y			= extractRawFloat(OT_y);

	tempNetBuffer.z			= extractRawFloat(OT_z);

	tempNetBuffer.heading	= extractRawFloat(OT_heading);

	tempNetBuffer.speed		= extractRawFloat(OT_speed);

	// When the packet is for process id's use this

	// otherwise use the real id.

	if (flags == 0x06) {

		tempNetBuffer.id		= _this;

	} else {

		tempNetBuffer.id		= extractRawDWord(OT_id);

	}
	

	tempNetBuffer.type		= extractRawByte(OT_type);

	tempNetBuffer._class	= extractRawByte(OT_class);

	if (race8 == true) {
		// Use an 8 bit race, and a 16 bit material id
		tempNetBuffer.race	= extractRawByte(OT_race);
		tempNetBuffer.primary	= extractRawWord(OT_primary);
		tempNetBuffer.offhand	= extractRawWord(OT_offhand);
		tempNetBuffer.id		= extractRawWord(OT_id);
		tempNetBuffer.owner = extractRawWord(OT_owner);
	} else {
		tempNetBuffer.race		= extractRawDWord(OT_race);
		tempNetBuffer.primary	= extractRawDWord(OT_primary);
		tempNetBuffer.offhand	= extractRawDWord(OT_offhand);
		tempNetBuffer.owner = extractRawDWord(OT_owner);
	}
	tempNetBuffer.level		= extractRawByte(OT_level);

	tempNetBuffer.hidden	= extractRawByte(OT_hidden);

	

}

void Spawn::packNetBufferEmpty(UINT flags, UINT _this)

{

	packNetBufferStrings(flags, "", "");

	tempNetBuffer.id		= 99999;

}





void Spawn::packNetBufferFrom(Item item)

{

	packNetBufferStrings(item.tempItemBuffer.flags, item.tempItemBuffer.name, "");

	tempNetBuffer.x      = item.tempItemBuffer.x;

	tempNetBuffer.y      = item.tempItemBuffer.y;

	tempNetBuffer.z      = item.tempItemBuffer.z;

	tempNetBuffer.id     = item.tempItemBuffer.id;

}



void Spawn::packNetBufferWorld(World world)

{

	packNetBufferStrings(world.tempWorldBuffer.flags, "", "");

	tempNetBuffer.type      = world.tempWorldBuffer.hour;

	tempNetBuffer._class    = world.tempWorldBuffer.minute;

	tempNetBuffer.level     = world.tempWorldBuffer.day;

	tempNetBuffer.hidden    = world.tempWorldBuffer.month;

	tempNetBuffer.race      = world.tempWorldBuffer.year;

}

