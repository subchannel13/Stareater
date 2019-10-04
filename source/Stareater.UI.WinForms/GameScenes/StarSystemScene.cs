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
		private const float Layers = 8.0f;
		
		private const float MarkColorZ = 5 / Layers;
		private const float MarkZ = 6 / Layers;
		private const float OrbitZ = 7 / Layers;
		
		private const float PanClickTolerance = 0.01f;
		
		private const float BodiesY = 0.2f;
		private const float OrbitStep = 0.2f;
		private const float OrbitOffset = 0.3f;
		private const float OrbitWidth = 0.01f;
		
		private const float OrbitPieces = 32;
		private const float PlanetScale = 0.15f;

		private const char ReturnToGalaxyKey = (char)27; //TODO(later): Make rebindable

		private StarSystemController controller;
		private PlayerController currentPlayer;
		private readonly ConstructionSiteView siteView;
		private readonly EmpyPlanetView emptyPlanetView;
		private readonly Action systemClosedHandler;

		private readonly SelectableImage<int> starSelector;
		private readonly List<GuiAnchor> planetAnchors = new List<GuiAnchor>();
		private readonly List<AGuiElement> planetElements = new List<AGuiElement>();

		private IEnumerable<SceneObject> colonizationMarkers = null;
		private IEnumerable<SceneObject> planetOrbits = null;
		
		private Vector4? lastMousePosition = null;
		private float panAbsPath = 0;
		private float originOffset;
		private float minOffset;
		private float maxOffset;

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

			foreach (var element in this.planetElements)
				this.RemoveElement(element);
			this.planetElements.Clear();

			var traitGridBuilder = new GridPositionBuilder(2, 20, 20, 3);
			foreach (var trait in controller.HostStar.Traits)
			{
				var traitImage = new GuiImage
				{
					Image = GalaxyTextures.Get.Sprite(trait.ImagePath),
					Tooltip = new SimpleTooltip("Traits", trait.LangCode)
				};
				traitImage.Position.FixedSize(20, 20).RelativeTo(this.starSelector, 1, -1, -1, 1).WithMargins(3, 0);
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
				this.addPlanetElement(planetSelector);

				var popInfo = new GuiText { TextHeight = 20 };
				popInfo.Position.WrapContent().Then.RelativeTo(planetSelector, 0, -1, 0, 1).WithMargins(0, 20);

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
						Image = GalaxyTextures.Get.Sprite(trait.ImagePath),
						Tooltip = new SimpleTooltip("Traits", trait.LangCode)
					};
					traitImage.Position.FixedSize(20, 20).RelativeTo(popInfo, 0, -1, 0, 1).WithMargins(0, 10).Offset(-40, 0);
					traitGridBuilder.Add(traitImage.Position);
					this.addPlanetElement(traitImage);
				}
			}
		}

		private void addPlanetElement(AGuiElement element)
		{
			this.AddElement(element);
			this.planetElements.Add(element);
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
	}
}
