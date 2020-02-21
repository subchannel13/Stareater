using OpenTK;
using Stareater.GLData;
using System.Drawing;

namespace Stareater.GraphicsEngine
{
	class Sprite
	{
		public TextureInfo Texture { get; private set; }
		public Color ModulationColor { get; private set; }
		public Matrix4 Transform { get; internal set; }

		public Sprite(TextureInfo texture) : this(texture, Color.White, Matrix4.Identity)
		{ }

		public Sprite(TextureInfo texture, Color modulationColor) : this(texture, modulationColor, Matrix4.Identity)
		{ }

		public Sprite(TextureInfo texture, Color modulationColor, Matrix4 transform)
		{
			this.Texture = texture;
			this.ModulationColor = modulationColor;
			this.Transform = transform;
		}
	}
}
