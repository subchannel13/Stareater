@echo off
rem Copies Stareater.exe from {projRoot}/bin/release/
rem to {projRoot}/build/
@echo on

del ..\build\*.exe
copy ..\source\Stareater.UI.WinForms\bin\Release\stareater.exe ..\build\
