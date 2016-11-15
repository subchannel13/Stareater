using System;

namespace Stareater.GLData
{
	static class ShaderLibrary
	{
		public static SpriteGlProgram Sprite { get; private set; }
		
		public static void Load()
		{
			Sprite = new SpriteGlProgram();
			Sprite.Load();
		}
	}
}
