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

#include "Common.h"
#include "IniReader.h"
#include "NetworkServer.h"
#include "resource.h"

class EQGameScanner
{
public:
	EQGameScanner();
	~EQGameScanner();

	bool executableExists() const;

	void setExe(TCHAR* str);

	DWORD findEQPointerOffset(DWORD startAddress, std::size_t blockSize, const PBYTE byteMask, const PCHAR charMask);

	DWORD findEQStructureOffset(DWORD startAddress, std::size_t blockSize, const PBYTE byteMask, const PCHAR charMask, const QWORD baseEQPointerAddress);

	bool ScanExecutable(HWND hDlg, IniReaderInterface* ir_intf, NetworkServerInterface* net_intf, bool write_out = false);

	void ScanSecondary(HWND hDlg, IniReaderInterface* ir_intf, NetworkServerInterface* net_intf);

private:
	bool compareData(PBYTE data, PBYTE byteMask, PCHAR charMask);
	void updateFileInfoSection(std::ostringstream& outputStream, IniReaderInterface* ir_intf, bool write_out);
	DWORD findAndProcessOffset(HWND hDlg, const std::string& section, const std::string& entry, IniReaderInterface* ir_intf, NetworkServerInterface* net_intf, std::ostringstream& outputStream, bool write_out);
	void handleMatchResult(HWND hDlg, DWORD matchAddr, NetworkServerInterface* net_intf, int offsetType, const std::string& offsetName, IniReaderInterface* ir_intf, std::ostringstream& outputStream, bool write_out, bool& reload);

	std::string executablePath;
};

#endif // EQGAMESCANNER_H
