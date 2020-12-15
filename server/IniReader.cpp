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
	StartMinimized=false;
}

IniReader::~IniReader(void)
{

}



string IniReader::GetPatchDate() {

		return patchDate;
}


void IniReader::openFile(string _filename)

{

	filename = _filename;
	
	patchDate = readStringEntry("File Info", "PatchDate");

	if ( patchDate == "" )

	{
		MessageBox(NULL, "Error: Invalid INI file", "Error Loading INI file", 0);
		ostrstream strm;
		strm << "Error: IniReader: Invalid INI file " << filename << ends ; \
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

	UINT rtn=0;

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

	} else {
		return "";
	}

	return rtn;

}

string IniReader::readEscapeStrings(string section, string entry)

{

	string rtn("");
	TCHAR newbuff[1024];
	if (GetPrivateProfileString(section.c_str(), entry.c_str(), TEXT(""), newbuff, sizeof(newbuff), configfilename.c_str()) > 0)

	{
		bool inescape = false;
		bool inhex = false;
		int digits = 0;
		int p = 0;
		string in = newbuff;
		string out("");
		size_t j = in.length();
		for (size_t i = 0;i<in.length();i++)
		{
			if (inescape == true) {
				if (_T(in.at(i)) == _T('x'))
				{
					inhex = true;
					inescape = false;
					digits = 1;
				} else {
					// Bad escape sequence, so reset.  Should always start with \x
					inescape = false;
				}
			} else if (inhex == true) {
				if(IS_HEX_CHAR(_T(in.at(i)))) {
					// add a check to make sure they are hex digits, ie 0-9, a-f
					if(digits++ == 2) {
						char buff[16];
						buff[0] = '0';
						buff[1] = 'x';
						buff[2] = in[i-1];
						buff[3] = in[i];
						buff[4] = '\0';
						int value = strtol(buff, NULL, 16);
						out += char(value);
						//buffer[p] = value;
						//p++;
						inhex = false;
					}
				} else {
					// Bad hex character, so let's not try to convert it
					inhex = false;
				}
			} else if (_T(in.at(i)) == _T('\\')) {
				inescape = true;
			} else {
				inescape = false;
			}

		}
		
		rtn = out;

	}

	return rtn;

}


long IniReader::readIntegerEntry(string section, string entry, bool config)

{

	long rtn=0;
	_TCHAR buffer[255];
	if (GetPrivateProfileString(section.c_str(), entry.c_str(), TEXT(""), buffer, sizeof(buffer), config ? configfilename.c_str() : filename.c_str()) > 0)

	{

		// See if number is hex (prefixed with 0x) or decimal (no prefix)

		if ( buffer[0] == '0' )

			rtn = strtol(buffer, NULL,16);

		else

			rtn = (long) atoi(buffer);

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

		WritePrivateProfileString("Server","StartMinimized",TEXT("1"), configfilename.c_str());
	else

		WritePrivateProfileString("Server","StartMinimized",TEXT("0"), configfilename.c_str());

}

