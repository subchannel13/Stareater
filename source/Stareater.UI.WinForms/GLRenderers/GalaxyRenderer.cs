using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;
using Stareater.GraphicsEngine;

namespace Stareater.GLRenderers
{
	class GalaxyRenderer : AScene
	{
		private const double DefaultViewSize = 15;
		private const double ZoomBase = 1.2f;
		private const int MaxZoom = 10;
		private const int MinZoom = -10;
		
		private const float FarZ = -1;
		private const float Layers = 16.0f;
		private const float StarNameZRange = 1 / Layers;
		
		private const float WormholeZ = -8 / Layers;
		private const float PathZ = -7 / Layers;
		private const float StarColorZ = -6 / Layers;
		private const float StarSaturationZ = -5 / Layers;
		private const float StarNameZ = -4 / Layers;
		private const float FleetZ = -3 / Layers;
		private const float SelectionIndicatorZ = -2 / Layers;
		private const float EtaZ = -1 / Layers;
		
		private const float PanClickTolerance = 0.01f;
		private const float ClickRadius = 0.02f;
		private const float StarMinClickRadius = 0.6f;
		
		private const float EtaTextScale = 0.25f;
		private const float FleetIndicatorScale = 0.2f;
		private const float FleetSelectorScale = 0.3f;
		private const double PathWidth = 0.1;
		private const float StarNameScale = 0.35f;

		public FleetController SelectedFleet { private get; set; }
		private IGalaxyViewListener galaxyViewListener;
		private SignalFlag refreshData = new SignalFlag();
		
		private Matrix4 invProjection;
		private int starDrawList = NoCallList;
		private int wormholeDrawList = NoCallList;

		private int zoomLevel = 2;
		private Vector4? lastMousePosition = null;
		private float panAbsPath = 0;
		private Vector2 originOffset = Vector2.Zero;
		private float screenLength;
		private Vector2 mapBoundsMin;
		private Vector2 mapBoundsMax;

		private QuadTree<StarData> stars = new QuadTree<StarData>();
		private QuadTree<FleetInfo> fleets = new QuadTree<FleetInfo>();
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
				
				if (this.currentPlayer.VisualPositioner == null)
					this.currentPlayer.VisualPositioner = new VisualPositioner();
				
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
		
		private void rebuildCache()
		{
			this.stars.Clear();
			foreach(var star in this.currentPlayer.Stars)
				this.stars.Add(star, star.Position, new NGenerics.DataStructures.Mathematical.Vector2D(0, 0));
			
			this.fleets.Clear();
		}
		
		#region ARenderer implementation
		public override void Activate()
		{
			this.rebuildCache();
			
			if (this.currentSelection == GalaxySelectionType.Star)
				this.galaxyViewListener.SystemSelected(this.currentPlayer.OpenStarSystem(this.lastSelectedStar));
		}
		
		public override void Deactivate()
		{
			if (starDrawList >= 0){
				GL.DeleteLists(starDrawList, 1);
				starDrawList = -1;
			}
		}
		
		public override void Draw(double deltaTime)
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
			
			drawList(wormholeDrawList, setupWormholeList);
			drawFleetMovement();
			drawMovementSimulation();
			drawList(starDrawList, setupStarsList);
			drawFleetMarkers();
			drawSelectionMarkers();
			drawMovementEta();
		}

		public override void ResetLists()
		{
			GL.DeleteLists(starDrawList, 1);
			GL.DeleteLists(wormholeDrawList, 1);
			this.starDrawList = NoCallList;
			this.wormholeDrawList = NoCallList;
		}
		#endregion

		#region Drawing setup and helpers
		private void drawFleetMarkers()
		{
			foreach (var fleet in this.currentPlayer.Fleets) {
				GL.Color4(fleet.Owner.Color);
				
				GL.PushMatrix();
				GL.Translate(fleet.VisualPosition.X, fleet.VisualPosition.Y, FleetZ);
				GL.Scale(FleetIndicatorScale, FleetIndicatorScale, FleetIndicatorScale);

				TextureUtils.DrawSprite(GalaxyTextures.Get.FleetIndicator);
				GL.PopMatrix();
			}
		}

