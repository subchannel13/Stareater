@echo off
rem Generates texture atlas
@echo on

"../tools/Stareater.TextureAtlas/bin/Debug/Stareater.TextureAtlas.exe" ../graphics/GalaxyTextures -O ../build/images/GalaxyTextures.png ../build/images/GalaxyTextures.txt
