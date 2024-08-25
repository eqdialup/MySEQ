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

#include "Common.h"


  // Typedef for a 64-bit unsigned integer
typedef uint64_t QWORD;

// Interface for IniReader to allow flexibility and extendability
class IniReaderInterface {
public:
    virtual ~IniReaderInterface() = default;

    // Pure virtual functions to be implemented by derived classes
    virtual void openFile(const std::string& filename) = 0;
    virtual void openConfigFile(const std::string& filename) = 0;
    virtual std::string readStringEntry(const std::string& section, const std::string& entry, bool config = false) = 0;
    virtual QWORD readIntegerEntry(const std::string& section, const std::string& entry, bool config = false) = 0;
    virtual bool writeStringEntry(const std::string& section, const std::string& entry, const std::string& value, bool config = false) = 0;
    virtual std::string readEscapeStrings(const std::string& section, const std::string& entry) = 0;
};

// IniReader class that implements the IniReaderInterface
class IniReader : public IniReaderInterface {
public:
    // Constructor and Destructor
    IniReader();
    ~IniReader();

    // Public functions for file operations and data retrieval
    void openFile(const std::string& filename) override;
    void openConfigFile(const std::string& filename) override;
    std::string readStringEntry(const std::string& section, const std::string& entry, bool config = false) override;
    std::string readEscapeStrings(const std::string& section, const std::string& entry) override;
    QWORD readIntegerEntry(const std::string& section, const std::string& entry, bool config = false) override;
    bool writeStringEntry(const std::string& section, const std::string& entry, const std::string& value, bool config = false) override;

    // Getter for PatchDate
    std::string GetPatchDate() const;

    // Functions to manage the StartMinimized state
    bool GetStartMinimized() const { return StartMinimized; }
    void ToggleStartMinimized();

    // Patch date from the INI file
    std::string patchDate;

private:
    // Private functions and member variables
    void SetStartMinimized(bool value) { StartMinimized = value; }

    // File paths for the INI files
    std::string filename;
    std::string configfilename;

    // Buffer for reading string entries
    _TCHAR buffer[255]{};

    // State for whether the application should start minimized
    bool StartMinimized;
};
