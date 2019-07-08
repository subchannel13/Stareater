using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using Stareater.GLData.OrbitShader;
using Stareater.GLData.SdfShader;
using Stareater.GLData.SpriteShader;

namespace Stareater.GLData
{
	static class ShaderLibrary
	{
		public static PlanetOrbitGlProgram PlanetOrbit { get; private set; }
		public static SdfGlProgram Sdf { get; private set; }
		public static SpriteGlProgram Sprite { get; private set; }
		
		public static void Load()
		{
			PlanetOrbit = new PlanetOrbitGlProgram();
			PlanetOrbit.Load();

			Sdf = new SdfGlProgram();
			Sdf.Load();

			Sprite = new SpriteGlProgram();
			Sprite.Load();
		}
		
		public static void PrintGlErrors(string title)
		{
			ErrorCode err;
			var messages = new List<string>();
			
		    while ((err = GL.GetError()) != ErrorCode.NoError)
		    	messages.Add(title + ": OpenGL error " + err);
    
		    if (messages.Count > 0)
		    {
			    #if DEBUG
			    foreach(var message in messages)
			        System.Diagnostics.Trace.WriteLine(message);
			    #else
			    throw new System.Exception(string.Join(System.Environment.NewLine, messages));
			    #endif
		    }
		}
	}
}
