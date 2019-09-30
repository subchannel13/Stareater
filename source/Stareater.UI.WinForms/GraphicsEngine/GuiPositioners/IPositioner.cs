namespace Stareater.GraphicsEngine.GuiPositioners
{
	interface IPositioner
	{
		void Recalculate(ElementPosition element, ElementPosition parentPosition);
	}
}
