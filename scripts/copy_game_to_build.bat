@echo off
rem Copies Stareater.exe from {projDir}/bin/release/
rem to {svnRoot}/build/

del ..\build\*.exe
del ..\build\*.zip
copy ..\source\Stareater.UI.WinForms\bin\Release\stareater.exe ..\build\
