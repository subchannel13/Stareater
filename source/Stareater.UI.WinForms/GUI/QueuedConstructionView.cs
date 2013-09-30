using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers.Data;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class QueuedConstructionView : UserControl
	{
		private ConstructableItem data;
		
		public QueuedConstructionView()
		{
			InitializeComponent();
		}
		
		public ConstructableItem Data 
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
				
				ThousandsFormatter formatter = new ThousandsFormatter();
				costLabel.Text = "";
				investmentLabel.Text = "";
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
