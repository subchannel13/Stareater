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
		private FormIgra frmIgra;
		
		private Flota izvornaFlota;
		
		private Dictionary<Dizajn, long> poslaniBrodovi = new Dictionary<Dizajn,long>();
		private Zvijezda polaznaZvijezda = null;

		public FormFlotaPokret(IgraZvj igra, FormIgra frmIgra)
		{
			InitializeComponent();

			this.frmIgra = frmIgra;
			this.igra = igra;
		}

		private void initPoslaniBrodovi()
		{
			poslaniBrodovi.Clear();
			foreach (Dizajn dizajn in izvornaFlota.brodovi.Keys)
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

			lblKolicina.Hide();
			hscbKolicina.Hide();
			txtKolicina.Hide();

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

		private void postaviKolicinu(long trenutno)
		{
			TagTekst<Dizajn> tagDizajn = (TagTekst<Dizajn>)lstBrodovi.SelectedItem;
			long max = izvornaFlota[tagDizajn.tag].kolicina;
			if (trenutno > max) trenutno = max;
			if (trenutno < 0) trenutno = 0;

			if (trenutno.ToString().CompareTo(txtKolicina.Text) != 0)
				txtKolicina.Text = trenutno.ToString();
			
			int novaHScBPoz = (int)Math.Ceiling(Math.Sqrt(trenutno / (double) max) * hscbKolicina.Maximum);
			if (novaHScBPoz > hscbKolicina.Maximum) novaHScBPoz = hscbKolicina.Maximum;
			if (hscbKolicina.Value != novaHScBPoz)
				hscbKolicina.Value = novaHScBPoz;

			if (poslaniBrodovi[tagDizajn.tag] != trenutno) {
				poslaniBrodovi[tagDizajn.tag] = trenutno;
				tagDizajn.tekst = stavkaListe(tagDizajn.tag);
				lstBrodovi.Items[lstBrodovi.SelectedIndex] = tagDizajn;
				procjenaBrzine();
			}
		}

		private Dizajn trenutniDizajn()
		{
			return ((TagTekst<Dizajn>)lstBrodovi.SelectedItem).tag;
		}

		public void postaviOdrediste(Zvijezda zvj)
		{
			foreach(object o in cbOdrediste.Items)
				if (((TagTekst<Zvijezda>)o).tag.id == zvj.id) {
					cbOdrediste.SelectedItem = o;
					return;
				}
		}

		private void cbOdrediste_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbOdrediste.SelectedItem == null) return;
			
			igrac.odredisnaZvijezda = ((TagTekst<Zvijezda>)cbOdrediste.SelectedItem).tag;
			frmIgra.osvjeziMapu();
			procjenaBrzine();
		}

		private void lstBrodovi_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstBrodovi.SelectedItem == null) {
				lblKolicina.Hide();
				hscbKolicina.Hide();
				txtKolicina.Hide();
				return;
			}

			lblKolicina.Show();
			hscbKolicina.Show();
			txtKolicina.Show();

			postaviKolicinu(poslaniBrodovi[trenutniDizajn()]);
		}

		private void hscbKolicina_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.NewValue == e.OldValue) return;
			
			long izvornaKolicina = izvornaFlota[trenutniDizajn()].kolicina;
			long trenutno = (long)Math.Ceiling(Math.Pow(e.NewValue / (double)hscbKolicina.Maximum, 2) * izvornaKolicina);
			postaviKolicinu(trenutno);
		}

		private void txtKolicina_TextChanged(object sender, EventArgs e)
		{
			try {
				long trenutno = long.Parse(txtKolicina.Text);
				long izvornaKolicina = izvornaFlota[trenutniDizajn()].kolicina;
				if (trenutno <= izvornaKolicina)
					postaviKolicinu(trenutno);
			}
			catch (FormatException) {
				return;
			}
		}

		private void FormFlotaPokret_FormClosing(object sender, FormClosingEventArgs e)
		{
			igrac.odredisnaZvijezda = null;
			frmIgra.osvjeziMapu();
			frmIgra = null;
		}
	}
}
