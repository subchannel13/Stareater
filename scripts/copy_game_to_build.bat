@echo off
rem Copies Zvjezdojedac.exe from {projDir}/bin/release/
rem to {svnRoot}/build/

del ..\build\*.exe
del ..\build\*.zip
copy ..\source\Zvjezdojedac\bin\Release\zvjezdojedac.exe ..\build\
