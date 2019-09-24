using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.GLData;
using Stareater.GLData.OrbitShader;
using Stareater.GLData.SpriteShader;
using Stareater.GUI;
using Stareater.GraphicsEngine;
using Stareater.Utils.NumberFormatters;
using Stareater.Localization;
using Stareater.GraphicsEngine.GuiElements;

namespace Stareater.GameScenes
{
	class StarSystemScene : AScene
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
		private const float OrbitStep = 0.2f;
		private const float OrbitOffset = 0.3f;
		private const float OrbitWidth = 0.01f;
		
		private const float OrbitPieces = 32;
		private const float StarScale = 0.5f;
		private const float PlanetScale = 0.15f;
		private const float StarSelectorScale = 1.1f;
		private const float PlanetSelectorScale = 1.1f;
		private const float TraitScale = 0.04f;
		private const float TraitSpacing = 0.01f;

		private const char ReturnToGalaxyKey = (char)27; //TODO(later): Make rebindable

		private StarSystemController controller;
		private PlayerController currentPlayer;
		private readonly ConstructionSiteView siteView;
		private readonly EmpyPlanetView emptyPlanetView;
		private readonly Action systemClosedHandler;

		private IEnumerable<SceneObject> colonizationMarkers = null;
		private IEnumerable<SceneObject> planetOrbits = null;
		private IEnumerable<SceneObject> planetSprites = null;
		private SceneObject selectionMarker = null;
		private SceneObject starSprite = null;
		
		private Vector4? lastMousePosition = null;
		private float panAbsPath = 0;
		private float originOffset;
		private float minOffset;
		private float maxOffset;
		private float pixelSize;

		private int selectedBody;
		private HashSet<PlanetInfo> colonizationMarked = new HashSet<PlanetInfo>();
		
