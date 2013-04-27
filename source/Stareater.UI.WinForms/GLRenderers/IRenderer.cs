using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.GLRenderers
{
	interface IRenderer : IDisposable
	{
		void Draw(double deltaTime);
	}
}
