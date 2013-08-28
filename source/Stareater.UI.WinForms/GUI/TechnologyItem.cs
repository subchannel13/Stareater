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
	}
}