		private void drawFleetMovement()
		{
			foreach (var fleet in this.currentPlayer.Fleets) {
				if (!fleet.IsMoving)
					continue;
				
				var lastPosition = fleet.VisualPosition;
				GL.Color4(Color.DarkGreen);
				
				foreach(var waypoint in fleet.Missions.Waypoints)
				{
					GL.PushMatrix();
					GL.MultMatrix(pathMatrix(
						new Vector2d(lastPosition.X, lastPosition.Y),
						new Vector2d(waypoint.Destionation.X, waypoint.Destionation.Y)
					));
						
					TextureUtils.DrawSprite(GalaxyTextures.Get.PathLine, PathZ);
					GL.PopMatrix();
					
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
					
					GL.PushMatrix();
					GL.Translate(destination.X, destination.Y + 0.5, EtaZ);
					GL.Scale(EtaTextScale, EtaTextScale, EtaTextScale);
		
					TextRenderUtil.Get.RenderText(SettingsWinforms.Get.Language["FormMain"]["FleetEta"].Text(numVars, textVars), -0.5f);
					GL.PopMatrix();
				}
			}
		}
		
		private void drawMovementSimulation()
		{
			if (this.SelectedFleet != null && this.SelectedFleet.SimulationWaypoints.Count > 0)
			{
				GL.Enable(EnableCap.Texture2D);
				GL.Color4(Color.LimeGreen);
				
				var last = this.SelectedFleet.Fleet.VisualPosition;
				foreach (var next in this.SelectedFleet.SimulationWaypoints) {
					GL.PushMatrix();
					GL.MultMatrix(pathMatrix(new Vector2d(last.X, last.Y), new Vector2d(next.X, next.Y)));
					TextureUtils.DrawSprite(GalaxyTextures.Get.PathLine, PathZ);
					GL.PopMatrix();
					last = next;
				}
			}
		}
		
		private void drawSelectionMarkers()
		{
			if (this.currentSelection == GalaxySelectionType.Star) {
				GL.Color4(Color.White);
				GL.PushMatrix();
				GL.Translate(this.lastSelectedStarPosition.X, this.lastSelectedStarPosition.Y, SelectionIndicatorZ);

				TextureUtils.DrawSprite(GalaxyTextures.Get.SelectedStar);
				GL.PopMatrix();
			}
			
			if (this.currentSelection == GalaxySelectionType.Fleet) {
				GL.Color4(Color.White);
				GL.PushMatrix();
				GL.Translate(this.lastSelectedIdleFleet.VisualPosition.X, this.lastSelectedIdleFleet.VisualPosition.Y, SelectionIndicatorZ);
				GL.Scale(FleetSelectorScale, FleetSelectorScale, FleetSelectorScale);

				TextureUtils.DrawSprite(GalaxyTextures.Get.SelectedStar);
				GL.PopMatrix();
			}
		}
		
		protected override void setupPerspective()
		{
			double aspect = canvasSize.X / (double)canvasSize.Y;
			double semiRadius = 0.5 * DefaultViewSize / Math.Pow(ZoomBase, zoomLevel);

			//TODO(later): test this, perhaps by flipping the monitor.
			screenLength = screenSize.X > screenSize.Y ? 
				(float)(2 * screenSize.X * semiRadius * aspect / screenSize.X) : 
				(float)(2 * screenSize.Y * semiRadius * aspect / screenSize.Y);
			
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(
				-aspect * semiRadius + originOffset.X, aspect * semiRadius + originOffset.X,
				-semiRadius + originOffset.Y, semiRadius + originOffset.Y, 
				0, -FarZ);

			GL.GetFloat(GetPName.ProjectionMatrix, out invProjection);
			invProjection.Invert();
			GL.MatrixMode(MatrixMode.Modelview);
		}
		
		private void setupStarsList()
		{
			this.starDrawList = GL.GenLists(1);
			GL.NewList(starDrawList, ListMode.CompileAndExecute);

			GL.Enable(EnableCap.Texture2D);
			
			foreach (var star in this.stars.GetAll()) {
				GL.Color4(star.Color);
				GL.PushMatrix();
				GL.Translate(star.Position.X, star.Position.Y, StarColorZ);

				TextureUtils.DrawSprite(GalaxyTextures.Get.StarColor);
			
				GL.Color4(Color.White);
				TextureUtils.DrawSprite(GalaxyTextures.Get.StarGlow, StarSaturationZ - StarColorZ);
			
				GL.PopMatrix();
			}
			
			float starNameZ = StarNameZ;
			foreach (var star in this.currentPlayer.Stars) {
				GL.Color4(starNameColor(star));
				
				GL.PushMatrix();
				GL.Translate(star.Position.X, star.Position.Y - 0.5, starNameZ);
				GL.Scale(StarNameScale, StarNameScale, StarNameScale);

				TextRenderUtil.Get.RenderText(star.Name.ToText(SettingsWinforms.Get.Language), -0.5f);
				GL.PopMatrix();
				starNameZ += StarNameZRange / this.currentPlayer.StarCount;
			}

			GL.EndList();
		}
		
