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
#include "MemReader.h"
#include "Spawn.h"
#include "Item.h"
#include "World.h"

#include <vector>
#include <string>
#include <sstream>
#include <stdexcept>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <windows.h>
#include <iostream>
#include <stdlib.h>
#include <IPHlpApi.h>


#include "resource.h"

class NetworkServerInterface
{
public:
	virtual UINT current_offset(int type) = 0;
};

class NetworkServer : public NetworkServerInterface
{
public:
	enum OffsetType { OT_zonename, OT_spawnlist, OT_self, OT_target, OT_ground, OT_world, OT_max };
	DWORD current_offset(OffsetType type);

private:
	int port;
	SOCKET sockListener;
	SOCKET sockClient;
	struct sockaddr sockAddr;
	sockaddr_in* psockAddrIn;
	size_t sockAddrSize;
	int clientRequest;
	bool change_process;
	int loopCount;
	QWORD offsets[OT_max];
	bool quickInfo;
	std::string zoneName;
	long LastProcess;
	WSADATA wsaData;
	Spawn spawnParser;
	Item itemParser;
	World worldParser;
	int numSpawns;
	int numItems;

public:
	// Bit flags (in clientRequest) determining what data the client is requesting. See client's Structures.cs.
	enum inc_packet_types : int
	{
		IPT_zone = 0x01,
		IPT_self = 0x02,
		IPT_target = 0x04,
		IPT_spawns = 0x08,
		IPT_ground = 0x10,
		IPT_getproc = 0x20,
		IPT_setproc = 0x40,
		IPT_world = 0x80
	};

	// The type of element we are sending back to client. Lives in the 'flags' field of the outgoing packets.
	enum out_packet_types : int
	{
		OPT_spawns = 0x00,
		OPT_target = 0x01,
		OPT_zone = 0x04,
		OPT_ground = 0x05,
		OPT_process = 0x06,
		OPT_world = 0x08,
		OPT_self = 0xFD
	};

public:
	HWND hwnd;
	HWND h_MySEQServer;
	NetworkServer();
	~NetworkServer();

	void listIPAddresses();
	bool openListenerSocket(bool service = false);
	void collectIPAddresses(const char* hostname, std::vector<std::string>& ipAddresses);
	void logListeningAddresses();
	void logAddresses(const addrinfo* addrInfo, const std::string& message);
	in_addr getBestNonLocalhostAddress(const addrinfo* addrInfo);
	bool shouldUpdateBestAddress(const in_addr& currentAddr, const in_addr& newAddr);
	void updateGUIWithPortAndIP(const std::string& ipAddress);
	void closeListenerSocket();
	bool requestContains(int pt);
	void openClientSocket();
	void closeClientSocket();
	void setOffset(OffsetType ot, QWORD value);
	void init(IniReaderInterface* ir_intf);
	void enterReceiveLoop(MemReaderInterface* mr_intf);
	bool processReceivedData(MemReaderInterface* mr_intf);
	bool processInvalidProcess();
	void processGetProcess();
	void processZoneName(MemReaderInterface* mr_intf);
	void processPlayerInfo(MemReaderInterface* mr_intf);
	void processSpawnList(MemReaderInterface* mr_intf);
	void processTarget(MemReaderInterface* mr_intf);
	void processGroundItems(MemReaderInterface* mr_intf);
	void processWorldInfo(MemReaderInterface* mr_intf);
	void sendSpawnInfo();
	void resetZoneName();
	void updateGuiZoneName(MemReaderInterface* mr_intf);
	void updateGuiSpawnCount(MemReaderInterface* mr_intf);
	void updateGuiItemCount();
	bool ChangeProcess(MemReaderInterface* mr_intf);
	string getCharName(MemReaderInterface* mr_intf);
	UINT current_offset(int type);
};
