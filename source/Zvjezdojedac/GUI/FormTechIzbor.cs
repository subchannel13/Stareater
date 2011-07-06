using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Igra;
using Zvjezdojedac.Igra.Brodovi;

namespace Zvjezdojedac.GUI
{
	public partial class FormTechIzbor : Form
	{
		public static double[] RaspodijelaPoena = new double[] { 0, 0.25, 0.5, 0.75, 1, 1.5, 2, 3, 5};

		private delegate void PrikazKomponente(IKomponenta komponenta);

		private Igrac igrac;
		private int raspodijelaPoena;
		private Dictionary<KategorijaOpreme, ListViewItem[]> opremaStavke = new Dictionary<KategorijaOpreme, ListViewItem[]>();
		private int[] opremaOstaloZadnjiIndeks = new int[4];
		private Dictionary<KategorijaOpreme, PrikazKomponente> prikaziKomponenti = new Dictionary<KategorijaOpreme, PrikazKomponente>();

		public FormTechIzbor(Igrac igrac)
		{
			InitializeComponent();
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormTech];
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
			tabOprema.Text = jezik["tabOprema"].tekst();
			tabKnjiznica.Text = jezik["tabKnjiz"].tekst();
			tabRazvoj.Text = jezik["tabRaz"].tekst();

			this.igrac = igrac;
			txtRazOpis.Text = "";
			txtIstOpis.Text = "";
			lblIstPoeni.Text = jezik["lblIstPoeni"].tekst() + ": " + Fje.PrefiksFormater(igrac.istrazivanjePoSustavu[igrac.istrazivanjeSustav]);
			lblIstSustav.Text = " (" + igrac.istrazivanjeSustav.ime + ")";
			lblKnjizNaziv.Text = "";
			txtKnjizOpis.Text = "";
			lblOpNaziv.Text = "";
			txtOpOpis.Text = "";

			InizijalizirajRazvoj();
			InicijalizirajIstrazivanje();
			InicijalizirajKnjiznicu();
			InicijalizirajOpremu();

			izracunajPoeneRazvoja();
		}

