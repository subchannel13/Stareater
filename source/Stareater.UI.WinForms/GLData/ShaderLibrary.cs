using System;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLData
{
	static class ShaderLibrary
	{
		public static SpriteGlProgram Sprite { get; private set; }
		
		private static IGlProgram lastProgram = null;
		
		public static void Load()
		{
			Sprite = new SpriteGlProgram();
			Sprite.Load();
		}
		
		public static void Use(IGlProgram program)
		{
			if (program == lastProgram)
				return;
			
			//GL.UseProgram(program.ProgramId);
			int lastAttributes = (lastProgram != null) ? lastProgram.AttributeIndices : 0;
			int newAttributes = program.AttributeIndices;
			int difference = lastAttributes ^ newAttributes;
			
			for(int i = 0; difference != 0; i++)
			{
				int bit = 1 << i;
				if ((difference & bit) != 0)
				{
					if ((newAttributes & bit) != 0)
						GL.EnableVertexAttribArray(i);
					else
						GL.DisableVertexAttribArray(i);
			
					difference -= bit;
				}
			}
			lastProgram = program;
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
