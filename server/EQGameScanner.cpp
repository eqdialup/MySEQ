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
#include <fstream>
#include "EQGameScanner.h"
#include "minwindef.h"


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

	int typelen = static_cast<int>(std::string(charMask).find_last_of('t')) - static_cast<int>(std::string(charMask).find_first_of('t')) + 1;
	typelen = (typelen < 1) ? 4 : typelen;

	// Setup our temporary storage variables
	std::unique_ptr<BYTE[]> buffer(new BYTE[blockSize]);
	DWORD matchAddr = NULL;

	// I like clean memory.
	memset(buffer.get(), 0, blockSize);

	// Move get pointer to the start of the block we want to search
	// Then attempt to read blockSize to the buffer
	file.seekg(startAddress, std::ios::beg);
	file.read(reinterpret_cast<char*>(buffer.get()), blockSize);

	// Search for a position that fits our masks in memory.
	for (DWORD i = 0; i < blockSize; ++i)
	{
		if (compareData(buffer.get() + i, byteMask, charMask))
		{
			DWORD checkRet;
			matchAddr = i;

			auto offset = static_cast<int>(std::string(charMask).find_first_of('t'));

			if (typelen == 1) {
				BYTE chechbyteRet = *reinterpret_cast<PBYTE>(buffer.get() + matchAddr + offset);
				checkRet = static_cast<DWORD>(chechbyteRet);
			}
			else if (typelen == 2) {
				WORD checkwordRet = *reinterpret_cast<PWORD>(buffer.get() + matchAddr + offset);
				checkRet = static_cast<DWORD>(checkwordRet);
			}
			else {
				checkRet = *reinterpret_cast<PDWORD>(buffer.get() + matchAddr + offset);
			}

			if (checkRet < 536870912) {
				break;
			}
			else {
				matchAddr = NULL;
			}
		}
	}

	// If we didn't find a match, return NULL
	if (matchAddr == NULL)
		return NULL;

	DWORD nRet;
	auto offset = static_cast<int>(std::string(charMask).find_first_of('t'));

	// Find where our target address we're searching for is stored, and return its value.
	if (typelen == 1) {
		BYTE cRet = *reinterpret_cast<PBYTE>(buffer.get() + matchAddr + offset);
		nRet = static_cast<DWORD>(cRet);
	}
	else if (typelen == 2) {
		WORD wRet = *reinterpret_cast<PWORD>(buffer.get() + matchAddr + offset);
		nRet = static_cast<DWORD>(wRet);
	}
	else {
		nRet = *reinterpret_cast<PDWORD>(buffer.get() + matchAddr + offset);
	}

	return nRet;
}

/*findEQPointerOffset(DWORD startAddress, std::size_t blockSize, const PBYTE byteMask, const PCHAR charMask)
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
	PBYTE buffer = new BYTE[blockSize];
	DWORD matchAddr = NULL;

	// I like clean memory.
	memset(buffer, 0, blockSize);

	// Move get pointer to the start of the block we want to search
	// Then attempt to read blockSize to the buffer
	file.seekg(startAddress, std::ios::beg);
	file.read((char*)buffer, blockSize);

	// Search for a position that fits our masks in memory.
	// Thanks to dom1n1k for the piece of code this is based off of.
	for (DWORD i = 0; i < blockSize; ++i)
	{
		if (compareData(buffer + i, byteMask, charMask))
		{
			DWORD checkRet;
			matchAddr = i;
			if (typelen == 1) {
				BYTE chechbyteRet = *reinterpret_cast<PBYTE>(buffer + matchAddr + std::string(charMask).find_first_of("t"));
				checkRet = (DWORD)chechbyteRet;
			}
			else if (typelen == 2) {
				WORD checkwordRet = *reinterpret_cast<PWORD>(buffer + matchAddr + std::string(charMask).find_first_of("t"));
				checkRet = (DWORD)checkwordRet;
			}
			else {
				checkRet = *reinterpret_cast<PDWORD>(buffer + matchAddr + std::string(charMask).find_first_of("t"));
			}

			//DWORD checkRet = *reinterpret_cast<PDWORD>(buffer + matchAddr + std::string(charMask).find_first_of("t"));
			if (checkRet < 536870912)
				break;
			else
				matchAddr = NULL;
		}
	}

	// If we didn't find a match, return NULL
	if (matchAddr == NULL)
		return NULL;

	DWORD nRet;

	// Find where our target address we're searching for is stored, and return its value.
	if (typelen == 1) {
		BYTE cRet = *reinterpret_cast<PBYTE>(buffer + matchAddr + std::string(charMask).find_first_of("t"));
		nRet = (DWORD)cRet;
	}
	else if (typelen == 2) {
		WORD wRet = *reinterpret_cast<PWORD>(buffer + matchAddr + std::string(charMask).find_first_of("t"));
		nRet = (DWORD)wRet;
	}
	else {
		nRet = *reinterpret_cast<PDWORD>(buffer + matchAddr + std::string(charMask).find_first_of("t"));
	}

	delete[] buffer;

	return nRet;
}*/

