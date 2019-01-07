using Ikadn.Ikon.Types;
using OpenTK;

namespace Stareater.GLData
{
	public struct TextureInfo
	{
		public int Id { get; private set; }
		
		/// <summary>
		/// 0 - Bottom left,
		/// 1 - Bottom right,
		/// 2 - Top right,
		/// 3 - Top left,
		/// </summary>
		public Vector2[] Coordinates { get; private set; }

		public TextureInfo(int textureId, IkonArray textureCoordsIkon)
		{
			this.Id = textureId;
			this.Coordinates = new Vector2[textureCoordsIkon.Count];
			
			for(int i = 0; i < textureCoordsIkon.Count; i++) {
				float[] coords = textureCoordsIkon[i].To<float[]>();
				this.Coordinates[i] = new Vector2(coords[0], coords[1]);
			}
		}

		public TextureInfo(int id, Vector2[] coordinates)
		{
			this.Id = id;
			this.Coordinates = coordinates;
		}
	}
}
