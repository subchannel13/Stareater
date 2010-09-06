using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace Prototip
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();

#if DEBUG
#else
			try
			{
#endif
				PodaciAlat.postaviPodatke();
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
			FormNovaIgra novaIgra = new FormNovaIgra();
			this.Visible = false;
			if (novaIgra.ShowDialog() == DialogResult.OK)
			{
				FormIgra igra = new FormIgra(new Igra(novaIgra.igraci, novaIgra.mapa));
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

		private void btnUcitaj_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.InitialDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "pohranjeno";
			dialog.FileName = "sejv.igra";
			dialog.Filter = "Zvjezdojedac igra (*.igra)|*.igra";

			if (dialog.ShowDialog() == DialogResult.OK) {

				GZipStream zipStream = new GZipStream(new FileStream(dialog.FileName, FileMode.Open), CompressionMode.Decompress);
				StreamReader citac = new StreamReader(zipStream);

				string ucitanaIgra = citac.ReadToEnd();
				citac.Close();

				Igra igra = Igra.Ucitaj(ucitanaIgra);
				
				this.Visible = false;
				FormIgra frmIgra = new FormIgra(igra);
				frmIgra.ShowDialog();
				this.Visible = true;
			}
		}
	}
}
