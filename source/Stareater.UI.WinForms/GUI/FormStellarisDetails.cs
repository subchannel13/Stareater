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
	public partial class FormStellarisDetails : Form
	{
		private StellarisAdminController controller;

		public FormStellarisDetails()
		{
			InitializeComponent();
		}

		public FormStellarisDetails(StellarisAdminController controller) : this()
		{
			this.controller = controller;
			
			Context context = SettingsWinforms.Get.Language["FormStellaris"];
			//TODO: set form title
			
			buildingsGroup.Text = context["buildingsGroup"].Text();
			coloniesInfoGroup.Text = context["coloniesGroup"].Text();
			outputInfoGroup.Text = context["outputGroup"].Text();
			
			var vars = new TextVar();
			var prefixFormat = new ThousandsFormatter();
			var percentFormat = new DecimalsFormatter(0, 1);
			vars.And("pop", prefixFormat.Format(controller.PopulationTotal));
			vars.And("popOrg", percentFormat.Format(controller.OrganisationAverage));
			
			populationInfo.Text = context["populationInfo"].Text(null, vars.Get);
			infrastructureInfo.Text = context["infrastructureInfo"].Text(null, vars.Get);
			
			vars.And("outInd", prefixFormat.Format(controller.IndustryTotal));
			vars.And("outDev", prefixFormat.Format(controller.DevelopmentTotal));
			vars.And("outRes", prefixFormat.Format(controller.Research));
			
			industryInfo.Text = context["industryInfo"].Text(null, vars.Get);
			developmentInfo.Text = context["developmentInfo"].Text(null, vars.Get);
			researchInfo.Text = context["researchInfo"].Text(null, vars.Get);
			
			foreach (var data in controller.Buildings) {
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
