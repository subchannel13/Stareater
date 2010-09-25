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
		}

		public FormPlanetInfo(Kolonija kolonija) : this()
		{
			this.kolonija = kolonija;
			this.Text = kolonija.ime;
			postaviTekstove();

			picSlika.Image = kolonija.slika;
			
			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormPlanetInfo];

			lblVelicina.Text = jezik["plVelicina"].tekst() + ": " + Fje.PrefiksFormater(kolonija.planet.velicina);
			lblGravitacija.Text = jezik["plGravitacija"].tekst() + ": " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.Gravitacija]);
			lblZracenje.Text = jezik["plZracenje"].tekst() + ": " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.Zracenje]);
			lblAtmGustoca.Text = jezik["plAtmGustoca"].tekst() + ": " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.AtmGustoca]);
			lblAtmKvaliteta.Text = jezik["plAtmKvaliteta"].tekst() + ": " + (int)(kolonija.efekti[Kolonija.AtmKvaliteta] * 100) + "%";
			lblAtmTemperatura.Text = jezik["plAtmTemperatura"].tekst() + ": " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.Temperatura]);

			string predznak = "";
			if (kolonija.efekti[Kolonija.PopulacijaPromjena] < 0) predznak = "-";
			else if (kolonija.efekti[Kolonija.PopulacijaPromjena] > 0) predznak = "+";
			
			Dictionary<string, ITekst> jezikKl = Postavke.jezik[Kontekst.Kolonija];
			lblPopBr.Text = jezikKl["plPopulacija"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.Populacija]);
			lblPopDelta.Text = jezikKl["plPromjenaPop"].tekst() + ": " + predznak + Fje.PrefiksFormater(kolonija.efekti[Kolonija.PopulacijaPromjena]);
			lblPopMax.Text = jezikKl["plPopMax"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.PopulacijaMax]);

			lblMinPovrsina.Text = jezikKl["plMinPovrsina"].tekst() + ": " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.RudePovrsinske]);
			lblMinDubina.Text = jezikKl["plMinDubina"].tekst() + ": " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.RudeDubinske]);
			lblMinOstvareno.Text = jezikKl["plMinOstvareno"].tekst() + ": " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.RudeEfektivno]);

			lblOdrzavanjeGrav.Text = jezik["plGravitacija"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeGravitacija]);
			lblOdrzavanjeZrac.Text = jezik["plZracenje"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeZracenje]);
			lblOdrzavanjeKvalAtm.Text = jezik["plAtmKvaliteta"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeAtmKvaliteta]);
			lblOdrzavanjeGustAtm.Text = jezik["plAtmGustoca"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeAtmGustoca]);
			lblOdrzavanjeTempAtm.Text = jezik["plAtmTemperatura"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeTemperatura]);
			lblOdrzavanjeZgrada.Text = jezik["plZgrade"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeZgrada]);
			lblOdrzavanjeUkupno.Text = jezik["ukupno"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.OdrzavanjeUkupno]);

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

			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormPlanetInfo];
			lblVelicina.Text = jezik["plVelicina"].tekst() + ": " + Fje.PrefiksFormater(planet.velicina);
			lblGravitacija.Text = jezik["plGravitacija"].tekst() + ": " + String.Format("{0:0.##}", planet.gravitacija());
			lblZracenje.Text = jezik["plZracenje"].tekst() + ": " + String.Format("{0:0.##}", planet.ozracenost());
			lblAtmGustoca.Text = jezik["plAtmGustoca"].tekst() + ": " + String.Format("{0:0.##}", planet.gustocaAtmosfere);
			lblAtmKvaliteta.Text = jezik["plAtmKvaliteta"].tekst() + ": " + (int)(planet.kvalitetaAtmosfere * 100) + "%";
			lblAtmTemperatura.Text = jezik["plAtmTemperatura"].tekst() + ": " + String.Format("{0:0.##}", planet.temperatura());
			lblKoefOrbitalne.Text = jezik["plCijenaOrbGradnje"].tekst() + ": x" + kolonija.efekti[Kolonija.FaktorCijeneOrbitalnih].ToString("0.##");

			Dictionary<string, ITekst> jezikKl = Postavke.jezik[Kontekst.Kolonija];
			lblPopBr.Text = jezik["plNenaseljeno"].tekst();
			lblPopDelta.Text = "";
			lblPopMax.Text = "";
			lblRadnaMjesta.Text = "";

			tabControl.TabPages.Clear();
			tabControl.TabPages.Add(tabPageResursi);

			groupRadnici.Hide();

			lblMinPovrsina.Text = jezikKl["plMinPovrsina"].tekst() + ": " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.RudePovrsinske]);
			lblMinDubina.Text = jezikKl["plMinDubina"].tekst() + ": " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.RudeDubinske]);
			lblMinOstvareno.Text = jezikKl["plMinOstvareno"].tekst() + ": " + String.Format("{0:0.##}", kolonija.efekti[Kolonija.RudeEfektivno]);
		}

		private void postaviJezik()
		{
			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormPlanetInfo];
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

			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.Kolonija];

			if (PrikazKolicina)
			{
				lblRadnaMjesta.Text = jezik["plRadnaMjesta"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.RadnaMjesta]);
				lblBrFarmera.Text = jezik["plBrFarmera"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.BrFarmera]);
				lblBrRudara.Text = jezik["plBrRudara"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.BrRudara]);
				lblBrOdrzavatelja.Text = jezik["plBrOdrz"].tekst() + ": " + Fje.PrefiksFormater(kolonija.efekti[Kolonija.BrOdrzavatelja]);
			}
			else
			{
				double koef = 100 / kolonija.efekti[Kolonija.Populacija];
				lblRadnaMjesta.Text = jezik["plRadnaMjesta"].tekst() + ": " + String.Format("{0:0.##}", koef * kolonija.efekti[Kolonija.RadnaMjesta]) + "%";
				lblBrFarmera.Text = jezik["plBrFarmera"].tekst() + ": " + String.Format("{0:0.##}", koef * kolonija.efekti[Kolonija.BrFarmera]) + "%";
				lblBrRudara.Text = jezik["plBrRudara"].tekst() + ": " + String.Format("{0:0.##}", koef * kolonija.efekti[Kolonija.BrRudara]) + "%";
				lblBrOdrzavatelja.Text = jezik["plBrOdrz"].tekst() + ": " + String.Format("{0:0.##}", koef * kolonija.efekti[Kolonija.BrOdrzavatelja]) + "%";
			}

			Dictionary<string, double> maxEfekti = kolonija.maxEfekti();
			lblHranaPoStan.Text = jezik["HranaPoStan"].tekst() + String.Format(": {0:0.##} / {1:0.##}", kolonija.efekti[Kolonija.HranaPoFarmeru], maxEfekti[Kolonija.HranaPoFarmeru]);
			lblRudePoStan.Text = jezik["RudePoStan"].tekst() + String.Format(": {0:0.##} / {1:0.##}", kolonija.efekti[Kolonija.RudePoRudaru], maxEfekti[Kolonija.RudePoRudaru]);
			lblOdrzavanjePoStan.Text = jezik["OdrzavanjePoStan"].tekst() + String.Format(": {0:0.##}", (kolonija.efekti[Kolonija.OdrzavanjeUkupno] / kolonija.efekti[Kolonija.Populacija]));
			lblIndustrijaPoStan.Text = jezik["IndustrijaPoStan"].tekst() + String.Format(": {0:0.##} / {1:0.##}", kolonija.efekti[Kolonija.IndustrijaPoRadniku], maxEfekti[Kolonija.IndustrijaPoRadniku]);
			lblRazvojPoStan.Text = jezik["RazvojPoStan"].tekst() + String.Format(": {0:0.##} / {1:0.##}", kolonija.efekti[Kolonija.RazvojPoRadniku], maxEfekti[Kolonija.RazvojPoRadniku]);
		}

		private void btnZatvori_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void osvjeziLabele()
		{
			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.Kolonija];

			lblCivilnaIndustrija.Text = Fje.PrefiksFormater(kolonija.poeniCivilneIndustrije()) + " " + jezik["jedInd"].tekst();
			lblVojnaIndustrija.Text = Fje.PrefiksFormater(kolonija.poeniVojneIndustrije()) + " " + jezik["jedInd"].tekst();
			lblProcjenaCivilneGradnje.Text = kolonija.procjenaVremenaCivilneGradnje();
			lblProcjenaVojneGradnje.Text = kolonija.procjenaVremenaVojneGradnje();
			lblRazvoj.Text = jezik["lblRazvoj"].tekst() + Fje.PrefiksFormater(kolonija.poeniRazvoja());
			lblKoefOrbitalne.Text = Postavke.jezik[Kontekst.FormPlanetInfo, "plCijenaOrbGradnje"].tekst() + ": x" + kolonija.efekti[Kolonija.FaktorCijeneOrbitalnih].ToString("0.##");

			if (kolonija.redCivilneGradnje.Count > 0)
			{
				btnCivilnaGradnja.Image = kolonija.redCivilneGradnje.First.Value.slika;
				btnCivilnaGradnja.Text = "";
			}
			else
			{
				btnCivilnaGradnja.Image = null;
				btnCivilnaGradnja.Text = jezik["Civilna_Gradnja"].tekst();
			}

			if (kolonija.redVojneGradnje.Count > 0)
			{
				btnVojnaGradnja.Image = kolonija.redVojneGradnje.First.Value.slika;
				btnVojnaGradnja.Text = "";
			}
			else
			{
				btnVojnaGradnja.Image = null;
				btnVojnaGradnja.Text = jezik["Vojna_Gradnja"].tekst();
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
				osvjeziLabele();
		}

		private void btnVojnaGradnja_Click(object sender, EventArgs e)
		{
			FormGradnja frmGradnja = new FormGradnja(kolonija, false);

			if (frmGradnja.ShowDialog() == DialogResult.OK)
				osvjeziLabele();
		}

		private void hscrIndustrijaRazvoj_Scroll(object sender, ScrollEventArgs e)
		{
			kolonija.civilnaIndustrija = e.NewValue / (double)hscrCivilnaIndustrija.Maximum;
			int val = (int)Math.Round(kolonija.civilnaIndustrija * hscrCivilnaIndustrija.Maximum, MidpointRounding.AwayFromZero);
			if (hscrCivilnaIndustrija.Value != val)
				e.NewValue = val;
			else
				osvjeziLabele();
		}

		private void hscrVojnaIndustrija_Scroll(object sender, ScrollEventArgs e)
		{
			kolonija.vojnaIndustrija = e.NewValue / (double)hscrVojnaIndustrija.Maximum;
			int val = (int)Math.Round(kolonija.vojnaIndustrija * hscrVojnaIndustrija.Maximum, MidpointRounding.AwayFromZero);
			if (hscrVojnaIndustrija.Value != val)
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
			lblZgradaInfo.Text = zgrada.tip.opis;
		}
	}
}
