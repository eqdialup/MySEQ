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

#include "MySEQ.server.h"
#include "EQGameScanner.h"

using namespace std;

// Credits:
//
// Code for minimizing to system tray gracefully borrowed from
// bobobobo's weblog http://bobobobo.wordpress.com
// which takes code from http://www.gidforums.com/t-5815.html
//
// Changing dialog extended style, so button for dialog shows in taskbar:
// Code taken from #Winprog FAQ http://winprog.org/faq

#define MAX_LOADSTRING 100

#define ID_TRAY_EXIT_CONTEXT_MENU_ITEM  3000
#define ID_TRAY_OPEN_CONTEXT_MENU_ITEM  3001
#define ID_TRAY_START_MINIMIZED_MENU_ITEM 3002
#define ID_TRAY_SEPARATOR_MENU_ITEM 3003
#define ID_TRAY_APP_ICON 6000
#define WM_TRAYICON ( WM_USER + 1 )

UINT WM_TASKBARCREATED = 0;

//#define _DEBUG_SERVICE

// Services Variables
LPCTSTR lpszServiceName = "MySEQServer";
LPCTSTR lpszServiceNameDisplay = "MySEQ Open";
SERVICE_STATUS m_ServiceStatus;
SERVICE_STATUS_HANDLE m_ServiceStatusHandle;

// background color
HBRUSH g_hbrBackground = GetSysColorBrush(COLOR_MENU);
bool bRunning;

// Global Variables:
HINSTANCE hInst;	// current instance
bool running;

HWND h_Main = NULL;
HWND h_MySEQServer = NULL;
HWND h_EQGameScanner = NULL;
HWND h_MyConsole = NULL;
HWND h_NotePad = NULL;
HMENU g_menu;

TCHAR eqFileName[_MAX_PATH];
TCHAR eqFilePath[_MAX_PATH];
TCHAR eqExeName[_MAX_PATH];

STARTUPINFO siStartupInfo;
PROCESS_INFORMATION piProcessInfo;

NOTIFYICONDATA g_notifyIconData;

BOOL kill_console = FALSE;

TCHAR szTitle[MAX_LOADSTRING];
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name

int nClients = 0;

// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	ServerDialog(HWND, UINT, WPARAM, LPARAM);
bool FindNotepadPath(TCHAR(&notePad)[MAX_PATH]);
INT_PTR CALLBACK	OffsetDialog(HWND, UINT, WPARAM, LPARAM);
void GetPatchdate();
BOOL WINAPI CtrlHandler(DWORD dwCtrlType);

void DoDebugLoop(void* dummy);

// Services Functions
void WINAPI ServiceMain(DWORD argc, LPTSTR* argv);

void WINAPI ServiceCtrlHandler(DWORD Opcode);

BOOL InstallService();

BOOL DeleteService();

void WINAPI ServiceMain(DWORD argc, LPTSTR* argv)
{
	m_ServiceStatus.dwServiceType = SERVICE_WIN32;

	m_ServiceStatus.dwCurrentState = SERVICE_START_PENDING;

	m_ServiceStatus.dwControlsAccepted = SERVICE_ACCEPT_STOP;

	m_ServiceStatus.dwWin32ExitCode = 0;

	m_ServiceStatus.dwServiceSpecificExitCode = 0;

	m_ServiceStatus.dwCheckPoint = 0;

	m_ServiceStatus.dwWaitHint = 0;

	m_ServiceStatusHandle = RegisterServiceCtrlHandler(lpszServiceName, ServiceCtrlHandler);

	if (m_ServiceStatusHandle == (SERVICE_STATUS_HANDLE)0)
	{
		return;
	}

	m_ServiceStatus.dwCurrentState = SERVICE_RUNNING;

	m_ServiceStatus.dwCheckPoint = 0;

	m_ServiceStatus.dwWaitHint = 0;

	if (!SetServiceStatus(m_ServiceStatusHandle, &m_ServiceStatus))
	{
	}

	bRunning = true;

	while (bRunning)
	{
		// Start up service

		memReader.enableDebugPrivileges();

		iniReader.openFile(iniFile);
		iniReader.openConfigFile(configIniFile);

		netServer.init(&iniReader);

		netServer.closeListenerSocket();

		// A service will sit here on this step waiting for a client to connect.
		bRunning = netServer.openListenerSocket(true);

		netServer.openClientSocket();

		// since we are non-blocking, for services, we will reload the ini
		// any new connection.  That way restarting services is not necessary
		// if the offsets are updated.

		iniReader.openFile(iniFile);

		iniReader.openConfigFile(configIniFile);

		netServer.init(&iniReader);

		if (memReader.openFirstProcess("eqgame"))
		{
			netServer.enterReceiveLoop(&memReader);
		}

		netServer.closeClientSocket();

		netServer.closeListenerSocket();

		WSACleanup();

		Sleep(3000);
	}

	return;
}

