using System;

namespace Stareater.GLData
{
	class SpriteInfo
	{
		public VertexArray SpriteSheet { get; private set; }
		public int SpriteIndex { get; private set; }
		public TextureInfo Texture { get; private set; }
		
		public SpriteInfo(VertexArray spriteSheet, int spriteIndex, TextureInfo texture)
		{
			this.SpriteSheet = spriteSheet;
			this.SpriteIndex = spriteIndex;
			this.Texture = texture;
		}
	}
}
