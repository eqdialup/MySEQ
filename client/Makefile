#============================================================================
#  Copyright (C) 2005  All developers at http://sourceforge.net/projects/seq
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
	@echo "   Example: VER=1.19.0 make release"


RELFILES += myseq.exe 
RELFILES += FolderBrowser.dll
RELFILES += SpeechLib.dll
RELFILES += MagicLibrary.DLL
RELFILES += ClientReleaseNotes.htm
RELFILES += Docking.bmp
RELFILES += cfg/Weapons.txt
RELFILES += cfg/SpawnTypes.txt
RELFILES += cfg/VisTypes.txt
RELFILES += cfg/Races.txt
RELFILES += cfg/MQConsole.Ini
RELFILES += cfg/RGB.txt
RELFILES += cfg/GroundItems.Ini
RELFILES += cfg/Lookup.Ini
RELFILES += cfg/Classes.txt
RELFILES += cfg/ConLevels.Ini
RELFILES += cfg/BodyTypes.txt
RELFILES += $(wildcard filters/filters_*.conf)

ZIPNAME = releases/myseq.client.$(VER).zip

.PHONY : release
release: $(RELFILES)
ifeq ($(strip $(VER)),)
	@echo " ==> Error: Please set VER when using the 'release' target. See 'make help'."
else
	@echo " ==> Creating release myseq.client.$(VER).zip"
	@if [ ! -d releases ]; then mkdir releases; fi
	@if [ -e $(ZIPNAME) ]; then rm $(ZIPNAME); fi
	@zip -9 $(ZIPNAME) $(RELFILES)
endif

.PHONY : crunchmaps
crunchmaps:
	@crunchmaps
