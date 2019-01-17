﻿using Stareater.Controllers;
using Stareater.GameData;
using Stareater.GLData;
using Stareater.GraphicsEngine;
using Stareater.GraphicsEngine.GuiElements;
using Stareater.GUI;
using Stareater.GuiUtils;
using Stareater.Localization;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Stareater.GameScenes
{
	class ConstructionSiteView : GuiPanel
	{
		private AConstructionSiteController controller;

		private readonly GuiText title;
		private readonly GuiButton projectButton;
		private readonly GuiButton detailsButton;
		private readonly GuiText estimationLabel;

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
				Padding = 8,
				TextColor = Color.White,
				TextSize = 10,
				ClickCallback = projectButton_Click
			};
			this.projectButton.Position.FixedSize(88, 88).ParentRelative(-1, -1, 8, 8);

			this.detailsButton = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 8),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 8),
				Padding = 8,
				TextColor = Color.White,
				TextSize = 10,
				Text = context["SiteDetails"].Text(),
				ClickCallback = detailsButton_Click
			};
			this.detailsButton.Position.WrapContent().ParentRelative(1, -1, 8, 8);

			this.estimationLabel = new GuiText { TextColor = Color.Black, TextSize = 10 };
			this.estimationLabel.Position.WrapContent().RelativeTo(this.projectButton, 1, 1, -1, 1, 8, 0);
		}

		public override void Attach(AScene scene, AGuiElement parent)
		{
			base.Attach(scene, parent);
			scene.AddElement(this.title, this);
			scene.AddElement(this.projectButton, this);
			scene.AddElement(this.detailsButton, this);
			scene.AddElement(this.estimationLabel, this);
		}

		public void SetView(AConstructionSiteController siteController)
		{
			this.controller = siteController;
			if (this.controller.SiteType == SiteType.Colony)
			{
				var colonyController = this.controller as ColonyController;
				this.title.Text = LocalizationMethods.PlanetName(colonyController.PlanetBody);
			}
			else
				this.title.Text = this.controller.HostStar.Name.ToText(LocalizationManifest.Get.CurrentLanguage);

			this.resetView();
		}

		private void resetView()
		{
			if (controller.ConstructionQueue.Any())
			{
				this.projectButton.Text = null;
				this.projectButton.ForgroundImage = GalaxyTextures.Get.Sprite(this.controller.ConstructionQueue.First().ImagePath);
			}
			else
			{
				this.projectButton.Text = context["NotBuilding"].Text();
				this.projectButton.ForgroundImage = null;
			}

			this.resetEstimation();
		}

		private void resetEstimation()
		{
			if (this.controller.ConstructionQueue.Any())
				this.estimationLabel.Text = LocalizationMethods.ConstructionEstimation(
					this.controller.ConstructionQueue.First(),
					context["EtaNever"],
					context["BuildingsPerTurn"],
					context["Eta"]
				);
			else
				this.estimationLabel.Text = "No construction plans";
		}

		private void detailsButton_Click()
		{
			Form form = null;

			switch (this.controller.SiteType)
			{
				case SiteType.Colony:
					form = new FormColonyDetails(this.controller as ColonyController);
					break;
				case SiteType.StarSystem:
					form = new FormStellarisDetails(this.controller as StellarisAdminController);
					break;
			}

			form.ShowDialog();
			form.Dispose();
		}

		private void projectButton_Click()
		{
			if (this.controller == null)
				return;

			using (var form = new FormBuildingQueue(this.controller))
				form.ShowDialog();

			this.resetView();
		}

		private static Context context
		{
			get { return LocalizationManifest.Get.CurrentLanguage["FormMain"]; }
		}
	}
}