void WINAPI ServiceCtrlHandler(DWORD Opcode)
{
	switch (Opcode)

	{
	case SERVICE_CONTROL_PAUSE:

		m_ServiceStatus.dwCurrentState = SERVICE_PAUSED;

		break;

	case SERVICE_CONTROL_CONTINUE:

		m_ServiceStatus.dwCurrentState = SERVICE_RUNNING;

		break;

	case SERVICE_CONTROL_STOP:

		m_ServiceStatus.dwWin32ExitCode = 0;

		m_ServiceStatus.dwCurrentState = SERVICE_STOPPED;

		m_ServiceStatus.dwCheckPoint = 0;

		m_ServiceStatus.dwWaitHint = 0;

		SetServiceStatus(m_ServiceStatusHandle, &m_ServiceStatus);

		bRunning = false;

		break;

	case SERVICE_CONTROL_INTERROGATE:

		break;
	}

	return;
}

BOOL InstallService()
{
	SERVICE_DESCRIPTION sd;
	LPTSTR szDesc = TEXT("This is the MySEQ Open Service for Everquest");
	SC_HANDLE schSCManager;
	SC_HANDLE schService;
	TCHAR ServiceAppPath[MAX_PATH + 1];

	GetModuleFileNameA(0, ServiceAppPath, MAX_PATH);

	strcat_s(ServiceAppPath, " -k");

	schSCManager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);

	if (schSCManager == NULL) {
		return false;
	}
	LPCTSTR lpszBinaryPathName = ServiceAppPath;

	schService = CreateService(schSCManager, lpszServiceName,

		lpszServiceNameDisplay, // service name to display

		SERVICE_ALL_ACCESS, // desired access

		SERVICE_WIN32_OWN_PROCESS, // service type

		SERVICE_AUTO_START, // start type

		SERVICE_ERROR_NORMAL, // error control type

		lpszBinaryPathName, // service's binary

		NULL, // no load ordering group

		NULL, // no tag identifier

		NULL, // no dependencies

		NULL, // LocalSystem account

		NULL); // no password

	if (schService != NULL) {
		sd.lpDescription = szDesc;
		ChangeServiceConfig2(schService, SERVICE_CONFIG_DESCRIPTION, &sd);
	}

	if (schService == NULL) {
		return false;
	}

	CloseServiceHandle(schService);

	return true;
}

BOOL DeleteService()
{
	SC_HANDLE schSCManager;
	SC_HANDLE hService;

	schSCManager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);

	if (schSCManager == NULL)
		return false;

	hService = OpenService(schSCManager, lpszServiceName, SERVICE_ALL_ACCESS);

	if (hService == NULL)
		return false;

	if (DeleteService(hService) == 0)
		return false;

	if (CloseServiceHandle(hService) == 0)
		return false;

	return true;
}

// Entry Point for WinMain

