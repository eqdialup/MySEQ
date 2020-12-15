/* 
 * Smart EQ Offset Finder - GPL Edition
 * Copyright 2007-2009, Carpathian <Carpathian01@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#ifndef EQGAMEPATTERNS_H
#define EQGAMEPATTERNS_H


/* Offset: ZoneAddr
 * Last Updated: 10-07-2008
 * Found At: 00492F84
 * Search In: 0x480000 (Memory), 0x7F000 (File)
 */
DWORD startZoneAddr = 0x4F000;
PBYTE patternZoneAddr = (PBYTE) "\x6A\x20\x8B\x85\xAC\xE1\xFF\xFF"
                                "\x83\xC0\x04\x50\x68\x44\xF0\x99"
                                "\x00\xE8\x2F\xC5\x00\x00\x83\xC4"
                                "\x0C\x6A\x20\x8B\x8D\xAC\xE1\xFF"
                                "\xFF\x83\xC1\x44\x51\x68\x84\xF0"
                                "\x99\x00\xE8";
char maskZoneAddr[255] = "xx????xx????xttttx????xx???????????x?x????x";

/* Offset: ZoneInfoAddr
 * Last Updated: 10-07-2008
 * Found At: 0049180D
 * Search In: 0x480000 (Memory), 0x7F000 (File)
 */
DWORD startZoneInfoAddr = 0x7F000;
PBYTE patternZoneInfoAddr = (PBYTE)"\x51\x68\x84\xF0\x99\x00\xE8\x16\xC5"
                                   "\x00\x00\x83\xC4\x0C\xB9\x84\xED\x99"
                                   "\x00\xE8\xD9\xB1\xFC\xFF\x50\x8B\x0D"
                                   "\x2C\x0E\x98\x00\xE8\x0D\xFA\xFC\xFF";
char maskZoneInfoAddr[] = "xx????x????xx?xttttx??xxxxx????x??xx";


/* Offset: SpawnHeaderAddr (Test)
 * Last Updated: 05-20-2008
 * Found At: 0048CEE0
 * Search In: 0x470000 (Memory), 0x6F000 (File)
 */
DWORD startSpawnHeaderAddr = 0x6F000;
PBYTE patternSpawnHeaderAddr = (PBYTE)"\x8B\x0D\x5C\x62\xAC\x00\x55\x8B"
									  "\x6C\x24\x08\x8B\x45\x00\x57\x50"
									  "\xE8\x4B\xE6\x09\x00\x85\xC0";
char maskSpawnHeaderAddr[] = "xxttttxxxxxxxxxxx????xx";


/* Offset: CharInfoAddr
 * Last Updated: 10-07-2008
 * Found At: 00469D30
 * Search In: 0x450000 (Memory), 0x4F000 (File)
 */
DWORD startCharInfoAddr = 0x4F000;
PBYTE patternCharInfoAddr = (PBYTE)"\x8B\x0D\xA4\x07\x95\x00\x8A"
									  "\x81\x49\x0E\x00\x00\x84\xC0"
									  "\x0F\x85\x2C\x02\x00\x00\x8B"
									  "\x15\x18\xED\x89\x00\x8B\x0C"
									  "\x95\x80\xFD\x96\x00\x8B\x01"
									  "\x68\x33\x33\xB3\x3F\xFF\x50"
									  "\x18\xE9\x10\x02\x00\x00";
char maskCharInfoAddr[] = "x?ttttx?????xxx?????x?????x??????x?xxxxxx?xxxxxx";


/* Offset: ItemsAddr
 * Last Updated: 3-16-2010
 * Found At: 004BAA00
 * Search In: 0x4b0000 (Memory), 0x4b000 (File)
 */
DWORD startItemsAddr = 0x3F000;
PBYTE patternItemsAddr = (PBYTE)"\x00\xC7\x05\xF0\x47\xC5\x00\x00"
								"\x00\x00\x00\xE8\x1B\xEE\x37\x00"
								"\x83\xC4\x04\xA1\xF0\x48\xC5\x00"
								"\x85\xC0\x75\x43\x56\x6A\x04";
char maskItemsAddr[] = "x??ttttxxxx????xxxx????xxxxxxxx";


/* Offset: TargetAddr
 * Last Updated: 05-20-2009
 * Found At: 004A8F10
 * Search In: 0x490000 (Memory), 0x8F000 (File)
 */
DWORD startTargetAddr = 0x8F000;
PBYTE patternTargetAddr = (PBYTE)"\x8A\x81\xB8\x01\x00\x00\x3C\x64"
                                 "\x8B\x35\x70\xEA\xA4\x00\x74\x26"
                                 "\x84\xD2\x75\x3E\x3C\x66\x8B\x0D"
                                 "\xFC\xE9\xA4\x00\x55\x55\x6A\x0D";
