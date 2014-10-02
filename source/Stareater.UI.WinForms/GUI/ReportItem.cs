using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stareater.Controllers.Data;

namespace Stareater.GUI
{
	public partial class ReportItem : UserControl
	{
		private IReportInfo data;
		
		public ReportItem()
		{
			InitializeComponent();
		}
		
		public IReportInfo Data 
		{
			get
			{
				return data;
			}
			set
			{
				this.data = value;
				
				//TODO(v0.5): get image, possibly through visitor
				//thumbnail.Image = ImageCache.Get[data.ImagePath];
				messageLabel.Text = data.Message;
			}
		}
		
		private void thumbnail_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void messageLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
	}
}
