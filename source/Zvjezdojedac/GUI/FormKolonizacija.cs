using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alati;

namespace Prototip
{
	public partial class FormKolonizacija : Form
	{
		private IgraZvj igra;
		private Igrac igrac;
		private Brod brod;
		private Zvijezda zvijezda;

		private long brodPopulacija = 0;
		private long brodRadnaMjesta = 0;
		private long[] brBrodova = new long[Mapa.GraditeljMape.BR_PLANETA];
		private long[] dodatnaPopulacija = new long[Mapa.GraditeljMape.BR_PLANETA];

		public FormKolonizacija(IgraZvj igra, Igrac igrac, Brod brod, Zvijezda zvijezda)
		{
			InitializeComponent();
		
			groupPlanet.Hide();
			groupPoStan.Hide();
			groupRude.Hide();

			hscrBrBrodova.Minimum = 0;
			hscrBrBrodova.Maximum = 40;
			hscrBrBrodova.Hide();

			lblBrBrodova.Hide();
			lblBrStanovnika.Hide();
			lblBrRadnihMjesta.Hide();

			this.igra = igra;
			this.igrac = igrac;
			this.brod = brod;
			this.zvijezda = zvijezda;

			brodPopulacija = brod.dizajn.populacija;
			brodRadnaMjesta = brod.dizajn.radnaMjesta;
			
			lstvPlaneti.LargeImageList = new ImageList();
			lstvPlaneti.LargeImageList.ImageSize = new Size(32, 32);
			Image[] planetImages = new Image[Slike.PlanetImageIndex.Count];
			foreach (Image img in Slike.PlanetImageIndex.Keys)
				planetImages[Slike.PlanetImageIndex[img]] = img;
			lstvPlaneti.LargeImageList.Images.AddRange(planetImages);

			lstvPlaneti.Items.Clear();
			for (int i = 0; i < zvijezda.planeti.Count; i++) {
				Planet planet = zvijezda.planeti[i];
				ListViewItem item = new ListViewItem();
				item.ImageIndex = Slike.PlanetImageIndex[planet.slika];

				if (planet.tip != Planet.Tip.NIKAKAV) {
					item.Text = planet.ime;
					if (planet.kolonija != null)
						item.ForeColor = planet.kolonija.igrac.boja;
				}
				lstvPlaneti.Items.Add(item);
			}

			foreach (Flota.Kolonizacija kolonizacija in igrac.floteStacionarne[zvijezda].kolonizacije)
				if (kolonizacija.brod == brod)
					brBrodova[kolonizacija.planet] += kolonizacija.brBrodova;
				else
					dodatnaPopulacija[kolonizacija.planet] += kolonizacija.brod.dizajn.populacija * kolonizacija.brBrodova;

			postaviZvjezdice();
		}

		private long maxBrodova()
		{
			long rez = brod.kolicina;
			int planetI = lstvPlaneti.SelectedIndices[0];

			for (int i = 0; i < brBrodova.Length; i++ )
				if (i != planetI)
					rez -= brBrodova[i];

			double planetMax = 0;
			if (zvijezda.planeti[planetI].kolonija == null){
				Kolonija kolonija = new Kolonija(igrac, zvijezda.planeti[planetI], brodPopulacija, brodRadnaMjesta);
				planetMax = (kolonija.efekti[Kolonija.PopulacijaMax] - dodatnaPopulacija[planetI]) / brodPopulacija;
			} else{
				Kolonija kolonija = zvijezda.planeti[planetI].kolonija;
				planetMax = (kolonija.efekti[Kolonija.PopulacijaMax] - kolonija.populacija - dodatnaPopulacija[planetI]) / brodPopulacija;
			}

			return (long)Math.Min(rez, Math.Ceiling(planetMax));
		}

		private void postaviBrBrodova()
		{
			int planetI = lstvPlaneti.SelectedIndices[0];

			lblBrBrodova.Text = "Br. brodova: " + Fje.PrefiksFormater(brBrodova[planetI]);
			lblBrStanovnika.Text = "Br. stanovnika: " + Fje.PrefiksFormater(brBrodova[planetI] * brodPopulacija);
			lblBrRadnihMjesta.Text = "Radna mjesta: " + Fje.PrefiksFormater(brBrodova[planetI] * brodRadnaMjesta);

			hscrBrBrodova.Enabled = false;
			hscrBrBrodova.Value = Fje.Ogranici((int)(hscrBrBrodova.Maximum * (Math.Log(brBrodova[planetI]) / Math.Log(maxBrodova()))), hscrBrBrodova.Minimum, hscrBrBrodova.Maximum);
			hscrBrBrodova.Enabled = true;
		}

