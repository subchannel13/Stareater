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
using Stareater.GraphicsEngine;
using Stareater.Utils.NumberFormatters;
using Stareater.Localization;
using Stareater.GraphicsEngine.GuiElements;
using Stareater.GameScenes.Widgets;

namespace Stareater.GameScenes
{
	class StarSystemScene : AScene
	{
		private const float DefaultViewSize = 1;
		
		private const float FarZ = 1;
		private const float Layers = 4.0f;
		
		private const float OrbitZ = 3 / Layers;
		
		private const float PanClickTolerance = 0.01f;
		
		private const float BodiesY = 0.2f;
		private const float OrbitStep = 0.2f;
		private const float OrbitOffset = 0.3f;
		private const float OrbitWidth = 0.01f;
		private const float OrbitPieces = 32;

		private const char ReturnToGalaxyKey = (char)27; //TODO(later): Make rebindable

		private StarSystemController controller;
		private PlayerController currentPlayer;
		private readonly ConstructionSiteView siteView;
		private readonly EmptyPlanetView emptyPlanetView;
		private readonly Action systemClosedHandler;

		private readonly SelectableImage<int> starSelector;
		private readonly Dictionary<int, AGuiElement> planetSelectors = new Dictionary<int, AGuiElement>();
		private readonly Dictionary<int, AGuiElement> colonizationMarkers = new Dictionary<int, AGuiElement>();
		private readonly List<GuiAnchor> planetAnchors = new List<GuiAnchor>();
		private readonly List<AGuiElement> otherPlanetElements = new List<AGuiElement>();

		private IEnumerable<SceneObject> planetOrbits = null;
		
		private Vector4? lastMousePosition = null;
		private float panAbsPath = 0;
		private float originOffset;
		private float minOffset;
		private float maxOffset;

