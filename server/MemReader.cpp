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

#include "MemReader.h"

#include <Psapi.h>



// Runtime debug macros

#define RTDEBUG(...) if (debug) cout << __VA_ARGS__ << endl

#define TO_LOWER(str) (transform(str.begin(), str.end(), str.begin(), (int(*)(int))tolower))



MemReader::MemReader() :

	currentEQProcessID(0),

	currentEQProcessHandle(NULL),

	currentEQProcessBaseAddress(0),

	readCount(0)

{
	currentEQProcessID = 0;
	currentEQProcessHandle = NULL;
	currentEQProcessBaseAddress = 0x400000;
}



MemReader::~MemReader()

{

	closeProcess();

}



bool MemReader::isValid()

{
	if (currentEQProcessID == 0)
		return false;

	return ( validateProcess(false) );

}



DWORD MemReader::getCurrentPID()

{

	return currentEQProcessID;

}

DWORD MemReader::getCurrentBaseAddress()

{

	return currentEQProcessBaseAddress;

}

HANDLE MemReader::getCurrentHandle()
{

	return currentEQProcessHandle;

}

void MemReader::enableDebugPrivileges()

{

	// Allows this process to peek into other another processes memory space.

	TOKEN_PRIVILEGES TP, OldTP;

	LUID ALUID;

	HANDLE hToken;

	DWORD Bufferlen;



	OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES ^ TOKEN_QUERY, &hToken);

	LookupPrivilegeValue(NULL, SE_DEBUG_NAME, &ALUID);



	TP.PrivilegeCount = 1;

	TP.Privileges[0].Luid = ALUID;

	TP.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;



	AdjustTokenPrivileges(hToken, false, &TP, sizeof(OldTP), &OldTP, &Bufferlen);

	CloseHandle(hToken);

}



/* Find the first process to match the given filename */

bool MemReader::openFirstProcess(string filename, bool debug)

{

	currentEQProcessHandle = NULL;

	currentEQProcessID = 0;

	currentEQProcessBaseAddress = 0x400000;

	return openProcess(filename, true, debug);

}



/* Find the next process to match the given filename */

bool MemReader::openNextProcess(string filename, bool debug)

{

	return openProcess(filename, false, debug);

}





/* Find the first process to match the given filename */

bool MemReader::openProcess(string filename, bool first, bool debug)

{

	HANDLE hProcessSnap = NULL; 

	PROCESSENTRY32 pe32 = {0}; 

	bool okToAttach = first;

	bool rtn = false;

	

	//  Fill in the size of the structure before using it. 

	pe32.dwSize = sizeof(PROCESSENTRY32); 

		

	RTDEBUG("Looking for process with name: " << filename);

	TO_LOWER(filename);

	

	//  Take a snapshot of all processes in the system. 

	hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0); 

	RTDEBUG("hProcessSnap is 0x" << hex << hProcessSnap);

	

	// Walk thru each process looking for the given filename

	if (Process32First(hProcessSnap, &pe32))

	{ 

		do 

		{ 

			string procExe(pe32.szExeFile);

			RTDEBUG("->Detected process with name: " << procExe);

			TO_LOWER(procExe);

			

			if (procExe.find(filename.c_str()) != string::npos )

			{

				/* During our walk thru matching processes, only attach to

				   a PID after we have seen our current PID. If this is our

				   initial PID, just attach immediately */

				if (!okToAttach)

                {

					RTDEBUG("->Match found (PID:0x" << pe32.th32ProcessID << "), but we are in 'Next' mode. Continuing search...");

					

					if (pe32.th32ProcessID == currentEQProcessID)

						okToAttach = true;

					//	currentEQProcessHandle = OpenProcess(PROCESS_VM_READ, false, currentEQProcessID);
						
					//	currentEQProcessBaseAddress = GetModuleBaseAddress( pe32.th32ProcessID, pe32.szExeFile);


					continue;
					

				}

					

				// We found a matching process that we are allowed to open

				RTDEBUG("->Match found (PID:0x" << pe32.th32ProcessID << "). Attempting to attach...");

				if (OpenProcess(PROCESS_VM_READ|PROCESS_QUERY_INFORMATION, false, pe32.th32ProcessID))

				{

					// Access granted. Stop searching and set 'current' values.

					RTDEBUG("-->Access granted.");
					
					currentEQProcessID = pe32.th32ProcessID;

					currentEQProcessHandle = OpenProcess(PROCESS_VM_READ|PROCESS_QUERY_INFORMATION, false, currentEQProcessID);

					currentEQProcessBaseAddress = GetModuleBaseAddress( pe32.th32ProcessID, pe32.szExeFile);
					
					originalFilename = procExe;

					rtn = true;

					break;

				}

				else

				{

					// Access denied. Continue search

					RTDEBUG("-->Access denied.");

					currentEQProcessID = 0;

					currentEQProcessBaseAddress = 0x400000;

					currentEQProcessHandle = NULL;

				}

			}

		} 

		while (hProcessSnap && Process32Next(hProcessSnap, &pe32)); 

	} 



	CloseHandle(hProcessSnap);

	

	if (rtn)

		cout << "MemReader: Found process ID " << dec << currentEQProcessID << " Base Address: 0x" << hex << (DWORD)currentEQProcessBaseAddress << endl;

	else if (first)

		cout << "MemReader: Failed to locate process '" << filename << "'" << endl;



	return rtn;

}





