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

namespace Zvjezdojedac.GUI
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
			postaviJezik();

			radKolicina.Checked = true;
			this.Font = Postavke.FontSucelja(this.Font);
		}

		public FormPlanetInfo(Kolonija kolonija) : this()
		{
			this.kolonija = kolonija;
			this.Text = kolonija.ime;
			postaviTekstove();

			picSlika.Image = kolonija.slika;
			
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormPlanetInfo];

			lblVelicina.Text = jezik["plVelicina"].tekst() + ": " + Fje.PrefiksFormater(kolonija.planet.velicina);
			lblGravitacija.Text = jezik["plGravitacija"].tekst() + ": " + String.Format("{0:0.##}", kolonija.Efekti[Kolonija.Gravitacija]);
			lblZracenje.Text = jezik["plZracenje"].tekst() + ": " + String.Format("{0:0.##}", kolonija.Efekti[Kolonija.Zracenje]);
			lblAtmGustoca.Text = jezik["plAtmGustoca"].tekst() + ": " + String.Format("{0:0.##}", kolonija.Efekti[Kolonija.AtmGustoca]);
			lblAtmKvaliteta.Text = jezik["plAtmKvaliteta"].tekst() + ": " + (int)(kolonija.Efekti[Kolonija.AtmKvaliteta] * 100) + "%";
			lblAtmTemperatura.Text = jezik["plAtmTemperatura"].tekst() + ": " + String.Format("{0:0.##}", kolonija.Efekti[Kolonija.Temperatura]);

			string predznak = "";
			if (kolonija.Efekti[Kolonija.PopulacijaPromjena] < 0) predznak = "-";
			else if (kolonija.Efekti[Kolonija.PopulacijaPromjena] > 0) predznak = "+";
			
			Dictionary<string, ITekst> jezikKl = Postavke.Jezik[Kontekst.Kolonija];
			lblPopBr.Text = jezikKl["plPopulacija"].tekst() + ": " + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.PopulacijaBr]);
			lblPopDelta.Text = jezikKl["plPromjenaPop"].tekst() + ": " + predznak + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.PopulacijaPromjena]);
			lblPopMax.Text = jezikKl["plPopMax"].tekst() + ": " + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.PopulacijaMax]);

			lblMinPovrsina.Text = jezikKl["plMinPovrsina"].tekst() + ": " + String.Format("{0:0.##}", kolonija.Efekti[Kolonija.RudePovrsinske]);
			lblMinDubina.Text = jezikKl["plMinDubina"].tekst() + ": " + String.Format("{0:0.##}", kolonija.Efekti[Kolonija.RudeDubinske]);
			lblMinOstvareno.Text = jezikKl["plMinOstvareno"].tekst() + ": " + String.Format("{0:0.##}", kolonija.Efekti[Kolonija.RudeEfektivno]);

			lblOdrzavanjeGrav.Text = jezik["plGravitacija"].tekst() + ": " + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.OdrzavanjeGravitacija]);
			lblOdrzavanjeZrac.Text = jezik["plZracenje"].tekst() + ": " + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.OdrzavanjeZracenje]);
			lblOdrzavanjeKvalAtm.Text = jezik["plAtmKvaliteta"].tekst() + ": " + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.OdrzavanjeAtmKvaliteta]);
			lblOdrzavanjeGustAtm.Text = jezik["plAtmGustoca"].tekst() + ": " + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.OdrzavanjeAtmGustoca]);
			lblOdrzavanjeTempAtm.Text = jezik["plAtmTemperatura"].tekst() + ": " + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.OdrzavanjeTemperatura]);
			lblOdrzavanjeZgrada.Text = jezik["plZgrade"].tekst() + ": " + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.OdrzavanjeZgrada]);
			lblOdrzavanjeUkupno.Text = jezik["ukupno"].tekst() + ": " + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.OdrzavanjeUkupno]);

			lblZgradaInfo.Text = "";
			osvjeziPogledNaKoloniju();
		}

		public FormPlanetInfo(Igrac igrac, Planet planet)
			: this()
		{
			this.kolonija = new Kolonija(igrac, planet, 100000, 0);
			this.Text = planet.ime;
			postaviTekstove();

			lblPrikaz.Visible = false;
			radKolicina.Visible = false;
			radPostotak.Visible = false;

			picSlika.Image = planet.slika;

			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormPlanetInfo];
			lblVelicina.Text = jezik["plVelicina"].tekst() + ": " + Fje.PrefiksFormater(planet.velicina);
			lblGravitacija.Text = jezik["plGravitacija"].tekst() + ": " + String.Format("{0:0.##}", planet.gravitacija());
			lblZracenje.Text = jezik["plZracenje"].tekst() + ": " + String.Format("{0:0.##}", planet.ozracenost());
			lblAtmGustoca.Text = jezik["plAtmGustoca"].tekst() + ": " + String.Format("{0:0.##}", planet.gustocaAtmosfere);
			lblAtmKvaliteta.Text = jezik["plAtmKvaliteta"].tekst() + ": " + (int)(planet.kvalitetaAtmosfere * 100) + "%";
			lblAtmTemperatura.Text = jezik["plAtmTemperatura"].tekst() + ": " + String.Format("{0:0.##}", planet.temperatura());
			lblKoefOrbitalne.Text = jezik["plCijenaOrbGradnje"].tekst() + ": x" + kolonija.Efekti[Kolonija.FaktorCijeneOrbitalnih].ToString("0.##");

			Dictionary<string, ITekst> jezikKl = Postavke.Jezik[Kontekst.Kolonija];
			lblPopBr.Text = jezikKl["plNenaseljeno"].tekst();
			lblPopDelta.Text = "";
			lblPopMax.Text = "";
			lblRadnaMjesta.Text = "";

			tabControl.TabPages.Clear();
			tabControl.TabPages.Add(tabPageResursi);

			groupRadnici.Hide();

			lblMinPovrsina.Text = jezikKl["plMinPovrsina"].tekst() + ": " + String.Format("{0:0.##}", kolonija.Efekti[Kolonija.RudePovrsinske]);
			lblMinDubina.Text = jezikKl["plMinDubina"].tekst() + ": " + String.Format("{0:0.##}", kolonija.Efekti[Kolonija.RudeDubinske]);
			lblMinOstvareno.Text = jezikKl["plMinOstvareno"].tekst() + ": " + String.Format("{0:0.##}", kolonija.Efekti[Kolonija.RudeEfektivno]);
		}

		private void postaviJezik()
		{
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormPlanetInfo];
			btnZatvori.Text = jezik["btnZatvori"].tekst();
			groupPlanet.Text = jezik["groupPlanet"].tekst();
			groupPopulacija.Text = jezik["groupPopulacija"].tekst();
			groupPoStan.Text = jezik["groupPoStan"].tekst();
			groupRadnici.Text = jezik["groupRadnici"].tekst();
			groupRude.Text = jezik["groupRude"].tekst();
			lblAtmosfera1.Text = jezik["lblAtmosfera"].tekst();
			lblAtmosfera2.Text = jezik["lblAtmosfera"].tekst();
			lblPrikaz.Text = jezik["lblPrikaz"].tekst();
			radKolicina.Text = jezik["radKolicina"].tekst();
			radPostotak.Text = jezik["radPostotak"].tekst();
			tabPageOdrzavanje.Text = jezik["tabPageOdrz"].tekst();
			tabPageProizvodnja.Text = jezik["tabPageProiz"].tekst();
			tabPageResursi.Text = jezik["tabPageResursi"].tekst();
			tabPageZgrade.Text = jezik["tabPageZgrade"].tekst();
		}

		private void postaviTekstove()
		{
			if (kolonija == null) return;

			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.Kolonija];

			double brOdrzavatelja = kolonija.Efekti[Kolonija.BrOdrzavatelja] * (1 + kolonija.Efekti[Kolonija.RudariPoOdrzavatelju]);

			if (PrikazKolicina)
			{
				lblRadnaMjesta.Text = jezik["plRadnaMjesta"].tekst() + ": " + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.RadnaMjesta]);
				lblBrFarmera.Text = jezik["plBrFarmera"].tekst() + ": " + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.BrFarmera]);
				//lblBrRudara.Text = jezik["plBrRudara"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.BrRudara]);
				lblBrOdrzavatelja.Text = jezik["plBrOdrz"].tekst() + ": " + Fje.PrefiksFormater(brOdrzavatelja);
				lblBrRadnika.Text = jezik["plBrRadnika"].tekst() + ": " + Fje.PrefiksFormater(kolonija.Efekti[Kolonija.BrRadnika]);
			}
			else
			{
				double koef = 100 / kolonija.Efekti[Kolonija.PopulacijaBr];
				lblRadnaMjesta.Text = jezik["plRadnaMjesta"].tekst() + ": " + String.Format("{0:0.##}", koef * kolonija.Efekti[Kolonija.RadnaMjesta]) + "%";
				lblBrFarmera.Text = jezik["plBrFarmera"].tekst() + ": " + String.Format("{0:0.##}", koef * kolonija.Efekti[Kolonija.BrFarmera]) + "%";
				//lblBrRudara.Text = jezik["plBrRudara"].tekst() + ": " + String.Format("{0:0.##}", koef * kolonija.efekti[Kolonija.BrRudara]) + "%";
				lblBrOdrzavatelja.Text = jezik["plBrOdrz"].tekst() + ": " + String.Format("{0:0.##}", koef * brOdrzavatelja) + "%";
				lblBrRadnika.Text = jezik["plBrRadnika"].tekst() + ": " + String.Format("{0:0.##}", koef * kolonija.Efekti[Kolonija.BrRadnika]) + "%";
			}

			Dictionary<string, double> maxEfekti = kolonija.maxEfekti();
			lblHranaPoStan.Text = jezik["HranaPoStan"].tekst() + String.Format(": {0:0.##} / {1:0.##}", kolonija.Efekti[Kolonija.HranaPoFarmeru], maxEfekti[Kolonija.HranaPoFarmeru]);
			lblRudePoStan.Text = jezik["RudePoStan"].tekst() + String.Format(": {0:0.##} / {1:0.##}", kolonija.Efekti[Kolonija.RudePoRudaru], maxEfekti[Kolonija.RudePoRudaru]);
			lblOdrzavanjePoStan.Text = jezik["OdrzavanjePoStan"].tekst() + String.Format(": {0:0.##}", (kolonija.OdrzavanjePoStan));
			lblIndustrijaPoStan.Text = jezik["IndustrijaPoStan"].tekst() + String.Format(": {0:0.##} / {1:0.##}", kolonija.Efekti[Kolonija.IndPoRadnikuEfektivno], maxEfekti[Kolonija.IndPoRadnikuEfektivno]);
			lblRazvojPoStan.Text = jezik["RazvojPoStan"].tekst() + String.Format(": {0:0.##} / {1:0.##}", kolonija.Efekti[Kolonija.RazPoRadnikuEfektivno], maxEfekti[Kolonija.RazPoRadnikuEfektivno]);
		}

		private void btnZatvori_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void osvjeziLabele()
		{
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.Kolonija];

			lblCivilnaIndustrija.Text = Fje.PrefiksFormater(kolonija.poeniIndustrije()) + " " + jezik["jedInd"].tekst();
			lblProcjenaCivilneGradnje.Text = kolonija.ProcjenaVremenaGradnje();
			lblRazvoj.Text = jezik["lblRazvoj"].tekst() + Fje.PrefiksFormater(kolonija.poeniRazvoja());
			lblKoefOrbitalne.Text = Postavke.Jezik[Kontekst.FormPlanetInfo, "plCijenaOrbGradnje"].tekst() + ": x" + kolonija.Efekti[Kolonija.FaktorCijeneOrbitalnih].ToString("0.##");

			if (kolonija.RedGradnje.Count > 0)
			{
				btnCivilnaGradnja.Image = kolonija.RedGradnje.First.Value.slika;
				btnCivilnaGradnja.Text = "";
			}
			else
			{
				btnCivilnaGradnja.Image = null;
				btnCivilnaGradnja.Text = jezik["Civilna_Gradnja"].tekst();
			}
		}

		private void osvjeziPogledNaKoloniju()
		{
			hscrCivilnaIndustrija.Value = (int)(kolonija.UdioIndustrije * hscrCivilnaIndustrija.Maximum);

			foreach (Zgrada z in kolonija.Zgrade.Values)
				lstZgrade.Items.Add(z);

			osvjeziLabele();
		}

		private void btnCivilnaGradnja_Click(object sender, EventArgs e)
		{
            FormGradnja frmGradnja = new FormGradnja(kolonija);

			if (frmGradnja.ShowDialog() == DialogResult.OK)
				osvjeziLabele();
		}

		private void btnVojnaGradnja_Click(object sender, EventArgs e)
		{
			FormGradnja frmGradnja = new FormGradnja(kolonija);

			if (frmGradnja.ShowDialog() == DialogResult.OK)
				osvjeziLabele();
		}

		private void hscrIndustrijaRazvoj_Scroll(object sender, ScrollEventArgs e)
		{
			kolonija.UdioIndustrije = e.NewValue / (double)hscrCivilnaIndustrija.Maximum;
			int val = (int)Math.Round(kolonija.UdioIndustrije * hscrCivilnaIndustrija.Maximum, MidpointRounding.AwayFromZero);
			if (hscrCivilnaIndustrija.Value != val)
				e.NewValue = val;
			else
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
			lblZgradaInfo.Text = zgrada.tip.Opis;
		}
	}
}
