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

#include <Windows.h>
#include <string>
#include <iostream>
#include <strstream>
#include <vector>
#include <iomanip>
#include <sstream>
#include <algorithm>
#include <math.h>
#include <tlhelp32.h>
#include <winsock2.h>



using namespace std;

#define EXCLEV_WARNING 1

#define EXCLEV_ERROR 2

class Exception : public std::exception {
private:
	int level;
	std::string message;  // Use composition instead of inheritance

public:
	Exception(int l, const std::string& s) : level(l), message(s) {}

	// Override the what() method to provide a custom message
	const char* what() const noexcept override {
		return message.c_str();
	}

	// Provide a getter for the error level
	int getLevel() const {
		return level;
	}
};