DWORD EQGameScanner::findEQStructureOffset(DWORD startAddress, std::size_t blockSize, const PBYTE byteMask, const PCHAR charMask, const QWORD baseEQPointerAddress)
{
	DWORD nRet = 0;

	if (strlen(charMask) == 0)
		return nRet;

	// Create a new, editable, copy of byteMask using std::vector.
	std::vector<BYTE> newByteMask(strlen(charMask));
	std::memcpy(newByteMask.data(), byteMask, strlen(charMask));

	// Replace the EQPointer in our byteMask with the EQPointer given as an argument.
	// The location of the EQPointer is denoted by the letter 'o' in our character mask.
	// Currently we require and always assume that the EQPointer is 4 bytes.
	PQWORD valueToChange = reinterpret_cast<PQWORD>(newByteMask.data() + std::string(charMask).find_first_of('o'));
	*valueToChange = baseEQPointerAddress;

	// Use the updated byteMask to locate the EQStructureOffset
	nRet = findEQPointerOffset(startAddress, blockSize, newByteMask.data(), charMask);

	return nRet;
}

// Thanks to dom1n1k for the piece of code this is based off of.
bool EQGameScanner::compareData(PBYTE data, PBYTE byteMask, PCHAR charMask)
{
	for (; *charMask != '\0'; ++charMask, ++data, ++byteMask)
	{
		if ((*charMask == 'x' || *charMask == 'o') && *data != *byteMask)
		{
			return false;
		}
	}
	return true;
}


const int MAX_PATH_LEN = _MAX_PATH;

bool EQGameScanner::ScanExecutable(HWND hDlg, IniReaderInterface* ir_intf, NetworkServerInterface* net_intf, bool write_out)
{
	if (!executableExists())
	{
		SetDlgItemText(hDlg, IDC_EDIT2, "Error: Could not locate the specified executable file.");
		return false;
	}

	bool reload = false;
	std::ostringstream outputStream;
	outputStream << GetFileInfo(ir_intf, write_out);

	// Offsets to check
	std::vector<std::tuple<std::string, std::string, NetworkServer::OffsetType >> offsets = {
		{"ZoneAddr", "ZoneAddr", NetworkServer::OT_zonename},
		{"SpawnHeaderAddr", "SpawnHeaderAddr", NetworkServer::OT_spawnlist},
		{"CharInfo", "CharInfo", NetworkServer::OT_self},
		{"ItemsAddr", "ItemsAddr", NetworkServer::OT_ground},
		{"TargetAddr", "TargetAddr", NetworkServer::OT_target},
		{"WorldAddr", "WorldAddr", NetworkServer::OT_world},
	};

	for (const auto& offset : offsets)
	{
		const std::string& section = std::get<0>(offset);
		const std::string& entry = std::get<1>(offset);
		NetworkServer::OffsetType offsetType = std::get<2>(offset);

		reload |= CheckAndWriteOffset(hDlg, ir_intf, net_intf, section, entry, offsetType, write_out, outputStream);
	}

	SetDlgItemText(hDlg, IDC_EDIT2, outputStream.str().c_str());

	return reload;
}

std::string EQGameScanner::GetFileInfo(IniReaderInterface* ir_intf, bool write_out)
{
	WIN32_FILE_ATTRIBUTE_DATA FileData = { 0 };
	std::ostringstream outputStream;

	if (GetFileAttributesEx(executablePath.c_str(), GetFileExInfoStandard, &FileData))
	{
		TCHAR szFileDate[255];
		FILETIME ftLastMod = FileData.ftLastWriteTime;
		SYSTEMTIME st;
		FileTimeToSystemTime(&ftLastMod, &st);
		GetDateFormat(LOCALE_USER_DEFAULT, DATE_SHORTDATE, &st, NULL, szFileDate, 255);

		if (write_out)
		{
			ir_intf->writeStringEntry("File Info", "PatchDate", szFileDate);
		}

		outputStream << "[File Info]\r\n";
		outputStream << "PatchDate=" << szFileDate << "\r\n\r\n";
		outputStream << "[Port]\r\n";
		UINT ini_port = static_cast<UINT>(ir_intf->readIntegerEntry("Port", "Port"));
		outputStream << "Port=" << ini_port << "\r\n\r\n";
		outputStream << "[Memory Offsets]" << "\r\n";
	}

	return outputStream.str();
}

