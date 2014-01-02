using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers.Data;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class ConstructableItemView : UserControl
	{
		private static readonly Color EnabledColor = Color.Silver;
		private static readonly Color DisabledColor = Color.FromArgb(216, 216, 216);
		
		private ConstructableItem data;
		
		public ConstructableItemView()
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
				
				thumbnail.Image = ImageCache.Get[data.ImagePath];
				nameLabel.Text = data.Name;
				
				ThousandsFormatter formatter = new ThousandsFormatter();
				costLabel.Text = "";
			}
		}
		
		private void thumbnail_Click(object sender, EventArgs e)
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
		
		private void thumbnail_MouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
		}
		
		private void nameLabel_MouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
		}
		
		private void costLabel_MouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
		}
		
		private void thumbnail_EnabledChanged(object sender, EventArgs e)
		{
			thumbnail.Image = this.Enabled ? ImageCache.Get[data.ImagePath] : ImageCache.Get.Disabled(data.ImagePath);
			this.BackColor = this.Enabled ? EnabledColor : DisabledColor;
		}
	}
}
