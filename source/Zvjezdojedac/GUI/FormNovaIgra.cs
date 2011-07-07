using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Igra;

namespace Zvjezdojedac.GUI
{
	partial class FormNovaIgra : Form
	{
		public List<Igrac.ZaStvoriti> igraci;
		public Mapa.GraditeljMape mapa;
		public PocetnaPopulacija PocetnaPop;

		public FormNovaIgra()
		{
			InitializeComponent();
			igraci = new List<Igrac.ZaStvoriti>();

			postaviJezik();
			this.Font = Postavke.FontSucelja(this.Font);
		}

		private void postaviJezik()
		{
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormNovaIgra];

			this.Text = jezik["NASLOV"].tekst(null);

			btnKreni.Text = jezik["BTN_KRENI"].tekst(null);
			btnOdustani.Text = jezik["BTN_ODUSTANI"].tekst(null);

			lblBrojIgraca.Text = jezik["LBL_BR_IGRACA"].tekst();
			lblImeIgraca.Text = jezik["LBL_IME_IGRACA"].tekst();
			lblOrganizacija.Text = jezik["LBL_ORGANIZACIJA"].tekst();
			lblPocetnaPop.Text = jezik["lblPocetnaPop"].tekst();
			lblVelicinaMape.Text = jezik["LBL_VELICINA_MAPE"].tekst();
		}

		private void frmNovaIgra_Load(object sender, EventArgs e)
		{
			foreach (Mapa.VelicinaMape vm in Mapa.velicinaMape)
				cbVelicinaMape.Items.Add(vm.naziv);
			cbVelicinaMape.SelectedIndex = Postavke.ProslaIgra.VelicinaMape;

			foreach (Organizacija org in Organizacija.lista)
				cbOrganizacija.Items.Add(org.naziv);
			cbOrganizacija.SelectedIndex = Postavke.ProslaIgra.Organizacija;

			for (int i = 2; i <= IgraZvj.maxIgraca; i++)
				cbBrIgraca.Items.Add(i);
			cbBrIgraca.SelectedIndex = Postavke.ProslaIgra.BrIgraca-2;

			foreach(var pocentaPop in PocetnaPopulacija.konfiguracije)
				cbPocetnaPop.Items.Add(new TagTekst<PocetnaPopulacija>(
					pocentaPop,
					Postavke.Jezik[Kontekst.PocetnaPopulacija][pocentaPop.NazivKod].tekst()));
			cbPocetnaPop.SelectedIndex = Postavke.ProslaIgra.PocetnaPop;

			txtIme.Text = Postavke.ProslaIgra.ImeIgraca;
		}

		private void cbVelicinaMape_SelectedIndexChanged(object sender, EventArgs e)
		{
			Mapa.VelicinaMape vm = Mapa.velicinaMape[cbVelicinaMape.SelectedIndex];
			
			Dictionary<string, double> varijable = new Dictionary<string, double>();
			varijable.Add("BR", vm.velicina * vm.velicina);
			lblOpisMape.Text = Postavke.Jezik[Kontekst.FormNovaIgra, "LBL_OPIS_MAPE"].tekst(varijable);
		}

		private void cbOrganizacija_SelectedIndexChanged(object sender, EventArgs e)
		{
			Organizacija org = Organizacija.lista[cbOrganizacija.SelectedIndex];
			lblOpisOrg.Text = org.opis;
		}

		private void btnOdustani_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btnKreni_Click(object sender, EventArgs e)
		{
			int brIgraca = cbBrIgraca.SelectedIndex + 2;
			Postavke.ProslaIgra.BrIgraca = brIgraca;
			Postavke.ProslaIgra.ImeIgraca = txtIme.Text;
			Postavke.ProslaIgra.Organizacija = cbOrganizacija.SelectedIndex;
			Postavke.ProslaIgra.PocetnaPop = cbPocetnaPop.SelectedIndex;
			Postavke.ProslaIgra.VelicinaMape = cbVelicinaMape.SelectedIndex;

			Vadjenje<Organizacija> organizacije = new Vadjenje<Organizacija>();
			for (int i = 0; i < Organizacija.lista.Count; i++)
				if (i != cbOrganizacija.SelectedIndex)
					organizacije.dodaj(Organizacija.lista[i]);

			Vadjenje<Color> boje = new Vadjenje<Color>();
			foreach (Color boja in Igrac.BojeIgraca)
				boje.dodaj(boja);
			
			igraci.Add(new Igrac.ZaStvoriti(Igrac.Tip.COVJEK, txtIme.Text, Organizacija.lista[cbOrganizacija.SelectedIndex], boje.izvadi()));
			for (int i = 1; i < brIgraca; i++)
				igraci.Add(new Igrac.ZaStvoriti(Igrac.Tip.RACUNALO, "Komp " + i, organizacije.izvadi(), boje.izvadi()));

			mapa = new Mapa.GraditeljMape(
				cbVelicinaMape.SelectedIndex,
				brIgraca);

			PocetnaPop = (cbPocetnaPop.SelectedItem as TagTekst<PocetnaPopulacija>).tag;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cbPocetnaPop_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbPocetnaPop.SelectedItem == null) return;

			PocetnaPopulacija pocetnaPop = (cbPocetnaPop.SelectedItem as TagTekst<PocetnaPopulacija>).tag;
			var jezik = Postavke.Jezik[Kontekst.FormNovaIgra];

			lblBrKolonija.Text = jezik["lblBrKolonija"].tekst() + ": " + pocetnaPop.BrKolonija;
			lblPopulacija.Text = jezik["lblPopulacija"].tekst() + ": " + Fje.PrefiksFormater(pocetnaPop.Populacija);
		}
	}
}
