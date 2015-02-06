using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Data;
using Stareater.Controllers.Data.Ships;
using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.GLRenderers
{
	class GalaxyRenderer : IRenderer
	{
		private const double DefaultViewSize = 15;
		private const double ZoomBase = 1.2f;
		private const int MaxZoom = 10;
		private const int MinZoom = -10;
		
		private const float FarZ = -1;
		private const float WormholeZ = -0.8f;
		private const float PathZ = -0.7f;
		private const float StarColorZ = -0.6f;
		private const float StarSaturationZ = -0.5f;
		private const float StarNameZ = -0.4f;
		private const float StarNameZRange = 0.1f;
		private const float IdleFleetZ = -0.2f;
		private const float SelectionIndicatorZ = -0.1f;
		
		private const float PanClickTolerance = 0.01f;
		private const float ClickRadius = 0.02f;
		private const float StarMinClickRadius = 0.6f;
		
		private const float FleetIndicatorScale = 0.2f;
		private const float FleetSelectorScale = 0.3f;
		private const double PathWidth = 0.1;
		private const float StarNameScale = 0.35f;

		private const int NoCallList = -1;

		private GameController controller;
		private FleetController fleetController = null;
		private Control eventDispatcher;
		private IGalaxyViewListener galaxyViewListener;
		
		private bool resetProjection = true;
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

		private GalaxySelectionType currentSelection = GalaxySelectionType.None;
		private Dictionary<int, NGenerics.DataStructures.Mathematical.Vector2D> lastSelectedStars;
		private Dictionary<int, FleetInfo> lastSelectedIdleFleets;

		public GalaxyRenderer(GameController controller, IGalaxyViewListener galaxyViewListener)
		{ 
			this.controller = controller;
			this.galaxyViewListener = galaxyViewListener;
			
			this.mapBoundsMin = new Vector2(
				(float)controller.Stars.Select(star => star.Position.X).Min() - StarMinClickRadius,
				(float)controller.Stars.Select(star => star.Position.Y).Min() - StarMinClickRadius
			);
			this.mapBoundsMax = new Vector2(
				(float)controller.Stars.Select(star => star.Position.X).Max() + StarMinClickRadius,
				(float)controller.Stars.Select(star => star.Position.Y).Max() + StarMinClickRadius
			);
			
			this.controller.VisualPositioner = new VisualPositioner();
			
			//TODO(v0.5): move to more appropriate begin turn setup
			this.lastSelectedIdleFleets = new Dictionary<int, FleetInfo>();
			this.lastSelectedStars = new Dictionary<int, NGenerics.DataStructures.Mathematical.Vector2D>();
			
			//TODO(v0.5): move to more appropriate begin turn setup
			this.lastSelectedStars.Add(this.controller.CurrentPlayer, this.controller.ResearchCenter.Position);
			this.originOffset = new Vector2((float)this.lastSelectedStar.Position.X, (float)this.lastSelectedStar.Position.Y);
			this.currentSelection = GalaxySelectionType.Star;
		}
		
		#region IRenderer implementation
		public void AttachToCanvas(Control eventDispatcher)
		{
			this.eventDispatcher = eventDispatcher;

			eventDispatcher.MouseMove += mouseMove;
			eventDispatcher.MouseWheel += mouseZoom;
			eventDispatcher.MouseClick += mouseClick;
			eventDispatcher.MouseDoubleClick += mouseDoubleClick;
			
			resetProjection = true;
		}

		public void DetachFromCanvas()
		{
			if (eventDispatcher == null)
				return;
			
			eventDispatcher.MouseMove -= mouseMove;
			eventDispatcher.MouseWheel -= mouseZoom;
			eventDispatcher.MouseClick -= mouseClick;
			eventDispatcher.MouseDoubleClick -= mouseDoubleClick;

			this.eventDispatcher = null;
		}

		public void ResetProjection()
		{
			resetProjection = true;
		}
		
		public void Load()
		{
			GalaxyTextures.Get.Load();
			TextRenderUtil.Get.Prepare(controller.Stars.Select(x => x.Name.ToText(SettingsWinforms.Get.Language)));
		}
		
		public void Unload()
		{
			GalaxyTextures.Get.Unload();
			
			if (starDrawList >= 0){
				GL.DeleteLists(starDrawList, 1);
				starDrawList = -1;
			}
		}
		
		public void Draw(double deltaTime)
		{
			if (resetProjection) {
				double aspect = eventDispatcher.Width / (double)eventDispatcher.Height;
				double semiRadius = 0.5 * DefaultViewSize / Math.Pow(ZoomBase, zoomLevel);

				var screen = Screen.FromControl(eventDispatcher);
				if (screen.Bounds.Width > screen.Bounds.Height)
					screenLength = (float)(2 * screen.Bounds.Width * semiRadius * aspect / eventDispatcher.Width);
				else
					//TODO(v0.5): test this, perhaps by flipping the monitor.
					screenLength = (float)(2 * screen.Bounds.Height * semiRadius * aspect / eventDispatcher.Height);
				
				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();
				GL.Ortho(
					-aspect * semiRadius + originOffset.X, aspect * semiRadius + originOffset.X,
					-semiRadius + originOffset.Y, semiRadius + originOffset.Y, 
					0, -FarZ);

				GL.GetFloat(GetPName.ProjectionMatrix, out invProjection);
				invProjection.Invert();
				GL.MatrixMode(MatrixMode.Modelview);
				resetProjection = false;
			}

			if (wormholeDrawList == NoCallList) {
				wormholeDrawList = GL.GenLists(1);
				GL.NewList(wormholeDrawList, ListMode.CompileAndExecute);
				
				GL.Enable(EnableCap.Texture2D);
				GL.Color4(Color.Blue);
				
				foreach (var wormhole in controller.Wormholes) {
					GL.PushMatrix();
					GL.MultMatrix(pathMatrix(
						new Vector2d(wormhole.FromStar.Position.X, wormhole.FromStar.Position.Y), 
						new Vector2d(wormhole.ToStar.Position.X, wormhole.ToStar.Position.Y)
					));
					
					TextureUtils.Get.DrawSprite(GalaxyTextures.Get.PathLine, WormholeZ);
					
					GL.PopMatrix();
				}
				
				GL.EndList();
			}
			else
				GL.CallList(wormholeDrawList);
			
			if (this.fleetController != null && this.fleetController.SimulationWaypoints != null)
			{
				GL.Enable(EnableCap.Texture2D);
				GL.Color4(Color.LimeGreen);
				
				var last = this.fleetController.SimulationWaypoints[0];
				for(int i = 1; i < this.fleetController.SimulationWaypoints.Count; i++) {
					var next = this.fleetController.SimulationWaypoints[i];
					GL.PushMatrix();
					
					GL.MultMatrix(pathMatrix(
						new Vector2d(last.X, last.Y), 
						new Vector2d(next.X, next.Y)
					));
					
					TextureUtils.Get.DrawSprite(GalaxyTextures.Get.PathLine, PathZ);
					
					GL.PopMatrix();
					last = next;
				}
			}
			
			if (starDrawList == NoCallList) {
				starDrawList = GL.GenLists(1);
				GL.NewList(starDrawList, ListMode.CompileAndExecute);

				GL.Enable(EnableCap.Texture2D);
				
				foreach (var star in controller.Stars) {
					GL.Color4(star.Color);
					GL.PushMatrix();
					GL.Translate(star.Position.X, star.Position.Y, StarColorZ);

					TextureUtils.Get.DrawSprite(GalaxyTextures.Get.StarColor);
				
					GL.Color4(Color.White);
					TextureUtils.Get.DrawSprite(GalaxyTextures.Get.StarGlow, StarSaturationZ - StarColorZ);
				
					GL.PopMatrix();
				}
				
				float starNameZ = StarNameZ;
				foreach (var star in controller.Stars) {
					GL.Color4(starNameColor(star));
					
					GL.PushMatrix();
					GL.Translate(star.Position.X, star.Position.Y - 0.5, starNameZ);
					GL.Scale(StarNameScale, StarNameScale, StarNameScale);

					TextRenderUtil.Get.RenderText(star.Name.ToText(SettingsWinforms.Get.Language), -0.5f);
					GL.PopMatrix();
					starNameZ += StarNameZRange / controller.StarCount;
				}

				GL.EndList();
			}
			else
				GL.CallList(starDrawList);
			
			foreach (var fleet in controller.Fleets) {
				GL.Color4(fleet.Owner.Color);
				
				GL.PushMatrix();
				GL.Translate(fleet.VisualPosition.X, fleet.VisualPosition.Y, IdleFleetZ);
				GL.Scale(FleetIndicatorScale, FleetIndicatorScale, FleetIndicatorScale);

				TextureUtils.Get.DrawSprite(GalaxyTextures.Get.FleetIndicator);
				GL.PopMatrix();
			}

			if (this.currentSelection == GalaxySelectionType.Star) {
				GL.Color4(Color.White);
				GL.PushMatrix();
				GL.Translate(this.lastSelectedStar.Position.X, this.lastSelectedStar.Position.Y, SelectionIndicatorZ);

				TextureUtils.Get.DrawSprite(GalaxyTextures.Get.SelectedStar);
				GL.PopMatrix();
			}
			
			if (this.currentSelection == GalaxySelectionType.IdleFleet) {
				GL.Color4(Color.White);
				GL.PushMatrix();
				GL.Translate(this.lastSelectedIdleFleet.VisualPosition.X, this.lastSelectedIdleFleet.VisualPosition.Y, SelectionIndicatorZ);
				GL.Scale(FleetSelectorScale, FleetSelectorScale, FleetSelectorScale);

				TextureUtils.Get.DrawSprite(GalaxyTextures.Get.SelectedStar);
				GL.PopMatrix();
			}
		}

		public void OnNewTurn()
		{
			if (this.fleetController != null && !this.fleetController.Valid)
				this.fleetController = null;
				
			this.ResetLists();
		}
		
		public void ResetLists()
		{
			GL.DeleteLists(starDrawList, 1);
			GL.DeleteLists(wormholeDrawList, 1);
			this.starDrawList = NoCallList;
			this.wormholeDrawList = NoCallList;
		}
		#endregion

		#region Mouse events
		private void mouseMove(object sender, MouseEventArgs e)
		{
			Vector4 currentPosition = mouseToView(e.X, e.Y);

			if (!lastMousePosition.HasValue)
				lastMousePosition = currentPosition;

			if (e.Button.HasFlag(MouseButtons.Left)) 
				mousePan(currentPosition);
			else {
				lastMousePosition = currentPosition;
				panAbsPath = 0;
				
				if (this.fleetController != null)
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
			resetProjection = true;
			eventDispatcher.Refresh();
		}
		
		private void simulateFleetMovement(Vector4 currentPosition)
		{
			if (!this.fleetController.CanMove)
				return;
			
			Vector4 mousePoint = Vector4.Transform(currentPosition, invProjection);
			var closestObjects = controller.FindClosest(
				mousePoint.X, mousePoint.Y, 
				Math.Max(screenLength * ClickRadius, StarMinClickRadius));
			
			if (closestObjects.Stars.Count == 0)
				return;
			
			this.fleetController.SimulateTravel(closestObjects.Stars[0]);
		}

		private void mouseZoom(object sender, MouseEventArgs e)
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
			resetProjection = true;
		}

		private void mouseClick(object sender, MouseEventArgs e)
		{
			if (panAbsPath > PanClickTolerance)
				return;
			
			Vector4 mousePoint = Vector4.Transform(mouseToView(e.X, e.Y), invProjection);
			var closestObjects = controller.FindClosest(
				mousePoint.X, mousePoint.Y, 
				Math.Max(screenLength * ClickRadius, StarMinClickRadius));
			
			if (closestObjects.FoundObjects.Count == 0 || closestObjects.FoundObjects[0].Type != GalaxyObjectType.IdleFleet) {
				this.galaxyViewListener.FleetDeselected();
				this.fleetController = null;
			}
			
			if (closestObjects.FoundObjects.Count == 0)
			{
				return;
			}
			
			switch (closestObjects.FoundObjects[0].Type)
			{
				case GalaxyObjectType.Star:
					this.currentSelection = GalaxySelectionType.Star;
					this.lastSelectedStars[this.controller.CurrentPlayer] = closestObjects.Stars[0].Position;
					this.galaxyViewListener.SystemSelected(controller.OpenStarSystem(closestObjects.Stars[0]));
					break;
				case GalaxyObjectType.IdleFleet:
					this.currentSelection = GalaxySelectionType.IdleFleet;
					this.lastSelectedIdleFleets[this.controller.CurrentPlayer] = closestObjects.IdleFleets[0];
					this.fleetController = this.controller.SelectFleet(closestObjects.IdleFleets[0]);
					this.galaxyViewListener.FleetSelected(this.fleetController);
					break;
			}
			
		}
		
		private void mouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (panAbsPath > PanClickTolerance)
				return;
			
			Vector4 mousePoint = Vector4.Transform(mouseToView(e.X, e.Y), invProjection);
			var closestObjects = controller.FindClosest(
				mousePoint.X, mousePoint.Y, 
				Math.Max(screenLength * ClickRadius, StarMinClickRadius));
			
			if (closestObjects.Stars.Count > 0)
				this.galaxyViewListener.SystemOpened(controller.OpenStarSystem(closestObjects.Stars[0]));
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
				2 * x / (float)this.eventDispatcher.Width - 1,
				1 - 2 * y / (float)this.eventDispatcher.Height, 
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
				return this.lastSelectedIdleFleets[this.controller.CurrentPlayer];
			}
		}
		
		private StarData lastSelectedStar
		{
			get 
			{
				return this.controller.Star(this.lastSelectedStars[this.controller.CurrentPlayer]);
			}
		}
		
		private Color starNameColor(StarData star)
		{
			if (controller.IsStarVisited(star)) {
				var colonies = controller.KnownColonies(star);
				
				if (colonies.Count() > 0) {
					var dominantPlayer = colonies.GroupBy(x => x.Owner).OrderByDescending(x => x.Count()).First().Key;
					return dominantPlayer.Color;
				}
				
				return Color.LightGray;
			}
			
			return Color.FromArgb(64, 64, 64);
		}
		#endregion
		
		public void Dispose()
		{
			if (eventDispatcher != null) {
				DetachFromCanvas();
				Unload();
			}
		}
	}
}
