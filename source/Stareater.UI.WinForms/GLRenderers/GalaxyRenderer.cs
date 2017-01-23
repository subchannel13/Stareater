using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using OpenTK;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.Localization;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;
using Stareater.GLData;
using Stareater.GraphicsEngine;
using Stareater.GLData.SpriteShader;

namespace Stareater.GLRenderers
{
	class GalaxyRenderer : AScene
	{
		private const float DefaultViewSize = 15;
		private const double ZoomBase = 1.2f;
		private const int MaxZoom = 10;
		private const int MinZoom = -10;
		
		private const float FarZ = 1;
		private const float Layers = 16.0f;
		private const float StarNameZRange = 1 / Layers;
		
		private const float WormholeZ = 8 / Layers;
		private const float PathZ = 7 / Layers;
		private const float StarColorZ = 6 / Layers;
		private const float StarSaturationZ = 5 / Layers;
		private const float StarNameZ = 4 / Layers;
		private const float FleetZ = 3 / Layers;
		private const float SelectionIndicatorZ = 2 / Layers;
		private const float EtaZ = 1 / Layers;
		
		private const float PanClickTolerance = 0.01f;
		private const float ClickRadius = 0.02f;
		private const float StarMinClickRadius = 0.6f;
		
		private const float EtaTextScale = 0.25f;
		private const float FleetIndicatorScale = 0.2f;
		private const float FleetSelectorScale = 0.3f;
		private const float PathWidth = 0.1f;
		private const float StarNameScale = 0.35f;

		public FleetController SelectedFleet { private get; set; }
		private IGalaxyViewListener galaxyViewListener;
		private SignalFlag refreshData = new SignalFlag();
		
		private SceneObject wormholeSprites = null;
		private IEnumerable<SceneObject> starSprites = null;
		private TextDrawable etaTextSprites = null;

		private int zoomLevel = 2;
		private Vector4? lastMousePosition = null;
		private float panAbsPath = 0;
		private Vector2 originOffset = Vector2.Zero;
		private float screenLength;
		private Vector2 mapBoundsMin;
		private Vector2 mapBoundsMax;

		private QuadTree<StarData> stars = new QuadTree<StarData>();
		private QuadTree<FleetInfo> fleetsReal = new QuadTree<FleetInfo>();
		private QuadTree<FleetInfo> fleetsDisplayed = new QuadTree<FleetInfo>(); //TODO(v0.6) try to remove either this member or fleetPositions
		private Dictionary<FleetInfo, NGenerics.DataStructures.Mathematical.Vector2D> fleetPositions = new Dictionary<FleetInfo, NGenerics.DataStructures.Mathematical.Vector2D>();
		
		private GalaxySelectionType currentSelection = GalaxySelectionType.None;
		private Dictionary<int, NGenerics.DataStructures.Mathematical.Vector2D> lastSelectedStars = new Dictionary<int, NGenerics.DataStructures.Mathematical.Vector2D>();
		private Dictionary<int, FleetInfo> lastSelectedIdleFleets = new Dictionary<int, FleetInfo>();
		private PlayerController currentPlayer = null;

		public GalaxyRenderer(IGalaxyViewListener galaxyViewListener)
		{ 
			this.galaxyViewListener = galaxyViewListener;
		}
		
		public PlayerController CurrentPlayer
		{
			set
			{
				this.currentPlayer = value;
				
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
				
				if (!this.lastSelectedStars.ContainsKey(this.currentPlayer.PlayerIndex))
				{
					var bestStar = this.currentPlayer.Stellarises().Aggregate((a, b) => a.Population > b.Population ? a : b);

					this.lastSelectedStars.Add(this.currentPlayer.PlayerIndex, bestStar.HostStar.Position);
					this.originOffset = new Vector2((float)this.lastSelectedStar.Position.X, (float)this.lastSelectedStar.Position.Y);
					this.currentSelection = GalaxySelectionType.Star;
					this.galaxyViewListener.SystemSelected(this.currentPlayer.OpenStarSystem(this.lastSelectedStar));
				}
			}
		}
		
		public void OnNewTurn()
		{
			this.refreshData.Set();
		}

