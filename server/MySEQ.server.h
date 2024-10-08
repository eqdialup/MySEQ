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

#include "resource.h"
#include "Common.h"
#include "IniReader.h"
#include "MemReader.h"
#include "NetworkServer.h"
#include "EQGameScanner.h"
#include "Debugger.h"

using namespace std;

bool debug_mode;
bool console_mode;
bool services;

bool otherini;

IniReader iniReader;

char iniFile[_MAX_PATH + 1];
char configIniFile[_MAX_PATH + 1];

int server_status;
int check_delay;

void ReadArgs(int argc, char* argv[]);

Debugger debugger;

MemReader memReader;

NetworkServer netServer;

EQGameScanner scanner;

void Minimize();

void Restore();

void InitNotifyIconData();

void ToggleStartMinimized();

void HandleColorStatic(HWND hDlg, WPARAM wParam, LPARAM lParam);
void HandleCommand(HWND hDlg, WPARAM wParam);
void HandleTrayIcon(HWND hDlg, WPARAM wParam, LPARAM lParam);
void MinimizeWindow(HWND hDlg);
void ShowPopupMenu(HWND hDlg);
void RestoreWindow(HWND hDlg);

bool CheckAndSetEQPath(const TCHAR* basePath, const TCHAR* subDir, TCHAR* eqFilePath, size_t pathSize, TCHAR* eqFileName, size_t fileNameSize, TCHAR* eqExeName, size_t exeNameSize);
void SetInitialDirectory(TCHAR* eqFilePath, size_t pathSize, TCHAR* eqExeName, OPENFILENAME& ofn);
void SetEQFileName(HWND hDlg, TCHAR* eqExeName, TCHAR* eqFileName);

bool InstallAndNotify();
bool UninstallAndNotify();
void startService();
const char* INSTALLER_TITLE = "MySEQ Open Service Installer";
const char*  INSTALL_ERROR_MSG = "Error Installing MySEQ Open Services";
const char* INSTALL_SUCCESS_MSG = "MySEQ Open Services Installed Successfully";
const char*  UNINSTALLER_TITLE = "MySEQ Open Service Uninstaller";
const char*  UNINSTALL_ERROR_MSG = "Error Uninstalling MySEQ Open Services";
const char* UNINSTALL_SUCCESS_MSG = "MySEQ Open Services Uninstalled Successfully";