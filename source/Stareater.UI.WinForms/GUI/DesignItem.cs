using System;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers.Views.Ships;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class DesignItem : UserControl
	{
		private DesignInfo data;
		
		public DesignItem()
		{
			InitializeComponent();
		}
		
		public DesignInfo Data 
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
			}
		}
		
		public double Count 
		{
			set
			{
				var formatter = new ThousandsFormatter();
				countLabel.Text = formatter.Format(value);
			}
		}
	}
}
