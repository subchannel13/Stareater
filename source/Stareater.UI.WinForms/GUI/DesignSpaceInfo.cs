using System;
using System.Drawing;
using System.Windows.Forms;
using Stareater.Utils;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class DesignSpaceInfo : UserControl
	{
		private float freePortion = 0;
		
		public DesignSpaceInfo()
		{
			InitializeComponent();
		}
		
		public void SetSpace(double current, double maximum)
		{
			var formatter = new ThousandsFormatter(maximum);
			
			this.infoText.Text = formatter.Format(current) + " / " + formatter.Format(maximum);
			this.freePortion = (float)Methods.Clamp(current / maximum, 0, maximum);
			this.Refresh();
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			if (e.ClipRectangle.IsEmpty)
				return;
			
			e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), 0, 0, e.ClipRectangle.Width * freePortion, e.ClipRectangle.Height);
		}
	}
}
