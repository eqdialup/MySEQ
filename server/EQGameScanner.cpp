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

#include "stdafx.h"
#include "EQGameScanner.h"

typedef uint64_t* PQWORD;

/*
 * Offset Value Storage
 */
namespace EQPrimaryOffsets
{
	QWORD Zone = 0x0;
	QWORD ZoneInfo = 0x0;
	QWORD SpawnHeader = 0x0;
	QWORD CharInfo = 0x0;
	QWORD Items = 0x0;
	QWORD Target = 0x0;
	QWORD World = 0x0;
};

namespace EQSpawnInfoOffsets
{
	QWORD Next = 0x0;
	QWORD Prev = 0x0;
	QWORD Lastname = 0x0;
	QWORD X = 0x0;
	QWORD Y = 0x0;
	QWORD Z = 0x0;
	QWORD Speed = 0x0;
	QWORD Heading = 0x0;
	QWORD Name = 0x0;
	QWORD Type = 0x0;
	QWORD SpawnId = 0x0;
	QWORD Hide = 0x0;
	QWORD Level = 0x0;
	QWORD Race = 0x0;
	QWORD Class = 0x0;
};

EQGameScanner::EQGameScanner(void) {}

EQGameScanner::~EQGameScanner(void) {}

void EQGameScanner::setExe(TCHAR* str)
{
	executablePath = str;
}

bool EQGameScanner::executableExists() const
{
	std::ifstream file(executablePath.c_str(), std::ios::in);

	if (file)
	{
		file.close();
		return true;
	}

	return false;
}

DWORD EQGameScanner::findEQPointerOffset(DWORD startAddress, std::size_t blockSize, const PBYTE byteMask, const PCHAR charMask)
{
	std::ifstream file(executablePath.c_str(), std::ios::in | std::ios::binary);

	// If the file can't be opened, return NULL for pointer offset.
	if (!file)
		return NULL;

	int typelen = 0;
	typelen = (int)std::string(charMask).find_last_of("t") - (int)std::string(charMask).find_first_of("t") + 1;

	if (typelen < 1)
		typelen = 4;

	// Setup our temporary storage variables
	std::vector<BYTE> buffer(blockSize, 0); // Using vector for automatic memory management
	DWORD matchAddr = NULL;

	// Move get pointer to the start of the block we want to search
	file.seekg(startAddress, std::ios::beg);
	file.read(reinterpret_cast<char*>(buffer.data()), blockSize);

	// Search for a position that fits our masks in memory.
	for (DWORD i = 0; i < blockSize; ++i)
	{
		if (compareData(buffer.data() + i, byteMask, charMask))
		{
			DWORD checkRet;
			matchAddr = i;
			if (typelen == 1) {
				BYTE chechbyteRet = *reinterpret_cast<PBYTE>(buffer.data() + matchAddr + std::string(charMask).find_first_of("t"));
				checkRet = static_cast<DWORD>(chechbyteRet);
			}
			else if (typelen == 2) {
				WORD checkwordRet = *reinterpret_cast<PWORD>(buffer.data() + matchAddr + std::string(charMask).find_first_of("t"));
				checkRet = static_cast<DWORD>(checkwordRet);
			}
			else {
				checkRet = *reinterpret_cast<PDWORD>(buffer.data() + matchAddr + std::string(charMask).find_first_of("t"));
			}
			if (checkRet < 536870912)
				break;
			else
				matchAddr = NULL;
		}
	}

	// Close the file before returning
	file.close();

	// If we didn't find a match, return NULL
	if (matchAddr == NULL)
		return NULL;

	DWORD nRet;

	// Find where our target address we're searching for is stored, and return its value.
	if (typelen == 1) {
		BYTE cRet = *reinterpret_cast<PBYTE>(buffer.data() + matchAddr + std::string(charMask).find_first_of("t"));
		nRet = static_cast<DWORD>(cRet);
	}
	else if (typelen == 2) {
		WORD wRet = *reinterpret_cast<PWORD>(buffer.data() + matchAddr + std::string(charMask).find_first_of("t"));
		nRet = static_cast<DWORD>(wRet);
	}
	else {
		nRet = *reinterpret_cast<PDWORD>(buffer.data() + matchAddr + std::string(charMask).find_first_of("t"));
	}

	return nRet;
}


DWORD EQGameScanner::findEQStructureOffset(DWORD startAddress, std::size_t blockSize, const PBYTE byteMask, const PCHAR charMask, const QWORD baseEQPointerAddress)
{
	DWORD nRet = 0;

	std::string maskStr = charMask;

	if (maskStr.empty()) {
		return nRet;
	}

	// Create a new, editable copy of byteMask using std::vector for automatic memory management
	std::vector<BYTE> newByteMask(maskStr.size());
	memcpy(newByteMask.data(), byteMask, maskStr.size());

	// Find the position of 'o' in the character mask
	size_t pointerPos = maskStr.find_first_of('o');
	if (pointerPos == std::string::npos || pointerPos + sizeof(QWORD) > newByteMask.size()) {
		// Invalid mask or pointer out of bounds, return 0
		return nRet;
	}

	// Replace the EQPointer in our byteMask with the EQPointer given as an argument
	*reinterpret_cast<QWORD*>(newByteMask.data() + pointerPos) = baseEQPointerAddress;

	// Use the updated byteMask to locate the EQStructureOffset
	nRet = findEQPointerOffset(startAddress, blockSize, newByteMask.data(), charMask);

	return nRet;
}


