/*==============================================================================

	Copyright (C) 2006  All developers at http://sourceforge.net/projects/seq



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



#include <winsock2.h>

#include <stdio.h>

#include <stdlib.h>

#include <windows.h>

#include <time.h>

#include <sys/types.h>

#include <sys/timeb.h>

#include <string.h>

#include <tlhelp32.h>

#include <aclapi.h>

#include <vector>



//#define WRITE_STREAM 

//#define READ_STREAM



#define BYTES_TO_FLOAT(offset, buf, dst) (memcpy(&dst, (buf)+(offset), sizeof(float)))

#define BYTES_TO_INT(offset, buf, dst) (memcpy(&dst, (buf)+(offset), sizeof(int)))

#define BYTES_TO_BYTE(offset, buf, dst) (dst = (buf)[(offset)])

#define BYTES_TO_SHORT(offset, buf, dst) (memcpy(&dst, (buf)+(offset), sizeof(short))) 

#define BYTES_TO_STRING(offset, buf, dst, len) (memcpy(&dst, (buf)+(offset), len))



#define WARNING(msg, ...) if (!gFirstPassDone) printf("WARNING: " msg "\n", __VA_ARGS__);

#define INFO(msg, ...)    if (!gFirstPassDone) printf("INFO: " msg "\n", __VA_ARGS__);

#define DEBUG(msg, ...)   printf("DEBUG:  -> " msg "\n", __VA_ARGS__);



#define SCHEMA_SWAPXY					0x0001  // the x and y values for spawns were swapped

#define SCHEMA_PLAYERINFO_DIRECT		0x0002	// the pCharInfo value is directly visible

#define SCHEMA_WALK_BACKWARDS			0x0003	// in the 12/7/2005 patch, the spawnlist is walked using pPrev



HANDLE eqprocess;

DWORD eqProcessID;

DWORD LastProcessID;



int ListenerPort = 5555;



// Offsets in the process

int SchemaVersion = 1;

void *EQADDR_SHORTZONE = (void*)0;

void *EQADDR_CHAR  = (void*)0;

void *EQADDR_CHARINFO  = (void*)0;

void *EQADDR_SPAWNLIST  = (void*)0;

void *EQADDR_TARGET = (void*)0;

void *EQADDR_ITEMS = (void*)0;



// Offsets in the CharInfo structure

int SpawnInfoOffset = 0;



// Offsets in the spawninfo structure

int NameOffset = 0;

int XOffset = 0;

int YOffset = 0;

int ZOffset = 0;

int HeadingOffset = 0;

int SpeedOffset = 0;

int SpawnIDOffset = 0;

int TypeOffset = 0;

int ClassOffset = 0;

int RaceOffset = 0;

int LevelOffset = 0;

int HideOffset = 0;

int LastnameOffset = 0;

int NextOffset = 0;

int PrevOffset = 0;



int LargestSpawnOffset = 0;

char *spbuf;



int GINameOffset = 0;

int GIXOffset = 0;

int GIYOffset = 0;

int GIZOffset = 0;

int GISpawnIDOffset = 0;

int GINextOffset = 0;



int LargestItemOffset = 0;

char *gibuf;



char oldZone[128];



const int MAX_X_COORD = 1000000;

const int MIN_X_COORD = -1000000;

const int MAX_Y_COORD = 1000000;

const int MIN_Y_COORD = -1000000;

const int MAX_Z_COORD = 1000000;

const int MIN_Z_COORD = -1000000;

const int MAX_HEADING = 512;

const int MIN_HEADING = 0;

//TODO: check min and max values for runspeed

const int MAX_RUNSPEED = 1000000;

const int MIN_RUNSPEED = -1000000;



const int BUFFER_SIZE = 100000;



const int MAX_PROCESSES = 10;

DWORD maProcesses[MAX_PROCESSES];

int processCount=0;



//Bit Flags determining what data to send to the client

const int PACKET_TYPE_ZONE = 0x00000001;

const int PACKET_TYPE_PLAYER = 0x00000002;

const int PACKET_TYPE_TARGET = 0x00000004;

const int PACKET_TYPE_MOBS = 0x00000008;

const int PACKET_TYPE_GROUND_ITEMS = 0x00000010;

const int PACKET_TYPE_GET_PROCESSINFO = 0x00000020;

const int PACKET_TYPE_SET_PROCESSINFO = 0x00000040;



// Debug flags, constants and defines

const UINT DEBUG_WALK_TARGET  = 0x00001000; // Will attempt to walk spawnlist from pTarget

const UINT DEBUG_DUMP_SELF    = 0x00002000; // Display all self character data

const UINT DEBUG_WALK_DUMP    = 0x00004000; // Walk the spawnlist dumping offset values in given formats

const UINT DEBUG_MONITOR_TGTS = 0x00008000; // Monitor targets listed in the ini file

enum DebugWalkerFormat { DWF_NOTSET, DWF_BYTE, DWF_SHORT, DWF_DWORD, DWF_FLOAT, DWF_STRING };

#define DEBUG_MON_MAX           5			// Maximum number of allowable 'monitor' strings

#define DEBUG_MON_SIZE          50			// Maximum length of 'monitor' strings

#define DEBUG_READBUFSIZE		0x2000		// The size of the buffer to use in debug mode

#define DEBUG_WALK_MAX			1000		// Maximum number of spawns to walk



// Global flags to aid in debugging

bool gFirstPassDone = false;

UINT gDebugFlags = 0;

UINT gDebugSpawnCount=0;



// Debug test values (from ini file)

float Debug_XValue   = 0;

float Debug_YValue   = 0;

float Debug_ZValue   = 0;

int Debug_WalkFormat = 0;

int Debug_WalkOffset = 0;

int Debug_WalkIter   = 0;

int Debug_WalkSize   = 0;

int Debug_WalkLimit  = 0;

char Debug_MonMatch[DEBUG_MON_MAX][DEBUG_MON_SIZE];

char Debug_MonChange[DEBUG_MON_MAX][DEBUG_MON_SIZE];



// Prototypes

void DebugWalkTarget(UINT paddr, bool bForward);

void DebugDumpSelf(UINT paddr);

void DebugWalkSpawnsAndDump(UINT offset, DebugWalkerFormat format, UINT limit);

void DebugMonitorTargets(void);



#pragma pack(push, 1)

class SPAWNINFO_SEND {

public:

	SPAWNINFO_SEND()  {



	}

	SPAWNINFO_SEND(const SPAWNINFO_SEND &s)  {

		strncpy(Name, s.Name, 30);

		Y = s.Y;

		Z = s.Z;

		X = s.X;

		Heading  = s.Heading;

		SpeedRun = s.SpeedRun;

		SpawnID = s.SpawnID;

		Type = s.Type;

		Class = s.Class;

		Race = s.Race;

		Level = s.Level;

		Hide = s.Hide;

		strcpy(Lastname, s.Lastname);

		flags = s.flags;

	}



	SPAWNINFO_SEND &operator=(const SPAWNINFO_SEND &s)  {

		strncpy(Name, s.Name, 30);

		Y = s.Y;

		Z = s.Z;

		X = s.X;

		Heading  = s.Heading;

		SpeedRun = s.SpeedRun;

		SpawnID = s.SpawnID;

		Type = s.Type;

		Class = s.Class;

		Race = s.Race;

		Level = s.Level;

		Hide = s.Hide;

		strcpy(Lastname, s.Lastname);

		flags = s.flags;

		return *this;

	}

	CHAR Name[30];

	FLOAT Y;

	FLOAT X;

	FLOAT Z;

	FLOAT Heading;

	FLOAT SpeedRun;

	DWORD SpawnID;

	BYTE Type;							

	BYTE Class;

	DWORD Race;

	BYTE Level;

	BYTE Hide;

	CHAR Lastname[22];

	int flags;

} ;

#pragma pack(pop)



SPAWNINFO_SEND sisend;



/*typedef struct _GROUNDITEM {

struct _GROUNDITEM *pPrev;		// 0

struct _GROUNDITEM *pNext;		// 4

DWORD ID;						// 8

DWORD DropID;					// 12

DWORD Unknown;					// 16

DWORD DxID;						// 20

BYTE Unknown0024[176];			// 24

DWORD Unknown2;					// 200

FLOAT Heading;					// 204

FLOAT Z;						// 208

FLOAT X;						// 212

FLOAT Y;						// 216

CHAR Name[30];					// 220

int flags;						// 250

} GROUNDITEM, *PGROUNDITEM;

*/