int APIENTRY _tWinMain(_In_   HINSTANCE hInstance,
	_In_opt_ HINSTANCE hPrevInstance,
	_In_     LPTSTR    lpCmdLine,
	_In_     int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

	MSG msg = { 0 };
	HACCEL hAccelTable;

	// This is trigger for re-creating system tray icon if taskbar is restarted while minimized
	WM_TASKBARCREATED = RegisterWindowMessageA("TaskbarCreated");
	server_status = 0;
	check_delay = 0;
	// Read command line parameters

	int argc = 0;
	char** argv = NULL;

	argc = __argc;
	argv = __argv;

	ReadArgs(argc, argv);

	string arg;

	if (argc > 1)
	{
		arg = argv[1];
		if (arg == "-i")
		{
			DeleteService();

			if (InstallService())
				MessageBox(NULL, "MySEQ Open Services Installed Sucessfully", "MySEQ Open Service Installer", 0);
			else
				MessageBox(NULL, "Error Installing MySEQ Open Services", "MySEQ Open Service Installer", 0);

			return 0;
		}

		if (arg == "-d")
		{
			if (DeleteService())
				MessageBox(NULL, "MySEQ Open Services UnInstalled Sucessfully", "MySEQ Open Service Uninstaller", 0);
			else
				MessageBox(NULL, "Error UnInstalling MySEQ Open Services", "MySEQ Open Service Uninstaller", 0);

			return 0;
		}

		if (arg == "-k")
		{
			SERVICE_TABLE_ENTRY DispatchTable[] = { {"MySEQServer",ServiceMain},{NULL,NULL} };

			StartServiceCtrlDispatcher(DispatchTable);

			return 0;
		}
	}

	// Initialize global strings
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_MYSEQSERVER, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// Perform application initialization:
	if (!InitInstance(hInstance, nCmdShow))
	{
		return FALSE;
	}

	if (argc > 1 && (!console_mode && !debug_mode && !services && !otherini))
	{
		string arg = argv[1];

		cout << "   Usage: server debug" << endl;
		cout << "          server console" << endl;
		cout << "          server -f [IniFileName]" << endl;
		cout << "          server -i" << endl;
		cout << "          server -d" << endl << endl;
		cout << "      debug - enter debug command line interface" << endl << endl;
		cout << "      console - run server as a console" << endl << endl;
		cout << "      -f   - Load alternate Ini FileName" << endl << endl;
		cout << "      -i   - Install server.exe as a service" << endl << endl;
		cout << "      -d   - Removed MySEQ server.exe which was installed as service." << endl << endl;

		ostrstream strm;

		strm << "Warning: Unknown parameters(s) '" << arg << "'" << ends;

		return 0;
	}

	if (debug_mode) {
		cout << "========================" << endl <<
			" MySEQ Server Debug Mode" << endl <<
			"========================" << endl << endl;
	}
	else if (!services) {
		cout << "========================" << endl <<
			"  MySEQServer v3.0.0.0  " << endl <<
			"========================" << endl << endl <<
			"This software is covered under the GNU Public License (GPL)" << endl <<
			"Copyright MySEQ Project 2003-2022" << endl << endl;
	}

	// Debug priviledges allow us to peek inside the EQ process.
	memReader.enableDebugPrivileges();

	iniReader.openFile(iniFile);

	iniReader.openConfigFile(configIniFile);

	netServer.hwnd = h_Main;

	if (h_MySEQServer)
		netServer.h_MySEQServer = h_MySEQServer;

	if (!debug_mode) {
		netServer.init(&iniReader);

		running = netServer.openListenerSocket(false);

		server_status = 1;
		if (h_MySEQServer) {
			GetPatchdate();
			SetDlgItemText(h_MySEQServer, IDC_TEXT_STATUS, "Listening");
			SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, "");
			SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, "");
		}

		if (iniReader.GetStartMinimized()) {
			Minimize();
		}
		else {
			Restore();
		}
	}

	hAccelTable = LoadAccelerators(hInst, MAKEINTRESOURCE(IDC_MYSEQSERVER));

	// debug runs the debug console in a new thread
	if (debug_mode) {
		memReader.openFirstProcess("eqgame");
		_beginthread(DoDebugLoop, 0, NULL);
	}
	int loop_counts = 0;

	// Main message loop:
	while (running) {
		GetMessage(&msg, NULL, 0, 0);
		if (h_MySEQServer) {
			if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg) && !IsDialogMessage(h_MySEQServer, &msg)) {
				TranslateMessage(&msg);
				DispatchMessage(&msg);
			}
		}
	}

	// debug never opens a listener socket
	if (!debug_mode) {
		netServer.closeClientSocket();

		netServer.closeListenerSocket();

		WSACleanup();
	}

	if (h_MySEQServer) {
		if (!IsWindowVisible(h_MySEQServer)) {
			Shell_NotifyIcon(NIM_DELETE, &g_notifyIconData);
		}
	}

	return (int)msg.wParam;
}

