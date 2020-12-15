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
#include "NetworkServer.h"
#include <stdlib.h>
#include <IPHlpApi.h>

NetworkServer::NetworkServer()
{
	sockAddrSize = sizeof(sockAddr);
	psockAddrIn = (sockaddr_in*) &sockAddr;
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
		TCHAR mybuffer[1024];
		mybuffer[0] = _T('\0');
		hostent* localHost;
		int j = 0;
		localHost = gethostbyname("localhost");
		for (int i = 0; localHost->h_addr_list[i] != 0; ++i) {
			struct in_addr addr;
			memcpy(&addr, localHost->h_addr_list[i], sizeof(struct in_addr));
			// assign our local host to the designated ip address
			if (mybuffer[0] == _T('\0'))
				sprintf_s(mybuffer, "%s", inet_ntoa(addr));
			else
				strcat_s(mybuffer, inet_ntoa(addr));
			j = j + 1;
			if (j > 10)
				break;
		}

		localHost = gethostbyname("");
		for (int i = 0; localHost->h_addr_list[i] != 0; ++i) {
			struct in_addr addr;
			memcpy(&addr, localHost->h_addr_list[i], sizeof(struct in_addr));
			if (mybuffer[0] == _T('\0')) {
				sprintf_s(mybuffer, "%s", inet_ntoa(addr));
			} else {
				strcat_s(mybuffer, "\r\n");
				strcat_s(mybuffer, inet_ntoa(addr));
			}
			j = j + 1;
			if (j > 10)
				break;
		}
		MessageBox(h_MySEQServer ? h_MySEQServer: NULL, (LPCSTR)&mybuffer, "MySEQ Open Server: Local IP Addresses", MB_OK | MB_TOPMOST | MB_ICONINFORMATION);
		
}

