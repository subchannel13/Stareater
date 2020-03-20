using OpenTK;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.GameScenes.Widgets;
using Stareater.GLData;
using Stareater.GraphicsEngine;
using Stareater.GraphicsEngine.GuiElements;
using Stareater.Utils;
using Stareater.Utils.NumberFormatters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stareater.GameScenes
{
	class StarSystemScene : AStarSystemScene
	{
		private const char ReturnToGalaxyKey = (char)27; //TODO(later): Make rebindable

		private StarSystemController controller;
		private PlayerController currentPlayer;
		private readonly ConstructionSiteView siteView;
		private readonly EmptyPlanetView emptyPlanetView;
		private readonly Action systemClosedHandler;

		private readonly SelectableImage<int> starSelector;
		private readonly Dictionary<int, AGuiElement> planetSelectors = new Dictionary<int, AGuiElement>();
		private readonly Dictionary<int, AGuiElement> colonizationMarkers = new Dictionary<int, AGuiElement>();
		private readonly List<AGuiElement> otherPlanetElements = new List<AGuiElement>();

		public StarSystemScene(Action systemClosedHandler)
		{
			this.systemClosedHandler = systemClosedHandler; 
			
			this.siteView = new ConstructionSiteView();
			this.siteView.Position.ParentRelative(0, -1);

			this.emptyPlanetView = new EmptyPlanetView(this.setupColonizationMarkers);
			this.emptyPlanetView.Position.ParentRelative(0, -1);

			this.starSelector = new SelectableImage<int>(StarSystemController.StarIndex)
			{
				ForgroundImage = GalaxyTextures.Get.SystemStar,
				SelectorImage = GalaxyTextures.Get.SelectedStar,
				SelectCallback = select,
				Padding = 24,
			};
			starSelector.Position.FixedSize(400, 400).RelativeTo(this.StarAnchor);
			this.AddElement(starSelector);
		}

		protected override void onReturn()
		{
			this.systemClosedHandler();
		}

		public void OnNewTurn()
		{
			if (this.controller != null)
				this.setupSystem(this.controller.Planets.ToList(), this.controller.PlanetsColony);
		}
		
		public void SetStarSystem(StarSystemController controller, PlayerController playerController)
		{
			this.setupSystem(controller.Planets.ToList(), controller.PlanetsColony);
			this.controller = controller;
			this.currentPlayer = playerController;

			var colonies = controller.Planets.Select(x => controller.PlanetsColony(x)).Where(x => x != null);
			
			if (colonies.Any())
				this.panTo(Methods.FindBest(colonies, x => x.Population).Location);
			else
				this.panToStar();

			this.starSelector.ForgroundImageColor = controller.HostStar.Color;
			this.starSelector.Select();

			foreach (var element in this.planetSelectors.Values.Concat(this.colonizationMarkers.Values).Concat(this.otherPlanetElements))
				this.RemoveElement(element);
			this.planetSelectors.Clear();
			this.colonizationMarkers.Clear();
			this.otherPlanetElements.Clear();

			var traitGridBuilder = new GridPositionBuilder(2, 20, 20, 3);
			foreach (var trait in controller.HostStar.Traits)
			{
				var traitImage = new GuiImage
				{
					Below = this.starSelector,
					Margins = new Vector2(3, 0),
					Image = GalaxyTextures.Get.Sprite(trait.ImagePath),
					Tooltip = new SimpleTooltip("Traits", trait.LangCode)
				};
				traitImage.Position.FixedSize(20, 20).RelativeTo(this.starSelector, 0.8f, -0.8f, -1, 1).UseMargins();
				traitGridBuilder.Add(traitImage.Position);
				this.addPlanetElement(traitImage);
			}

			foreach (var planet in this.controller.Planets)
			{
				var planetSelector = new SelectableImage<int>(planet.Position)
				{
					ForgroundImage = GalaxyTextures.Get.PlanetSprite(planet.Type),
					SelectorImage = GalaxyTextures.Get.SelectedStar,
					SelectCallback = select,
					Padding = 16,
				};
				planetSelector.Position.FixedSize(100, 100).RelativeTo(this.planetAnchor(planet));
				planetSelector.GroupWith(starSelector);
				this.planetSelectors[planet.Position] = planetSelector;
				this.AddElement(planetSelector);

				var popInfo = new GuiText 
				{
					Margins = new Vector2(0, 20),
					TextHeight = 20 
				};
				popInfo.Position.WrapContent().Then.RelativeTo(planetSelector, 0, -1, 0, 1).UseMargins();

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
						Margins = new Vector2(0, 10),
						Image = GalaxyTextures.Get.Sprite(trait.ImagePath),
						Tooltip = new SimpleTooltip("Traits", trait.LangCode)
					};
					traitImage.Position.FixedSize(20, 20).RelativeTo(popInfo, 0, -1, 0, 1).UseMargins().Offset(-40, 0);
					traitGridBuilder.Add(traitImage.Position);
					this.addPlanetElement(traitImage);
				}
			}

			this.setupColonizationMarkers();
		}

		private void addPlanetElement(AGuiElement element)
		{
			this.AddElement(element);
			this.otherPlanetElements.Add(element);
		}

		#region Input events
		protected override void onKeyPress(char c)
		{
			switch (c)
			{
				case ReturnToGalaxyKey:
					this.systemClosedHandler();
					break;
					//TODO(later) add hotkeys for star and planets
			}
		}
		#endregion

		private void select(int bodyIndex)
		{
			switch(controller.BodyType(bodyIndex))
			{
				case BodyType.OwnStellaris:
					this.siteView.SetView(this.controller.StellarisController());
					this.setView(siteView);
					break;
				case BodyType.OwnColony:
					this.siteView.SetView(this.controller.ColonyController(bodyIndex));
					this.setView(siteView);
					break;
				case BodyType.NotColonised:
					this.emptyPlanetView.SetView(controller.EmptyPlanetController(bodyIndex), currentPlayer);
					this.setView(this.emptyPlanetView);
					break;
				default:
					this.setView(null);
					//TODO(later) add implementation, foregin planet, empty system, foreign system
					break;
			}
		}

		private void setView(GuiPanel view)
		{
			foreach (var planetView in new GuiPanel[] { this.siteView, this.emptyPlanetView })
				if (planetView.Equals(view))
					this.ShowElement(planetView);
				else
					this.HideElement(planetView);
		}
		
		private void setupColonizationMarkers()
		{
			foreach (var marker in this.colonizationMarkers.Values)
				this.RemoveElement(marker);
			this.colonizationMarkers.Clear();

			foreach(var planet in this.controller.Planets.Where(x => this.controller.IsColonizing(x.Position)))
			{
				if (!this.planetSelectors.ContainsKey(planet.Position))
					continue;

				var marker = new GuiImage
				{
					Images = new[] {
						new Sprite(GalaxyTextures.Get.ColonizationMark),
						new Sprite(GalaxyTextures.Get.ColonizationMarkColor, this.currentPlayer.Info.Color)
					}
				};
				marker.Position.FixedSize(40, 40).RelativeTo(this.planetSelectors[planet.Position], 0.8f, 0.8f, -1, -1);
				this.colonizationMarkers[planet.Position] = marker;
				this.AddElement(marker);
			}
		}
	}
}