		private void cacheFleet(FleetInfo fleet, bool atStar)
		{
			NGenerics.DataStructures.Mathematical.Vector2D displayPosition = fleet.Position;

			if (!fleet.IsMoving)
			{
				var players = this.fleetsReal.
					Query(fleet.Position, new NGenerics.DataStructures.Mathematical.Vector2D(0, 0)).
					Select(x => x.Owner).
					Where(x => x != this.currentPlayer.Info).
					Distinct().ToList(); //TODO(v0.6) sort players by some key
				
				int index = (fleet.Owner == this.currentPlayer.Info) ? 0 : (1 + players.IndexOf(fleet.Owner));
				displayPosition += new NGenerics.DataStructures.Mathematical.Vector2D(0.5, 0.5 - 0.2 * index);
			}
			else if (fleet.IsMoving && atStar)
				displayPosition += new NGenerics.DataStructures.Mathematical.Vector2D(-0.5, 0.5);

			this.fleetPositions.Add(fleet, displayPosition);
			this.fleetsDisplayed.Add(fleet, displayPosition, new NGenerics.DataStructures.Mathematical.Vector2D(0, 0));
		}

		private void rebuildCache()
		{
			TextRenderUtil.Get.Prepare(this.currentPlayer.Stars.Select(x => x.Name.ToText(LocalizationManifest.Get.CurrentLanguage)));

			this.stars.Clear();
			foreach (var star in this.currentPlayer.Stars)
				this.stars.Add(star, star.Position, new NGenerics.DataStructures.Mathematical.Vector2D(0, 0));
			
			this.fleetsReal.Clear();
			foreach(var fleet in this.currentPlayer.Fleets)
				this.fleetsReal.Add(fleet, fleet.Position, new NGenerics.DataStructures.Mathematical.Vector2D(0, 0));

			this.fleetsDisplayed.Clear();
			this.fleetPositions.Clear();
			foreach(var fleet in this.currentPlayer.Fleets)
			{
				bool atStar = this.stars.Query(fleet.Position, new NGenerics.DataStructures.Mathematical.Vector2D(1, 1)).Any();
				this.cacheFleet(fleet, atStar);
			}
		}
		
		private void updateFleetCache(NGenerics.DataStructures.Mathematical.Vector2D position)
		{
			var toRemove = this.fleetsReal.Query(position, new NGenerics.DataStructures.Mathematical.Vector2D(0, 0)).ToList();
			foreach(var fleet in toRemove)
			{
				this.fleetPositions.Remove(fleet);
				this.fleetsDisplayed.Remove(fleet);
				this.fleetsReal.Remove(fleet);
			}
			
			var toAdd = this.currentPlayer.FleetsAt(position).ToList();
			foreach(var fleet in toAdd)
				this.fleetsReal.Add(fleet, fleet.Position, new NGenerics.DataStructures.Mathematical.Vector2D(0, 0));
			
			bool atStar = this.stars.Query(position, new NGenerics.DataStructures.Mathematical.Vector2D(1, 1)).Any();
			foreach(var fleet in this.currentPlayer.FleetsAt(position))
				this.cacheFleet(fleet, atStar);
		}
		
		#region ARenderer implementation
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
			
			drawFleetMovement();
			drawMovementSimulation();
			drawFleetMarkers();
			drawSelectionMarkers();
			drawMovementEta();
		}

		//TODO(0.6) refactor and remove
		public void ResetLists()
		{
			this.setupVaos();
		}
		#endregion

		#region Drawing setup and helpers
		private void drawFleetMarkers()
		{
			foreach (var fleet in this.fleetPositions)
				TextureUtils.DrawSprite(
					GalaxyTextures.Get.FleetIndicator, 
					this.projection, 
					Matrix4.CreateScale(FleetSelectorScale) * Matrix4.CreateTranslation((float)fleet.Value.X, (float)fleet.Value.Y, 0),
					FleetZ, 
					fleet.Key.Owner.Color
				);
		}

		private void drawFleetMovement()
		{
			foreach (var fleetPos in this.fleetPositions) {
				if (!fleetPos.Key.IsMoving)
					continue;

				var lastPosition = fleetPos.Value;
				foreach(var waypoint in fleetPos.Key.Missions.Waypoints)
				{
					TextureUtils.DrawSprite(
						GalaxyTextures.Get.PathLine, 
						this.projection, 
						pathMatrix(
							new Vector2((float)lastPosition.X, (float)lastPosition.Y),
							new Vector2((float)waypoint.Destionation.X, (float)waypoint.Destionation.Y)
						),
						PathZ, 
						Color.DarkGreen
					);
					
					lastPosition = waypoint.Destionation;
				}
			}
		}
		