//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
//  COMMENTS:
//
//    This function and its usage are only necessary if you want this code
//    to be compatible with Win32 systems prior to the 'RegisterClassEx'
//    function that was added to Windows 95. It is important to call this function
//    so that the application will get 'well formed' small icons associated
//    with it.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = WndProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_MYSEQSERVER));
	wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW);
	wcex.lpszMenuName = NULL;
	wcex.lpszClassName = szWindowClass;
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_MYSEQSERVER));
	return RegisterClassEx(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
	HWND hWnd;
	hInst = hInstance; // Store instance handle in our global variable

#define WS_SERVERWINDOW     ( WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_BORDER | \
                             WS_MINIMIZEBOX)

	// This is the main window for the application.  We might add stuff to it later.  But not now.
	hWnd = CreateWindow(szWindowClass, szTitle, WS_SERVERWINDOW,
		CW_USEDEFAULT, CW_USEDEFAULT, 300, 250, NULL, NULL, hInstance, NULL);

	if (!hWnd) {
		return FALSE;
	}

	h_Main = hWnd; // store global hwnd

	running = true;

	// We are running a modeless dialog attached to the parent main window which is hidden.
	// We use the main window for network communictions
	if (!console_mode && !debug_mode) {
		h_MySEQServer = CreateDialog(hInstance, MAKEINTRESOURCE(IDD_SERVERBOX), hWnd, ServerDialog);
	}

	// Set window style of dialog so it shows a button in task bar, with main window hidden.
	// When the dialog is minimized, i minimizes to the system tray.
	// Code taken from #Winprog FAQ (winprog.org/faq)
	DWORD dwStyle, dwNewStyle;
	SetLastError(0);
	dwStyle = GetWindowLong(h_MySEQServer, GWL_EXSTYLE);
	dwNewStyle = (dwStyle & (~WS_EX_TOOLWINDOW)) | WS_EX_APPWINDOW;
	SetWindowLong(h_MySEQServer, GWL_EXSTYLE, dwNewStyle);
	SetWindowPos(h_MySEQServer, NULL, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOMOVE | SWP_NOSIZE);

	// for debug mode, we open a console for use
	if (debug_mode || console_mode) {
		AllocConsole();
		// Redirect the streams to the console
		FILE* stream;
		freopen_s(&stream, "CON", "w", stdout);
		std::cout.clear();
		freopen_s(&stream, "CON", "r", stdin);
		std::cin.clear();
		ios::sync_with_stdio();
	}

	ShowWindow(hWnd, SW_HIDE);
	UpdateWindow(hWnd);
	Sleep(100);

	h_MyConsole = GetConsoleWindow();

	// set up system tray
	memset(&g_notifyIconData, 0, sizeof(NOTIFYICONDATA));
	g_notifyIconData.cbSize = sizeof(NOTIFYICONDATA);

	/////
	// Tie the NOTIFYICONDATA struct to our
	// global HWND (that will have been initialized
	// before calling this function)
	g_notifyIconData.hWnd = h_MySEQServer;
	// Now GIVE the NOTIFYICON.. the thing that
	// will sit in the system tray, an ID.
	g_notifyIconData.uID = ID_TRAY_APP_ICON;
	// The COMBINATION of HWND and uID form
	// a UNIQUE identifier for EACH ITEM in the
	// system tray.  Windows knows which application
	// each icon in the system tray belongs to
	// by the HWND parameter.
	/////

	/////
	// Set up flags.
	g_notifyIconData.uFlags = NIF_ICON | // promise that the hIcon member WILL BE A VALID ICON!!
		NIF_MESSAGE | // when someone clicks on the system tray icon,
		// we want a WM_ type message to be sent to our WNDPROC
		NIF_TIP;      // we're gonna provide a tooltip as well, son.

	g_notifyIconData.uCallbackMessage = WM_TRAYICON;

	g_notifyIconData.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_MYSEQSERVER));

	// set the tooltip text.  must be LESS THAN 64 chars
	strcpy_s(g_notifyIconData.szTip, TEXT(szTitle));
	//IDS_APP_TITLE
	// Set dialog window icon
	if (h_MySEQServer) {
		SendMessage(h_MySEQServer, WM_SETICON, ICON_SMALL, (LPARAM)LoadIcon(hInstance, MAKEINTRESOURCE(IDI_MYSEQSERVER)));
		SendMessage(h_MySEQServer, WM_SETICON, ICON_BIG, (LPARAM)LoadIcon(hInstance, MAKEINTRESOURCE(IDI_MYSEQSERVER)));

		// Set caption title to be executable name
		TCHAR AppPath[MAX_PATH + 1];
		GetModuleFileName(0, AppPath, MAX_PATH);
		string::size_type index = string(AppPath).find_last_of("\\/");
		string myfilename = string(AppPath).substr(index + 1, string(AppPath).size()).c_str();
		LPCSTR thisfilename = myfilename.c_str();
		SetWindowText(h_MySEQServer, thisfilename);
	}

	if (debug_mode || console_mode) {
		SendMessage(h_MyConsole, WM_SETICON, ICON_SMALL, (LPARAM)LoadIcon(hInstance, MAKEINTRESOURCE(IDI_MYSEQSERVER)));
		SendMessage(h_MyConsole, WM_SETICON, ICON_BIG, (LPARAM)LoadIcon(hInstance, MAKEINTRESOURCE(IDI_MYSEQSERVER)));

		// Set caption title to be executable name
		TCHAR AppPath[MAX_PATH + 1];
		GetModuleFileName(0, AppPath, MAX_PATH);
		string::size_type index = string(AppPath).find_last_of("\\/");
		string myfilename = string(AppPath).substr(index + 1, string(AppPath).size()).c_str();
		LPCSTR thisfilename = myfilename.c_str();
		SetWindowText(h_MyConsole, thisfilename);
	}

	// Set up process info for starting a notepad edit session
	memset(&siStartupInfo, 0, sizeof(siStartupInfo));
	memset(&piProcessInfo, 0, sizeof(piProcessInfo));

	siStartupInfo.cb = sizeof(siStartupInfo);

	return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId;
	int wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;

	if (message == WM_TASKBARCREATED && h_MySEQServer != NULL && !IsWindowVisible(h_MySEQServer)) {
		Minimize();
		return 0;
	}

	switch (message)
	{
		//Winsock related message...
	case 1045:
		switch (lParam)
		{
		case FD_CONNECT: //Connected OK
			MessageBeep(MB_OK);
			fprintf(stdout, "New Connection Established.\n");
			netServer.closeClientSocket();

			break;

		case FD_CLOSE: //Lost connection
			MessageBeep(MB_ICONERROR);

			//Clean up
			netServer.closeClientSocket();
			server_status = 1;
			if (h_MySEQServer) {
				SetDlgItemText(h_MySEQServer, IDC_TEXT_STATUS, "Listening");
				SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, "");
				SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, "");
			}

			fprintf(stdout, "Connection to MySEQ Client Lost.\n");
			break;

		case FD_READ: //Incoming data to receive
			if (memReader.isValid()) {
				// we have a good eqgame.exe process
				check_delay = 0;
				server_status = 2;
			}
			else {
				server_status = 1;
				check_delay = (check_delay + 1) % 10;
				if (check_delay == 2) {
					if (!memReader.openFirstProcess("eqgame", false)) {
						if (h_MySEQServer) {
							SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, _T(""));
							SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, _T(""));
						}
					}
				}
			}
			if (netServer.processReceivedData(&memReader) == true)
				netServer.closeClientSocket();
			break;

		case FD_ACCEPT: //Incoming Client Connection
		{
			MessageBeep(MB_OK);
			if (memReader.getCurrentPID() == 0)
				memReader.openFirstProcess("eqgame", false);
			netServer.closeClientSocket();
			netServer.openClientSocket();
			if (memReader.getCurrentPID() == 0)
				server_status = 1;
			else
				server_status = 2;
			if (h_MySEQServer) {
				SetDlgItemText(h_MySEQServer, IDC_TEXT_STATUS, "Connected");
				check_delay = 0;
			}
		}
		break;

		case 658833440: // Connection crashed
		{
			netServer.closeClientSocket();
			server_status = 1;
			if (h_MySEQServer) {
				SetDlgItemText(h_MySEQServer, IDC_TEXT_STATUS, "Listening");
				SetDlgItemText(h_MySEQServer, IDC_TEXT_ZONE, "");
				SetDlgItemText(h_MySEQServer, IDC_TEXT_NAME, "");
			}

			fprintf(stdout, "Connection to MySEQ Client Lost.\n");
		}
		break;
		}
		break;
	case WM_CREATE:
		// create the tray icon context menu
		g_menu = CreatePopupMenu();
		AppendMenu(g_menu, MF_STRING, ID_TRAY_START_MINIMIZED_MENU_ITEM, "Start Minimized");
		AppendMenu(g_menu, MF_SEPARATOR, ID_TRAY_SEPARATOR_MENU_ITEM, "");
		AppendMenu(g_menu, MF_STRING, ID_TRAY_OPEN_CONTEXT_MENU_ITEM, TEXT("Open Server"));
		AppendMenu(g_menu, MF_STRING, ID_TRAY_EXIT_CONTEXT_MENU_ITEM, TEXT("Exit MySEQ"));

		break;
	case WM_COMMAND:
		wmId = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		// Add any drawing code here...
		SetTextColor(hdc, GetSysColor(COLOR_MENUTEXT));
		TextOut(hdc, 10, 10, "Patch Date:", 11);
		EndPaint(hWnd, &ps);
		break;

	case WM_CLOSE:
		DestroyWindow(hWnd);
		Shell_NotifyIcon(NIM_DELETE, &g_notifyIconData);
		//	Minimize() ;
		return 0;
		break;
	case WM_QUIT:
	case WM_DESTROY:
		running = false;
		FreeConsole();
		Shell_NotifyIcon(NIM_DELETE, &g_notifyIconData);
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

