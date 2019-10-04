using Stareater.GLData;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiImage : AGuiElement
	{
		private TextureInfo? mImage = null;
		public TextureInfo? Image
		{
			get { return this.mImage; }
			set
			{
				this.apply(ref this.mImage, value);
			}
		}

		protected override SceneObject makeSceneObject()
		{
			var soBuilder = new SceneObjectBuilder();
			if (this.mImage.HasValue)
				soBuilder.StartSimpleSprite(this.Z0, this.mImage.Value, Color.White).
					Scale(this.Position.Size).
					Translate(this.Position.Center);

			return soBuilder.Build();
		}
	}
}
