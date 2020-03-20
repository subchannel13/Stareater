using OpenTK;
using Stareater.Controllers;
using Stareater.GLData;
using Stareater.GraphicsEngine;
using Stareater.GraphicsEngine.GuiElements;
using Stareater.Localization;
using Stareater.Utils.NumberFormatters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stareater.GameScenes
{
	class BombardmentScene : AStarSystemScene
	{
		private BombardmentController controller;

		private readonly GuiImage starImage = null;
		private List<AGuiElement> planetElements = new List<AGuiElement>();

		public BombardmentScene()
		{
			var titleText = new GuiText
			{
				Margins = new Vector2(0, 10),
				Text = LocalizationManifest.Get.CurrentLanguage["FormMain"]["BombardTitle"].Text(),
				TextColor = Color.White,
				TextHeight = 32
			};
			titleText.Position.WrapContent().Then.ParentRelative(0, 1).UseMargins();
			this.AddElement(titleText);

			this.starImage = new GuiImage();
			this.starImage.Position.FixedSize(400, 400).RelativeTo(this.StarAnchor);
			this.AddElement(this.starImage);
		}

		protected override void onReturn()
		{
			this.controller.Leave();
		}

		public void StartBombardment(BombardmentController controller)
		{
			this.controller = controller;

			this.starImage.Images = new[]
			{
				new Sprite(GalaxyTextures.Get.SystemStar, controller.Star.Color)
			};
			var planets = controller.Planets.ToDictionary(x => x.Planet, x => x.Colony);
			this.setupSystem(planets.Keys, x => planets[x]);

			this.panTo(controller.Targets.First().Planet);
			this.setupUi();
		}

		public void NewTurn()
		{
			this.setupUi();
		}

		private void addPlanetElement(AGuiElement element)
		{
			this.AddElement(element);
			this.planetElements.Add(element);
		}

		private void setupUi()
		{
			foreach (var element in planetElements)
				this.RemoveElement(element);
			this.planetElements = new List<AGuiElement>();

			foreach (var planet in this.controller.Planets)
			{
				var planetImage = new GuiImage
				{
					Image = GalaxyTextures.Get.PlanetSprite(planet.Type),
				};
				planetImage.Position.FixedSize(100, 100).RelativeTo(this.planetAnchor(planet.Planet));
				this.addPlanetElement(planetImage);

				if (planet.Owner == null)
					continue;

				var colony = planet.Colony;
				var formatter = new ThousandsFormatter();
				var popInfo = new GuiText
				{
					Margins = new Vector2(0, 20),
					TextHeight = 20,
					Text = formatter.Format(colony.Population) + " / " + formatter.Format(colony.PopulationMax),
					TextColor = colony.Owner.Color
				};
				popInfo.Position.WrapContent().Then.RelativeTo(planetImage, 0, -1, 0, 1).UseMargins();
				this.addPlanetElement(popInfo);

				if (this.controller.Targets.Contains(planet))
				{
					var bombButton = new GuiButton
					{
						BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.BombButton, 6),
						BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.BombButton, 6),
						ClickCallback = () => this.controller.Bombard(planet.OrdinalPosition),
						Margins = new Vector2(0, 20)
					};
					bombButton.Position.FixedSize(80, 80).RelativeTo(popInfo, 0, -1, 0, 1).UseMargins();
					this.addPlanetElement(bombButton);
				}
			}
		}
	}
}