// Message handler for server dialog.
INT_PTR CALLBACK ServerDialog(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam) {
	UNREFERENCED_PARAMETER(lParam);

	switch (message) {
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_CTLCOLORDLG:
		return (INT_PTR)g_hbrBackground;

	case WM_CTLCOLORSTATIC:
		HandleColorStatic(hDlg, wParam, lParam);
		return (INT_PTR)g_hbrBackground;

	case WM_COMMAND:
		HandleCommand(hDlg, wParam);
		break;

	case WM_SYSCOMMAND:
		if (wParam == SC_MINIMIZE) {
			MinimizeWindow(hDlg);
			return (INT_PTR)TRUE;
		}
		break;

	case WM_TRAYICON:
		HandleTrayIcon(hDlg, wParam, lParam);
		break;

	default:
		return (INT_PTR)FALSE;
	}

	return (INT_PTR)FALSE;
}

// Handle WM_CTLCOLORSTATIC
void HandleColorStatic(HWND hDlg, WPARAM wParam, LPARAM lParam) {
	HDC hdcStatic = (HDC)wParam;
	switch (GetDlgCtrlID((HWND)lParam)) {
	case IDC_TEXT_STATUS:
		if (server_status == 0)  // Starting Up - Red
			SetTextColor(hdcStatic, RGB(255, 0, 0));
		else if (server_status == 1)  // Listening - Blue or Connected with no eqgame.exe found
			SetTextColor(hdcStatic, RGB(0, 0, 255));
		else  // Connected with EQGame - Green
			SetTextColor(hdcStatic, RGB(0, 255, 0));
		break;
	default:
		SetTextColor(hdcStatic, GetSysColor(COLOR_MENUTEXT));
		break;
	}
	SetBkMode(hdcStatic, TRANSPARENT);
}

