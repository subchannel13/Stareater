using System;
using Ikadn.Ikon.Types;
using OpenTK;

namespace Stareater.GLRenderers
{
	public struct TextureInfo
	{
		public int TextureId;
		public Vector2[] TextureCoords;
		
		public TextureInfo(int textureId, IkonArray textureCoordsIkon)
		{
			this.TextureId = textureId;
			this.TextureCoords = new Vector2[textureCoordsIkon.Count];
			
			for(int i = 0; i < textureCoordsIkon.Count; i++) {
				float[] coords = textureCoordsIkon[i].To<float[]>();
				this.TextureCoords[i] = new Vector2(coords[0], coords[1]);
			}
		}
	}
}
