using System;
using System.Drawing;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers.Data.Ships;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class ShipGroupItem : UserControl
	{
		Color lastBackColor;
		Color lastForeColor;
		
		bool isSelected = false;
			
		public ShipGroupInfo Data { get; private set; }
		
		public ShipGroupItem()
		{
			InitializeComponent();
		}
		
		public void SetData(ShipGroupInfo groupInfo)
		{
			this.Data = groupInfo;
			
			ThousandsFormatter thousandsFormat = new ThousandsFormatter();
			
			hullThumbnail.Image = ImageCache.Get[groupInfo.Design.ImagePath];
			quantityLabel.Text = thousandsFormat.Format(groupInfo.Quantity);
		}
		
		private void quantityLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		private void hullThumbnail_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		private void primaryMissionThumbnail_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		private void secondaryMissionThumbnail_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		private void shipGroupItem_Click(object sender, EventArgs e)
		{
			if (this.isSelected) {
				this.BackColor = this.lastBackColor;
				this.ForeColor = this.lastForeColor;
				this.isSelected = false;
			}
			else {
				this.lastBackColor = this.BackColor;
				this.lastForeColor = this.ForeColor;
				this.BackColor = SystemColors.Highlight;
				this.ForeColor = SystemColors.HighlightText;
				this.isSelected = true;
			}
			
			if (this.SelectionChanged != null)
				this.SelectionChanged(this, new EventArgs());
		}
		
		public event EventHandler SelectionChanged;
		
		public bool IsSelected
		{
			get { return this.isSelected; }
		}
	}
}
