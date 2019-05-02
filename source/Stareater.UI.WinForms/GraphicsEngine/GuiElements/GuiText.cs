using OpenTK;
using Stareater.GameScenes;
using Stareater.GLData;
using System.Drawing;
using System;

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
		public float TextSize
		{
			get { return this.mTextSize; }
			set
			{
				this.apply(ref this.mTextSize, value);
			}
		}

		protected override SceneObject makeSceneObject()
		{
			if (string.IsNullOrWhiteSpace(this.Text))
				return null;

			var soBuilder = new SceneObjectBuilder().
				Clip(this.Position.ClipArea).
				StartSprite(this.Z0, TextRenderUtil.Get.TextureId, this.TextColor).
				AddVertices(TextRenderUtil.Get.BufferText(this.Text, -0.5f, Matrix4.Identity)).
				Scale(this.TextSize, this.TextSize).
				Translate(this.Position.Center + new Vector2(0, this.TextSize * this.lineCount() / 2));

			if (this.Animation != null)
				return soBuilder.Build(polygons => this.Animation(polygons[0]));
			else
				return soBuilder.Build();
		}

		protected override Vector2 measureContent()
		{
			return new Vector2(
				TextRenderUtil.Get.MeasureWidth(this.Text) * this.TextSize,
				this.TextSize * this.lineCount()
			);
		}

		private int lineCount()
		{
			//TODO(later) count lines in a better way
			return (this.Text != null) ? this.Text.Split('\n').Length : 0;
		}
	}
}