bool CheckITEMtoSEND(SPAWNINFO_SEND *send)  {

	return 1;

	/*

	return ( strlen(send->Name)>0 &&

		send->X < MAX_X_COORD && send->X > MIN_X_COORD &&

		send->Y < MAX_Y_COORD && send->Y > MIN_Y_COORD &&

		send->Z < MAX_Z_COORD && send->Z > MIN_Z_COORD );

		*/

}



bool AssignITEMtoSEND(char *s, SPAWNINFO_SEND *send)  {

	BYTES_TO_STRING(GINameOffset, s, send->Name,30);

	BYTES_TO_INT(GIXOffset, s, send->X);

	BYTES_TO_INT(GIYOffset, s, send->Y);

	BYTES_TO_INT(GIZOffset, s, send->Z);

	BYTES_TO_INT(GISpawnIDOffset, s, send->SpawnID);



	// Sanity Check

	return CheckITEMtoSEND(send);

}

bool CheckSPAWNtoSEND(SPAWNINFO_SEND *send)  {

	return 1;

	/*return ( strlen(send->Name)>0 &&

		send->X < MAX_X_COORD && send->X > MIN_X_COORD &&

		send->Y < MAX_Y_COORD && send->Y > MIN_Y_COORD &&

		send->Z < MAX_Z_COORD && send->Z > MIN_Z_COORD &&

		send->Heading < MAX_HEADING && send->Heading > MIN_HEADING //&&

		//send->SpeedRun < MAX_RUNSPEED && send->SpeedRun > MIN_RUNSPEED 

		);*/

}

bool AssignSPAWNtoSEND(char *s, SPAWNINFO_SEND *send)  {

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

	



	// Sanity Check

	return CheckSPAWNtoSEND(send);

}





// For debugging, we want to see if the linked list pointers look somewhat valid

// before dereferencing them.

bool PtrSanityCheck(UINT ptr1, UINT ptr2)

{

	if ((UINT) ptr1 < 0xA00000)

		return false;

	if ((UINT) ptr2 < 0xA00000)

		return false;

	if (ptr1 == ptr2)

		return false;

	//if (abs(ptr1 - ptr2) > 0x2000000)

	//	return false;

	return true;

}





// Scan a given block of memory for a pointer value

void ScanForStaticPointer(UINT ptrBlock, UINT sizeBlock, UINT ptrToFind)

{

	UINT paddr, pTest;

	DWORD tmp;





	DEBUG("Scanning block from 0x%x to 0x%x looking for 0x%x", ptrBlock, ptrBlock + sizeBlock, ptrToFind)

	for (paddr=ptrBlock; paddr < ( ptrBlock + sizeBlock ); paddr += 4 )

	{

		if (ReadProcessMemory(eqprocess, (void*)paddr, spbuf, 8, &tmp))

		{

			BYTES_TO_INT(0, spbuf, pTest);

			if (ptrToFind == pTest)

			{

				DEBUG("Found target pointer 0x%x at 0x%x!", ptrToFind, paddr);

			}

		}

	}

	DEBUG("Block scan complete.")

}



// Scan a given block of memory for a float value

void ScanForFloat(UINT ptrBlock, UINT sizeBlock, float valToFind, const char* msg)

{

	UINT paddr;

	float valTest;

	DWORD tmp;



	DEBUG("Scanning block from 0x%x to 0x%x looking for float %f", ptrBlock, ptrBlock + sizeBlock, valToFind)

	for (paddr=ptrBlock; paddr < ( ptrBlock + sizeBlock ); paddr ++ )

	{

		if (ReadProcessMemory(eqprocess, (void*)paddr, spbuf, 4, &tmp))

		{

			BYTES_TO_FLOAT(0, spbuf, valTest);

			if (valToFind == valTest)

			{

				DEBUG("Found exact target float %f at 0x%x! Offset is 0x%x", valToFind, paddr, paddr-ptrBlock);

			}

			else if ((valToFind-valTest)*(valToFind-valTest) < 1)

			{

				DEBUG("Found close target float %f at 0x%x! Offset is 0x%x", valToFind, paddr, paddr-ptrBlock);

			}

		}

	}

	DEBUG("Block scan complete.")

}



void Debug_ExtractMonitorTargets(char pMon[DEBUG_MON_MAX][DEBUG_MON_SIZE], char* buffer)

{

	char *pContext, *pToken;

	UINT index = 0;

	

	memset(pMon, 0, DEBUG_MON_MAX*DEBUG_MON_SIZE);

	pToken = strtok_s(buffer, " ", &pContext);

	while (pToken && (index < 5))

	{

		strncpy(pMon[index++], pToken, 30);

		pToken = strtok_s(NULL, " ", &pContext);

	}

}



bool AdjustDacl(HANDLE h, DWORD DesiredAccess)

{

	// the WORLD Sid is trivial to form programmatically (S-1-1-0)

	SID world = { SID_REVISION, 1, SECURITY_WORLD_SID_AUTHORITY, 0 };



	EXPLICIT_ACCESS ea =  {

		DesiredAccess,

			SET_ACCESS,

			NO_INHERITANCE,

		{

			0, NO_MULTIPLE_TRUSTEE,

				TRUSTEE_IS_SID,

				TRUSTEE_IS_USER,

				reinterpret_cast<LPTSTR>(&world)

		}

	};

	ACL* pdacl = 0;

	DWORD err = SetEntriesInAcl(1, &ea, 0, &pdacl);

	if (err == ERROR_SUCCESS)

	{

		err = SetSecurityInfo(h, SE_KERNEL_OBJECT, DACL_SECURITY_INFORMATION, 0, 0, pdacl, 0);

		LocalFree(pdacl);

		return(err == ERROR_SUCCESS);

	}

	else

		return(FALSE);

}

void SetDebugPriv()

{

	// Doesnt seem to work properly :(

	TOKEN_PRIVILEGES TP, OldTP;

	LUID ALUID;

	HANDLE hToken;

	int lret;

	DWORD Bufferlen;



	lret = OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES ^ TOKEN_QUERY, &hToken);

	lret = LookupPrivilegeValue(NULL, SE_DEBUG_NAME, &ALUID);



	TP.PrivilegeCount = 1;

	TP.Privileges[0].Luid = ALUID;

	TP.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;



	lret = AdjustTokenPrivileges(hToken, false, &TP, sizeof(OldTP), &OldTP, &Bufferlen);

	CloseHandle(hToken);

}

