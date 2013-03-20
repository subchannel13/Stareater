@echo off
rem Runs Coco/R parser generator to generate expression parser. Before running
rem make sure there is path in PATH environment variable to a folder with
rem the Coco.exe or that Coco.exe in in the same folder where the script is
rem executed.
@echo on

Coco.exe ..\source\Stareater.Core\AppData\Expressions\Grammar.atg
@pause
