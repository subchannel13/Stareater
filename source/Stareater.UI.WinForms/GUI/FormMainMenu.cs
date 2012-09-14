using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace Stareater.GUI
{
	public partial class FormMainMenu : Form
	{
		Font pocetniFont;

		public FormMainMenu()
		{
			InitializeComponent();
			this.pocetniFont = Font;
		}

		/*private void postaviJezik()
		{
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormMain];

			btnNovaIgra.Text = jezik["NOVA_IGRA"].tekst(null);
			btnPostavke.Text = jezik["POSTAVKE"].tekst(null);
			btnUcitaj.Text = jezik["UCITAJ"].tekst(null);
			btnUgasi.Text = jezik["UGASI"].tekst(null);
		}*/
	}
}