void BuildProcessList()

{

	HANDLE         hProcessSnap = NULL; 

	PROCESSENTRY32 pe32      = {0}; 



	processCount = 0;



	//  Take a snapshot of all processes in the system. 

	hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0); 



	if (hProcessSnap == INVALID_HANDLE_VALUE) 

		return; 



	//  Fill in the size of the structure before using it. 

	pe32.dwSize = sizeof(PROCESSENTRY32); 



	if (Process32First(hProcessSnap, &pe32))

	{ 

		do 

		{ 

			LPSTR pCurChar;

			char pName[512];



			// strip path and leave exe filename 

			for (pCurChar = (pe32.szExeFile + strlen (pe32.szExeFile)); 

				*pCurChar != '\\' && pCurChar != pe32.szExeFile - 1;  

				--pCurChar) 



			strcpy(pName, pCurChar); 

			strlwr(pName);



			if ( (strncmp (pName, "notepad", 70) == 0) || (strncmp (pName, "eqgame", 6) == 0) )

			{

				if (processCount < MAX_PROCESSES)

				{

					maProcesses[processCount] = pe32.th32ProcessID;

					processCount++;

				}

			}

		} 

		while (Process32Next(hProcessSnap, &pe32)); 

	} 



	CloseHandle (hProcessSnap);	

	return; 

}

void CloseEqProcess()

{

	if (eqprocess != 0)

		{

			CloseHandle(eqprocess);

			eqprocess = 0;

			eqProcessID = 0;

		}

}

bool IsValidProcessID(DWORD dwProcessID)

{

	for(int i=0; i<processCount; i++)

	{

		if (dwProcessID == maProcesses[i]) 

			return true;

	}

	return false;

}

void SetActiveProcess(DWORD dwProcessID)

{

	HANDLE hProcess;



	if (eqProcessID != dwProcessID && IsValidProcessID(dwProcessID))

	{

		CloseEqProcess();

		

		hProcess = OpenProcess(PROCESS_VM_READ, FALSE, dwProcessID);

		if (hProcess == NULL)

		{

			DWORD dw;

			dw = GetLastError();

			printf ("OpenProcess failed DACL, error: %u\n", dw);

			return;

		}

		eqprocess = hProcess;

		eqProcessID = dwProcessID;

		LastProcessID = eqProcessID;

		strcpy(oldZone, "");

		printf ("found - pid = %u\n\n", dwProcessID);

	}

}



SOCKET InitListener()

{

	SOCKET sd;

	struct sockaddr_in myaddr;



	sd = socket(AF_INET, SOCK_STREAM, 0);



	if (sd == INVALID_SOCKET)  {

		int errcode = WSAGetLastError();

		char msg[20];



		if (errcode == WSANOTINITIALISED)

			strcpy(msg, "WSANOTINITIALISED");

		else if (errcode == WSAENETDOWN)

			strcpy(msg, "WSAENETDOWN");

		else if (errcode == WSAEFAULT)

			strcpy(msg, "WSAEFAULT");

		else if (errcode == WSAEINTR)

			strcpy(msg, "WSAEINTR");

		else if (errcode == WSAEINPROGRESS)

			strcpy(msg, "WSAEINPROGRESS");

		else if (errcode == WSAEINVAL)

			strcpy(msg, "WSAEINVAL");

		else if (errcode == WSAEMFILE)

			strcpy(msg, "WSAEMFILE");

		else if (errcode == WSAENOBUFS)

			strcpy(msg, "WSAENOBUFS");

		else if (errcode == WSAENOTSOCK)

			strcpy(msg, "WSAENOTSOCK");

		else if (errcode == WSAEOPNOTSUPP)

			strcpy(msg, "WSAEOPNOTSUPP");

		else if (errcode == WSAEWOULDBLOCK)

			strcpy(msg, "WSAEWOULDBLOCK");

		else

			strcpy(msg, "Unknown Error");



		printf("Error listening: %s\n", msg);

		return 0;

	}



	int yes = 1;

	setsockopt(sd, SOL_SOCKET, SO_REUSEADDR, (char*)&yes, sizeof(int));



	ZeroMemory(&myaddr, sizeof(struct sockaddr_in));



	myaddr.sin_family = AF_INET;

	myaddr.sin_port = htons(ListenerPort);

	myaddr.sin_addr.s_addr = INADDR_ANY;



	if (bind(sd, (struct sockaddr*)&myaddr, sizeof(struct sockaddr)) == SOCKET_ERROR)  {

		int errcode = WSAGetLastError();

		char msg[20];



		if (errcode == WSANOTINITIALISED)

			strcpy(msg, "WSANOTINITIALISED");

		else if (errcode == WSAENETDOWN)

			strcpy(msg, "WSAENETDOWN");

		else if (errcode == WSAEACCES)

			strcpy(msg, "WSAEACCES");

		else if (errcode == WSAEADDRINUSE)

			strcpy(msg, "WSAEADDRINUSE");

		else if (errcode == WSAEADDRNOTAVAIL)

			strcpy(msg, "WSAEADDRNOTAVAIL");

		else if (errcode == WSAEFAULT)

			strcpy(msg, "WSAEFAULT");

		else if (errcode == WSAEINPROGRESS)

			strcpy(msg, "WSAEINPROGRESS");

		else if (errcode == WSAEINVAL)

			strcpy(msg, "WSAEINVAL");

		else if (errcode == WSAENOBUFS)

			strcpy(msg, "WSAENOBUFS");

		else if (errcode == WSAENOTSOCK)

			strcpy(msg, "WSAENOTSOCK");

		else

			strcpy(msg, "Unknown Error");



		printf("Error binding socket: %s\n", msg);

		return 0;

	}



	if (listen(sd, 2) == SOCKET_ERROR)  {

		int errcode = WSAGetLastError();

		char msg[20];



		if (errcode == WSANOTINITIALISED)

			strcpy(msg, "WSANOTINITIALISED");

		else if (errcode == WSAENETDOWN)

			strcpy(msg, "WSAENETDOWN");

		else if (errcode == WSAEFAULT)

			strcpy(msg, "WSAEFAULT");

		else if (errcode == WSAEINTR)

			strcpy(msg, "WSAEINTR");

		else if (errcode == WSAEINPROGRESS)

			strcpy(msg, "WSAEINPROGRESS");

		else if (errcode == WSAEINVAL)

			strcpy(msg, "WSAEINVAL");

		else if (errcode == WSAEMFILE)

			strcpy(msg, "WSAEMFILE");

		else if (errcode == WSAENOBUFS)

			strcpy(msg, "WSAENOBUFS");

		else if (errcode == WSAENOTSOCK)

			strcpy(msg, "WSAENOTSOCK");

		else if (errcode == WSAEOPNOTSUPP)

			strcpy(msg, "WSAEOPNOTSUPP");

		else if (errcode == WSAEWOULDBLOCK)

			strcpy(msg, "WSAEWOULDBLOCK");

		else

			strcpy(msg, "Unknown Error");



		printf("Error listening: %s\n", msg);

		return 0;

	}

	return sd;

}

SOCKET WaitForConnection(SOCKET sd)

