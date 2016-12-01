using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLRenderers
{
	abstract class AScene
	{
		protected const int NoCallList = -1;
		
		public abstract void Draw(double deltaTime);
		
		#region Initialization/deinitialization
		public virtual void Activate()
		{ }
		
		public virtual void Deactivate()
		{ }
		#endregion
		
		#region Events
		public void ResetProjection(float screenWidth, float screenHeigth, float canvasWidth, float canvasHeigth)
		{
			this.canvasSize = new Vector2(canvasWidth, canvasHeigth);
			this.screenSize = new Vector2(screenWidth, screenHeigth);
			this.setupPerspective();
		}
		
		public abstract void ResetLists();
		#endregion
		
		protected Vector2 canvasSize { get; private set; }
		protected Vector2 screenSize { get; private set; }
		protected Matrix4 invProjection { get; private set; }
		protected Matrix4 projection { get; private set; }
		
		protected abstract Matrix4 calculatePerspective();
		
		protected void setupPerspective()
		{
			this.projection = this.calculatePerspective();
			this.invProjection = Matrix4.Invert(new Matrix4(this.projection.Row0, this.projection.Row1, this.projection.Row2, this.projection.Row3));
		}
		
		//TODO(v0.6) remove
		protected static void drawList(int listId, Action<int> listGenerator)
		{
			if (listId == NoCallList)
			{
				listId = GL.GenLists(1);
				
				GL.NewList(listId, ListMode.Compile);
				listGenerator(listId);
				GL.EndList();
			}
			
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
		
		protected Vector4 mouseToView(int x, int y)
		{
			return new Vector4(
				2 * x / canvasSize.X - 1,
				1 - 2 * y / canvasSize.Y, 
				0, 1
			);
		}
		
		protected static Matrix4 orthogonalPerspective(float width, float height, float farZ, Vector2 originOffset)
		{
			var left = (float)(-width / 2 + originOffset.X);
			var right = (float)(width / 2 + originOffset.X);
			var bottom = (float)(-height / 2 + originOffset.Y);
			var top = (float)(height / 2 + originOffset.Y);
			
			return new Matrix4(
				2 / (right - left), 0, 0, 0,
				0, 2 / (top - bottom), 0, 0,
				0, 0, 2 / farZ, 0,
				-(right + left) / (right - left), -(top + bottom) / (top - bottom), -1, 1
			);
		}
	}
}
