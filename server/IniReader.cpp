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
#include "IniReader.h"
#include "resource.h"


  // Macro to assist in decoding escape sequences
#define IS_HEX_CHAR(st) ((st >= _T('0') && st <= _T('9')) || (st >= _T('a') && st <= _T('f')) || (st >= _T('A') && st <= _T('F')))

IniReader::IniReader() : StartMinimized(false) {}

IniReader::~IniReader() {}

std::string IniReader::GetPatchDate() const {
    return patchDate;
}

void IniReader::openFile(const std::string& _filename) {
    filename = _filename;
    patchDate = readStringEntry("File Info", "PatchDate");

    if (patchDate.empty()) {
        MessageBox(NULL, "Error: Invalid INI file", "Error Loading INI file", 0);
        std::ostringstream strm;
        strm << "Error: IniReader: Invalid INI file " << filename;
        throw std::runtime_error(strm.str());
    }
    else {
        std::cout << "IniReader: Reading INI file" << std::endl;
        std::cout << "IniFile: " << filename << std::endl;
        std::cout << "Patch Date: " << patchDate << std::endl;
    }
}

void IniReader::openConfigFile(const std::string& _filename) {
    configfilename = _filename;

    if (GetPrivateProfileInt("Server", "StartMinimized", 0, configfilename.c_str()) > 0) {
        SetStartMinimized(true);
    }

    std::cout << "IniReader: Reading Config INI file" << std::endl;
    std::cout << "ConfigIniFile: " << configfilename << std::endl;
}

std::string IniReader::readStringEntry(const std::string& section, const std::string& entry, bool config) {
    std::string result;
    std::string file = config ? configfilename : filename;

    if (GetPrivateProfileString(section.c_str(), entry.c_str(), "", buffer, sizeof(buffer), file.c_str()) > 0) {
        result = buffer;
    }

    return result;
}

std::string IniReader::readEscapeStrings(const std::string& section, const std::string& entry) {
    TCHAR newbuff[1024];
    std::string result;

    if (GetPrivateProfileString(section.c_str(), entry.c_str(), "", newbuff, sizeof(newbuff), configfilename.c_str()) > 0) {
        std::string in(newbuff);
        std::string out;
        bool inescape = false;
        bool inhex = false;
        int digits = 0;

        for (size_t i = 0; i < in.length(); ++i) {
            if (inescape) {
                if (in[i] == 'x') {
                    inhex = true;
                    inescape = false;
                    digits = 1;
                }
                else {
                    inescape = false;
                }
            }
            else if (inhex) {
                if (IS_HEX_CHAR(in[i])) {
                    if (digits++ == 2) {
                        char hexStr[5] = { '0', 'x', in[i - 1], in[i], '\0' };
                        int value = strtol(hexStr, NULL, 16);
                        out += static_cast<char>(value);
                        inhex = false;
                    }
                }
                else {
                    inhex = false;
                }
            }
            else if (in[i] == '\\') {
                inescape = true;
            }
            else {
                out += in[i];
            }
        }
        result = out;
    }

    return result;
}

QWORD IniReader::readIntegerEntry(const std::string& section, const std::string& entry, bool config) {
    QWORD result = 0;
    std::string file = config ? configfilename : filename;

    if (GetPrivateProfileString(section.c_str(), entry.c_str(), "", buffer, sizeof(buffer), file.c_str()) > 0) {
        if (buffer[0] == '0') {
            result = strtoull(buffer, nullptr, 16);
        }
        else {
            result = std::stoull(buffer);
        }
    }

    return result;
}

bool IniReader::writeStringEntry(const std::string& section, const std::string& entry, const std::string& value, bool config) {
    std::string file = config ? configfilename : filename;
    return WritePrivateProfileString(section.c_str(), entry.c_str(), value.c_str(), file.c_str()) > 0;
}

void IniReader::ToggleStartMinimized() {
    bool minimized = !GetStartMinimized();
    SetStartMinimized(minimized);

    std::string value = minimized ? "1" : "0";
    WritePrivateProfileString("Server", "StartMinimized", value.c_str(), configfilename.c_str());
}
