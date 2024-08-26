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

#include "stdafx.h"
#define _WINSOCK_DEPRECATED_NO_WARNINGS

#include "NetworkServer.h"

NetworkServer::NetworkServer()
{
	sockAddrSize = sizeof(sockAddr);
	psockAddrIn = (sockaddr_in*)&sockAddr;
	zoneName = "StartUp";
	sockClient = INVALID_SOCKET;
	LastProcess = 0;
	change_process = false;
}

NetworkServer::~NetworkServer(void)
{
	WSACleanup();
}

void NetworkServer::listIPAddresses()
{
	std::vector<std::string> ipAddresses;
	collectIPAddresses("localhost", ipAddresses);
	collectIPAddresses("", ipAddresses);

	std::ostringstream oss;
	for (const auto& ip : ipAddresses)
	{
		oss << ip << "\r\n";
	}

	MessageBox(h_MySEQServer ? h_MySEQServer : NULL, oss.str().c_str(), "MySEQ Open Server: Local IP Addresses", MB_OK | MB_TOPMOST | MB_ICONINFORMATION);
}

void NetworkServer::collectIPAddresses(const char* hostname, std::vector<std::string>& ipAddresses)
{
	hostent* localHost = gethostbyname(hostname);
	if (localHost == nullptr) return;  // Handle the error case where gethostbyname fails

	for (int i = 0; localHost->h_addr_list[i] != 0; ++i)
	{
		struct in_addr addr;
		memcpy(&addr, localHost->h_addr_list[i], sizeof(struct in_addr));
		ipAddresses.push_back(inet_ntoa(addr));
		if (ipAddresses.size() > 10)
			break;
	}
}

void NetworkServer::logListeningAddresses()
{
	hostent* localHost = gethostbyname("localhost");
	logAddresses(localHost, "MySEQServer: Listening on ");

	in_addr current_host;
	current_host.S_un.S_addr = NULL;

	localHost = gethostbyname("");
	current_host = getBestNonLocalhostAddress(localHost);

	if (current_host.S_un.S_addr != NULL)
	{
		std::cout << "MySEQServer: Listening on " << inet_ntoa(current_host) << ":" << port << std::endl;
	}
}

in_addr NetworkServer::getBestNonLocalhostAddress(hostent* localHost)
{
	in_addr bestAddr;
	bestAddr.S_un.S_addr = NULL;

	for (int i = 0; localHost->h_addr_list[i] != 0; ++i)
	{
		struct in_addr addr;
		memcpy(&addr, localHost->h_addr_list[i], sizeof(struct in_addr));

		if (addr.s_net != 0 && shouldUpdateBestAddress(bestAddr, addr))
		{
			bestAddr = addr;
		}
	}

	return bestAddr;
}

bool NetworkServer::shouldUpdateBestAddress(const in_addr& currentAddr, const in_addr& newAddr)
{
	if (currentAddr.S_un.S_addr == NULL)
	{
		return true;
	}

	bool isNewPrivate = (newAddr.s_net == 192 && newAddr.s_host == 168) || newAddr.s_net == 10 ||
		(newAddr.s_net == 177 && newAddr.s_host >= 16 && newAddr.s_host <= 31);

	bool isCurrentNonPrivate = (currentAddr.s_net != 192 || currentAddr.s_host != 168) &&
		(currentAddr.s_net != 10) && (currentAddr.s_net != 176);

	if (isNewPrivate && isCurrentNonPrivate)
	{
		return true;
	}

	if (newAddr.s_net == currentAddr.s_net && newAddr.s_host == currentAddr.s_host)
	{
		return (newAddr.s_lh < currentAddr.s_lh) ||
			(newAddr.s_lh == currentAddr.s_lh && newAddr.s_impno <= currentAddr.s_impno);
	}

	return false;
}

