using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers.Data;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class ShipGroupItem : UserControl
	{
		public ShipGroupInfo Data { get; private set; }
		
		public ShipGroupItem()
		{
			InitializeComponent();
		}
		
		public void SetData(ShipGroupInfo groupInfo)
		{
			this.Data = groupInfo;
			
			ThousandsFormatter thousandsFormat = new ThousandsFormatter();
			
			thumbnailImage.Image = ImageCache.Get[groupInfo.Design.ImagePath];
			quantityLabel.Text = thousandsFormat.Format(groupInfo.Quantity);
		}
	}
}
