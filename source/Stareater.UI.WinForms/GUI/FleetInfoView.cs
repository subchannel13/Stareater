using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views.Ships;
using Stareater.Localization;

namespace Stareater.GUI
{
	public partial class FleetInfoView : UserControl
	{
		public FleetInfo Data { get; private set; }
		
		public FleetInfoView()
		{
			InitializeComponent();
		}
		
		private void hullThumbnail_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		private void quantityLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		private void fleetInfoView_Click(object sender, EventArgs e)
		{
			this.OnSelect(this, new EventArgs());
		}
		
		public event EventHandler OnSelect;
		
		public void SetData(FleetInfo fleetInfo, PlayerController controller)
		{
			this.Data = fleetInfo;
			
			var biggestGroup = fleetInfo.Ships.Aggregate((a, b) => (a.Quantity * a.Design.Size > b.Quantity * b.Design.Size) ? a : b);
			
			this.hullThumbnail.Image = ImageCache.Get[biggestGroup.Design.ImagePath];
			this.hullThumbnail.BackColor = fleetInfo.Owner.Color;
			
			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];
			if (fleetInfo.Missions.Waypoints.Length == 0)
				this.quantityLabel.Text = context["StationaryFleet"].Text();
			else
			{
				var placeholders = new Dictionary<string, string>();
				placeholders.Add("destination", controller.Star(fleetInfo.Missions.Waypoints[0].Destionation).Name.ToText(LocalizationManifest.Get.CurrentLanguage));
				
				this.quantityLabel.Text = context["MovingFleet"].Text(placeholders);
			}
		}
	}
}
