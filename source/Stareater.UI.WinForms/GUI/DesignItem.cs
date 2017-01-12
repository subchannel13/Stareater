using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views.Ships;
using Stareater.Localization;
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
		
		private void makeNameText()
		{
			string refitText = "";
			if (controller.IsMarkedForRemoval(this.data))
				refitText = Environment.NewLine + LocalizationManifest.Get.CurrentLanguage["FormDesign"]["markedForRemoval"].Text();
			else if (controller.RefittingWith(this.data) != null)
				refitText = Environment.NewLine + LocalizationManifest.Get.CurrentLanguage["FormDesign"]["refittingWith"].Text() + " " + controller.RefittingWith(this.data).Name;
			
			nameLabel.Text = data.Name + refitText;
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

				this.thumbnail.Image = ImageCache.Get[data.ImagePath];
				this.actionButton.Visible = this.data.Constructable;
				this.makeNameText();
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
			var context = LocalizationManifest.Get.CurrentLanguage["FormDesign"];
			
			Action<DesignInfo> disbandDesign = x => this.controller.DisbandDesign(this.data);
			Action<DesignInfo> keepDesign = x => this.controller.KeepDesign(this.data);
			Action<DesignInfo> refitDesign = x => this.controller.RefitDesign(this.data, x);
			var formatter = new ThousandsFormatter();
			
			var refitOptions = new IShipComponentType[] 
			{ 
				new ShipComponentType<DesignInfo>(context["disbandDesign"].Text(), global::Stareater.Properties.Resources.cancel, null, disbandDesign),  
				new ShipComponentType<DesignInfo>(context["keepDesign"].Text(), global::Stareater.Properties.Resources.start, null, keepDesign)
			}.Concat(
					this.controller.RefitCandidates(this.data).Where(x => x.Constructable).Select(x => new ShipComponentType<DesignInfo>(
						x.Name + Environment.NewLine + context["refitCost"].Text() + ": " + formatter.Format(this.controller.RefitCost(this.data, x)),
						ImageCache.Get[x.ImagePath],
						x, refitDesign
			)));
			
			using(var form = new FormPickComponent(context["refitTitle"].Text(), refitOptions))
				form.ShowDialog();
			
			this.makeNameText();
		}
	}
}
