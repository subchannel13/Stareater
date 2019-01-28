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
				this.apply(ref this.mValue, Methods.Clamp(value, 0, 1));
			}
		}

		private bool mReadOnly = false;
		public bool ReadOnly
		{
			get { return this.mReadOnly; }
			set
			{
				this.apply(ref this.mReadOnly, value);
			}
		}

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			this.OnMouseDrag(mousePosition);

			return true;
		}

		public override void OnMouseDrag(Vector2 mousePosition)
		{
			if (this.mReadOnly)
				return;

			var newValue = Methods.Clamp((mousePosition.X - this.Position.Center.X) / (this.Position.Size.X - this.knobSize) + 0.5f, 0, 1);

			if (newValue != this.mValue)
			{
				this.mValue = newValue;
				this.SlideCallback(newValue);
				this.updateScene();
			}
		}

		protected override SceneObject makeSceneObject()
		{
			var railWidth = this.Position.Size.X - this.knobSize;
			var knobColor = this.mReadOnly ? Color.White : Color.DarkGray;
			var panelSprite = new BackgroundTexture(GalaxyTextures.Get.PanelBackground, 2);
			//TODO(v0.8) maybe make readonly mode have no knob but a fill from 0 to value

			return new SceneObjectBuilder().
				Clip(this.Position.ClipArea).
				StartSimpleSprite(this.Z0, GalaxyTextures.Get.PanelBackground, Color.White).
				Translate(this.Position.Center).
				AddVertices(SpriteHelpers.GuiBackground(panelSprite, railWidth, this.Position.Size.Y - 4)).

				StartSprite(this.Z0 - this.ZRange / 2, GalaxyTextures.Get.PanelBackground.Id, knobColor).
				Translate(this.Position.Center + new Vector2(railWidth * (this.mValue - 0.5f), 0)).
				AddVertices(SpriteHelpers.GuiBackground(panelSprite, this.knobSize, this.knobSize)).
				Build();
		}

		private float knobSize
		{
			get { return Math.Min(this.Position.Size.X, this.Position.Size.Y); }
		}
	}
}
