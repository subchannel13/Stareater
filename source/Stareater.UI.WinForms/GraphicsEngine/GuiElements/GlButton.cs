using Stareater.GLData;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GlButton : IGuiElement
	{
		private AScene scene;
		private float z;
		private SceneObject graphicObject = null;

		public ElementPosition Position { get; private set; }

		public GlButton()
		{
			this.Position = new ElementPosition();
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
			scene.UpdateScene(
				ref this.graphicObject,
				new SceneObjectBuilder().
					StartSimpleSprite(this.z, GalaxyTextures.Get.BombButton, Color.White). //TODO(v0.7) make separate drawing mechanism
					Scale(this.Position.Size.X, this.Position.Size.Y).
					Translate(this.Position.Center).
					Build()
			);
		}
	}
}
