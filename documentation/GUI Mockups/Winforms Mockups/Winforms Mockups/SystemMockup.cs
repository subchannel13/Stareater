using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Winforms_Mockups
{
	/// <summary>
	/// Description of SystemMockup.
	/// </summary>
	public partial class SystemMockup : UserControl
	{
		public SystemMockup()
		{
			InitializeComponent();
		}
		
		void SystemMockupPaint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.Black);
			
			var centerRect = new RectangleF(-100, 50, 200, 200);
			e.Graphics.FillEllipse(new SolidBrush(Color.Yellow), centerRect);
			
			var bodyRect = new RectangleF(75, 125, 50, 50);
			bodyRect.Inflate(10, 10);
			for(int i = 0; i < 3; i++)
			{
				centerRect.Inflate(100, 100);
				e.Graphics.DrawEllipse(new Pen(Color.LimeGreen, 3), centerRect);
				
				bodyRect.Offset(100, 0);
				e.Graphics.FillEllipse(new SolidBrush(
					new Color[]{
						Color.Peru,
						Color.Blue,
						Color.LightGray
					}[i]), bodyRect);
			}
		}
	}
}
