
#============================================================================
#  Copyright (C) 2007  All developers at http://sourceforge.net/projects/seq
#
#  This program is free software; you can redistribute it and/or
#  modify it under the terms of the GNU General Public License
#  as published by the Free Software Foundation; either version 2
#  of the License, or (at your option) any later version.
#
#  This program is distributed in the hope that it will be useful,
#  but WITHOUT ANY WARRANTY; without even the implied warranty of
#  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#  GNU General Public License for more details.
#
#  You should have received a copy of the GNU General Public License
#  along with this program; if not, write to the Free Software
#  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
#
#  Use of this tool or any of it's files is strictly forbidden by anyone who intends
#  to use the information for a non-GPL product. You may NOT use this to aid in the
#  MySEQv3 development project, or any other non-GPL derivative.
#
#  Makefile
#
#============================================================================

help:
	@echo "   Usage:   VER=version_num make release"
	@echo "   Example: VER=1.22 make release"


RELFILES += client/myseq.exe 
RELFILES += client/folderbrowser.dll
RELFILES += client/speechlib.dll
RELFILES += client/magiclibrary.dll
RELFILES += client/ClientReleaseNotes.htm
RELFILES += client/OVERHAUL-v1.22
RELFILES += client/plus.gif
RELFILES += client/Docking.bmp
RELFILES += client/cfg/SpawnTypes.txt
RELFILES += client/cfg/VisTypes.txt
RELFILES += client/cfg/Races.txt
RELFILES += client/cfg/GroundItems.Ini
RELFILES += client/cfg/Classes.txt
RELFILES += client/cfg/ConLevels.Ini
RELFILES += client/maps/default.txt

RELFILES += server/myseqserver.exe
RELFILES += server/myseqserver.ini

RELNAME = releases/myseq-v$(VER).msi

.PHONY : release
r release: $(RELFILES)
ifeq ($(strip $(VER)),)
	@echo " ==> Error: Please set VER when using the 'release' target. See 'make help'."
else
	@echo " ==> Creating $(RELNAME)"
	@if [ ! -d releases ]; then mkdir releases; fi
	@if [ -e $(RELNAME) ]; then rm $(RELNAME); fi
	@wix/candle.exe myseq.xml
	@wix/light.exe -out $(RELNAME) myseq.wixobj wix/wixui.wixlib -loc wix/WixUI_en-us.wxl
	@echo " ==> Complete."
endif


t test: uninstall install

u uninstall:
	msiexec /x myseq.msi

i install:
	msiexec /i myseq.msi

sample:
	wix/candle.exe SampleWixUI.wxs
	wix/light.exe -out SampleWixUI.msi SampleWixUI.wixobj wix/wixui.wixlib -loc wix/WixUI_en-us.wxl

