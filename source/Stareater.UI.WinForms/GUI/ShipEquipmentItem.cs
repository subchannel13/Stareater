using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.Utils.NumberFormatters;
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

				thumbnail.Image = data.Image;
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
				var formatter = new ThousandsFormatter();

                if (double.IsPositiveInfinity(this.data.AmountLimit))
					amountLabel.Text = formatter.Format(amount) + " x";
				else
					amountLabel.Text = formatter.Format(amount) + " / " + formatter.Format(this.data.AmountLimit);
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
