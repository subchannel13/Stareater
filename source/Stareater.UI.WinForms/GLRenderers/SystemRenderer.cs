using System;
using System.Windows.Forms;
using Stareater.Controllers;

namespace Stareater.GLRenderers
{
	public class SystemRenderer : IRenderer
	{
		private GameController controller;
		private Control eventDispatcher;
		private Action systemClosedHandler;
		
		public SystemRenderer(GameController controller, Action systemClosedHandler)
		{
			this.controller = controller;
			this.systemClosedHandler = systemClosedHandler;
		}
		
		public void Draw(double deltaTime)
		{
			//no op
		}
		
		public void Load()
		{
			//no op
		}
		
		public void Unload()
		{
			//no op
		}
		
		public void AttachToCanvas(Control eventDispatcher)
		{
			this.eventDispatcher = eventDispatcher;
			
			eventDispatcher.MouseClick += mouseClick;
		}
		
		public void DetachFromCanvas()
		{
			eventDispatcher.MouseClick -= mouseClick;
			
			this.eventDispatcher = null;
		}
		
		public void ResetProjection()
		{
			throw new NotImplementedException();
		}
		
		private void mouseClick(object sender, MouseEventArgs e)
		{
			this.systemClosedHandler();
		}
		
		public void Dispose()
		{
			if (eventDispatcher != null) {
				DetachFromCanvas();
				Unload();
			}
		}
	}
}
