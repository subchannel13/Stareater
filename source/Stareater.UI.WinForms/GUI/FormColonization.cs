using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Ships;
using Stareater.GUI.ShipDesigns;
using Stareater.GuiUtils;
using Stareater.Localization;
using Stareater.Utils.NumberFormatters;

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

			this.projectList.RowStyles.Clear();
			foreach(var project in controller.ColonizationProjects)
			{
				var itemView = new ColonizationTargetView(project, controller);
				this.projectList.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				this.projectList.Controls.Add(itemView);
			}

			this.shipyardList.RowStyles.Clear();
			this.updateSourceList();
			this.updateSelectedColonizer();
			this.capacityInput.Text = new ThousandsFormatter().Format(controller.TargetTransportCapacity);
			this.updateSourceButton();

			var context = LocalizationManifest.Get.CurrentLanguage["FormColonization"];
			this.Text = context["title"].Text();
			this.Font = SettingsWinforms.Get.FormFont;

			this.colonizerDesignText.Text = context["colonizerLabel"].Text() + ":";
			this.capacityText.Text = context["capacityLabel"].Text() + ":";
			this.projectListTitle.Text = context["projectsTitle"].Text() + ":";
			this.shipyardListTitle.Text = context["shipyardsTitle"].Text() + ":";
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
			this.updateSourceButton();
		}

		private void updateSourceButton()
		{
			this.addSourceAction.Enabled = this.controller.AvailableColonizationSources.Any();
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
			using (var form = new FormPickColonizationSource(this.controller))
				if (form.ShowDialog() == DialogResult.OK)
				{
					this.controller.AddColonizationSource(form.SelectedSource);
					this.updateSourceList();
					this.updateSourceButton();
				}
		}

		private void capacityInput_TextChanged(object sender, EventArgs e)
		{
			var quantity = NumberInput.DecodeQuantity(this.capacityInput.Text);
			if (quantity.HasValue && quantity.Value < 0)
				quantity = null;

			if (quantity.HasValue)
			{
				this.controller.TargetTransportCapacity = quantity.Value;
				this.capacityInput.BackColor = SystemColors.Window;
			}
			else
				this.capacityInput.BackColor = Color.LightPink;
		}
	}
}