char maskTargetAddr[] = "x?????????tttt????????????????xx";


/* Offset: WorldAddr
 * Last Updated: 05-20-2009
 * Found At: 004695CF
 * Search In: 0x450000 (Memory), 0x4F000 (File)
 */
DWORD startWorldAddr = 0x4F000;
PBYTE patternWorldAddr = (PBYTE)"\x8A\x81\x59\x01\x00\x00\x84\xC0"
                                "\x75\x10\xA1\x24\xEA\xA4\x00\x8A"
                                "\x40\x04\x3C\x05\x72\x04\x3C\x12"
                                "\x76\x21\xC6\x42\x27\x01\xC2\x04"
                                "\x00";
char maskWorldAddr[] = "xx??xxxx??xttttxx?xx??xx??xx?xxxx";


 /* Offset: SpawnInfo::Next
 * Last Updated: 05-20-2009
 * Found At: 0042B373
 * Search In: 0x410000 (Memory), 0xF000 (File)
 */
DWORD startSpawnInfoNextAddr = 0xF000;
PBYTE patternSpawnInfoNextAddr = (PBYTE)"\xA1\x58\xEA\xA4\x00\x3B\xC5\x74"
                                     "\x65\x39\x46\x08\x75\x60\x8B\x46"
                                     "\x04\x8B\x48\x04";
char maskSpawnInfoNextAddr[] = "xooooxxx?xx?x?xx?xxt";


/* Offset: SpawnInfo::X
 * Last Updated: 05-20-2009
 * Found At: 0048C769
 * Search In: 0x470000 (Memory), 0x6F000 (File)
 */
DWORD startSpawnInfoXAddr = 0x6F000;
PBYTE patternSpawnInfoXAddr = (PBYTE)"\xA1\x58\xEA\xA4\x00\x8B\x50\x6C"
                                     "\x51\x8B\x48\x68\x52\x8B\x50\x64"
                                     "\x51";
char maskSpawnInfoXAddr[] = "xooooxx?xxx?xxxtx";


/* Offset: SpawnInfo::Y
 * Last Updated: 05-20-2009
 * Found At: 0048C769
 * Search In: 0x470000 (Memory), 0x4700 (File)
 */
DWORD startSpawnInfoYAddr = 0x4700;
PBYTE patternSpawnInfoYAddr = (PBYTE)"\xA1\x58\xEA\xA4\x00\x8B\x50\x6C"
                                     "\x51\x8B\x48\x68\x52\x8B\x50\x64"
                                     "\x51";
char maskSpawnInfoYAddr[] = "xooooxx?xxxtxxx?x";


/* Offset: SpawnInfo::Z
 * Last Updated: 05-20-2009
 * Found At: 0048C769
 * Search In: 0x470000 (Memory), 0x6F000 (File)
 */
DWORD startSpawnInfoZAddr = 0x6F000;
PBYTE patternSpawnInfoZAddr = (PBYTE)"\xA1\x58\xEA\xA4\x00\x8B\x50\x6C"
                                     "\x51\x8B\x48\x68\x52\x8B\x50\x64"
                                     "\x51";
char maskSpawnInfoZAddr[] = "xooooxxtxxx?xxx?x";


/* Offset: SpawnInfo::Speed
 * Last Updated: 05-20-2009
 * Found At: 004906AB
 * Search In: 0x480000 (Memory), 0x7F000 (File)
 */
DWORD startSpawnInfoSpeedAddr = 0x7F000;
PBYTE patternSpawnInfoSpeedAddr = (PBYTE)"\x8B\x15\x58\xEA\xA4\x00\x8B\x4C"
                                     "\x24\x10\x89\x5A\x7C\x8B\x54\x24"
                                     "\x0C\x89\x44\x24\x14\x6A\x03";
char maskSpawnInfoSpeedAddr[] = "xxoooox???xxtx???x???xx";


/* Offset: SpawnInfo::Heading
 * Last Updated: 05-20-2009
 * Found At: 0047647A
 * Search In: 0x460000 (Memory), 0x5F000 (File)
 */
DWORD startSpawnInfoHeadingAddr = 0x5F000;
PBYTE patternSpawnInfoHeadingAddr = (PBYTE)"\xA1\x58\xEA\xA4\x00\x8B\x80\x80"
                                           "\x00\x00\x00\x8B\x11\x50";
char maskSpawnInfoHeadingAddr[] = "xooooxxttttxxx";