		private void setupWormholeList()
		{
			this.wormholeDrawList = GL.GenLists(1);
			GL.NewList(wormholeDrawList, ListMode.CompileAndExecute);
			
			GL.Enable(EnableCap.Texture2D);
			GL.Color4(Color.Blue);
			
			foreach (var wormhole in this.currentPlayer.Wormholes) {
				GL.PushMatrix();
				GL.MultMatrix(pathMatrix(
					new Vector2d(wormhole.FromStar.Position.X, wormhole.FromStar.Position.Y), 
					new Vector2d(wormhole.ToStar.Position.X, wormhole.ToStar.Position.Y)
				));
				GL.Scale(1, 0.8, 1);
				
				TextureUtils.DrawSprite(GalaxyTextures.Get.PathLine, WormholeZ);
				
				GL.PopMatrix();
			}
			
			GL.EndList();
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
			var closestObjects = this.currentPlayer.FindClosest(
				mousePoint.X, mousePoint.Y, 
				Math.Max(screenLength * ClickRadius, StarMinClickRadius));
			
			if (closestObjects.Stars.Count == 0)
				return;
			
			this.SelectedFleet.SimulateTravel(closestObjects.Stars[0]);
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
			if (panAbsPath > PanClickTolerance) //TODO(v0.6) 
				return;
			
			Vector4 mousePoint = Vector4.Transform(mouseToView(e.X, e.Y), invProjection);
			var closestObjects = this.currentPlayer.FindClosest(
				mousePoint.X, mousePoint.Y, 
				Math.Max(screenLength * ClickRadius, StarMinClickRadius));
			
			if (this.SelectedFleet != null)
				if (closestObjects.FoundObjects.Count > 0 && closestObjects.FoundObjects[0].Type == GalaxyObjectType.Star) {
					this.SelectedFleet = this.SelectedFleet.Send(this.SelectedFleet.SimulationWaypoints);
					this.lastSelectedIdleFleets[this.currentPlayer.PlayerIndex] = this.SelectedFleet.Fleet;
					this.galaxyViewListener.FleetClicked(new FleetInfo[] { this.SelectedFleet.Fleet });
					return;
				}
				else {
					this.galaxyViewListener.FleetDeselected();
					this.SelectedFleet = null;
				}

			
			if (closestObjects.FoundObjects.Count == 0)
				return;
			
			switch (closestObjects.FoundObjects[0].Type)
			{
				case GalaxyObjectType.Star:
					this.currentSelection = GalaxySelectionType.Star;
					this.lastSelectedStars[this.currentPlayer.PlayerIndex] = closestObjects.Stars[0].Position;
					this.galaxyViewListener.SystemSelected(this.currentPlayer.OpenStarSystem(closestObjects.Stars[0]));
					break;
				case GalaxyObjectType.Fleet:
					this.currentSelection = GalaxySelectionType.Fleet;
					this.lastSelectedIdleFleets[this.currentPlayer.PlayerIndex] = closestObjects.Fleets[0];
					this.galaxyViewListener.FleetClicked(closestObjects.Fleets);
					break;
			}
			
		}
		
		public override void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (panAbsPath > PanClickTolerance)
				return;
			
			Vector4 mousePoint = Vector4.Transform(mouseToView(e.X, e.Y), invProjection);
			var closestObjects = this.currentPlayer.FindClosest(
				mousePoint.X, mousePoint.Y, 
				Math.Max(screenLength * ClickRadius, StarMinClickRadius));
			
			if (closestObjects.Stars.Count > 0)
				this.galaxyViewListener.SystemOpened(this.currentPlayer.OpenStarSystem(closestObjects.Stars[0]));
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
		
		private Vector4 mouseToView(int x, int y)
		{
			return new Vector4(
				2 * x / (float)this.canvasSize.X - 1,
				1 - 2 * y / (float)this.canvasSize.Y, 
				0, 1
			);
		}
		
		private double[] pathMatrix(Vector2d fromPoint, Vector2d toPoint)
		{
			var xAxis = toPoint - fromPoint;
			var yAxis = new Vector2d(xAxis.Y, -xAxis.X);
			double yScale = PathWidth / yAxis.Length;
			
			var center = (fromPoint + toPoint) / 2;
			return new double[] {
				xAxis.X, yAxis.X, 0, 0,
				xAxis.Y * yScale, yAxis.Y * yScale, 0, 0,
				0, 0, 1, 0,
				center.X, center.Y, 0, 1
			};
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
