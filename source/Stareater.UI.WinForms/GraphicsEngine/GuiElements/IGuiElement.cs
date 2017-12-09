namespace Stareater.GraphicsEngine.GuiElements
{
	interface IGuiElement
	{
		void Attach(AScene scene, float z);

		ElementPosition Position { get; }
		void RecalculatePosition(float parentWidth, float parentHeight);
	}
}