bool NetworkServer::openListenerSocket(bool service)
{
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(1, 1), &wsa) != 0)
	{
		MessageBox(NULL, "Error: NetworkServer: Failed to initialize Winsock.", "Failed to initialize Winsock", 0);
		return false;
	}

	sockListener = socket(AF_INET, SOCK_STREAM, 0);
	if (sockListener == INVALID_SOCKET)
	{
		MessageBox(NULL, "Error: NetworkServer: Error creating listener socket.", "Failed to create listener socket.", 0);
		WSACleanup();
		return false;
	}

	memset(&sockAddr, 0, sizeof(sockAddr));
	psockAddrIn->sin_family = AF_INET;
	psockAddrIn->sin_addr.s_addr = INADDR_ANY;
	psockAddrIn->sin_port = htons(port);

	if (bind(sockListener, &sockAddr, sizeof(sockAddr)) == SOCKET_ERROR)
	{
		std::ostringstream oss;
		oss << "Failed binding to port: " << port << "\nCheck for a suitable port or currently running MySEQ Server.\nExiting.";
		MessageBox(NULL, oss.str().c_str(), "Winsock Error: Failed binding to port.", MB_OK | MB_TOPMOST | MB_ICONERROR);
		closesocket(sockListener);
		WSACleanup();
		return false;
	}

	if (listen(sockListener, 10) == SOCKET_ERROR)
	{
		MessageBox(NULL, "Error: NetworkServer: Listen request failed.", "Listen request failed", 0);
		closesocket(sockListener);
		WSACleanup();
		return false;
	}

	if (!service)
	{
		WSAAsyncSelect(sockListener, hwnd, 1045, FD_READ | FD_CONNECT | FD_CLOSE | FD_ACCEPT);
		logListeningAddresses();
	}

	return true;
}

void NetworkServer::logAddresses(hostent* localHost, const std::string& prefix)
{
	for (int i = 0; localHost->h_addr_list[i] != 0; ++i)
	{
		struct in_addr addr;
		memcpy(&addr, localHost->h_addr_list[i], sizeof(struct in_addr));
		std::cout << prefix << inet_ntoa(addr) << ":" << port << std::endl;
	}
}

void NetworkServer::updateGUIWithPortAndIP(const std::string& ipAddress)
{
	if (h_MySEQServer != NULL)
	{
		char buffer[65];
		_itoa_s(port, buffer, 10);
		SetDlgItemText(h_MySEQServer, IDC_TEXT_PORT, buffer);
		SetDlgItemText(h_MySEQServer, IDC_TEXT_PRIMARY, ipAddress.c_str());
	}
}

void NetworkServer::resetUI() {
	zoneName = "StartUp";
	SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, _T(""));
	SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, _T(""));
}
void NetworkServer::openClientSocket()
{
	// Wait for incoming connections
	sockClient = accept(sockListener, &sockAddr, &sockAddrSize);

	if (sockClient == INVALID_SOCKET)
	{
		MessageBox(NULL, "Error: NetworkServer: Error with connection request.", "Error with connection request", 0);
		ostrstream strm;
		strm << "Error: NetworkServer: Error with connection request 0x" << dec << WSAGetLastError() << ends;
		throw Exception(EXCLEV_ERROR, strm.str());
	}
	resetUI();

	cout << "MySEQServer: New connection from: " << inet_ntoa(psockAddrIn->sin_addr) << endl;
}

void NetworkServer::closeClientSocket()
{
	if (sockClient != INVALID_SOCKET) {
		cout << "MySEQServer: Closing client socket" << endl << endl;
		closesocket(sockClient);
		sockClient = INVALID_SOCKET;
	}
}

void NetworkServer::closeListenerSocket()
{
	cout << "MySEQServer: Closing listener socket" << endl << endl;
	closesocket(sockListener);
	sockListener = INVALID_SOCKET;
}

bool NetworkServer::requestContains(inc_packet_types pt)
{
	return ((clientRequest & pt) != 0);
}