// Thanks to dom1n1k for the piece of code this is based off of.
bool EQGameScanner::compareData(PBYTE data, PBYTE byteMask, PCHAR charMask)
{
	for (; *charMask; ++charMask, ++data, ++byteMask)
	{
		if ((*charMask == 'x' || *charMask == 'o') && *data != *byteMask)
			return false;
	}
	return (*charMask) == NULL;
}

bool EQGameScanner::ScanExecutable(HWND hDlg, IniReaderInterface* ir_intf, NetworkServerInterface* net_intf, bool write_out)
{
	if (!executableExists())
	{
		SetDlgItemText(hDlg, IDC_EDIT2, "Error: Could not locate the specified executable file.");
		return false;
	}

	bool reload = false;
	std::ostringstream outputStream;
	std::ostringstream findResults;

	// Update the File Info section
	updateFileInfoSection(outputStream, ir_intf, write_out);

	// Process each memory offset
	std::vector<std::pair<std::string, int>> offsets = {
		{"ZoneAddr", NetworkServer::OT_zonename},
		{"SpawnHeaderAddr", NetworkServer::OT_spawnlist},
		{"CharInfo", NetworkServer::OT_self},
		{"ItemsAddr", NetworkServer::OT_ground},
		{"TargetAddr", NetworkServer::OT_target},
		{"WorldAddr", NetworkServer::OT_world}
	};

	for (const auto& offset : offsets)
	{
		DWORD matchAddr = findAndProcessOffset(hDlg, offset.first, "Start", ir_intf, net_intf, outputStream, write_out);
		handleMatchResult(hDlg, matchAddr, net_intf, offset.second, offset.first, ir_intf, outputStream, write_out, reload);
	}

	// Update the dialog with the results
	std::string resultText = findResults.str() + outputStream.str();
	SetDlgItemText(hDlg, IDC_EDIT2, resultText.c_str());

	return reload;
}

void EQGameScanner::updateFileInfoSection(std::ostringstream& outputStream, IniReaderInterface* ir_intf, bool write_out) {
	// Initialize file attributes and retrieve the last write time
	WIN32_FILE_ATTRIBUTE_DATA fileData;
	if (!GetFileAttributesEx(executablePath.c_str(), GetFileExInfoStandard, &fileData)) {
		return; // Early return if file attributes can't be retrieved
	}
	SYSTEMTIME st;
	FileTimeToSystemTime(&fileData.ftLastWriteTime, &st);
	TCHAR szFileDate[16]; // A smaller buffer size is sufficient
	GetDateFormat(LOCALE_USER_DEFAULT, DATE_SHORTDATE, &st, nullptr, szFileDate, sizeof(szFileDate) / sizeof(szFileDate[0]));

	// Write the date to the INI file if necessary
	if (write_out) {
		ir_intf->writeStringEntry("File Info", "PatchDate", szFileDate);
	}
	outputStream << "[File Info]\r\n"
		<< "PatchDate=" << szFileDate << "\r\n\r\n"
		<< "[Port]\r\n"
		<< "Port=" << ir_intf->readIntegerEntry("Port", "Port") << "\r\n\r\n"
		<< "[Memory Offsets]\r\n";
}


DWORD EQGameScanner::findAndProcessOffset(HWND hDlg, const std::string& section, const std::string& entry, IniReaderInterface* ir_intf, NetworkServerInterface* net_intf, std::ostringstream& outputStream, bool write_out)
{
	QWORD mystart = ir_intf->readIntegerEntry(section.c_str(), entry.c_str(), true);
	std::string mypattern = ir_intf->readEscapeStrings(section.c_str(), "Pattern");
	std::string mymask = ir_intf->readStringEntry(section.c_str(), "Mask", true);

	DWORD matchAddr = findEQPointerOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str());
	outputStream << section << "=0x" << std::hex << matchAddr;
	return matchAddr;
}

void EQGameScanner::handleMatchResult(HWND hDlg, DWORD matchAddr, NetworkServerInterface* net_intf, int offsetType, const std::string& offsetName, IniReaderInterface* ir_intf, std::ostringstream& outputStream, bool write_out, bool& reload)
{
	if (matchAddr != NULL) {
		if (matchAddr == net_intf->current_offset(offsetType)) {
			outputStream << " # Match\r\n";
		}
		else {
			if (write_out) {
				std::stringstream strm;
				strm << "0x" << std::hex << matchAddr;
				if (ir_intf->writeStringEntry("Memory Offsets", offsetName.c_str(), strm.str().c_str())) {
					reload = true;
					outputStream << " # Written to ini file\r\n";
				}
				else {
					outputStream << " # Found - Write failed\r\n";
				}
			}
			else {
				outputStream << " # Does not match ini file.\r\n";
				EnableWindow(GetDlgItem(hDlg, IDC_BUTTON2), TRUE);
			}
		}
	}
	else {
		outputStream << " # Not Found\r\n";
	}
}

