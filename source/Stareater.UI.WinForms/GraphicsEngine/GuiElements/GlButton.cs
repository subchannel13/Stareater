using Stareater.GLData;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GlButton : IGuiElement
	{
		private AScene scene;
		private SceneObject graphicObject = null;

		void IGuiElement.Attach(AScene scene, float z)
		{
			this.scene = scene;

			scene.UpdateScene(
				ref this.graphicObject,
				new SceneObjectBuilder().
					StartSimpleSprite(z, GalaxyTextures.Get.BombButton, Color.White). //TODO(v0.7) make separate drawing mechanism
					Scale(0.1f). //TODO(v0.7) calculate from position and UI scaling factor
					Build()
			);
		}
	}
}
