using System;
using System.Windows.Forms;
using OpenTK;
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
			
			resetProjection = true;
		}
		
		public void DetachFromCanvas()
		{
			if (eventDispatcher == null)
				return;
			
			this.eventDispatcher = null;
		}
		
		//TODO(v0.6) repurpose to scene activation
		public virtual void Load()
		{ }
		
		public virtual void Unload()
		{ }
		#endregion
		
		#region Events
		public void ResetProjection(Vector2d screenSize)
		{
			this.resetProjection = true;
			this.screenSize = screenSize;
		}
		
		public virtual void OnNewTurn()
		{ }
		
		public abstract void ResetLists();
		#endregion
		
		protected Control eventDispatcher { get; private set; }
		protected Vector2d screenSize  { get; private set; }
		
		protected abstract void setupPerspective();

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

		#region Input handling
		public virtual void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
		{ }
		
		public virtual void OnMouseDoubleClick(MouseEventArgs e)
		{ }
		
		public virtual void OnMouseClick(MouseEventArgs e)
		{ }
		
		public virtual void OnMouseMove(MouseEventArgs e)
		{ }

		public virtual void OnMouseScroll(MouseEventArgs e)
		{ }
		#endregion
		
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
