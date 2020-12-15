/*==============================================================================

	Copyright (C) 2006  All developers at http://sourceforge.net/projects/seq



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



#include <windows.h>

#include <string>

#include <iostream>

#include <strstream>

#include <vector>

#include <iomanip>

#include <sstream>

#include <algorithm>

#include <math.h>



using namespace std;



#define EXCLEV_WARNING 1

#define EXCLEV_ERROR 2



class Exception : public string

{

private:

	int level;

public:

	Exception(int l, string s) : level(l), string(s){;}

};

