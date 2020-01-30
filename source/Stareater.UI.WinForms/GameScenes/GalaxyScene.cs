using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using OpenTK;
using Stareater.Controllers;
using Stareater.Controllers.Views.Ships;
using Stareater.Localization;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;
using Stareater.GraphicsEngine;
using Stareater.GLData.SpriteShader;
using Stareater.GLData;
using Stareater.GraphicsEngine.GuiElements;
using Stareater.Controllers.Views;
using Stareater.GLData.OrbitShader;
using Stareater.AppData;
using System.Windows.Forms;
using Stareater.GUI;
using Stareater.GUI.Reports;
using Stareater.GameScenes.Widgets;

namespace Stareater.GameScenes
{
	class GalaxyScene : AScene
	{
		private const float DefaultViewSize = 15;
		private const double ZoomBase = 1.2f;
		private const int MaxZoom = 10;
		private const int MinZoom = -10;
		private const int NameZoomLimit = -2;

		private const float FarZ = 1;
		private const float Layers = 16.0f;
		private const float InterlayerZRange = 1 / Layers;

		private const float ScanRangeZ = 10 / Layers;
		private const float WormholeZ = 9 / Layers;
		private const float PathZ = 8 / Layers;
		private const float StarColorZ = 7 / Layers;
		private const float StarSaturationZ = 6 / Layers;
		private const float StarNameZ = 5 / Layers;
		private const float FleetZ = 4 / Layers;
		private const float SelectionIndicatorZ = 3 / Layers;
		private const float EtaZ = 2 / Layers;

		private const float PanClickTolerance = 0.01f;
		private const float ClickRadius = 0.5f;
		private const float StarMinClickRadius = 0.75f;

		private const float EtaTextScale = 0.3f;
		private const float FleetIndicatorScale = 0.25f;
		private const float FleetSelectorScale = 0.3f;
		private const float PathWidth = 0.1f;
		private const float StarNameScale = 0.35f;

		public FleetController SelectedFleet { private get; set; }
		private readonly IGalaxyViewListener galaxyViewListener;
		private readonly SignalFlag refreshData = new SignalFlag();

		private readonly GuiText turnCounter;
		private readonly GuiText fuelInfo;
		private readonly ConstructionSiteView starInfo;
		private readonly ListPanel fleetsPanel;

		private IEnumerable<SceneObject> fleetMovementPaths = null;
		private IEnumerable<SceneObject> fleetMarkers = null;
		private IEnumerable<SceneObject> scanRanges = null;
		private SceneObject movementEtaText = null;
		private SceneObject movementSimulationPath = null;
		private SceneObject selectionMarkers = null;
		private SceneObject wormholeSprites = null;
		private IEnumerable<SceneObject> starSprites = null;

		private int zoomLevel = 2;
		private Vector4? lastMousePosition = null;
		private float panAbsPath = 0;
		private Vector2 originOffset = Vector2.Zero;
		private float screenUnitScale;
		private Vector2 mapBoundsMin;
		private Vector2 mapBoundsMax;
		private float pixelSize = 1;

		private readonly OpenReportVisitor reportOpener;
		private GalaxySelectionType currentSelection = GalaxySelectionType.None;
		private readonly Dictionary<int, Vector2D> lastSelectedStars = new Dictionary<int, Vector2D>();
		private readonly Dictionary<int, FleetInfo> lastSelectedIdleFleets = new Dictionary<int, FleetInfo>();
		private readonly Dictionary<int, Vector2> lastOffset = new Dictionary<int, Vector2>(); //TODO(v0.9) remember player's zoom level too, unify with last selected object
		private PlayerController currentPlayer = null;

