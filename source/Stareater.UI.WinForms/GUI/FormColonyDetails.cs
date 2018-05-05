using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.Localization;
using Stareater.Utils.NumberFormatters;
using Stareater.GuiUtils;
using Stareater.Properties;

namespace Stareater.GUI
{
	public sealed partial class FormColonyDetails : Form
	{
		private ColonyController controller;

		public FormColonyDetails()
		{
			InitializeComponent();
		}

		public FormColonyDetails(ColonyController controller) : this()
		{
			this.controller = controller;
			
			switch(controller.PlanetBody.Type)
			{
				case PlanetType.Asteriod:
					this.planetImage.Image = Resources.asteroids;
					break;
				case PlanetType.GasGiant:
					this.planetImage.Image = Resources.gasGiant;
					break;
				case PlanetType.Rock:
					this.planetImage.Image = Resources.rockPlanet;
					break;
			}
			
			Context context = LocalizationManifest.Get.CurrentLanguage["FormColony"];
			this.Text = LocalizationMethods.PlanetName(controller.PlanetBody);
			this.Font = SettingsWinforms.Get.FormFont;
			
			var popFormat = new ThousandsFormatter(controller.PopulationMax, controller.Population);
			var decimalFormat = new DecimalsFormatter(0, 1);
			var prefixFormat = new ThousandsFormatter();
			
			Func<string, double, string> statText = (label, x) => context[label].Text() + ": " + decimalFormat.Format(x);
			Func<string, double, string> perPop = (label, x) => context[label].Text() + ": " + decimalFormat.Format(x) + " / " + context["perPop"].Text();
			Func<string, double, string> totalText = (label, x) => context[label].Text() + ": " + prefixFormat.Format(x);
			
			buildingsGroup.Text = context["buildingsGroup"].Text();
			planetInfoGroup.Text = context["planetGroup"].Text();
			popInfoGroup.Text = context["popGroup"].Text();
			productivityGroup.Text = context["productivityGroup"].Text();

			populationInfo.Text = popFormat.Format(controller.Population) + " / " + popFormat.Format(controller.PopulationMax);
			growthInfo.Text = context["growthInfo"].Text() + ": " + DecimalsFormatter.Sign(controller.PopulationGrowth) + popFormat.Format(controller.PopulationGrowth);
			infrastructureInfo.Text = statText("infrastructureInfo", controller.Organization * 100) + "%";
			
			sizeInfo.Text = statText("sizeInfo", controller.PlanetSize);
			environmentInfo.Text = statText("environmentInfo", controller.PlanetEnvironment * 100) + "%";
			
			foodInfo.Text = perPop("foodInfo", controller.FoodPerPop);
			miningInfo.Text = perPop("miningInfo", controller.OrePerPop);
			industryInfo.Text = perPop("industryInfo", controller.IndustryPerPop);
			developmentInfo.Text = perPop("developmentInfo", controller.DevelopmentPerPop);
			industryTotalInfo.Text = totalText("industryTotalInfo", controller.IndustryTotal);
			developmentTotalInfo.Text = totalText("developmentTotalInfo", controller.DevelopmentTotal);
			
			foreach(var trait in controller.Traits)
			{
				var thumbnail = new PictureBox();
				thumbnail.Size = new Size(32, 32);
				thumbnail.SizeMode = PictureBoxSizeMode.Zoom;
				thumbnail.Image = ImageCache.Get[trait.ImagePath];
				this.traitList.Controls.Add(thumbnail);
			}
			
			foreach (var data in controller.Buildings) 
			{
				var itemView = new BuildingItem();
				itemView.Data = data;
				buildingsList.Controls.Add(itemView);
			}
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
