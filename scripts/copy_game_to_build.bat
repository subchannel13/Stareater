@echo off
rem Copies Stareater.exe from {projDir}/bin/release/
rem to {svnRoot}/build/
@echo on

del ..\build\*.exe
copy ..\source\Stareater.UI.WinForms\bin\Release\stareater.exe ..\build\
