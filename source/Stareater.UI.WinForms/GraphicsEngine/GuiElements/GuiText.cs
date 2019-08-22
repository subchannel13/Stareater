using OpenTK;
using Stareater.GLData;
using System.Drawing;
using System;
using Stareater.AppData;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiText : AGuiElement
	{
		private Func<PolygonData, IAnimator> mAnimation = null;
		public Func<PolygonData, IAnimator> Animation
		{
			private get
			{
				return this.mAnimation;
			}
			set
			{
				this.apply(ref this.mAnimation, value);
			}
		}

		private string mText = null;
		public string Text
		{
			get { return this.mText; }
			set
			{
				this.apply(ref this.mText, value);
				this.reposition();
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

		protected override SceneObject makeSceneObject()
		{
			if (string.IsNullOrWhiteSpace(this.Text))
				return null;

			var soBuilder = new SceneObjectBuilder().
				Clip(this.Position.ClipArea).
				StartText(this.Text, this.fontSize(), -0.5f, this.Z0, this.ZRange, TextRenderUtil.Get.TextureId, this.TextColor, Matrix4.Identity).
				Scale(this.fontHeight, this.fontHeight).
				Translate(this.Position.Center + new Vector2(0, this.fontHeight * this.lineCount() / 2));

			if (this.Animation != null)
				return soBuilder.Build(polygons => this.Animation(polygons[0]));
			else
				return soBuilder.Build();
		}

		protected override Vector2 measureContent()
		{
			return new Vector2(
				TextRenderUtil.Get.WidthOf(this.Text, this.fontSize()) * this.fontHeight,
				this.fontHeight * this.lineCount()
			);
		}

		private float fontSize()
		{
			return this.TextSize * SettingsWinforms.Get.GuiScale;
		}

		private int lineCount()
		{
			//TODO(later) count lines in a better way
			return (this.Text != null) ? this.Text.Split('\n').Length : 0;
		}
	}
}
