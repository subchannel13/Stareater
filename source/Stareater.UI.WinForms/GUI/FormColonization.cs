using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views;
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
			
			projectList.RowStyles.Clear();
			foreach(var project in controller.ColonizationProjects)
			{
				var itemView = new ColonizationTargetView(project, controller);
				projectList.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				projectList.Controls.Add(itemView);
			}

			shipyardList.RowStyles.Clear();
			this.updateSourceList();

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

		private void removeSource(StellarisInfo stellaris)
		{
			this.controller.RemoveColonizationSource(stellaris);
			this.updateSourceList();
		}

		private void updateSourceList()
		{
			var sources = controller.ColonizationSources.ToList();
			this.shipyardList.SuspendLayout();

			while (this.shipyardList.Controls.Count > sources.Count)
				this.shipyardList.Controls.RemoveAt(this.shipyardList.Controls.Count - 1);
			while (this.shipyardList.Controls.Count < sources.Count)
			{
				if (shipyardList.RowStyles.Count < sources.Count)
					shipyardList.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				shipyardList.Controls.Add(new ColonizationSourceView(removeSource));
			}

			for (int i = 0; i < sources.Count; i++)
			{
				(this.shipyardList.Controls[i] as ColonizationSourceView).Data = sources[i];
			}
			this.shipyardList.ResumeLayout();
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

		private void addSourceAction_Click(object sender, EventArgs e)
		{
			//TODO(v0.8) disable button if no sources available
			using (var form = new FormPickColonizationSource(this.controller))
				if (form.ShowDialog() == DialogResult.OK)
				{
					this.controller.AddColonizationSource(form.SelectedSource);
					this.updateSourceList();
				}
		}
	}
}
