using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;
using Stareater.Properties;

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
			this.setStarImage();
			
			Context context = SettingsWinforms.Get.Language["FormStellaris"];
			//TODO(v0.5): set form title
			
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
		
		private void setStarImage()
		{
			var baseImage = Resources.starCloseup;
			var coloredImage = new Bitmap(baseImage.Width, baseImage.Height);
			var starColor = this.controller.HostStar.Color;

        	var matrix = new ColorMatrix();
        	matrix.Matrix00 = starColor.R / 255f;
        	matrix.Matrix11 = starColor.G / 255f;
        	matrix.Matrix22 = starColor.B / 255f;

        	var attributes = new ImageAttributes();
        	attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

        	using(var g = Graphics.FromImage(coloredImage))
        		g.DrawImage(
        			baseImage, 
        			new Rectangle(0, 0, coloredImage.Width, coloredImage.Height), 
        			0, 0, coloredImage.Width, coloredImage.Height, 
        			GraphicsUnit.Pixel, attributes
        		);
        	
        	this.starImage.Image = coloredImage;
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void formStellarisDetails_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (this.starImage.Image != null)
				this.starImage.Image.Dispose();
		}
	}
}
