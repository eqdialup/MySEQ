#============================================================================
#  Copyright (C) 2006  All developers at http://sourceforge.net/projects/seq
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
#  Makefile
#
#============================================================================

help:
	@echo "   Usage:   VER=version_num make release"
	@echo "   Example: VER=1.19.1 make release"


RELFILES += myseqserver.exe myseqserver.ini ServerReleaseNotes.html

.PHONY : release
release: $(RELFILES)
ifeq ($(strip $(VER)),)
	@echo " ==> Error: Please set VER when using the 'release' target. See 'make help'."
else
	@echo " ==> Creating release myseq.server.$(VER).zip"
	@if [ ! -d releases ]; then mkdir releases; fi
	@zip releases/myseq.server.$(VER).zip $(RELFILES)
endif
