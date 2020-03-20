using OpenTK;
using Stareater.Controllers.Views;
using Stareater.GLData;
using Stareater.GLData.OrbitShader;
using Stareater.GraphicsEngine;
using Stareater.GraphicsEngine.GuiElements;
using Stareater.Localization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Stareater.GameScenes
{
	abstract class AStarSystemScene : AScene
	{
		private const float DefaultViewSize = 1;

		private const float FarZ = 1;
		private const float Layers = 4.0f;

		private const float OrbitZ = 3 / Layers;

		private const float BodiesY = 0.2f;
		private const float OrbitStep = 0.2f;
		private const float OrbitOffset = 0.3f;
		private const float OrbitWidth = 0.01f;
		private const float OrbitPieces = 32;

		private IEnumerable<SceneObject> planetOrbits = null;
		private Dictionary<PlanetInfo, GuiAnchor> planetAnchors = new Dictionary<PlanetInfo, GuiAnchor>();

		private Vector4? lastMousePosition = null;
		private float originOffset;
		private float minOffset;
		private float maxOffset;

		protected GuiAnchor StarAnchor { get; private set; }

		protected AStarSystemScene()
		{
			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];
			var returnButton = new GuiButton
			{
				ClickCallback = this.onReturn,
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

			this.StarAnchor = new GuiAnchor(0, 0);
			this.AddAnchor(this.StarAnchor);
		}

		protected abstract void onReturn();

		protected void setupSystem(ICollection<PlanetInfo> planets, Func<PlanetInfo, ColonyInfo> colonies)
		{
			this.maxOffset = (planets.Count + 1) * OrbitStep + OrbitOffset;
			this.lastMousePosition = null;

			this.UpdateScene(
				ref this.planetOrbits,
				planets.Select(
					planet =>
					{
						var orbitR = planet.Position * OrbitStep + OrbitOffset;
						var colony = colonies(planet);
						var color = colony != null ? Color.FromArgb(192, colony.Owner.Color) : Color.FromArgb(64, 64, 64);

						return new SceneObject(new PolygonData(
							OrbitZ,
							new OrbitData(orbitR - OrbitWidth / 2, orbitR + OrbitWidth / 2, color, Matrix4.Identity, GalaxyTextures.Get.PathLine),
							OrbitHelpers.PlanetOrbit(orbitR, OrbitWidth, OrbitPieces).ToList()
						));
					}
				).ToList()
			);

			foreach (var anchor in this.planetAnchors.Values)
				this.RemoveAnchor(anchor);
			this.planetAnchors = new Dictionary<PlanetInfo, GuiAnchor>();
			foreach(var planet in planets)
			{
				var anchor = new GuiAnchor(planet.Position * OrbitStep + OrbitOffset, 0);
				this.planetAnchors[planet] = anchor;
				this.AddAnchor(anchor);
			}
		}

		protected void panToStar()
		{
			this.originOffset = 0.5f;
		}

		protected void panTo(PlanetInfo planet)
		{
			this.originOffset = planet.Position * OrbitStep + OrbitOffset;
		}

		protected GuiAnchor planetAnchor(PlanetInfo planet) => this.planetAnchors[planet];

		#region AScene implementation
		protected override float guiLayerThickness => 1 / Layers;

		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			this.minOffset = aspect * DefaultViewSize / 2 - OrbitStep - OrbitOffset;
			this.limitPan();

			return calcOrthogonalPerspective(aspect * DefaultViewSize, DefaultViewSize, FarZ, new Vector2(originOffset, -BodiesY));
		}
		#endregion

		protected override void onMouseMove(Vector4 mouseViewPosition, Keys modiferKeys)
		{
			this.lastMousePosition = mouseViewPosition;
		}

		protected override void onMouseDrag(Vector4 mouseViewPosition)
		{
			if (!lastMousePosition.HasValue)
				this.lastMousePosition = mouseViewPosition;

			this.originOffset -= (Vector4.Transform(mouseViewPosition, this.invProjection) -
				Vector4.Transform(this.lastMousePosition.Value, this.invProjection)
				).X;

			this.limitPan();

			this.lastMousePosition = mouseViewPosition;
			this.setupPerspective();
		}

		private void limitPan()
		{
			if (originOffset > maxOffset)
				originOffset = maxOffset;
			if (originOffset < minOffset)
				originOffset = minOffset;
		}
	}
}