		private void postaviZvjezdice()
		{
			for (int i = 0; i < zvijezda.planeti.Count; i++)
				if (brBrodova[i] > 0 || dodatnaPopulacija[i] > 0)
					lstvPlaneti.Items[i].Text = "* " + zvijezda.planeti[i].ime;
				else if (zvijezda.planeti[i].tip != Planet.Tip.NIKAKAV)
					lstvPlaneti.Items[i].Text = zvijezda.planeti[i].ime;
		}

		private void btnPrihvati_Click(object sender, EventArgs e)
		{
			igrac.floteStacionarne[zvijezda].dodajKolonizacije(brod, brBrodova);
			Close();
		}

		private void lstvPlaneti_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstvPlaneti.SelectedItems.Count == 0) return;

			Planet planet = zvijezda.planeti[lstvPlaneti.SelectedIndices[0]];

			if (planet.tip == Planet.Tip.NIKAKAV) {
				groupPlanet.Hide();
				groupPoStan.Hide();
				groupRude.Hide();

				hscrBrBrodova.Hide();
				lblBrBrodova.Hide();
				lblBrStanovnika.Hide();
				lblBrRadnihMjesta.Hide();
			}
			else {
				groupPlanet.Show();
				groupPoStan.Show();
				groupRude.Show();

				hscrBrBrodova.Show();
				lblBrBrodova.Show();
				lblBrStanovnika.Show();
				lblBrRadnihMjesta.Show();

				Kolonija kolonija = (planet.kolonija == null) ?
					new Kolonija(igrac, planet, brodPopulacija, brodRadnaMjesta) :
					planet.kolonija;

				lblVelicina.Text = "Veličina: " + Fje.PrefiksFormater(planet.velicina);
				lblGravitacija.Text = "Gravitacija: " + String.Format("{0:0.##}", planet.gravitacija());
				lblZracenje.Text = "Zračenje: " + String.Format("{0:0.##}", planet.ozracenost());
				lblAtmGustoca.Text = "Gustoča: " + String.Format("{0:0.##}", planet.gustocaAtmosfere);
				lblAtmKvaliteta.Text = "Kvaliteta: " + (int)(planet.kvalitetaAtmosfere * 100) + "%";
				lblAtmoTemperatura.Text = "Temperatura: " + String.Format("{0:0.##}", planet.temperatura());
				lblKoefOrbitalne.Text = "Cijena orbitalne gradnje: x" + kolonija.efekti[Kolonija.FaktorCijeneOrbitalnih].ToString("0.##");

				lblMinPovrsina.Text = "Površina: " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.RudePovrsinske]);
				lblMinDubina.Text = "Dubina: " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.RudeDubinske]);
				lblMinOstvareno.Text = "Ostvareno: " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.RudeEfektivno]);

				Dictionary<string, double> maxEfekti = kolonija.maxEfekti();
				lblHranaPoStan.Text = String.Format("Po farmeru: {0:0.##} / {1:0.##}", kolonija.efekti[Kolonija.HranaPoFarmeru], maxEfekti[Kolonija.HranaPoFarmeru]);
				lblRudePoStan.Text = String.Format("Po rudaru: {0:0.##} / {1:0.##}", kolonija.efekti[Kolonija.RudePoRudaru], maxEfekti[Kolonija.RudePoRudaru]);
				lblOdrzavanjePoStan.Text = String.Format("Održavanje: {0:0.##}", (kolonija.efekti[Kolonija.OdrzavanjeUkupno] / kolonija.efekti[Kolonija.Populacija]));
				lblIndustrijaPoStan.Text = String.Format("Industrija: {0:0.##} / {1:0.##}", kolonija.efekti[Kolonija.IndustrijaPoRadniku], maxEfekti[Kolonija.IndustrijaPoRadniku]);
				lblRazvojPoStan.Text = String.Format("Razvoj: {0:0.##} / {1:0.##}", kolonija.efekti[Kolonija.RazvojPoRadniku], maxEfekti[Kolonija.RazvojPoRadniku]);

				postaviBrBrodova();
			}
		}

		private void hscrBrBrodova_Scroll(object sender, ScrollEventArgs e)
		{
			int planetI = lstvPlaneti.SelectedIndices[0];
			brBrodova[planetI] = (e.NewValue == 0) ? 0 : (long)Math.Pow(maxBrodova(), e.NewValue / (double)hscrBrBrodova.Maximum);

			lblBrBrodova.Text = "Br. brodova: " + Fje.PrefiksFormater(brBrodova[planetI]);
			lblBrStanovnika.Text = "Br. stanovnika: " + Fje.PrefiksFormater(brBrodova[planetI] * brodPopulacija);
			lblBrRadnihMjesta.Text = "Radna mjesta: " + Fje.PrefiksFormater(brBrodova[planetI] * brodRadnaMjesta);

			if (e.NewValue == 0 || e.OldValue == 0)
				postaviZvjezdice();
		}
	}
}
