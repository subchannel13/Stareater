using System;

namespace Stareater.GLData
{
	class SpriteInfo
	{
		public VertexArray SpriteSheet { get; private set; }
		public int TextureId { get; private set; }
		public int SpriteIndex { get; private set; }
		
		public SpriteInfo(VertexArray spriteSheet, int textureId, int spriteIndex)
		{
			this.SpriteSheet = spriteSheet;
			this.TextureId = textureId;
			this.SpriteIndex = spriteIndex;
		}
	}
}
