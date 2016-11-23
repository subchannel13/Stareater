using System;
using Ikadn.Ikon.Types;
using OpenTK;

namespace Stareater.GLRenderers
{
	public struct TextureInfo
	{
		public int Id;
		public Vector2[] Coordinates;
		
		public TextureInfo(int textureId, IkonArray textureCoordsIkon)
		{
			this.Id = textureId;
			this.Coordinates = new Vector2[textureCoordsIkon.Count];
			
			for(int i = 0; i < textureCoordsIkon.Count; i++) {
				float[] coords = textureCoordsIkon[i].To<float[]>();
				this.Coordinates[i] = new Vector2(coords[0], coords[1]);
			}
		}
	}
}
