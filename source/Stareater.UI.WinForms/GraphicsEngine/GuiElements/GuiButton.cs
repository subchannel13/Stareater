using OpenTK;
using Stareater.GameScenes;
using Stareater.GLData;
using System;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiButton : AGuiElement
	{
		private bool isHovered = false;

		public Action ClickCallback { get; set; }

		private TextureInfo? mBackgroundHover = null;
		public TextureInfo? BackgroundHover
		{
			get { return this.mBackgroundHover; }
			set
			{
				this.mBackgroundHover = value;
				this.UpdateScene();
			}
		}

		private TextureInfo? mBackgroundNormal = null;
		public TextureInfo? BackgroundNormal
		{
			get { return this.mBackgroundNormal; }
			set
			{
				this.mBackgroundNormal = value;
				this.UpdateScene();
			}
		}

		private string mText = null;
		public string Text
		{
			get { return this.mText; }
			set
			{
				this.mText = value;
				this.UpdateScene();
			}
		}

		private Color mTextColor = Color.Black;
		public Color TextColor
		{
			get { return this.mTextColor; }
			set
			{
				this.mTextColor = value;
				this.UpdateScene();
			}
		}

		private float mTextSize = 0;
		public float TextSize
		{
			get { return this.mTextSize; }
			set
			{
				this.mTextSize = value;
				this.UpdateScene();
			}
		}

		public override bool OnMouseClick(Vector2 mousePosition)
		{
			if (this.isOutside(mousePosition))
				return false;

			this.ClickCallback();
			return true;
		}

		public override void OnMouseMove(Vector2 mousePosition)
		{
			var oldState = this.isHovered;

            this.isHovered = !this.isOutside(mousePosition);
			if (this.isHovered != oldState)
				this.UpdateScene();
		}

		protected override SceneObject MakeSceneObject()
		{
			var background = (this.isHovered ? this.BackgroundHover : this.mBackgroundNormal).Value;
			var soBuilder = new SceneObjectBuilder().
				StartSimpleSprite(this.Z, background, Color.White).
				Scale(this.Position.Size.X, this.Position.Size.Y).
				Translate(this.Position.Center);

			if (!string.IsNullOrWhiteSpace(this.Text))
				soBuilder.StartSprite(this.Z / 2, TextRenderUtil.Get.TextureId, this.TextColor). //TODO(v0.7) better define GUI z range
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
