using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alati;
using Prototip.Podaci.Jezici;
using Prototip.Podaci;

namespace Prototip
{
	public partial class FormTechIzbor : Form
	{
		private Igrac igrac;

		public static double[] RaspodijelaPoena = new double[] { 0, 0.25, 0.5, 0.75, 1, 1.5, 2, 3, 5};

		private int raspodijelaPoena;

		public FormTechIzbor(Igrac igrac)
		{
			InitializeComponent();
			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormTech];
			btnIstDno.Text = jezik["btnDno"].tekst();
			btnIstDolje.Text = jezik["btnDolje"].tekst();
			btnIstGore.Text = jezik["btnGore"].tekst();
			btnIstVrh.Text = jezik["btnVrh"].tekst();
			btnOk.Text = jezik["btnOk"].tekst();
			btnRazDno.Text = jezik["btnDno"].tekst(); ;
			btnRazDolje.Text = jezik["btnDolje"].tekst();
			btnRazGore.Text = jezik["btnGore"].tekst();
			btnRazVrh.Text = jezik["btnVrh"].tekst();

			chIstNaziv.Text = jezik["chNaziv"].tekst();
			chIstNivo.Text = jezik["chNivo"].tekst();
			chIstPoeni.Text = jezik["chPoeni"].tekst();
			chIstPrioritet.Text = jezik["chIstPrioritet"].tekst();
			chKnjizNaziv.Text = jezik["chNaziv"].tekst();
			chKnjizNivo.Text = jezik["chNivo"].tekst();
			chRazNaziv.Text = jezik["chNaziv"].tekst();
			chRazNivo.Text = jezik["chNivo"].tekst();
			chRazPoeni.Text = jezik["chPoeni"].tekst();
			chRazUlaganje.Text = jezik["chRazUlaganje"].tekst();

			lblFokusirano.Text = jezik["lblFokusirano"].tekst();
			lblRaspodjela.Text = jezik["lblRaspodjela"].tekst();
			lblRavnomjerno.Text = jezik["lblRavnomjerno"].tekst();
			lblUIstrazivanju.Text = jezik["lblUIstrazivanju"].tekst() + ":";
			lblURazvoju.Text = jezik["lblURazvoju"].tekst() + ":";

			this.Text = jezik["naslov"].tekst();
			tabIstrazivanje.Text = jezik["tabIst"].tekst();
			tabKnjiznica.Text = jezik["tabKnjiz"].tekst();
			tabRazvoj.Text = jezik["tabRaz"].tekst();

			this.igrac = igrac;
			lblRazOpis.Text = "";
			lblIstOpis.Text = "";
			lblIstPoeni.Text = jezik["lblIstPoeni"].tekst() + ": " + Fje.PrefiksFormater(igrac.istrazivanjePoSustavu[igrac.istrazivanjeSustav]);
			lblIstSustav.Text = " (" + igrac.istrazivanjeSustav.ime + ")";
			lblKnjizNaziv.Text = "";
			txtKnjizOpis.Text = "";
			
			raspodijelaPoena = 0;
			for (int i = 0; i < RaspodijelaPoena.Length; i++)
				if (Math.Abs(RaspodijelaPoena[i] - igrac.koncentracijaPoenaRazvoja) < Math.Abs(RaspodijelaPoena[raspodijelaPoena] - igrac.koncentracijaPoenaRazvoja))
					raspodijelaPoena = i;
			trkRazKoncentracija.Maximum = RaspodijelaPoena.Length - 1;
			trkRazKoncentracija.Value = raspodijelaPoena;
			
			foreach (Tehnologija t in igrac.tehnologijeURazvoju)
			{
				ListViewItem item = new ListViewItem(t.tip.naziv);
				item.SubItems.Add("" + (t.nivo + 1));
				item.SubItems.Add("");
				item.SubItems.Add("");
				item.Tag = t;
				lstRazvoj.Items.Add(item);
			}

			int j = 1;
			foreach (Tehnologija t in igrac.tehnologijeUIstrazivanju)
			{
				ListViewItem item = new ListViewItem(t.tip.naziv);
				item.SubItems.Add("" + (t.nivo + 1));
				item.SubItems.Add(Fje.PrefiksFormater(t.ulozenoPoena) + " / " + Fje.PrefiksFormater(t.cijena(igrac.efekti)));
				item.SubItems.Add(j + ".");
				item.Tag = t;
				lstIstrazivanje.Items.Add(item);
				j++;
			}

