using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GUI;

namespace Stareater.GLRenderers
{
	class SystemRenderer : AScene
	{
		private const float DefaultViewSize = 1;
		
		private const float FarZ = 1;
		private const float Layers = 16.0f;
		
		private const float SelectionZ = 5 / Layers;
		private const float MarkColorZ = 6 / Layers;
		private const float MarkZ = 7 / Layers;
		private const float StarColorZ = 8 / Layers;
		private const float PlanetZ = 8 / Layers;
		private const float OrbitZ = 9 / Layers;
		
		private const float PanClickTolerance = 0.01f;
		
		private const float BodiesY = 0.2f;
		private const float OrbitStep = 0.3f;
		private const float OrbitOffset = 0.5f;
		private const float OrbitWidth = 0.005f;
		
		private const float StarScale = 0.5f;
		private const float PlanetScale = 0.15f;
		private const float StarSelectorScale = 1.1f;
		private const float PlanetSelectorScale = 1.1f;
		
		private const char ReturnToGalaxyKey = (char)27; //TODO(later): Make rebindable

		private StarSystemController controller;
		private PlayerController currentPlayer;
		private readonly ConstructionSiteView siteView;
		private readonly EmpyPlanetView emptyPlanetView;
		private readonly Action systemClosedHandler;

		private Vector4? lastMousePosition = null;
		private float panAbsPath = 0;
		private float originOffset;
		private const float minOffset = -StarScale / 2;
		private float maxOffset;
		
		private int selectedBody;
		
		public SystemRenderer(Action systemClosedHandler, ConstructionSiteView siteView, EmpyPlanetView emptyPlanetView)
		{
			this.systemClosedHandler = systemClosedHandler; 
			this.emptyPlanetView = emptyPlanetView;
			this.siteView = siteView;
		}
		
		public void OnNewTurn()
		{
			this.ResetLists();
		}
		
		public void SetStarSystem(StarSystemController controller, PlayerController gameController)
		{
			this.controller = controller;
			this.currentPlayer = gameController;
			
			this.maxOffset = controller.Planets.Count() * OrbitStep + OrbitOffset + PlanetScale / 2;
			
			var bestColony = controller.Planets.
				Select(x => controller.PlanetsColony(x)).
				Aggregate(
					(ColonyInfo)null, 
					(prev, next) => next == null || (prev != null && prev.Population >= next.Population) ? prev : next
				);
			this.originOffset = bestColony != null ? bestColony.Location.Position * OrbitStep + OrbitOffset : 0.5f;
			
			this.select(StarSystemController.StarIndex);
		}
		
		#region ARenderer implementation
		public override void Draw(double deltaTime)
		{
			/*GL.PushMatrix();
			GL.Translate(0, BodiesY, 0);
			
			GL.Color4(controller.Star.Color);
			GL.PushMatrix();
			GL.Scale(StarScale, StarScale, StarScale);*/
			var transform = Matrix4.CreateScale(StarScale) * Matrix4.CreateTranslation(0, BodiesY, 0);

			//TODO(v0.6) convert to sprite info
			//TextureUtils.DrawSprite(GalaxyTextures.Get.SystemStar, StarColorZ);
			TextureUtils.DrawSprite(GalaxyTextures.Get.SystemStar, this.projection, transform, StarColorZ, controller.Star.Color);
			if (selectedBody == StarSystemController.StarIndex) {
				GL.Color4(Color.White);
				GL.Scale(StarSelectorScale, StarSelectorScale, StarSelectorScale);
				//TODO(v0.6) convert to sprite info
				//TextureUtils.DrawSprite(GalaxyTextures.Get.SelectedStar, SelectionZ);
			}
		
			//GL.PopMatrix();
			
			foreach(Planet planet in controller.Planets)
			{ 
				float orbitR = planet.Position * OrbitStep + OrbitOffset;
				float orbitMin = orbitR - OrbitWidth;
				float orbitMax = orbitR + OrbitWidth;
				
				GL.Disable(EnableCap.Texture2D);
				
				var colony = controller.PlanetsColony(planet);
				GL.Color4(colony != null ? colony.Owner.Color : Color.FromArgb(64, 64, 64));
				
				GL.Begin(BeginMode.Quads);
				for(int i = 0; i < 100; i++)
				{
					float angle0 = (float)Math.PI * i / 50f;
					float angle1 = (float)Math.PI * (i +1) / 50f;
					
					GL.Vertex3(orbitMin * (float)Math.Cos(angle0), orbitMin * (float)Math.Sin(angle0), OrbitZ);
					GL.Vertex3(orbitMax * (float)Math.Cos(angle0), orbitMax * (float)Math.Sin(angle0), OrbitZ);
					GL.Vertex3(orbitMax * (float)Math.Cos(angle1), orbitMax * (float)Math.Sin(angle1), OrbitZ);
					GL.Vertex3(orbitMin * (float)Math.Cos(angle1), orbitMin * (float)Math.Sin(angle1), OrbitZ);
				}
				GL.End();
				
				GL.Color4(Color.White);
				GL.Enable(EnableCap.Texture2D);
				
				GL.PushMatrix();
				GL.Translate(orbitR, 0, 0);
				GL.Scale(PlanetScale, PlanetScale, PlanetScale);
	
				switch(planet.Type)
				{
					case PlanetType.Asteriod:
						//TODO(v0.6) convert to sprite info
						//TextureUtils.DrawSprite(GalaxyTextures.Get.Asteroids, StarColorZ);
						break;
					case PlanetType.GasGiant:
						//TODO(v0.6) convert to sprite info
						//TextureUtils.DrawSprite(GalaxyTextures.Get.GasGiant, StarColorZ);
						break;
					case PlanetType.Rock:
						//TODO(v0.6) convert to sprite info
						//TextureUtils.DrawSprite(GalaxyTextures.Get.RockPlanet, StarColorZ);
						break;
				}
				
				if (this.controller.IsColonizing(planet.Position))
				{
					GL.PushMatrix();
					GL.Translate(0.6, 0.5, 0);
					GL.Scale(0.4, 0.4, 1);
					
					//TODO(v0.6) convert to sprite info
					//TextureUtils.DrawSprite(GalaxyTextures.Get.ColonizationMark, MarkZ);
					//TODO(v0.6) convert to sprite info
					//TextureUtils.DrawSprite(GalaxyTextures.Get.ColonizationMarkColor, MarkColorZ);
					GL.PopMatrix();
				}
				
				if (selectedBody == planet.Position){
					GL.Scale(PlanetSelectorScale, PlanetSelectorScale, PlanetSelectorScale);
					//TODO(v0.6) convert to sprite info
					//TextureUtils.DrawSprite(GalaxyTextures.Get.SelectedStar, SelectionZ);
				}
			
				GL.PopMatrix();
			}
			
			GL.PopMatrix();
		}

