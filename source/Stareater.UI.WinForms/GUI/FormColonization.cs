using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views.Ships;
using Stareater.GUI.ShipDesigns;
using Stareater.Localization;

namespace Stareater.GUI
{
	public sealed partial class FormColonization : Form
	{
		private readonly PlayerController controller;
		
		public FormColonization()
		{
			InitializeComponent();
		}
		
		public FormColonization(PlayerController controller) : this()
		{
			this.controller = controller;
			
			var projects = controller.ColonizationProjects().ToList();
			projectList.RowStyles.Clear();
			for (int i = 0; i < projects.Count; i++)
				projectList.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			
			for (int i = 0; i < projects.Count; i++) 
			{
				var itemView = new ColonizationTargetView(projects[i], controller);
				projectList.Controls.Add(itemView);
			}
			
			var context = LocalizationManifest.Get.CurrentLanguage["FormColonization"];
			this.Text = context["title"].Text();
			this.Font = SettingsWinforms.Get.FormFont;

			this.colonizerDesignText.Text = context["colonizerLabel"].Text() + ":";
			this.capacityText.Text = context["capacityLabel"].Text() + ":";
			this.projectListTitle.Text = context["projectsTitle"].Text() + ":";
			this.shipyardListTitle.Text = context["shipyardsTitle"].Text() + ":";
			updateSelectedColonizer();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void updateSelectedColonizer()
		{
			var design = controller.ColonizerDesign;

			this.selectColonizerAction.Text = design.Name;
			this.selectColonizerAction.Image = ImageCache.Get.Resized(design.ImagePath, new Size(20, 20));
		}

		private void pickColonizer(DesignInfo design)
		{
			controller.ColonizerDesign = design;
			updateSelectedColonizer();
		}

		private void selectColonizerAction_Click(object sender, EventArgs e)
		{
			var title = LocalizationManifest.Get.CurrentLanguage["FormColonization"]["colonizerTitle"].Text();
			var options = controller.ColonizerDesignOptions.Select(desing => new ShipComponentType<DesignInfo>(
				desing.Name, ImageCache.Get[desing.ImagePath], 0, desing, x => pickColonizer(x)
			));

			using (var form = new FormPickComponent(title, options))
				form.ShowDialog();
		}
	}
}
