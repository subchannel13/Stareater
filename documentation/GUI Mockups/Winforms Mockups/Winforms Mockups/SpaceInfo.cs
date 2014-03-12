using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Winforms_Mockups
{
	public partial class SpaceInfo : UserControl
	{
		public SpaceInfo()
		{
			InitializeComponent();
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			if (e.ClipRectangle.IsEmpty)
				return;
			
			e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), 0, 0, e.ClipRectangle.Width * 0.7f, e.ClipRectangle.Height);
		}
	}
}
