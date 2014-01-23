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
			
			buildingsGroup.Text = context["buildingsGroup"].Text();
			planetInfoGroup.Text = context["planetGroup"].Text();
			popInfoGroup.Text = context["popGroup"].Text();
			productivityGroup.Text = context["productivityGroup"].Text();

			var vars = new TextVar();
			var popFormat = new ThousandsFormatter(controller.PopulationMax, controller.Population);
			vars.And("pop", popFormat.Format(controller.Population));
			vars.And("popGrowth", popFormat.Format(controller.PopulationGrowth));
			vars.And("popMax", popFormat.Format(controller.PopulationMax));
			
			populationInfo.Text = context["populationInfo"].Text(null, vars.Get);
			growthInfo.Text = context["growthInfo"].Text(null, vars.Get); //TODO: add sign
			//TODO: set language
		}
	}
}
