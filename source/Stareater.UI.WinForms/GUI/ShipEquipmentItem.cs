using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.GUI.ShipDesigns;

namespace Stareater.GUI
{
	public partial class ShipEquipmentItem : UserControl
	{
		private double amount;
		private IShipComponentType data;
		
		public ShipEquipmentItem()
		{
			InitializeComponent();
		}

		public IShipComponentType Data
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

		public double Amount
		{
			get
			{
				return amount;
			}
			set
			{
				this.amount = value;

				amountLabel.Text = amount.ToString() + " x"; //TODO(v0.5) format
			}
		}
		
		private void amountLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		private void thumbnail_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		private void nameLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
	}
}
