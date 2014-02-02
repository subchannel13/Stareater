@echo off
rem Creates links (junctions) to folders in {projRoot}\build\
rem in {projRoot}/source/Stareater.UI.WinForms/bin/Debug/
rem and {projRoot}/source/Stareater.UI.WinForms/bin/Release/

setlocal

set debugDir=..\source\Stareater.UI.WinForms\bin\Debug
set releaseDir=..\source\Stareater.UI.WinForms\bin\Release

for %%B in (%debugDir% %releaseDir%) do (
	if not exist %%B (
		echo Creating directory %%B
		mkdir %%B
	)
	
	for %%D in (data images languages maps players) do (
		if exist %%B\%%D (
			echo Path %%B\%%D already exists, no action taken
		) else (
			mklink /J %%B\%%D ..\build\%%D
		)
	)
)
pause