// Handle WM_COMMAND
void HandleCommand(HWND hDlg, WPARAM wParam) {
	switch (LOWORD(wParam)) {
	case IDCLOSE:
	case IDCANCEL:
		EndDialog(hDlg, LOWORD(wParam));
		running = false;
		FreeConsole();
		break;

	case IDC_BUTTON1:  // Edit offsets
		DWORD dwStatus;
		GetExitCodeProcess(piProcessInfo.hProcess, &dwStatus);
		if (dwStatus != STILL_ACTIVE) {
			TCHAR notePad[MAX_PATH];
			if (FindNotepadPath(notePad)) {
				TCHAR commandLine[MAX_PATH];
				_stprintf_s(commandLine, _T(" %s"), iniFile);
				CreateProcess(notePad, commandLine, NULL, NULL, FALSE, CREATE_DEFAULT_ERROR_MODE, NULL, NULL, &siStartupInfo, &piProcessInfo);
			}
		}
		break;

	case IDC_BUTTON2:  // Reload offsets
		iniReader.openFile(iniFile);
		iniReader.openConfigFile(configIniFile);
		netServer.init(&iniReader);
		netServer.closeListenerSocket();
		running = netServer.openListenerSocket(false);
		GetPatchdate();  // Update patch date in GUI
		break;

	case IDC_BUTTON3:  // List local IP Addresses
		netServer.listIPAddresses();
		break;

	case IDC_BUTTON4:  // Show dialog for smart offset finder
		DialogBox(GetModuleHandle(NULL), MAKEINTRESOURCE(IDD_EQOFFSETSFINDER), h_MySEQServer, OffsetDialog);
		break;

	default:
		break;
	}
}

// Handle WM_SYSCOMMAND Minimize
void MinimizeWindow(HWND hDlg) {
	// Implement minimize behavior here
	Minimize();
}

// Handle WM_TRAYICON
void HandleTrayIcon(HWND hDlg, WPARAM wParam, LPARAM lParam) {
	switch (wParam) {
	case ID_TRAY_APP_ICON:
		// Handle tray icon interactions
		break;
	}

	if (lParam == WM_LBUTTONDBLCLK) {
		RestoreWindow(hDlg);
	}
	else if (lParam == WM_RBUTTONDOWN) {
		ShowPopupMenu(hDlg);
	}
}

// Restore Window from Tray
void RestoreWindow(HWND hDlg) {
	// Implement window restoration behavior here
	Restore();
}

// Show Popup Menu
void ShowPopupMenu(HWND hDlg) {
	POINT curPoint;
	GetCursorPos(&curPoint);
	SetForegroundWindow(hDlg);

	UINT clicked = TrackPopupMenu(g_menu,
		TPM_RETURNCMD | TPM_NONOTIFY,
		curPoint.x,
		curPoint.y,
		0,
		hDlg,
		NULL);

	switch (clicked) {
	case ID_TRAY_EXIT_CONTEXT_MENU_ITEM:
		Shell_NotifyIcon(NIM_DELETE, &g_notifyIconData);
		running = false;
		EndDialog(hDlg, (INT_PTR)clicked);
		FreeConsole();
		PostQuitMessage(0);
		break;

	case ID_TRAY_OPEN_CONTEXT_MENU_ITEM:
		RestoreWindow(hDlg);
		break;

	case ID_TRAY_START_MINIMIZED_MENU_ITEM:
		ToggleStartMinimized();
		CheckMenuItem(g_menu, ID_TRAY_START_MINIMIZED_MENU_ITEM,
			iniReader.GetStartMinimized() ? MF_CHECKED : MF_UNCHECKED);
		break;

	default:
		break;
	}
}

bool FindNotepadPath(TCHAR(&notePad)[MAX_PATH]) {
	TCHAR basePath[MAX_PATH];
	if (!GetWindowsDirectory(basePath, MAX_PATH)) {
		std::cout << "Windows directory lookup failed";
		return false;  // Return false if Windows directory lookup fails
	}

	std::vector<std::string> subDirs = { "", "\\System32", "\\SysWOW64" };
	for (const auto& dir : subDirs) {
		_stprintf_s(notePad, _T("%s%s\\NOTEPAD.exe"), basePath, dir.c_str());
		if (_access(notePad, 0) == 0) {
			return true;  // Return true if Notepad.exe is found
		}
	}

	std::cout << "Notepad.exe not found in default directories";
	return false;  // Return false if Notepad.exe is not found
}

