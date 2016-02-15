using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.Controllers.Views;

namespace Stareater.GLRenderers
{
	public class SpaceCombatRenderer : IRenderer
	{
		private const double DefaultViewSize = 1;
		
		private const float FarZ = -1;
		private const float Layers = 16.0f;
		
		private const float GridZ = -8 / Layers;
		
		private const int NoCallList = -1;
		
		private Control eventDispatcher;
		
		private bool resetProjection = true;
		private Matrix4 invProjection;
		private int gridList = NoCallList;
		
		public SpaceBattleController controller { get; private set; }
		
		#region IRenderer implementation
		public void Draw(double deltaTime)
		{
			if (resetProjection) {
				double aspect = eventDispatcher.Width / (double)eventDispatcher.Height;
				const double semiRadius = 0.5 * DefaultViewSize;

				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();
				GL.Ortho(
					-aspect * semiRadius, aspect * semiRadius,
					-semiRadius, semiRadius, 
					0, -FarZ);

				GL.GetFloat(GetPName.ProjectionMatrix, out invProjection);
				invProjection.Invert();
				GL.MatrixMode(MatrixMode.Modelview);
				resetProjection = false;
			}
			
			//TODO(v0.5)
		}
		
		public void Load()
		{
			//no op
		}
		
		public void Unload()
		{
			//no op
		}
		
		public void AttachToCanvas(System.Windows.Forms.Control eventDispatcher)
		{
			this.eventDispatcher = eventDispatcher;
		}
		
		public void DetachFromCanvas()
		{
			if (eventDispatcher == null)
				return;
			
			this.eventDispatcher = null;
		}
		
		public void ResetProjection()
		{
			resetProjection = true;
		}
		
		public void OnNewTurn()
		{
			this.ResetLists();
		}
		
		public void ResetLists()
		{
			GL.DeleteLists(gridList, 1);
			this.gridList = NoCallList;
		}
		#endregion
		
		#region IDisposable implementation
		public void Dispose()
		{
			if (eventDispatcher != null) {
				DetachFromCanvas();
				Unload();
			}
		}
		#endregion
	}
}