string NetworkServer::getCharName(MemReaderInterface* mr_intf)
{
	// Check if the self-offset is valid
	if (!offsets[OT_self]) {
		return ""; // Return empty string if offset is invalid
	}

	// Extract the pointer to the current character
	QWORD pTemp = mr_intf->extractRAWPointer(offsets[OT_self]);
	if (!pTemp) {
		return ""; // Return empty string if pointer extraction failed
	}

	// Get the offset for the character name
	QWORD nameOffset = spawnParser.offsets[spawnParser.OT_name];

	// Extract and return the character name
	return mr_intf->extractString(pTemp + nameOffset);
}

void NetworkServer::setOffset(offset_types ot, QWORD value)
{
	offsets[ot] = value;
}

void NetworkServer::init(IniReaderInterface* ir_intf)
{
	spawnParser.init(ir_intf);
	itemParser.init(ir_intf);
	worldParser.init(ir_intf);

	port = (UINT)ir_intf->readIntegerEntry("Port", "Port");

	setOffset(OT_spawnlist, ir_intf->readIntegerEntry("Memory Offsets", "SpawnHeaderAddr"));
	setOffset(OT_self, ir_intf->readIntegerEntry("Memory Offsets", "CharInfo"));
	setOffset(OT_target, ir_intf->readIntegerEntry("Memory Offsets", "TargetAddr"));
	setOffset(OT_zonename, ir_intf->readIntegerEntry("Memory Offsets", "ZoneAddr"));
	setOffset(OT_ground, ir_intf->readIntegerEntry("Memory Offsets", "ItemsAddr"));
	setOffset(OT_world, ir_intf->readIntegerEntry("Memory Offsets", "WorldAddr"));

	if (h_MySEQServer) {
		// Array of control IDs corresponding to each offset type
		int controlIds[OT_max] = {
			IDC_TEXT_ZONEADDR,    // OT_zonename
			IDC_TEXT_SPAWNHEADER, // OT_spawnlist
			IDC_TEXT_CHARINFO,    // OT_self
			IDC_TEXT_TARGETADDR,  // OT_target
			IDC_ITEMSADDR,        // OT_ground
			IDC_TEXT_WORLDADDR    // OT_world
		};

		// Iterate through the offsets array and update the GUI
		for (int i = 0; i < OT_max; ++i)
		{
			// Convert the offset to a hex string
			std::ostringstream oss;
			oss << "0x" << std::hex << offsets[i];
			// Convert std::string to LPCSTR (const char*) and update the GUI
			SetDlgItemText(h_MySEQServer, controlIds[i], oss.str().c_str());
		}
	}
	cout << "MySEQServer: Port value and memory offsets read in." << endl;
}

void NetworkServer::enterReceiveLoop(MemReaderInterface* mr_intf)
{
	bool exitLoop = false;

	while (!exitLoop)
	{
		exitLoop = processReceivedData(mr_intf);
	}
}

UINT NetworkServer::current_offset(int type)
{
	return (UINT)offsets[(offset_types)type];
}

bool NetworkServer::processReceivedData(MemReaderInterface* mr_intf)
{
	int numSpawns = 0;
	int numItems = 0;
	int numElements;

	quickInfo = false;

	if (LastProcess != mr_intf->getCurrentPID()) {
		quickInfo = true;
		LastProcess = mr_intf->getCurrentPID();
	}

	// Wait for request from the client
	int bytesRecvd = recv(sockClient, (char*)&clientRequest, sizeof(clientRequest), 0);

	if (bytesRecvd == 0 || bytesRecvd == SOCKET_ERROR || bytesRecvd != sizeof(clientRequest))
	{
		return true;
	}
	if (!handleProcessChange(mr_intf))
		return false;

	if (!mr_intf->isValid()) {
		// we dont have a valid process
		// send a reply to keep stream connected

		spawnParser.packNetBufferRaw(OPT_process, 0);
		spawnParser.pushNetBuffer();
		// First send the number of elements
		numElements = spawnParser.getNetBufferSize();
		send(sockClient, (char*)&numElements, sizeof(numElements), 0);

		// Now sent the array of elements
		if (numElements)
		{
			// Show the spawncount (minus the zonename and yourself, if there is targeted mob we will get one extra)
			if (quickInfo)
				cout << "MySEQServer: numSpawns(" << dec << numSpawns << ") numItems(" << numItems << ")" << endl;

			send(sockClient, (char*)spawnParser.getNetBufferStart(), numElements * sizeof(netBuffer_t), 0);
			spawnParser.clearNetBuffer();
			numSpawns = numItems = 0;
		}

		return false;
	}
	handleRequests(mr_intf);

	// Send spawn information (most other information is also packed into a spawn structure)
	numElements = spawnParser.getNetBufferSize();

	// First send the number of elements
	send(sockClient, (char*)&numElements, sizeof(numElements), 0);

	// Now sent the array of elements
	if (numElements)
	{
		// Show the spawncount (minus the zonename and yourself, if there is targeted mob we will get one extra)
		if (quickInfo)
			cout << "MySEQServer: numSpawns(" << dec << numSpawns << ") numItems(" << numItems << ")" << endl;

		send(sockClient, (char*)spawnParser.getNetBufferStart(), numElements * sizeof(netBuffer_t), 0);
		spawnParser.clearNetBuffer();
		numSpawns = numItems = 0;
	}
	return false;
}

