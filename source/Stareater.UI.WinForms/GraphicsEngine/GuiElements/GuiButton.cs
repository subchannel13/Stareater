using OpenTK;
using Stareater.GameScenes;
using Stareater.GLData;
using System;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiButton : AGuiElement
	{
		private const float PressOffsetX = 1.5f;
		private const float PressOffsetY = -3;

		private bool isPressed = false;
		private bool isHovered = false;

		public Action ClickCallback { get; set; }

		private TextureInfo? mBackgroundHover = null;
		public TextureInfo? BackgroundHover
		{
			get { return this.mBackgroundHover; }
			set
			{
				Apply(ref this.mBackgroundHover, value);
			}
		}

		private TextureInfo? mBackgroundNormal = null;
		public TextureInfo? BackgroundNormal
		{
			get { return this.mBackgroundNormal; }
			set
			{
				Apply(ref this.mBackgroundNormal, value);
			}
		}

		private string mText = null;
		public string Text
		{
			get { return this.mText; }
			set
			{
				Apply(ref this.mText, value);
			}
		}

		private Color mTextColor = Color.Black;
		public Color TextColor
		{
			get { return this.mTextColor; }
			set
			{
				Apply(ref this.mTextColor, value);
			}
		}

		private float mTextSize = 0;
		public float TextSize
		{
			get { return this.mTextSize; }
			set
			{
				Apply(ref this.mTextSize, value);
			}
		}

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			var inside = !this.isOutside(mousePosition);

			Apply(ref this.isPressed, inside);

			return inside;
		}

		public override bool OnMouseUp(Vector2 mousePosition)
		{
			if (this.isOutside(mousePosition) || !this.isPressed)
			{
				Apply(ref this.isPressed, false);

				return false;
			}

			this.isPressed = false;
			this.ClickCallback();
			this.UpdateScene();
			return true;
		}

		public override void OnMouseMove(Vector2 mousePosition)
		{
			Apply(ref this.isHovered, !this.isOutside(mousePosition));
		}

		protected override SceneObject MakeSceneObject()
		{
			var pressOffset = this.isPressed && this.isHovered ? new Vector2(PressOffsetX, PressOffsetY) : new Vector2(0, 0);

			var background = (this.isHovered ? this.BackgroundHover : this.mBackgroundNormal).Value;
			var soBuilder = new SceneObjectBuilder().
				StartSimpleSprite(this.Z, background, Color.White).
				Scale(this.Position.Size.X, this.Position.Size.Y).
				Translate(this.Position.Center + pressOffset);

			if (!string.IsNullOrWhiteSpace(this.Text))
				soBuilder.StartSprite(this.Z / 2, TextRenderUtil.Get.TextureId, this.TextColor). //TODO(v0.8) better define GUI z range
					AddVertices(TextRenderUtil.Get.BufferText(this.Text, -0.5f, Matrix4.Identity)).
					Scale(this.TextSize, this.TextSize).
					Translate(this.Position.Center);

			return soBuilder.Build();
		}

		private bool isOutside(Vector2 mousePosition)
		{
			var innerPoint = mousePosition - this.Position.Center;
			return Math.Abs(innerPoint.X) > this.Position.Size.X / 2 ||
				Math.Abs(innerPoint.Y) > this.Position.Size.Y / 2;
        }
	}
}