bool NetworkServer::openListenerSocket(bool service)
{
	WSADATA wsa;
	
	if (WSAStartup(MAKEWORD(1,1), &wsa) != 0)
	{
		MessageBox(NULL, "Error: NetworkServer: Failed to initialize Winsock.", "Failed to initialize Winsock", 0);
		// ostrstream strm;
		return false;
		// strm << "Error: NetworkServer: Failed to initialize Winsock." << ends ; \
		// throw Exception(EXCLEV_ERROR, strm.str());
	}
	// Attempt to get a socket
	sockListener = socket(AF_INET, SOCK_STREAM, 0);
	if (sockListener == INVALID_SOCKET)
	{
		MessageBox(NULL, "Error: NetworkServer: Error creating listener socket.", "Failed to create listener socket.", 0);
		return false;
		//ostrstream strm;
		//strm << "Error: NetworkServer: Error creating listener socket." << ends ; \
		//throw Exception(EXCLEV_ERROR, strm.str());
	}

	// Fill out the sockaddr structure with typical values
	memset(&sockAddr, 0, sockAddrSize);
    psockAddrIn->sin_family = AF_INET;
	
	psockAddrIn->sin_addr.s_addr = INADDR_ANY;

	// Attempt to bind the listener socket
	psockAddrIn->sin_port = htons(port);	
		
	if ( bind(sockListener, (struct sockaddr*)&sockAddr, sockAddrSize) == SOCKET_ERROR )
	{
		cout << "MySEQServer: Failed binding to port: " << port << endl;
		std::string str("Failed binding to port: ");
		std::stringstream strm;
		strm << dec << port;
		str.append(strm.str());
		str.append("\r\nCheck for a suitable port or currently running MySEQ Server.\r\nExiting.");
		MessageBox(NULL, str.c_str(), "WindSock Error: Failed binding to port.", MB_OK | MB_TOPMOST | MB_ICONERROR);
		return false;
	}
	
	// Setup the backlog
	if (service) {
		if (listen(sockListener, 10) == SOCKET_ERROR)
		{
			MessageBox(NULL, "Error: NetworkServer: Listen request failed.", "Listen request failed", 0);
			return false;
			//ostrstream strm;
			//strm << "Error: NetworkServer: Listen request failed with code " << dec << WSAGetLastError() << ends ;
			//throw Exception(EXCLEV_ERROR, strm.str());
		}
	} else {
		// Setup the backlog
		if (listen(sockListener, 10) == SOCKET_ERROR)
		{
			MessageBox(NULL, "Error: NetworkServer: Listen request failed.", "Listen request failed", 0);
			return false;
			//ostrstream strm;
			//strm << "Error: NetworkServer: Listen request failed with code " << dec << WSAGetLastError() << ends ;
			//throw Exception(EXCLEV_ERROR, strm.str());
		}
		//Switch to Non-Blocking mode
		WSAAsyncSelect (sockListener, hwnd, 1045, FD_READ | FD_CONNECT | FD_CLOSE | FD_ACCEPT); 

		TCHAR active_address[128];
		hostent* localHost;

		localHost = gethostbyname("localhost");
		for (int i = 0; localHost->h_addr_list[i] != 0; ++i) {
			struct in_addr addr;
			memcpy(&addr, localHost->h_addr_list[i], sizeof(struct in_addr));
			// assign our local host to the designated ip address
			sprintf_s(active_address, "%s", inet_ntoa(addr));
			cout << "MySEQServer: Listening on " << inet_ntoa(addr) << ":" << dec << port << endl;
		}

		// use this to store the current non-localhost address being used
		in_addr current_host;
		current_host.S_un.S_addr = NULL;
		
		localHost = gethostbyname("");
		for (int i = 0; localHost->h_addr_list[i] != 0; ++i) {
			struct in_addr addr;
			memcpy(&addr, localHost->h_addr_list[i], sizeof(struct in_addr));
			if (addr.s_net == 0) {
				//cout << "Broadcast address " << inet_ntoa(addr) << endl;
			} else if (addr.s_net == 127) {
				//cout << "Localhost " << inet_ntoa(addr) << endl;
			} else if (addr.s_net == 169 && addr.s_host == 254) {
				//cout << "Autoconfig address " << inet_ntoa(addr) << endl;
			} else if (addr.s_net == 192 && addr.s_host == 0) {
				//cout << "Test-Net-1 address " << inet_ntoa(addr) << endl;
			} else if (addr.s_net == 198 && addr.s_host == 51 && addr.s_lh == 100) {
				//cout << "Test-Net-2 address " << inet_ntoa(addr) << endl;
			} else if (addr.s_net == 203 && addr.s_host == 0 && addr.s_lh == 113) {
				//cout << "Test-Net-3 address " << inet_ntoa(addr) << endl;
			} else if (addr.s_net == 192 && addr.s_host == 88 && addr.s_lh == 99) {
				//cout << "6 to 4 anycast relays address " << inet_ntoa(addr) << endl;
			} else if (addr.s_net == 198 && (addr.s_host == 18 || addr.s_lh == 19)) {
				//cout << "inter-network comms address " << inet_ntoa(addr) << endl;
			} else if (addr.s_net >= 224 && addr.s_net <= 240) {
				//cout << "Multicast / Limited broadcast address " << inet_ntoa(addr) << endl;
			} else {
				if (current_host.S_un.S_addr == NULL) {
					// cout << "current host is null, copying in some values." << endl;
					memcpy(&current_host, localHost->h_addr_list[i], sizeof(struct in_addr));
					sprintf_s(active_address, "%s", inet_ntoa(addr));
				} else {
					// we have a value in current host
					// do some handling for lan addresses
					if ((addr.s_net == 192 && addr.s_host == 168) || addr.s_net == 10 || (addr.s_net == 177 && addr.s_host >= 16 && addr.s_host <= 31)) {
						// we have what looks like a local lan addres
						// 192.168.xx.xx or 10.xx.xx.xx or 176.16.xx.xx to 176.31.xx.xx
						if ((current_host.s_net != 192 || current_host.s_host != 168) && (current_host.s_net != 10) && (current_host.s_net != 176))  {
							// our current address does not look like a lan address, so set it.
							memcpy(&current_host, localHost->h_addr_list[i],sizeof(struct in_addr));
							//cout << "We have a new lan address." << endl;
						}
					}
					// now update for better (lower) addresses on the same net/host range
					if (addr.s_net == current_host.s_net && addr.s_host == current_host.s_host && ((addr.s_lh < current_host.s_lh) || (addr.s_lh <= current_host.s_lh && addr.s_impno <= current_host.s_impno))) {
						memcpy(&current_host, localHost->h_addr_list[i],sizeof(struct in_addr));
						//cout << "We have an updated better address." << endl;
					}					
				}
				sprintf_s(active_address, "%s", inet_ntoa(current_host));
			}
			cout << "MySEQServer: Listening on " << inet_ntoa(addr) << ":" << dec << port << endl;
		}

		// Set the port in the gui
		if (h_MySEQServer != NULL) {
			char buffer[65];
			_itoa_s( port, buffer, 65, 10);
			SetDlgItemText(h_MySEQServer, IDC_TEXT_PORT, (LPCSTR) &buffer);
			// Set the best ip address in the gui
			SetDlgItemText(h_MySEQServer, IDC_TEXT_PRIMARY, (LPCSTR) &active_address);
		}
	}
	return true;
}

