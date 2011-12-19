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

			foreach (Taktika taktika in Taktika.Taktike.Keys)
				cbTaktika.Items.Add(new TagTekst<Taktika>(taktika, taktika.naziv));
			
			foreach (SpecijalnaOprema so in dizajner.trupKomponente.specijalnaOprema) {
				ListViewItem item = new ListViewItem("");
				item.SubItems.Add(so.naziv);
				item.Tag = so;
				lstvSpecOprema.Items.Add(item);
			}

			// jezik
			btnSpremi.Text = jezik["btnSpremi"].tekst();
			chSpecOpNaziv.Text = jezik["chSpecOpNaziv"].tekst();
			chMZpogon.Text = jezik["chNDMZpogon"].tekst();
			lblNaziv.Text = jezik["lblNaziv"].tekst() + ":";
			lblPrimMisija.Text = jezik["lblPrimMisija"].tekst() + ":";
			lblSekMisija.Text = jezik["lblSekMisija"].tekst() + ":";
			lblSpecOprema.Text = jezik["lblSpecOprema"].tekst() + ":";
			lblStit.Text = jezik["lblStit"].tekst() + ":";
			lblTaktika.Text = jezik["lblTaktika"].tekst() + ":";
			lblUdioSek.Text = jezik["lblUdioSek"].tekst() + ":";
			lblVelicina.Text = jezik["lblVelicina"].tekst() + ":";
			this.Text = jezik["tabNoviDizajn"].tekst();
		}

		private void FormDizajn_Load(object sender, EventArgs e)
		{
			if (cbVelicina.Items.Count > Postavke.PrethodnaVelicinaBroda)
				cbVelicina.SelectedIndex = Postavke.PrethodnaVelicinaBroda;

			cbTaktika.SelectedIndex = 0;
			hscrUdioMisija.Value = (int)((1 - dizajner.dizajnUdioPrimMisije) * 100);
			lblUdioMisija.Text = hscrUdioMisija.Value + "%";
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

			lblOklop.Text = jezik["lblNDoklop"].tekst() + ": " + Fje.PrefiksFormater(dizajn.izdrzljivostOklopa);
			lblOmetanje.Text = jezik["lblOmetanje"].tekst() + ": " + dizajn.ometanje;
			lblPokretljivost.Text = jezik["lblNDpokretljivost"].tekst() + ": " + Fje.PrefiksFormater(dizajn.pokretljivost);
			lblPrikrivanje.Text = jezik["lblPrikrivanje"].tekst() + ": " + dizajn.prikrivenost;
			lblReaktor.Text = jezik["lblReaktor"].tekst() + ": " + (dizajn.koefSnageReaktora * 100).ToString("0") + "%";
			lblSenzori.Text = jezik["lblNDsenzori"].tekst() + ": " + Fje.PrefiksFormater(dizajn.snagaSenzora);

			lblCijena.Text = jezik["lblNDcijena"].tekst() + ": " + Fje.PrefiksFormater(dizajn.cijena);
			lblSlobodno.Text = jezik["lblNDslobodno"].tekst() + ": " + Fje.PrefiksFormater(dizajner.slobodnaNosivost) + " / " + Fje.PrefiksFormater(dizajn.trup.Nosivost);
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
			Postavke.PrethodnaVelicinaBroda = cbVelicina.SelectedIndex;
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

		private void promjeniNDspecOpremu(bool dodaj, int soIndeks)
		{
			SpecijalnaOprema so = dizajner.trupKomponente.specijalnaOprema[soIndeks];
			int n;

			if (dodaj)
				n = dizajner.dodajSpecOpremu(so);
			else
				n = dizajner.makniSpecOpremu(so);

			if (n > 0)
				lstvSpecOprema.Items[soIndeks].SubItems[0].Text = n + "x";
			else
				lstvSpecOprema.Items[soIndeks].SubItems[0].Text = "";

			osvjeziStatistike();
		}

		private void btnSpecOpremaPlus_Click(object sender, EventArgs e)
		{
			if (lstvSpecOprema.SelectedItems.Count == 0)
				return;

			promjeniNDspecOpremu(true, lstvSpecOprema.SelectedIndices[0]);
		}

		private void btnSpecOpremaMinus_Click(object sender, EventArgs e)
		{
			if (lstvSpecOprema.SelectedItems.Count == 0)
				return;

			promjeniNDspecOpremu(false, lstvSpecOprema.SelectedIndices[0]);
		}

		private void cbTaktika_SelectedIndexChanged(object sender, EventArgs e)
		{
			dizajner.dizajnTaktika = ((TagTekst<Taktika>)cbTaktika.SelectedItem).tag;
		}
	}
}
