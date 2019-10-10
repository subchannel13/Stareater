using Stareater.GLData;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiImage : AGuiElement
	{
		private Sprite[] mImages = null;

		public Sprite[] Images
		{
			get { return this.mImages; }
			set
			{
				this.apply(ref this.mImages, value);
			}
		}

		public TextureInfo Image
		{
			set
			{
				this.apply(ref this.mImages, new[] { new Sprite(value, Color.White) });
			}
		}

		protected override SceneObject makeSceneObject()
		{
			var soBuilder = new SceneObjectBuilder();
			if (this.mImages != null && this.mImages.Length > 0)
				for (int i = 0; i < this.mImages.Length; i++)
					soBuilder.StartSimpleSprite(this.Z0 - i * this.ZRange / this.mImages.Length, this.mImages[i].Texture, this.mImages[i].ModulationColor).
						Scale(this.Position.Size).
						Translate(this.Position.Center);

			return soBuilder.Build();
		}
	}
}
