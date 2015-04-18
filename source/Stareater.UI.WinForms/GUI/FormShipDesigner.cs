using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views.Ships;
using Stareater.GuiUtils;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;
using Stareater.Utils;

namespace Stareater.GUI
{
	public partial class FormShipDesigner : Form
	{
		private readonly Context context;
		private readonly ShipDesignController controller;

		private bool automaticName = true;
		private IList<HullInfo> hulls;
		private Dictionary<HullInfo, int> imageIndices = new Dictionary<HullInfo, int>();
		
		public FormShipDesigner()
		{
			InitializeComponent();
		}
		
		public FormShipDesigner(ShipDesignController controller) : this()
		{
			this.context = SettingsWinforms.Get.Language["FormDesign"];
			this.controller = controller;
			this.hulls = this.controller.Hulls().OrderBy(x => x.Size).ToList();
			
			var rand = new Random();
			
			foreach(var hull in hulls) {
				this.hullPicker.Items.Add(new Tag<HullInfo>(hull, hull.Name));
				this.imageIndices.Add(hull, rand.Next(hull.ImagePaths.Length));
			}
			
			this.hullPicker.SelectedIndex = 0;
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void changeHullImage(int direction)
		{
			if (this.hullPicker.SelectedItem == null)
				return;
			var hull = (this.hullPicker.SelectedItem as Tag<HullInfo>).Value;
			
			this.imageIndices[hull] = 
				(this.imageIndices[hull] + hull.ImagePaths.Length + direction) % 
				hull.ImagePaths.Length;
			this.hullImage.Image = ImageCache.Get[hull.ImagePaths[imageIndices[hull]]];
			
			this.controller.ImageIndex = imageIndices[hull];
			checkValidity();
		}
		
		private void checkValidity()
		{
			this.acceptButton.Enabled = controller.IsDesignValid;
		}
		
		private void updateInfos()
		{
			Context context = SettingsWinforms.Get.Language["FormDesign"];
			var percentFormat = new DecimalsFormatter(0, 1);

			double powerGenerated = this.controller.Reactor.Power;
			double powerUsed = this.controller.PowerUsed;
			
			this.powerInfo.Text = this.context["power"].Text(
					new TextVar("powerPercent", percentFormat.Format(Methods.Clamp(1 - powerUsed / powerGenerated, 0, 1) * 100)).Get
				);
		}
		
		private void acceptButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}
		
		private void hullSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.hullPicker.SelectedItem == null)
				return;
			var hull = (this.hullPicker.SelectedItem as Tag<HullInfo>).Value;
			
			if (this.automaticName)
				nameInput.Text = hull.Name; //TODO(later): get hull and organization specific name
			
			this.hullImage.Image = ImageCache.Get[hull.ImagePaths[this.imageIndices[hull]]];
			
			this.controller.Hull = hull;
			this.controller.ImageIndex = this.imageIndices[hull];
			
			this.hasIsDrive.Visible = this.controller.AvailableIsDrive != null;
			this.hasIsDrive.Checked &= this.hasIsDrive.Visible;
			this.isDriveImage.Visible = this.hasIsDrive.Checked;
			
			this.controller.HasIsDrive = this.hasIsDrive.Checked;
			
			if (this.controller.AvailableIsDrive != null) {
				var speedFormat = new DecimalsFormatter(0, 2);

				this.hasIsDrive.Text = this.context["isDrive"].Text(
					new TextVar("name", this.controller.AvailableIsDrive.Name).
					And("speed", speedFormat.Format(this.controller.AvailableIsDrive.Speed)).Get
				);
				this.isDriveImage.Image = ImageCache.Get[this.controller.AvailableIsDrive.ImagePath];
			}
			
			this.checkValidity();
			this.updateInfos();
		}
		
		private void imageLeft_ButtonClick(object sender, EventArgs e)
		{
			this.changeHullImage(-1);
		}
		
		private void imageRight_ButtonClick(object sender, EventArgs e)
		{
			this.changeHullImage(1);
		}
		
		private void nameInput_KeyPress(object sender, KeyPressEventArgs e)
		{
			this.automaticName = false;
		}
		
		private void nameInput_TextChanged(object sender, EventArgs e)
		{
			controller.Name = nameInput.Text;
			this.checkValidity();
		}
		
		private void hasIsDrive_CheckedChanged(object sender, EventArgs e)
		{
			this.controller.HasIsDrive = this.hasIsDrive.Checked;
			this.isDriveImage.Visible = this.hasIsDrive.Checked;
			this.checkValidity();
		}
	}
}
