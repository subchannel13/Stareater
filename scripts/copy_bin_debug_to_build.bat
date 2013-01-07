rem Copies data from {svnRoot}/source/Stareater.UI.WinForms/bin/Debug/
rem to {svnRoot}/build

echo off
robocopy ../source/Stareater.UI.WinForms/bin/Debug/data/ ../build/data/ /e
robocopy ../source/Stareater.UI.WinForms/bin/Debug/languages/ ../build/languages/ /e
robocopy ../source/Stareater.UI.WinForms/bin/Debug/maps/ ../build/maps/ /e
robocopy ../source/Stareater.UI.WinForms/bin/Debug/players/ ../build/players/ /e
pause
