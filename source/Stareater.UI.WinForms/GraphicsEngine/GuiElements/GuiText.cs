using OpenTK;
using Stareater.GLData;
using System.Drawing;
using System;
using Stareater.AppData;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiText : AGuiElement
	{
		private Vector2 textSize;

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
				this.textSize = TextRenderUtil.Get.SizeOf(value);
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

		private float mTextHeight = 0;
		public float TextHeight
		{
			get { return this.mTextHeight; }
			set
			{
				this.apply(ref this.mTextHeight, value);
			}
		}

		protected override SceneObject makeSceneObject()
		{
			if (string.IsNullOrWhiteSpace(this.Text))
				return null;

			var soBuilder = new SceneObjectBuilder().
				PixelSize(1 / SettingsWinforms.Get.GuiScale).
				Clip(this.Position.ClipArea).
				StartText(this.Text, -0.5f, 0, this.Z0, this.ZRange, this.TextColor).
				Scale(this.TextHeight).
				Translate(this.Position.Center + new Vector2(0, this.TextHeight * textSize.Y / 2));

			if (this.Animation != null)
				return soBuilder.Build(polygons => this.Animation(polygons[0]));
			else
				return soBuilder.Build();
		}

		protected override Vector2 measureContent()
		{
			return this.textSize * this.TextHeight;
		}
	}
}
