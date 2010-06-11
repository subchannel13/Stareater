@echo off
rem Runs copy_game_to_build.bat script and then
rem copies game data from {svnRoot}\build\ to 
rem {svnRoot}\release\Zvjezdojedac

call copy_game_to_build.bat

rmdir /s /q ..\release\Zvjezdojedac
del /q ..\release\
robocopy ../build/ ../release/Zvjezdojedac/ /xd .svn /e