bool NetworkServer::handleProcessChange(MemReaderInterface* mr_intf)
{
	if (!change_process)
		return true;

	DWORD originalPID = mr_intf->getCurrentPID();
	cout << "MySEQServer: Setting process to 0x" << hex << clientRequest << endl;

	if (!switchToProcess(mr_intf, clientRequest, originalPID))
		return true;

	change_process = false;
	return false;
}

bool NetworkServer::switchToProcess(MemReaderInterface* mr_intf, DWORD clientRequest, DWORD originalPID)
{
	mr_intf->openFirstProcess("eqgame", false);
	while (mr_intf->getCurrentPID() != clientRequest)
	{
		if (!mr_intf->openNextProcess("eqgame", false))
			break;
	}
	if (mr_intf->getCurrentPID() != clientRequest)
	{
		mr_intf->openFirstProcess("eqgame", false);
		if (mr_intf->getCurrentPID() != originalPID)
			resetUI();
	}
	return mr_intf->getCurrentPID() == clientRequest;
}

void NetworkServer::handleRequests(MemReaderInterface* mr_intf)
{
	if (requestContains(IPT_getproc))
		handleGetProcRequest();
	if (requestContains(IPT_setproc))
		handleSetProcRequest();
	if (requestContains(IPT_zone))
		handleZoneRequest(mr_intf);
	if (requestContains(IPT_self))
		handleSelfRequest(mr_intf);
	if (requestContains(IPT_spawns))
		handleSpawnsRequest(mr_intf);
	if (requestContains(IPT_target))
		handleTargetRequest(mr_intf);
	if (requestContains(IPT_ground))
		handleGroundRequest(mr_intf);
	if (requestContains(IPT_world))
		handleWorldRequest(mr_intf);
}

bool NetworkServer::handleSetProcRequest()
{
	change_process = true;
	return false;
	// client does not expect a return packet now
}

void NetworkServer::handleGetProcRequest()
{
	QWORD pTemp = 0;
	MemReader tempMemReader;

	// Look for the first available process match
	tempMemReader.openFirstProcess("eqgame");
	while (tempMemReader.isValid())
	{
		if (offsets[OT_self])
			pTemp = tempMemReader.extractRAWPointer(offsets[OT_self]);

		if (pTemp && tempMemReader.extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset))
		{
			// For this type of packet, we only use the gamer name and the PID
			spawnParser.packNetBufferRaw(OPT_process, tempMemReader.getCurrentPID());
			spawnParser.pushNetBuffer();
		}

		// Look for the next available process match
		if (!tempMemReader.openNextProcess("eqgame"))
			break;
	}
}