			List<Tehnologija> tehnologije = igrac.istrazeneTehnologije();
			if (tehnologije.Count == 0) {
				ListViewItem item = new ListViewItem(jezik["nemaTeh"].tekst());
				item.Font = new Font(lstKnjiznica.Font, FontStyle.Italic);
				lstKnjiznica.Items.Add(item);
			}
			else {
				Font kategorijaFont = new Font(lstKnjiznica.Font, FontStyle.Bold);
				ListViewItem itemIstrazivanje = new ListViewItem(jezik["tabIst"].tekst());
				ListViewItem itemRazvoj = new ListViewItem(jezik["tabRaz"].tekst());
				itemIstrazivanje.Font = kategorijaFont;
				itemRazvoj.Font = kategorijaFont;
				Tehnologija.Kategorija zadanjaKategorija = Tehnologija.Kategorija.NISTA;
				
				foreach (Tehnologija t in igrac.istrazeneTehnologije()) {
					if (t.tip.kategorija != zadanjaKategorija) {
						if (t.tip.kategorija == Tehnologija.Kategorija.ISTRAZIVANJE)
							lstKnjiznica.Items.Add(itemIstrazivanje);
						else if (t.tip.kategorija == Tehnologija.Kategorija.RAZVOJ)
							lstKnjiznica.Items.Add(itemRazvoj);
						zadanjaKategorija = t.tip.kategorija;
					}
					ListViewItem item = new ListViewItem(t.tip.naziv);
					item.SubItems.Add(t.nivo.ToString());
					item.Tag = t;
					lstKnjiznica.Items.Add(item);
				}
			}

			izracunajPoeneRazvoja();
		}

		private void izracunajPoeneRazvoja()
		{
			int brTehnologija = lstRazvoj.Items.Count;
			List<long> rasporedPoena = Tehnologija.RasporedPoena(igrac.poeniRazvoja(), brTehnologija, RaspodijelaPoena[raspodijelaPoena]);

			for (int i = 0; i < brTehnologija; i++)
			{
				ListViewItem item = lstRazvoj.Items[i];
				Tehnologija teh = (Tehnologija)item.Tag;

				item.SubItems[2].Text = Fje.PrefiksFormater(teh.ulozenoPoena) + " / " + Fje.PrefiksFormater(teh.cijena(igrac.efekti));
				item.SubItems[3].Text = Fje.PrefiksFormater(rasporedPoena[i]);
			}
		}

		private void premjestiListViewItem(int indeks1, int indeks2, ListView listView)
		{
			ListViewItem tmp = listView.Items[indeks1];
			listView.Items.RemoveAt(indeks1);
			listView.Items.Insert(indeks2, tmp);

			string tmpStr = listView.Items[indeks1].SubItems[3].Text;
			listView.Items[indeks1].SubItems[3].Text = listView.Items[indeks2].SubItems[3].Text;
			listView.Items[indeks2].SubItems[3].Text = tmpStr;
		}

		private void btnRazGore_Click(object sender, EventArgs e)
		{
			if (lstRazvoj.SelectedIndices.Count < 1) return;
			int indeks = lstRazvoj.SelectedIndices[0];
			if (indeks == 0) return;

			premjestiListViewItem(indeks, indeks - 1, lstRazvoj);
		}

		private void btnRazDolje_Click(object sender, EventArgs e)
		{
			if (lstRazvoj.SelectedIndices.Count < 1) return;
			int indeks = lstRazvoj.SelectedIndices[0];
			if (indeks == lstRazvoj.Items.Count - 1) return;

			premjestiListViewItem(indeks, indeks + 1, lstRazvoj);
		}

		private void btnRazDno_Click(object sender, EventArgs e)
		{
			if (lstRazvoj.SelectedIndices.Count < 1) return;
			int indeks = lstRazvoj.SelectedIndices[0];
			if (indeks == lstRazvoj.Items.Count - 1) return;

			ListViewItem tmp = lstRazvoj.SelectedItems[0];
			lstRazvoj.Items.RemoveAt(indeks);
			lstRazvoj.Items.Add(tmp);
			izracunajPoeneRazvoja();
		}

