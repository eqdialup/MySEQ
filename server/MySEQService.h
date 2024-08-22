#pragma once

#include "resource.h"
#include "Common.h"
#include "IniReader.h"
#include "MemReader.h"
#include "NetworkServer.h"
#include "EQGameScanner.h"
#include "Debugger.h"
#include "MySEQ.server.h"
class MySEQService
{
public:
	bool bRunning;
	void WINAPI ServiceMain(DWORD argc, LPTSTR* argv);

	void WINAPI ServiceCtrlHandler(DWORD Opcode);
	BOOL InstallService();
	BOOL DeleteService();

	LPCTSTR lpszServiceName = "MySEQServer";
	LPCTSTR lpszServiceNameDisplay = "MySEQ Open";
	SERVICE_STATUS m_ServiceStatus;
	SERVICE_STATUS_HANDLE m_ServiceStatusHandle;

};