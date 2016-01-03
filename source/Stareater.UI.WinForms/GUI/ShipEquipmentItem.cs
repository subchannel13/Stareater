using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.GUI.ShipDesigns;
using Stareater.AppData;

namespace Stareater.GUI
{
	public partial class ShipEquipmentItem : UserControl
	{
		private double amount;
		private IShipComponentChoice data;
		
		public ShipEquipmentItem()
		{
			InitializeComponent();
		}

		public IShipComponentChoice Data
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

				amountLabel.Text = amount.ToString(); //TODO(v0.5) format
			}
		}
	}
}
