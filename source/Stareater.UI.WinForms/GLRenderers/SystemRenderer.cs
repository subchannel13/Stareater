using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.Controllers;
using Stareater.Galaxy;

namespace Stareater.GLRenderers
{
	public class SystemRenderer : IRenderer
	{
		private const double DefaultViewSize = 1;
		private const float BodiesY = 0.2f;
		private const float OrbitStep = 0.3f;
		private const float OrbitOffset = 0.5f;
		private const float OrbitWidth = 0.005f;
		
		private const float FarZ = -1;
		private const float StarColorZ = -0.8f;
		private const float PlanetZ = -0.8f;
		private const float OrbitZ = -0.9f;
		
		private const float StarScale = 0.5f;
		private const float PlanetScale = 0.15f;
		
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
			
			foreach(Planet planet in controller.Planets)
			{ 
				float orbitR = planet.Position * OrbitStep + OrbitOffset;
				float orbitMin = orbitR - OrbitWidth;
				float orbitMax = orbitR + OrbitWidth;
				
				GL.Disable(EnableCap.Texture2D);
				GL.Color4(Color.Gray);
				GL.Begin(BeginMode.Quads);
				for(int i = 0; i < 100; i++)
				{
					float angle0 = (float)Math.PI * i / 50f;
					float angle1 = (float)Math.PI * (i +1) / 50f;
					
					GL.Vertex3(orbitMin * (float)Math.Cos(angle0), orbitMin * (float)Math.Sin(angle0) + BodiesY, OrbitZ);
					GL.Vertex3(orbitMax * (float)Math.Cos(angle0), orbitMax * (float)Math.Sin(angle0) + BodiesY, OrbitZ);
					GL.Vertex3(orbitMax * (float)Math.Cos(angle1), orbitMax * (float)Math.Sin(angle1) + BodiesY, OrbitZ);
					GL.Vertex3(orbitMin * (float)Math.Cos(angle1), orbitMin * (float)Math.Sin(angle1) + BodiesY, OrbitZ);
				}
				GL.End();
				
				GL.Color4(Color.Blue);
				GL.Enable(EnableCap.Texture2D);
				GL.PushMatrix();
				GL.Translate(orbitR, BodiesY, StarColorZ);
				GL.Scale(PlanetScale, PlanetScale, PlanetScale);
	
				TextureUtils.Get.DrawSprite(GalaxyTextures.Get.Planet);
			
				GL.PopMatrix();
			}
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
