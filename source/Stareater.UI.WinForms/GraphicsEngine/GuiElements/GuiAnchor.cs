using OpenTK;
using Stareater.GraphicsEngine.GuiPositioners;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiAnchor : IGuispaceElement
	{
		private Vector4 scenePosition;

		public ElementPosition Position { get; private set; }

		public GuiAnchor(double x, double y)
		{
			this.scenePosition = new Vector4((float)x, (float)y, 0, 1);
			this.Position = new ElementPosition(() => new Vector2());
		}

		public void Setup(Matrix4 sceneProjection, Matrix4 guiInvProjection)
		{
			var guiPosition = Vector4.Transform(Vector4.Transform(this.scenePosition, sceneProjection), guiInvProjection);

			this.Position.FixedCenter(guiPosition.X, guiPosition.Y);
			this.Position.Propagate();
		}
	}
}
