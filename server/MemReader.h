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

#include <tlhelp32.h>



// The interface classes can be extended, but never changed! They force backwards compatibility.

class MemReaderInterface

{

public:

	virtual bool isValid() = 0;

	virtual bool openFirstProcess(string filename, bool debug = false) = 0;

	virtual bool openNextProcess(string filename, bool debug = false) = 0;

	virtual UINT extractPointer(UINT offset) = 0;

	virtual UINT extractRAWPointer(UINT offset) = 0;

	virtual string extractString(UINT offset) = 0;

	virtual string extractString2(UINT offset) = 0;

	virtual bool extractToBuffer(UINT offset, char* buffer, UINT size) = 0;

	virtual DWORD getCurrentPID() = 0;

	virtual DWORD getCurrentBaseAddress() = 0;

	virtual HANDLE getCurrentHandle() = 0;

	virtual float extractFloat(UINT offset) = 0;

	virtual BYTE extractBYTE(UINT offset) = 0;

	virtual UINT extractUINT(UINT offset) = 0;

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

	DWORD	currentEQProcessBaseAddress;

public:

	MemReader();

	~MemReader();

	bool isValid();

	void enableDebugPrivileges();

	bool openFirstProcess(string filename, bool debug = false);

	bool openNextProcess(string filename, bool debug = false);

	void closeProcess();

	bool validateProcess(bool forceCheck);

	UINT extractPointer(UINT offset);

	UINT extractRAWPointer(UINT offset);

	string extractString(UINT offset);

	string extractString2(UINT offset);

	bool extractToBuffer(UINT offset, char* buffer, UINT size);

	DWORD getCurrentPID();

	DWORD getCurrentBaseAddress();

	HANDLE getCurrentHandle();

	float extractFloat(UINT offset);

	BYTE extractBYTE(UINT offset);

	UINT extractUINT(UINT offset);

	bool AdjustPrivileges();

	DWORD MemReader::GetModuleBaseAddress(DWORD iProcId, TCHAR* DLLName);

};

