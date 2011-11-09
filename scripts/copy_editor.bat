@echo off
rem Copies ZvjEdit.exe from {projDir}/bin/debug/
rem or {projDir}/bin/release/ to {svnRoot}/editors/
rem
rem Used as post build action in Zvjezdojedac editori
rem project.

IF EXIST ..\..\..\..\editors\ GOTO do_copy
mkdir ..\..\..\..\editors\

:do_copy
copy ZvjEdit.exe ..\..\..\..\editors\
