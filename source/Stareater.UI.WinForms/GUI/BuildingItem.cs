using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers.Views;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class BuildingItem : UserControl
	{
		private BuildingInfo data;
		
		public BuildingItem()
		{
			InitializeComponent();
		}
		
		public BuildingInfo Data 
		{
			get
			{
				return data;
			}
			set
			{
				this.data = value;
				
				thumbnail.Image = ImageCache.Get[data.ImagePath];
				nameLabel.Text = data.Name;
				
				var costFormatter = new ThousandsFormatter();
				countLabel.Text = costFormatter.Format(data.Quantity);
			}
		}
	}
}
