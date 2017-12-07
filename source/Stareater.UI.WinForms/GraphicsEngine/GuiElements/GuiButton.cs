using Stareater.GLData;
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
			this.Position = new ElementPosition(this);
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
			return 0;
		}

		float IGuiElement.ContentHeight()
		{
			return 0;
		}

		private void updateScene()
		{
			scene.UpdateScene(
				ref this.graphicObject,
				new SceneObjectBuilder().
					StartSimpleSprite(this.z, GalaxyTextures.Get.ButtonBackground, Color.White). //TODO(v0.7) make separate drawing mechanism
					Scale(this.Position.Size.X, this.Position.Size.Y).
					Translate(this.Position.Center).
					Build()
			);
		}
	}
}
