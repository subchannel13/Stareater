﻿using System;
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
		public void ResetProjection(Vector2d screenSize, Vector2d canvasSize)
		{
			this.canvasSize = canvasSize;
			this.screenSize = screenSize;
			this.setupPerspective();
		}
		
		public abstract void ResetLists();
		#endregion
		
		protected Vector2d canvasSize { get; private set; }
		protected Vector2d screenSize { get; private set; }
		
		protected abstract void setupPerspective();
		
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
	}
}