{

	SOCKET newfd;

	struct sockaddr_in theiraddr;

	int sinsize;



	sinsize = sizeof(struct sockaddr);

	printf("Waiting for new connection...\n");

	newfd = accept(sd, (struct sockaddr*)&theiraddr, &sinsize);



	if (newfd == INVALID_SOCKET)  {

		int errcode = WSAGetLastError();

		char msg[20];



		if (errcode == WSANOTINITIALISED)

			strcpy(msg, "WSANOTINITIALISED");

		else if (errcode == WSAENETDOWN)

			strcpy(msg, "WSAENETDOWN");

		else if (errcode == WSAEFAULT)

			strcpy(msg, "WSAEFAULT");

		else if (errcode == WSAEINTR)

			strcpy(msg, "WSAEINTR");

		else if (errcode == WSAEINPROGRESS)

			strcpy(msg, "WSAEINPROGRESS");

		else if (errcode == WSAEINVAL)

			strcpy(msg, "WSAEINVAL");

		else if (errcode == WSAEMFILE)

			strcpy(msg, "WSAEMFILE");

		else if (errcode == WSAENOBUFS)

				strcpy(msg, "WSAENOBUFS");

		else if (errcode == WSAENOTSOCK)

			strcpy(msg, "WSAENOTSOCK");

		else if (errcode == WSAEOPNOTSUPP)

			strcpy(msg, "WSAEOPNOTSUPP");

		else if (errcode == WSAEWOULDBLOCK)

			strcpy(msg, "WSAEWOULDBLOCK");

		else

			strcpy(msg, "Unknown Error");



		printf("Error accepting connection: %s\n", msg);

		return 0;

	}



	printf("new connection from: %s\n", inet_ntoa(theiraddr.sin_addr));

	return newfd;

}

bool PushBack(char buffer[], int *index, void *source, int size)

{

	if ((*index+size) < BUFFER_SIZE)

	{

		CopyMemory(&buffer[*index], source, size);

		*index += size;

		return true;

	}

	else

		return false;

}

void CheckAndSendZoneName(char buffer[], int *index)

{

	char memZone[128];

	SIZE_T tmp;



	if ( ReadProcessMemory(eqprocess, EQADDR_SHORTZONE, &memZone, 128, &tmp) != 0 )

	{

		if (strlen(memZone)>0)

		{

			INFO("Zonename is %s.", memZone);

			if (strnicmp(oldZone, memZone, 128) != 0)  

			{

				strcpy(oldZone, memZone);

				ZeroMemory(&sisend, sizeof(SPAWNINFO_SEND));

				strncpy(sisend.Name, memZone, 30);



				sisend.flags = 4;

				PushBack(buffer, index, &sisend, sizeof(SPAWNINFO_SEND));

			}

		}

	}

	else

	{

        WARNING("Zonename value 0x%X is incorrect.", EQADDR_SHORTZONE);

	}

}

void SendCurrentTarget(char buffer[], int *index)

{

	UINT paddr;

	SIZE_T tmp;



	// Current Target 

	if (ReadProcessMemory(eqprocess, EQADDR_TARGET, &paddr, sizeof(int), &tmp) != 0)

	{

		ReadProcessMemory(eqprocess, (void*)paddr, spbuf, LargestSpawnOffset, &tmp);

		

		// Just to test the ScanForStaticPointer function.

		//ScanForStaticPointer((((UINT)EQADDR_TARGET - 0x100000) & ~0xFFFFF), 0x200000, paddr);

		

		INFO("pTarget is 0x%X.", paddr);

		

		ZeroMemory(&sisend, sizeof(SPAWNINFO_SEND));

		if (AssignSPAWNtoSEND(spbuf, &sisend))

		{

			sisend.flags = 1;

			PushBack(buffer, index, &sisend, sizeof(SPAWNINFO_SEND));



			// Try and walk the linked list from this spawn target, displaying information as we go.

			if (!gFirstPassDone && (gDebugFlags & DEBUG_WALK_TARGET))

			{

				// First walk forward, then backward

				gDebugSpawnCount = 0;

				DebugWalkTarget(paddr, false);

				DebugWalkTarget(paddr, true);

			}

		}

	}

	else

	{

        WARNING("Current Target value 0x%X is incorrect.", EQADDR_TARGET);

	}

}



void SendPlayerInfo(char buffer[], int *index)

{

	int ptmp=0;

	int paddr;

	SIZE_T tmp;

    if (ReadProcessMemory(eqprocess, EQADDR_CHARINFO, &ptmp, sizeof(int), &tmp) != 0)

	{

		// On 11/16/2005, the player info has a direct pointer.

		if (SchemaVersion & SCHEMA_PLAYERINFO_DIRECT)

		{

			paddr = ptmp;

		}

		else

		{

			ptmp+=SpawnInfoOffset;

			ReadProcessMemory(eqprocess, (void*) (ptmp), &paddr, sizeof(int), &tmp);

		}



		INFO("pSelf is 0x%X.", paddr);



		ReadProcessMemory(eqprocess, (void*)paddr, spbuf, LargestSpawnOffset, &tmp);

		if (AssignSPAWNtoSEND(spbuf, &sisend))

		{

			sisend.flags = 253;		

			PushBack(buffer, index, &sisend, sizeof(SPAWNINFO_SEND));

		}



		if (!gFirstPassDone)

		{

			if (gDebugFlags & DEBUG_DUMP_SELF)

				DebugDumpSelf(paddr);

			

			if (gDebugFlags & DEBUG_WALK_DUMP)

			{

				int i;

				for (i=0; i<Debug_WalkIter; i++)

					DebugWalkSpawnsAndDump(Debug_WalkOffset + i, (DebugWalkerFormat) Debug_WalkFormat, Debug_WalkLimit);

			}

			

			if (gDebugFlags & DEBUG_MONITOR_TGTS)

			{

				DebugMonitorTargets();

			}

		}

	}

	else

	{

        WARNING("Player Info value 0x%X is incorrect.", EQADDR_CHARINFO);

	}

}



void SendMobData(char buffer[], int *index)

{

	int paddr;

	SIZE_T tmp;

	if (ReadProcessMemory(eqprocess, EQADDR_SPAWNLIST, &paddr, sizeof(int), &tmp) != 0)

	{

		INFO("pSpawnList is 0x%X.", paddr);

		while(true)  {

			int pNext = 0;

			if (ReadProcessMemory(eqprocess, (void*)paddr, spbuf, LargestSpawnOffset, &tmp) != 0)  {

				ZeroMemory(&sisend, sizeof(SPAWNINFO_SEND));

				if (AssignSPAWNtoSEND(spbuf, &sisend)==true)

				{

					sisend.flags = 0;

					PushBack(buffer, index, &sisend, sizeof(SPAWNINFO_SEND));

					

					// In 12/7/2005 patch, we must walk the spawnlist backwards

					if (SchemaVersion & SCHEMA_WALK_BACKWARDS)

						BYTES_TO_INT(PrevOffset, spbuf, pNext);

					else

						BYTES_TO_INT(NextOffset, spbuf, pNext);

				}

				else

				{

					pNext = 0;

				}



				if (pNext == 0)  

					break;



				paddr = pNext;

			}

			else

				break;

		}

	}

	else

	{

        WARNING("Spawn List value 0x%X is incorrect.", EQADDR_SPAWNLIST);

	}

}



// Ground Items

void SendGroundItems(char buffer[], int *index)

