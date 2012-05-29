using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Igra;
using Zvjezdojedac.Igra.Bitka;
using Zvjezdojedac.Igra.Brodovi;
using Zvjezdojedac.Alati;
using Zvjezdojedac.GUI.Events;
using Zvjezdojedac.GUI.SmallData;

namespace Zvjezdojedac.GUI
{
	public partial class FormBorba : Form
	{
		const int TrackLargeQuantity = 1000;

		ModeratorBorbe borba;

		public FormBorba(ModeratorBorbe borba, Zvijezda lokacija)
		{
			this.borba = borba;

			InitializeComponent();
			
			this.Text = lokacija.ime;
			picSelectAll.Image = Zvjezdojedac.Podaci.Slike.BoraciSvi;

			foreach (var pozicija in Pozicije.PonudjenePozicije())
				cbPozicija.Items.Add(new TagTekst<int>(pozicija, Pozicije.Naziv(pozicija)));

			postaviPozicije();
			pocetakKruga();
		}

		private void pocetakKruga()
		{
			prikazZapovijedi(false);
			postaviPrikazStrana(pnlNapadac, borba.SviBorci, borba.LijevaStrana, false);
			postaviPrikazStrana(pnlObrana, borba.SviBorci, borba.DesnaStrana, true);

			if (borba.PreostaloKrugova == 0)
				lblBrKruga.Text = IgracevKonflikt.MaxBrPoteza + " / " + IgracevKonflikt.MaxBrPoteza;
			else
				lblBrKruga.Text = (IgracevKonflikt.MaxBrPoteza - borba.PreostaloKrugova + 1) + " / " + IgracevKonflikt.MaxBrPoteza;

			if (borba.Razrjeseno()) 
				btnKrajKruga.Text = "Close";
		}

		private void postaviPrikazStrana(FlowLayoutPanel panel, IEnumerable<Borac> borci, HashSet<Igrac> filtarIgraca, bool prikazPrikrivanja)
		{
			Dictionary<Dizajn, List<Borac>> borciPoDizajnu = new Dictionary<Dizajn, List<Borac>>();
			foreach (Borac borac in borci) {
				if (!filtarIgraca.Contains(borac.Igrac))
					continue;

				if (!borciPoDizajnu.ContainsKey(borac.Dizajn))
					borciPoDizajnu.Add(borac.Dizajn, new List<Borac>());
				borciPoDizajnu[borac.Dizajn].Add(borac);
			}

			List<Dizajn> dizajnovi = new List<Dizajn>(borciPoDizajnu.Keys);
			dizajnovi.Sort((a, b) => a.ime.CompareTo(b.ime));

			panel.SuspendLayout();
			panel.Controls.Clear();
			CombatantItem prviUGrupi = null;
			
			foreach (var dizajn in dizajnovi) {
				CombatantItem borciItem = new CombatantItem();
				borciItem.SetData(borciPoDizajnu[dizajn], prikazPrikrivanja);
				borciItem.OnSelect += borci_OnSelect;
				borciItem.Selectable = borba.LijevaStrana.Contains(borciPoDizajnu[dizajn][0].Igrac);

				if (prviUGrupi != null)
					borciItem.GroupWith(prviUGrupi);
				else
					prviUGrupi = borciItem;

				panel.Controls.Add(borciItem);
			}

			if (prviUGrupi != null && !prikazPrikrivanja)
				prviUGrupi.SelectThis();
			panel.ResumeLayout();
		}

		private void prikazZapovijedi(bool prikazi)
		{
			listPositions.Visible = prikazi;
			prikazZadavanjaZapovijedi(false);
		}

		private void prikazZadavanjaZapovijedi(bool prikazi)
		{
			lblZapovijed.Visible = prikazi;
			trackKolicina.Visible = prikazi;
			lblKolicina.Visible = prikazi;
			cbPozicija.Visible = prikazi;
			btnPosalji.Visible = prikazi;
		}

		private void postaviPozicije()
		{
			cpNapadac.ThisSidePlayers(borba.LijevaStrana);
			cpBranitelj.ThisSidePlayers(borba.DesnaStrana);

			cpNapadac.SetCombatants(borba.SviBorci);
			cpBranitelj.SetCombatants(borba.SviBorci);
		}

		private void cpNapadac_OnPositionClick(object sender, ObjectEventArgs<ICollection<Borac>> eventArgs)
		{
			prikazZapovijedi(false);
			postaviPrikazStrana(pnlNapadac, eventArgs.Value, borba.LijevaStrana, false);
			postaviPrikazStrana(pnlObrana, eventArgs.Value, borba.DesnaStrana, true);
			cpBranitelj.Deselect();
		}

		private void cpBranitelj_OnPositionClick(object sender, ObjectEventArgs<ICollection<Borac>> eventArgs)
		{
			prikazZapovijedi(false);
			postaviPrikazStrana(pnlNapadac, eventArgs.Value, borba.LijevaStrana, false);
			postaviPrikazStrana(pnlObrana, eventArgs.Value, borba.DesnaStrana, true);
			cpNapadac.Deselect();
		}

		private void picSelectAll_Click(object sender, EventArgs e)
		{
			postaviPrikazStrana(pnlNapadac, borba.SviBorci, borba.LijevaStrana, false);
			postaviPrikazStrana(pnlObrana, borba.SviBorci, borba.DesnaStrana, true);
			cpBranitelj.Deselect();
			cpNapadac.Deselect();
		}

