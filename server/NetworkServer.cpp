/*==============================================================================
	Copyright (C) 2006 - 2013  All developers at http://sourceforge.net/projects/seq

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
#define _WINSOCK_DEPRECATED_NO_WARNINGS

#include "NetworkServer.h"


#pragma comment(lib, "Ws2_32.lib")

NetworkServer::NetworkServer()
	: sockClient(INVALID_SOCKET),
	  sockAddrSize(sizeof(sockAddr)),
	  change_process(false),
	  zoneName("StartUp"),
	  LastProcess(0)
	{
		psockAddrIn = reinterpret_cast<sockaddr_in*>(&sockAddr);
	}

NetworkServer::~NetworkServer()
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

	MessageBox(hwnd, oss.str().c_str(), "MySEQ Open Server: Local IP Addresses", MB_OK | MB_TOPMOST | MB_ICONINFORMATION);
}

void NetworkServer::collectIPAddresses(const char* hostname, std::vector<std::string>& ipAddresses)
{
	struct addrinfo hints = {}, * result = nullptr;
	hints.ai_family = AF_INET;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;

	// Get IP address for the given hostname
	int ret = getaddrinfo(hostname, nullptr, &hints, &result);
	if (ret != 0) {
		std::cerr << "getaddrinfo error: " << gai_strerror(ret) << std::endl;
		WSACleanup();
		return;
	}

	// Iterate through the results and collect IP addresses
	for (struct addrinfo* ptr = result; ptr != nullptr; ptr = ptr->ai_next) {
		char ipStr[INET_ADDRSTRLEN];
		void* addrPtr = nullptr;

		if (ptr->ai_family == AF_INET) { // IPv4
			sockaddr_in* sockaddr_ipv4 = reinterpret_cast<sockaddr_in*>(ptr->ai_addr);
			addrPtr = &(sockaddr_ipv4->sin_addr);
		}

		if (addrPtr) {
			inet_ntop(ptr->ai_family, addrPtr, ipStr, sizeof(ipStr));
			ipAddresses.push_back(ipStr);
		}
	}
	freeaddrinfo(result);
}

bool NetworkServer::openListenerSocket(bool service)
{
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
	{
		MessageBox(nullptr, "Error: NetworkServer: Failed to initialize Winsock.", "Failed to initialize Winsock", 0);
		return false;
	}

	sockListener = socket(AF_INET, SOCK_STREAM, 0);
	if (sockListener == INVALID_SOCKET)
	{
		MessageBox(nullptr, "Error: NetworkServer: Error creating listener socket.", "Failed to create listener socket.", 0);
		WSACleanup();
		return false;
	}

	memset(&sockAddr, 0, sizeof(sockAddr));
	psockAddrIn->sin_family = AF_INET;
	psockAddrIn->sin_addr.s_addr = INADDR_ANY;
	psockAddrIn->sin_port = htons(port);

	if (bind(sockListener, reinterpret_cast<sockaddr*>(&sockAddr), sizeof(sockAddr)) == SOCKET_ERROR)
	{
		std::ostringstream oss;
		oss << "Failed binding to port: " << port << "\nCheck for a suitable port or currently running MySEQ Server.\nExiting.";
		MessageBox(nullptr, oss.str().c_str(), "Winsock Error: Failed binding to port.", MB_OK | MB_TOPMOST | MB_ICONERROR);
		closesocket(sockListener);
		WSACleanup();
		return false;
	}

	if (listen(sockListener, 10) == SOCKET_ERROR)
	{
		MessageBox(nullptr, "Error: NetworkServer: Listen request failed.", "Listen request failed", 0);
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

void NetworkServer::logListeningAddresses()
{
	struct addrinfo hints = {}, * result = nullptr;
	hints.ai_family = AF_UNSPEC; // Support both IPv4 and IPv6
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;

	if (getaddrinfo("localhost", nullptr, &hints, &result) == 0)
	{
		logAddresses(result, "MySEQServer: Listening on ");
		freeaddrinfo(result);
	}

	if (getaddrinfo(nullptr, nullptr, &hints, &result) == 0)
	{
		in_addr current_host = getBestNonLocalhostAddress(result);
		if (current_host.s_addr != INADDR_NONE)
		{
			std::cout << "MySEQServer: Listening on " << inet_ntoa(current_host) << ":" << port << std::endl;
		}
		freeaddrinfo(result);
	}
}

void NetworkServer::logAddresses(const struct addrinfo* addrInfo, const std::string& message)
{
	for (const struct addrinfo* ptr = addrInfo; ptr != nullptr; ptr = ptr->ai_next)
	{
		char ipStr[INET_ADDRSTRLEN];
		void* addrPtr = nullptr;

		if (ptr->ai_family == AF_INET) // IPv4
		{
			sockaddr_in* sockaddr_ipv4 = reinterpret_cast<sockaddr_in*>(ptr->ai_addr);
			addrPtr = &(sockaddr_ipv4->sin_addr);
		}

		if (addrPtr)
		{
			inet_ntop(ptr->ai_family, addrPtr, ipStr, sizeof(ipStr));
			std::cout << message << ipStr << std::endl;
		}
	}
}

in_addr NetworkServer::getBestNonLocalhostAddress(const struct addrinfo* addrInfo)
{
	for (const struct addrinfo* ptr = addrInfo; ptr != nullptr; ptr = ptr->ai_next)
	{
		if (ptr->ai_family == AF_INET) // Only consider IPv4 addresses
		{
			sockaddr_in* sockaddr_ipv4 = reinterpret_cast<sockaddr_in*>(ptr->ai_addr);
			if (sockaddr_ipv4->sin_addr.s_addr != htonl(INADDR_LOOPBACK))
			{
				return sockaddr_ipv4->sin_addr;
			}
		}
	}

	in_addr addr;
	addr.s_addr = INADDR_NONE;
	return addr;
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

// Set the port in the gui
void NetworkServer::updateGUIWithPortAndIP(const std::string& ipAddress)
{
	if (!h_MySEQServer)
	{
		return;
	}
	std::string portStr = std::to_string(port);
	SetDlgItemText(h_MySEQServer, IDC_TEXT_PORT, portStr.c_str());
	SetDlgItemText(h_MySEQServer, IDC_TEXT_PRIMARY, ipAddress.c_str());
}

void NetworkServer::openClientSocket()
{
	auto addrSize = static_cast<int>(sockAddrSize); // Define an int variable for sockAddrSize
	sockClient = accept(sockListener, reinterpret_cast<sockaddr*>(&sockAddr), &addrSize);

	if (sockClient == INVALID_SOCKET)
	{
		MessageBox(nullptr, "Error: NetworkServer: Error with connection request.", "Error with connection request", 0);
		std::ostringstream oss;
		oss << "Error: NetworkServer: Error with connection request 0x" << std::dec << WSAGetLastError();
		throw std::runtime_error(oss.str());
	}

	zoneName = "StartUp";
	if (hwnd)
	{
		SetDlgItemText(hwnd, IDC_TEXT_ZONE, _T(""));
		SetDlgItemText(hwnd, IDC_TEXT_NAME, _T(""));
	}

	std::cout << "MySEQServer: New connection from: " << inet_ntoa(psockAddrIn->sin_addr) << std::endl;
}

void NetworkServer::closeClientSocket()
{
	if (sockClient != INVALID_SOCKET)
	{
		std::cout << "MySEQServer: Closing client socket" << std::endl << std::endl;
		closesocket(sockClient);
		sockClient = INVALID_SOCKET;
	}
}
void NetworkServer::closeListenerSocket()
{
	if (sockListener != INVALID_SOCKET)
	{
		std::cout << "MySEQServer: Closing listener socket" << std::endl << std::endl;
		closesocket(sockListener);
		sockListener = INVALID_SOCKET;
	}
}

bool NetworkServer::requestContains(int pt)
{
	return ((clientRequest & pt) != 0);
}

string NetworkServer::getCharName(MemReaderInterface* mr_intf)
{
	// get current character name
	string rtn = "";
	QWORD pTemp = 0;
	QWORD nameOff;

	if (offsets[OT_self])
		pTemp = mr_intf->extractRAWPointer(offsets[OT_self]);

	nameOff = spawnParser.offsets[spawnParser.OT_name];
	if (pTemp)
	{
		rtn = mr_intf->extractString(pTemp + nameOff);
	}
	return rtn;
}

void NetworkServer::setOffset(OffsetType ot, QWORD value)
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
		// Set offsets in GUI
		TCHAR ot_spawnlist[16];
		TCHAR ot_self[16];
		TCHAR ot_target[16];
		TCHAR ot_zonename[16];
		TCHAR ot_ground[16];
		TCHAR ot_world[16];
		// Put in our offsets in a hex format
		sprintf_s(ot_spawnlist, "0x%llx", offsets[OT_spawnlist]);
		sprintf_s(ot_self, "0x%llx", offsets[OT_self]);
		sprintf_s(ot_target, "0x%llx", offsets[OT_target]);
		sprintf_s(ot_zonename, "0x%llx", offsets[OT_zonename]);
		sprintf_s(ot_ground, "0x%llx", offsets[OT_ground]);
		sprintf_s(ot_world, "0x%llx", offsets[OT_world]);
		// Update the dialog
		SetDlgItemText(h_MySEQServer, IDC_TEXT_SPAWNHEADER, ot_spawnlist);
		SetDlgItemText(h_MySEQServer, IDC_TEXT_CHARINFO, ot_self);
		SetDlgItemText(h_MySEQServer, IDC_TEXT_TARGETADDR, ot_target);
		SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONEADDR, ot_zonename);
		SetDlgItemText(h_MySEQServer, IDC_ITEMSADDR, ot_ground);
		SetDlgItemText(h_MySEQServer, IDC_TEXT_WORLDADDR, ot_world);
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
	return (UINT)offsets[(OffsetType)type];
}

bool NetworkServer::processReceivedData(MemReaderInterface* mr_intf)
{
	//int maxLoop;
	//QWORD pTemp, pTemp2;
	string newZoneName;

	quickInfo = false;

	if (LastProcess != mr_intf->getCurrentPID()) {
		quickInfo = true;
		LastProcess = mr_intf->getCurrentPID();
	}
	int bytesRecvd = recv(sockClient, reinterpret_cast<char*>(&clientRequest), sizeof(clientRequest), 0);
	if (bytesRecvd == 0 || bytesRecvd == SOCKET_ERROR || bytesRecvd != sizeof(clientRequest)) {
		return true;
	}

	// Wait for request from the client

	if (change_process) {
		return ChangeProcess(mr_intf);
	}

	if (!mr_intf->isValid()) {
		return processInvalidProcess();
	}
	// Send get_process requests
	if (requestContains(IPT_getproc))
	{
		processGetProcess();
	}
	// Send set_process requests
	if (requestContains(IPT_setproc))
	{
		change_process = true;
		return false;
		// client does not expect a return packet now
	}

	// Send zonename requests
	if (requestContains(IPT_zone))
	{
		processZoneName(mr_intf);
	}

	// Send player requests
	if (requestContains(IPT_self)) {
		processPlayerInfo(mr_intf);
	}

	// Send spawnlist requests
	if (requestContains(IPT_spawns)) {
		processSpawnList(mr_intf);
	}

	// Send target requests
	if (requestContains(IPT_target)) {
		processTarget(mr_intf);
	}

	// Send grounditem requests
	if (requestContains(IPT_ground)) {
		processGroundItems(mr_intf);
	}
	// Send world requests
	if (requestContains(IPT_world)) {
		processWorldInfo(mr_intf);
	}

	// Send spawn information (most other information is also packed into a spawn structure)
	sendSpawnInfo();
	return false;
}

bool NetworkServer::processInvalidProcess() {
	spawnParser.packNetBufferRaw(OPT_process, 0);
	spawnParser.pushNetBuffer();
	int numElements = spawnParser.getNetBufferSize();
	if (send(sockClient, reinterpret_cast<char*>(&numElements), sizeof(numElements), 0) == -1) {
		perror("send");
		return false;
	}
	if (numElements > 0) {
		if (quickInfo) {
			cout << "MySEQServer: numSpawns(" << numSpawns << ") numItems(" << numItems << ")" << endl;
		}
		if (send(sockClient, reinterpret_cast<char*>(spawnParser.getNetBufferStart()), numElements * sizeof(netBuffer_t), 0) == -1) {
			perror("send");
			return false;
		}
		spawnParser.clearNetBuffer();
		numSpawns = numItems = 0;
	}
	return false;
}

void NetworkServer::processGetProcess() {
	MemReader tempMemReader;
	for (bool processOpen = tempMemReader.openFirstProcess("eqgame"); processOpen; processOpen = tempMemReader.openNextProcess("eqgame")) {
		if (offsets[OT_self]) {
			QWORD pTemp = tempMemReader.extractRAWPointer(offsets[OT_self]);
			if (pTemp && tempMemReader.extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset)) {
				spawnParser.packNetBufferRaw(OPT_process, tempMemReader.getCurrentPID());
				spawnParser.pushNetBuffer();
			}
		}
	}
}

void NetworkServer::processZoneName(MemReaderInterface* mr_intf) {
	string newZoneName;
	if (offsets[OT_zonename]) {
		newZoneName = mr_intf->extractString2(offsets[OT_zonename] - 0x140000000 + (QWORD)mr_intf->getCurrentBaseAddress());
	}
	if (newZoneName != zoneName) {
		quickInfo = true;
		zoneName = newZoneName;
		updateGuiZoneName(mr_intf);
		spawnParser.packNetBufferStrings(OPT_zone, zoneName, "");
		spawnParser.pushNetBuffer();
	}
	if (quickInfo) {
		cout << "MySEQServer: Zonename is " << newZoneName << endl;
	}
}

void NetworkServer::processPlayerInfo(MemReaderInterface* mr_intf) {
	QWORD pTemp = 0;
	if (offsets[OT_self]) {
		pTemp = mr_intf->extractRAWPointer(offsets[OT_self]);
	}
	if (quickInfo) {
		cout << "MySEQServer: pSelf is 0x" << hex << pTemp << endl;
	}
	if (pTemp && mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset)) {
		spawnParser.packNetBufferRaw(OPT_self, pTemp);
		spawnParser.pushNetBuffer();
	}
}

void NetworkServer::processSpawnList(MemReaderInterface* mr_intf) {
	QWORD pTemp = 0;
	QWORD pTemp2 = 0;
	int maxLoop;
	if (offsets[OT_spawnlist]) {
		pTemp = pTemp2 = mr_intf->extractRAWPointer(offsets[OT_spawnlist]);
	}
	cout << "ptemp " << pTemp << " pTemp2 " << pTemp2 << endl;
	for (maxLoop = 0; maxLoop < 2000; ++maxLoop) {
		if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset)) {
			auto prevPointer = spawnParser.extractPrevPointer();
			if (prevPointer) {
				pTemp = prevPointer;
				cout << "pTemp extract " << pTemp << " raw " << spawnParser.rawBuffer << " largest offset " << spawnParser.largestOffset << endl;
			}
			else {
				break;
			}
		}
	}
	if (quickInfo) {
		cout << "MySEQServer: pSpawnlist is 0x" << hex << pTemp << endl;
		if (maxLoop == 2000) {
			cout << "MySEQServer: Warning: maxLoop reached finding pSpawnlist!" << endl;
		}
		if (pTemp != pTemp2) {
			cout << "MySEQServer: pSpawnlist changed from INI setting of 0x" << hex << pTemp2 << " to " << pTemp << endl;
		}
	}
	loopCount++;
	while (pTemp) {
		if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset)) {
			spawnParser.packNetBufferRaw(OPT_spawns, pTemp);
			spawnParser.pushNetBuffer();
			numSpawns++;
			pTemp = spawnParser.extractNextPointer();
		}
		else {
			pTemp = 0;
		}
	}
	if (loopCount > 20) {
		updateGuiSpawnCount(mr_intf);
	}
}

void NetworkServer::processTarget(MemReaderInterface* mr_intf) {
	QWORD pTemp = 0;
	if (offsets[OT_target]) {
		pTemp = mr_intf->extractRAWPointer(offsets[OT_target]);
	}
	if (quickInfo) {
		cout << "MySEQServer: pTarget is 0x" << hex << pTemp << endl;
	}
	if (pTemp) {
		if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset)) {
			spawnParser.packNetBufferRaw(OPT_target, pTemp);
			spawnParser.pushNetBuffer();
		}
		else {
			spawnParser.packNetBufferEmpty(OPT_target, pTemp);
			spawnParser.pushNetBuffer();
		}
	}
	else {
		spawnParser.packNetBufferEmpty(OPT_target, pTemp);
		spawnParser.pushNetBuffer();
	}
}

void NetworkServer::processGroundItems(MemReaderInterface* mr_intf) {
	QWORD pTemp = 0, pTemp2 = 0;
	UINT nameOff = itemParser.offsets[itemParser.OT_name];
	string itName = "";
	if (offsets[OT_ground]) {
		pTemp2 = mr_intf->extractRAWPointer(offsets[OT_ground]);
	}
	if (pTemp2) {
		itName = mr_intf->extractString(pTemp2 + nameOff);
	}
	if (itName.compare(0, 2, "IT") == 0) {
		pTemp = pTemp2;
	}
	else if (pTemp2) {
		pTemp = mr_intf->extractPointer(pTemp2);
	}
	if (quickInfo) {
		cout << "MySEQServer: pItems is 0x" << hex << pTemp << endl;
	}
	while (pTemp) {
		if (mr_intf->extractToBuffer(pTemp, itemParser.rawBuffer, itemParser.largestOffset)) {
			itemParser.packItemBuffer(OPT_ground);
			spawnParser.packNetBufferFrom(itemParser);
			spawnParser.pushNetBuffer();
			numItems++;
			if ((numItems > 300) || (pTemp == itemParser.extractNextPointer())) {
				pTemp = 0;
			}
			else {
				pTemp = itemParser.extractNextPointer();
			}
		}
		else {
			pTemp = 0;
		}
	}
	if (loopCount > 20) {
		updateGuiItemCount();
	}
}

void NetworkServer::processWorldInfo(MemReaderInterface* mr_intf) {
	QWORD pTemp = 0;
	if (offsets[OT_world]) {
		pTemp = mr_intf->extractRAWPointer(offsets[OT_world]);
	}
	if (quickInfo) {
		cout << "MySEQServer: pWorldInfo is 0x" << hex << pTemp << endl;
	}
	if (pTemp && mr_intf->extractToBuffer(pTemp, worldParser.rawBuffer, worldParser.largestOffset)) {
		worldParser.packWorldBuffer(OPT_world);
		spawnParser.packNetBufferWorld(worldParser);
		spawnParser.pushNetBuffer();
	}
}

void NetworkServer::sendSpawnInfo() {
	int numElements = spawnParser.getNetBufferSize();
	send(sockClient, reinterpret_cast<char*>(&numElements), sizeof(numElements), 0);
	if (numElements) {
		if (quickInfo) {
			cout << "MySEQServer: numSpawns(" << dec << numSpawns << ") numItems(" << numItems << ")" << endl;
		}
		send(sockClient, reinterpret_cast<char*>(spawnParser.getNetBufferStart()), numElements * sizeof(netBuffer_t), 0);
		spawnParser.clearNetBuffer();
		numSpawns = numItems = 0;
	}
}

// Helper methods

void NetworkServer::resetZoneName() {
	zoneName.clear();
	SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, _T(""));
	SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, _T(""));
	cout << "MySEQServer: Process set to 0x" << hex << clientRequest << endl;
}

void NetworkServer::updateGuiZoneName(MemReaderInterface* mr_intf) {
	if (h_MySEQServer) {
		if (zoneName != "StartUp") {
			string charName = getCharName(mr_intf);
			SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, charName.c_str());
			SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, zoneName.c_str());
			loopCount = 50; // force an update of the spawns count numbers on gui
		}
		else {
			SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, _T(""));
			SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, _T(""));
		}
	}
}

void NetworkServer::updateGuiSpawnCount(MemReaderInterface* mr_intf) {
	QWORD pTemp = 0;
	if (offsets[OT_spawnlist]) {
		pTemp = mr_intf->extractRAWPointer(offsets[OT_spawnlist]);
	}
	for (int maxLoop = 0; maxLoop < 2000; ++maxLoop) {
		if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset)) {
			if (spawnParser.extractPrevPointer()) {
				pTemp = spawnParser.extractPrevPointer();
			}
			else {
				break;
			}
		}
	}
	UINT typeOffset = spawnParser.offsets[spawnParser.OT_type];
	int pcNum = 0, npcNum = 0, corpseNum = 0;
	while (pTemp) {
		if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset)) {
			BYTE result = spawnParser.extractRawByte(spawnParser.OT_type);
			switch (result) {
			case 0: pcNum++; break;
			case 1: npcNum++; break;
			default: corpseNum++;
			}
			pTemp = spawnParser.extractNextPointer();
		}
		else {
			pTemp = 0;
		}
	}
	char buffer[65];
	_itoa_s(npcNum, buffer, 10);
	SetDlgItemText(h_MySEQServer, IDC_TEXT_SPAWNS, buffer);
	_itoa_s(pcNum, buffer, 10);
	SetDlgItemText(h_MySEQServer, IDC_TEXT_SPAWNS2, buffer);
	_itoa_s(corpseNum, buffer, 10);
	SetDlgItemText(h_MySEQServer, IDC_TEXT_SPAWNS3, buffer);
}

void NetworkServer::updateGuiItemCount() {
	char buffer[65];
	_itoa_s(numItems, buffer, 65, 10);
	SetDlgItemText(h_MySEQServer, IDC_TEXT_ITEMS, buffer);
	loopCount = 0;
}

bool NetworkServer::ChangeProcess(MemReaderInterface* mr_intf)
{
	// The last request was to change the process. This packet should contain the process ID to switch to.
	DWORD originalPID = mr_intf->getCurrentPID();
	cout << "MySEQServer: Setting process to 0x" << hex << clientRequest << endl;

	if (mr_intf->openFirstProcess("eqgame", false)) {
		do {
			if (mr_intf->getCurrentPID() == static_cast<DWORD>(clientRequest)) {
				zoneName = "StartUp";
				SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, _T(""));
				SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, _T(""));
				break;
			}
		} while (mr_intf->openNextProcess("eqgame", false));
	}

	if (mr_intf->getCurrentPID() != static_cast<DWORD>(clientRequest)) {
		// We failed to connect to the desired PID, try to open a process again.
		if (mr_intf->openFirstProcess("eqgame", false) && originalPID != mr_intf->getCurrentPID()) {
			zoneName = "StartUp";
			SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, _T(""));
			SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, _T(""));
		}
	}

	// The client no longer expects a return packet for this.
	// The client will continue requesting data the next tick().
	change_process = false;
	return false;
}