{

	int paddr;

	

	if (!EQADDR_ITEMS)

		return;



	if (ReadProcessMemory(eqprocess, EQADDR_ITEMS, &paddr, sizeof(int), NULL) != 0)

	{

		void *addr = (void*)paddr;

		INFO("pGroundItems is 0x%X.", paddr);

		while (true)

		{

			if (ReadProcessMemory(eqprocess, addr, gibuf, LargestItemOffset, NULL) != 0)  {

				int pNext = 0;

				ZeroMemory(&sisend, sizeof(SPAWNINFO_SEND));

				if (AssignITEMtoSEND(gibuf, &sisend)== true)

				{

					sisend.flags  = 5;



					PushBack(buffer, index, &sisend, sizeof(SPAWNINFO_SEND));



					BYTES_TO_INT(GINextOffset, gibuf, pNext);

				}

				else

				{

					pNext = 0;

				}

				if (pNext == 0)

					break;



				addr = (void*)pNext;



			}

			else

				break;

		}

	}

	else

	{

        WARNING("Ground Items value 0x%X is incorrect.", EQADDR_ITEMS);

	}

}

void SendProcessInfo(char buffer[], int *index)

{

	HANDLE hProcess;

	DWORD dwProcessID;



	BuildProcessList();



	for(int i=0;i<=processCount;i++)

	{

		if (i==0)

		{

			dwProcessID = eqProcessID;

		}

		else

		{

			dwProcessID = maProcesses[i-1];

		}

		if (dwProcessID != 0)

			{

			hProcess = OpenProcess (PROCESS_VM_READ, FALSE, dwProcessID);

			if (hProcess == NULL)

			{

				DWORD dw;

				dw = GetLastError();

				printf ("OpenProcess failed DACL, error: %u\n", dw);

				return;

				

			}

					

			int pCharInfo=0;

			int pSpawnInfo=0;

			SIZE_T tmp;

			if (ReadProcessMemory(hProcess, EQADDR_CHARINFO, &pCharInfo, sizeof(int), &tmp) != 0)  

			{

				if (ReadProcessMemory(hProcess, (void*)(pCharInfo+SpawnInfoOffset), &pSpawnInfo, sizeof(int), &tmp)!= 0)

				{



					if (ReadProcessMemory(hProcess, (void*)pSpawnInfo, spbuf, LargestSpawnOffset, &tmp)!=0)

					{

						if (AssignSPAWNtoSEND(spbuf, &sisend))

						{

							sisend.flags = 6;		

							sisend.SpawnID = dwProcessID;

							sisend.Level = i;

						}

					}

					else

					{

						strcpy(sisend.Name, "(Unknown)");

						sisend.flags = 6;		

						sisend.SpawnID = dwProcessID;

						sisend.Level = i;

					}

					PushBack(buffer, index, &sisend, sizeof(SPAWNINFO_SEND));



				}

			}

			CloseHandle(hProcess);

		}

	}

	

	return; 

}

void SvrLoop()

{

	SOCKET sd, newfd;

	char buffer[BUFFER_SIZE];

	int bufferIndex;

	int ret;

	int packetcount = 0;

	bool done;





	// Setup Listening Socket

	sd = InitListener();

	if (!sd) return;



	while (1)  {

		// Wait For connection

		newfd = WaitForConnection(sd);



		int iRetry = 0;

		do {

			Sleep(200);

			iRetry++;



			BuildProcessList();

			if (processCount>0)

			{

				if (IsValidProcessID(LastProcessID))

				{	// If the Last eq process we used is still valid reconnect to that

					SetActiveProcess(LastProcessID);

				}

				else

				{   // else reconnect to the first process we found

					SetActiveProcess(maProcesses[0]);

				}

			}



		} while( eqprocess == 0 && iRetry < 10);



		if (eqprocess == 0)

		{

			printf("Failed to detect everquest process - closing socket.\n");

			closesocket(newfd);

			continue;

		}



		//reset the connection specific variables

		strcpy(oldZone, "");

		packetcount = 0;

		done = false;

		gFirstPassDone = false;



		while (!done)  {



			// Wait for request...

			int req = 0;

			ret = recv(newfd, (char*)&req, sizeof(req), 0);

			if (ret == 0 || ret == SOCKET_ERROR || ret != sizeof(req))  // Wait for request

			{

				done = true;

				continue;

			}

			

			bufferIndex = sizeof(int);





			// Check if we are changing the Process FIRST

			// - so we dont send info for the wrong process...

			if ((req & PACKET_TYPE_GET_PROCESSINFO) || (req & PACKET_TYPE_SET_PROCESSINFO))

			{

				

				if (req & PACKET_TYPE_SET_PROCESSINFO)

				{

					DWORD newProcID;

					// Change Process 

				

					// Get the Requested ProcessID From the packet Stream

					ret = recv(newfd,(char*)&newProcID, sizeof(newProcID), 0);



					// Set the active process (If it is a valid EQ process)

					SetActiveProcess(newProcID);

				}



				// Send an updated process List

				SendProcessInfo(buffer,&bufferIndex);

			}



			if (req & PACKET_TYPE_ZONE)

			{

				CheckAndSendZoneName(buffer,&bufferIndex);

			}

		

			if (req & PACKET_TYPE_PLAYER)

			{

				SendPlayerInfo(buffer,&bufferIndex);

			}



			if (req & PACKET_TYPE_TARGET)

			{

				SendCurrentTarget(buffer,&bufferIndex);

			}

			

			if (req & PACKET_TYPE_MOBS)

			{

				SendMobData(buffer,&bufferIndex);

			}



			if (req & PACKET_TYPE_GROUND_ITEMS)

			{

				SendGroundItems(buffer,&bufferIndex);

			}



			int ItemCount =0;

			ItemCount = (bufferIndex - sizeof(bufferIndex)) / sizeof(SPAWNINFO_SEND);

			memcpy(&buffer[0],&ItemCount,sizeof(ItemCount));



			if (!done)  {

				packetcount++;



				ret = send(newfd, buffer, bufferIndex, 0);

				if (ret < 0)

					done = true;



				if (ret < bufferIndex)

					printf("Incomplete send!\n");



			}

			

			gFirstPassDone = true;

		}

		try

		{

			closesocket(newfd);

		}

		catch (char *str)

		{

		}

		newfd = 0;

	}

}



void FindLargestSpawnOffset()  {

	LargestSpawnOffset = NameOffset+22;



	if (XOffset > LargestSpawnOffset)

		LargestSpawnOffset = XOffset+4;

	if (YOffset > LargestSpawnOffset)

		LargestSpawnOffset = YOffset+4;

	if (ZOffset > LargestSpawnOffset)

		LargestSpawnOffset = ZOffset+4;

	if (HeadingOffset > LargestSpawnOffset)

		LargestSpawnOffset = HeadingOffset+4;

	if (SpeedOffset> LargestSpawnOffset)

		LargestSpawnOffset = SpeedOffset+4;

	if (SpawnIDOffset > LargestSpawnOffset)

		LargestSpawnOffset = SpawnIDOffset+4;

	if (TypeOffset > LargestSpawnOffset)

		LargestSpawnOffset = TypeOffset+1;

	if (ClassOffset > LargestSpawnOffset)

		LargestSpawnOffset = ClassOffset+1;

	if (RaceOffset > LargestSpawnOffset)

		LargestSpawnOffset = RaceOffset+4;

	if (LevelOffset > LargestSpawnOffset)

		LargestSpawnOffset = LevelOffset+1;

	if (HideOffset > LargestSpawnOffset)

		LargestSpawnOffset = HideOffset+1;

	if (LastnameOffset > LargestSpawnOffset)

		LargestSpawnOffset = LastnameOffset+30;

	if (NextOffset > LargestSpawnOffset)

		LargestSpawnOffset = NextOffset+4;

	// In any debug mode, use a large buffer 

	if (gDebugFlags)

		LargestSpawnOffset = DEBUG_READBUFSIZE;

	

	spbuf = new char[LargestSpawnOffset];

}