bool EQGameScanner::CheckAndWriteOffset(HWND hDlg, IniReaderInterface* ir_intf, NetworkServerInterface* net_intf,
	const std::string& section, const std::string& entry,
	NetworkServer::OffsetType offsetType, bool write_out,
	std::ostringstream& outputStream)
{
	QWORD start = ir_intf->readIntegerEntry(section.c_str(), "Start", true);
	std::string pattern = ir_intf->readEscapeStrings(section.c_str(), "Pattern");
	std::string mask = ir_intf->readStringEntry(section.c_str(), "Mask", true);

	DWORD matchAddr = findEQPointerOffset(start, 0x100000, (PBYTE)pattern.c_str(), (PCHAR)mask.c_str());

	outputStream << entry << "=0x" << std::hex << matchAddr;

	if (matchAddr != NULL)
	{
		if (matchAddr == net_intf->current_offset(static_cast<int>(offsetType)))
		{
			outputStream << " # Match\r\n";
		}
		else
		{
			HandleMismatch(hDlg, ir_intf, entry, matchAddr, write_out, outputStream);
		}
	}
	else
	{
		outputStream << " #Not Found\r\n";
	}

	return false;
}

void EQGameScanner::HandleMismatch(HWND hDlg, IniReaderInterface* ir_intf, const std::string& entry, DWORD matchAddr,
	bool write_out, std::ostringstream& outputStream)
{
	if (write_out)
	{
		std::string strout = "0x" + ToHexString(matchAddr);

		if (ir_intf->writeStringEntry("Memory Offsets", entry.c_str(), strout.c_str()))
		{
			outputStream << " # Written to ini file\r\n";
		}
		else
		{
			outputStream << " # Found - Write failed\r\n";
		}
	}
	else
	{
		outputStream << " # Does not match ini file.\r\n";
		EnableWindow(GetDlgItem(hDlg, IDC_BUTTON2), TRUE);
	}
}

std::string EQGameScanner::ToHexString(DWORD value)
{
	std::stringstream strm;
	strm << std::hex << value;
	return strm.str();
}


