using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.Controllers;
using Stareater.AppData;
using Stareater.Localization;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public sealed partial class FormShipDesignList : Form
	{
		private readonly PlayerController controller;
		
		public FormShipDesignList()
		{
			InitializeComponent();
		}
		
		public FormShipDesignList(PlayerController controller) : this()
		{
			this.Text = LocalizationManifest.Get.CurrentLanguage["FormDesign"]["ListFormTitle"].Text();
			this.Font = SettingsWinforms.Get.FormFont;

			this.controller = controller;
			updateList();
			
			designName.Text = "";
			hullName.Text = "";
			equipmentInfo.Text = "";
		}
		
		private void updateList()
		{
			var designs = controller.ShipsDesigns().ToArray();
			designList.SuspendLayout();
			
			while (designList.Controls.Count < designs.Length)
			{
				var item = new DesignItem(this.controller);
				item.MouseEnter += design_OnMouseEnter;
				designList.Controls.Add(item);
			}
			while (designList.Controls.Count > designs.Length)
				designList.Controls.RemoveAt(designList.Controls.Count - 1);

			for (int i = 0; i < designs.Length; i++) {
				(designList.Controls[i] as DesignItem).Data = designs[i];
				(designList.Controls[i] as DesignItem).Count = controller.ShipCount(designs[i]);
			}
			
			designList.ResumeLayout();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void newDesignButton_Click(object sender, EventArgs e)
		{
			if (this.controller.IsReadOnly)
				return;
			
			var designer = controller.NewDesign();
			
			using(var form = new FormShipDesigner(designer))
				if (form.ShowDialog() == DialogResult.OK) {
					designer.Commit();
					updateList();
				}
		}

		private void design_OnMouseEnter(object sender, EventArgs e)
		{
			var design = (sender as DesignItem).Data;
			var formatter = new ThousandsFormatter();
			var sb = new StringBuilder();
			var context = LocalizationManifest.Get.CurrentLanguage["FormDesign"];

			sb.AppendLine(design.IsDrive != null ? design.IsDrive.Name : context["noIsDrive"].Text());
			sb.AppendLine(design.Shield != null ? design.Shield.Name : context["noShield"].Text());
			sb.AppendLine();

			foreach(var equip in design.Equipment)
				sb.AppendLine(formatter.Format(equip.Value) + " x " + equip.Key.Name);

			if (design.Equipment.Any())
				sb.AppendLine();
			
			foreach(var equip in design.SpecialEquipment)
				sb.AppendLine(formatter.Format(equip.Value) + " x " + equip.Key.Name);

			designThumbnail.Image = ImageCache.Get[design.ImagePath];
			designName.Text = design.Name;
			hullName.Text = design.Hull.Name;
			equipmentInfo.Text = sb.ToString();
		}
	}
}