void FindLargestItemOffset()  {

	LargestItemOffset = GINameOffset+31;



	if (GIXOffset > LargestItemOffset)

		LargestItemOffset = GIXOffset+4;

	if (GIYOffset > LargestItemOffset)

		LargestItemOffset = GIYOffset+4;

	if (GIZOffset > LargestItemOffset)

		LargestItemOffset = GIZOffset+4;

	if (GISpawnIDOffset > LargestItemOffset)

		LargestItemOffset = GISpawnIDOffset+4;

	if (GINextOffset > LargestItemOffset)

		LargestItemOffset = GINextOffset+4;



	gibuf = new char[LargestItemOffset];

}

void *SettingToVoidPtr(char *Setting)

{

	if (Setting[0] == '0')

	{

		return (void*)strtoul(Setting,NULL,16);

	}

	else

	{

		return (void*)atoi(Setting);

	}

}



int SettingToInt(char *Setting)

{

	if (Setting[0] == '0')

	{

		return strtol(Setting,NULL,16);

	}

	else

	{

		return atoi(Setting);

	}

}



float SettingToFloat(char *Setting)

{

	float rtn;



	if (sscanf(Setting, "%f", &rtn) != 1)

		rtn = 0.0;

	

	return rtn;

}



bool LoadSettings(char *filename)  {

	FILE *file = NULL;

	char buffer[255];



	if ((file= fopen(filename, "rt")) == NULL)  {

		printf("Error loading settings - could not find file %s\n", filename);

		return false;

	}

	fclose(file);



	GetPrivateProfileString("File Info", "SchemaVersion", "1", buffer, 255, filename);

	SchemaVersion = SettingToInt(buffer);



	GetPrivateProfileString("Port", "Port", "0", buffer, 255, filename);

	ListenerPort = SettingToInt(buffer);

	if(ListenerPort <= 0) 

	{

		ListenerPort = 5555;

	}



	GetPrivateProfileString("Memory Offsets", "ZoneAddr", "0", buffer, 255, filename);

	EQADDR_SHORTZONE = SettingToVoidPtr(buffer);



	GetPrivateProfileString("Memory Offsets", "CharAddr", "0", buffer, 255, filename);

	EQADDR_CHAR = SettingToVoidPtr(buffer);



	GetPrivateProfileString("Memory Offsets", "SpawnHeaderAddr", "0", buffer, 255, filename);

	EQADDR_SPAWNLIST = SettingToVoidPtr(buffer);



	GetPrivateProfileString("Memory Offsets", "TargetAddr", "0", buffer, 255, filename);

	EQADDR_TARGET = SettingToVoidPtr(buffer);



	GetPrivateProfileString("Memory Offsets", "ItemsAddr", "0", buffer, 255, filename);

	EQADDR_ITEMS = SettingToVoidPtr(buffer);



	GetPrivateProfileString("SpawnInfo Offsets", "NameOffset", "0", buffer, 255, filename);

	NameOffset = SettingToInt(buffer);



	if (SchemaVersion & SCHEMA_SWAPXY)

	{

		/*

		// The original Offset files had the X + Y offsets reversed

		// and the code in the Client was confusing as a result

		// I now fix this problem when reading in the settings from the file.

		// For schema version 2+ the values will be the right way round...

		*/

		GetPrivateProfileString("SpawnInfo Offsets", "XOffset", "0", buffer, 255, filename);

		YOffset = SettingToInt(buffer);



		GetPrivateProfileString("SpawnInfo Offsets", "YOffset", "0", buffer, 255, filename);

		XOffset = SettingToInt(buffer);

	}

	else

	{

		GetPrivateProfileString("SpawnInfo Offsets", "XOffset", "0", buffer, 255, filename);

		XOffset = SettingToInt(buffer);



		GetPrivateProfileString("SpawnInfo Offsets", "YOffset", "0", buffer, 255, filename);

		YOffset = SettingToInt(buffer);

	}





	GetPrivateProfileString("SpawnInfo Offsets", "ZOffset", "0", buffer, 255, filename);

	ZOffset = SettingToInt(buffer);



	GetPrivateProfileString("SpawnInfo Offsets", "HeadingOffset", "0", buffer, 255, filename);

	HeadingOffset = SettingToInt(buffer);



	GetPrivateProfileString("SpawnInfo Offsets", "SpeedOffset", "0", buffer, 255, filename);

	SpeedOffset = SettingToInt(buffer);



	GetPrivateProfileString("SpawnInfo Offsets", "SpawnIDOffset", "0", buffer, 255, filename);

	SpawnIDOffset = SettingToInt(buffer);



	GetPrivateProfileString("SpawnInfo Offsets", "TypeOffset", "0", buffer, 255, filename);

	TypeOffset = SettingToInt(buffer);



	GetPrivateProfileString("SpawnInfo Offsets", "ClassOffset", "0", buffer, 255, filename);

	ClassOffset = SettingToInt(buffer);



	GetPrivateProfileString("SpawnInfo Offsets", "RaceOffset", "0", buffer, 255, filename);

	RaceOffset = SettingToInt(buffer);



	GetPrivateProfileString("SpawnInfo Offsets", "HideOffset", "0", buffer, 255, filename);

	HideOffset = SettingToInt(buffer);



	GetPrivateProfileString("SpawnInfo Offsets", "LastnameOffset", "0", buffer, 255, filename);

	LastnameOffset = SettingToInt(buffer);



	GetPrivateProfileString("SpawnInfo Offsets", "NextOffset", "0", buffer, 255, filename);

	NextOffset = SettingToInt(buffer);

	

	GetPrivateProfileString("SpawnInfo Offsets", "PrevOffset", "0", buffer, 255, filename);

	PrevOffset = SettingToInt(buffer);



	GetPrivateProfileString("SpawnInfo Offsets", "LevelOffset", "0", buffer, 255, filename);

	LevelOffset = SettingToInt(buffer);



	GetPrivateProfileString("GroundItem Offsets", "NameOffset", "0", buffer, 255, filename);

	GINameOffset = SettingToInt(buffer);



	GetPrivateProfileString("GroundItem Offsets", "XOffset", "0", buffer, 255, filename);

	GIXOffset = SettingToInt(buffer);



	GetPrivateProfileString("GroundItem Offsets", "YOffset", "0", buffer, 255, filename);

	GIYOffset = SettingToInt(buffer);



	GetPrivateProfileString("GroundItem Offsets", "ZOffset", "0", buffer, 255, filename);

	GIZOffset = SettingToInt(buffer);



	GetPrivateProfileString("GroundItem Offsets", "SpawnIDOffset", "0", buffer, 255, filename);

	GISpawnIDOffset = SettingToInt(buffer);



	GetPrivateProfileString("GroundItem Offsets", "NextOffset", "0", buffer, 255, filename);

	GINextOffset = SettingToInt(buffer);



	GetPrivateProfileString("Memory Offsets", "CharInfo", "0", buffer, 255, filename);

	EQADDR_CHARINFO = SettingToVoidPtr(buffer);



	GetPrivateProfileString("CharInfo Offsets", "SpawnInfo", "0", buffer, 255, filename);

	SpawnInfoOffset = SettingToInt(buffer);



	GetPrivateProfileString("Debug", "Flags", "0", buffer, 255, filename);

	gDebugFlags |= SettingToInt(buffer);

	if (gDebugFlags)

		INFO("gDebugFlags detected: 0x%X.", gDebugFlags);



	GetPrivateProfileString("Debug", "XValue", "0", buffer, 255, filename);

	Debug_XValue = SettingToFloat(buffer);

	

	GetPrivateProfileString("Debug", "YValue", "0", buffer, 255, filename);

	Debug_YValue = SettingToFloat(buffer);

	

	GetPrivateProfileString("Debug", "ZValue", "0", buffer, 255, filename);

	Debug_ZValue = SettingToFloat(buffer);



	GetPrivateProfileString("Debug", "WalkFormat", "0", buffer, 255, filename);

	Debug_WalkFormat = SettingToInt(buffer);



	GetPrivateProfileString("Debug", "WalkOffset", "0", buffer, 255, filename);

	Debug_WalkOffset = SettingToInt(buffer);



	GetPrivateProfileString("Debug", "WalkIter", "0", buffer, 255, filename);

	Debug_WalkIter = SettingToInt(buffer);



	GetPrivateProfileString("Debug", "WalkSize", "0", buffer, 255, filename);

	Debug_WalkSize = SettingToInt(buffer);

	if (Debug_WalkSize == 0)

		Debug_WalkSize = DEBUG_WALK_MAX;



	GetPrivateProfileString("Debug", "WalkLimit", "0", buffer, 255, filename);

	Debug_WalkLimit = SettingToInt(buffer);



	GetPrivateProfileString("Debug", "MonitorForMatch", "0", buffer, 255, filename);

	Debug_ExtractMonitorTargets(Debug_MonMatch, buffer);

	

	GetPrivateProfileString("Debug", "MonitorForChange", "0", buffer, 255, filename);

	Debug_ExtractMonitorTargets(Debug_MonChange, buffer);

		

	FindLargestSpawnOffset();

	FindLargestItemOffset();



	return true;

}



