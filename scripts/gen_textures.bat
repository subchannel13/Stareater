@echo off
rem Generates texture atlas
@echo on

"../tools/Stareater.TextureAtlas/bin/Debug/Stareater.TextureAtlas.exe" ../graphics/galaxyTextures -O ../build/images/galaxyTextures.png ../build/images/galaxyTextures.txt
