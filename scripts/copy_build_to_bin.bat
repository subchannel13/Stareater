@echo off
rem Copies {svnRoot}/podaci and {svnRoot}/slike
rem to source/Stareater.UI.WinForms/bin/Debug/ and
rem source/Stareater.UI.WinForms/bin/Release/
@echo on

robocopy ../build/ ../source/Stareater.UI.WinForms/bin/Debug/ /xf *.exe /xf settings.txt /e

rem robocopy ../build/languages/ ../source/Stareater.UI.WinForms/bin/Release/languages/ /e
pause