// Try and walk the linked list from this spawn target, displaying information as we go.

void DebugWalkTarget(UINT paddr, bool bForward)

{

	UINT pNext, pPrev;

	DWORD tmp;

	int max = DEBUG_WALK_MAX;

	

	if (bForward)

		DEBUG("Walking pTarget forward")

	else

		DEBUG("Walking pTarget backward")

	

	while (max--)

	{

		gDebugSpawnCount++;

		DEBUG("pTarget is 0x%X.", paddr);

		DEBUG("pTarget->name is %s", sisend.Name);

		BYTES_TO_INT(NextOffset, spbuf, pNext);

		BYTES_TO_INT(PrevOffset, spbuf, pPrev);

		DEBUG("pTarget->prev is 0x%x", pPrev);

		DEBUG("pTarget->next is 0x%x", pNext);

		

		// Walk to next target

		if (bForward)

		{

			// Do some sanity checking on the pNext pointer first

			if (!PtrSanityCheck(paddr,pNext))

			{

				DEBUG("pNext appears invalid. Aborting walk with %i hits.", gDebugSpawnCount);

				// Example: If the static pointer is 0x906240, we want to scan from

				//    0x800000 - 0xA00000.

				ScanForStaticPointer((((UINT)EQADDR_TARGET - 0x100000) & ~0xFFFFF), 0x200000, paddr);

				break;

			}

			ReadProcessMemory(eqprocess, (void*)pNext, spbuf, LargestSpawnOffset, &tmp);

			paddr = pNext;

		}

		else

		{

			// Do some sanity checking on the pPrev pointer first

			if (!PtrSanityCheck(paddr,pPrev))

			{

				DEBUG("pPrev appears invalid. Aborting walk with %i hits.", gDebugSpawnCount);

				// Example: If the static pointer is 0x906240, we want to scan from

				//    0x800000 - 0xA00000.

				ScanForStaticPointer((((UINT)EQADDR_TARGET - 0x100000) & ~0xFFFFF), 0x200000, paddr);

				break;

			}					

			ReadProcessMemory(eqprocess, (void*)pPrev, spbuf, LargestSpawnOffset, &tmp);

			paddr = pPrev;

		}

		

		// Stuff it into the global sisend buffer, even though we won't send it anywhere

		ZeroMemory(&sisend, sizeof(SPAWNINFO_SEND));

		AssignSPAWNtoSEND(spbuf, &sisend);

	}

}





// Display details about yourself as parsed by the offsets in the ini file

void DebugDumpSelf(UINT paddr)

{

	DEBUG("pTarget->class is %i",		sisend.Class)

    DEBUG("pTarget->heading is %f",		sisend.Heading)

	DEBUG("pTarget->hide is %i",		sisend.Hide)

	DEBUG("pTarget->lastname is %s",	sisend.Lastname)

	DEBUG("pTarget->level is %i",		sisend.Level)

	DEBUG("pTarget->name is %s",		sisend.Name)

	DEBUG("pTarget->race is %l",		sisend.Race)

	DEBUG("pTarget->id is %l",			sisend.SpawnID)

	DEBUG("pTarget->speed is %f",		sisend.SpeedRun)

	DEBUG("pTarget->type is %i",		sisend.Type)

	DEBUG("pTarget->x is %f",			sisend.X)

	DEBUG("pTarget->y is %f",			sisend.Y)

	DEBUG("pTarget->z is %f",			sisend.Z)



	ScanForFloat(paddr, DEBUG_READBUFSIZE, Debug_XValue, "X offset");

	ScanForFloat(paddr, DEBUG_READBUFSIZE, Debug_YValue, "Y offset");

	ScanForFloat(paddr, DEBUG_READBUFSIZE, Debug_ZValue, "Z offset");

}



// Walk spawnlist and dump info at a given offset for each spawn. If limit is non-zero,

// then only values lower than it will be displayed for certain formats.

void DebugWalkSpawnsAndDump(UINT offset, DebugWalkerFormat format, UINT limit)

