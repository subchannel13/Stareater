using OpenTK;

namespace Stareater.GraphicsEngine.GuiElements
{
	//TODO(v0.7) consider converting to abstract class with scene attachment mechanism and overridable input handling
	interface IGuiElement
	{
		void Attach(AScene scene, float z);

		ElementPosition Position { get; }
		void RecalculatePosition(float parentWidth, float parentHeight);

		bool OnMouseClick(Vector2 mousePosition);
		void OnMouseMove(Vector2 mousePosition);
	}
}
