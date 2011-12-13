using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Igra.Brodovi.Dizajner;
using Zvjezdojedac.Igra;
using Zvjezdojedac.Igra.Brodovi;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Podaci;

namespace Zvjezdojedac.GUI
{
	public partial class FormDizajn : Form
	{
		Igrac igrac;
		Dizajner dizajner;
		
		Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormFlote];

		public FormDizajn(Igrac igrac)
		{
			this.igrac = igrac;
			dizajner = new Dizajner(igrac);

			// inicijalizacija GUIa
			InitializeComponent();

			this.Text = jezik["tabNoviDizajn"].tekst();
			foreach (Trup trup in dizajner.trupovi)
				cbVelicina.Items.Add(trup);
		}

		private void FormDizajn_Load(object sender, EventArgs e)
		{
			if (cbVelicina.Items.Count > Postavke.PrethodnaVelicinaBroda)
				cbVelicina.SelectedIndex = Postavke.PrethodnaVelicinaBroda;
		}

		private void osvjeziStatistike()
		{
			//throw new NotImplementedException();
		}

		private void cbVelicina_SelectedIndexChanged(object sender, EventArgs e)
		{
			Trup trup = (Trup)cbVelicina.SelectedItem;
			dizajner.odabranTrup = trup;

			if (dizajner.komponente[trup].mzPogon == null) {
				chMZpogon.Checked = false;
				chMZpogon.Enabled = false;
			}
			else
				chMZpogon.Enabled = true;

			osvjeziStatistike();
			picSlika.Image = dizajner.odabranTrup.slika;
		}

		private void btnPrimMisija_Click(object sender, EventArgs e)
		{
			using (FormDizajnMisija frmMisija = new FormDizajnMisija(dizajner.oruzja, dizajner.dizajnPrimMisija))
				if (frmMisija.ShowDialog() == DialogResult.OK) {
					dizajner.dizajnPrimMisija = frmMisija.OdabranaMisija;
					btnPrimMisija.Text = Fje.PrefiksFormater(dizajner.dizajn.primarnoOruzje.kolicina) + " x " + dizajner.dizajn.primarnoOruzje.komponenta.naziv;
					osvjeziStatistike();
				}
		}
	}
}
