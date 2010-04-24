using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Prototip
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
#if DEBUG
#else
			try
			{
#endif
				Podaci.ucitajPodatke();
#if DEBUG
#else
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
#endif
		}

		private void btnNovaIgra_Click(object sender, EventArgs e)
		{
			frmNovaIgra novaIgra = new frmNovaIgra();
			this.Visible = false;
			if (novaIgra.ShowDialog() == DialogResult.OK)
			{
				frmIgra igra = new frmIgra(new Igra(novaIgra.igraci, novaIgra.mapa));
				igra.ShowDialog();
			}
			this.Visible = true;
		}

		private void btnUgasi_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Postavke.ProslaIgra.spremi();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
		}
	}
}
