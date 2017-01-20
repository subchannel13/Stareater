using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK;

namespace Stareater.GraphicsEngine
{
	abstract class AScene
	{
		protected const int NoCallList = -1;
		
		private List<SceneObject> Children = new List<SceneObject>();
		
		public void Draw(double deltaTime)
		{
			this.FrameUpdate(deltaTime);
		}
		
		protected abstract void FrameUpdate(double deltaTime);
		
		#region Initialization/deinitialization
		public virtual void Activate()
		{ }
		
		public virtual void Deactivate()
		{ }
		#endregion
		
		#region Scene objects
		protected void Add(SceneObject sceneObject)
		{
			this.Children.Add(sceneObject);
		}
		
		protected void ClearScene()
		{
			this.Children.Clear();
		}
		#endregion
		
		#region Events
		public void ResetProjection(float screenWidth, float screenHeigth, float canvasWidth, float canvasHeigth)
		{
			this.canvasSize = new Vector2(canvasWidth, canvasHeigth);
			this.screenSize = new Vector2(screenWidth, screenHeigth);
			this.setupPerspective();
		}
		#endregion
		
		#region Perspective and viewport
		protected Vector2 canvasSize { get; private set; }
		protected Vector2 screenSize { get; private set; }
		protected Matrix4 invProjection { get; private set; }
		protected Matrix4 projection { get; private set; }
		
		protected abstract Matrix4 calculatePerspective();
		
		protected static Matrix4 calcOrthogonalPerspective(float width, float height, float farZ, Vector2 originOffset)
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
		
		protected Vector4 mouseToView(int x, int y)
		{
			return new Vector4(
				2 * x / canvasSize.X - 1,
				1 - 2 * y / canvasSize.Y, 
				0, 1
			);
		}
		
		protected void setupPerspective()
		{
			this.projection = this.calculatePerspective();
			this.invProjection = Matrix4.Invert(new Matrix4(this.projection.Row0, this.projection.Row1, this.projection.Row2, this.projection.Row3));
		}
		#endregion

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
	}
}
