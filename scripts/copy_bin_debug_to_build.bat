rem Copies data from {svnRoot}/source/Stareater.UI.WinForms/bin/Debug/
rem to {svnRoot}/build

echo off
robocopy ../source/Stareater.UI.WinForms/bin/Debug/languages/ ../build/languages/ /e
pause
