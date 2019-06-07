using Stareater.GraphicsEngine.GuiElements;
using System.Collections.Generic;

namespace Stareater.GraphicsEngine.GuiPositioners
{
	interface IPositioner
	{
		void Recalculate(ElementPosition element, ElementPosition parentPosition);
		IEnumerable<AGuiElement> Dependencies { get; }
	}
}
