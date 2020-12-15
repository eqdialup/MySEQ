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

#include "Item.h"

 

Item::Item(void)

{

	// Initialize member data

	itemList.reserve(300);

	largestOffset = 0;

}

 

void Item::init(IniReaderInterface* ir_intf)

{

	setOffset(OT_prev, ir_intf->readIntegerEntry("GroundItem Offsets", "PrevOffset"), "Previous");

	setOffset(OT_next, ir_intf->readIntegerEntry("GroundItem Offsets", "NextOffset"), "Next");

	setOffset(OT_id, ir_intf->readIntegerEntry("GroundItem Offsets", "IdOffset"), "ID");

	setOffset(OT_dropid, ir_intf->readIntegerEntry("GroundItem Offsets", "DropIdOffset"), "Drop ID");

	setOffset(OT_x, ir_intf->readIntegerEntry("GroundItem Offsets", "XOffset"), "X");

	setOffset(OT_y, ir_intf->readIntegerEntry("GroundItem Offsets", "YOffset"), "Y");

	setOffset(OT_z, ir_intf->readIntegerEntry("GroundItem Offsets", "ZOffset"), "Z");

	setOffset(OT_name, ir_intf->readIntegerEntry("GroundItem Offsets", "NameOffset"), "Model Name");

 

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



	cout << "Item: GroundItem Offsets read in." << endl;

}

 

void Item::setOffset(offset_types ot, int value, string name)

{

	offsets[ot] = value;

	offsetNames[ot] = name;

}



void Item::packItemBuffer(UINT flags)

{

	tempItemBuffer.name = extractRawString(OT_name);

	

	tempItemBuffer.x			= extractRawFloat(OT_x);

	tempItemBuffer.y			= extractRawFloat(OT_y);

	tempItemBuffer.z			= extractRawFloat(OT_z);

	

	tempItemBuffer.id		= extractRawDWord(OT_id);

	tempItemBuffer.dropid	= extractRawDWord(OT_dropid);

	

	tempItemBuffer.flags = flags;

}