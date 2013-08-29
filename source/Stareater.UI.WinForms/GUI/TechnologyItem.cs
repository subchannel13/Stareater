using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stareater.GUI
{
	public partial class TechnologyItem : UserControl
	{
		public TechnologyItem()
		{
			InitializeComponent();
		}
		
		void thumbnailImage_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(new Pen(Color.Gray, 2), e.ClipRectangle);
		}
		
		void thumbnailImage_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void nameLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void levelLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void costLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void investmentLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
	}
}