void MemReader::closeProcess()

{

	if (currentEQProcessHandle)

		CloseHandle(currentEQProcessHandle);

	currentEQProcessHandle = 0;

	currentEQProcessID = 0;

	currentEQProcessBaseAddress = 0x400000;

}



bool MemReader::validateProcess(bool forceCheck)

{

	bool stillValid = true; // only return false if we check and fail

	

	// Every 100 checks, make sure the process is still around
	// This is now called only once for each receive.

	readCount = (readCount + 1) % 100;

	if ( forceCheck || (readCount == 2) )

	{

		HANDLE         hProcessSnap = NULL; 

		PROCESSENTRY32 pe32      = {0}; 

		

		stillValid = false;

	

		//  Fill in the size of the structure before using it. 

		pe32.dwSize = sizeof(PROCESSENTRY32); 

	

		//  Take a snapshot of all processes in the system. 

		hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0); 

	

		// Walk thru each process looking for the process ID we had before

		if (Process32First(hProcessSnap, &pe32))

		{ 

			do 

			{ 

				if ( currentEQProcessID != 0 && pe32.th32ProcessID == currentEQProcessID )

				{

					string procExe(pe32.szExeFile);

					TO_LOWER(procExe);

					

					if ( procExe == originalFilename )

					{

						stillValid = true;

					}

					break;

				}

			}

			while (Process32Next(hProcessSnap, &pe32)); 

		} 

		CloseHandle(hProcessSnap);

		if ( !stillValid )

			closeProcess();

	} 

	

	return stillValid;

}



UINT MemReader::extractPointer(UINT offset)

{

	UINT rtn = 0;

	ReadProcessMemory(currentEQProcessHandle, (void*) offset,(void*) &rtn, sizeof(rtn), NULL);

	return rtn;

}

UINT MemReader::extractRAWPointer(UINT offset)

{

	UINT rtn = 0;

	ReadProcessMemory(currentEQProcessHandle, (void*) (offset - 0x400000 + currentEQProcessBaseAddress),(void*) &rtn, sizeof(rtn), NULL);

	return rtn;

}



string MemReader::extractString(UINT offset)

{

	string rtn("");

	char buffer[50];

	memset(buffer, 0, 50);

	ReadProcessMemory(currentEQProcessHandle, (void*) offset,(void*) buffer, 30, NULL);

	rtn = buffer;

	return rtn;

}

