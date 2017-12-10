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

		public ElementPosition Position { get; private set; }

		public GuiButton()
		{
			this.Position = new ElementPosition(() => 0, () => 0);
		}

		public Action ClickCallback { get; set; }

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

		private void updateScene()
		{
			if (this.scene == null)
				return;

			var soBuilder = new SceneObjectBuilder().
				StartSimpleSprite(this.z, GalaxyTextures.Get.ButtonBackground, Color.White). //TODO(v0.7) make separate drawing mechanism
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
	}
}
