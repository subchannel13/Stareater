using System;
using System.Drawing;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers.Views.Ships;
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
			
			this.lastBackColor = this.BackColor;
			this.lastForeColor = this.ForeColor;
		}
		
		public void SetData(ShipGroupInfo groupInfo)
		{
			this.Data = groupInfo;
			
			var thousandsFormat = new ThousandsFormatter();
			
			this.hullThumbnail.Image = ImageCache.Get[groupInfo.Design.ImagePath];
			this.quantityLabel.Text = thousandsFormat.Format(groupInfo.Quantity);
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
			this.IsSelected = !this.isSelected;
		}
		
		public event EventHandler SelectionChanged;
		
		public bool IsSelected
		{
			get { return this.isSelected; }
			set {
				this.isSelected = value;
				
				if (!this.isSelected) {
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
		}
	}
}
