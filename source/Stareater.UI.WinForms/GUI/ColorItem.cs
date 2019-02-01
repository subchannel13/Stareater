using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stareater.GUI
{
	public partial class ColorItem : UserControl
	{
		public ColorItem()
		{
			InitializeComponent();
		}

		public Color Color
		{
			get { return colorBox.BackColor; }
			set { colorBox.BackColor = value; }
		}

		private void colorBox_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
	}
}
