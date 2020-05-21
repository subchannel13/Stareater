using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views.Ships;
using Stareater.Localization;
using Stareater.Utils.NumberFormatters;
using Stareater.GUI.ShipDesigns;
using Stareater.Properties;

namespace Stareater.GUI
{
	public partial class DesignItem : UserControl
	{
		private DesignInfo data;
		private readonly PlayerController controller;
		
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
			var context = LocalizationManifest.Get.CurrentLanguage["FormDesign"];
			string refitText = "";
			var refittingWith = controller.RefittingWith(this.data);

			if (controller.IsMarkedForRemoval(this.data))
				refitText = context["markedForRemoval"].Text();
			else if (refittingWith != null)
				if (!refittingWith.Name.Equals(this.data.Name, StringComparison.InvariantCulture))
					refitText = context["refittingWith"].Text() + " " + refittingWith.Name;
				else
					refitText = context["upgradeTo"].Text() + " " + RomanFromatter.Fromat(refittingWith.Version + 1);

			nameLabel.Text = string.Join(Environment.NewLine, 
				data.Name,
				context["version"].Text() + " " + RomanFromatter.Fromat(data.Version + 1),
				refitText
			).Trim();
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
			var formatter = new ThousandsFormatter();
			
			var refitOptions = new IShipComponentType[] 
			{ 
				new ShipComponentType<DesignInfo>(context["disbandDesign"].Text(), Resources.cancel, 0, null, x => this.controller.DisbandDesign(this.data)),  
				new ShipComponentType<DesignInfo>(context["keepDesign"].Text(), Resources.start, 0, null, x => this.controller.KeepDesign(this.data))
			}.Concat(
					this.controller.RefitCandidates(this.data).Where(x => x.Constructable).Select(x => new ShipComponentType<DesignInfo>(
						x.Name + Environment.NewLine + context["refitCost"].Text() + ": " + formatter.Format(this.controller.RefitCost(this.data, x)),
						ImageCache.Get[x.ImagePath], 
						0,
						x, 
						design => this.controller.RefitDesign(this.data, design)
			)));
			
			using(var form = new FormPickComponent(context["refitTitle"].Text(), refitOptions))
				form.ShowDialog();
			
			this.makeNameText();
		}
	}
}
