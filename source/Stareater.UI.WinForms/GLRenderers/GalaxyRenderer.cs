using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Stareater.Controllers;

namespace Stareater.GLRenderers
{
	class GalaxyRenderer : IRenderer
	{
		private GameController controller;

		public GalaxyRenderer(GameController controller)
		{
			this.controller = controller;
		}

		public void Draw(double deltaTime)
		{
			foreach (var star in controller.Stars) {
				GL.Color4(star.Color);
				GL.PushMatrix();
				GL.Translate(star.Position.X, star.Position.Y, -0.5);
				GL.Begin(BeginMode.Quads);

				GL.Vertex2(-0.5, -0.5);
				GL.Vertex2(0.5, -0.5);
				GL.Vertex2(0.5, 0.5);
				GL.Vertex2(-0.5, 0.5);
				
				GL.End();
				GL.PopMatrix();
			}
			
		}

		public void Dispose()
		{ }
	}
}
