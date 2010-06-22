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
	public partial class FormPlanetInfo : Form
	{
		private Kolonija kolonija;
		private static bool PrikazKolicina = true;

		public FormPlanetInfo()
		{
			InitializeComponent();
			if (PrikazKolicina)
				radKolicina.Select();
		}

		public FormPlanetInfo(Kolonija kolonija) : this()
		{
			this.kolonija = kolonija;
			this.Text = kolonija.ime;
			postaviTekstove();

			picSlika.Image = kolonija.slika;

			lblGravitacija.Text = "Gravitacija: " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.Gravitacija]);
			lblZracenje.Text = "Zračenje: " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.Zracenje]);
			lblAtmGustoca.Text = "Gustoča: " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.AtmGustoca]);
			lblAtmKvaliteta.Text = "Kvaliteta: " + (int)(kolonija.efekti[Kolonija.AtmKvaliteta] * 100) + "%";
			lblAtmoTemperatura.Text = "Temperatura: " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.Temperatura]);

			string predznak = "";
			if (kolonija.efekti[Kolonija.PopulacijaPromjena] < 0) predznak = "-";
			else if (kolonija.efekti[Kolonija.PopulacijaPromjena] > 0) predznak = "+";
			lblPopBr.Text = "Br. stanovnika: " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.Populacija]);
			lblPopDelta.Text = "Promjena: " + predznak + Fje.PrefiksFormater(kolonija.efekti[Kolonija.PopulacijaPromjena]);
			lblPopMax.Text = "Maksimum: " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.PopulacijaMax]);

			lblPoFarmeru.Text = "Po farmeru: " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.HranaPoFarmeru]);

			lblPoRudaru.Text = "Po rudaru: " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.RudePoRudaru]);
			lblMinPovrsina.Text = "Površina: " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.RudePovrsinske]);
			lblMinDubina.Text = "Dubina: " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.RudeDubinske]);

			lblOdrzavanjeGrav.Text = "Gravitacija: " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeGravitacija]);
			lblOdrzavanjeZrac.Text = "Zračenje: " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeZracenje]);
			lblOdrzavanjeKvalAtm.Text = "Kvaliteta: " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeAtmKvaliteta]);
			lblOdrzavanjeGustAtm.Text = "Gustoča: " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeAtmGustoca]);
			lblOdrzavanjeTempAtm.Text = "Temperatura: " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeTemperatura]);
			lblOdrzavanjeZgrada.Text = "Zgrade: " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeZgrada]);

			lblZgradaInfo.Text = "";
			osvjeziPogledNaKoloniju();
		}

		private void postaviTekstove()
		{
			if (kolonija == null) return;

			if (PrikazKolicina)
			{
				lblRadnaMjesta.Text = "Br. radnih mjesta: " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.RadnaMjesta]);
				lblBrFarmera.Text = "Br. farmera: " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.BrFarmera]);
				lblBrRudara.Text = "Br. rudara: " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.BrRudara]);
				lblOdrzavanjeUkupno.Text = "Ukupno: " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeUkupno]) + " ind";
			}
			else
			{
				lblRadnaMjesta.Text = "Br. radnih mjesta: " + String.Format("{0:0.##}", 100 * kolonija.efekti[Kolonija.RadnaMjesta] / kolonija.efekti[Kolonija.Populacija]) + "%";
				lblBrFarmera.Text = "Br. farmera: " + String.Format("{0:0.##}", 100 * kolonija.efekti[Kolonija.BrFarmera] / kolonija.efekti[Kolonija.Populacija]) + "%";
				lblBrRudara.Text = "Br. rudara: " + String.Format("{0:0.##}", 100 * kolonija.efekti[Kolonija.BrRudara] / kolonija.efekti[Kolonija.Populacija]) + "%";
				lblOdrzavanjeUkupno.Text = "Ukupno: " + String.Format("{0:0.##}", 100 * kolonija.efekti[Kolonija.BrOdrzavatelja] / kolonija.efekti[Kolonija.Populacija]) + "% pop";
			}
		}

		private void btnZatvori_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void osvjeziLabele()
		{
			lblCivilnaIndustrija.Text = Fje.PrefiksFormater(kolonija.poeniCivilneIndustrije()) + " ind";
			lblVojnaIndustrija.Text = Fje.PrefiksFormater(kolonija.poeniVojneIndustrije()) + " ind";
			lblProcjenaCivilneGradnje.Text = kolonija.procjenaVremenaCivilneGradnje();
			lblProcjenaVojneGradnje.Text = kolonija.procjenaVremenaVojneGradnje();
			lblRazvoj.Text = "Razvoj: " + Fje.PrefiksFormater(kolonija.poeniRazvoja());
			lblKoefOrbitalne.Text = "Cijena orbitalne gradnje: x"+kolonija.efekti[Kolonija.FaktorCijeneOrbitalnih].ToString("0.##");

			if (kolonija.redCivilneGradnje.Count > 0)
			{
				btnCivilnaGradnja.Image = kolonija.redCivilneGradnje.First.Value.slika;
				btnCivilnaGradnja.Text = "";
			}
			else
			{
				btnCivilnaGradnja.Image = null;
				btnCivilnaGradnja.Text = "Civilna gradnja";
			}

			if (kolonija.redVojneGradnje.Count > 0)
			{
				btnVojnaGradnja.Image = kolonija.redVojneGradnje.First.Value.slika;
				btnVojnaGradnja.Text = "";
			}
			else
			{
				btnVojnaGradnja.Image = null;
				btnVojnaGradnja.Text = "Vojna gradnja";
			}
		}

		private void osvjeziPogledNaKoloniju()
		{
			hscrCivilnaIndustrija.Value = (int)(kolonija.civilnaIndustrija * hscrCivilnaIndustrija.Maximum);
			hscrVojnaIndustrija.Value = (int)(kolonija.vojnaIndustrija * hscrVojnaIndustrija.Maximum);

			foreach (Zgrada z in kolonija.zgrade.Values)
				lstZgrade.Items.Add(z);

			osvjeziLabele();
		}

		private void btnCivilnaGradnja_Click(object sender, EventArgs e)
		{
            FormGradnja frmGradnja = new FormGradnja(kolonija, true);

			if (frmGradnja.ShowDialog() == DialogResult.OK)
				//osvjeziPogledNaKoloniju(true);
				osvjeziLabele();
		}

		private void btnVojnaGradnja_Click(object sender, EventArgs e)
		{
			FormGradnja frmGradnja = new FormGradnja(kolonija, false);

			if (frmGradnja.ShowDialog() == DialogResult.OK)
				//osvjeziPogledNaKoloniju(true);
				osvjeziLabele();
		}

		private void hscrIndustrijaRazvoj_Scroll(object sender, ScrollEventArgs e)
		{
			kolonija.civilnaIndustrija = hscrCivilnaIndustrija.Value / (double)hscrCivilnaIndustrija.Maximum;
			int val = (int)(kolonija.civilnaIndustrija * hscrCivilnaIndustrija.Maximum);
			if (hscrCivilnaIndustrija.Value != val)
				e.NewValue = val;
			else
				//osvjeziPogledNaKoloniju(true);
				osvjeziLabele();
		}

		private void hscrVojnaIndustrija_Scroll(object sender, ScrollEventArgs e)
		{
			kolonija.vojnaIndustrija = hscrVojnaIndustrija.Value / (double)hscrVojnaIndustrija.Maximum;
			int val = (int)(kolonija.vojnaIndustrija * hscrVojnaIndustrija.Maximum);
			if (hscrVojnaIndustrija.Value != val)
				e.NewValue = val;
			else
				//osvjeziPogledNaKoloniju(true);
				osvjeziLabele();
		}

		private void radKolicina_CheckedChanged(object sender, EventArgs e)
		{
			PrikazKolicina = true;
			postaviTekstove();
		}

		private void radPostotak_CheckedChanged(object sender, EventArgs e)
		{
			PrikazKolicina = false;
			postaviTekstove();
		}

		private void lstZgrade_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstZgrade.SelectedItem == null)
				return;

			Zgrada zgrada = (Zgrada)lstZgrade.SelectedItem;
			picZgrada.Image = zgrada.tip.slika;
			lblZgradaInfo.Text = zgrada.tip.opis;
		}
	}
}
