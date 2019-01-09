using Stareater.GLData;
using Stareater.GLData.SpriteShader;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiPanel : AGuiElement
	{
		private BackgroundTexture mBackground = null;
		public BackgroundTexture Background
		{
			get { return this.mBackground; }
			set
			{
				apply(ref this.mBackground, value);
			}
		}

		protected override SceneObject makeSceneObject()
		{
			return new SceneObjectBuilder().
				Clip(this.Position.ClipArea).
				StartSprite(this.Z0, this.mBackground.Sprite.Id, Color.White).
				Translate(this.Position.Center).
				AddVertices(SpriteHelpers.GuiBackground(this.mBackground, this.Position.Size.X, this.Position.Size.Y)).
				Build();
		}
	}
}
