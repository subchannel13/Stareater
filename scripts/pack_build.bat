@echo off
rem Runs copy_game_to_build.bat script and then
rem copies game data from {projRoot}\build\ to 
rem {projRoot}\release\Stareater
@echo on

call copy_game_to_build.bat

rmdir /s /q ..\release\Stareater
del /q ..\release\
robocopy ../build/ ../release/Stareater/
robocopy ../build/data/ ../release/Stareater/data/ /e
robocopy ../build/images/ ../release/Stareater/images/ /e
robocopy ../build/languages/ ../release/Stareater/languages/ /e
robocopy ../build/maps/ ../release/Stareater/maps/ /XF *.pdb
robocopy ../build/players/ ../release/Stareater/players/ /XF *.pdb /XF *.txt
