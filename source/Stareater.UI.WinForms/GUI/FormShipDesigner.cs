using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Data.Ships;
using Stareater.GuiUtils;

namespace Stareater.GUI
{
	public partial class FormShipDesigner : Form
	{
		private ShipDesignController controller;
		private IList<HullInfo> hulls;
		
		private bool automaticName = true;
		private Dictionary<HullInfo, int> imageIndices = new Dictionary<HullInfo, int>();
		
		public FormShipDesigner()
		{
			InitializeComponent();
		}
		
		public FormShipDesigner(ShipDesignController controller) : this()
		{
			this.controller = controller;
			this.hulls = controller.Hulls().OrderBy(x => x.Size).ToList();
			
			Random rand = new Random();
			
			foreach(var hull in hulls) {
				hullPicker.Items.Add(new Tag<HullInfo>(hull, hull.Name));
				imageIndices.Add(hull, rand.Next(hull.ImagePaths.Length));
			}
			
			hullPicker.SelectedIndex = 0;
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void changeHullImage(int direction)
		{
			if (hullPicker.SelectedItem == null)
				return;
			var hull = (hullPicker.SelectedItem as Tag<HullInfo>).Value;
			
			imageIndices[hull] = 
				(imageIndices[hull] + hull.ImagePaths.Length + direction) % 
				hull.ImagePaths.Length;
			hullImage.Image = ImageCache.Get[hull.ImagePaths[imageIndices[hull]]];
			
			controller.ImageIndex = imageIndices[hull];
			checkValidity();
		}
		
		private void checkValidity()
		{
			acceptButton.Enabled = controller.IsDesignValid;
		}
		
		private void acceptButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
		
		private void hullSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (hullPicker.SelectedItem == null)
				return;
			var hull = (hullPicker.SelectedItem as Tag<HullInfo>).Value;
			
			if (automaticName)
				nameInput.Text = hull.Name; //TODO(later): get hull and organization specific name
			
			hullImage.Image = ImageCache.Get[hull.ImagePaths[imageIndices[hull]]];
			
			controller.Hull = hull;
			controller.ImageIndex = imageIndices[hull];
			
			checkValidity();
		}
		
		private void imageLeft_ButtonClick(object sender, EventArgs e)
		{
			changeHullImage(-1);
		}
		
		
		private void imageRight_ButtonClick(object sender, EventArgs e)
		{
			changeHullImage(1);
		}
		
		private void nameInput_KeyPress(object sender, KeyPressEventArgs e)
		{
			automaticName = false;
		}
		
		private void nameInput_TextChanged(object sender, EventArgs e)
		{
			controller.Name = nameInput.Text;
			checkValidity();
		}
	}
}
