using OpenTK;

namespace Stareater.GraphicsEngine.GuiPositioners
{
	interface IWrapPositioner : IPositioner
	{
		void Padding(ValueReference<Vector2> padding);
	}
}