{

	UINT paddr, pNext, max;

	DWORD tmp;

	DWORD out;

	char strout[50];

	bool limitBroken;

	

	if (format == DWF_NOTSET)

		return;

	

	memset(strout,0,50);

	limitBroken = false;

	max = Debug_WalkSize; // Never walk more than the requested number of spawns.

	if (ReadProcessMemory(eqprocess, EQADDR_SPAWNLIST, &paddr, sizeof(int), &tmp) != 0)

	{

		while(!limitBroken && (max > 0) && (paddr != 0))

		{

			if (ReadProcessMemory(eqprocess, (void*)paddr, spbuf, LargestSpawnOffset, &tmp) != 0)

			{

				ZeroMemory(&sisend, sizeof(SPAWNINFO_SEND));

				if (AssignSPAWNtoSEND(spbuf, &sisend))

				{

					out = 0;

					max--;

					// Stuff 'out' with properly formatted data

					switch (format)

					{

						case DWF_BYTE:

							BYTES_TO_BYTE(offset, spbuf, out);

							if ( !limit || out < limit )

								DEBUG("Offset 0x%x Spawn(%s) Format BYTE gives %i", offset, sisend.Name, out)

							else

								limitBroken = true;

							break;

						case DWF_SHORT:

							BYTES_TO_SHORT(offset, spbuf, out);

							if ( !limit || out < limit )

								DEBUG("Offset 0x%x Spawn(%s) Format SHORT gives %i", offset, sisend.Name, out)

							else

								limitBroken = true;

							break;

						case DWF_DWORD:

							BYTES_TO_INT(offset, spbuf, out);

							if ( !limit || out < limit )

								DEBUG("Offset 0x%x Spawn(%s) Format INT gives %i", offset, sisend.Name, out)

							else

								limitBroken = true;

							break;

						case DWF_FLOAT:

							BYTES_TO_FLOAT(offset, spbuf, out);

							if ( !limit || out < limit )

								DEBUG("Offset 0x%x Spawn(%s) Format FLOAT gives %f", offset, sisend.Name, out)

							else

								limitBroken = true;

							break;

						case DWF_STRING:

							BYTES_TO_STRING(offset, spbuf, strout, 30);

							DEBUG("Offset 0x%x Spawn(%s) Format STRING gives %s", offset, sisend.Name, strout);

							break;

					}



					// In 12/7/2005 patch, we must walk the spawnlist backwards

					if (SchemaVersion & SCHEMA_WALK_BACKWARDS)

						BYTES_TO_INT(PrevOffset, spbuf, pNext);

					else

						BYTES_TO_INT(NextOffset, spbuf, pNext);

				}

				else

				{

					pNext = 0;

				}



				paddr = pNext;

			}

			else

			{

				paddr = 0;

			}

		}

		if (!limitBroken)

			DEBUG("Offset 0x%x did not break the limit of %i !", offset, limit)

		else

			DEBUG("Offset 0x%x Spawn(%s) broke the limit of %i !", offset, sisend.Name, limit)

	}

}



// Walk the spawnlist looking for matches to the Monitor target names.

// For each matched name in the MonitorForMatch, check to see what offsets contain

// matching data for all targets, and display the results.

// For each matched name in the MonitorForChange, check to see what offsets contain

// unique data for all targets, and display the results.

void DebugMonitorTargets()

{

	UINT paddr, pNext, max, i;

	class MonBuffer

	{

	public:

		bool valid;

		char pBuffer[DEBUG_READBUFSIZE];

		MonBuffer() : valid(false)

		{

			memset(pBuffer, 0xEE, DEBUG_READBUFSIZE);

		};

	};

	MonBuffer monMatch[DEBUG_MON_MAX], monChange[DEBUG_MON_MAX];

	DWORD tmp;

	bool performScan = false;

		

	max = DEBUG_WALK_MAX;

	

	if (ReadProcessMemory(eqprocess, EQADDR_SPAWNLIST, &paddr, sizeof(int), &tmp) != 0)

	{

		while((max > 0) && (paddr != 0))

		{

			if (ReadProcessMemory(eqprocess, (void*)paddr, spbuf, DEBUG_READBUFSIZE, &tmp) != 0)

			{

				ZeroMemory(&sisend, sizeof(SPAWNINFO_SEND));

				if (AssignSPAWNtoSEND(spbuf, &sisend))

				{

					max--;

					

					for (i=0; i<DEBUG_MON_MAX; i++)

					{

						if (strcmp(sisend.Name,Debug_MonMatch[i]) == 0)

						{

							memcpy(monMatch[i].pBuffer, spbuf, DEBUG_READBUFSIZE);

							monMatch[i].valid=true;

							performScan = true;

							DEBUG("Monitor Match target (%s) is at 0x%x", sisend.Name, paddr);

						}

						if (strcmp(sisend.Name,Debug_MonChange[i]) == 0)

						{

							memcpy(monChange[i].pBuffer, spbuf, DEBUG_READBUFSIZE);

							monChange[i].valid=true;

							performScan = true;

							DEBUG("Monitor Change target (%s) is at 0x%x", sisend.Name, paddr);

						}

					}

					

					// In 12/7/2005 patch, we must walk the spawnlist backwards

					if (SchemaVersion & SCHEMA_WALK_BACKWARDS)

						BYTES_TO_INT(PrevOffset, spbuf, pNext);

					else

						BYTES_TO_INT(NextOffset, spbuf, pNext);

				}

				else

				{

					pNext = 0;

				}

				paddr = pNext;

			}

			else

			{

				paddr = 0;

			}

		}

	}



	if (performScan)

	{

		char match1, match2;

		UINT change1, change2, changeMax;

		UINT j;

		bool matchPassed, match1Set, match2Set, changePassed, change1Set, change2Set;

		

		for (j=Debug_WalkOffset; j < (Debug_WalkOffset + Debug_WalkIter); j++)

		{

			matchPassed = changePassed = true;

			match1Set = match2Set = false;

			change1Set = change2Set = false;

			changeMax = 0;



			for (i=0; i<DEBUG_MON_MAX; i++)

			{

				// Check with MonMatch targets every byte

				if (monMatch[i].valid)

				{

					if (!match1Set)

					{

						match1Set = true;

						BYTES_TO_BYTE(j, monMatch[i].pBuffer, match1);

					}

					else

					{

						match2Set = true;

						BYTES_TO_BYTE(j, monMatch[i].pBuffer, match2);

						if (match1 != match2)

						{

							matchPassed = false;

						}

					}

				}



				// Check with MonChange targets every 4 bytes

				if (monChange[i].valid)

				{

					if (j%4 == 0)

					{

						if (!change1Set)

						{

							change1Set = true;

							BYTES_TO_INT(j, monChange[i].pBuffer, change1);

						}

						else

						{

							change2Set = true;

							BYTES_TO_INT(j, monChange[i].pBuffer, change2);

							if (change1 == change2)

							{

								changePassed = false;

							}

							else

							{

								if (abs(change1-change2)> changeMax)

									changeMax=abs(change1-change2);

							}

						}

					}

				}

			}

			if (!match1Set || !match2Set)

			{

				matchPassed = false;

				if (j==0)

					DEBUG("Less than two Match targets found! Check MonitorForMatch setting.");

			}

			if (!change1Set || !change2Set)

			{

				changePassed = false;

				if (j==0)

					DEBUG("Less than two Change targets found! Check MonitorForChange setting.");

			}

			if (matchPassed)

			{

				DEBUG("Monitor Match at offset 0x%x had values of 0x%x", j, (match1&0xff));

			}

			if (j%4==0 && changePassed)

			{

				DEBUG("Monitor Change at offset 0x%x had max delta of 0x%x", j, changeMax);

			}

		}

	}

}



int main(int argc, char *argv[])  

{

	WSADATA wsa;



	INFO("\t========================\n"

		 "\t  MySEQ Open v1.18.2    \n"

		 "\t========================\n");

	INFO("\tThis software is covered under the GNU Public License (GPL)\n");



	if (WSAStartup(MAKEWORD(1,1), &wsa) != 0)  {

		return 1;

	}



	if (argc > 1)

	{

		 sscanf(argv[1], "%x", &gDebugFlags);			

	}



	if (LoadSettings("./myseqserver.ini"))  {

		SetDebugPriv();

		SvrLoop();



	}



	WSACleanup();



	return 0;

}