		public StarSystemScene(Action systemClosedHandler, EmpyPlanetView emptyPlanetView)
		{
			this.systemClosedHandler = systemClosedHandler; 
			this.emptyPlanetView = emptyPlanetView;

			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];
			var returnButton = new GuiButton
			{
				ClickCallback = systemClosedHandler,
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ButtonHover, 9),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ButtonNormal, 9),
				Padding = 12,
				Text = context["Return"].Text(),
				TextColor = Color.Black,
				TextHeight = 20
			};
			returnButton.Position.WrapContent().Then.ParentRelative(1, 1).WithMargins(10, 5);
			this.AddElement(returnButton);

			this.siteView = new ConstructionSiteView();
			this.siteView.Position.ParentRelative(0, -1);
			this.AddElement(this.siteView);
		}
		
		public override void Activate()
		{
			this.setupVaos();
		}
		
		public void OnNewTurn()
		{
			this.ResetLists();
		}
		
		public void SetStarSystem(StarSystemController controller, PlayerController playerController)
		{
			this.controller = controller;
			this.currentPlayer = playerController;
			
			this.maxOffset = controller.Planets.Count() * OrbitStep + OrbitOffset + PlanetScale / 2;
			
			var bestColony = controller.Planets.
				Select(x => controller.PlanetsColony(x)).
				Aggregate(
					(ColonyInfo)null, 
					(prev, next) => next == null || (prev != null && prev.Population >= next.Population) ? prev : next
				);
			this.originOffset = bestColony != null ? bestColony.Location.Position * OrbitStep + OrbitOffset : 0.5f;
			this.lastMousePosition = null;
			
			this.select(StarSystemController.StarIndex);
		}

		#region AScene implementation
		protected override float guiLayerThickness => 1 / Layers;

		protected override void frameUpdate(double deltaTime)
		{
			var beingColonized = new HashSet<PlanetInfo>(this.controller.Planets.Where(x => this.controller.IsColonizing(x.Position)));
			if (!this.colonizationMarked.SetEquals(beingColonized))
				this.setupColonizationMarkers();
			this.colonizationMarked = beingColonized;
		}

		//TODO(v0.8) refactor and remove
		public void ResetLists()
		{
			this.setupVaos();
		}

		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			this.minOffset = aspect * DefaultViewSize / 2 - StarScale / 2;
			this.pixelSize = DefaultViewSize / canvasSize.Y;
			this.limitPan();
			
			return calcOrthogonalPerspective(aspect * DefaultViewSize, DefaultViewSize, FarZ, new Vector2(originOffset, -BodiesY));
		}

		protected override void onResize()
		{
			this.setupVaos();
		}
		#endregion

		#region Input events
		protected override void onKeyPress(char c)
		{
			switch (c) {
				case ReturnToGalaxyKey:
					this.systemClosedHandler();
					break;
				//TODO(later) add hotkeys for star and planets
			}
		}

		protected override void onMouseClick(Vector2 mousePoint, Keys modiferKeys)
		{
			if (this.panAbsPath > PanClickTolerance)
				return;
			
			int? newSelection = null;
			
			if (mousePoint.X > -(OrbitOffset - OrbitStep / 2))
				newSelection = StarSystemController.StarIndex;
			
			foreach(var planet in controller.Planets)
				if (mousePoint.X > planet.Position * OrbitStep + OrbitOffset - OrbitStep / 2)
					newSelection = planet.Position;
			
			if (newSelection.HasValue)
				this.select(newSelection.Value);
		}

		protected override void onMouseMove(Vector4 mouseViewPosition, Keys modiferKeys)
		{
			this.lastMousePosition = mouseViewPosition;
			this.panAbsPath = 0;
		}

		protected override void onMouseDrag(Vector4 mouseViewPosition)
		{
			if (!lastMousePosition.HasValue)
				this.lastMousePosition = mouseViewPosition;

			this.panAbsPath += (mouseViewPosition - this.lastMousePosition.Value).Length;

			this.originOffset -= (Vector4.Transform(mouseViewPosition, this.invProjection) -
				Vector4.Transform(this.lastMousePosition.Value, this.invProjection)
				).X;

			this.limitPan();

			this.lastMousePosition = mouseViewPosition;
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
					//TODO(later) add implementation, foregin planet, empty system, foreign system
					break;
			}
			
			this.setupSelectionMarker();
		}

		private void limitPan()
		{
			if (originOffset > maxOffset) 
				originOffset = maxOffset;
			if (originOffset < minOffset) 
				originOffset = minOffset;
		}
		
		private void setView(object view)
		{
			if (emptyPlanetView.InvokeRequired)
			{
				emptyPlanetView.BeginInvoke(new Action<object>(setView), view);
				return;
			}
			
			emptyPlanetView.Visible = view.Equals(emptyPlanetView);
			if (view.Equals(siteView))
				this.ShowElement(siteView);
			else
				this.HideElement(siteView);
		}

		private SceneObject planetDrawables(PlanetInfo planet)
		{
			var planetX = planet.Position * OrbitStep + OrbitOffset;
			var soBuilder = new SceneObjectBuilder().
						PixelSize(this.pixelSize).
						StartSimpleSprite(PlanetZ, GalaxyTextures.Get.PlanetSprite(planet.Type), Color.White).
						Scale(PlanetScale).
						Translate(planetX, 0);

			var formatter = new ThousandsFormatter();
			var colony = this.controller.PlanetsColony(planet);
			if (colony != null)
				soBuilder.StartText(
					formatter.Format(colony.Population) + " / " + formatter.Format(colony.PopulationMax),
					-0.5f, 0, PlanetZ, 1 / Layers, colony.Owner.Color
				);
			else
				soBuilder.StartText(
					formatter.Format(planet.PopulationMax),
					-0.5f, 0, PlanetZ, 1 / Layers, Color.Gray
				);

			soBuilder.Scale(this.pixelSize * 20).
				Translate(planetX, -PlanetScale * 0.7);

			int i = 0;
			var traitY0 = -PlanetScale * 0.7 - this.pixelSize * 25 - TraitSpacing - TraitScale / 2;
			foreach (var trait in planet.Traits)
			{
				soBuilder.StartSimpleSprite(PlanetZ, GalaxyTextures.Get.Sprite(trait.ImagePath), Color.White).
					Scale(TraitScale).
					Translate(planetX - PlanetScale / 2 + (i % 4) * (TraitScale + TraitSpacing), traitY0 - (i / 4) * (TraitScale + TraitSpacing));
				i++;
			}

			return soBuilder.Build();
		}

		private Matrix4 planetTransform(int position)
		{
			return Matrix4.CreateScale(PlanetScale) * Matrix4.CreateTranslation(position * OrbitStep + OrbitOffset, 0, 0);
		}
		
		private void setupVaos()
		{
			if (this.controller == null)
				return; //TODO(v0.7) move check to better place
			
			this.setupBodies();
			this.setupColonizationMarkers();
			this.setupSelectionMarker();
		}

		private void setupBodies()
		{
			this.UpdateScene(
				ref this.starSprite,
				new SceneObject(new PolygonData(
					StarColorZ,
					new SpriteData(Matrix4.CreateScale(StarScale), GalaxyTextures.Get.SystemStar.Id, controller.HostStar.Color, null),
					SpriteHelpers.UnitRect(GalaxyTextures.Get.SystemStar).ToList()
				))
			);

			this.UpdateScene(
				ref this.planetSprites,
				this.controller.Planets.Select(planet => planetDrawables(planet)).ToList()
			);
			

			this.UpdateScene(
				ref this.planetOrbits,
				this.controller.Planets.Select(
					planet => 
					{
						var orbitR = planet.Position * OrbitStep + OrbitOffset;
						var colony = controller.PlanetsColony(planet);
						var color = colony != null ? Color.FromArgb(192, colony.Owner.Color) : Color.FromArgb(64, 64, 64);
						
						return new SceneObject(new PolygonData(
							OrbitZ,
							new OrbitData(orbitR - OrbitWidth / 2, orbitR + OrbitWidth / 2, color, Matrix4.Identity, GalaxyTextures.Get.PathLine),
							OrbitHelpers.PlanetOrbit(orbitR, OrbitWidth, OrbitPieces).ToList()
						));
					}
				).ToList()
			);
		}
		
		private void setupColonizationMarkers()
		{
			this.UpdateScene(
				ref this.colonizationMarkers,
				this.controller.Planets.Where(x => this.controller.IsColonizing(x.Position)).Select(
					planet => 
					{
						var markTransform = Matrix4.CreateScale(0.4f, 0.4f, 1) * 
							Matrix4.CreateTranslation(0.6f, 0.5f, 0) * 
							planetTransform(planet.Position);
						
						return new SceneObject(
							new [] {
								new PolygonData(
									MarkZ,
									new SpriteData(markTransform, GalaxyTextures.Get.ColonizationMark.Id, Color.White, null),
									SpriteHelpers.UnitRect(GalaxyTextures.Get.ColonizationMark).ToList()
								),
								new PolygonData(
									MarkColorZ,
									new SpriteData(markTransform, GalaxyTextures.Get.ColonizationMarkColor.Id, this.currentPlayer.Info.Color, null),
									SpriteHelpers.UnitRect(GalaxyTextures.Get.ColonizationMarkColor).ToList()
								)
							});
					}).ToList()
			);
		}
		
		private void setupSelectionMarker()
		{
			var transform = (selectedBody == StarSystemController.StarIndex) ? 
				Matrix4.CreateScale(StarSelectorScale) * Matrix4.CreateScale(StarScale) :
				Matrix4.CreateScale(PlanetSelectorScale) * planetTransform(selectedBody);

			this.UpdateScene(
				ref this.selectionMarker,
				new SceneObject(new PolygonData(
					SelectionZ,
					new SpriteData(transform, GalaxyTextures.Get.SelectedStar.Id, Color.White, null),
					SpriteHelpers.UnitRect(GalaxyTextures.Get.SelectedStar).ToList()
				))
			);
		}
	}
}
