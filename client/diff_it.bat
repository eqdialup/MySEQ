@echo off

rem  This assumes a specific directory layout:
rem
rem  + ...
rem  |
rem  +-+ MySeq
rem    |
rem    +-- MySeq_Client_1_16_1
rem    |
rem    +-- SomeDirWithCurrentChanges
rem
rem  All differences between SomeDirWithCurrentChanges and MySeq_Client_1_16_1 are created;
rem  the latter should contain the official sources. Only SomeDirWithCurrentChanges (or whatever)
rem  can be edited, and diff_it.bat must be run from this directory.
rem  
rem  When new sources are released, MySeq_Client_1_16_1 has to be replaced with the new sources, 
rem  of course.

diff -x patch.txt -N -x "*.resources" -x "*.exe" -x maps -x "*.xml" -x timers -x filters -x cfg --ignore-file-name-case -w -B ..\MySeq_Client_1_16_1 . > ..\patch.txt

diff -q -x patch.txt -N -x "*.resources" -x "*.exe" -x maps -x "*.xml" -x timers -x filters -x cfg --ignore-file-name-case -w -B ..\MySeq_Client_1_16_1 .

type ..\patch.txt

