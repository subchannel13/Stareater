# Stareater

Stareater is a turn based 4X strategy game set in space. Play as a leader of a nation, research futuristic technologies, colonize other star systems, fight or negotiate with other leaders and solve the mysteries of a star eating creature. Previously known under Croatian name Zvjezdojedac, the project is primarily inspired by Master of Orion II: Battle at Antares but there are influences of the other games of the genre such as Master of Orion I & III, Ogame, Sword of the Stars and Space Empires 5. This project is a prototype meaning it's main objective is to implement and test the idea.

Although this is a C# project, builds can be run with Mono in both Linux and Mac OS.

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.me/IvanKravarscan/5)

# How to build

Instructions for Windows and debug configuration:

1. Install NuGet (command line tool for managing external dependencies)
  * You may need to configure your IDE to use it
  * There is an extension for Visual Studio
  * SharpDevelop already comes with integrated NuGet
2. Build `{Stareater}/tools/Tools.sln`
  * `{Stareater}` is repository root directory
  * Build with "Debug" configuration
  * It will create texture generator tool needed for the next step
4. Run `{Stareater}/scripts/gen_textures.bat`
  * OS-es other then Windows may not know how to execute the script, in that case one has to manually run texture generator with appropriate parameters
5. Make sure game data is available to game's executable
  * Game data files are in `{Stareater}/build/` directory
  * Game's executable file is in `{Stareater}/source/Stareater.UI.WinForms/bin/Debug/` directory
    * Set IDE to run the project with following command line parameter: `-root ../../../../build/`
    * Alternatively, run `{Stareater}/scripts/copy_build_to_bin.bat` to copy files next to executable
  * Again, scrips may not work on OS-es other then Windows
6. Build `{Stareater}/source/Zvjezdojedac.sln`
7. If all steps above were successful, the game can be run

# Licence

Stareater executable and it's source in `{Stareater}/source/Stareater.UI.WinForms/` are licenced under GPLv3.

Following code is licenced under LGPL:
* `{Stareater}/source/Stareater.Core/` (Stareater core logic)
* `{Stareater}/source/Stareater.Maps.ProximityLanes/` (2nd stage of default map generator)
* `{Stareater}/source/Stareater.Maps.RybPopulator/` (3rd stage of default map generator)
* `{Stareater}/source/Stareater.Maps.Square/` (1st stage of default map generator)
* `{Stareater}/source/Stareater.Players.DefaultAI/` (default AI player)
* `{Stareater}/source/Zvjezdojedac editori/` (modding tools for v0.4)
* `{Stareater}/source/Zvjezdojedac/` (v0.4 game code)
* `{Stareater}/documentation/` (GUI mockups)
* `{Stareater}/tools/` (development helper tools)