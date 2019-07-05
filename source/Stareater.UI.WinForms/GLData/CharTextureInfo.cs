using System.Drawing;
using OpenTK;

namespace Stareater.GLData
{
	public class CharTextureInfo
	{
		public Vector2[] TextureCoords { get; private set; }
		public float Aspect { get; private set; }
		
		public CharTextureInfo(Rectangle rect, float width, float height)
		{
			var offset = new Vector2(rect.X / width, rect.Y / height);
			width = rect.Width / width;
			height = rect.Height / height;

			this.TextureCoords = new Vector2[] {
				offset,
				offset + new Vector2(width, 0),
				offset + new Vector2(width, height),

				offset + new Vector2(width, height),
				offset + new Vector2(0, height),
				offset,
			};

			this.Aspect = width / height;
		}
	}
}