		private void borci_OnSelect(object sender, ObjectEventArgs<ICollection<Borac>> eventArgs)
		{
			if (!borba.LijevaStrana.Contains(eventArgs.Value.First().Igrac)) {
				prikazZapovijedi(false);
				return;
			}

			prikazZapovijedi(true);

			Dictionary<int, List<Borac>> raspodjelaBoraca = new Dictionary<int, List<Borac>>();
			for(int slot = 0; slot <= Pozicije.MaxPozicija; slot++)
				raspodjelaBoraca.Add(slot, new List<Borac>());
			foreach (Borac borac in eventArgs.Value)
				raspodjelaBoraca[borac.CiljnaPozicija].Add(borac);

			int maxKolicina = 0;
			listPositions.Items.Clear();
			for (int slot = 0; slot <= Pozicije.MaxPozicija; slot++) {
				raspodjelaBoraca[slot].Sort(
					(a, b) => a.IzdrzljivostOklopa.CompareTo(b.IzdrzljivostOklopa));

				listPositions.Items.Add(new TagTekst<List<Borac>>(
					raspodjelaBoraca[slot],
					Fje.PrefiksFormater(raspodjelaBoraca[slot].Count) + " " + Pozicije.Naziv(slot)));

				if (raspodjelaBoraca[slot].Count > raspodjelaBoraca[maxKolicina].Count)
					maxKolicina = slot;
			}
			listPositions.SelectedIndex = maxKolicina;
		}

		private void listPositions_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listPositions.SelectedItem == null) {
				prikazZadavanjaZapovijedi(false);
				return;
			}

			List<Borac> borci = (listPositions.SelectedItem as TagTekst<List<Borac>>).tag;
			if (borci.Count == 0) {
				prikazZadavanjaZapovijedi(false);
				return;
			} else
				prikazZadavanjaZapovijedi(true);

			int odrediste = borci[0].CiljnaPozicija;
			
			int cbIndex = 0;
			for(int i = 0; i < cbPozicija.Items.Count; i++)
				if ((cbPozicija.Items[i] as TagTekst<int>).tag == odrediste)
					cbIndex = i;
			cbPozicija.SelectedIndex = cbIndex;

			if (borci.Count < 20) {
				trackKolicina.Maximum = borci.Count;
				trackKolicina.LargeChange = Math.Min(borci.Count, 2);
				trackKolicina.SmallChange = 1;
				trackKolicina.TickFrequency = 1;
			}
			else {
				int trackMaximum = Math.Min(borci.Count, TrackLargeQuantity);
				trackKolicina.Maximum = trackMaximum;
				trackKolicina.LargeChange = trackMaximum / 20;
				trackKolicina.SmallChange = Math.Max(trackMaximum / 100, 1);
				trackKolicina.TickFrequency = trackKolicina.LargeChange;
			}

			lblKolicina.Text = Fje.PrefiksFormater(borci.Count);
		}

		private void trackKolicina_Scroll(object sender, EventArgs e)
		{
			if (listPositions.SelectedItem == null)
				return;

			List<Borac> borci = (listPositions.SelectedItem as TagTekst<List<Borac>>).tag;
			if (borci.Count == 0)
				return;

			int kolicina = (borci.Count > TrackLargeQuantity) ?
				(int)(borci.Count * (trackKolicina.Value / (double)TrackLargeQuantity)) :
				trackKolicina.Value;

			lblKolicina.Text = Fje.PrefiksFormater(kolicina);
		}

		private void btnPosalji_Click(object sender, EventArgs e)
		{
			if (listPositions.SelectedItem == null || cbPozicija.SelectedItem == null)
				return;

			List<Borac> borci = (listPositions.SelectedItem as TagTekst<List<Borac>>).tag;
			if (borci.Count == 0)
				return;

			int kolicina = (borci.Count > TrackLargeQuantity) ?
				(int)(borci.Count * (trackKolicina.Value / (double)TrackLargeQuantity)) :
				trackKolicina.Value;

			int novoOdrediste = (cbPozicija.SelectedItem as TagTekst<int>).tag;
			int staroOdrediste = borci[0].CiljnaPozicija;
			int indeksOd, indeksDo;
			if (novoOdrediste > staroOdrediste) {
				indeksOd = 0;
				indeksDo = kolicina;
			}
			else {
				indeksOd = borci.Count - kolicina;
				indeksDo = borci.Count;
			}

			List<Borac> odredisniaLista = (listPositions.Items[novoOdrediste] as TagTekst<List<Borac>>).tag;
			odredisniaLista.AddRange(borci.GetRange(indeksOd, kolicina));
			borci.RemoveRange(indeksOd, kolicina);
			listPositions.Items[staroOdrediste] = new TagTekst<List<Borac>>(borci, Fje.PrefiksFormater(borci.Count) + " " + Pozicije.Naziv(staroOdrediste));
			listPositions.Items[novoOdrediste] = new TagTekst<List<Borac>>(odredisniaLista, Fje.PrefiksFormater(odredisniaLista.Count) + " " + Pozicije.Naziv(novoOdrediste));
			listPositions.SelectedIndex = novoOdrediste;
		}

		private void btnKrajKruga_Click(object sender, EventArgs e)
		{
			if (borba.Razrjeseno())
				Close();
			else {
				borba.SlijedecaFaza();

				cpNapadac.SetCombatants(borba.SviBorci);
				cpBranitelj.SetCombatants(borba.SviBorci);

				pocetakKruga();
			}
		}
	}
}