void NetworkServer::handleZoneRequest(MemReaderInterface* mr_intf)
{
	std::string newZoneName;
	if (offsets[OT_zonename])
		newZoneName = mr_intf->extractString2(offsets[OT_zonename] - 0x140000000 + mr_intf->getCurrentBaseAddress());

	// Only send zonename response if zone changed
	if (newZoneName != zoneName)
	{
		quickInfo = true;
		zoneName = newZoneName;
		if (h_MySEQServer) {
			if (zoneName != "StartUp") {
				string charName = getCharName(mr_intf);
				SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, charName.c_str());
				SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, zoneName.c_str());
				// force an update of the spawns count numbers on gui
				loopCount = 50;
			}
			else {
				SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, _T(""));
				SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, _T(""));
			}
		}
		spawnParser.packNetBufferStrings(OPT_zone, zoneName, "");
		spawnParser.pushNetBuffer();
	}

	if (quickInfo)
		cout << "MySEQServer: Zonename is " << newZoneName << endl;
}

void NetworkServer::handleWorldRequest(MemReaderInterface* mr_intf)
{
	QWORD pTemp = 0;

	// Extract the pointer to world data
	if (offsets[OT_world])
		pTemp = mr_intf->extractRAWPointer(offsets[OT_world]);
	if (quickInfo)
		cout << "MySEQServer: pWorldInfo is 0x" << hex << pTemp << endl;

	// If a valid pointer was found, process the world data
	if (pTemp && mr_intf->extractToBuffer(pTemp, worldParser.rawBuffer, worldParser.largestOffset))
	{
		worldParser.packWorldBuffer(OPT_world);
		spawnParser.packNetBufferWorld(worldParser);
		spawnParser.pushNetBuffer();
	}
}

void NetworkServer::handleGroundRequest(MemReaderInterface* mr_intf)
{
	QWORD pTemp = 0;
	QWORD pTemp2 = 0;
	int numItems = 0;
	UINT nameOff;
	string itName = "";
	nameOff = itemParser.offsets[itemParser.OT_name];

	if (offsets[OT_ground])
		pTemp2 = mr_intf->extractRAWPointer(offsets[OT_ground]);

	if (pTemp2)
		itName = mr_intf->extractString(pTemp2 + nameOff);

	if (itName.compare(0, 2, "IT") == 0) {
		pTemp = pTemp2;
	}
	else if (pTemp2) {
		pTemp = mr_intf->extractPointer(pTemp2);
	}
	if (quickInfo)
	{
		cout << "MySEQServer: pItems is 0x" << hex << pTemp << endl;
	}

	while (pTemp)
	{
		if (mr_intf->extractToBuffer(pTemp, itemParser.rawBuffer, itemParser.largestOffset))
		{
			itemParser.packItemBuffer(OPT_ground);
			spawnParser.packNetBufferFrom(itemParser);
			spawnParser.pushNetBuffer();
			numItems++;
			// Avoid infinite loops
			if ((numItems > 300) || (pTemp == itemParser.extractNextPointer()))
				pTemp = 0;
			else
				pTemp = itemParser.extractNextPointer();
		}
		else
			pTemp = 0;
	}
	if (loopCount > 20) {
		// Update the gui with the spawn/item counts
		char buffer[65];
		_itoa_s(numItems, buffer, 65, 10);
		SetDlgItemText(h_MySEQServer, IDC_TEXT_ITEMS, (LPCSTR)&buffer);

		loopCount = 0;
	}
}

void NetworkServer::handleTargetRequest(MemReaderInterface* mr_intf)
{
	QWORD pTemp = 0;

	if (offsets[OT_target])
		pTemp = mr_intf->extractRAWPointer(offsets[OT_target]);

	if (quickInfo)
		cout << "MySEQServer: pTarget is 0x" << hex << pTemp << endl;

	if (pTemp)
	{
		if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset))
		{
			spawnParser.packNetBufferRaw(OPT_target, pTemp);
			spawnParser.pushNetBuffer();
		}
		else {
			// Send target of spawn ID 99999 (no target)
			spawnParser.packNetBufferEmpty(OPT_target, pTemp);
			spawnParser.pushNetBuffer();
		}
	}
	else {
		// Send target of spawn ID 99999 (no target)
		spawnParser.packNetBufferEmpty(OPT_target, pTemp);
		spawnParser.pushNetBuffer();
	}
}

