using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Prototip;

namespace Zvjezdojedac_editori
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
			txtPutanja.Text = Environment.CurrentDirectory;
		}

		private bool _dostupniPodaci = false;
		public bool dostupniPodaci 
		{
			get { return _dostupniPodaci; }
			private set
			{
				if (value == true) lblDostupniPodaci.Text = "";
				else lblDostupniPodaci.Text = "Neispravne datoteke!";
				_dostupniPodaci = value;
			}
		}
		
		public string putanja
		{
			get { return Environment.CurrentDirectory; }
			set
			{
				try
				{
					Environment.CurrentDirectory = value;
				}
				catch (Exception) { }
				txtPutanja.Text = Environment.CurrentDirectory;
			}
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			string zvjDirFajla = Application.ExecutablePath + "_zvjDir.txt";
			if (File.Exists(zvjDirFajla))
			{
				StreamReader citac = new StreamReader(zvjDirFajla);
				putanja = citac.ReadLine();
				citac.Close();
			}

			try
			{
				dostupniPodaci = false;
				Podaci.ucitajPodatke();
				dostupniPodaci = true;
			}
			catch (FileNotFoundException exc)
			{
				MessageBox.Show("Slijedeća datoteka nije pronađena u odabranom direktoriju:\n" + exc.FileName, "Datoteka nedostaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnPutanja_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog browser = new FolderBrowserDialog();
			browser.SelectedPath = putanja;

			if (browser.ShowDialog() == DialogResult.OK)
			{
				putanja = browser.SelectedPath;

				try
				{
					dostupniPodaci = false;
					Podaci.ucitajPodatke();
					dostupniPodaci = true;
				}
				catch (FileNotFoundException exc)
				{
					MessageBox.Show("Slijedeća datoteka nije pronađena u odabranom direktoriju:\n" + exc.FileName, "Datoteka nedostaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				
				StreamWriter pisac = new StreamWriter(Application.ExecutablePath + "_zvjDir.txt");
				pisac.WriteLine(putanja);
				pisac.Close();
			}
		}

		private void btnTehnologije_Click(object sender, EventArgs e)
		{
			if (!dostupniPodaci) return;

			new FormTehnologije().Show();
		}
	}
}