void EQGameScanner::ScanSecondary(HWND hDlg, IniReaderInterface* ir_intf, NetworkServerInterface* net_intf)
{
	if (!executableExists())
	{
		SetDlgItemText(hDlg, IDC_EDIT2, "Error: Could not locate the specified executable file.");
		return;
	}

	// We'll use this for comparisons
	QWORD matchAddr = NULL;

	std::ostringstream findResults;
	std::ostringstream outputStream;

	WIN32_FILE_ATTRIBUTE_DATA FileData = { 0 };

	if (GetFileAttributesEx(executablePath.c_str(), GetFileExInfoStandard, &FileData))
	{
		TCHAR szFileDate[255];
		FILETIME ftLastMod = FileData.ftLastWriteTime;
		SYSTEMTIME st;
		FileTimeToSystemTime(&ftLastMod, &st);
		GetDateFormat(LOCALE_USER_DEFAULT, DATE_SHORTDATE, &st, NULL, szFileDate, 255);
		string::size_type index = executablePath.find_last_of("\\/");
		string myfilename = executablePath.substr(index + 1, executablePath.size()).c_str();
		findResults << myfilename.c_str() << " Modified=" << szFileDate << "\r\n";
	}

	EQPrimaryOffsets::CharInfo = net_intf->current_offset((int)NetworkServer::OT_self);

	QWORD mystart;
	string mypattern;
	string mymask;

	mystart = (QWORD)ir_intf->readIntegerEntry("CharInfo", "Start", true);
	mypattern = ir_intf->readEscapeStrings("CharInfo", "Pattern");
	mymask = ir_intf->readStringEntry("CharInfo", "Mask", true);

	// CharInfo
	matchAddr = findEQPointerOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str());

	if (matchAddr != NULL)
	{
		// If we match char info offset by pattern search use it
		EQPrimaryOffsets::CharInfo = matchAddr;
	}
	else
	{
		// Otherwise use what is in the myseqserver.ini file
		EQPrimaryOffsets::CharInfo = net_intf->current_offset((int)NetworkServer::OT_self);
	}

	outputStream << "SpawnInfo Offsets" << "\r\n";
	matchAddr = 0;

	// SpawnInfo::NextOffset
	mystart = (QWORD)ir_intf->readIntegerEntry("SpawnInfoNextOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoNextOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoNextOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "NextOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::PrevOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoPrevOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoPrevOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoPrevOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "PrevOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::LastnameOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoLastnameOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoLastnameOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoLastnameOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "LastnameOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::XOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoXOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoXOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoXOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "XOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::YOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoYOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoYOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoYOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "YOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::ZOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoZOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoZOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoZOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "ZOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::SpeedOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoSpeedOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoSpeedOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoSpeedOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "SpeedOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::HeadingOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoHeadingOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoHeadingOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoHeadingOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "HeadingOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::NameOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoNameOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoNameOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoNameOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "NameOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::TypeOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoTypeOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoTypeOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoTypeOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "TypeOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::SpawnIDOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoSpawnIDOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoSpawnIDOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoSpawnIDOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "SpawnIDOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::OwnerIDOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoOwnerIDOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoOwnerIDOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoOwnerIDOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "OwnerIDOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::HideOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoHideOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoHideOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoHideOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "HideOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::Prev
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoLevelOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoLevelOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoLevelOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "LevelOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::Prev
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoRaceOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoRaceOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoRaceOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "RaceOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::ClassOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoClassOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoClassOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoClassOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "ClassOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::PrimaryOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoPrimaryOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoPrimaryOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoPrimaryOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "PrimaryOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	// SpawnInfo::OffhandOffset
	matchAddr = 0;
	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnInfoOffhandOffset", "Start", true);
	mypattern = ir_intf->readEscapeStrings("SpawnInfoOffhandOffset", "Pattern");
	mymask = ir_intf->readStringEntry("SpawnInfoOffhandOffset", "Mask", true);

	matchAddr = findEQStructureOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str(), EQPrimaryOffsets::CharInfo);

	outputStream << "OffhandOffset" << ":" << "\r\n";
	outputStream << "| Match Found @ " << ((matchAddr == NULL) ? "FALSE" : "TRUE") << "\r\n";
	outputStream << "| Offset -> 0x" << std::hex << matchAddr << "\r\n";
	outputStream << "\r\n";

	findResults << "\r\n";

	std::string v = findResults.str() + outputStream.str();
	SetDlgItemText(hDlg, IDC_EDIT2, v.c_str());

	return;
}