		public override void ResetLists()
		{
			//no op
			//TODO(later): make call list
		}

		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			return orthogonalPerspective(aspect * DefaultViewSize, DefaultViewSize, FarZ, new Vector2(originOffset, 0));
		}
		#endregion
		
		#region Input events
		public override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
		{
			switch (e.KeyChar) {
				case ReturnToGalaxyKey:
					this.systemClosedHandler();
					break;
				//TODO(later): add hotkeys for star and planets
			}
		}
		
		public override void OnMouseClick(MouseEventArgs e)
		{
			if (panAbsPath > PanClickTolerance)
				return;
			
			int? newSelection = null;
			float mouseX = Vector4.Transform(mouseToView(e.X, e.Y), invProjection).X;
			
			if (mouseX > -(OrbitOffset - OrbitStep / 2))
				newSelection = StarSystemController.StarIndex;
			
			foreach(var planet in controller.Planets)
				if (mouseX > planet.Position * OrbitStep + OrbitOffset - OrbitStep / 2)
					newSelection = planet.Position;
			
			if (newSelection.HasValue)
				select(newSelection.Value);
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

			limitPan();
			
			lastMousePosition = currentPosition;
			this.setupPerspective();
		}
		#endregion
		
		private void select(int bodyIndex)
		{
			this.selectedBody = bodyIndex;
			
			switch(controller.BodyType(bodyIndex))
			{
				case BodyType.OwnStellaris:
					siteView.SetView(controller.StellarisController());
					setView(siteView);
					break;
				case BodyType.OwnColony:
					siteView.SetView(controller.ColonyController(bodyIndex));
					setView(siteView);
					break;
				case BodyType.NotColonised:
					emptyPlanetView.SetView(controller.EmptyPlanetController(bodyIndex), currentPlayer);
					setView(emptyPlanetView);
					break;
				default:
					//TODO(later): add implementation, foregin planet, empty system, foreign system
					break;
			}
		}

		private Vector4 mouseToView(int x, int y)
		{
			return new Vector4(
				2 * x / (float)canvasSize.X - 1,
				1 - 2 * y / (float)canvasSize.Y, 
				0, 1
			);
		}
		
		private void limitPan()
		{
			if (originOffset < minOffset) 
				originOffset = minOffset;
			if (originOffset > maxOffset) 
				originOffset = maxOffset;
		}
		
		private void setView(object view)
		{
			if (emptyPlanetView.InvokeRequired)
			{
				emptyPlanetView.BeginInvoke(new Action<object>(setView), view);
				return;
			}
			
			emptyPlanetView.Visible = view.Equals(emptyPlanetView);
			siteView.Visible = view.Equals(siteView);
		}
		
	}
}