		private void btnRazVrh_Click(object sender, EventArgs e)
		{
			if (lstRazvoj.SelectedIndices.Count < 1) return;
			int indeks = lstRazvoj.SelectedIndices[0];
			if (indeks == 0) return;

			ListViewItem tmp = lstRazvoj.SelectedItems[0];
			lstRazvoj.Items.RemoveAt(indeks);
			lstRazvoj.Items.Insert(0, tmp);
			izracunajPoeneRazvoja();
		}

		private void btnIstVrh_Click(object sender, EventArgs e)
		{
			if (lstIstrazivanje.SelectedIndices.Count < 1) return;
			int indeks = lstIstrazivanje.SelectedIndices[0];
			if (indeks == 0) return;

			ListViewItem tmp = lstIstrazivanje.SelectedItems[0];
			lstIstrazivanje.Items.RemoveAt(indeks);
			lstIstrazivanje.Items.Insert(0, tmp);
		}

		private void btnIstGore_Click(object sender, EventArgs e)
		{
			if (lstIstrazivanje.SelectedIndices.Count < 1) return;
			int indeks = lstIstrazivanje.SelectedIndices[0];
			if (indeks == 0) return;

			premjestiListViewItem(indeks, indeks - 1, lstIstrazivanje);
		}

		private void btnIstDolje_Click(object sender, EventArgs e)
		{
			if (lstIstrazivanje.SelectedIndices.Count < 1) return;
			int indeks = lstIstrazivanje.SelectedIndices[0];
			if (indeks == lstIstrazivanje.Items.Count - 1) return;

			premjestiListViewItem(indeks, indeks + 1, lstIstrazivanje);
		}

		private void btnIstDno_Click(object sender, EventArgs e)
		{
			if (lstIstrazivanje.SelectedIndices.Count < 1) return;
			int indeks = lstIstrazivanje.SelectedIndices[0];
			if (indeks == lstIstrazivanje.Items.Count - 1) return;

			ListViewItem tmp = lstIstrazivanje.SelectedItems[0];
			lstIstrazivanje.Items.RemoveAt(indeks);
			lstIstrazivanje.Items.Add(tmp);
		}

		private void lstRazvoj_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstRazvoj.SelectedItems.Count == 0) return;

			ListViewItem tmp = lstRazvoj.SelectedItems[0];
			Tehnologija teh = ((Tehnologija)tmp.Tag);
			lblRazOpis.Text = teh.opis;
			picRazSlika.Image = teh.tip.slika;
		}

		private void trkRazKoncentracija_Scroll(object sender, EventArgs e)
		{
			if (raspodijelaPoena != trkRazKoncentracija.Value)
			{
				raspodijelaPoena = trkRazKoncentracija.Value;
				izracunajPoeneRazvoja();
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void frmTechIzbor_FormClosing(object sender, FormClosingEventArgs e)
		{
			igrac.koncentracijaPoenaRazvoja = RaspodijelaPoena[raspodijelaPoena];
			igrac.tehnologijeURazvoju.Clear();
			foreach (ListViewItem item in lstRazvoj.Items)
				igrac.tehnologijeURazvoju.AddLast((Tehnologija)item.Tag);
			
			igrac.tehnologijeUIstrazivanju.Clear();
			foreach (ListViewItem item in lstIstrazivanje.Items)
				igrac.tehnologijeUIstrazivanju.AddLast((Tehnologija)item.Tag);
		}

		private void lstIstrazivanje_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstIstrazivanje.SelectedItems.Count == 0) return;

			ListViewItem tmp = lstIstrazivanje.SelectedItems[0];
			Tehnologija teh = ((Tehnologija)tmp.Tag);
			lblIstOpis.Text = teh.opis;
			picIstSlika.Image = teh.tip.slika;
		}

		private void lstKnjiznica_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstKnjiznica.SelectedItems.Count == 0) return;
			if (lstKnjiznica.SelectedItems[0].Tag == null) return;

			Tehnologija teh = (Tehnologija)lstKnjiznica.SelectedItems[0].Tag;
			picKnjizSlika.Image = teh.tip.slika;
			lblKnjizNaziv.Text = teh.tip.naziv;
			txtKnjizOpis.Text = teh.opis;
		}
	}
}
