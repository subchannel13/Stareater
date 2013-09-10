@echo off
rem Copies {projRoot}/build to source/Stareater.UI.WinForms/bin/Debug/
rem and source/Stareater.UI.WinForms/bin/Release/
@echo on

robocopy ../build/ ../source/Stareater.UI.WinForms/bin/Debug/ /xf *.exe /xf settings.txt /e /XO
robocopy ../build/ ../source/Stareater.UI.WinForms/bin/Release/ /xf *.exe settings.txt /e /XO
pause
