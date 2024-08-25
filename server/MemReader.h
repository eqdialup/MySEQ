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


typedef uint64_t QWORD;

// The interface classes can be extended, but never changed! They force backwards compatibility.
class MemReaderInterface
{
public:
	virtual bool isValid() = 0;

	virtual bool openFirstProcess(string filename, bool debug = false) = 0;

	virtual bool openNextProcess(string filename, bool debug = false) = 0;

	virtual QWORD extractPointer(QWORD offset) = 0;

	virtual QWORD extractRAWPointer(QWORD offset) = 0;

	virtual string extractString(QWORD offset) = 0;

	virtual string extractString2(QWORD offset) = 0;

	virtual bool extractToBuffer(QWORD offset, char* buffer, UINT size) = 0;

	virtual DWORD getCurrentPID() = 0;

	virtual QWORD getCurrentBaseAddress() = 0;

	virtual HANDLE getCurrentHandle() = 0;

	virtual float extractFloat(QWORD offset) = 0;

	virtual BYTE extractBYTE(QWORD offset) = 0;

	virtual UINT extractUINT(QWORD offset) = 0;
};

class MemReader : public MemReaderInterface
{
private:
	string	originalFilename;

	//HANDLE 	currentEQProcessHandle;

	//DWORD	currentEQProcessID;

	//DWORD	currentEQProcessBaseAddress;

	UINT	readCount;

	bool openProcess(string filename, bool first, bool debug);

protected:
	HANDLE 	currentEQProcessHandle;

	DWORD	currentEQProcessID;

	QWORD	currentEQProcessBaseAddress;

public:
	MemReader();

	~MemReader();

	bool isValid();

	void enableDebugPrivileges();

	bool openFirstProcess(string filename, bool debug = false);

	bool openNextProcess(string filename, bool debug = false);

	void closeProcess();

	bool validateProcess(bool forceCheck);

	QWORD extractPointer(QWORD offset);

	QWORD extractRAWPointer(QWORD offset);

	string extractString(QWORD offset);

	string extractString2(QWORD offset);

	bool extractToBuffer(QWORD offset, char* buffer, UINT size);

	DWORD getCurrentPID();

	QWORD getCurrentBaseAddress();

	HANDLE getCurrentHandle();

	float extractFloat(QWORD offset);

	BYTE extractBYTE(QWORD offset);

	UINT extractUINT(QWORD offset);

	bool AdjustPrivileges();

	QWORD MemReader::GetModuleBaseAddress(DWORD iProcId, TCHAR* DLLName);
};