		public StarSystemScene(Action systemClosedHandler)
		{
			this.systemClosedHandler = systemClosedHandler; 
			
			this.siteView = new ConstructionSiteView();
			this.siteView.Position.ParentRelative(0, -1);

			this.emptyPlanetView = new EmptyPlanetView(this.setupColonizationMarkers);
			this.emptyPlanetView.Position.ParentRelative(0, -1);

			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];
			var returnButton = new GuiButton
			{
				ClickCallback = systemClosedHandler,
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ButtonHover, 9),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ButtonNormal, 9),
				Padding = 12,
				Margins = new Vector2(10, 5),
				Text = context["Return"].Text(),
				TextColor = Color.Black,
				TextHeight = 20
			};
			returnButton.Position.WrapContent().Then.ParentRelative(1, 1).UseMargins();
			this.AddElement(returnButton);

			var starAnchor = new GuiAnchor(0, 0);
			this.AddAnchor(starAnchor);

			this.starSelector = new SelectableImage<int>(StarSystemController.StarIndex)
			{
				ForgroundImage = GalaxyTextures.Get.SystemStar,
				SelectorImage = GalaxyTextures.Get.SelectedStar,
				SelectCallback = select,
				Padding = 24,
			};
			starSelector.Position.FixedSize(400, 400).RelativeTo(starAnchor);
			this.AddElement(starSelector);
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

			this.maxOffset = (controller.Planets.Count() + 1) * OrbitStep + OrbitOffset;
			
			var bestColony = controller.Planets.
				Select(x => controller.PlanetsColony(x)).
				Aggregate(
					(ColonyInfo)null, 
					(prev, next) => next == null || (prev != null && prev.Population >= next.Population) ? prev : next
				);
			this.originOffset = bestColony != null ? bestColony.Location.Position * OrbitStep + OrbitOffset : 0.5f;
			this.lastMousePosition = null;

			this.starSelector.ForgroundImageColor = controller.HostStar.Color;
			this.starSelector.Select();

			foreach (var anchor in this.planetAnchors)
				this.RemoveAnchor(anchor);
			this.planetAnchors.Clear();

			foreach (var element in this.planetSelectors.Values.Concat(this.colonizationMarkers.Values).Concat(this.otherPlanetElements))
				this.RemoveElement(element);
			this.planetSelectors.Clear();
			this.colonizationMarkers.Clear();
			this.otherPlanetElements.Clear();

			var traitGridBuilder = new GridPositionBuilder(2, 20, 20, 3);
			foreach (var trait in controller.HostStar.Traits)
			{
				var traitImage = new GuiImage
				{
					Below = this.starSelector,
					Margins = new Vector2(3, 0),
					Image = GalaxyTextures.Get.Sprite(trait.ImagePath),
					Tooltip = new SimpleTooltip("Traits", trait.LangCode)
				};
				traitImage.Position.FixedSize(20, 20).RelativeTo(this.starSelector, 0.8f, -0.8f, -1, 1).UseMargins();
				traitGridBuilder.Add(traitImage.Position);
				this.addPlanetElement(traitImage);
			}

			foreach (var planet in this.controller.Planets)
			{
				var anchor = new GuiAnchor(planet.Position * OrbitStep + OrbitOffset, 0);
				this.AddAnchor(anchor);
				this.planetAnchors.Add(anchor);

				var planetSelector = new SelectableImage<int>(planet.Position)
				{
					ForgroundImage = GalaxyTextures.Get.PlanetSprite(planet.Type),
					SelectorImage = GalaxyTextures.Get.SelectedStar,
					SelectCallback = select,
					Padding = 16,
				};
				planetSelector.Position.FixedSize(100, 100).RelativeTo(anchor);
				planetSelector.GroupWith(starSelector);
				this.planetSelectors[planet.Position] = planetSelector;
				this.AddElement(planetSelector);

				var popInfo = new GuiText 
				{
					Margins = new Vector2(0, 20),
					TextHeight = 20 
				};
				popInfo.Position.WrapContent().Then.RelativeTo(planetSelector, 0, -1, 0, 1).UseMargins();

				var formatter = new ThousandsFormatter();
				var colony = this.controller.PlanetsColony(planet);
				if (colony != null)
				{
					popInfo.Text = formatter.Format(colony.Population) + " / " + formatter.Format(colony.PopulationMax);
					popInfo.TextColor = colony.Owner.Color;
				}
				else
				{
					popInfo.Text = formatter.Format(planet.PopulationMax);
					popInfo.TextColor = Color.Gray;
				}
				this.addPlanetElement(popInfo);

				traitGridBuilder = new GridPositionBuilder(4, 20, 20, 3);
				foreach (var trait in planet.Traits)
				{
					var traitImage = new GuiImage
					{
						Margins = new Vector2(0, 10),
						Image = GalaxyTextures.Get.Sprite(trait.ImagePath),
						Tooltip = new SimpleTooltip("Traits", trait.LangCode)
					};
					traitImage.Position.FixedSize(20, 20).RelativeTo(popInfo, 0, -1, 0, 1).UseMargins().Offset(-40, 0);
					traitGridBuilder.Add(traitImage.Position);
					this.addPlanetElement(traitImage);
				}
			}

			this.setupColonizationMarkers();
		}

		private void addPlanetElement(AGuiElement element)
		{
			this.AddElement(element);
			this.otherPlanetElements.Add(element);
		}

		#region AScene implementation
		protected override float guiLayerThickness => 1 / Layers;

		//TODO(v0.8) refactor and remove
		public void ResetLists()
		{
			this.setupVaos();
		}

		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			this.minOffset = aspect * DefaultViewSize / 2 - OrbitStep - OrbitOffset;
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
			switch(controller.BodyType(bodyIndex))
			{
				case BodyType.OwnStellaris:
					this.siteView.SetView(this.controller.StellarisController());
					this.setView(siteView);
					break;
				case BodyType.OwnColony:
					this.siteView.SetView(this.controller.ColonyController(bodyIndex));
					this.setView(siteView);
					break;
				case BodyType.NotColonised:
					this.emptyPlanetView.SetView(controller.EmptyPlanetController(bodyIndex), currentPlayer);
					this.setView(this.emptyPlanetView);
					break;
				default:
					this.setView(null);
					//TODO(later) add implementation, foregin planet, empty system, foreign system
					break;
			}
		}

		private void limitPan()
		{
			if (originOffset > maxOffset) 
				originOffset = maxOffset;
			if (originOffset < minOffset) 
				originOffset = minOffset;
		}
		
		private void setView(GuiPanel view)
		{
			foreach (var planetView in new GuiPanel[] { this.siteView, this.emptyPlanetView })
				if (planetView.Equals(view))
					this.ShowElement(planetView);
				else
					this.HideElement(planetView);
		}

		private void setupVaos()
		{
			if (this.controller == null)
				return; //TODO(v0.7) move check to better place
			
			this.setupBodies();
		}

		private void setupBodies()
		{
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
			foreach (var marker in this.colonizationMarkers.Values)
				this.RemoveElement(marker);
			this.colonizationMarkers.Clear();

			foreach(var planet in this.controller.Planets.Where(x => this.controller.IsColonizing(x.Position)))
			{
				if (!this.planetSelectors.ContainsKey(planet.Position))
					continue;

				var marker = new GuiImage
				{
					Images = new[] {
						new Sprite(GalaxyTextures.Get.ColonizationMark, Color.White),
						new Sprite(GalaxyTextures.Get.ColonizationMarkColor, this.currentPlayer.Info.Color)
					}
				};
				marker.Position.FixedSize(40, 40).RelativeTo(this.planetSelectors[planet.Position], 0.8f, 0.8f, -1, -1);
				this.colonizationMarkers[planet.Position] = marker;
				this.AddElement(marker);
			}
		}
	}
}
