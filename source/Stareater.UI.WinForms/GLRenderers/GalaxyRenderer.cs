using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Stareater.Controllers;
using System.Windows.Forms;

namespace Stareater.GLRenderers
{
	class GalaxyRenderer : IRenderer
	{
		private const float StarPlaneZ = -0.5f;

		private GameController controller;
		private Control eventDispatcher;

		private Vector4? lastMousePosition = null;
		private Vector3 originOffset = Vector3.Zero;

		public GalaxyRenderer(GameController controller, Control eventDispatcher)
		{
			this.controller = controller;
			this.eventDispatcher = eventDispatcher;

			eventDispatcher.MouseMove += mousePan;
		}

		public void Draw(double deltaTime)
		{
			GL.PushMatrix();
			GL.Translate(originOffset);

			foreach (var star in controller.Stars) {
				GL.Color4(star.Color);
				GL.PushMatrix();
				GL.Translate(star.Position.X, star.Position.Y, StarPlaneZ);
				GL.Begin(BeginMode.Quads);

				GL.Vertex2(-0.5, -0.5);
				GL.Vertex2(0.5, -0.5);
				GL.Vertex2(0.5, 0.5);
				GL.Vertex2(-0.5, 0.5);
				
				GL.End();
				GL.PopMatrix();
			}
			GL.PopMatrix();
		}

		public void mousePan(object sender, MouseEventArgs e)
		{
			float mouseX = 2 * e.X / (float)eventDispatcher.Width - 1;
			float mouseY = 1 - 2 * e.Y / (float)eventDispatcher.Height;

			if (!lastMousePosition.HasValue)
				lastMousePosition = new Vector4(mouseX, mouseY, 0, 1);

			if (!e.Button.HasFlag(MouseButtons.Left)) {
				lastMousePosition = new Vector4(mouseX, mouseY, 0, 1);
				return;
			}

			Matrix4 invProjection;

			GL.MatrixMode(MatrixMode.Projection);
			GL.GetFloat(GetPName.ProjectionMatrix, out invProjection);
			invProjection.Invert();
			GL.MatrixMode(MatrixMode.Modelview);

			originOffset += (Vector4.Transform(new Vector4(mouseX, mouseY, 0, 1), invProjection) -
				Vector4.Transform(lastMousePosition.Value, invProjection)
				).Xyz;

			lastMousePosition = new Vector4(mouseX, mouseY, 0, 1);
			eventDispatcher.Refresh();
		}

		public void Dispose()
		{
			eventDispatcher.MouseMove -= mousePan;
		}
	}
}
