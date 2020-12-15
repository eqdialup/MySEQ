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

#include "Item.h"

#include "World.h"



#pragma pack(push, 1)

struct netBuffer_t

{

	char name[30];

	float x,y,z;

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

	enum offset_types { OT_name, OT_lastname, OT_x, OT_y, OT_z, OT_speed, OT_heading,

						OT_id, OT_owner, OT_race, OT_class, OT_type, OT_level, OT_hidden,
						
						OT_primary, OT_offhand, 

						OT_prev, OT_next, OT_max };

	UINT largestOffset;

	char* rawBuffer;

	/*

	BYTES_TO_STRING(NameOffset, s, send->Name, 30);

	BYTES_TO_FLOAT(YOffset, s, send->Y);

	BYTES_TO_FLOAT(XOffset, s, send->X);

	BYTES_TO_FLOAT(ZOffset, s, send->Z);

	BYTES_TO_FLOAT(HeadingOffset, s, send->Heading);

	BYTES_TO_FLOAT(SpeedOffset, s, send->SpeedRun);

	BYTES_TO_INT(SpawnIDOffset, s, send->SpawnID);

	BYTES_TO_BYTE(TypeOffset, s, send->Type);

	BYTES_TO_BYTE(ClassOffset, s, send->Class);

	BYTES_TO_INT(RaceOffset, s, send->Race);

	BYTES_TO_BYTE(LevelOffset, s, send->Level);

	BYTES_TO_BYTE(HideOffset, s, send->Hide);

	BYTES_TO_STRING(LastnameOffset, s, send->Lastname, 22);

	*/

	netBuffer_t tempNetBuffer;

	vector<netBuffer_t> spawnList;

	

	//string name, lastname;

	//float x,y,z;

	//float speed, heading;

	//UINT id, race;

	//BYTE _class, type, level, hidden;



private:

	UINT pSpawnZero;

	string extractRawString(offset_types ot)	{ return string(&rawBuffer[offsets[ot]]); }

	float extractRawFloat(offset_types ot)		{ return *((float*) &rawBuffer[offsets[ot]]); }

	DWORD extractRawDWord(offset_types ot)		{ return *((DWORD*) &rawBuffer[offsets[ot]]); }

	WORD extractRawWord(offset_types ot)		{ return *((WORD*) &rawBuffer[offsets[ot]]); }

	int extractRawInt(offset_types ot)			{ return *((int*)	&rawBuffer[offsets[ot]]); }

public:

	BYTE extractRawByte(offset_types ot)		{ return *((BYTE*)  &rawBuffer[offsets[ot]]); }

	UINT offsets[OT_max];

	string ptrNames[OT_max];

	Spawn(void);

	void setOffset(offset_types ot, UINT value, string ptrName);

	void init(IniReaderInterface* ir_intf);

	void packNetBufferStrings(UINT flags, string firstname, string lastname);

	void packNetBufferRaw(UINT flags, UINT _this);

	void packNetBufferEmpty(UINT flags, UINT _this);

	/* when you are done filling out a NetBuffer, push it for shipping across the network */

	void pushNetBuffer()						{ spawnList.push_back(tempNetBuffer); }

	UINT getNetBufferSize()						{ return (UINT) spawnList.size(); }

	netBuffer_t* getNetBufferStart()			{ return &spawnList.front(); }

	/* when you are done shipping all the data across the network, reset/clear the NetBuffers */

	void clearNetBuffer()						{ spawnList.clear(); }

	UINT extractNextPointer()					{ return extractRawDWord(OT_next); }

	UINT extractPrevPointer()					{ return extractRawDWord(OT_prev); }

	/* convert other structures into spawn structures for shipping across the network */

	void packNetBufferFrom(Item item);

	void packNetBufferWorld(World world);
private:
	bool race8;

};

