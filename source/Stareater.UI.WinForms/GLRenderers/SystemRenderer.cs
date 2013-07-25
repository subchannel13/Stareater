using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.Controllers;

namespace Stareater.GLRenderers
{
	public class SystemRenderer : IRenderer
	{
		private const double DefaultViewSize = 1;
		private const float BodiesY = 0.2f;
		
		private const float FarZ = -1;
		private const float StarColorZ = -0.9f;
		
		private const float StarScale = 0.5f;
		
		private StarSystemController controller;
		private Control eventDispatcher;
		private Action systemClosedHandler;
		
		private bool resetProjection = true;
		private Matrix4 invProjection;
		private Vector2 originOffset;
		
		public SystemRenderer(Action systemClosedHandler)
		{
			this.systemClosedHandler = systemClosedHandler;
		}
		
		public void Draw(double deltaTime)
		{
			if (resetProjection) {
				double aspect = eventDispatcher.Width / (double)eventDispatcher.Height;
				double semiRadius = 0.5 * DefaultViewSize;

				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();
				GL.Ortho(
					-aspect * semiRadius + originOffset.X, aspect * semiRadius + originOffset.X,
					-semiRadius, semiRadius, 
					0, -FarZ);

				GL.GetFloat(GetPName.ProjectionMatrix, out invProjection);
				invProjection.Invert();
				GL.MatrixMode(MatrixMode.Modelview);
				resetProjection = false;
			}
			
			GL.Color4(controller.Star.Color);
			GL.PushMatrix();
			GL.Translate(0, BodiesY, StarColorZ);
			GL.Scale(StarScale, StarScale, StarScale);

			TextureUtils.Get.DrawSprite(GalaxyTextures.Get.SystemStar);
		
			GL.PopMatrix();
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
		
		public void SetStarSystem(StarSystemController controller)
		{
			this.controller = controller;
			
			this.resetProjection = true;
			this.originOffset = new Vector2(0.5f, 0);
		}
		
		public void ResetProjection()
		{
			resetProjection = true;
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