void NetworkServer::openClientSocket()
{
	// Wait for incoming connections
	sockClient = accept(sockListener, &sockAddr, &sockAddrSize);
	
	if (sockClient == INVALID_SOCKET)
	{
		MessageBox(NULL, "Error: NetworkServer: Error with connection request.", "Error with connection request", 0);
		ostrstream strm;
		strm << "Error: NetworkServer: Error with connection request 0x" << dec << WSAGetLastError() << ends ;
		throw Exception(EXCLEV_ERROR, strm.str());
	}
	zoneName = "StartUp";
	if (h_MySEQServer) {
		SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, _T(""));
		SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, _T(""));
	}
			
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
	// get current character name
	string rtn = "";
	UINT pTemp = 0;
	UINT nameOff;
			
	if (offsets[OT_self])
		pTemp = mr_intf->extractRAWPointer(offsets[OT_self]);
	
    nameOff = spawnParser.offsets[spawnParser.OT_name];	
	if (pTemp)
	{
		rtn = mr_intf->extractString(pTemp + nameOff);
	}
	return rtn;
}

void NetworkServer::setOffset(offset_types ot, UINT value)
{
	offsets[ot] = value;
}

void NetworkServer::init(IniReaderInterface* ir_intf)
{
	spawnParser.init(ir_intf);
	itemParser.init(ir_intf);
	worldParser.init(ir_intf);
	
	port = ir_intf->readIntegerEntry("Port", "Port");
	
	setOffset(OT_spawnlist,	ir_intf->readIntegerEntry("Memory Offsets", "SpawnHeaderAddr"));
	setOffset(OT_self,		ir_intf->readIntegerEntry("Memory Offsets", "CharInfo"));
	setOffset(OT_target,	ir_intf->readIntegerEntry("Memory Offsets", "TargetAddr"));
	setOffset(OT_zonename,	ir_intf->readIntegerEntry("Memory Offsets", "ZoneAddr"));
	setOffset(OT_ground,	ir_intf->readIntegerEntry("Memory Offsets", "ItemsAddr"));
	setOffset(OT_world,		ir_intf->readIntegerEntry("Memory Offsets", "WorldAddr"));
	
	if (h_MySEQServer) {
		// Set offsets in GUI
		TCHAR ot_spawnlist[16];
		TCHAR ot_self[16];
		TCHAR ot_target[16];
		TCHAR ot_zonename[16];
		TCHAR ot_ground[16];
		TCHAR ot_world[16];
		// Put in our offsets in a hex format
		sprintf_s(ot_spawnlist,"0x%x",offsets[OT_spawnlist]);
		sprintf_s(ot_self,"0x%x",offsets[OT_self]);
		sprintf_s(ot_target,"0x%x",offsets[OT_target]);
		sprintf_s(ot_zonename,"0x%x",offsets[OT_zonename]);
		sprintf_s(ot_ground,"0x%x",offsets[OT_ground]);
		sprintf_s(ot_world,"0x%x",offsets[OT_world]);
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
		return (UINT)offsets[(offset_types)type]; 
}

bool NetworkServer::processReceivedData(MemReaderInterface* mr_intf)
{
	int bytesRecvd, maxLoop;
	UINT pTemp, pTemp2;
	string newZoneName;
	int numSpawns = 0, numItems = 0, numElements;
	
	quickInfo = false;

	if (LastProcess != mr_intf->getCurrentPID()) {
		quickInfo = true;
		LastProcess = mr_intf->getCurrentPID();
	}	
		
	// Wait for request from the client
	bytesRecvd = recv(sockClient, (char*)&clientRequest, sizeof(clientRequest), 0);
	
	if (bytesRecvd == 0 || bytesRecvd == SOCKET_ERROR || bytesRecvd != sizeof(clientRequest))
	{
		return true;
	}

	if (change_process) // The last request was to change the process.  So this packet
						// should contain the process id, to switch to
	{
		DWORD originalPID = mr_intf->getCurrentPID();
		cout << "MySEQServer: Setting process to 0x" << hex <<  clientRequest << endl;
		mr_intf->openFirstProcess("eqgame", false);
		while (1)
		{
			if (mr_intf->getCurrentPID() == (DWORD)clientRequest) {
				zoneName = "StartUp";
				SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, _T(""));
				SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, _T(""));
				break;
			}
			if (!mr_intf->openNextProcess("eqgame"), false)
				break;
		}
		if (mr_intf->getCurrentPID() != (DWORD)clientRequest)
		{
			// we failed to connect to the desired PID
			// lets see if we can open a process
			if (mr_intf->openFirstProcess("eqgame", false))
			{
				if (originalPID != mr_intf->getCurrentPID())
				{
					zoneName = "StartUp";
					SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, _T(""));
					SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, _T(""));
				}
			}
		}
		// the client no longer expects a return packet for this.
		// the client continue requesting data the next tick().
		//spawnParser.packNetBufferRaw(OPT_process, mr_intf->getCurrentPID());
		//spawnParser.pushNetBuffer();
		change_process = false;
		return false;
	}

	if ( !mr_intf->isValid() ) {
		// we dont have a valid process
		// send a reply to keep stream connected

		spawnParser.packNetBufferRaw(OPT_process, 0);
		spawnParser.pushNetBuffer();
		// First send the number of elements
		numElements = spawnParser.getNetBufferSize();
		send(sockClient, (char*) &numElements, sizeof(numElements), 0);
		
		// Now sent the array of elements
		if ( numElements )
		{
			// Show the spawncount (minus the zonename and yourself, if there is targeted mob we will get one extra)
			if (quickInfo)
				cout << "MySEQServer: numSpawns(" << dec << numSpawns << ") numItems(" << numItems << ")" << endl;
			
			send(sockClient, (char*) spawnParser.getNetBufferStart(), numElements * sizeof(netBuffer_t), 0);
			spawnParser.clearNetBuffer();
			numSpawns = numItems = 0;
		}

		return false;
		//return true;  // return true, if not good receive
	}

	// Send get_process requests
	if ( requestContains(IPT_getproc) )
	{
		pTemp = 0;
		MemReader tempMemReader;
			
		// Look for the first available process match
		tempMemReader.openFirstProcess("eqgame");
		while (tempMemReader.isValid())
		{
			if (offsets[OT_self])
				pTemp = tempMemReader.extractRAWPointer(offsets[OT_self]);
			
			if (pTemp)
			{
				if (tempMemReader.extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset))
				{
					// For this type of packet, we only use the gamer name and the PID
					spawnParser.packNetBufferRaw(OPT_process, tempMemReader.getCurrentPID());
					spawnParser.pushNetBuffer();
				}
			}
				
			// Look for the next available process match
			if (!tempMemReader.openNextProcess("eqgame"))
				break;
		}
	}

	// Send set_process requests
	if ( requestContains(IPT_setproc) )
	{
		change_process = true;
		return false;
		// client does not expect a return packet now

	}

	// Send zonename requests
	if ( requestContains(IPT_zone) )
	{
		if (offsets[OT_zonename])
			newZoneName = mr_intf->extractString2(offsets[OT_zonename] - 0x400000 + (UINT)mr_intf->getCurrentBaseAddress());
				
		// Only send zonename response if zone changed
		if ( newZoneName != zoneName )
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
				} else {
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
		
	// Send player requests
	if ( requestContains(IPT_self) )
	{
		pTemp = 0;
			
		if (offsets[OT_self])
			pTemp = mr_intf->extractRAWPointer(offsets[OT_self]);
				
		if (quickInfo)
			cout << "MySEQServer: pSelf is 0x" << hex << pTemp << endl;
			
		if (pTemp)
			if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset))
			{
				spawnParser.packNetBufferRaw(OPT_self, pTemp);
				spawnParser.pushNetBuffer();
			}
	}
		
	// Send spawnlist requests
	if ( requestContains(IPT_spawns) )
	{
		pTemp = pTemp2 = 0;
			
		if (offsets[OT_spawnlist])
			pTemp = pTemp2 = mr_intf->extractRAWPointer(offsets[OT_spawnlist]);
				
		/* As of TSS, after shrouds or hover, this pointer may point to a spawn in the
			middle of the list. Back up to the top of the spawn list just to be sure we
			grab the whole thing. */
		for (maxLoop=0; maxLoop<2000; maxLoop++)
		{
			if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset))
			{
				if (spawnParser.extractPrevPointer())
					pTemp = spawnParser.extractPrevPointer();
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
				cout << "MySEQServer: pSpawnlist changed from INI setting of 0x" << hex << pTemp2 << endl;
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
		if (loopCount > 20) {
			// Update the gui with the spawn/item counts
			if (offsets[OT_spawnlist])
			pTemp = mr_intf->extractRAWPointer(offsets[OT_spawnlist]);
		
			// Get us to the top of the list
			for (maxLoop=0; maxLoop<2000; maxLoop++)
			{
				if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset))
				{
					if (spawnParser.extractPrevPointer())
						pTemp = spawnParser.extractPrevPointer();
					else
						break;
				}
			}
			UINT typeOffset = spawnParser.offsets[spawnParser.OT_type];
			BYTE result;
			int pcNum=0, npcNum=0, corpseNum=0;
			while (pTemp)
			{
				if (mr_intf->extractToBuffer(pTemp, spawnParser.rawBuffer, spawnParser.largestOffset))
				{
					result = spawnParser.extractRawByte(spawnParser.OT_type);
					switch(result)
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
			_itoa_s( npcNum, buffer, 65, 10);
			SetDlgItemText(h_MySEQServer, IDC_TEXT_SPAWNS, (LPCSTR) &buffer);
			_itoa_s( pcNum, buffer, 65, 10);
			SetDlgItemText(h_MySEQServer, IDC_TEXT_SPAWNS2, (LPCSTR) &buffer);
			_itoa_s( corpseNum, buffer, 65, 10);
			SetDlgItemText(h_MySEQServer, IDC_TEXT_SPAWNS3, (LPCSTR) &buffer);
		}
	}

		// Send target requests
	if ( requestContains(IPT_target) )
	{
		pTemp = 0;
			
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
			} else {
				// Send target of spawn ID 99999 (no target)
				spawnParser.packNetBufferEmpty(OPT_target, pTemp);
				spawnParser.pushNetBuffer();
			}
		} else {
				// Send target of spawn ID 99999 (no target)
				spawnParser.packNetBufferEmpty(OPT_target, pTemp);
				spawnParser.pushNetBuffer();
			}
	}		

	// Send grounditem requests
	if ( requestContains(IPT_ground) )
	{
		pTemp = pTemp2 = 0;
		UINT nameOff;
		string itName = "";
		nameOff = itemParser.offsets[itemParser.OT_name];
			
		if (offsets[OT_ground])
			pTemp2 = mr_intf->extractRAWPointer(offsets[OT_ground]);

		if (pTemp2)
			itName = mr_intf->extractString(pTemp2 + nameOff);

		if (itName.compare(0,2,"IT") == 0) {
			pTemp = pTemp2;
		} else if (pTemp2) {
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
				if ((numItems > 300) || (pTemp == itemParser.extractNextPointer()) )
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
			_itoa_s( numItems, buffer, 65, 10);
			SetDlgItemText(h_MySEQServer, IDC_TEXT_ITEMS, (LPCSTR) &buffer);

			loopCount = 0;
		}
	}
				
	// Send world requests
	if ( requestContains(IPT_world) )
	{
		pTemp = 0;
			
		if (offsets[OT_world])
			pTemp = mr_intf->extractRAWPointer(offsets[OT_world]);
				
		if (quickInfo)
			cout << "MySEQServer: pWorldInfo is 0x" << hex << pTemp << endl;

		if (pTemp)
			pTemp = pTemp;

		if (pTemp)
			if (mr_intf->extractToBuffer(pTemp, worldParser.rawBuffer, worldParser.largestOffset))
			{
				worldParser.packWorldBuffer(OPT_world);
				spawnParser.packNetBufferWorld(worldParser);
				spawnParser.pushNetBuffer();
			}
	}
		
	// Send spawn information (most other information is also packed into a spawn structure)
	numElements = spawnParser.getNetBufferSize();
		
	// First send the number of elements
	send(sockClient, (char*) &numElements, sizeof(numElements), 0);
		
	// Now sent the array of elements
	if ( numElements )
	{
		// Show the spawncount (minus the zonename and yourself, if there is targeted mob we will get one extra)
		if (quickInfo)
			cout << "MySEQServer: numSpawns(" << dec << numSpawns << ") numItems(" << numItems << ")" << endl;
			
		send(sockClient, (char*) spawnParser.getNetBufferStart(), numElements * sizeof(netBuffer_t), 0);
		spawnParser.clearNetBuffer();
		numSpawns = numItems = 0;
	}
	return false;
}