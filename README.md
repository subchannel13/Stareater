# Stareater

Stareater (croatian Zvjezdojedac) is a turn based 4X strategy game. Game was primarily inspired by Master of Orion II: Battle at Antares but other games such as Master of Orion I & III, Ogame, Sword of the Stars and Space Empires 5 influenced some concepts of the Stareater.

This project is a prototype meaning it's main objective is to implement and test the idea. GUI, graphics and sounds are secondary. If prototype proves to be good, development of real game will be considered. 

Version 0.3 and later have multilingual support and have Croatian and English translation. Prior versions are Croatian only. Although this is a C# project, builds can be run under Mono in both Linux and Mac OS. 

# How to build

Instructions for Windows and debug configuration:

1. Install NuGet (command line tool for managing external dependencies)
  * You may need to configure your IDE to use it
  * There is an extension for Visual Studio
  * SharpDevelop comes with integrated NuGet
2. Build `{Stareater}/tools/Tools.sln`
  * `{Stareater}` is repository root directory
  * Build with "Debug" configuration
  * It will build texture generator tool for the next step
4. Run `{Stareater}/scripts/gen_textures.bat`
5. Make game data available to game's executable
  * Game data files are in `{Stareater}/build/ directory`
  * Game's executable file in `{Stareater}/Stareater.UI.WinForms/bin/Debug/` directory
  * Run `{Stareater}/scripts/link_build_to_bin.bat` to create symbolic links
  * Run `{Stareater}/scripts/copy_build_to_bin.bat` to copy files if you don't want symbolic links
6. Build `{Stareater}/source/Zvjezdojedac.sln`
7. If all steps above were successful, the game can be run