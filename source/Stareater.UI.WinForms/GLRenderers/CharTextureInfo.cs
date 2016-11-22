using System;
using System.Drawing;
using OpenTK;

namespace Stareater.GLRenderers
{
	public class CharTextureInfo
	{
		public Vector2[] TextureCoords { get; private set; }
		public float Aspect { get; private set; }
		
		public CharTextureInfo(Vector2 offset, SizeF size)
		{
			this.TextureCoords = new Vector2[] {
				offset,
				offset + new Vector2(size.Width, 0),
				offset + new Vector2(size.Width, size.Height),
				
				offset + new Vector2(size.Width, size.Height),
				offset + new Vector2(0, size.Height),
				offset,
			};
			
			this.Aspect = size.Width / size.Height;
		}
	}
}