		private void drawMovementEta()
		{
			if (this.SelectedFleet != null && this.SelectedFleet.SimulationWaypoints.Count > 0)
			{
				if (this.SelectedFleet.Eta > 0)
				{
					var destination = this.SelectedFleet.SimulationWaypoints[this.SelectedFleet.SimulationWaypoints.Count - 1];
					var numVars = new Var("eta", Math.Ceiling(this.SelectedFleet.Eta)).Get;
					var textVars = new TextVar("eta", new DecimalsFormatter(0, 1).Format(this.SelectedFleet.Eta, RoundingMethod.Ceil, 0)).Get;
					var transform = 
						Matrix4.CreateScale(EtaTextScale) * 
						Matrix4.CreateTranslation((float)destination.X, (float)destination.Y + 0.5f, 0);
					
					if (this.etaTextSprites == null)
						this.etaTextSprites = new TextDrawable(
							new SpriteData(
								new Matrix4(),
								EtaZ,
								TextRenderUtil.Get.TextureId,
								Color.White
							),
							-0.5f
						);
					
					this.etaTextSprites.ObjectData.LocalTransform = transform;
					this.etaTextSprites.Draw(this.projection, LocalizationManifest.Get.CurrentLanguage["FormMain"]["FleetEta"].Text(numVars, textVars));
				}
			}
		}
		
		private void drawMovementSimulation()
		{
			if (this.SelectedFleet != null && this.SelectedFleet.SimulationWaypoints.Count > 0)
			{
				var last = this.fleetPositions[this.SelectedFleet.Fleet];
				foreach (var next in this.SelectedFleet.SimulationWaypoints) 
				{
					TextureUtils.DrawSprite(
						GalaxyTextures.Get.PathLine, 
						this.projection, 
						pathMatrix(
							new Vector2((float)last.X, (float)last.Y),
							new Vector2((float)next.X, (float)next.Y)
						),
						PathZ, 
						Color.LimeGreen
					);
					
					last = next;
				}
			}
		}
		
		private void drawSelectionMarkers()
		{
			var transform = new Matrix4();
			
			if (this.currentSelection == GalaxySelectionType.Star)
				transform = Matrix4.CreateTranslation((float)this.lastSelectedStarPosition.X, (float)this.lastSelectedStarPosition.Y, 0);
			else if (this.currentSelection == GalaxySelectionType.Fleet) 
			{
				var markerPosition = this.fleetPositions[this.lastSelectedIdleFleet];
				transform = Matrix4.CreateScale(FleetSelectorScale) * Matrix4.CreateTranslation((float)markerPosition.X, (float)markerPosition.Y, 0);
			}
			
			TextureUtils.DrawSprite(
				GalaxyTextures.Get.SelectedStar, 
				this.projection, 
				transform,
				SelectionIndicatorZ, 
				Color.White
			);
		}
		
		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			var radius = DefaultViewSize / (float)Math.Pow(ZoomBase, zoomLevel);

			//TODO(later): test this, perhaps by flipping the monitor.
			screenLength = screenSize.X > screenSize.Y ? 
				(float)(screenSize.X * radius * aspect / screenSize.X) : 
				(float)(screenSize.Y * radius * aspect / screenSize.Y);

