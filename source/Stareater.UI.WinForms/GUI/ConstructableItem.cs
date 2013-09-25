using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stareater.GUI
{
	public partial class ConstructableItem : UserControl
	{
		public ConstructableItem()
		{
			InitializeComponent();
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
	}
}
