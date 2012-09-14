using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stareater.GUI
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
		}

		private void FormMain_Shown(object sender, EventArgs e)
		{
			using (FormMainMenu mainMenu = new FormMainMenu())
				mainMenu.ShowDialog();
		}
	}
}
