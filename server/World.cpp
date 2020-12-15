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

#include "World.h"

 

World::World(void)

{

	// Initialize member data

	worldList.reserve(1);

	largestOffset = 0;

}

 

void World::init(IniReaderInterface* ir_intf)

{
	if (ir_intf->readIntegerEntry("SpawnInfo Offsets", "EightBitRace") == 0)
		race8 = false;
	else
		race8 = true;
	setOffset(OT_hour, ir_intf->readIntegerEntry("WorldInfo Offsets", "WorldHourOffset"), "Hour");

	setOffset(OT_minute, ir_intf->readIntegerEntry("WorldInfo Offsets", "WorldMinuteOffset"), "Minute");

	setOffset(OT_day, ir_intf->readIntegerEntry("WorldInfo Offsets", "WorldDayOffset"), "Day");

	setOffset(OT_month, ir_intf->readIntegerEntry("WorldInfo Offsets", "WorldMonthOffset"), "Month");

	setOffset(OT_year, ir_intf->readIntegerEntry("WorldInfo Offsets", "WorldYearOffset"), "Year");

 

	// Determine how many bytes we should read at item in memory

	largestOffset = 0;

	for (int i = 0; i < OT_max; i++)

	{

		if (offsets[i] > largestOffset)

			largestOffset = offsets[i]; 

	}

	largestOffset += 30;

 

	// Allocate memory for our temporary buffer

	// We use this to store the raw data from the EQ process

	rawBuffer = new char[largestOffset];



	cout << "World: WorldInfo Offsets read in." << endl;

}

 

void World::setOffset(offset_types ot, int value, string name)

{

	offsets[ot] = value;

	offsetNames[ot] = name;

}



void World::packWorldBuffer(UINT flags)

{

	tempWorldBuffer.hour = extractRawByte(OT_hour);

	tempWorldBuffer.minute = extractRawByte(OT_minute);

	tempWorldBuffer.day = extractRawByte(OT_day);

	tempWorldBuffer.month = extractRawByte(OT_month);
	if (race8)
		tempWorldBuffer.year = extractRawWord(OT_year);
	else
		tempWorldBuffer.year = extractRawDWord(OT_year);

	tempWorldBuffer.flags = flags;

}