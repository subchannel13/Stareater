using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using Stareater.Controllers;
using Stareater.Controllers.Views.Combat;
using Stareater.Galaxy;
using Stareater.GLData;
using Stareater.GLData.OrbitShader;
using Stareater.GLData.SpriteShader;
using Stareater.GraphicsEngine;

namespace Stareater.GLRenderers
{
	class BombardmentRenderer : AScene
	{
		private const float DefaultViewSize = 1;
		private const float PanClickTolerance = 0.01f;
		
		private const float FarZ = 1;
		private const float Layers = 8.0f;
		
		private const float StarColorZ = 1 / Layers;
		private const float PlanetZ = 1 / Layers;
		private const float OrbitZ = 2 / Layers;
		
		private const float BodiesY = 0.2f;
		private const float OrbitStep = 0.3f;
		private const float OrbitOffset = 0.5f;
		private const float OrbitWidth = 0.01f;
		
		private const float OrbitPieces = 32;
		private const float StarScale = 0.5f;
		private const float PlanetScale = 0.15f;
		private const float StarSelectorScale = 1.1f;
		private const float PlanetSelectorScale = 1.1f;
		
		private IEnumerable<SceneObject> planetOrbits = null;
		private IEnumerable<SceneObject> planetSprites = null;
		private SceneObject starSprite = null;
		
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
			
			this.ResetLists();
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
		
		//TODO(0.6) refactor and remove
		public void ResetLists()
		{
			this.setupVaos();
		}
		
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
		
		private PolygonData planetSpriteData(CombatPlanetInfo planet)
		{
			var sprite = new TextureInfo();

			switch(planet.Type)
			{
				case PlanetType.Asteriod:
					sprite = GalaxyTextures.Get.Asteroids;
					break;
				case PlanetType.GasGiant:
					sprite = GalaxyTextures.Get.GasGiant;
					break;
				case PlanetType.Rock:
					sprite = GalaxyTextures.Get.RockPlanet;
					break;
			}
			
			return new PolygonData(
				PlanetZ,
				new SpriteData(planetTransform(planet.OrdinalPosition), sprite.Id, Color.White),
				SpriteHelpers.UnitRectVertexData(sprite)
			);
		}
		
		private Matrix4 planetTransform(int position)
		{
			return Matrix4.CreateScale(PlanetScale) * Matrix4.CreateTranslation(position * OrbitStep + OrbitOffset, 0, 0);
		}
		
		private void setupVaos()
		{
			if (this.controller == null)
				return; //FIXME(v0.6) move check to better place
			
			this.setupBodies();
		}
		
		private void setupBodies()
		{
			var starTransform = Matrix4.CreateScale(StarScale);
			
			this.UpdateScene(
				ref this.starSprite,
				new SceneObject(new PolygonData(
					StarColorZ,
					new SpriteData(starTransform, GalaxyTextures.Get.SystemStar.Id, controller.Star.Color),
					SpriteHelpers.UnitRectVertexData(GalaxyTextures.Get.SystemStar)
				))
			);
			
			this.UpdateScene(
				ref this.planetSprites,
				this.controller.Planets.Select(planet => new SceneObject(planetSpriteData(planet))).ToList()
			);
			
			this.UpdateScene(
				ref this.planetOrbits,
				this.controller.Planets.Select(
					planet => 
					{
						var orbitR = planet.OrdinalPosition * OrbitStep + OrbitOffset;
						var color = planet.Owner != null ? planet.Owner.Color : Color.FromArgb(64, 64, 64);
						
						return new SceneObject(new PolygonData(
							OrbitZ,
							new OrbitData(orbitR - OrbitWidth / 2, orbitR + OrbitWidth / 2, color, Matrix4.Identity, GalaxyTextures.Get.PathLine),
							OrbitHelpers.PlanetOrbit(orbitR, OrbitWidth, OrbitPieces).ToList()
						));
					}
				).ToList()
			);
		}
	}
}
