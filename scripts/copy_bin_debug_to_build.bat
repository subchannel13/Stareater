rem Copies data from {svnRoot}/source/Stareater.UI.WinForms/bin/Debug/
rem to {svnRoot}/build

echo off
robocopy ../source/Stareater.UI.WinForms/bin/Debug/podaci/ ../build/podaci/ /xd .svn /e
robocopy ../source/Stareater.UI.WinForms/bin/Debug/slike/ ../build/slike/ /xd .svn /e
robocopy ../source/Stareater.UI.WinForms/bin/Debug/jezici/ ../build/jezici/ /xd .svn /e
pause
