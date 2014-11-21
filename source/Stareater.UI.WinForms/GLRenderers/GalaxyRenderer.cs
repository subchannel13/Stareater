using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.AppData;
using Stareater.Controllers;
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
		private const float WormholeZ = -0.7f;
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
		private Control eventDispatcher;
		private Action<StarSystemController> systemOpenedHandler;
		
		private bool resetProjection = true;
		private Matrix4 invProjection;

		private int zoomLevel = 2;
		private Vector4? lastMousePosition = null;
		private float panAbsPath = 0;
		private Vector2 originOffset = Vector2.Zero;
		private float screenLength;
		private Vector2 mapBoundsMin;
		private Vector2 mapBoundsMax;

		private int staticList = NoCallList;

		public GalaxyRenderer(GameController controller, Action<StarSystemController> systemOpenedHandler)
		{ 
			this.controller = controller;
			this.systemOpenedHandler = systemOpenedHandler;
			
			this.mapBoundsMin = new Vector2(
				(float)controller.Stars.Select(star => star.Position.X).Min() - StarMinClickRadius,
				(float)controller.Stars.Select(star => star.Position.Y).Min() - StarMinClickRadius
			);
			this.mapBoundsMax = new Vector2(
				(float)controller.Stars.Select(star => star.Position.X).Max() + StarMinClickRadius,
				(float)controller.Stars.Select(star => star.Position.Y).Max() + StarMinClickRadius
			);
			
			this.controller.IdleFleetVisualPositioner = idleFleetVisualPosition;
			
			//TODO(v0.5): move to more appropriate begin turn setup
			originOffset = new Vector2((float)controller.SelectedStar.Position.X, (float)controller.SelectedStar.Position.Y);
		}

		public void AttachToCanvas(Control eventDispatcher)
		{
			this.eventDispatcher = eventDispatcher;

			eventDispatcher.MouseMove += mousePan;
			eventDispatcher.MouseWheel += mouseZoom;
			eventDispatcher.MouseClick += mouseClick;
			eventDispatcher.MouseDoubleClick += mouseDoubleClick;
			
			resetProjection = true;
		}

		public void DetachFromCanvas()
		{
			if (eventDispatcher == null)
				return;
			
			eventDispatcher.MouseMove -= mousePan;
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
			
			if (staticList >= 0){
				GL.DeleteLists(staticList, 1);
				staticList = -1;
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

			if (staticList == NoCallList) {
				staticList = GL.GenLists(1);
				GL.NewList(staticList, ListMode.CompileAndExecute);

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

				foreach (var fleet in controller.IdleFleets) {
					GL.Color4(fleet.Owner.Color);
					
					GL.PushMatrix();
					GL.Translate(fleet.VisualPosition.X, fleet.VisualPosition.Y, IdleFleetZ);
					GL.Scale(FleetIndicatorScale, FleetIndicatorScale, FleetIndicatorScale);

					TextureUtils.Get.DrawSprite(GalaxyTextures.Get.FleetIndicator);
					GL.PopMatrix();
				}
				GL.EndList();
			}
			else
				GL.CallList(staticList);

			if (controller.SelectedStar != null) {
				GL.Color4(Color.White);
				GL.PushMatrix();
				GL.Translate(controller.SelectedStar.Position.X, controller.SelectedStar.Position.Y, SelectionIndicatorZ);

				TextureUtils.Get.DrawSprite(GalaxyTextures.Get.SelectedStar);
				GL.PopMatrix();
			}
			
			if (controller.SelectedFleet != null) {
				GL.Color4(Color.White);
				GL.PushMatrix();
				GL.Translate(controller.SelectedFleet.VisualPosition.X, controller.SelectedFleet.VisualPosition.Y, SelectionIndicatorZ);
				GL.Scale(FleetSelectorScale, FleetSelectorScale, FleetSelectorScale);

				TextureUtils.Get.DrawSprite(GalaxyTextures.Get.SelectedStar);
				GL.PopMatrix();
			}
		}

		public void ResetLists()
		{
			GL.DeleteLists(staticList, 1);
			staticList = NoCallList;
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
		
		private static NGenerics.DataStructures.Mathematical.Vector2D idleFleetVisualPosition(NGenerics.DataStructures.Mathematical.Vector2D starPosition)
		{
			return starPosition + new NGenerics.DataStructures.Mathematical.Vector2D(0.5, 0.5);
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
		
		private Vector4 mouseToView(int x, int y)
		{
			return new Vector4(
				2 * x / (float)eventDispatcher.Width - 1,
				1 - 2 * y / (float)eventDispatcher.Height, 
				0, 1
			);
		}
		
		private void limitPan()
		{
			if (originOffset.X < mapBoundsMin.X) 
				originOffset.X = mapBoundsMin.X;
			if (originOffset.X > mapBoundsMax.X) 
				originOffset.X = mapBoundsMax.X;
			
			if (originOffset.Y < mapBoundsMin.Y) 
				originOffset.Y = mapBoundsMin.Y;
			if (originOffset.Y > mapBoundsMax.Y) 
				originOffset.Y = mapBoundsMax.Y;
		}
		
		private void mousePan(object sender, MouseEventArgs e)
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
				).Xy;

			limitPan();
			
			lastMousePosition = currentPosition;
			resetProjection = true;
			eventDispatcher.Refresh();
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
			controller.SelectClosest(
				mousePoint.X, mousePoint.Y, 
				Math.Max(screenLength * ClickRadius, StarMinClickRadius));
		}
		
		private void mouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (panAbsPath > PanClickTolerance)
				return;
			
			Vector4 mousePoint = Vector4.Transform(mouseToView(e.X, e.Y), invProjection);
			StarSystemController system = controller.OpenStarSystem(
				mousePoint.X, mousePoint.Y, 
				Math.Max(screenLength * ClickRadius, StarMinClickRadius));
			
			if (system != null)
				this.systemOpenedHandler(system);
		}

		public void Dispose()
		{
			if (eventDispatcher != null) {
				DetachFromCanvas();
				Unload();
			}
		}
	}
}