//bool EQGameScanner::ScanExecutable(HWND hDlg, IniReaderInterface* ir_intf, NetworkServerInterface* net_intf, bool write_out)
//{
//	if (!executableExists()) 
//	{
//		SetDlgItemText(hDlg, IDC_EDIT2, "Error: Could not locate the specified executable file.");
//		return false;
//	}
//
//	bool reload = false;
//
//	// We'll use this for comparisons
//	DWORD matchAddr = NULL;
//
//	std::ostringstream findResults;
//	std::ostringstream outputStream;
//
//	WIN32_FILE_ATTRIBUTE_DATA FileData = { 0 };
//	if (GetFileAttributesEx(executablePath.c_str(), GetFileExInfoStandard, &FileData)) {
//		TCHAR szFileDate[255];
//		FILETIME ftLastMod = FileData.ftLastWriteTime;
//		SYSTEMTIME st;
//		FileTimeToSystemTime(&ftLastMod, &st);
//		GetDateFormat(LOCALE_USER_DEFAULT, DATE_SHORTDATE, &st, NULL, szFileDate, 255);
//		string::size_type index = executablePath.find_last_of("\\/");
//		string myfilename = executablePath.substr(index + 1, executablePath.size()).c_str();
//		//findResults << myfilename.c_str() << " Modified=" << szFileDate << "\r\n";
//		if (write_out)
//			ir_intf->writeStringEntry("File Info", "PatchDate", szFileDate);
//		outputStream << "[File Info]\r\n";
//		outputStream << "PatchDate=" << szFileDate << "\r\n\r\n";
//		outputStream << "[Port]\r\n";
//		UINT ini_port = (UINT)ir_intf->readIntegerEntry("Port", "Port");
//		outputStream << "Port=" << ini_port << "\r\n\r\n";
//	}
//
//	outputStream << "[Memory Offsets]" << "\r\n";
//
//	// ZoneAddr Neighborhood: 0x4800
//	QWORD mystart;
//	string mypattern;
//	string mymask;
//	QWORD mystart2 = ir_intf->readIntegerEntry("ZoneAddr", "Start", true);
//
//	mystart = ir_intf->readIntegerEntry("ZoneAddr", "Start", true);
//	mypattern = ir_intf->readEscapeStrings("ZoneAddr", "Pattern");
//	mymask = ir_intf->readStringEntry("ZoneAddr", "Mask", true);
//
//	matchAddr = findEQPointerOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str());
//
//	outputStream << "ZoneAddr=0x" << std::hex << matchAddr;
//
//	if (matchAddr != NULL) {
//		if (matchAddr == net_intf->current_offset((int)NetworkServer::OT_zonename))
//		{
//			outputStream << " # Match\r\n";
//		}
//		else
//		{
//			if (write_out) {
//				std::string strout("0x");
//				std::stringstream strm;
//				strm << std::hex << matchAddr;
//				strout.append(strm.str());
//
//				if (ir_intf->writeStringEntry("Memory Offsets", "ZoneAddr", strout.c_str()))
//				{
//					reload = true;
//					outputStream << " # Written to ini file\r\n";
//				}
//				else
//				{
//					outputStream << " # Found - Write failed\r\n";
//				}
//			}
//			else {
//				outputStream << " # Does not match ini file.\r\n";
//				EnableWindow(GetDlgItem(hDlg, IDC_BUTTON2), TRUE);
//			}
//		}
//	}
//	else
//	{
//		outputStream << " #Not Found\r\n";
//	}
//
//	mystart = (DWORD)ir_intf->readIntegerEntry("SpawnHeaderAddr", "Start", true);
//	mypattern = ir_intf->readEscapeStrings("SpawnHeaderAddr", "Pattern");
//	mymask = ir_intf->readStringEntry("SpawnHeaderAddr", "Mask", true);
//
//	// SpawnHeaderAddr Neighborhood: 0x4500
//	matchAddr = findEQPointerOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str());
//
//	outputStream << "SpawnHeaderAddr=0x" << std::hex << matchAddr;
//
//	if (matchAddr != NULL) {
//		if (matchAddr == net_intf->current_offset((int)NetworkServer::OT_spawnlist))
//		{
//			outputStream << " # Match\r\n";
//		}
//		else
//		{
//			if (write_out) {
//				std::string strout("0x");
//				std::stringstream strm;
//				strm << std::hex << matchAddr;
//				strout.append(strm.str());
//
//				if (ir_intf->writeStringEntry("Memory Offsets", "SpawnHeaderAddr", strout.c_str()))
//				{
//					reload = true;
//					outputStream << " # Written to ini file\r\n";
//				}
//
//				else
//				{
//					outputStream << " # Found - Write failed\r\n";
//				}
//			}
//			else {
//				outputStream << " # Does not match ini file.\r\n";
//				EnableWindow(GetDlgItem(hDlg, IDC_BUTTON2), TRUE);
//			}
//		}
//	}
//	else
//	{
//		outputStream << " #Not Found\r\n";
//	}
//
//	mystart = (DWORD)ir_intf->readIntegerEntry("CharInfo", "Start", true);
//	mypattern = ir_intf->readEscapeStrings("CharInfo", "Pattern");
//	mymask = ir_intf->readStringEntry("CharInfo", "Mask", true);
//
//	// CharInfo
//	matchAddr = findEQPointerOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str());
//	outputStream << "CharInfo=0x" << std::hex << matchAddr;
//
//	if (matchAddr != NULL) {
//		if (matchAddr == net_intf->current_offset((int)NetworkServer::OT_self))
//		{
//			outputStream << " # Match\r\n";
//		}
//		else
//		{
//			if (write_out) {
//				std::string strout("0x");
//				std::stringstream strm;
//				strm << std::hex << matchAddr;
//				strout.append(strm.str());
//
//				if (ir_intf->writeStringEntry("Memory Offsets", "CharInfo", strout.c_str()))
//				{
//					reload = true;
//					outputStream << " # Written to ini file\r\n";
//				}
//
//				else
//				{
//					outputStream << " # Found - Write failed\r\n";
//				}
//			}
//			else
//			{
//				outputStream << " # Does not match ini file.\r\n";
//				EnableWindow(GetDlgItem(hDlg, IDC_BUTTON2), TRUE);
//			}
//		}
//	}
//	else
//	{
//		outputStream << " #Not Found\r\n";
//	}
//
//	mystart = (DWORD)ir_intf->readIntegerEntry("ItemsAddr", "Start", true);
//	mypattern = ir_intf->readEscapeStrings("ItemsAddr", "Pattern");
//	mymask = ir_intf->readStringEntry("ItemsAddr", "Mask", true);
//
//	// ItemsAddr Neighborhood: 0x4b00
//	matchAddr = findEQPointerOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str());
//	outputStream << "ItemsAddr=0x" << std::hex << matchAddr;
//
//	if (matchAddr != NULL) {
//		if (matchAddr == net_intf->current_offset((int)NetworkServer::OT_ground))
//		{
//			outputStream << " # Match\r\n";
//		}
//		else
//		{
//			if (write_out) {
//				std::string strout("0x");
//				std::stringstream strm;
//				strm << std::hex << matchAddr;
//				strout.append(strm.str());
//
//				if (ir_intf->writeStringEntry("Memory Offsets", "ItemsAddr", strout.c_str()))
//				{
//					reload = true;
//					outputStream << " # Written to ini file\r\n";
//				}
//
//				else
//				{
//					outputStream << " # Found - Write failed\r\n";
//				}
//			}
//			else
//			{
//				outputStream << " # Does not match ini file.\r\n";
//				EnableWindow(GetDlgItem(hDlg, IDC_BUTTON2), TRUE);
//			}
//		}
//	}
//	else
//	{
//		outputStream << " #Not Found\r\n";
//	}
//
//	mystart = (DWORD)ir_intf->readIntegerEntry("TargetAddr", "Start", true);
//	mypattern = ir_intf->readEscapeStrings("TargetAddr", "Pattern");
//	mymask = ir_intf->readStringEntry("TargetAddr", "Mask", true);
//
//	// TargetAddr Neighboorhood: 0x6300
//	matchAddr = findEQPointerOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str());
//	outputStream << "TargetAddr=0x" << std::hex << matchAddr;
//
//	if (matchAddr != NULL) {
//		if (matchAddr == net_intf->current_offset((int)NetworkServer::OT_target))
//		{
//			outputStream << " # Match\r\n";
//		}
//		else
//		{
//			if (write_out) {
//				std::string strout("0x");
//				std::stringstream strm;
//				strm << std::hex << matchAddr;
//				strout.append(strm.str());
//
//				if (ir_intf->writeStringEntry("Memory Offsets", "TargetAddr", strout.c_str()))
//				{
//					reload = true;
//					outputStream << " # Written to ini file\r\n";
//				}
//				else
//				{
//					outputStream << " # Found - Write failed\r\n";
//				}
//			}
//			else
//			{
//				outputStream << " # Does not match ini file.\r\n";
//				EnableWindow(GetDlgItem(hDlg, IDC_BUTTON2), TRUE);
//			}
//		}
//	}
//	else
//	{
//		outputStream << " #Not Found\r\n";
//	}
//
//	mystart = (DWORD)ir_intf->readIntegerEntry("WorldAddr", "Start", true);
//	mypattern = ir_intf->readEscapeStrings("WorldAddr", "Pattern");
//	mymask = ir_intf->readStringEntry("WorldAddr", "Mask", true);
//
//	// WorldAddr Neighboorhood: 0x6300
//	matchAddr = findEQPointerOffset(mystart, 0x100000, (PBYTE)mypattern.c_str(), (PCHAR)mymask.c_str());
//	outputStream << "WorldAddr=0x" << std::hex << matchAddr;
//
//	if (matchAddr != NULL) {
//		if (matchAddr == net_intf->current_offset((int)NetworkServer::OT_world))
//		{
//			outputStream << " # Match\r\n";
//		}
//		else
//		{
//			if (write_out) {
//				std::string strout("0x");
//				std::stringstream strm;
//				strm << std::hex << matchAddr;
//				strout.append(strm.str());
//
//				if (ir_intf->writeStringEntry("Memory Offsets", "WorldAddr", strout.c_str()))
//				{
//					reload = true;
//					outputStream << " # Written to ini file\r\n";
//				}
//				else
//				{
//					outputStream << " # Found - Write failed\r\n";
//				}
//			}
//			else
//			{
//				outputStream << " # Does not match ini file.\r\n";
//				EnableWindow(GetDlgItem(hDlg, IDC_BUTTON2), TRUE);
//			}
//		}
//	}
//	else
//	{
//		outputStream << " #Not Found\r\n";
//	}
//
//	//findResults << "\r\n";
//
//	std::string v = findResults.str() + outputStream.str();
//	SetDlgItemText(hDlg, IDC_EDIT2, v.c_str());
//
//	return reload;
//}

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