string MemReader::extractString2(UINT offset)

{
	// This one is for extracting a string that must begin with an alphanumeric
	// Zones always should begin with an alpha numeric
	string rtn("");

	char buffer[50];

	memset(buffer, 0, 50);

	ReadProcessMemory(currentEQProcessHandle, (void*) offset,(void*) buffer, 30, NULL);

	if (isalnum(buffer[0]))
		rtn = buffer;

	return rtn;

}


bool MemReader::extractToBuffer(UINT offset, char* buffer, UINT size) {
	//better check if we can actually read this much memory... -eqmule 12/31 2014
	//Basically if we ask ReadProcessMemory to read <size> bytes but the
	//region we read from is smaller than <size> we end up in a scenario where we dont get ANY
	//data read at all... tis fixes the bug where mobs wont show up on map for example...
	//it also explains why client will work on one machine but not on another...
	const int nSizeUpperBound = size;
	BYTE* lpAddressToReadFrom = (BYTE*)offset;
	MEMORY_BASIC_INFORMATION memInfo;
	ZeroMemory(&memInfo, sizeof(MEMORY_BASIC_INFORMATION));

	if(int vret = VirtualQueryEx(currentEQProcessHandle, lpAddressToReadFrom, &memInfo,sizeof(MEMORY_BASIC_INFORMATION))) {

		int nBytesIntoRegion = lpAddressToReadFrom - (BYTE*)memInfo.BaseAddress;
		int nBytesAwayFromEnd = memInfo.RegionSize - nBytesIntoRegion;
		int ActualNumberOfBytesToRead = min(nSizeUpperBound,nBytesAwayFromEnd);
		if(ActualNumberOfBytesToRead<(int)size)
			size = ActualNumberOfBytesToRead;
	}
	bool rtn = false;

	rtn = (ReadProcessMemory(currentEQProcessHandle, (void*) offset,(void*) buffer, size, NULL) != 0);
	DWORD hmm = GetLastError();
	if(rtn==false && hmm) {
		char *szError = 0;
		FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER |
			FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
			NULL,
			hmm,
			MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
			(LPTSTR) &szError,
			0,
			NULL);
		Sleep(0);
	}
	return rtn;

}



float MemReader::extractFloat(UINT offset)

{

	float rtn;

	ReadProcessMemory(currentEQProcessHandle, (void*) offset,(void*) &rtn, 4, NULL);

	return rtn;

}

BYTE MemReader::extractBYTE(UINT offset)

{

	BYTE rtn;

	ReadProcessMemory(currentEQProcessHandle, (void*) offset,(void*) &rtn, 1, NULL);

	return rtn;

}

UINT MemReader::extractUINT(UINT offset)

{

	UINT rtn;

	ReadProcessMemory(currentEQProcessHandle, (void*) offset,(void*) &rtn, 4, NULL);

	return rtn;

}

DWORD MemReader::GetModuleBaseAddress(DWORD iProcId, TCHAR* DLLName)
{
	HANDLE hSnap; // Process snapshot handle.
	MODULEENTRY32 xModule; // Module information structure.

	if ((hSnap = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, iProcId)) == INVALID_HANDLE_VALUE) // Creates a module
		return 0;

	xModule.dwSize = sizeof(MODULEENTRY32); // Needed for Module32First/Next to work.
	
	BOOL bModule = Module32First(hSnap, &xModule);
	while(bModule)
	{

		if (lstrcmpi (xModule.szModule, DLLName) == 0) // If this is the module we want...
		{
			CloseHandle(hSnap); // Free the handle.
			return (DWORD)xModule.modBaseAddr; // return the base address.
		}

		bModule = Module32Next(hSnap, &xModule); // Loops through the rest of the modules.

	}

	CloseHandle(hSnap); // Free the handle.

	return 0; // If the result of the function is 0, it didn't find the base address.
}