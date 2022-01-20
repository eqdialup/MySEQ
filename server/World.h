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

#include "IniReader.h"



#pragma pack(push, 1)

struct worldBuffer_t

{

	BYTE hour;

	BYTE minute;

	BYTE day;

	BYTE month;

	DWORD year;

	UINT flags;

};



#pragma pack(pop) 

class World

{

public:

	enum offset_types { OT_hour, OT_minute, OT_day, OT_month, OT_year, OT_max };

	UINT offsets[(UINT)World::offset_types::OT_max]{};

	UINT largestOffset;

	string offsetNames[(UINT)World::offset_types::OT_max];

	char* rawBuffer{};

	worldBuffer_t tempWorldBuffer{};

	vector<worldBuffer_t> worldList;



private:

	string extractRawString(World::offset_types ot) { return string(&rawBuffer[World::offsets[(UINT)ot]]); }

	float extractRawFloat(World::offset_types ot) { return *((float*)&rawBuffer[World::offsets[(UINT)ot]]); }

	DWORD extractRawDWord(World::offset_types ot) { return *((DWORD*)&rawBuffer[World::offsets[(UINT)ot]]); }

	WORD extractRawWord(World::offset_types ot) { return *((WORD*)&rawBuffer[World::offsets[(UINT)ot]]); }

	BYTE extractRawByte(World::offset_types ot) { return *((BYTE*)&rawBuffer[World::offsets[(UINT)ot]]); }

	int extractRawInt(World::offset_types ot) { return *((int*)&rawBuffer[World::offsets[(UINT)ot]]); }



public:

	World(void);

	void init(IniReaderInterface* ir_intf);

	void setOffset(World::offset_types ot, int value, string name);

	void packWorldBuffer(UINT flags);

	/* when you are done filling out a NetBuffer, push it for shipping across the network */

	void pushNetBuffer() { World::worldList.push_back(tempWorldBuffer); }

	UINT getNetBufferSize() { return (UINT)World::worldList.size(); }

	World::worldBuffer_t* getNetBufferStart() { return &World::worldList.front(); }

	/* when you are done shipping all the data across the network, reset/clear the NetBuffers */

	void clearNetBuffer() { World::worldList.clear(); }

private:

	bool race8{};

};
