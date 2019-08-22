using System.Drawing;
using OpenTK;

namespace Stareater.GLData
{
	public class CharTextureInfo
	{
		public Vector2[] VertexCoords { get; private set; }
		public Vector2[] TextureCoords { get; private set; }
		public float Aspect { get; private set; }

		public CharTextureInfo(Rectangle bitmapRegion, float textureWidth, float textureHeight, float polygonWidth, float polygonHeight, float polygonX, float polygonY)
		{
			var offset = new Vector2(bitmapRegion.X / textureWidth, bitmapRegion.Y / textureHeight);
			var width = bitmapRegion.Width / textureWidth;
			var height = bitmapRegion.Height / textureHeight;

			this.TextureCoords = new Vector2[] {
				offset,
				offset + new Vector2(width, 0),
				offset + new Vector2(width, height),

				offset + new Vector2(width, height),
				offset + new Vector2(0, height),
				offset,
			};

			this.Aspect = bitmapRegion.Width / (float)bitmapRegion.Height;

			this.VertexCoords = new Vector2[] {
				new Vector2(-polygonWidth / 2 + polygonX, polygonHeight / 2 + polygonY),
				new Vector2(polygonWidth / 2 + polygonX, polygonHeight / 2 + polygonY),
				new Vector2(polygonWidth / 2 + polygonX, -polygonHeight / 2 + polygonY),

				new Vector2(polygonWidth / 2 + polygonX, -polygonHeight / 2 + polygonY),
				new Vector2(-polygonWidth / 2 + polygonX, -polygonHeight / 2 + polygonY),
				new Vector2(-polygonWidth / 2 + polygonX, polygonHeight / 2 + polygonY),
			};
		}
	}
}
