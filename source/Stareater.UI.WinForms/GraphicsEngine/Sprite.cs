using Stareater.GLData;
using System.Drawing;

namespace Stareater.GraphicsEngine
{
	class Sprite
	{
		public TextureInfo Texture { get; private set; }
		public Color ModulationColor { get; private set; }

		public Sprite(TextureInfo texture, Color modulationColor)
		{
			this.Texture = texture;
			this.ModulationColor = modulationColor;
		}
	}
}
