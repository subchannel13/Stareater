using OpenTK;
using Stareater.GLData;
using Stareater.GraphicsEngine;
using Stareater.GraphicsEngine.GuiElements;
using System.Drawing;

namespace Stareater.GameScenes.Widgets
{
	class DevelopmentIndicator : AGuiElement
	{
		private float mValue = 0;
		public double Value
		{
			get { return this.mValue; }
			set
			{
				this.apply(ref this.mValue, (float)value);
			}
		}

		protected override SceneObject makeSceneObject()
		{
			return new SceneObjectBuilder().
				StartSimpleSprite(this.Z0, GalaxyTextures.Get.PathLine, Color.FromArgb(0, 148, 255)).
				Scale(this.Position.Size * new Vector2(this.mValue, 1)).
				Translate(this.Position.Center - new Vector2(this.Position.Size.X * (1 - this.mValue) / 2, 0)).

				StartSimpleSprite(this.Z0, GalaxyTextures.Get.PathLine, Color.FromArgb(0, 74, 127)).
				Scale(this.Position.Size * new Vector2(1 - this.mValue, 1)).
				Translate(this.Position.Center + new Vector2(this.Position.Size.X * this.mValue / 2, 0)).
				Build();
		}
	}
}
