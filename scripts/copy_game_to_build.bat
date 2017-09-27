@echo off
rem Copies Stareater.exe from {projRoot}/bin/release/
rem to {projRoot}/build/
@echo on

del ..\build\*.exe
del ..\build\*.dll
del ..\build\maps\*.dll
del ..\build\players\*.dll
copy ..\source\Stareater.UI.WinForms\bin\Release\stareater.exe ..\build\
copy ..\source\Stareater.UI.WinForms\bin\Release\*.dll ..\build\
copy ..\source\Stareater.UI.WinForms\bin\Release\maps\*.dll ..\build\maps\
copy ..\source\Stareater.UI.WinForms\bin\Release\players\*.dll ..\build\players\
