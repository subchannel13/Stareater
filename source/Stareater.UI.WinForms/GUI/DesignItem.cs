﻿using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views.Ships;
using Stareater.Utils.NumberFormatters;
using Stareater.GUI.ShipDesigns;

namespace Stareater.GUI
{
	public partial class DesignItem : UserControl
	{
		private DesignInfo data;
		private PlayerController controller;
		
		public DesignItem()
		{
			InitializeComponent();
		}
		
		public DesignItem(PlayerController controller) : this()
		{
			this.controller = controller;
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

		private void thumbnail_MouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
		}

		private void countLabel_MouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
		}

		private void nameLabel_MouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
		}
		
		private void actionButton_Click(object sender, EventArgs e)
		{
			var context = SettingsWinforms.Get.Language["FormDesign"];
			
			Action<DesignInfo> disbandDesign = x => this.controller.DisbandDesign(this.data);
			Action<DesignInfo> keepDesign = x => this.controller.KeepDesign(this.data);
			Action<DesignInfo> refitDesign = x => this.controller.RefitDesign(this.data, x);
			
			var shields = new IShipComponentType[] 
			{ 
				new ShipComponentType<DesignInfo>(context["disbandDesign"].Text(), null, null, disbandDesign), //TODO(v0.6) put image
				new ShipComponentType<DesignInfo>(context["keepDesign"].Text(), null, null, keepDesign) //TODO(v0.6) put image
			}.Concat(
				this.controller.ShipsDesigns().Select(x => new ShipComponentType<DesignInfo>(
				x.Name,
				x.ImagePath,
				x, refitDesign
			)));
			
			using(var form = new FormPickComponent(shields))
				form.ShowDialog();
		}
	}
}