/* Offset: SpawnInfo::Name
 * Last Updated: 05-20-2009
 * Found At: 0045DFDA
 * Search In: 0x440000 (Memory), 0x3F000 (File)
 */
DWORD startSpawnInfoNameAddr = 0x3F000;
PBYTE patternSpawnInfoNameAddr = (PBYTE)"\xA1\x58\xEA\xA4\x00\x05\xA4\x00"
                                     "\x00\x00\x8D\x54\x24\x50\x2B\xD0"
                                     "\x8D\x9B\x00\x00\x00\x00";
char maskSpawnInfoNameAddr[] = "xooooxtttt?x???xxxxxxx";


/* Offset: SpawnInfo::Type
 * Last Updated: 05-20-2009
 * Found At: 004EB752
 * Search In: 0x4D0000 (Memory), 0xCF000 (File)
 */
DWORD startSpawnInfoTypeAddr = 0xCF000;
PBYTE patternSpawnInfoTypeAddr = (PBYTE)"\xA1\x58\xEA\xA4\x00\x85\xC0\x0F"
                                        "\x84\x3D\x06\x00\x00\x8A\x88\x25"
                                        "\x01\x00\x00\x84\xC9";
char maskSpawnInfoTypeAddr[] = "xooooxxxx????xxttttxx";


/* Offset: SpawnInfo::SpawnId
 * Last Updated: 05-20-2009
 * Found At: 00491729
 * Search In: 0x480000 (Memory), 0x7F000 (File)
 */
DWORD startSpawnInfoSpawnIdAddr = 0x7F000;
PBYTE patternSpawnInfoSpawnIdAddr = (PBYTE)"\x8B\x0D\x58\xEA\xA4\x00\x8B\x86"
                                           "\x48\x01\x00\x00\x3B\x81\x48\x01"
                                           "\x00\x00\x75\x18";
char maskSpawnInfoSpawnIdAddr[] = "xxooooxx????xxttttx?";

 /* Offset: SpawnInfo::Hide
 * Last Updated: 05-20-2009
 * Found At: 004C5691
 * Search In: 0x4B0000 (Memory), 0xAF000 (File)
 */
DWORD startSpawnInfoHideAddr = 0xAF000;
PBYTE patternSpawnInfoHideAddr = (PBYTE)"\x3B\x0D\x58\xEA\xA4\x00\x0F\x85"
                                        "\xA4\x00\x00\x00\xA1\xE0\xE9\xA4"
                                        "\x00\x85\xC0\x8B\x91\xA8\x02\x00"
                                        "\x00";
char maskSpawnInfoHideAddr[] = "xxooooxx????x????xxxxtttt";


/* Offset: SpawnInfo::Level
 * Last Updated: 05-20-2009
 * Found At: 004272B2
 * Search In: 0x410000 (Memory), 0xF000 (File)
 */
DWORD startSpawnInfoLevelAddr = 0xF000;
PBYTE patternSpawnInfoLevelAddr = (PBYTE)"\x8A\x4C\x10\x40\x8B\x15\x58\xEA"
                                         "\xA4\x00\x88\x8A\xA8\x03\x00\x00"
                                         "\x8B\x8E\x9C\x01\x00\x00";
char maskSpawnInfoLevelAddr[] = "xxx?xxooooxxttttxx??xx";


 /* Offset: SpawnInfo::Race
 * Last Updated: 05-20-2009
 * Found At: 0055EDF7
 * Search In: 0x540000 (Memory), 0x13F000 (File)
 */
DWORD startSpawnInfoRaceAddr = 0x13F000;
PBYTE patternSpawnInfoRaceAddr = (PBYTE)"\x8B\x0D\x58\xEA\xA4\x00\x8B\x91"
                                        "\x68\x0E\x00\x00\x83\xFA\x0C\x8B"
                                        "\x80\x0C\x16\x00\x00\x7E";
char maskSpawnInfoRaceAddr[] = "xxooooxxttttxx?xx????x";


/* Offset: SpawnInfo::Class
 * Last Updated: 05-20-2009
 * Found At: 0042725C
 * Search In: 0x410000 (Memory), 0xF000 (File)
 */
DWORD startSpawnInfoClassAddr = 0xF000;
PBYTE patternSpawnInfoClassAddr = (PBYTE)"\x8A\x54\x10\x41\xA1\x58\xEA\xA4"
                                         "\x00\x88\x90\x6C\x0E\x00\x00\x8B"
                                         "\x8E\x9C\x01\x00\x00";
char maskSpawnInfoClassAddr[] = "xxx?xooooxxttttxx??xx";


#endif // EQGAMEPATTERNS_H