		public GalaxyScene(IGalaxyViewListener galaxyViewListener, Action mainMenuCallback)
		{
			this.galaxyViewListener = galaxyViewListener;
			this.reportOpener = new OpenReportVisitor(showDevelopment, showRelations, showResearch);

			var mainMenuButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 7),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 7),
				ForgroundImage = GalaxyTextures.Get.MainMenu,
				Padding = 4,
				Margins = new Vector2(5, 5),
				ClickCallback = mainMenuCallback,
				Tooltip = new SimpleTooltip("GalaxyScene", "MainMenuTooltip")
			};
			mainMenuButton.Position.FixedSize(36, 32).ParentRelative(-1, 1).UseMargins();
			this.AddElement(mainMenuButton);

			this.fuelInfo = new GuiText
			{
				Margins = new Vector2(20, 5),
				TextColor = Color.Yellow,
				TextHeight = 24,
				Tooltip = new SimpleTooltip("GalaxyScene", "FuelTooltip")
			};
			this.fuelInfo.Position.WrapContent().Then.RelativeTo(mainMenuButton, 1, 0, -1, 0).UseMargins();
			this.AddElement(this.fuelInfo);

			var designButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 7),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 7),
				ForgroundImage = GalaxyTextures.Get.Design,
				PaddingX = 12,
				PaddingY = 4,
				Margins = new Vector2(20, 5),
				ClickCallback = () => { using (var form = new FormShipDesignList(this.currentPlayer)) form.ShowDialog(); },
				Tooltip = new SimpleTooltip("GalaxyScene", "DesignTooltip")
			};
			designButton.Position.FixedSize(48, 32).RelativeTo(fuelInfo, 1, 0, -1, 0).UseMargins();
			this.AddElement(designButton);

			var developmentButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 7),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 7),
				ForgroundImage = GalaxyTextures.Get.Development,
				PaddingX = 12,
				PaddingY = 4,
				Margins = new Vector2(5, 5),
				ClickCallback = this.showDevelopment,
				Tooltip = new SimpleTooltip("GalaxyScene", "DevelopmentTooltip")
			};
			developmentButton.Position.FixedSize(48, 32).RelativeTo(designButton, 1, 0, -1, 0).UseMargins();
			this.AddElement(developmentButton);

			var researchButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 7),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 7),
				ForgroundImage = GalaxyTextures.Get.Research,
				PaddingX = 12,
				PaddingY = 4,
				Margins = new Vector2(5, 5),
				ClickCallback = this.showResearch,
				Tooltip = new SimpleTooltip("GalaxyScene", "ResearchTooltip")
			};
			researchButton.Position.FixedSize(48, 32).RelativeTo(developmentButton, 1, 0, -1, 0).UseMargins();
			this.AddElement(researchButton);

			var diplomacyButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 7),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 7),
				ForgroundImage = GalaxyTextures.Get.Diplomacy,
				PaddingX = 12,
				PaddingY = 4,
				Margins = new Vector2(5, 5),
				ClickCallback = showRelations,
				Tooltip = new SimpleTooltip("GalaxyScene", "DiplomacyTooltip")
			};
			diplomacyButton.Position.FixedSize(48, 32).RelativeTo(researchButton, 1, 0, -1, 0).UseMargins();
			this.AddElement(diplomacyButton);

			var colonizationButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 7),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 7),
				ForgroundImage = GalaxyTextures.Get.Colonization,
				PaddingX = 12,
				PaddingY = 4,
				Margins = new Vector2(5, 5),
				ClickCallback = () => { using (var form = new FormColonization(this.currentPlayer)) form.ShowDialog(); },
				Tooltip = new SimpleTooltip("GalaxyScene", "ColonizationTooltip")
			};
			colonizationButton.Position.FixedSize(48, 32).RelativeTo(diplomacyButton, 1, 0, -1, 0).UseMargins();
			this.AddElement(colonizationButton);

			var reportsButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 7),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 7),
				ForgroundImage = GalaxyTextures.Get.Reports,
				PaddingX = 12,
				PaddingY = 4,
				Margins = new Vector2(5, 5),
				ClickCallback = showReports,
				Tooltip = new SimpleTooltip("GalaxyScene", "ReportsTooltip")
			};
			reportsButton.Position.FixedSize(48, 32).RelativeTo(colonizationButton, 1, 0, -1, 0).UseMargins();
			this.AddElement(reportsButton);

			var stareaterButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 7),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 7),
				ForgroundImage = GalaxyTextures.Get.Stareater,
				PaddingX = 12,
				PaddingY = 4,
				Margins = new Vector2(5, 5),
				ClickCallback = () => { using (var form = new FormStareater(this.currentPlayer.Stareater)) form.ShowDialog(); },
				Tooltip = new SimpleTooltip("GalaxyScene", "StareaterTooltip")
			};
			stareaterButton.Position.FixedSize(48, 32).RelativeTo(reportsButton, 1, 0, -1, 0).UseMargins();
			this.AddElement(stareaterButton);

			var libraryButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 7),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 7),
				ForgroundImage = GalaxyTextures.Get.Library,
				PaddingX = 12,
				PaddingY = 4,
				Margins = new Vector2(5, 5),
				ClickCallback = () => { using (var form = new FormLibrary(this.currentPlayer.Library)) form.ShowDialog(); },
				Tooltip = new SimpleTooltip("GalaxyScene", "LibraryTooltip")
			};
			libraryButton.Position.FixedSize(48, 32).RelativeTo(stareaterButton, 1, 0, -1, 0).UseMargins();
			this.AddElement(libraryButton);

			this.turnCounter = new GuiText 
			{
				Margins = new Vector2(10, 5),
				TextColor = Color.LightGray, 
				TextHeight = 24 
			};
			this.turnCounter.Position.WrapContent().Then.ParentRelative(1, 1).UseMargins();
			this.AddElement(this.turnCounter);

			var turnButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.EndTurnHover, 0),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.EndTurnNormal, 0),
				Margins = new Vector2(10, 10),
				ClickCallback = this.galaxyViewListener.TurnEnded,
				Tooltip = new SimpleTooltip("GalaxyScene", "EndTurn")
			};
			turnButton.Position.FixedSize(80, 80).ParentRelative(1, -1).UseMargins();
			this.AddElement(turnButton);

			var radarToggle = new ToggleButton(SettingsWinforms.Get.ShowScanRange)
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 7),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 7),
				BackgroundToggled = new BackgroundTexture(GalaxyTextures.Get.ToggleToggled, 7),
				ForgroundImage = new BackgroundTexture(GalaxyTextures.Get.Radar, 0),
				Margins = new Vector2(15, 0),
				ToggleCallback = this.toggleRadar,
				Tooltip = new SimpleTooltip("GalaxyScene", "RadarSwitchToolip")
			};
			radarToggle.Position.FixedSize(24, 24).RelativeTo(turnButton, -1, 1, 1, 1).UseMargins();
			this.AddElement(radarToggle);

			this.starInfo = new ConstructionSiteView();
			this.starInfo.Position.ParentRelative(0, -1);
			this.AddElement(this.starInfo);

			this.fleetsPanel = new ListPanel(3, 2, AMapSelectableItem<ShipGroupInfo>.Width, AMapSelectableItem<ShipGroupInfo>.Height, 5)
			{
				Background = new BackgroundTexture(GalaxyTextures.Get.PanelBackground, 6),
				Padding = 5
			};
			this.fleetsPanel.Position.ParentRelative(0, -1);
		}

		public void OnNewTurn()
		{
			this.refreshData.Set();

			var filter = new FilterRepotVisitor();
			if (this.currentPlayer.Reports.Any(filter.ShowItem))
				this.showReports();
		}

		public void SwitchPlayer(PlayerController player)
		{
			player.RunAutomation();

			if (this.currentPlayer != null)
				this.lastOffset[this.currentPlayer.PlayerIndex] = originOffset;

			this.currentPlayer = player;

			//Assumes all players can see the same map size
			if (this.lastSelectedStars.Count == 0)
			{
				this.mapBoundsMin = new Vector2(
					(float)this.currentPlayer.Stars.Min(star => star.Position.X) - StarMinClickRadius,
					(float)this.currentPlayer.Stars.Min(star => star.Position.Y) - StarMinClickRadius
				);
				this.mapBoundsMax = new Vector2(
					(float)this.currentPlayer.Stars.Max(star => star.Position.X) + StarMinClickRadius,
					(float)this.currentPlayer.Stars.Max(star => star.Position.Y) + StarMinClickRadius
				);
			}

			if (!this.lastSelectedStars.ContainsKey(this.currentPlayer.PlayerIndex) ||
				!this.currentPlayer.Stars.Any(x => x.Position == this.lastSelectedStars[this.currentPlayer.PlayerIndex]))
				this.selectDefaultStar();

			this.originOffset = this.lastOffset[this.currentPlayer.PlayerIndex];
			this.currentSelection = GalaxySelectionType.Star;
			this.showStarInfo(this.lastSelectedStar);
			this.setupPerspective();
			this.setupFuelInfo();
		}

		private void showDevelopment()
		{
			using (var form = new FormDevelopment(this.currentPlayer))
				form.ShowDialog();
		}

		private void showRelations()
		{
			using (var form = new FormRelations(this.currentPlayer)) 
				form.ShowDialog();
		}

		private void showResearch()
		{
			//TODO(later) remove the need for if
			if (!this.currentPlayer.ResearchTopics().Any())
				return;

			using (var form = new FormResearch(this.currentPlayer))
				form.ShowDialog();
		}

		private void showSelectionPanel(StarInfo star, List<FleetInfo> fleets)
		{
			if (star != null && fleets.Count == 0)
			{
				this.currentSelection = GalaxySelectionType.Star;
				this.lastSelectedStars[this.currentPlayer.PlayerIndex] = star.Position;
				this.showStarInfo(this.lastSelectedStar);
				this.setupSelectionMarkers();
				return;
			}

			this.showBottomView(this.fleetsPanel);

			if (fleets.Count == 1 && fleets[0].Owner == this.currentPlayer.Info)
			{
				if (this.SelectedFleet == null)
				{
					this.currentSelection = GalaxySelectionType.Fleet;
					this.SelectedFleet = this.currentPlayer.SelectFleet(fleets[0]);
					if (fleets[0].Missions.Waypoints.Length == 0)
						this.lastSelectedIdleFleets[this.currentPlayer.PlayerIndex] = fleets[0]; //TODO(v0.9) marks wrong fleet when there are multiple players 
				}

				this.fleetsPanel.Children = this.SelectedFleet.ShipGroups.
					Select(x => new ShipSelectableItem(x)
					{
						ImageBackground = this.currentPlayer.Info.Color,
						Image = GalaxyTextures.Get.Sprite(x.Design.ImagePath),
						Text = shipGroupText(x),
						IsSelected = true,
						OnSelect = item => shipGroupSelect(item, item.Data.Quantity),
						OnDeselect = item => shipGroupSelect(item, 0),
						OnSplit = shipGroupSplit
					});
				return;
			}


			var stationaryFleet = fleets.FirstOrDefault(x => x.Owner == this.currentPlayer.Info && x.Missions.Waypoints.Length == 0);
			var otherOwnedFleets = fleets.Where(x => x.Owner == this.currentPlayer.Info && x != stationaryFleet);
			var othersFleets = fleets.Where(x => x.Owner != this.currentPlayer.Info);
			var fleetItems = new[] { stationaryFleet }.
				Concat(otherOwnedFleets).
				Concat(othersFleets).
				Select(x => addFleetSelection(x));

			var starItem = star != null ? new MapSelectableItem<StarInfo>(star)
			{
				ImageBackground = Color.Black,
				Images = new[] {
					new Sprite(GalaxyTextures.Get.StarColor, star.Color),
					new Sprite(GalaxyTextures.Get.StarGlow, Color.White),
				},
				Text = star.Name.ToText(LocalizationManifest.Get.CurrentLanguage),
				OnSelect = item => showSelectionPanel(item.Data, new List<FleetInfo>())
			} :
			null;

			this.fleetsPanel.Children = new AGuiElement[] { starItem }.Concat(fleetItems).Where(x => x != null);
		}

		private MapSelectableItem<FleetInfo> addFleetSelection(FleetInfo fleet)
		{
			if (fleet == null)
				return null;

			var lang = LocalizationManifest.Get.CurrentLanguage;
			var context = lang["GalaxyScene"];
			var biggestGroup = fleet.Ships.Aggregate((a, b) => (a.Quantity * a.Design.Size > b.Quantity * b.Design.Size) ? a : b);

			//TODO(v0.9) text might be long, do word wrap
			var text = fleet.Missions.Waypoints.Length == 0 ?
				context["StationaryFleet"].Text() :
				context["MovingFleet"].Text(new TextVar(
					"destination",
					this.currentPlayer.Star(fleet.Missions.Waypoints[0].Destionation).Name.ToText(lang)
				).Get);

			return new MapSelectableItem<FleetInfo>(fleet)
			{
				ImageBackground = fleet.Owner.Color,
				Image = GalaxyTextures.Get.Sprite(biggestGroup.Design.ImagePath),
				Text = text,
				OnSelect = item => showSelectionPanel(null, new List<FleetInfo> { item.Data })
			};
		}

		private void showStarInfo(StarInfo star)
		{
			var starSystem = this.currentPlayer.OpenStarSystem(star);

			//TODO(later) update owner check when multiple stellarises can exist at the same star
			if (starSystem.StarsAdministration() != null && starSystem.StarsAdministration().Owner == this.currentPlayer.Info)
			{
				this.starInfo.SetView(this.currentPlayer.OpenStarSystem(this.lastSelectedStar).StellarisController());
				this.showBottomView(this.starInfo);
			}
			else
				this.hideBottomView();
		}

		private void shipGroupSelect(ShipSelectableItem item, long quantity)
		{
			this.SelectedFleet.SelectGroup(item.Data, quantity);
			item.Text = shipGroupText(item.Data);
		}

		private string shipGroupText(ShipGroupInfo group)
		{
			var selected = this.SelectedFleet.SelectionCount(group);

			if (selected == 0 || selected == group.Quantity)
				return group.Design.Name + Environment.NewLine + new ThousandsFormatter().Format(group.Quantity);

			var thousandsFormat = new ThousandsFormatter(group.Quantity);
			return group.Design.Name + Environment.NewLine +
					thousandsFormat.Format(selected) + " / " + thousandsFormat.Format(group.Quantity);
		}

		private void shipGroupSplit(ShipSelectableItem shipItem)
		{
			using (var form = new FormSelectQuantity(shipItem.Data.Quantity, 1))
			{
				form.ShowDialog();

				var thousandsFormat = new ThousandsFormatter(shipItem.Data.Quantity);
				shipItem.IsSelected = form.SelectedValue > 0;
				shipItem.Text = shipItem.Data.Design.Name + Environment.NewLine +
					thousandsFormat.Format(form.SelectedValue) + " / " + thousandsFormat.Format(shipItem.Data.Quantity);

				this.SelectedFleet.SelectGroup(shipItem.Data, form.SelectedValue);
			}
		}

		private void toggleRadar(bool state)
		{
			SettingsWinforms.Get.ShowScanRange = state;
			this.setupScanRanges();
		}

		#region AScene implementation
		protected override float guiLayerThickness => 1 / Layers;

		public override void Activate()
		{
			this.setupVaos();

			if (this.currentSelection == GalaxySelectionType.Star)
				this.showStarInfo(this.lastSelectedStar);
		}

		protected override void frameUpdate(double deltaTime)
		{
			if (this.refreshData.Check())
			{
				if (this.SelectedFleet != null && !this.SelectedFleet.Valid)
					this.SelectedFleet = null;

				if (this.currentSelection == GalaxySelectionType.Fleet)
					this.currentSelection = GalaxySelectionType.None;

				this.ResetLists();
			}
		}

		//TODO(v0.9) refactor and remove
		public void ResetLists()
		{
			this.setupVaos();
		}
		#endregion

		#region Drawing setup and helpers
		protected override Matrix4 calculatePerspective()
		{
			var aspect = this.canvasSize.X / this.canvasSize.Y;
			var radius = DefaultViewSize * (float)Math.Pow(ZoomBase, -this.zoomLevel);

			this.pixelSize = radius / canvasSize.Y;
			this.screenUnitScale = (float)Math.Pow(ZoomBase, -this.zoomLevel) * this.screenSize.Y / this.canvasSize.Y;

			// Update screen space elements
			this.setupScanRanges();

			return calcOrthogonalPerspective(aspect * radius, radius, FarZ, originOffset);
		}

		private IEnumerable<Vector2> fleetMovementPathVertices(FleetInfo fleet, IEnumerable<Vector2> waypoints)
		{
			var lastPosition = fleetDisplayPosition(fleet);
			foreach (var nextPosition in waypoints)
			{
				foreach (var v in SpriteHelpers.PathRect(lastPosition, nextPosition, PathWidth, GalaxyTextures.Get.PathLine))
					yield return v;

				lastPosition = nextPosition;
			}
		}

		private Vector2 fleetDisplayPosition(FleetInfo fleet)
		{
			var atStar = this.queryScene(fleet.Position, 1).Any(x => x.Data is StarInfo);
			var displayPosition = new Vector2((float)fleet.Position.X, (float)fleet.Position.Y);

			if (!fleet.IsMoving)
			{
				var players = this.currentPlayer.FleetsAt(fleet.Position).
					Select(x => x.Owner).
					Where(x => x != this.currentPlayer.Info).
					Distinct().ToList(); //TODO(v0.9) sort players by some key

				int index = (fleet.Owner == this.currentPlayer.Info) ? 0 : (1 + players.IndexOf(fleet.Owner));
				displayPosition += new Vector2(0.5f, 0.5f - 0.2f * index);
			}
			else if (fleet.IsMoving && atStar)
				displayPosition += new Vector2(-0.5f, 0.5f);

			return displayPosition;
		}

		private void setupVaos()
		{
			//setup stars first so fleets can query them 
			this.setupStarSprites();

			this.setupScanRanges();
			this.setupFleetMarkers();
			this.setupFleetMovement();
			this.setupMovementEta();
			this.setupMovementSimulation();
			this.setupSelectionMarkers();
			this.setupWormholeSprites();

			this.setupFuelInfo();
			this.setupTurnCounter();
		}

		private void setupScanRanges()
		{
			if (!SettingsWinforms.Get.ShowScanRange)
			{
				this.RemoveFromScene(ref this.scanRanges);
				return;
			}

			var arcBuilder = new ArcBorderBuilder();
			arcBuilder.AddCircles(this.currentPlayer.ScanAreas().ToList());

			var borderThickness = 0.06f * this.screenUnitScale;
			var zStep = InterlayerZRange / (float)arcBuilder.Count;

			this.UpdateScene(
				ref this.scanRanges,
				arcBuilder.Vertices().Select((circle, i) =>
					new SceneObjectBuilder().
						StartOrbit(ScanRangeZ + i * zStep, circle.Radius - borderThickness, circle.Radius, GalaxyTextures.Get.PathLine, Color.FromArgb(128, 96, 0)).
						Translate(convert(circle.Center)).
						AddVertices(circle.Vertices).
						Build()
				).ToList()
			);
		}

		private void setupFleetMarkers()
		{
			this.UpdateScene(
				ref this.fleetMarkers,
				this.currentPlayer.FleetsAll.Select(
					fleet =>
					{
						var position = fleetDisplayPosition(fleet);

						return new SceneObjectBuilder(fleet, position, 0).
							StartSimpleSprite(FleetZ, GalaxyTextures.Get.FleetIndicator, fleet.Owner.Color).
							Scale(FleetIndicatorScale).
							Translate(position).
							Build();
					}).ToList()
			);
		}

		private void setupFleetMovement()
		{
			this.UpdateScene(
				ref this.fleetMovementPaths,
				this.currentPlayer.FleetsAll.Where(x => x.IsMoving).Select(fleet =>
					new SceneObjectBuilder().
					StartSprite(PathZ, GalaxyTextures.Get.PathLine.Id, Color.DarkGreen).
					AddVertices(fleetMovementPathVertices(fleet, fleet.Missions.Waypoints.Select(v => convert(v.Destionation)))).
					Build()
				).ToList()
			);
		}

		//TODO(v0.9) bundle with movement simulation
		private void setupMovementEta()
		{
			if (this.SelectedFleet == null || !this.SelectedFleet.SimulationWaypoints().Any())
			{
				if (this.movementEtaText != null)
					this.RemoveFromScene(ref this.movementEtaText);

				return;
			}

			var waypoints = this.SelectedFleet.SimulationWaypoints();

			var destination = waypoints[waypoints.Count - 1];
			var numVars = new Var("eta", Math.Ceiling(this.SelectedFleet.SimulationEta)).Get;
			var textVars = new TextVar("eta", new DecimalsFormatter(0, 1).Format(this.SelectedFleet.SimulationEta, RoundingMethod.Ceil, 0)).
				And("fuel", new ThousandsFormatter().Format(this.SelectedFleet.SimulationFuel)).Get;

			this.UpdateScene(
				ref this.movementEtaText,
				new SceneObjectBuilder().
					PixelSize(this.pixelSize).
					StartText(
						LocalizationManifest.Get.CurrentLanguage["FormMain"]["FleetEta"].Text(numVars, textVars),
						-0.5f, 0, EtaZ, InterlayerZRange, Color.White
					).
					Scale(EtaTextScale / (float)Math.Pow(ZoomBase, zoomLevel)).
					Translate(destination.Position.X, destination.Position.Y + 0.5).
					Build()
			);
		}

		private void setupMovementSimulation()
		{
			if (this.SelectedFleet == null)
			{
				if (this.movementSimulationPath != null)
					this.RemoveFromScene(ref this.movementSimulationPath);

				return;
			}

			var waypoints = this.SelectedFleet.SimulationWaypoints();

			if (this.SelectedFleet != null && waypoints.Count > 0)
				this.UpdateScene(
					ref this.movementSimulationPath,
					new SceneObjectBuilder().
						StartSprite(PathZ, GalaxyTextures.Get.PathLine.Id, Color.LimeGreen).
						AddVertices(fleetMovementPathVertices(this.SelectedFleet.Fleet, waypoints.Select(x => convert(x.Position)))).
						Build()
				);
		}

		private void setupFuelInfo()
		{
			var formatter = new ThousandsFormatter(this.currentPlayer.FuelAvailable);

			this.fuelInfo.Text = LocalizationManifest.Get.CurrentLanguage["GalaxyScene"]["FuelInfo"].Text(
				new TextVar("fuelLeft", formatter.Format(this.currentPlayer.FuelAvailable - this.currentPlayer.FuelUsage)).
				And("fuelAvailable", formatter.Format(this.currentPlayer.FuelAvailable)).Get
			);
		}

		private void setupTurnCounter()
		{
			this.turnCounter.Text = LocalizationManifest.Get.CurrentLanguage["GalaxyScene"]["Turn"].Text() + " " + this.currentPlayer.Turn;
		}

		private void setupSelectionMarkers()
		{
			var transform = new Matrix4();

			if (this.currentSelection == GalaxySelectionType.Star)
				transform = Matrix4.CreateTranslation((float)this.lastSelectedStarPosition.X, (float)this.lastSelectedStarPosition.Y, 0);
			else if (this.currentSelection == GalaxySelectionType.Fleet)
			{
				var position = this.fleetDisplayPosition(this.lastSelectedIdleFleet);
				transform = Matrix4.CreateScale(FleetSelectorScale) * Matrix4.CreateTranslation(position.X, position.Y, 0);
			}

			this.UpdateScene(
				ref this.selectionMarkers,
				new SceneObjectBuilder().
					StartSimpleSprite(SelectionIndicatorZ, GalaxyTextures.Get.SelectedStar, Color.White).
					Transform(transform).
					Build()
			);
		}

		private void setupStarSprites()
		{
			var stars = this.currentPlayer.Stars.OrderBy(x => x.Position.Y).ToList();
			var textZRange = InterlayerZRange / stars.Count;

			this.UpdateScene(
				ref this.starSprites,
				stars.Select((star, i) => starObject(star, i, textZRange)).ToList()
			);
		}

		private void setupWormholeSprites()
		{
			this.UpdateScene(
				ref this.wormholeSprites,
				new SceneObjectBuilder().
					StartSprite(WormholeZ, GalaxyTextures.Get.PathLine.Id, Color.Blue).
					AddVertices(
						this.currentPlayer.Wormholes.SelectMany(wormhole => SpriteHelpers.PathRect(
							convert(wormhole.Endpoints.First.Position),
							convert(wormhole.Endpoints.Second.Position),
							0.8f * PathWidth,
							GalaxyTextures.Get.PathLine
					))).
					Build()
			);
		}

		private SceneObject starObject(StarInfo star, int index, float textZRange)
		{
			var soBuilder = new SceneObjectBuilder(star, convert(star.Position), 0).
				StartSimpleSprite(StarColorZ, GalaxyTextures.Get.StarColor, star.Color).
				Scale(star.Size).
				Translate(convert(star.Position)).

				StartSimpleSprite(StarSaturationZ, GalaxyTextures.Get.StarGlow, Color.White).
				Scale(star.Size).
				Translate(convert(star.Position));

			if (this.zoomLevel > NameZoomLimit)
				soBuilder.PixelSize(this.pixelSize).
					StartText(
						star.Name.ToText(LocalizationManifest.Get.CurrentLanguage),
						-0.5f, 0, StarNameZ + index * textZRange, textZRange, starNameColor(star)
					).
					Scale(StarNameScale / (float)Math.Pow(ZoomBase, this.zoomLevel)).
					Translate(star.Position.X, star.Position.Y - 0.5);
			
			return soBuilder.Build();
		}
		#endregion

		#region Input events
		protected override void onMouseMove(Vector4 mouseViewPosition, Keys modiferKeys)
		{
			this.lastMousePosition = mouseViewPosition;
			this.panAbsPath = 0;

			if (this.SelectedFleet != null)
				this.simulateFleetMovement(mouseViewPosition, modiferKeys);
		}

		protected override void onMouseDrag(Vector4 currentPosition)
		{
			if (!this.lastMousePosition.HasValue)
				this.lastMousePosition = currentPosition;

			this.panAbsPath += (currentPosition - this.lastMousePosition.Value).Length;

			this.originOffset -= (Vector4.Transform(currentPosition, this.invProjection) -
				Vector4.Transform(this.lastMousePosition.Value, this.invProjection)
				).Xy;

			this.limitPan();

			this.lastMousePosition = currentPosition;
			this.setupPerspective();
		}

		protected override void onMouseScroll(Vector2 mousePoint, int delta)
		{
			float oldZoom = 1 / (float)(0.5 * DefaultViewSize / Math.Pow(ZoomBase, zoomLevel));

			if (delta > 0)
				this.zoomLevel++;
			else
				this.zoomLevel--;

			this.zoomLevel = Methods.Clamp(zoomLevel, MinZoom, MaxZoom);

			float newZoom = 1 / (float)(0.5 * DefaultViewSize / Math.Pow(ZoomBase, this.zoomLevel));

			this.originOffset = (this.originOffset * oldZoom + mousePoint * (newZoom - oldZoom)) / newZoom;
			this.limitPan();
			this.setupPerspective();
			this.setupScanRanges();
			this.setupStarSprites();
			this.setupMovementEta();
		}

		protected override void onMouseClick(Vector2 mousePoint, Keys modiferKeys)
		{
			if (panAbsPath > PanClickTolerance) //TODO(v0.9) maybe make AScene differentiate between click and drag
				return;

			var searchRadius = Math.Max(this.screenUnitScale * ClickRadius, StarMinClickRadius);
			var searchPoint = convert(mousePoint);

			var allObjects = this.queryScene(searchPoint, searchRadius).
				OrderBy(x => (x.PhysicalShape.Center - convert(searchPoint)).LengthSquared).
				ToList();
			var starFound = allObjects.Where(x => x.Data is StarInfo).Select(x => x.Data as StarInfo).FirstOrDefault();
			var fleetFound = allObjects.Where(x => x.Data is FleetInfo).Select(x => x.Data as FleetInfo).ToList();
			var foundAny = allObjects.Any();

			if (this.SelectedFleet != null)
				if (starFound != null)
				{
					var destination = this.SelectedFleet.SimulationWaypoints().Any() ? this.SelectedFleet.SimulationWaypoints().Last() : starFound;
					this.SelectedFleet = modiferKeys.HasFlag(Keys.Control) ? this.SelectedFleet.SendDirectly(destination) : this.SelectedFleet.Send(destination);
					this.lastSelectedIdleFleets[this.currentPlayer.PlayerIndex] = this.SelectedFleet.Fleet;
					this.showSelectionPanel(null, new List<FleetInfo> { this.SelectedFleet.Fleet });
					this.setupFleetMarkers();
					this.setupFleetMovement();
					this.setupSelectionMarkers();
					this.setupFuelInfo();
					return;
				}
				else
				{
					this.hideBottomView();
					this.SelectedFleet = null;
					this.setupMovementEta();
					this.setupMovementSimulation();
					this.setupSelectionMarkers();
				}


			if (!foundAny)
				return;

			this.showSelectionPanel(starFound, fleetFound);
			this.setupSelectionMarkers();

		}

		protected override void onMouseDoubleClick(Vector2 mousePoint)
		{
			if (panAbsPath > PanClickTolerance)
				return;

			var searchRadius = Math.Max(this.screenUnitScale * ClickRadius, StarMinClickRadius);
			var searchPoint = convert(mousePoint);

			var starsFound = this.queryScene(searchPoint, searchRadius).
				Where(x => x.Data is StarInfo).
				Select(x => x.Data as StarInfo).
				OrderBy(x => (x.Position - searchPoint).Length).
				ToList();

			if (starsFound.Any())
				this.galaxyViewListener.SystemOpened(this.currentPlayer.OpenStarSystem(starsFound[0]));
		}

		protected override void onKeyPress(char c)
		{
			//TODO(later) make rebindable
			if (c == ' ')
				this.showReports();
		}
		#endregion

		#region Helper methods
		private void simulateFleetMovement(Vector4 currentPosition, Keys modiferKeys)
		{
			if (!this.SelectedFleet.CanMove)
				return;

			Vector4 mousePoint = Vector4.Transform(currentPosition, invProjection);
			var searchRadius = Math.Max(this.screenUnitScale * ClickRadius, StarMinClickRadius);
			var searchPoint = new Vector2D(mousePoint.X, mousePoint.Y);

			var starsFound = this.queryScene(searchPoint, searchRadius).
				Where(x => x.Data is StarInfo).
				OrderBy(x => (x.PhysicalShape.Center - convert(searchPoint)).LengthSquared).
				ToList();

			if (!starsFound.Any())
				return;

			if (!modiferKeys.HasFlag(Keys.Control))
				this.SelectedFleet.SimulateTravel(starsFound[0].Data as StarInfo);
			else
				this.SelectedFleet.SimulateDirectTravel(starsFound[0].Data as StarInfo);
			this.setupMovementEta();
			this.setupMovementSimulation();
		}

		private void limitPan()
		{
			if (this.originOffset.X < mapBoundsMin.X)
				this.originOffset.X = mapBoundsMin.X;
			if (this.originOffset.X > mapBoundsMax.X)
				this.originOffset.X = mapBoundsMax.X;

			if (this.originOffset.Y < mapBoundsMin.Y)
				this.originOffset.Y = mapBoundsMin.Y;
			if (this.originOffset.Y > mapBoundsMax.Y)
				this.originOffset.Y = mapBoundsMax.Y;
		}

		private FleetInfo lastSelectedIdleFleet
		{
			get
			{
				return this.lastSelectedIdleFleets[this.currentPlayer.PlayerIndex];
			}
		}

		private StarInfo lastSelectedStar
		{
			get
			{
				return this.currentPlayer.Star(this.lastSelectedStars[this.currentPlayer.PlayerIndex]);
			}
		}

		//TODO(v0.9) remove one of lastSelectedStar methods
		private Vector2D lastSelectedStarPosition
		{
			get
			{
				return this.lastSelectedStars[this.currentPlayer.PlayerIndex];
			}
		}

		private Color starNameColor(StarInfo star)
		{
			if (this.currentPlayer.IsStarVisited(star))
			{
				var colonies = this.currentPlayer.KnownColonies(star);

				if (colonies.Any())
				{
					var dominantPlayer = colonies.GroupBy(x => x.Owner).OrderByDescending(x => x.Count()).First().Key;
					return dominantPlayer.Color;
				}

				return Color.LightGray;
			}

			return Color.FromArgb(64, 64, 64);
		}

		private void selectDefaultStar()
		{
			//TODO(v0.9) what if there are no stellarises?
			var bestStar = this.currentPlayer.Stellarises().
				Aggregate((a, b) => a.Population > b.Population ? a : b);

			this.lastSelectedStars[this.currentPlayer.PlayerIndex] = bestStar.HostStar.Position;
			this.lastOffset[this.currentPlayer.PlayerIndex] = convert(bestStar.HostStar.Position);
		}

		private void showBottomView(AGuiElement view)
		{
			foreach (var selectionView in new AGuiElement[] { this.starInfo, this.fleetsPanel })
				if (selectionView.Equals(view))
					this.ShowElement(selectionView);
				else
					this.HideElement(selectionView);
		}

		private void hideBottomView()
		{
			foreach (var selectionView in new AGuiElement[] { this.starInfo, this.fleetsPanel })
				this.HideElement(selectionView);
		}

		private void showReports()
		{
			using (var form = new FormReports(this.currentPlayer.Reports))
				if (form.ShowDialog() == DialogResult.OK)
					form.Result.Accept(this.reportOpener);
		}
		#endregion
	}
}
