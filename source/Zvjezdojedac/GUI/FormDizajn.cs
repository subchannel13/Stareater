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
			Dizajn dizajn = dizajner.dizajn;
			
			btnPrimMisija.Text = (dizajn.primarnoOruzje != null)
				? Fje.PrefiksFormater(dizajn.primarnoOruzje.kolicina) + " x " + dizajn.primarnoOruzje.komponenta.naziv
				: jezik["bezMisije"].tekst();

			btnSekMisija.Text = (dizajn.sekundarnoOruzje != null)
				? Fje.PrefiksFormater(dizajn.sekundarnoOruzje.kolicina) + " x " + dizajn.sekundarnoOruzje.komponenta.naziv
				: jezik["bezMisije"].tekst();

			btnStit.Text = (dizajn.stit != null)
				? dizajn.stit.naziv
				: jezik["bezStita"].tekst();

			lblCijena.Text = jezik["lblNDcijena"].tekst() + ": " + Fje.PrefiksFormater(dizajn.cijena);
			lblSlobodno.Text = jezik["lblNDslobodno"].tekst() + ": " + Fje.PrefiksFormater(dizajner.slobodnaNosivost) + " / " + Fje.PrefiksFormater(dizajn.trup.nosivost);
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
			using (var frmMisija = new FormDizajnKomponenta<Oruzje>(dizajner.oruzja, dizajner.dizajnPrimMisija, jezik["infoPrimMisija"].tekst(), btnPrimMisija))
				if (frmMisija.ShowDialog() == DialogResult.OK) {
					dizajner.dizajnPrimMisija = frmMisija.OdabranaKomponenta;

					osvjeziStatistike();
				}
		}

		private void btnSekMisija_Click(object sender, EventArgs e)
		{
			using (var frmMisija = new FormDizajnKomponenta<Oruzje>(dizajner.oruzja, dizajner.dizajnSekMisija, jezik["infoSekMisija"].tekst(), btnSekMisija))
				if (frmMisija.ShowDialog() == DialogResult.OK) {
					dizajner.dizajnSekMisija = frmMisija.OdabranaKomponenta;

					osvjeziStatistike();
				}
		}

		private void hscrUdioMisija_Scroll(object sender, ScrollEventArgs e)
		{
			lblUdioMisija.Text = hscrUdioMisija.Value + "%";
			dizajner.dizajnUdioPrimMisije = (100.0 - hscrUdioMisija.Value) / 100.0;
			osvjeziStatistike ();
		}

		private void btnStit_Click(object sender, EventArgs e)
		{
			using (var frmMisija = new FormDizajnKomponenta<Stit>(dizajner.trupKomponente.stitovi, dizajner.dizajnStit, jezik["infoStit"].tekst(), btnStit))
				if (frmMisija.ShowDialog() == DialogResult.OK) {
					dizajner.dizajnStit = frmMisija.OdabranaKomponenta;

					osvjeziStatistike();
				}
		}

		private void chMZpogon_CheckedChanged(object sender, EventArgs e)
		{
			dizajner.dizajnMZPogon = chMZpogon.Checked;
			osvjeziStatistike();
		}
	}
}
