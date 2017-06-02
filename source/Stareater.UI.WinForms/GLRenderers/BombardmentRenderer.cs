using System;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using Stareater.Controllers;
using Stareater.GraphicsEngine;

namespace Stareater.GLRenderers
{
	class BombardmentRenderer : AScene
	{
		private const float DefaultViewSize = 1;
		private const float PanClickTolerance = 0.01f;
		
		private const float FarZ = 1;
		
		private const float BodiesY = 0.2f;
		private const float OrbitStep = 0.3f;
		private const float OrbitOffset = 0.5f;
		private const float OrbitWidth = 0.01f;
		
		private const float OrbitPieces = 32;
		private const float StarScale = 0.5f;
		private const float PlanetScale = 0.15f;
		private const float StarSelectorScale = 1.1f;
		private const float PlanetSelectorScale = 1.1f;
		
		private BombardmentController controller;
		
		private Vector4? lastMousePosition = null;
		private float panAbsPath = 0;
		private float originOffset;
		private float minOffset;
		private float maxOffset;
		
		public void StartBombardment(BombardmentController controller)
		{
			this.controller = controller;
			
			this.maxOffset = controller.Planets.Count() * OrbitStep + OrbitOffset + PlanetScale / 2;
		}
		
		#region AScene implementation
		protected override void FrameUpdate(double deltaTime)
		{
			//no operation
		}
		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			this.minOffset = aspect * DefaultViewSize / 2 - StarScale / 2;
			this.limitPan();
			
			return calcOrthogonalPerspective(aspect * DefaultViewSize, DefaultViewSize, FarZ, new Vector2(originOffset, -BodiesY));
		}
		#endregion
		
		#region Input events
		public override void OnMouseClick(MouseEventArgs e)
		{
			if (panAbsPath > PanClickTolerance)
				return;
			
			int? newSelection = null;
			float mouseX = Vector4.Transform(mouseToView(e.X, e.Y), invProjection).X;
			
			if (mouseX > -(OrbitOffset - OrbitStep / 2))
				newSelection = StarSystemController.StarIndex;
			
			foreach(var planet in controller.Planets)
				if (mouseX > planet.OrdinalPosition * OrbitStep + OrbitOffset - OrbitStep / 2)
					newSelection = planet.OrdinalPosition;
			
		}
		
		public override void OnMouseMove(MouseEventArgs e)
		{
			Vector4 currentPosition = mouseToView(e.X, e.Y);

			if (!lastMousePosition.HasValue)
				lastMousePosition = currentPosition;

			if (!e.Button.HasFlag(MouseButtons.Left)) {
				lastMousePosition = currentPosition;
				panAbsPath = 0;
				return;
			}
			
			panAbsPath += (currentPosition - lastMousePosition.Value).Length;

			originOffset -= (Vector4.Transform(currentPosition, invProjection) -
				Vector4.Transform(lastMousePosition.Value, invProjection)
				).X;

			this.limitPan();
			
			lastMousePosition = currentPosition;
			this.setupPerspective();
		}
		#endregion
		
		private void limitPan()
		{
			if (originOffset > maxOffset) 
				originOffset = maxOffset;
			if (originOffset < minOffset) 
				originOffset = minOffset;
		}
	}
}
