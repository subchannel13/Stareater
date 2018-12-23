using Stareater.GLData;
using System.Collections.Generic;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiPanel : AGuiElement
	{
		private TextureInfo? mBackground = null;
		public TextureInfo? Background
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
				StartSimpleSprite(this.Z0, this.mBackground.Value, Color.White).
				Scale(this.Position.Size.X, this.Position.Size.Y).
				Translate(this.Position.Center).
				Build();
		}
	}
}
