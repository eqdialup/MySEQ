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
#include "MemReader.h"
#include "Spawn.h"
#include "Item.h"
#include "World.h"
#include "resource.h"

class NetworkServerInterface
{
public:
	virtual UINT current_offset(int type) = 0;
};

class NetworkServer : public NetworkServerInterface
{
public:

	enum offset_types { OT_zonename, OT_spawnlist, OT_self, OT_target, OT_ground, OT_world, OT_max };

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

	HWND hwnd;
	HWND h_MySEQServer;
	NetworkServer();

	~NetworkServer(void);

	void listIPAddresses();

	void collectIPAddresses(const char* hostname, std::vector<std::string>& ipAddresses);

	void logAddresses(hostent* localHost, const std::string& prefix);

	void logListeningAddresses();

	in_addr getBestNonLocalhostAddress(hostent* localHost);

	bool shouldUpdateBestAddress(const in_addr& currentAddr, const in_addr& newAddr);
	
	bool openListenerSocket(bool service = false);

	void updateGUIWithPortAndIP(const std::string& ipAddress);

	void closeListenerSocket();

	void openClientSocket();

	void closeClientSocket();

	void setOffset(offset_types ot, QWORD value);

	void init(IniReaderInterface* ir_intf);

	void enterReceiveLoop(MemReaderInterface* mr_intf);

	bool processReceivedData(MemReaderInterface* mr_intf);

	bool handleSetProcRequest();

	void handleZoneRequest(MemReaderInterface* mr_intf);

	void handleWorldRequest(MemReaderInterface* mr_intf);

	void handleGroundRequest(MemReaderInterface* mr_intf);

	void handleTargetRequest(MemReaderInterface* mr_intf);

	void handleSpawnsRequest(MemReaderInterface* mr_intf);

	void updateGUI(QWORD& pTemp, MemReaderInterface* mr_intf, int& maxLoop);

	void handleSelfRequest(MemReaderInterface* mr_intf);

	void handleGetProcRequest();

	bool handleProcessChange(MemReaderInterface* mr_intf);

	bool switchToProcess(MemReaderInterface* mr_intf, DWORD clientRequest, DWORD originalPID);

	void handleRequests(MemReaderInterface* mr_intf);

	void resetUI();

	bool requestContains(inc_packet_types pt);

	string getCharName(MemReaderInterface* mr_intf);

	UINT current_offset(int type);

private:

	UINT port;

	SOCKET sockListener;

	SOCKET sockClient;

	struct sockaddr sockAddr;

	struct sockaddr_in* psockAddrIn;

	int sockAddrSize;

	int clientRequest;

	bool change_process;

	int loopCount;

	QWORD offsets[OT_max];

	bool quickInfo;

	string zoneName;

	DWORD LastProcess;

	Spawn spawnParser;

	Item itemParser;

	World worldParser;

};
