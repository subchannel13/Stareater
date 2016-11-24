using System;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLData
{
	static class ShaderLibrary
	{
		public static PlanetOrbitGlProgram PlanetOrbit { get; private set; }
		public static SpriteGlProgram Sprite { get; private set; }
		
		public static void Load()
		{
			PlanetOrbit = new PlanetOrbitGlProgram();
			PlanetOrbit.Load();
				
			Sprite = new SpriteGlProgram();
			Sprite.Load();
		}
		
		public static void PrintGlErrors(string title)
		{
			ErrorCode err;
		    while ((err = GL.GetError()) != ErrorCode.NoError) {
		        System.Diagnostics.Trace.WriteLine(title + ": OpenGL error " + err);
		    }
		}
	}
}
