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

namespace Stareater.GameScenes
{
	class GalaxyScene : AScene
	{
		private const float DefaultViewSize = 15;
		private const double ZoomBase = 1.2f;
		private const int MaxZoom = 10;
		private const int MinZoom = -10;
		
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

		private readonly OpenReportVisitor reportOpener;
		private GalaxySelectionType currentSelection = GalaxySelectionType.None;
		private readonly Dictionary<int, Vector2D> lastSelectedStars = new Dictionary<int, Vector2D>();
		private readonly Dictionary<int, FleetInfo> lastSelectedIdleFleets = new Dictionary<int, FleetInfo>();
		private readonly Dictionary<int, Vector2> lastOffset = new Dictionary<int, Vector2>(); //TODO(v0.8) remember player's zoom level too, unify with last selected object
		private PlayerController currentPlayer = null;

		public GalaxyScene(IGalaxyViewListener galaxyViewListener, Action mainMenuCallback)
		{ 
			this.galaxyViewListener = galaxyViewListener;
			this.reportOpener = new OpenReportVisitor(showDevelopment, showResearch);

			var mainMenuButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 8),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 8),
				ForgroundImage = GalaxyTextures.Get.MainMenu,
				Padding = 4,
				ClickCallback = mainMenuCallback,
				Tooltip = new SimpleTooltip("GalaxyScene", "MainMenuTooltip")
			};
			mainMenuButton.Position.FixedSize(32, 32).ParentRelative(-1, 1).WithMargins(5, 5);
			this.AddElement(mainMenuButton);

			this.fuelInfo = new GuiText
			{
				TextColor = Color.Yellow,
				TextSize = 23,
				Tooltip = new SimpleTooltip("GalaxyScene", "FuelTooltip")
			};
			this.fuelInfo.Position.WrapContent().Then.RelativeTo(mainMenuButton, 1, 0, -1, 0).WithMargins(20, 5);
			this.AddElement(this.fuelInfo);

			var designButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 8),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 8),
				ForgroundImage = GalaxyTextures.Get.Design,
				PaddingX = 12,
				PaddingY = 4,
				ClickCallback = () => { using (var form = new FormShipDesignList(this.currentPlayer)) form.ShowDialog(); },
				Tooltip = new SimpleTooltip("GalaxyScene", "DesignTooltip")
			};
			designButton.Position.FixedSize(48, 32).RelativeTo(fuelInfo, 1, 0, -1, 0).WithMargins(20, 5);
			this.AddElement(designButton);

			var developmentButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 8),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 8),
				ForgroundImage = GalaxyTextures.Get.Development,
				PaddingX = 12,
				PaddingY = 4,
				ClickCallback = this.showDevelopment,
				Tooltip = new SimpleTooltip("GalaxyScene", "DevelopmentTooltip")
			};
			developmentButton.Position.FixedSize(48, 32).RelativeTo(designButton, 1, 0, -1, 0).WithMargins(5, 5);
			this.AddElement(developmentButton);

			var researchButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 8),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 8),
				ForgroundImage = GalaxyTextures.Get.Research,
				PaddingX = 12,
				PaddingY = 4,
				ClickCallback = this.showResearch,
				Tooltip = new SimpleTooltip("GalaxyScene", "ResearchTooltip")
			};
			researchButton.Position.FixedSize(48, 32).RelativeTo(developmentButton, 1, 0, -1, 0).WithMargins(5, 5);
			this.AddElement(researchButton);

			var diplomacyButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 8),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 8),
				ForgroundImage = GalaxyTextures.Get.Diplomacy,
				PaddingX = 12,
				PaddingY = 4,
				ClickCallback = () => { using (var form = new FormRelations(this.currentPlayer)) form.ShowDialog(); },
				Tooltip = new SimpleTooltip("GalaxyScene", "ResearchTooltip")
			};
			diplomacyButton.Position.FixedSize(48, 32).RelativeTo(researchButton, 1, 0, -1, 0).WithMargins(5, 5);
			this.AddElement(diplomacyButton);

			var colonizationButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 8),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 8),
				ForgroundImage = GalaxyTextures.Get.Colonization,
				PaddingX = 12,
				PaddingY = 4,
				ClickCallback = () => { using (var form = new FormColonization(this.currentPlayer)) form.ShowDialog(); },
				Tooltip = new SimpleTooltip("GalaxyScene", "ColonizationTooltip")
			};
			colonizationButton.Position.FixedSize(48, 32).RelativeTo(diplomacyButton, 1, 0, -1, 0).WithMargins(5, 5);
			this.AddElement(colonizationButton);

			var reportsButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 8),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 8),
				ForgroundImage = GalaxyTextures.Get.Reports,
				PaddingX = 12,
				PaddingY = 4,
				ClickCallback = () => { using (var form = new FormReports(this.currentPlayer.Reports)) if (form.ShowDialog() == DialogResult.OK) form.Result.Accept(this.reportOpener); },
				Tooltip = new SimpleTooltip("GalaxyScene", "ReportsTooltip")
			};
			reportsButton.Position.FixedSize(48, 32).RelativeTo(colonizationButton, 1, 0, -1, 0).WithMargins(5, 5);
			this.AddElement(reportsButton);

			var stareaterButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 8),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 8),
				ForgroundImage = GalaxyTextures.Get.Stareater,
				PaddingX = 12,
				PaddingY = 4,
				ClickCallback = () => { using (var form = new FormStareater(this.currentPlayer.Stareater)) form.ShowDialog(); },
				Tooltip = new SimpleTooltip("GalaxyScene", "StareaterTooltip")
			};
			stareaterButton.Position.FixedSize(48, 32).RelativeTo(reportsButton, 1, 0, -1, 0).WithMargins(5, 5);
			this.AddElement(stareaterButton);

			var libraryButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 8),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 8),
				ForgroundImage = GalaxyTextures.Get.Library,
				PaddingX = 12,
				PaddingY = 4,
				ClickCallback = () => { using (var form = new FormLibrary(this.currentPlayer.Library)) form.ShowDialog(); },
				Tooltip = new SimpleTooltip("GalaxyScene", "LibraryTooltip")
			};
			libraryButton.Position.FixedSize(48, 32).RelativeTo(stareaterButton, 1, 0, -1, 0).WithMargins(5, 5);
			this.AddElement(libraryButton);

			this.turnCounter = new GuiText { TextColor = Color.LightGray, TextSize = 23 };
			this.turnCounter.Position.WrapContent().Then.ParentRelative(1, 1).WithMargins(10, 5);
			this.AddElement(this.turnCounter);

			var turnButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.EndTurnHover, 0),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.EndTurnNormal, 0),
				ClickCallback = this.galaxyViewListener.TurnEnded,
				Tooltip = new SimpleTooltip("GalaxyScene", "EndTurn")
			};
			turnButton.Position.FixedSize(80, 80).ParentRelative(1, -1).WithMargins(10, 10);
			this.AddElement(turnButton);

			var radarToggle = new ToggleButton(SettingsWinforms.Get.ShowScanRange)
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 0),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 0),
				BackgroundToggled = new BackgroundTexture(GalaxyTextures.Get.ToggleToggled, 0),
				ForgroundImage = new BackgroundTexture(GalaxyTextures.Get.Radar, 0),
				ToggleCallback = this.toggleRadar,
				Tooltip = new SimpleTooltip("GalaxyScene", "RadarSwitchToolip")
			};
			radarToggle.Position.FixedSize(20, 20).RelativeTo(turnButton, -1, 1, 1, 1).WithMargins(15, 0);
			this.AddElement(radarToggle);

			this.starInfo = new ConstructionSiteView();
			this.starInfo.Position.ParentRelative(0, -1).WithMargins(0, 0);
			this.AddElement(this.starInfo);
		}

		public void OnNewTurn()
		{
			this.refreshData.Set();
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
			this.updateStarInfo(this.lastSelectedStar);
			this.setupPerspective();
			this.setupFuelInfo();
		}

		private void showDevelopment()
		{
			using (var form = new FormDevelopment(this.currentPlayer))
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

		private void updateStarInfo(StarInfo star)
		{
			var starSystem = this.currentPlayer.OpenStarSystem(star);
			this.galaxyViewListener.SystemSelected(starSystem);

			//TODO(later) update owner check when multiple stellarises can exist at the same star
			if (starSystem.StarsAdministration() != null && starSystem.StarsAdministration().Owner == this.currentPlayer.Info)
			{
				this.starInfo.SetView(this.currentPlayer.OpenStarSystem(this.lastSelectedStar).StellarisController());
				this.ShowElement(this.starInfo);
			}
			else
				this.HideElement(this.starInfo);
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
				this.updateStarInfo(this.lastSelectedStar);
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

		//TODO(v0.8) refactor and remove
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

			this.screenUnitScale = (float)Math.Pow(ZoomBase, -this.zoomLevel) * this.screenSize.Y / this.canvasSize.Y;
			
			// Update screen space elements
			this.setupScanRanges();

			return calcOrthogonalPerspective(aspect * radius, radius, FarZ, originOffset);
		}

		private IEnumerable<Vector2> fleetMovementPathVertices(FleetInfo fleet, IEnumerable<Vector2> waypoints)
		{
			var lastPosition = fleetDisplayPosition(fleet);
			foreach(var nextPosition in waypoints)
			{
				foreach(var v in SpriteHelpers.PathRect(lastPosition, nextPosition, PathWidth, GalaxyTextures.Get.PathLine))
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
					Distinct().ToList(); //TODO(v0.8) sort players by some key
				
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

		//TODO(v0.8) bundle with movement simulation
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
					StartText(
						LocalizationManifest.Get.CurrentLanguage["FormMain"]["FleetEta"].Text(numVars, textVars), TextRenderUtil.RasterFontSize, 
						-0.5f, EtaZ, InterlayerZRange, 
						TextRenderUtil.Get.TextureId, Color.White, Matrix4.Identity
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
				stars.Select((star, i) => new SceneObjectBuilder(star, convert(star.Position), 0).
					StartSimpleSprite(StarColorZ, GalaxyTextures.Get.StarColor, star.Color).
					Scale(star.Size).
					Translate(convert(star.Position)).

					StartSimpleSprite(StarSaturationZ, GalaxyTextures.Get.StarGlow, Color.White).
					Scale(star.Size).
					Translate(convert(star.Position)).

					//TODO(v0.8) don't show names when zoomed out too much
					StartText(
						star.Name.ToText(LocalizationManifest.Get.CurrentLanguage), TextRenderUtil.RasterFontSize,
						-0.5f, StarNameZ + i * textZRange, textZRange, 
						TextRenderUtil.Get.TextureId, starNameColor(star),
						Matrix4.CreateScale(StarNameScale / (float)Math.Pow(ZoomBase, zoomLevel)) * Matrix4.CreateTranslation((float)star.Position.X, (float)star.Position.Y - 0.5f, 0)
					).
					Build()
				).ToList()
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
		#endregion

		#region Mouse events
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
			if (panAbsPath > PanClickTolerance) //TODO(v0.8) maybe make AScene differentiate between click and drag
				return;
			
			var searchRadius = Math.Max(this.screenUnitScale * ClickRadius, StarMinClickRadius);
			var searchPoint = convert(mousePoint);

			var allObjects = this.queryScene(searchPoint, searchRadius).
				OrderBy(x => (x.PhysicalShape.Center - convert(searchPoint)).LengthSquared).
				ToList();
			var starsFound = allObjects.Where(x => x.Data is StarInfo).Select(x => x.Data as StarInfo).ToList();
			var fleetFound = allObjects.Where(x => x.Data is FleetInfo).Select(x => x.Data as FleetInfo).ToList();
			
			var foundAny = starsFound.Any() || fleetFound.Any();
			var isStarClosest = 
				starsFound.Any() && 
				(
					!fleetFound.Any() ||
					(starsFound[0].Position - searchPoint).Length <= (fleetFound[0].Position - searchPoint).Length
				);

			if (this.SelectedFleet != null)
				if (foundAny && isStarClosest)
				{
					var destination = this.SelectedFleet.SimulationWaypoints().Any() ? this.SelectedFleet.SimulationWaypoints().Last() : starsFound[0];
					this.SelectedFleet = modiferKeys.HasFlag(Keys.Control) ? this.SelectedFleet.SendDirectly(destination) : this.SelectedFleet.Send(destination);
					this.lastSelectedIdleFleets[this.currentPlayer.PlayerIndex] = this.SelectedFleet.Fleet;
					this.galaxyViewListener.FleetClicked(new FleetInfo[] { this.SelectedFleet.Fleet });
					this.setupFleetMarkers();
					this.setupFleetMovement();
					this.setupSelectionMarkers();
					this.setupFuelInfo();
					return;
				}
				else
				{
					this.galaxyViewListener.FleetDeselected();
					this.SelectedFleet = null;
					this.setupMovementEta();
					this.setupMovementSimulation();
					this.setupSelectionMarkers();
				}


			if (!foundAny)
				return;
			
			if (isStarClosest)
			{
				this.currentSelection = GalaxySelectionType.Star;
				this.lastSelectedStars[this.currentPlayer.PlayerIndex] = starsFound[0].Position;
				this.updateStarInfo(this.lastSelectedStar);
				this.setupSelectionMarkers();
			}
			else
			{
				this.currentSelection = GalaxySelectionType.Fleet;
				this.lastSelectedIdleFleets[this.currentPlayer.PlayerIndex] = fleetFound[0]; //TODO(v0.8) marks wrong fleet when there are multiple players 
				this.galaxyViewListener.FleetClicked(fleetFound);
				this.setupSelectionMarkers();
			}
			
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
		
		//TODO(v0.8) remove one of lastSelectedStar methods
		private Vector2D lastSelectedStarPosition
		{
			get 
			{
				return this.lastSelectedStars[this.currentPlayer.PlayerIndex];
			}
		}
		
		private Color starNameColor(StarInfo star)
		{
			if (this.currentPlayer.IsStarVisited(star)) {
				var colonies = this.currentPlayer.KnownColonies(star);
				
				if (colonies.Any()) {
					var dominantPlayer = colonies.GroupBy(x => x.Owner).OrderByDescending(x => x.Count()).First().Key;
					return dominantPlayer.Color;
				}
				
				return Color.LightGray;
			}
			
			return Color.FromArgb(64, 64, 64);
		}

		private void selectDefaultStar()
		{
			//TODO(v0.8) what if there are no stellarises?
            var bestStar = this.currentPlayer.Stellarises().
				Aggregate((a, b) => a.Population > b.Population ? a : b);

			this.lastSelectedStars[this.currentPlayer.PlayerIndex] = bestStar.HostStar.Position;
			this.lastOffset[this.currentPlayer.PlayerIndex] = convert(bestStar.HostStar.Position);
		}
		#endregion
	}
}
