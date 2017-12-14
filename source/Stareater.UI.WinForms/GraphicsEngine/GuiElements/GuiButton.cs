using OpenTK;
using Stareater.GameScenes;
using Stareater.GLData;
using System;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiButton : IGuiElement
	{
		private AScene scene;
		private float z;
		private SceneObject graphicObject = null;
		private bool isHovered = false;

		public ElementPosition Position { get; private set; }

		public GuiButton()
		{
			this.Position = new ElementPosition(() => 0, () => 0);
		}

		public Action ClickCallback { get; set; }

		private TextureInfo? mBackgroundHover = null;
		public TextureInfo? BackgroundHover
		{
			get { return this.mBackgroundHover; }
			set
			{
				this.mBackgroundHover = value;
				this.updateScene();
			}
		}

		private TextureInfo? mBackgroundNormal = null;
		public TextureInfo? BackgroundNormal
		{
			get { return this.mBackgroundNormal; }
			set
			{
				this.mBackgroundNormal = value;
				this.updateScene();
			}
		}

		private string mText = null;
		public string Text
		{
			get { return this.mText; }
			set
			{
				this.mText = value;
				this.updateScene();
			}
		}

		private Color mTextColor = Color.Black;
		public Color TextColor
		{
			get { return this.mTextColor; }
			set
			{
				this.mTextColor = value;
				this.updateScene();
			}
		}

		private float mTextSize = 0;
		public float TextSize
		{
			get { return this.mTextSize; }
			set
			{
				this.mTextSize = value;
				this.updateScene();
			}
		}

		void IGuiElement.Attach(AScene scene, float z)
		{
			this.scene = scene;
			this.z = z;

			this.updateScene();
		}

		void IGuiElement.RecalculatePosition(float parentWidth, float parentHeight)
		{
			this.Position.Recalculate(parentWidth, parentHeight);
			this.updateScene();
		}

		bool IGuiElement.OnMouseClick(Vector2 mousePosition)
		{
			if (this.isOutside(mousePosition))
				return false;

			this.ClickCallback();
			return true;
		}

		void IGuiElement.OnMouseMove(Vector2 mousePosition)
		{
			var oldState = this.isHovered;

            this.isHovered = !this.isOutside(mousePosition);
			if (this.isHovered != oldState)
				this.updateScene();
		}

		private void updateScene()
		{
			if (this.scene == null)
				return;

			var background = (this.isHovered ? this.BackgroundHover : this.mBackgroundNormal).Value;
			var soBuilder = new SceneObjectBuilder().
				StartSimpleSprite(this.z, background, Color.White).
				Scale(this.Position.Size.X, this.Position.Size.Y).
				Translate(this.Position.Center);

			if (!string.IsNullOrWhiteSpace(this.Text))
				soBuilder.StartSprite(z / 2, TextRenderUtil.Get.TextureId, this.TextColor). //TODO(v0.7) better define GUI z range
					AddVertices(TextRenderUtil.Get.BufferText(this.Text, -0.5f, Matrix4.Identity)).
					Scale(this.TextSize, this.TextSize).
					Translate(this.Position.Center);

			this.scene.UpdateScene(
				ref this.graphicObject,
				soBuilder.Build()
			);
		}

		private bool isOutside(Vector2 mousePosition)
		{
			var innerPoint = mousePosition - this.Position.Center;
			return Math.Abs(innerPoint.X) > this.Position.Size.X / 2 ||
				Math.Abs(innerPoint.Y) > this.Position.Size.Y / 2;
        }
	}
}