void NetworkServer::handleSpawnsRequest(MemReaderInterface* mr_intf)
{
	QWORD pTemp = 0;
	QWORD pTemp2 = 0;
	int maxLoop;
	int numSpawns = 0;

	if (offsets[OT_spawnlist])
		pTemp = pTemp2 = mr_intf->extractRAWPointer(offsets[OT_spawnlist]);
	cout << "ptemp " << pTemp << " pTemp2 " << pTemp2 << endl;

	/* This pointer may point to a spawn in the	middle of the list due to shroud or hover.
	Back up to the top of the spawn list just to be sure we	grab the whole thing. */
	for (maxLoop = 0; maxLoop < 2000; maxLoop++)
	{
		if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset))
		{
			if (spawnParser.extractPrevPointer())
			{
				pTemp = spawnParser.extractPrevPointer();
				cout << "pTemp extract " << pTemp << " raw " << spawnParser.rawBuffer << " largest offset " << spawnParser.largestOffset << endl;
			}
			else
				break;
		}
	}

	if (quickInfo)
	{
		cout << "MySEQServer: pSpawnlist is 0x" << hex << pTemp << endl;

		if (maxLoop == 2000)
			cout << "MySEQServer: Warning: maxLoop reached finding pSpawnlist!" << endl;

		if (pTemp != pTemp2)
			cout << "MySEQServer: pSpawnlist changed from INI setting of 0x" << hex << pTemp2 << " to " << pTemp << endl;
	}
	loopCount++;

	while (pTemp)
	{
		if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset))
		{
			spawnParser.packNetBufferRaw(OPT_spawns, pTemp);
			spawnParser.pushNetBuffer();
			numSpawns++;
			pTemp = spawnParser.extractNextPointer();
		}
		else
			pTemp = 0;
	}
	updateGUI(pTemp, mr_intf, maxLoop);
}

void NetworkServer::updateGUI(QWORD& pTemp, MemReaderInterface* mr_intf, int& maxLoop)
{
	if (loopCount > 20) {
		// Update the gui with the spawn/item counts
		if (offsets[OT_spawnlist])
			pTemp = mr_intf->extractRAWPointer(offsets[OT_spawnlist]);

		// Get us to the top of the list
		for (maxLoop = 0; maxLoop < 2000; maxLoop++)
		{
			if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset))
			{
				if (spawnParser.extractPrevPointer())
					pTemp = spawnParser.extractPrevPointer();
				else
					break;
			}
		}
		BYTE result;
		int pcNum = 0;
		int npcNum = 0;
		int corpseNum = 0;
		while (pTemp)
		{
			if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset))
			{
				result = spawnParser.extractRawByte(spawnParser.OT_type);
				switch (result)
				{
				case 0:
					pcNum++;
					break;
				case 1:
					npcNum++;
					break;
				default:
					corpseNum++;
				}
				pTemp = spawnParser.extractNextPointer();
			}
			else
				pTemp = 0;
		}
		char buffer[65];
		_itoa_s(npcNum, buffer, 65, 10);
		SetDlgItemText(h_MySEQServer, IDC_TEXT_SPAWNS, (LPCSTR)&buffer);
		_itoa_s(pcNum, buffer, 65, 10);
		SetDlgItemText(h_MySEQServer, IDC_TEXT_SPAWNS2, (LPCSTR)&buffer);
		_itoa_s(corpseNum, buffer, 65, 10);
		SetDlgItemText(h_MySEQServer, IDC_TEXT_SPAWNS3, (LPCSTR)&buffer);
	}
}

void NetworkServer::handleSelfRequest(MemReaderInterface* mr_intf)
{
	QWORD pTemp = 0;

	if (offsets[OT_self])
		pTemp = mr_intf->extractRAWPointer(offsets[OT_self]);

	if (quickInfo)
		cout << "MySEQServer: pSelf is 0x" << hex << pTemp << endl;

	if (pTemp && mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset))
	{
		spawnParser.packNetBufferRaw(OPT_self, pTemp);
		spawnParser.pushNetBuffer();
	}
}