			return calcOrthogonalPerspective(aspect * radius, radius, FarZ, originOffset);
		}
		
		private void setupVaos()
		{
			this.setupStarSprites();
			this.setupWormholeSprites();
		}
		
		private void setupStarSprites()
		{
			this.UpdateScene(
				ref this.starSprites,
				this.currentPlayer.Stars.Select(star => new SceneObject(
					new []{
						new PolygonData(
							StarColorZ,
							new SpriteData(Matrix4.Identity, StarColorZ, GalaxyTextures.Get.StarColor.Texture.Id, star.Color),
							SpriteHelpers.TexturedRectVertexData(convert(star.Position), 1, 1, GalaxyTextures.Get.StarColor.Texture)
						),
						new PolygonData(
							StarSaturationZ,
							new SpriteData(Matrix4.Identity, StarSaturationZ, GalaxyTextures.Get.StarGlow.Texture.Id, Color.White),
							SpriteHelpers.TexturedRectVertexData(convert(star.Position), 1, 1, GalaxyTextures.Get.StarGlow.Texture)
						),
						new PolygonData(
							StarNameZ,
							new SpriteData(Matrix4.Identity, StarNameZ, TextRenderUtil.Get.TextureId, starNameColor(star)),
							TextRenderUtil.Get.BufferText(
								star.Name.ToText(LocalizationManifest.Get.CurrentLanguage), 
								-0.5f, 
								Matrix4.CreateScale(StarNameScale) * Matrix4.CreateTranslation((float)star.Position.X, (float)star.Position.Y - 0.5f, 0)
							).ToList()
						)
					})));
		}
		
		private void setupWormholeSprites()
		{
			this.UpdateScene(
				ref this.wormholeSprites,
				new SceneObject(new[] {
					new PolygonData(
						WormholeZ,
						new SpriteData(
							Matrix4.Identity, WormholeZ, GalaxyTextures.Get.PathLine.Texture.Id, Color.Blue
						),
						this.currentPlayer.Wormholes.SelectMany(wormhole => SpriteHelpers.PathRectVertexData(
							convert(wormhole.FromStar.Position),
							convert(wormhole.ToStar.Position),
							0.8f * PathWidth,
							GalaxyTextures.Get.PathLine.Texture
						))
					)
				})
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
			var searchRadius = Math.Max(screenLength * ClickRadius, StarMinClickRadius); //TODO(v0.6) doesn't scale with zoom
			var searchPoint = new NGenerics.DataStructures.Mathematical.Vector2D(mousePoint.X, mousePoint.Y);
			var searchSize = new NGenerics.DataStructures.Mathematical.Vector2D(searchRadius, searchRadius);

			var starsFound = this.stars.Query(searchPoint, searchSize).OrderBy(x => (x.Position - searchPoint).Magnitude()).ToList();

			if (!starsFound.Any())
				return;

			this.SelectedFleet.SimulateTravel(starsFound[0]);
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
			if (panAbsPath > PanClickTolerance) //TODO(v0.6) maybe make AScene differentiate between click and drag
				return;
			
			Vector4 mousePoint = Vector4.Transform(mouseToView(e.X, e.Y), invProjection);
			var searchRadius = Math.Max(screenLength * ClickRadius, StarMinClickRadius); //TODO(v0.6) doesn't scale with zoom
			var searchPoint = new NGenerics.DataStructures.Mathematical.Vector2D(mousePoint.X, mousePoint.Y);
			var searchSize = new NGenerics.DataStructures.Mathematical.Vector2D(searchRadius, searchRadius);
			
			var starsFound = this.stars.Query(searchPoint, searchSize).OrderBy(x => (x.Position - searchPoint).Magnitude()).ToList();
			var fleetFound = this.fleetsDisplayed.Query(searchPoint, searchSize).OrderBy(x => (x.Position - searchPoint).Magnitude()).ToList();
			
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
					this.updateFleetCache(this.SelectedFleet.Fleet.Position);
					return;
				}
				else
				{
					this.galaxyViewListener.FleetDeselected();
					this.SelectedFleet = null;
				}


			if (!foundAny)
				return;
			
			if (isStarClosest)
			{
				this.currentSelection = GalaxySelectionType.Star;
				this.lastSelectedStars[this.currentPlayer.PlayerIndex] = starsFound[0].Position;
				this.galaxyViewListener.SystemSelected(this.currentPlayer.OpenStarSystem(starsFound[0]));
			}
			else
			{
				this.currentSelection = GalaxySelectionType.Fleet;
				this.lastSelectedIdleFleets[this.currentPlayer.PlayerIndex] = fleetFound[0]; //TODO(v0.6) marks wrong fleet when there are multiple players 
				this.galaxyViewListener.FleetClicked(fleetFound);
			}
			
		}
		
		public override void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (panAbsPath > PanClickTolerance)
				return;
			
			Vector4 mousePoint = Vector4.Transform(mouseToView(e.X, e.Y), invProjection);
			var searchRadius = Math.Max(screenLength * ClickRadius, StarMinClickRadius); //TODO(v0.6) doesn't scale with zoom
			var searchPoint = new NGenerics.DataStructures.Mathematical.Vector2D(mousePoint.X, mousePoint.Y);
			var searchSize = new NGenerics.DataStructures.Mathematical.Vector2D(searchRadius, searchRadius);

			var starsFound = this.stars.Query(searchPoint, searchSize).OrderBy(x => (x.Position - searchPoint).Magnitude()).ToList();

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

		private static Vector2 convert(NGenerics.DataStructures.Mathematical.Vector2D v)
		{
			return new Vector2((float)v.X, (float)v.Y);
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
		
		//TODO(v0.6) remove one of lastSelectedStar methods
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
		#endregion
	}
}
