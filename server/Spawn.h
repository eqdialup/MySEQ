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
#include "IniReader.h"
#include "Item.h"
#include "World.h"

typedef uint64_t QWORD;

#pragma pack(push, 1)

struct netBuffer_t

{
	char name[30];

	float x, y, z;

	float heading, speed;

	UINT id;

	UINT owner;

	BYTE type, _class;

	UINT race;

	BYTE level, hidden;

	UINT primary;

	UINT offhand;

	char lastName[22];

	UINT flags;
};

#pragma pack(pop)

class Spawn

{
public:

	enum offset_types {
		OT_name, OT_lastname, OT_x, OT_y, OT_z, OT_speed, OT_heading,
		OT_id, OT_owner, OT_race, OT_class, OT_type, OT_level, OT_hidden,
		OT_primary, OT_offhand, OT_prev, OT_next, OT_max
	};

	UINT largestOffset;

	char* rawBuffer{};

	netBuffer_t tempNetBuffer{};

	vector<netBuffer_t> spawnList;
	UINT offsets[OT_max]{};

	string ptrNames[OT_max];

	Spawn(void);

private:

	UINT pSpawnZero{};

	bool race8{};

	string extractRawString(offset_types ot) { return string(&rawBuffer[offsets[ot]]); }

	float extractRawFloat(offset_types ot) { return *((float*)&rawBuffer[offsets[ot]]); }

	DWORD extractRawDWord(offset_types ot) { return *((DWORD*)&rawBuffer[offsets[ot]]); }

	QWORD extractRawQWord(offset_types ot) { return *((QWORD*)&rawBuffer[offsets[ot]]); }

	WORD extractRawWord(offset_types ot) { return *((WORD*)&rawBuffer[offsets[ot]]); }

	int extractRawInt(offset_types ot) { return *((int*)&rawBuffer[offsets[ot]]); }

public:

	BYTE extractRawByte(offset_types ot) { return *((BYTE*)&rawBuffer[offsets[ot]]); }

	void setOffset(offset_types ot, UINT value, const string& ptrName);

	void init(IniReaderInterface* ir_intf);

	void packNetBufferStrings(UINT flags, const string& firstname, const string& lastname);

	void packNetBufferRaw(UINT flags, QWORD _this);

	void packNetBufferEmpty(UINT flags, QWORD _this);

	/* when you are done filling out a NetBuffer, push it for shipping across the network */

	void pushNetBuffer() { spawnList.push_back(tempNetBuffer); }

	UINT getNetBufferSize() { return (UINT)spawnList.size(); }

	netBuffer_t* getNetBufferStart() { return &spawnList.front(); }

	/* when you are done shipping all the data across the network, reset/clear the NetBuffers */

	void clearNetBuffer() { spawnList.clear(); }

	QWORD extractNextPointer() { return extractRawQWord(OT_next); }

	QWORD extractPrevPointer() { return extractRawQWord(OT_prev); }

	/* convert other structures into spawn structures for shipping across the network */

	void packNetBufferFrom(const Item& item);

	void packNetBufferWorld(const World& world);
};