		private void InicijalizirajIstrazivanje()
		{
			int j = 1;
			foreach (Tehnologija t in igrac.tehnologijeUIstrazivanju) {
				ListViewItem item = new ListViewItem(t.tip.naziv);
				item.SubItems.Add("" + (t.nivo + 1));
				item.SubItems.Add(Fje.PrefiksFormater(t.ulozenoPoena) + " / " + Fje.PrefiksFormater(t.cijena(igrac.efekti)));
				item.SubItems.Add(j + ".");
				item.Tag = t;
				lstIstrazivanje.Items.Add(item);
				j++;
			}
		}
		private void InicijalizirajKnjiznicu()
		{
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormTech];
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
		}
		private void InicijalizirajOpremu()
		{
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormTech];
			List<ListViewItem> items = new List<ListViewItem>();
			Font fontBold = new Font(lstOprema.Font, FontStyle.Bold);
			Font fontItalic = new Font(lstOprema.Font, FontStyle.Italic);

			Trup najveci = null;
			#region Trupovi
			cbOpKategorija.Items.Add(new TagTekst<KategorijaOpreme>(KategorijaOpreme.Trup, jezik["opKatTrup"].tekst()));
			items = new List<ListViewItem>();
			foreach (Trup trup in Trup.TrupInfo.DostupniTrupovi(igrac.efekti)) {
				items.Add(komponentaListViewItem(trup));
				
				if (najveci == null) najveci = trup;
				if (trup.velicina > najveci.velicina) najveci = trup;

				cbOpVelicine.Items.Add(new TagTekst<Trup>(trup, trup.naziv));
			}
			opremaStavke.Add(KategorijaOpreme.Trup, items.ToArray());
			prikaziKomponenti.Add(KategorijaOpreme.Trup, prikazTrupa);
			#endregion

			#region Misije
			cbOpKategorija.Items.Add(new TagTekst<KategorijaOpreme>(KategorijaOpreme.Misija, jezik["opKatMisija"].tekst()));
			items = new List<ListViewItem>();
			Dictionary<Misija.Tip, List<Oruzje>> misije = Oruzje.OruzjeInfo.DostupnaOruzja(igrac.efekti);
			for (int misijaId = 0; misijaId < (int)Misija.Tip.N; misijaId++) {
				Misija.Tip misijaTip = (Misija.Tip)misijaId;
				if (!misije.ContainsKey(misijaTip)) 
					continue;
				
				items.Add(specLVItem(Misija.Opisnici[misijaTip].naziv, fontBold));
				foreach (Oruzje oruzje in misije[misijaTip])
					items.Add(komponentaListViewItem(oruzje));
			}
			opremaStavke.Add(KategorijaOpreme.Misija, items.ToArray());
			prikaziKomponenti.Add(KategorijaOpreme.Misija, prikazOruzja);
			#endregion

			#region Stitovi
			cbOpKategorija.Items.Add(new TagTekst<KategorijaOpreme>(KategorijaOpreme.Stit, jezik["opKatStit"].tekst()));
			items = new List<ListViewItem>();
			foreach (Stit stit in Stit.StitInfo.DostupniStitovi(igrac.efekti, najveci.velicina_stita))
				items.Add(komponentaListViewItem(stit));
			opremaStavke.Add(KategorijaOpreme.Stit, items.ToArray());
			prikaziKomponenti.Add(KategorijaOpreme.Stit, prikazStita);
			#endregion

			#region Specijalna oprema
			cbOpKategorija.Items.Add(new TagTekst<KategorijaOpreme>(KategorijaOpreme.Specijalno, jezik["opKatSpecOp"].tekst()));
			items = new List<ListViewItem>();
			foreach (SpecijalnaOprema so in SpecijalnaOprema.SpecijalnaOpremaInfo.DostupnaOprema(igrac.efekti, najveci.velicina))
				items.Add(komponentaListViewItem(so));
			opremaStavke.Add(KategorijaOpreme.Specijalno, items.ToArray());
			prikaziKomponenti.Add(KategorijaOpreme.Specijalno, prikazSpecOp);
			#endregion

			#region Ostalo
			cbOpKategorija.Items.Add(new TagTekst<KategorijaOpreme>(KategorijaOpreme.Ostalo, jezik["opKatOstalo"].tekst()));
			items = new List<ListViewItem>();
			jezik = Postavke.Jezik[Kontekst.FormFlote];
			
			items.Add(specLVItem(jezik["infoMZPogon"].tekst(), fontBold));
			foreach (MZPogon komp in MZPogon.MZPogonInfo.Dostupni(igrac.efekti))
				items.Add(komponentaListViewItem(komp));
			opremaOstaloZadnjiIndeks[0] = items.Count;

			items.Add(specLVItem(jezik["infoPotisnici"].tekst(), fontBold));
			foreach (Potisnici komp in Potisnici.PotisnikInfo.Dostupni(igrac.efekti))
				items.Add(komponentaListViewItem(komp));
			opremaOstaloZadnjiIndeks[1] = items.Count;

			items.Add(specLVItem(jezik["infoReaktor"].tekst(), fontBold));
			foreach (Reaktor komp in Reaktor.ReaktorInfo.Dostupni(igrac.efekti))
				items.Add(komponentaListViewItem(komp));
			opremaOstaloZadnjiIndeks[2] = items.Count;

			items.Add(specLVItem(jezik["infoSenzori"].tekst(), fontBold));
			foreach (Senzor komp in Senzor.SenzorInfo.Dostupni(igrac.efekti))
				items.Add(komponentaListViewItem(komp));
			opremaOstaloZadnjiIndeks[3] = items.Count;

			opremaStavke.Add(KategorijaOpreme.Ostalo, items.ToArray());
			prikaziKomponenti.Add(KategorijaOpreme.Ostalo, prikazOstalog);
			#endregion

			foreach (var kategorija in opremaStavke.Keys)
				if (opremaStavke[kategorija].Length == 0) {
					ListViewItem item = specLVItem(jezik["nemaOp"].tekst(), fontItalic);
					opremaStavke[kategorija] = new ListViewItem[] { item };
				}
			cbOpKategorija.SelectedIndex = 0;
			cbOpVelicine.SelectedIndex = 0;
		}
		private void InizijalizirajRazvoj()
		{
			raspodijelaPoena = 0;
			for (int i = 0; i < RaspodijelaPoena.Length; i++)
				if (Math.Abs(RaspodijelaPoena[i] - igrac.koncentracijaPoenaRazvoja) < Math.Abs(RaspodijelaPoena[raspodijelaPoena] - igrac.koncentracijaPoenaRazvoja))
					raspodijelaPoena = i;
			trkRazKoncentracija.Maximum = RaspodijelaPoena.Length - 1;
			trkRazKoncentracija.Value = raspodijelaPoena;

			foreach (Tehnologija t in igrac.tehnologijeURazvoju) {
				ListViewItem item = new ListViewItem(t.tip.naziv);
				item.SubItems.Add("" + (t.nivo + 1));
				item.SubItems.Add("");
				item.SubItems.Add("");
				item.Tag = t;
				lstRazvoj.Items.Add(item);
			}
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

		#region Prikazi komponenti
		private void uobicajeniPrikaz(IKomponenta komponentaObj)
		{
			picOpSlika.Image = komponentaObj.slika;
			lblOpNaziv.Text = komponentaObj.naziv;
			txtOpOpis.Text = komponentaObj.opis;
		}
		private void prikazTrupa(IKomponenta komponentaObj)
		{
			Trup trup = (Trup)komponentaObj;
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormTech];

			StringBuilder sb = new StringBuilder(txtOpOpis.Text);
			sb.AppendLine();
			sb.AppendLine();
			sb.AppendLine(jezik["opTrupVelicina"].tekst() + ": " + Fje.PrefiksFormater(trup.velicina));
			sb.AppendLine(jezik["opTrupProstor"].tekst() + ": " + Fje.PrefiksFormater(trup.nosivost));
			sb.AppendLine(jezik["opTrupTromost"].tekst() + ": " + trup.tromost);
			sb.AppendLine(jezik["opCijena"].tekst() + ": " + Fje.PrefiksFormater(trup.cijena));
			sb.AppendLine();
			sb.AppendLine(jezik["opTrupOklop"].tekst() + ": " + Fje.PrefiksFormater(trup.bazaOklopa));
			sb.AppendLine(jezik["opTrupStit"].tekst() + ": " + Fje.PrefiksFormater(trup.bazaStita));
			sb.AppendLine(jezik["opTrupPrik"].tekst() + ": " + trup.kapacitetPrikrivanja);
			sb.AppendLine(jezik["opTrupSenzori"].tekst() + ": x" + Senzor.BonusKolicine(trup.brojSenzora).ToString("0.##"));
			sb.AppendLine();
			sb.AppendLine(jezik["opTrupVelReak"].tekst() + ": " + trup.velicina_reaktora + " " + jezik["opTrupRezerv"].tekst());
			sb.AppendLine(jezik["opTrupVelMZ"].tekst() + ": " + trup.velicina_MZPogona);
			txtOpOpis.Text = sb.ToString();
		}
		private void prikazOruzja(IKomponenta komponentaObj)
		{
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormTech];
			Oruzje oruzje = (Oruzje)komponentaObj;
			Misija misija = Misija.Opisnici[oruzje.misija];

			StringBuilder sb = new StringBuilder(txtOpOpis.Text);
			sb.AppendLine();
			sb.AppendLine();
			for (int paramI = 0; paramI < misija.brParametara; paramI++) {
				Misija.Parametar parametar = misija.parametri[paramI];
				sb.Append(parametar.opis);
				if (parametar.tip == Misija.TipParameta.Cijelobrojni)
					sb.AppendLine(": " + Fje.PrefiksFormater(oruzje.parametri[paramI]));
				else if (parametar.tip == Misija.TipParameta.Postotak)
					sb.AppendLine(": x" + oruzje.parametri[paramI].ToString("0.##"));
			}
			if (misija.imaCiljanje)
				sb.AppendLine(jezik["opOruzjeCilj"].tekst() + ": " + Postavke.Jezik[Kontekst.Misije, Oruzje.OruzjeInfo.CiljanjeKod[oruzje.ciljanje]].tekst());
			sb.AppendLine();
			sb.AppendLine(jezik["opCijena"].tekst() + ": " + Fje.PrefiksFormater(oruzje.cijena));
			sb.AppendLine(jezik["opSnaga"].tekst() + ": " + Fje.PrefiksFormater(oruzje.snaga));
			sb.AppendLine(jezik["opVelicina"].tekst() + ": " + Fje.PrefiksFormater(oruzje.velicina));
			txtOpOpis.Text = sb.ToString();
		}
		private void prikazStita(IKomponenta komponentaObj)
		{
			Trup trup = ((TagTekst<Trup>)cbOpVelicine.SelectedItem).tag;
			Stit stit = (Stit)komponentaObj;
			stit = stit.info.naciniKomponentu(stit.nivo, trup.velicina_stita);
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormFlote];

			StringBuilder sb = new StringBuilder(txtOpOpis.Text);
			sb.AppendLine();
			sb.AppendLine();
			sb.AppendLine(jezik["opisStitIzd"].tekst() + ": " + Fje.PrefiksFormater(stit.izdrzljivost));
			sb.AppendLine(jezik["opisStitDeb"].tekst() + ": " + Fje.PrefiksFormater(stit.debljina));
			sb.AppendLine(jezik["opisStitObn"].tekst() + ": " + Fje.PrefiksFormater(stit.obnavljanje));
			sb.AppendLine(jezik["opisSenzorOm"].tekst() + ": x" + stit.ometanje.ToString("0.##"));
			sb.AppendLine(jezik["opisSenzorPrik"].tekst() + ": +" + Fje.PrefiksFormater(stit.prikrivanje));
			sb.AppendLine();
			jezik = Postavke.Jezik[Kontekst.FormTech];
			sb.AppendLine(jezik["opCijena"].tekst() + ": " + Fje.PrefiksFormater(stit.cijena));
			sb.AppendLine(jezik["opSnaga"].tekst() + ": " + Fje.PrefiksFormater(stit.snaga));
			sb.AppendLine(jezik["opVelicina"].tekst() + ": " + Fje.PrefiksFormater(trup.velicina_stita));
			txtOpOpis.Text = sb.ToString();
		}
		private void prikazSpecOp(IKomponenta komponentaObj)
		{
			Trup trup = ((TagTekst<Trup>)cbOpVelicine.SelectedItem).tag;
			SpecijalnaOprema so = (SpecijalnaOprema)komponentaObj;
			so = so.info.naciniKomponentu(so.nivo, trup.velicina);
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormFlote];

			StringBuilder sb = new StringBuilder(txtOpOpis.Text);
			sb.AppendLine();
			sb.AppendLine();
			foreach (string efekt in so.opisEfekata)
				sb.AppendLine(efekt);
			sb.AppendLine();
			jezik = Postavke.Jezik[Kontekst.FormTech];
			sb.AppendLine(jezik["opCijena"].tekst() + ": " + Fje.PrefiksFormater(so.cijena));
			sb.AppendLine(jezik["opVelicina"].tekst() + ": " + Fje.PrefiksFormater(so.velicina));
			txtOpOpis.Text = sb.ToString();
		}
		private void prikazOstalog(IKomponenta komponentaObj)
		{
			Trup trup = ((TagTekst<Trup>)cbOpVelicine.SelectedItem).tag;
			int indeks = lstOprema.SelectedIndices[0];

			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormFlote];
			Dictionary<string, ITekst> jezikTech = Postavke.Jezik[Kontekst.FormTech];
			StringBuilder sb = new StringBuilder(txtOpOpis.Text);
			sb.AppendLine();
			sb.AppendLine();

			if (indeks < opremaOstaloZadnjiIndeks[0]) {
				cbOpVelicine.Visible = true;
				MZPogon pogon = (MZPogon)komponentaObj;
				pogon = pogon.info.naciniKomponentu(pogon.nivo, trup.velicina_MZPogona);

				jezik = jezikTech;
				if (trup.velicina_MZPogona >= pogon.info.minimalnaVelicina(pogon.nivo)) {	
					sb.AppendLine(jezik["opMZbrzina"].tekst() + ": " + pogon.brzina.ToString("0.###"));
					sb.AppendLine();
					sb.AppendLine(jezik["opCijena"].tekst() + ": " + Fje.PrefiksFormater(pogon.cijena));
					sb.AppendLine(jezik["opSnaga"].tekst() + ": " + Fje.PrefiksFormater(pogon.snaga));
					sb.AppendLine(jezik["opVelicina"].tekst() + ": " + Fje.PrefiksFormater(trup.velicina_MZPogona));
				}
				else {
					sb.AppendLine(jezik["opMinVel"].tekst() + ": " +  Fje.PrefiksFormater(pogon.info.minimalnaVelicina(pogon.nivo)));
					sb.AppendLine(jezik["opNeStane"].tekst());
				}
			}
			else if (indeks < opremaOstaloZadnjiIndeks[1]) {
				cbOpVelicine.Visible = false;
				Potisnici potisnici = (Potisnici)komponentaObj;
				sb.AppendLine(jezik["opisPokret"].tekst() + ": " + Fje.PrefiksFormater(potisnici.brzina));
			}
			else if (indeks < opremaOstaloZadnjiIndeks[2]) {
				cbOpVelicine.Visible = true;
				Reaktor reaktor = (Reaktor)komponentaObj;
				reaktor = reaktor.info.naciniKomponentu(reaktor.nivo, trup.velicina_reaktora);

				sb.AppendLine(jezikTech["opMinVel"].tekst() + ": " + Fje.PrefiksFormater(reaktor.info.minimalnaVelicina(reaktor.nivo)));
				if (trup.velicina_reaktora >= reaktor.info.minimalnaVelicina(reaktor.nivo))
					sb.AppendLine(jezik["opisReaktorDost"].tekst() + ": " + Fje.PrefiksFormater(reaktor.snaga));
				else
					sb.AppendLine(jezikTech["opNeStane"].tekst());
			}
			else if (indeks < opremaOstaloZadnjiIndeks[3]) {
				cbOpVelicine.Visible = false;
				Senzor senzor = (Senzor)komponentaObj;
				sb.AppendLine(jezik["opisSenzorSn"].tekst() + ": " + Fje.PrefiksFormater(senzor.razlucivost));
			}
			txtOpOpis.Text = sb.ToString();
		}
		#endregion

		private static void premjestiListViewItem(int indeks1, int indeks2, ListView listView)
		{
			ListViewItem tmp = listView.Items[indeks1];
			listView.Items.RemoveAt(indeks1);
			listView.Items.Insert(indeks2, tmp);

			string tmpStr = listView.Items[indeks1].SubItems[3].Text;
			listView.Items[indeks1].SubItems[3].Text = listView.Items[indeks2].SubItems[3].Text;
			listView.Items[indeks2].SubItems[3].Text = tmpStr;
		}
		private static ListViewItem komponentaListViewItem(IKomponenta komponenta)
		{
			ListViewItem item = new ListViewItem(komponenta.naziv);
			item.SubItems.Add(komponenta.nivo.ToString());
			item.Tag = komponenta;
			return item;
		}
		private static ListViewItem specLVItem(string tekst, Font font)
		{
			ListViewItem item = new ListViewItem(tekst);
			item.Font = font;
			return item;
		}

		#region Razvoj
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
		
		private void lstRazvoj_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstRazvoj.SelectedItems.Count == 0) return;

			ListViewItem tmp = lstRazvoj.SelectedItems[0];
			Tehnologija teh = ((Tehnologija)tmp.Tag);
			txtRazOpis.Lines = teh.slijedeciNivoOpis.Split(new char[] { '\n' });
			picRazSlika.Image = teh.tip.slika;

			/*Tehnologija teh = (Tehnologija)lstKnjiznica.SelectedItems[0].Tag;
			picKnjizSlika.Image = teh.tip.slika;
			lblKnjizNaziv.Text = teh.tip.naziv;
			txtKnjizOpis.Lines = teh.opis.Split(new char[] { '\n' });*/
		}
		
		private void trkRazKoncentracija_Scroll(object sender, EventArgs e)
		{
			if (raspodijelaPoena != trkRazKoncentracija.Value)
			{
				raspodijelaPoena = trkRazKoncentracija.Value;
				izracunajPoeneRazvoja();
			}
		}
		#endregion

		#region Istrazivanje
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

		private void lstIstrazivanje_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstIstrazivanje.SelectedItems.Count == 0) return;

			ListViewItem tmp = lstIstrazivanje.SelectedItems[0];
			Tehnologija teh = ((Tehnologija)tmp.Tag);
			txtIstOpis.Lines = teh.slijedeciNivoOpis.Split(new char[] { '\n' });
			picIstSlika.Image = teh.tip.slika;
		}

		private void lstKnjiznica_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstKnjiznica.SelectedItems.Count == 0) return;
			if (lstKnjiznica.SelectedItems[0].Tag == null) return;

			Tehnologija teh = (Tehnologija)lstKnjiznica.SelectedItems[0].Tag;
			picKnjizSlika.Image = teh.tip.slika;
			lblKnjizNaziv.Text = teh.tip.naziv;
			txtKnjizOpis.Lines = teh.opis.Split(new char[] { '\n' });
		}
		#endregion

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

		private void cbOpKategorija_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbOpKategorija.SelectedItem == null) return;

			KategorijaOpreme kategorija = ((TagTekst<KategorijaOpreme>)cbOpKategorija.SelectedItem).tag;
			lstOprema.Items.Clear();
			lstOprema.Items.AddRange(opremaStavke[kategorija]);
		}

		private void prikaziStavku()
		{
			if (lstOprema.SelectedItems.Count == 0) return;
			if (lstOprema.SelectedItems[0].Tag == null) return;
			if (cbOpVelicine.SelectedItem == null) return;

			KategorijaOpreme kategorija = ((TagTekst<KategorijaOpreme>)cbOpKategorija.SelectedItem).tag;
			if (kategorija != KategorijaOpreme.Ostalo)
				cbOpVelicine.Visible = (kategorija == KategorijaOpreme.Stit || kategorija == KategorijaOpreme.Specijalno);

			IKomponenta komponenta = (IKomponenta)lstOprema.SelectedItems[0].Tag;
			uobicajeniPrikaz(komponenta);
			prikaziKomponenti[kategorija](komponenta);
		}

		private void lstOprema_SelectedIndexChanged(object sender, EventArgs e)
		{
			prikaziStavku();
		}

		private void cbOpVelicine_SelectedIndexChanged(object sender, EventArgs e)
		{
			prikaziStavku();
		}
	}
}
