using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Data;
using Stareater.GuiUtils;

namespace Stareater
{
	public partial class FormShipDesigner : Form
	{
		private GameController controller;
		private IList<HullInfo> hulls;
		
		private Dictionary<HullInfo, int> imageIndices = new Dictionary<HullInfo, int>();
		
		public FormShipDesigner()
		{
			InitializeComponent();
		}
		
		public FormShipDesigner(GameController controller) : this()
		{
			this.controller = controller;
			this.hulls = controller.Hulls().ToList();
			
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
		}
		
		private void hullSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (hullPicker.SelectedItem == null)
				return;
			var hull = (hullPicker.SelectedItem as Tag<HullInfo>).Value;
			
			if (string.IsNullOrWhiteSpace(nameInput.Text))
				nameInput.Text = hull.Name; //TODO: get hull and organization specific name
			
			hullImage.Image = ImageCache.Get[hull.ImagePaths[imageIndices[hull]]];
		}
		
		private void imageLeft_ButtonClick(object sender, EventArgs e)
		{
			changeHullImage(-1);
		}
		
		
		private void imageRight_ButtonClick(object sender, EventArgs e)
		{
			changeHullImage(1);
		}
	}
}
