using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class FormColonyDetails : Form
	{
		private ColonyController controller;

		public FormColonyDetails()
		{
			InitializeComponent();
		}

		public FormColonyDetails(ColonyController controller) : this()
		{
			this.controller = controller;
			
			Context context = SettingsWinforms.Get.Language["FormColony"];
			//TODO: set form title
			
			buildingsGroup.Text = context["buildingsGroup"].Text();
			planetInfoGroup.Text = context["planetGroup"].Text();
			popInfoGroup.Text = context["popGroup"].Text();
			productivityGroup.Text = context["productivityGroup"].Text();

			var vars = new TextVar();
			var popFormat = new ThousandsFormatter(controller.PopulationMax, controller.Population);
			var percentFormat = new DecimalsFormatter(0, 1);
			vars.And("pop", popFormat.Format(controller.Population));
			vars.And("popGrowth", popFormat.Format(controller.PopulationGrowth));
			vars.And("popMax", popFormat.Format(controller.PopulationMax));
			vars.And("popOrg", percentFormat.Format(controller.Organization));
			
			populationInfo.Text = context["populationInfo"].Text(null, vars.Get);
			growthInfo.Text = context["growthInfo"].Text(null, vars.Get); //TODO: add sign
			infrastructureInfo.Text = context["infrastructureInfo"].Text(null, vars.Get);
			
			var prefixFormat = new ThousandsFormatter();
			vars.And("planetSize", prefixFormat.Format(controller.PlanetSize));
			vars.And("planetEnv", percentFormat.Format(controller.PlanetEnvironment * 100));
			
			sizeInfo.Text = context["sizeInfo"].Text(null, vars.Get);
			environmentInfo.Text = context["environmentInfo"].Text(null, vars.Get);
			
			vars.And("prodFood", percentFormat.Format(controller.FoodPerPop));
			vars.And("prodOre", percentFormat.Format(controller.OrePerPop));
			vars.And("prodInd", percentFormat.Format(controller.IndustryPerPop));
			vars.And("prodDev", percentFormat.Format(controller.DevelopmentPerPop));
			vars.And("totalInd", prefixFormat.Format(controller.IndustryTotal));
			vars.And("totalDev", prefixFormat.Format(controller.DevelopmentTotal));
			
			foodInfo.Text = context["foodInfo"].Text(null, vars.Get);
			miningInfo.Text = context["miningInfo"].Text(null, vars.Get);
			industryInfo.Text = context["industryInfo"].Text(null, vars.Get);
			developmentInfo.Text = context["developmentInfo"].Text(null, vars.Get);
			industryTotalInfo.Text = context["industryTotalInfo"].Text(null, vars.Get);
			developmentTotalInfo.Text = context["developmentTotalInfo"].Text(null, vars.Get);
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
