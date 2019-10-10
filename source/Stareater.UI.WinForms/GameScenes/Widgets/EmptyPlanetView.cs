using Stareater.Controllers;
using Stareater.GLData;
using Stareater.GraphicsEngine.GuiElements;
using Stareater.GuiUtils;
using Stareater.Localization;
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

		public EmptyPlanetView()
		{
			this.Background = new BackgroundTexture(GalaxyTextures.Get.PanelBackground, 6);
			this.Position.FixedSize(360, 116);

			this.title = new GuiText { TextColor = Color.Black, TextHeight = 12 };
			this.title.Position.WrapContent().Then.ParentRelative(-1, 1).WithMargins(8, 4);
			this.AddChild(this.title);

			this.colonizeButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ButtonHover, 9),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ButtonNormal, 9),
				Padding = 10,
				TextColor = Color.Black,
				TextHeight = 12,
				ClickCallback = colonizeButton_Click
			};
			this.colonizeButton.Position.FixedSize(88, 88).ParentRelative(-1, -1).WithMargins(8, 8);
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
				this.controller.StartColonization(this.gameController.Stellarises().ToArray());
			}
			this.controller.RunAutomation();

			this.resetView();
		}

		private void resetView()
		{
			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];

			this.colonizeButton.Text = this.controller.IsColonizing ? context["ColonizeStop"].Text() : context["ColonizeStart"].Text();
		}
	}
}
