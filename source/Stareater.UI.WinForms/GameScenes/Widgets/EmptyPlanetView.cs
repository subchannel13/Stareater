using OpenTK;
using Stareater.Controllers;
using Stareater.GLData;
using Stareater.GraphicsEngine.GuiElements;
using Stareater.GuiUtils;
using Stareater.Localization;
using System;
using System.Drawing;
using System.Linq;

namespace Stareater.GameScenes.Widgets
{
	class EmptyPlanetView : GuiPanel
	{
		private ColonizationController controller;
		private PlayerController gameController;

		private readonly GuiText title;
		private readonly GuiButton colonizeButton;
		private readonly Action onColonizationChange;

		public EmptyPlanetView(Action onColonizationChange)
		{
			this.onColonizationChange = onColonizationChange;
			this.Background = new BackgroundTexture(GalaxyTextures.Get.PanelBackground, 6);
			this.Position.FixedSize(360, 116);

			this.title = new GuiText 
			{
				Margins = new Vector2(8, 4),
				TextColor = Color.Black, 
				TextHeight = 12 
			};
			this.title.Position.WrapContent().Then.ParentRelative(-1, 1).UseMargins();
			this.AddChild(this.title);

			this.colonizeButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ButtonHover, 9),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ButtonNormal, 9),
				Padding = 10,
				Margins = new Vector2(8, 8),
				TextColor = Color.Black,
				TextHeight = 12,
				ClickCallback = colonizeButton_Click
			};
			this.colonizeButton.Position.FixedSize(88, 88).ParentRelative(-1, -1).UseMargins();
			this.AddChild(this.colonizeButton);
		}

		public void SetView(ColonizationController controller, PlayerController gameController)
		{
			this.controller = controller;
			this.gameController = gameController;

			this.title.Text = LocalizationMethods.PlanetName(this.controller.PlanetBody);

			this.resetView();
		}

		private void colonizeButton_Click()
		{
			if (this.controller == null)
				return;

			if (this.controller.IsColonizing)
				this.controller.StopColonization();
			else
			{
				//TODO(v0.9) smarter default colonization source selection
				this.controller.StartColonization();
			}
			this.controller.RunAutomation();

			this.resetView();
			this.onColonizationChange();
		}

		private void resetView()
		{
			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];

			this.colonizeButton.Text = this.controller.IsColonizing ? context["ColonizeStop"].Text() : context["ColonizeStart"].Text();
		}
	}
}
