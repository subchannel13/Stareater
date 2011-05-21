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
	public partial class FormNovaIgra : Form
	{
		public List<Igrac.ZaStvoriti> igraci;

		public Mapa.GraditeljMape mapa;

		public FormNovaIgra()
		{
			InitializeComponent();
			igraci = new List<Igrac.ZaStvoriti>();

			postaviJezik();
		}

		private void postaviJezik()
		{
			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormNovaIgra];

			this.Text = jezik["NASLOV"].tekst(null);

			btnKreni.Text = jezik["BTN_KRENI"].tekst(null);
			btnOdustani.Text = jezik["BTN_ODUSTANI"].tekst(null);

			lblBrojIgraca.Text = jezik["LBL_BR_IGRACA"].tekst(null);
			lblImeIgraca.Text = jezik["LBL_IME_IGRACA"].tekst(null);
			lblOrganizacija.Text = jezik["LBL_ORGANIZACIJA"].tekst(null);
			lblVelicinaMape.Text = jezik["LBL_VELICINA_MAPE"].tekst(null);
		}

		private void frmNovaIgra_Load(object sender, EventArgs e)
		{
			foreach (Mapa.VelicinaMape vm in Mapa.velicinaMape)
				cbVelicinaMape.Items.Add(vm.naziv);
			cbVelicinaMape.SelectedIndex = Postavke.ProslaIgra.velicinaMape;

			foreach (Organizacija org in Organizacija.lista)
				cbOrganizacija.Items.Add(org.naziv);
			cbOrganizacija.SelectedIndex = Postavke.ProslaIgra.organizacija;

			for (int i = 2; i <= IgraZvj.maxIgraca; i++)
				cbBrIgraca.Items.Add(i);
			cbBrIgraca.SelectedIndex = Postavke.ProslaIgra.brIgraca-2;

			txtIme.Text = Postavke.ProslaIgra.imeIgraca;
		}

		private void cbVelicinaMape_SelectedIndexChanged(object sender, EventArgs e)
		{
			Mapa.VelicinaMape vm = Mapa.velicinaMape[cbVelicinaMape.SelectedIndex];
			
			Dictionary<string, double> varijable = new Dictionary<string, double>();
			varijable.Add("BR", vm.velicina * vm.velicina);
			lblOpisMape.Text = Postavke.jezik[Kontekst.FormNovaIgra, "LBL_OPIS_MAPE"].tekst(varijable);
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
			Postavke.ProslaIgra.brIgraca = brIgraca;
			Postavke.ProslaIgra.imeIgraca = txtIme.Text;
			Postavke.ProslaIgra.organizacija = cbOrganizacija.SelectedIndex;
			Postavke.ProslaIgra.velicinaMape = cbVelicinaMape.SelectedIndex;

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

			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
