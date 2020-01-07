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

		private Vector2 direction = new Vector2(1, 0);
		public Orientation Orientation
		{
			set
			{
				if (value == Orientation.Horizontal)
					this.apply(ref this.direction, new Vector2(1, 0));
				else
					this.apply(ref this.direction, new Vector2(0, -1));
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

			var newValue = Methods.Clamp(
				Vector2.Dot(mousePosition - this.Position.Center, this.direction) /
				Math.Abs(Vector2.Dot(this.Position.Size, this.direction)) + 0.5f,
				0, 1
			);

			if (this.apply(ref this.mValue, newValue))
				this.SlideCallback(newValue);
		}

		public override void OnMouseScroll(Vector2 mousePosition, int delta)
		{
			if (this.direction.X != 0)
				delta *= -1;

			//TODO(v0.9) add scroll step value
			var newValue = Methods.Clamp(
				this.mValue + (delta < 0 ? 0.1f : -0.1f),
				0, 1
			);

			if (this.apply(ref this.mValue, newValue))
				this.SlideCallback(newValue);
		}

		protected override SceneObject makeSceneObject()
		{
			var railX = this.direction;
			var railY = this.direction.PerpendicularLeft;
			var railLength = Math.Abs(Vector2.Dot(this.Position.Size, railX)) - this.knobSize;
			var railWidth = Math.Abs(Vector2.Dot(this.Position.Size, railY)) - 4;
			var railSize = railX * railLength + railY * railWidth;

			var knobColor = this.mReadOnly ? Color.White : Color.DarkGray;
			var panelSprite = new BackgroundTexture(GalaxyTextures.Get.PanelBackground, 2);
			//TODO(v0.9) maybe make readonly mode have no knob but a fill from 0 to value

			return new SceneObjectBuilder().
				Clip(this.Position.ClipArea).
				StartSprite(this.Z0, GalaxyTextures.Get.PanelBackground.Id, Color.White).
				Translate(this.Position.Center).
				AddVertices(SpriteHelpers.GuiBackground(panelSprite, railSize.X, railSize.Y)).
				
				StartSprite(this.Z0 - this.ZRange / 2, GalaxyTextures.Get.PanelBackground.Id, knobColor).
				Translate(this.Position.Center + railX * railLength * (this.mValue - 0.5f)).
				AddVertices(SpriteHelpers.GuiBackground(panelSprite, this.knobSize, this.knobSize)).
				Build();
		}

		private float knobSize
		{
			get { return Math.Min(this.Position.Size.X, this.Position.Size.Y); }
		}
	}
}
