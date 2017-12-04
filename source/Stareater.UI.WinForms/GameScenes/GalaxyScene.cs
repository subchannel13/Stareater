using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using OpenTK;
using Stareater.Controllers;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.Localization;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;
using Stareater.GraphicsEngine;
using Stareater.GLData.SpriteShader;
using Stareater.GLData;
using Stareater.GraphicsEngine.GuiElements;

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
		private const float StarNameZRange = 1 / Layers;
		
		private const float WormholeZ = 9 / Layers;
		private const float PathZ = 8 / Layers;
		private const float StarColorZ = 7 / Layers;
		private const float StarSaturationZ = 6 / Layers;
		private const float StarNameZ = 5 / Layers;
		private const float FleetZ = 4 / Layers;
		private const float SelectionIndicatorZ = 3 / Layers;
		private const float EtaZ = 2 / Layers;
		
		private const float PanClickTolerance = 0.01f;
		private const float ClickRadius = 0.01f;
		private const float TurnTextMargin = 0.005f;
		private const float StarMinClickRadius = 0.75f;
		
		private const float EtaTextScale = 0.25f;
		private const float FleetIndicatorScale = 0.2f;
		private const float FleetSelectorScale = 0.3f;
		private const float PathWidth = 0.1f;
		private const float TurnTextScale = 0.02f;
		private const float StarNameScale = 0.35f;

		public FleetController SelectedFleet { private get; set; }
		private IGalaxyViewListener galaxyViewListener;
		private SignalFlag refreshData = new SignalFlag();
		
		private IEnumerable<SceneObject> fleetMovementPaths = null;
		private IEnumerable<SceneObject> fleetMarkers = null;
		private SceneObject movementEtaText = null;
		private SceneObject movementSimulationPath = null;
		private SceneObject turnCounter = null;
		private SceneObject selectionMarkers = null;
		private SceneObject wormholeSprites = null;
		private IEnumerable<SceneObject> starSprites = null;

		private int zoomLevel = 2;
		private Vector4? lastMousePosition = null;
		private float panAbsPath = 0;
		private Vector2 originOffset = Vector2.Zero;
		private float screenLength;
		private Vector2 mapBoundsMin;
		private Vector2 mapBoundsMax;

		private GalaxySelectionType currentSelection = GalaxySelectionType.None;
		private Dictionary<int, NGenerics.DataStructures.Mathematical.Vector2D> lastSelectedStars = new Dictionary<int, NGenerics.DataStructures.Mathematical.Vector2D>();
		private Dictionary<int, FleetInfo> lastSelectedIdleFleets = new Dictionary<int, FleetInfo>();
		private Dictionary<int, Vector2> lastOffset = new Dictionary<int, Vector2>(); //TODO(v0.7) remember player's zoom level too, unify with last selected object
		private PlayerController currentPlayer = null;

		public GalaxyScene(IGalaxyViewListener galaxyViewListener)
		{ 
			this.galaxyViewListener = galaxyViewListener;

			var turnButton = new GlButton();
			turnButton.Position.
				FixedSize(80, 80).
				ParentRelative(1, 0, 10, 10);
			this.AddElement(turnButton);
		}
		
		public void OnNewTurn()
		{
			this.refreshData.Set();
		}
		
		public void SwitchPlayer(PlayerController player)
		{
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
			this.galaxyViewListener.SystemSelected(this.currentPlayer.OpenStarSystem(this.lastSelectedStar));
			this.setupPerspective();
		}

		private void rebuildCache()
		{
			TextRenderUtil.Get.Prepare(this.currentPlayer.Stars.Select(x => x.Name.ToText(LocalizationManifest.Get.CurrentLanguage)));
		}

		#region AScene implementation
		protected override float GuiLayerThickness => 1 / Layers;

		public override void Activate()
		{
			this.rebuildCache();
			this.setupVaos();
			
			if (this.currentSelection == GalaxySelectionType.Star)
				this.galaxyViewListener.SystemSelected(this.currentPlayer.OpenStarSystem(this.lastSelectedStar));
		}
		
		protected override void FrameUpdate(double deltaTime)
		{
			if (this.refreshData.Check())
			{
				if (this.SelectedFleet != null && !this.SelectedFleet.Valid)
				this.SelectedFleet = null;

				if (this.currentSelection == GalaxySelectionType.Fleet)
					this.currentSelection = GalaxySelectionType.None;
		
				this.rebuildCache();
				this.ResetLists();
			}
		}

		//TODO(v0.7) refactor and remove
		public void ResetLists()
		{
			this.setupVaos();
		}
		#endregion

		#region Drawing setup and helpers
		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			var radius = DefaultViewSize / (float)Math.Pow(ZoomBase, zoomLevel);

			//TODO(later) test this, perhaps by flipping the monitor.
			screenLength = screenSize.X > screenSize.Y ? 
				(float)(screenSize.X * radius * aspect / screenSize.X) : 
				(float)(screenSize.Y * radius * aspect / screenSize.Y);

			this.setupTurnCounter();

			return calcOrthogonalPerspective(aspect * radius, radius, FarZ, originOffset);
		}

		private IEnumerable<Vector2> fleetMovementPathVertices(FleetInfo fleet, IEnumerable<Vector2> waypoints)
		{
			var lastPosition = fleetDisplayPosition(fleet);
			foreach(var nextPosition in waypoints)
			{
				foreach(var v in SpriteHelpers.PathRectVertexData(lastPosition, nextPosition, PathWidth, GalaxyTextures.Get.PathLine))
					yield return v;

				lastPosition = nextPosition;
			}
		}
		
		//TODO(v0.7) rename to fleetPosition
		private Vector2 fleetDisplayPosition(FleetInfo fleet)
		{
			var atStar = this.QueryScene(fleet.Position, 1).Any(x => x.Data is StarData);
			var displayPosition = new Vector2((float)fleet.Position.X, (float)fleet.Position.Y);

			if (!fleet.IsMoving)
			{
				var players = this.currentPlayer.FleetsAt(fleet.Position).
					Select(x => x.Owner).
					Where(x => x != this.currentPlayer.Info).
					Distinct().ToList(); //TODO(v0.7) sort players by some key
				
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

			this.setupFleetMarkers();
			this.setupFleetMovement();
			this.setupMovementEta();
			this.setupMovementSimulation();
			this.setupSelectionMarkers();
			this.setupWormholeSprites();
			this.setupTurnCounter();
        }
		
		private void setupFleetMarkers()
		{
			this.UpdateScene(
				ref this.fleetMarkers,
				this.currentPlayer.Fleets.Select(
					fleet => 
					{
						var displayPosition = fleetDisplayPosition(fleet);

						return new SceneObjectBuilder(fleet, displayPosition, 0).
							StartSimpleSprite(FleetZ, GalaxyTextures.Get.FleetIndicator, fleet.Owner.Color).
							Scale(FleetSelectorScale).
							Translate(displayPosition).
							Build();
					}).ToList()
			);
		}
		
		private void setupFleetMovement()
		{
			this.UpdateScene(
				ref this.fleetMovementPaths,
				this.currentPlayer.Fleets.Where(x => x.IsMoving).Select(fleet =>
					new SceneObjectBuilder().
					StartSprite(PathZ, GalaxyTextures.Get.PathLine.Id, Color.DarkGreen).
					AddVertices(fleetMovementPathVertices(fleet, fleet.Missions.Waypoints.Select(v => convert(v.Destionation)))).
					Build()
				).ToList()
			);
		}
		
		//TODO(v0.7) bundle with movement simulation
		private void setupMovementEta()
		{
			if (this.SelectedFleet != null && this.SelectedFleet.SimulationWaypoints.Count > 0 && this.SelectedFleet.Eta > 0)
			{
				var destination = this.SelectedFleet.SimulationWaypoints[this.SelectedFleet.SimulationWaypoints.Count - 1];
				var numVars = new Var("eta", Math.Ceiling(this.SelectedFleet.Eta)).Get;
				var textVars = new TextVar("eta", new DecimalsFormatter(0, 1).Format(this.SelectedFleet.Eta, RoundingMethod.Ceil, 0)).Get;
				
				this.UpdateScene(
					ref this.movementEtaText,
					new SceneObjectBuilder().
						StartSprite(EtaZ, TextRenderUtil.Get.TextureId, Color.White).
						Scale(EtaTextScale).
						Translate(destination.X, destination.Y + 0.5).
						AddVertices(
							TextRenderUtil.Get.BufferText(
								LocalizationManifest.Get.CurrentLanguage["FormMain"]["FleetEta"].Text(numVars, textVars),
								-0.5f,
								Matrix4.Identity
						)).
						Build()
				);
			}
			else if (this.movementEtaText != null)
				this.RemoveFromScene(ref this.movementEtaText);
		}
		
		private void setupMovementSimulation()
		{
			if (this.SelectedFleet != null && this.SelectedFleet.SimulationWaypoints.Count > 0)
				this.UpdateScene(
					ref this.movementSimulationPath,
					new SceneObjectBuilder().
						StartSprite(PathZ, GalaxyTextures.Get.PathLine.Id, Color.LimeGreen).
						AddVertices(fleetMovementPathVertices(this.SelectedFleet.Fleet, this.SelectedFleet.SimulationWaypoints.Select(v => convert(v)))).
						Build()
				);
			else if (this.movementSimulationPath != null)
				this.RemoveFromScene(ref this.movementSimulationPath);
		}

		private void setupTurnCounter()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			var zoom = (float)Math.Pow(ZoomBase, zoomLevel);
            var radius = DefaultViewSize / zoom;
			var uiScale = screenLength;
			var transform =
					Matrix4.CreateScale(TurnTextScale * uiScale) *
					Matrix4.CreateTranslation(
						aspect * radius / 2 + originOffset.X - uiScale * TurnTextMargin, 
						radius / 2 + originOffset.Y - uiScale * TurnTextMargin, 
						0
					);

			this.UpdateScene(
				ref this.turnCounter,
				new SceneObjectBuilder().
					StartSprite(EtaZ, TextRenderUtil.Get.TextureId, Color.LightGray).
					Transform(transform).
					AddVertices(
						TextRenderUtil.Get.BufferText(
							LocalizationManifest.Get.CurrentLanguage["FormMain"]["Turn"].Text() + " " + this.currentPlayer.Turn,
							-1,
							Matrix4.Identity
					)).
					Build()
			);
		}

		private void setupSelectionMarkers()
		{
			var transform = new Matrix4();

			if (this.currentSelection == GalaxySelectionType.Star)
				transform = Matrix4.CreateTranslation((float)this.lastSelectedStarPosition.X, (float)this.lastSelectedStarPosition.Y, 0);
			else if (this.currentSelection == GalaxySelectionType.Fleet)
			{
				var displayPosition = this.fleetDisplayPosition(this.lastSelectedIdleFleet);
				transform = Matrix4.CreateScale(FleetSelectorScale) * Matrix4.CreateTranslation(displayPosition.X, displayPosition.Y, 0);
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

			this.UpdateScene(
				ref this.starSprites,
				stars.Select((star, i) => new SceneObjectBuilder(star, convert(star.Position), 0).
					StartSimpleSprite(StarColorZ, GalaxyTextures.Get.StarColor, star.Color).
					Translate(convert(star.Position)).

					StartSimpleSprite(StarSaturationZ, GalaxyTextures.Get.StarGlow, Color.White).
					Translate(convert(star.Position)).

					//TODO(v0.7) scale star names with zoom level
					StartSprite(StarNameZ - (i * StarNameZRange) / stars.Count, TextRenderUtil.Get.TextureId, starNameColor(star)).
					AddVertices(
						TextRenderUtil.Get.BufferText(
								star.Name.ToText(LocalizationManifest.Get.CurrentLanguage),
								-0.5f,
								Matrix4.CreateScale(StarNameScale) * Matrix4.CreateTranslation((float)star.Position.X, (float)star.Position.Y - 0.5f, 0)
					)).
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
						this.currentPlayer.Wormholes.SelectMany(wormhole => SpriteHelpers.PathRectVertexData(
							convert(wormhole.FromStar.Position),
							convert(wormhole.ToStar.Position),
							0.8f * PathWidth,
							GalaxyTextures.Get.PathLine
					))).
					Build()
			);
		}
		#endregion
		
		#region Mouse events
		public override void OnMouseMove(MouseEventArgs e)
		{
			Vector4 currentPosition = mouseToView(e.X, e.Y);

			if (!lastMousePosition.HasValue)
				lastMousePosition = currentPosition;

			if (e.Button.HasFlag(MouseButtons.Left)) 
				mousePan(currentPosition);
			else {
				lastMousePosition = currentPosition;
				panAbsPath = 0;
				
				if (this.SelectedFleet != null)
					simulateFleetMovement(currentPosition);
			}
		}
		
		private void mousePan(Vector4 currentPosition)
		{
			panAbsPath += (currentPosition - lastMousePosition.Value).Length;

			originOffset -= (Vector4.Transform(currentPosition, invProjection) -
				Vector4.Transform(lastMousePosition.Value, invProjection)
				).Xy;

			limitPan();
			
			lastMousePosition = currentPosition;
			this.setupPerspective();
		}
		
		private void simulateFleetMovement(Vector4 currentPosition)
		{
			if (!this.SelectedFleet.CanMove)
				return;
			
			Vector4 mousePoint = Vector4.Transform(currentPosition, invProjection);
			var searchRadius = Math.Max(screenLength * ClickRadius / Math.Pow(ZoomBase, zoomLevel), StarMinClickRadius);
			var searchPoint = new NGenerics.DataStructures.Mathematical.Vector2D(mousePoint.X, mousePoint.Y);
			var searchSize = new NGenerics.DataStructures.Mathematical.Vector2D(searchRadius, searchRadius);

			var starsFound = this.QueryScene(searchPoint, searchRadius).
				Where(x => x.Data is StarData).
				OrderBy(x => (x.PhysicalShape.Center - convert(searchPoint)).LengthSquared).
				ToList();

			if (!starsFound.Any())
				return;

			this.SelectedFleet.SimulateTravel(starsFound[0].Data as StarData);
			this.setupMovementEta();
			this.setupMovementSimulation();
		}

		public override void OnMouseScroll(MouseEventArgs e)
		{
			float oldZoom = 1 / (float)(0.5 * DefaultViewSize / Math.Pow(ZoomBase, zoomLevel));

			if (e.Delta > 0)
				zoomLevel++;
			else 
				zoomLevel--;

			zoomLevel = Methods.Clamp(zoomLevel, MinZoom, MaxZoom);

			float newZoom = 1 / (float)(0.5 * DefaultViewSize / Math.Pow(ZoomBase, zoomLevel));
			Vector2 mousePoint = Vector4.Transform(mouseToView(e.X, e.Y), invProjection).Xy;

			originOffset = (originOffset * oldZoom + mousePoint * (newZoom - oldZoom)) / newZoom;
			limitPan();
			this.setupPerspective();
		}

		public override void OnMouseClick(MouseEventArgs e)
		{
			if (panAbsPath > PanClickTolerance) //TODO(v0.7) maybe make AScene differentiate between click and drag
				return;
			
			Vector4 mousePoint = Vector4.Transform(mouseToView(e.X, e.Y), invProjection);
			var searchRadius = Math.Max(screenLength * ClickRadius / Math.Pow(ZoomBase, zoomLevel), StarMinClickRadius);
			var searchPoint = convert(mousePoint.Xy);
			var searchSize = new NGenerics.DataStructures.Mathematical.Vector2D(searchRadius, searchRadius);
			
			var allObjects = this.QueryScene(searchPoint, searchRadius).
				OrderBy(x => (x.PhysicalShape.Center - convert(searchPoint)).LengthSquared).
				ToList();
			var starsFound = allObjects.Where(x => x.Data is StarData).Select(x => x.Data as StarData).ToList();
			var fleetFound = allObjects.Where(x => x.Data is FleetInfo).Select(x => x.Data as FleetInfo).ToList();
			
			var foundAny = starsFound.Any() || fleetFound.Any();
			var isStarClosest = 
				starsFound.Any() && 
				(
					!fleetFound.Any() ||
					(starsFound[0].Position - searchPoint).Magnitude() <= (fleetFound[0].Position - searchPoint).Magnitude()
				);

			if (this.SelectedFleet != null)
				if (foundAny && isStarClosest)
				{
					this.SelectedFleet = this.SelectedFleet.Send(this.SelectedFleet.SimulationWaypoints);
					this.lastSelectedIdleFleets[this.currentPlayer.PlayerIndex] = this.SelectedFleet.Fleet;
					this.galaxyViewListener.FleetClicked(new FleetInfo[] { this.SelectedFleet.Fleet });
					this.setupFleetMarkers();
					this.setupFleetMovement();
					this.setupSelectionMarkers();
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
				this.galaxyViewListener.SystemSelected(this.currentPlayer.OpenStarSystem(starsFound[0]));
				this.setupSelectionMarkers();
			}
			else
			{
				this.currentSelection = GalaxySelectionType.Fleet;
				this.lastSelectedIdleFleets[this.currentPlayer.PlayerIndex] = fleetFound[0]; //TODO(v0.7) marks wrong fleet when there are multiple players 
				this.galaxyViewListener.FleetClicked(fleetFound);
				this.setupSelectionMarkers();
			}
			
		}
		
		public override void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (panAbsPath > PanClickTolerance)
				return;
			
			Vector4 mousePoint = Vector4.Transform(mouseToView(e.X, e.Y), invProjection);
			var searchRadius = Math.Max(screenLength * ClickRadius / Math.Pow(ZoomBase, zoomLevel), StarMinClickRadius);
			var searchPoint = new NGenerics.DataStructures.Mathematical.Vector2D(mousePoint.X, mousePoint.Y);
			var searchSize = new NGenerics.DataStructures.Mathematical.Vector2D(searchRadius, searchRadius);

			var starsFound = this.QueryScene(searchPoint, searchRadius).
				Where(x => x.Data is StarData).
				Select(x => x.Data as StarData).
				OrderBy(x => (x.Position - searchPoint).Magnitude()).
				ToList();

			if (starsFound.Any())
				this.galaxyViewListener.SystemOpened(this.currentPlayer.OpenStarSystem(starsFound[0]));
		}
		#endregion
		
		#region Helper methods
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

		private static Matrix4 pathMatrix(Vector2 fromPoint, Vector2 toPoint)
		{
			var xAxis = toPoint - fromPoint;
			var yAxis = new Vector2(xAxis.Y, -xAxis.X);
			var yScale = PathWidth / yAxis.Length;
			
			var center = (fromPoint + toPoint) / 2;
			return new Matrix4(
				xAxis.X, yAxis.X, 0, 0,
				xAxis.Y * yScale, yAxis.Y * yScale, 0, 0,
				0, 0, 1, 0,
				center.X, center.Y, 0, 1
			);
		}
		
		private FleetInfo lastSelectedIdleFleet
		{
			get 
			{
				return this.lastSelectedIdleFleets[this.currentPlayer.PlayerIndex];
			}
		}
		
		private StarData lastSelectedStar
		{
			get 
			{
				return this.currentPlayer.Star(this.lastSelectedStars[this.currentPlayer.PlayerIndex]);
			}
		}
		
		//TODO(v0.7) remove one of lastSelectedStar methods
		private NGenerics.DataStructures.Mathematical.Vector2D lastSelectedStarPosition
		{
			get 
			{
				return this.lastSelectedStars[this.currentPlayer.PlayerIndex];
			}
		}
		
		private Color starNameColor(StarData star)
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
			var stellarises = this.currentPlayer.Stellarises();
            var bestStar = stellarises.Any() ? 
				stellarises.Aggregate((a, b) => a.Population > b.Population ? a : b) : 
				stellarises.First();

			this.lastSelectedStars[this.currentPlayer.PlayerIndex] = bestStar.HostStar.Position;
			this.lastOffset[this.currentPlayer.PlayerIndex] = convert(bestStar.HostStar.Position);
		}
		#endregion
	}
}