INT_PTR CALLBACK OffsetDialog(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_INITDIALOG:
	{
		// attempt to set initial eqgame.exe values
		if (eqFileName[0] == _T('\0')) {
			TCHAR basePath[_MAX_PATH];
			TCHAR szChkFile[_MAX_PATH];
			TCHAR szPath[_MAX_PATH];
			if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_PROGRAM_FILES, NULL, 0, basePath))) {
				strcpy_s(szPath, basePath);
				strcat_s(szPath, "\\sony\\everquest");
				if (_access(szPath, 0) == 0) {
					strcpy_s(szChkFile, szPath);
					strcat_s(szChkFile, "\\eqgame.exe");
					if (_access(szPath, 0) == 0) {
						strcpy_s(eqFileName, szChkFile);
					}
				}
				if (eqFileName[0] == _T('\0')) {
					// attempt 2
					strcpy_s(szPath, basePath);
					strcat_s(szPath, "\\soe\\everquest");
					if (_access(szPath, 0) == 0) {
						strcpy_s(szChkFile, szPath);
						strcat_s(szChkFile, "\\eqgame.exe");
						if (_access(szPath, 0) == 0) {
							strcpy_s(eqFileName, szChkFile);
						}
					}
				}
				if (eqFileName[0] == _T('\0')) {
					// attempt 2
					strcpy_s(szPath, basePath);
					strcat_s(szPath, "\\everquest");
					if (_access(szPath, 0) == 0) {
						strcpy_s(szChkFile, szPath);
						strcat_s(szChkFile, "\\eqgame.exe");
						if (_access(szPath, 0) == 0) {
							strcpy_s(eqFileName, szChkFile);
						}
					}
				}
			}
		}

		if (eqFileName[0] != _T('\0'))
			SetDlgItemText(hDlg, IDC_EQFILENAME, eqFileName);

		return TRUE;
	}
	break;
	case WM_COMMAND:
		switch (LOWORD(wParam))
		{
		case IDOK:
			scanner.setExe(eqFileName);
			scanner.ScanExecutable(hDlg, &iniReader, &netServer);
			break;
		case IDC_BUTTON2:
			scanner.setExe(eqFileName);
			if (scanner.ScanExecutable(hDlg, &iniReader, &netServer, true))
			{
				// reload offsets
				iniReader.openFile(iniFile);
				netServer.init(&iniReader);

				// close and reopen listener socket, in case port changed
				netServer.closeListenerSocket();
				running = netServer.openListenerSocket(false);

				GetPatchdate();
			}
			break;
		case IDC_BUTTON3:
			scanner.setExe(eqFileName);
			scanner.ScanSecondary(hDlg, &iniReader, &netServer);
			break;
		case IDCANCEL:
			EndDialog(hDlg, IDCANCEL);
			break;
		case IDC_BUTTON1:
			GetDlgItemText(hDlg, IDC_EQFILENAME, eqFileName, _MAX_PATH);
			OPENFILENAME ofn;
			ZeroMemory(&ofn, sizeof(ofn));
			ofn.lStructSize = sizeof(ofn);
			ofn.hwndOwner = hDlg;
			ofn.lpstrFilter = "EverQuest Executable (eqgame.exe)\0eqgame.exe\0All Files (*.*)\0*.*\0";
			ofn.nMaxFile = _MAX_PATH;

			TCHAR basePath[_MAX_PATH];
			TCHAR szChkFile[_MAX_PATH];

			if (eqExeName[0] == _T('\0')) {
				if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_PROGRAM_FILES, NULL, 0, basePath))) {
					// Check default directories for EQ
					if (!CheckAndSetEQPath(basePath, _T("\\Sony Online Entertainment\\Installed Games\\EverQuest"), eqFilePath, _MAX_PATH, eqFileName, _MAX_PATH, eqExeName, _MAX_PATH) &&
						!CheckAndSetEQPath(basePath, _T("\\sony\\everquest"), eqFilePath, _MAX_PATH, eqFileName, _MAX_PATH, eqExeName, _MAX_PATH) &&
						!CheckAndSetEQPath(basePath, _T("\\soe\\everquest"), eqFilePath, _MAX_PATH, eqFileName, _MAX_PATH, eqExeName, _MAX_PATH) &&
						!CheckAndSetEQPath(basePath, _T("\\everquest"), eqFilePath, _MAX_PATH, eqFileName, _MAX_PATH, eqExeName, _MAX_PATH)) {

						GetCurrentDirectory(_MAX_PATH, eqFilePath);
						ofn.lpstrInitialDir = eqFilePath;
						_stprintf_s(szChkFile, _T("%s\\eqgame.exe"), eqFilePath);
						if (_access(szChkFile, 0) == 0) {
							_tcscpy_s(eqFileName, _MAX_PATH, szChkFile);
							_tcscpy_s(eqExeName, _MAX_PATH, _T("eqgame.exe"));
						}
					}
				}
			}
			SetInitialDirectory(eqFilePath, _MAX_PATH, eqExeName, ofn);

			if (GetOpenFileName(&ofn)) {
				SetEQFileName(hDlg, eqExeName, eqFileName);
			}
			break;
		}
		break;
	default:
		return FALSE;
	}
	return TRUE;
}

// Check if the directory exists and set EQ path
bool CheckAndSetEQPath(const TCHAR* basePath, const TCHAR* subDir, TCHAR* eqFilePath, size_t pathSize, TCHAR* eqFileName, size_t fileNameSize, TCHAR* eqExeName, size_t exeNameSize) {
	_stprintf_s(eqFilePath, pathSize, _T("%s%s"), basePath, subDir);
	if (_access(eqFilePath, 0) == 0) {
		_stprintf_s(eqFileName, fileNameSize, _T("%s\\eqgame.exe"), eqFilePath);
		if (_access(eqFileName, 0) == 0) {
			_tcscpy_s(eqExeName, exeNameSize, _T("eqgame.exe"));
			return true;
		}
	}
	return false;
}

