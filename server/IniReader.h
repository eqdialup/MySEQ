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

#pragma once



#include "Common.h"



class IniReaderInterface

{

public:

	virtual void openFile(string filename) = 0;

	virtual void openConfigFile(string filename) = 0;

	virtual string readStringEntry(string section, string entry, bool config = false) = 0;

	virtual long readIntegerEntry(string section, string entry, bool config = false) = 0;

	virtual bool writeStringEntry(string section, string entry, string value, bool config = false) = 0;

	virtual string readEscapeStrings(string section, string entry) = 0;
};



class IniReader : public IniReaderInterface

{
public:

	IniReader();

	~IniReader(void);

private:

	string filename;

	string configfilename;

	_TCHAR buffer[255];

	bool StartMinimized;
	

public:

	void openFile(string filename);

	void openConfigFile(string filename);

	string readStringEntry(string section, string entry, bool config = false);

	string readEscapeStrings(string section, string entry);

	long readIntegerEntry(string section, string entry, bool config = false);

	bool writeStringEntry(string section, string entry, string value, bool config = false);

	string GetPatchDate();

	string patchDate;

	bool GetStartMinimized() { return StartMinimized; }

	void ToggleStartMinimized();

private:

	void SetStartMinimized(bool value) { StartMinimized = value; }

};

