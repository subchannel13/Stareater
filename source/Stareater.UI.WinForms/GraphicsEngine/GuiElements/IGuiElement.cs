using OpenTK;

namespace Stareater.GraphicsEngine.GuiElements
{
	interface IGuiElement
	{
		void Attach(AScene scene, float z);

		ElementPosition Position { get; }
		void RecalculatePosition(float parentWidth, float parentHeight);

		bool OnMouseClick(Vector2 mousePosition);
	}
}
