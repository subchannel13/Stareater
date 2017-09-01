using System;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers.Views;
using Stareater.Localization;
using Stareater.Utils.NumberFormatters;
using Stareater.GuiUtils;

namespace Stareater.GUI
{
	public partial class QueuedConstructionView : UserControl
	{
		private ConstructableInfo data;
		
		public QueuedConstructionView()
		{
			InitializeComponent();
		}
		
		public ConstructableInfo Data 
		{
			get
			{
				return data;
			}
			set
			{
				this.data = value;
				
				thumbnailImage.Image = ImageCache.Get[data.ImagePath];
				nameLabel.Text = data.Name;
				
				if (this.data.CompletedCount > 0)
				{
					var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];
				
					costLabel.Text = LocalizationMethods.ConstructionEstimation(
						this.data, 
						null, 
						context["BuildingsPerTurn"], 
						null
					);
				}
				else
				{
					var costFormatter = new ThousandsFormatter(data.Cost);
					costLabel.Text = costFormatter.Format(data.Stockpile) + " / " + costFormatter.Format(data.Cost);
				}
				
				var formatter = new ThousandsFormatter();
				investmentLabel.Text = "+" + formatter.Format(data.Investment);
			}
		}
		
		private void thumbnailImage_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		private void nameLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		private void costLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		private void investmentLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
	}
}
