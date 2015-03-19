= How to build =

1. Install NuGet (VS extension or command line tool)
2. Enable NuGet package restore
3. Build {Stareater}/tools/Tools.sln
4. Run {Stareater}/scripts/gen_textures.bat
5. Run {Stareater}/scripts/link_build_to_bin.bat or {Stareater}/scripts/copy_build_to_bin.bat if you don't want symbolic links
6. Build {Stareater}/source/Zvjezdojedac.sln