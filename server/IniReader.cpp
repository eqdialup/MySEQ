/*==============================================================================

	Copyright (C) 2006-2013  All developers at http://sourceforge.net/projects/seq

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
#include "IniReader.h"
#include "resource.h"

  // Macro to assist in decoding escape sequence
#define IS_HEX_CHAR( st ) ((st >= _T('0')) && (st <= _T('9'))) || ((st >= _T('a')) && (st <= _T('f'))) || (( st >= _T('A')) && (st <= _T('F')))

IniReader::IniReader()
{
	StartMinimized = false;
}

IniReader::~IniReader(void) = default;

string IniReader::GetPatchDate()
{
	return patchDate;
}

void IniReader::openFile(string _filename)
{
	filename = _filename;
	patchDate = readStringEntry("File Info", "PatchDate");

	if (patchDate == "")
	{
		MessageBox(NULL, "Error: Invalid INI file", "Error Loading INI file", 0);
		ostrstream strm;
		strm << "Error: IniReader: Invalid INI file " << filename << ends; \
			throw Exception(EXCLEV_ERROR, strm.str());
	}
	else
	{
		cout << "IniReader: Reading INI file" << endl;
		cout << "IniFile: " << filename << endl;
		cout << "Patch Date: " << patchDate << endl;
	}
}

void IniReader::openConfigFile(string _filename)
{
	configfilename = _filename;
	UINT rtn = 0;
	rtn = GetPrivateProfileInt("Server", "StartMinimized", 0, configfilename.c_str());

	if (rtn > 0)
		SetStartMinimized(true);

	cout << "IniReader: Reading Config INI file" << endl;
	cout << "ConfigIniFile: " << filename << endl;
}

string IniReader::readStringEntry(string section, string entry, bool config)
{
	string rtn("");

	if (GetPrivateProfileString(section.c_str(), entry.c_str(), TEXT(""), buffer, sizeof(buffer), config ? configfilename.c_str() : filename.c_str()) > 0)
	{
		rtn = buffer;
	}
	else
	{
		return "";
	}
	return rtn;
}

string IniReader::readEscapeStrings(const string& section, const string& entry) {
	string result;
	TCHAR buffer[1024];

	if (GetPrivateProfileString(section.c_str(), entry.c_str(), TEXT(""), buffer, sizeof(buffer) / sizeof(TCHAR), configfilename.c_str()) > 0) {
		string input = buffer;
		result.reserve(input.length());  // Preallocate memory to avoid reallocations
		bool inEscape = false;
		int hexValue = 0;
		int hexDigits = 0;

		for (size_t i = 0; i < input.length(); ++i) {
			char currentChar = input[i];

			if (inEscape) {
				if (currentChar == 'x' && hexDigits == 0) {
					hexDigits = 1;  // Start collecting hex digits
				}
				else if (hexDigits > 0 && isxdigit(currentChar)) {
					hexValue = (hexValue << 4) | (isdigit(currentChar) ? currentChar - '0' : tolower(currentChar) - 'a' + 10);
					hexDigits++;
					if (hexDigits == 3) {  // We need exactly two hex digits
						result += static_cast<char>(hexValue);
						hexDigits = 0;
						inEscape = false;
					}
				}
				else {
					// Handle common escape sequences like \n, \t, etc.
					switch (currentChar) {
					case 'n': result += '\n'; break;
					case 't': result += '\t'; break;
					case '\\': result += '\\'; break;
					case '\"': result += '\"'; break;
					default:
						result += '\\';  // Keep the backslash as-is
						result += currentChar;
						break;
					}
					inEscape = false;
					hexDigits = 0;
				}
			}
			else if (currentChar == '\\') {
				inEscape = true;
				hexValue = 0;
			}
			else {
				result += currentChar;
			}
		}

		// If the string ends with an incomplete escape sequence, handle it
		if (inEscape && hexDigits == 0) {
			result += '\\';
		}
	}

	return result;
}
/*string IniReader::readEscapeStrings(const string& section, const string& entry) {
	string rtn("");
	TCHAR newbuff[1024];

	if (GetPrivateProfileString(section.c_str(), entry.c_str(), TEXT(""), newbuff, sizeof(newbuff) / sizeof(TCHAR), configfilename.c_str()) > 0) {
		bool inEscape = false;
		bool inHex = false;
		int hexDigits = 0;
		string out("");
		string in = newbuff;

		for (size_t i = 0; i < in.length(); ++i) {
			char currentChar = in[i];

			if (inEscape) {
				if (currentChar == 'x') {
					inHex = true;
					hexDigits = 0;
				}
				else {
					// Bad escape sequence, just append the backslash and the character
					out += '\\';
					out += currentChar;
				}
				inEscape = false;
			}
			else if (inHex) {
				if (isxdigit(currentChar)) {
					hexDigits++;
					out += currentChar;

					if (hexDigits == 2) {
						// Convert last two hex digits to a character
						int value = strtol(out.substr(out.length() - 2).c_str(), nullptr, 16);
						out.replace(out.length() - 2, 2, 1, static_cast<char>(value));
						inHex = false;
					}
				}
				else {
					// Not a valid hex sequence, reset and include the invalid part
					inHex = false;
				}
			}
			else if (currentChar == '\\') {
				inEscape = true;
			}
			else {
				out += currentChar;
			}
		}

		rtn = out;
	}

	return rtn;
}*/
QWORD IniReader::readIntegerEntry(string section, string entry, bool config)
{
	QWORD rtn = 0;
	_TCHAR buffer[255];

	if (GetPrivateProfileString(section.c_str(), entry.c_str(), TEXT(""), buffer, sizeof(buffer), config ? configfilename.c_str() : filename.c_str()) > 0)
	{
		// See if number is hex (prefixed with 0x) or decimal (no prefix)
		if (buffer[0] == '0')
			rtn = strtoull(buffer, NULL, 16);
		else
			rtn = (QWORD)atoi(buffer);
	}

	return rtn;
}

bool IniReader::writeStringEntry(string section, string entry, string value, bool config)
{
	if (WritePrivateProfileString(section.c_str(), entry.c_str(), value.c_str(), config ? configfilename.c_str() : filename.c_str()) > 0)
		return true;
	else
		return false;
}

void IniReader::ToggleStartMinimized()
{
	SetStartMinimized(GetStartMinimized() == false);

	if (GetStartMinimized())
		WritePrivateProfileString("Server", "StartMinimized", TEXT("1"), configfilename.c_str());
	else
		WritePrivateProfileString("Server", "StartMinimized", TEXT("0"), configfilename.c_str());
}