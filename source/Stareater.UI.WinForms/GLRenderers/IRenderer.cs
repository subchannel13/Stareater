using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stareater.GLRenderers
{
	interface IRenderer : IDisposable
	{
		void Draw(double deltaTime);

		void Load();
		void Unload();
		
		void AttachToCanvas(Control eventDispatcher);
		void DetachFromCanvas();
		void ResetProjection();
	}
}