// Set the initial directory for the file dialog
void SetInitialDirectory(TCHAR* eqFilePath, size_t pathSize, TCHAR* eqExeName, OPENFILENAME& ofn) {
	if (ofn.lpstrInitialDir == 0) {
		if (eqFilePath[0] == _T('\0')) {
			GetCurrentDirectory(static_cast<DWORD>(pathSize), eqFilePath);
		}
		ofn.lpstrInitialDir = eqFilePath;
	}
	ofn.lpstrFile = eqExeName;
	ofn.Flags = OFN_EXPLORER | OFN_FILEMUSTEXIST | OFN_HIDEREADONLY;
	ofn.lpstrDefExt = _T("exe");
}

// Set the EQ file name in the dialog
void SetEQFileName(HWND hDlg, TCHAR* eqExeName, TCHAR* eqFileName) {
	_tcscpy_s(eqFileName, _MAX_PATH, eqExeName);
	SetDlgItemText(hDlg, IDC_EDIT2, _T(""));
	SetDlgItemText(hDlg, IDC_EQFILENAME, eqFileName);
}

void GetPatchdate()
{
	// update patch date in GUI
	LPCSTR patchdate;
	patchdate = iniReader.patchDate.c_str();
	SetDlgItemText(h_MySEQServer, IDC_TEXT_PATCH, patchdate);
}

void ReadArgs(int argc, char* argv[])
{
	string arg;

	if (argc > 1)

		arg = argv[1];

	debug_mode = (arg == "debug");
#ifdef _DEBUG_CONSOLE
	debug_mode = TRUE;
#endif

	console_mode = (arg == "console");

	services = (arg == "-k");

	otherini = (arg == "-f");

	if ((arg == "-f") && (argc > 2))
	{
		string::size_type index = string(argv[2]).find_last_of("\\/");
		if (index != -1) {
			// assume that it contains entire path and file name
			strcpy_s(iniFile, argv[2]);
		}
		else {
			GetCurrentDirectory(_MAX_PATH, iniFile);

			strcat_s(iniFile, "\\");

			strcat_s(iniFile, argv[2]);
		}
		GetCurrentDirectory(_MAX_PATH, configIniFile);
		strcat_s(configIniFile, "\\config.ini");
	}
	else if (arg == "-k")
	{
		TCHAR AppPath[MAX_PATH + 1];
		string mypath;
		iniFile[0] = _T('\0');
		GetModuleFileName(0, AppPath, MAX_PATH);
		string::size_type index = string(AppPath).find_last_of("\\/");
		mypath = string(AppPath).substr(0, index);
		string(mypath)._Copy_s(iniFile, MAX_PATH, index, 0);
		string(mypath)._Copy_s(configIniFile, _MAX_PATH, index, 0);

		strcat_s(iniFile, "\\myseqserver.ini");
		strcat_s(configIniFile, "\\config.ini");

#ifdef _DEBUG_CONSOLE

		AllocConsole();
		// Redirect the streams to the console
		FILE* stream;
		freopen_s(&stream, "CON", "w", stdout);
		std::cout.clear();
		freopen_s(&stream, "CON", "r", stdin);
		std::cin.clear();

		ios::sync_with_stdio();

		printf("%s\n", AppPath);
		printf("%s\n", mypath);
		printf("%s\n", iniFile);
#endif
	}
	else
	{
		TCHAR AppPath[MAX_PATH + 1];
		string mypath;
		iniFile[0] = _T('\0');
		GetModuleFileName(0, AppPath, MAX_PATH);
		string::size_type index = string(AppPath).find_last_of("\\/");
		mypath = string(AppPath).substr(0, index);
		string(mypath)._Copy_s(iniFile, MAX_PATH, index, 0);
		string(mypath)._Copy_s(configIniFile, _MAX_PATH, index, 0);

		strcat_s(iniFile, "\\myseqserver.ini");
		strcat_s(configIniFile, "\\config.ini");
	}
}

void DoDebugLoop(void* dummy)
{
	debugger.enterDebugLoop(&memReader, &iniReader);

	running = false;

	ShowWindow(h_MyConsole, SW_HIDE);

	ExitProcess(3);
}

void Minimize()
{
	// Update whether the Start Minimized is checked.
	if (iniReader.GetStartMinimized())
		CheckMenuItem(g_menu, ID_TRAY_START_MINIMIZED_MENU_ITEM, MF_CHECKED);
	else
		CheckMenuItem(g_menu, ID_TRAY_START_MINIMIZED_MENU_ITEM, MF_UNCHECKED);

	// add the icon to the system tray
	Shell_NotifyIcon(NIM_ADD, &g_notifyIconData);

	// hide the console
	ShowWindow(h_MySEQServer, SW_HIDE);
}

void Restore()
{
	// Remove the icon from the system tray
	Shell_NotifyIcon(NIM_DELETE, &g_notifyIconData);

	// ..and show the window
	ShowWindow(h_MySEQServer, SW_SHOW);
}

void ToggleStartMinimized()
{
	iniReader.ToggleStartMinimized();
}