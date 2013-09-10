@echo off
rem Runs copy_game_to_build.bat script and then
rem copies game data from {projRoot}\build\ to 
rem {projRoot}\release\Stareater
@echo on

call copy_game_to_build.bat

rmdir /s /q ..\release\Stareater
del /q ..\release\
robocopy ../build/ ../release/Stareater/ /xd .svn /e
