using OpenTK;
using Stareater.GameScenes;
using Stareater.GLData;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiText : IGuiElement
	{
		private AScene scene;
		private float z;
		private SceneObject graphicObject = null;
		
		public ElementPosition Position { get; private set; }

		public GuiText()
		{
			this.Position = new ElementPosition(this);
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

		float IGuiElement.ContentWidth()
		{
			return string.IsNullOrWhiteSpace(this.Text) ? 0 :
				TextRenderUtil.Get.MeasureWidth(this.Text) * this.TextSize;
		}

		float IGuiElement.ContentHeight()
		{
			return this.TextSize;
		}

		private void updateScene()
		{
			if (string.IsNullOrWhiteSpace(this.Text))
				return;

			scene.UpdateScene(
				ref this.graphicObject,
				new SceneObjectBuilder().
					StartSprite(z, TextRenderUtil.Get.TextureId, this.TextColor).
					AddVertices(TextRenderUtil.Get.BufferText(this.Text, -0.5f, Matrix4.Identity)).
					Scale(this.TextSize, this.TextSize).
					Translate(this.Position.Center).
					Build()
			);
		}
	}
}
