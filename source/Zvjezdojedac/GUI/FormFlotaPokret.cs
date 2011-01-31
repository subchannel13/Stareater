using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Prototip.Podaci;
using Prototip.Podaci.Jezici;
using Alati;

namespace Prototip
{
	public partial class FormFlotaPokret : Form
	{
		private const double Epsilon = 1e-3;

		private IgraZvj igra;
		private Igrac igrac;
		
		private Flota izvornaFlota;
		
		private Dictionary<Dizajn, long> poslaniBrodovi = new Dictionary<Dizajn,long>();
		private Zvijezda polaznaZvijezda = null;

		public FormFlotaPokret(IgraZvj igra)
		{
			InitializeComponent();

			this.igra = igra;
		}

		private void initPoslaniBrodovi()
		{
			poslaniBrodovi.Clear();
			foreach (Dictionary<Dizajn, Brod> dizajnovi in izvornaFlota.brodovi.Values)
				foreach (Dizajn dizajn in dizajnovi.Keys)
					poslaniBrodovi.Add(dizajn, 0);
		}

		public void pomicanjeBroda(Flota izvornaFlota, Brod brod, Igrac igrac)
		{
			this.izvornaFlota = izvornaFlota;
			this.igrac = igrac;
			initPoslaniBrodovi();
			poslaniBrodovi[brod.dizajn] = brod.kolicina;
			postaviGUI();
			Show();
		}

		public void pomicanjeFlote(Flota izvornaFlota, Igrac igrac)
		{
			this.izvornaFlota = izvornaFlota;
			this.igrac = igrac;
			initPoslaniBrodovi();
			postaviGUI();
			Show();
		}

		private void postaviGUI()
		{
			this.polaznaZvijezda = igra.mapa.najblizaZvijezda(izvornaFlota.x, izvornaFlota.y, Epsilon);
			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormFlotaPokret];
			Dictionary<string, double> varijable = new Dictionary<string, double>();

			lblPolaznaZvijezda.Text = polaznaZvijezda.ime;
			varijable.Add("ID", igrac.noviIdFlote());
			lblNazivFlote.Text = jezik["lblNazivFlote"].tekst(varijable);

			cbOdrediste.Items.Clear();
			foreach (Zvijezda zvj in igra.mapa.zvijezde)
				cbOdrediste.Items.Add(new TagTekst<Zvijezda>(zvj, zvj.ime));
			cbOdrediste.Sorted = true;
			cbOdrediste.SelectedIndex = 0;

			List<Dizajn> sortiraniDizajnovi = new List<Dizajn>(poslaniBrodovi.Keys);
			sortiraniDizajnovi.Sort(dizajnSort);
			foreach(Dizajn dizajn in sortiraniDizajnovi)
				lstBrodovi.Items.Add(new TagTekst<Dizajn>(dizajn, stavkaListe(dizajn)));

			lblKolicina.Text = jezik["lblKolicina"].tekst();
			
			lblPridruzi.Hide();
			cbPridruzi.Hide();
		}

		private int dizajnSort(Dizajn a, Dizajn b)
		{
			return a.ime.CompareTo(b.ime);
		}

		private string stavkaListe(Dizajn dizajn)
		{
			return Fje.PrefiksFormater(poslaniBrodovi[dizajn]) + " x " + dizajn.ime;
		}

		private void procjenaBrzine()
		{
			List<Brod> brodovi = new List<Brod>();
			foreach (KeyValuePair<Dizajn, long> brod in poslaniBrodovi)
				brodovi.Add(new Brod(brod.Key, brod.Value));

			double brzina = igrac.procjenaBrzineFlote(brodovi);
			Zvijezda odredisnaZvj = ((TagTekst<Zvijezda>)cbOdrediste.SelectedItem).tag;
			if (brzina > 0 && polaznaZvijezda != null)
				if (polaznaZvijezda.crvotocine.Contains(odredisnaZvj))
					brzina += igrac.efekti["BRZINA_CRVOTOCINA"];

			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormFlotaPokret];
			Dictionary<string, double> varijable = new Dictionary<string, double>();

			if (brzina == 0)
				lblBrPoteza.Text = jezik["lblBrPotezaNikad"].tekst();
			else {
				varijable.Add("BR_POTEZA", Math.Ceiling(odredisnaZvj.udaljenost(izvornaFlota.x, izvornaFlota.y) / brzina));
				lblBrPoteza.Text = jezik["lblBrPoteza"].tekst(varijable);
			}
		}

		private void cbOdrediste_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbOdrediste.SelectedItem != null)
				procjenaBrzine();
		}
	}
}
