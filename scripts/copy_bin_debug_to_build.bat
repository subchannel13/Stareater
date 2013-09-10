@echo off
rem Copies data from {projRoot}/source/Stareater.UI.WinForms/bin/Debug/
rem to {projRoot}/build
@echo on

@robocopy ../source/Stareater.UI.WinForms/bin/Debug/data/ ../build/data/ /e /njh /njs /ndl
@robocopy ../source/Stareater.UI.WinForms/bin/Debug/languages/ ../build/languages/ /e /njh /njs /ndl
@robocopy ../source/Stareater.UI.WinForms/bin/Debug/maps/ ../build/maps/ /e /njh /njs /ndl
@robocopy ../source/Stareater.UI.WinForms/bin/Debug/players/ ../build/players/ /e /njh /njs /ndl
@robocopy ../source/Stareater.UI.WinForms/bin/Debug/images/ ../build/images/ /e /njh /njs /ndl
pause
