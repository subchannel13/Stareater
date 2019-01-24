using OpenTK;
using Stareater.GLData;
using Stareater.GLData.SpriteShader;
using Stareater.Utils;
using System;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiSlider : AGuiElement
	{
		public Action<float> SlideCallback { get; set; }

		private float mValue = 0;
		public float Value
		{
			get { return this.mValue; }
			set
			{
				this.apply(ref this.mValue, Methods.Clamp(mValue, 0, 1));
			}
		}

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			var newValue = Methods.Clamp((mousePosition.X - this.Position.Center.X) / (this.Position.Size.X - this.knobSize) + 0.5f, 0, 1);

			if (newValue != this.mValue)
			{
				this.mValue = newValue;
				this.SlideCallback(newValue);
				this.updateScene();
			}

			return true;
		}

		protected override SceneObject makeSceneObject()
		{
			var railWidth = this.Position.Size.X - this.knobSize;

			return new SceneObjectBuilder().
				Clip(this.Position.ClipArea).
				StartSimpleSprite(this.Z0, GalaxyTextures.Get.PanelBackground, Color.White).
				Translate(this.Position.Center).
				AddVertices(SpriteHelpers.GuiBackground(new BackgroundTexture(GalaxyTextures.Get.PanelBackground, 2), railWidth, this.Position.Size.Y - 4)).

				StartSimpleSprite(this.Z0 - this.ZRange / 2, GalaxyTextures.Get.PanelBackground, Color.Gray).
				Scale(this.knobSize, this.knobSize).
				Translate(this.Position.Center + new Vector2(railWidth * (this.mValue - 0.5f), 0)).
				Build();
		}

		private float knobSize
		{
			get { return Math.Min(this.Position.Size.X, this.Position.Size.Y); }
		}
	}
}
