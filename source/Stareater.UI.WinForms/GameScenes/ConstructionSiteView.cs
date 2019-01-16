using Stareater.Controllers;
using Stareater.GameData;
using Stareater.GLData;
using Stareater.GraphicsEngine;
using Stareater.GraphicsEngine.GuiElements;
using Stareater.GuiUtils;
using Stareater.Localization;
using System.Drawing;
using System.Linq;

namespace Stareater.GameScenes
{
	class ConstructionSiteView : GuiPanel
	{
		private AConstructionSiteController controller;

		private readonly GuiText title;
		private readonly GuiButton projectButton;

		public ConstructionSiteView()
		{
			this.Background = new BackgroundTexture(GalaxyTextures.Get.PanelBackground, 3);
			this.Position.FixedSize(360, 116);

			this.title = new GuiText { TextColor = Color.White, TextSize = 14 };
			this.title.Position.WrapContent().ParentRelative(-1, 1, 8, 4);

			this.projectButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 8),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 8),
				TextColor = Color.White,
				TextSize = 10,
				ClickCallback = () => System.Diagnostics.Trace.WriteLine("Click! " + System.DateTime.Now.ToLongTimeString())
			};
			this.projectButton.Position.FixedSize(88, 88).ParentRelative(-1, -1, 8, 8);
		}

		public override void Attach(AScene scene, AGuiElement parent)
		{
			base.Attach(scene, parent);
			scene.AddElement(this.title, this);
			scene.AddElement(this.projectButton, this);
		}

		internal void SetView(AConstructionSiteController siteController)
		{
			this.controller = siteController;
			if (this.controller.SiteType == SiteType.Colony)
			{
				var colonyController = this.controller as ColonyController;
				this.title.Text = LocalizationMethods.PlanetName(colonyController.PlanetBody);
			}
			else
				this.title.Text = this.controller.HostStar.Name.ToText(LocalizationManifest.Get.CurrentLanguage);

			if (controller.ConstructionQueue.Any())
			{
				this.projectButton.Text = null;
				this.projectButton.ForgroundImage = new BackgroundTexture(GalaxyTextures.Get.Sprite(this.controller.ConstructionQueue.First().ImagePath), 8);
			}
			else
			{
				this.projectButton.Text = LocalizationManifest.Get.CurrentLanguage["FormMain"]["NotBuilding"].Text();
				this.projectButton.ForgroundImage = null;
			}
		}
	}
}
