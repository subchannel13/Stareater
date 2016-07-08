using System;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLRenderers
{
	abstract class ARenderer : IDisposable
	{
		protected const int NoCallList = -1;
		
		private bool resetProjection = true;
		
		protected ARenderer()
		{
			this.eventDispatcher = null;
		}
		
		public abstract void Draw(double deltaTime);
		
		#region Initialization/deinitialization
		public virtual void AttachToCanvas(Control eventDispatcher)
		{
			this.eventDispatcher = eventDispatcher;
			this.attachEventHandlers();
			
			resetProjection = true;
		}
		
		public void DetachFromCanvas()
		{
			if (eventDispatcher == null)
				return;
			
			this.detachEventHandlers();
			this.eventDispatcher = null;
		}
		
		public virtual void Load()
		{ }
		
		public virtual void Unload()
		{ }
		#endregion
		
		#region Events
		public void ResetProjection()
		{
			this.resetProjection = true;
		}
		
		public virtual void OnNewTurn()
		{ }
		
		public abstract void ResetLists();
		#endregion
		
		protected Control eventDispatcher { get; private set; }
		
		protected abstract void setupPerspective();

		protected abstract void attachEventHandlers();
		
		protected abstract void detachEventHandlers();
		
		protected void checkPerspective()
		{
			if (resetProjection) 
			{
				this.setupPerspective();
				this.resetProjection = false;
			}
		}
		
		protected void requestPerspectiveReset()
		{
			this.resetProjection = true;
		}
		
		protected static void drawList(int listId, Action listGenerator)
		{
			if (listId == NoCallList)
				listGenerator();
			else
				GL.CallList(listId);
		}		

		#region IDisposable
		public void Dispose()
		{
			if (eventDispatcher != null) {
				this.DetachFromCanvas();
				this.Unload();
			}
		}
		#endregion
	}
}
