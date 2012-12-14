using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
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
