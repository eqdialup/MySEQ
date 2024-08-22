/*
 * Smart EQ Offset Finder - GPL Edition
 * Copyright 2007-2009, Carpathian <Carpathian01@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#ifndef EQGAMESCANNER_H
#define EQGAMESCANNER_H

#include <string>
#include <windows.h> // Provides "Windows Style" Type Definitions
#include "IniReader.h"
#include "NetworkServer.h"
#include "resource.h"
#include <tuple>
#include <vector>
#include <string>

class EQGameScanner
{
public:
	EQGameScanner();
	~EQGameScanner();

	bool executableExists() const;

	void setExe(TCHAR* str);

	DWORD findEQPointerOffset(DWORD startAddress, std::size_t blockSize, const PBYTE byteMask, const PCHAR charMask);

	DWORD findEQStructureOffset(DWORD startAddress, std::size_t blockSize, const PBYTE byteMask, const PCHAR charMask, const QWORD baseEQPointerAddress);

	bool CheckAndWriteOffset(HWND hDlg, IniReaderInterface* ir_intf, NetworkServerInterface* net_intf, const std::string& section, const std::string& entry, NetworkServer::OffsetType offsetType, bool write_out, std::ostringstream& outputStream);

	void HandleMismatch(HWND hDlg, IniReaderInterface* ir_intf, const std::string& entry, DWORD matchAddr, bool write_out, std::ostringstream& outputStream);

	std::string ToHexString(DWORD value);

	bool ScanExecutable(HWND hDlg, IniReaderInterface* ir_intf, NetworkServerInterface* net_intf, bool write_out = false);

	std::string GetFileInfo(IniReaderInterface* ir_intf, bool write_out);

	void ScanSecondary(HWND hDlg, IniReaderInterface* ir_intf, NetworkServerInterface* net_intf);

private:
	bool compareData(PBYTE data, PBYTE byteMask, PCHAR charMask);

private:
	std::string executablePath;
};

#endif // EQGAMESCANNER_H
