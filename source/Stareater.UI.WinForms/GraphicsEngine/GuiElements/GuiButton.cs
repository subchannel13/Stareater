using OpenTK;
using Stareater.GameScenes;
using Stareater.GLData;
using Stareater.GLData.SpriteShader;
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

		private BackgroundTexture mBackgroundHover = null;
		public BackgroundTexture BackgroundHover
		{
			get { return this.mBackgroundHover; }
			set
			{
				apply(ref this.mBackgroundHover, value);
			}
		}

		private BackgroundTexture mBackgroundNormal = null;
		public BackgroundTexture BackgroundNormal
		{
			get { return this.mBackgroundNormal; }
			set
			{
				apply(ref this.mBackgroundNormal, value);
			}
		}

		private TextureInfo? mForgroundImage = null;
		public TextureInfo? ForgroundImage
		{
			get { return this.mForgroundImage; }
			set
			{
				apply(ref this.mForgroundImage, value);
			}
		}

		private string mText = null;
		public string Text
		{
			get { return this.mText; }
			set
			{
				apply(ref this.mText, value);
			}
		}

		private Color mTextColor = Color.Black;
		public Color TextColor
		{
			get { return this.mTextColor; }
			set
			{
				apply(ref this.mTextColor, value);
			}
		}

		private float mTextSize = 0;
		public float TextSize
		{
			get { return this.mTextSize; }
			set
			{
				apply(ref this.mTextSize, value);
			}
		}

		private float mPadding = 0;
		public float Padding
		{
			get { return this.mPadding; }
			set
			{
				apply(ref this.mPadding, value);
			}
		}

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			var inside = this.isInside(mousePosition);

			apply(ref this.isPressed, inside);

			return inside;
		}

		public override bool OnMouseUp(Vector2 mousePosition)
		{
			/*
			 * If mouse is moved outside of the button after pressing down
			 * the button has to become unpressed.
			 */
			if (!this.isInside(mousePosition) || !this.isPressed)
			{
				apply(ref this.isPressed, false);

				return false;
			}

			this.isPressed = false;
			this.ClickCallback();
			this.updateScene();
			return true;
		}

		public override void OnMouseMove(Vector2 mousePosition)
		{
			apply(ref this.isHovered, this.isInside(mousePosition));
		}

		protected override SceneObject makeSceneObject()
		{
			var pressOffset = this.isPressed && this.isHovered ? new Vector2(PressOffsetX, PressOffsetY) : new Vector2(0, 0);

			var background = this.isHovered ? this.BackgroundHover : this.mBackgroundNormal;
			var soBuilder = new SceneObjectBuilder().
				Clip(this.Position.ClipArea). //TODO(v0.8) add press offset to clip area
				StartSprite(this.Z0, background.Sprite.Id, Color.White).
				Translate(this.Position.Center + pressOffset).
				AddVertices(SpriteHelpers.GuiBackground(background, this.Position.Size.X, this.Position.Size.Y));

			if (!string.IsNullOrWhiteSpace(this.Text))
			{
				//TODO(later) split single line if it doesn't fit
				var lines = this.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
				for (int i = 0; i < lines.Length; i++)
					soBuilder.StartSprite(this.Z0 - this.ZRange / 2, TextRenderUtil.Get.TextureId, this.TextColor).
						AddVertices(TextRenderUtil.Get.BufferText(lines[i], -0.5f, Matrix4.Identity)).
						Scale(this.TextSize, this.TextSize).
						Translate(this.Position.Center + new Vector2(0, (lines.Length / 2f - i) * this.TextSize));
			}

			if (this.mForgroundImage.HasValue)
				soBuilder.
					StartSimpleSprite(this.Z0 - this.ZRange / 2, this.mForgroundImage.Value, Color.White).
					Scale(this.Position.Size - new Vector2(2 * this.mPadding, 2 * this.mPadding)).
					Translate(this.Position.Center);

			return soBuilder.Build();
		}

		protected override float contentWidth()
		{
			return TextRenderUtil.Get.MeasureWidth(this.Text) * this.TextSize + 2 * this.mPadding;
		}

		protected override float contentHeight()
		{
			//TODO(later) count lines
			return (string.IsNullOrWhiteSpace(this.Text) ? 0 : this.TextSize) + 2 * this.mPadding;
		}
	}
}
