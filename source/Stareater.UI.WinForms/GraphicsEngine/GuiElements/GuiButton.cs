using OpenTK;
using Stareater.AppData;
using Stareater.GLData;
using Stareater.GLData.SpriteShader;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiButton : AGuiElement
	{
		private const float PressOffsetX = 1.5f;
		private const float PressOffsetY = -3;

		private bool isPressed = false;
		private bool isHovered = false;
		private float paddingX = 0;
		private float paddingY = 0;

		public Action ClickCallback { get; set; }

		private BackgroundTexture mBackgroundHover = null;
		public BackgroundTexture BackgroundHover
		{
			get { return this.mBackgroundHover; }
			set
			{
				this.apply(ref this.mBackgroundHover, value);
			}
		}

		private BackgroundTexture mBackgroundNormal = null;
		public BackgroundTexture BackgroundNormal
		{
			get { return this.mBackgroundNormal; }
			set
			{
				this.apply(ref this.mBackgroundNormal, value);
			}
		}

		private TextureInfo? mForgroundImage = null;
		public TextureInfo? ForgroundImage
		{
			get { return this.mForgroundImage; }
			set
			{
				this.apply(ref this.mForgroundImage, value);
			}
		}

		private string mText = null;
		public string Text
		{
			get { return this.mText; }
			set
			{
				this.apply(ref this.mText, value);
			}
		}

		private Color mTextColor = Color.Black;
		public Color TextColor
		{
			get { return this.mTextColor; }
			set
			{
				this.apply(ref this.mTextColor, value);
			}
		}

		private float mTextSize = 0;
		private float fontHeight = 0;
		public float TextSize
		{
			get { return this.mTextSize; }
			set
			{
				this.fontHeight = TextRenderUtil.Get.FontHeight(value);
				this.apply(ref this.mTextSize, value);
			}
		}

		public float Padding
		{
			set
			{
				this.apply(ref this.paddingX, value);
				this.apply(ref this.paddingY, value);
			}
		}

		public float PaddingX
		{
			get { return this.paddingX; }
			set
			{
				this.apply(ref this.paddingX, value);
			}
		}

		public float PaddingY
		{
			get { return this.paddingY; }
			set
			{
				this.apply(ref this.paddingY, value);
			}
		}

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			this.apply(ref this.isPressed, true);

			return true;
		}

		public override void OnMouseUp(Keys modiferKeys)
		{
			this.isPressed = false;
			this.ClickCallback();
			this.updateScene();
		}

		public override void OnMouseDownCanceled()
		{
			this.apply(ref this.isPressed, false);
		}

		public override void OnMouseMove(Vector2 mousePosition, Keys modiferKeys)
		{
			this.apply(ref this.isHovered, true);
		}

		public override void OnMouseLeave()
		{
			this.apply(ref this.isHovered, false);
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
					soBuilder.StartText(lines[i], this.fontSize(), -0.5f, this.Z0 - this.ZRange / 2, TextRenderUtil.Get.TextureId, this.TextColor, Matrix4.Identity).
						Scale(this.fontHeight, this.fontHeight).
						Translate(this.Position.Center + new Vector2(0, (lines.Length / 2f - i) * this.fontHeight));
			}

			if (this.mForgroundImage.HasValue)
				soBuilder.
					StartSimpleSprite(this.Z0 - this.ZRange / 2, this.mForgroundImage.Value, Color.White).
					Scale(this.Position.Size - new Vector2(2 * this.paddingX, 2 * this.paddingY)).
					Translate(this.Position.Center);

			return soBuilder.Build();
		}

		protected override Vector2 measureContent()
		{
			//TODO(later) count lines
			return new Vector2(
				TextRenderUtil.Get.MeasureWidth(this.Text, this.fontSize()) * this.fontHeight + 2 * this.paddingX,
				(string.IsNullOrWhiteSpace(this.Text) ? 0 : this.fontHeight) + 2 * this.paddingY
			);
		}

		private float fontSize()
		{
			return this.fontHeight * SettingsWinforms.Get.GuiScale;
		}
	}
}
