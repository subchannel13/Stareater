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
using Zvjezdojedac.Igra.Brodovi;

namespace Zvjezdojedac.GUI
{
	public partial class FormFlotaPokret : Form
	{
		private const double Epsilon = 1e-3;
		private const int hscrKolicinaMax = 20;
		Dictionary<string, ITekst> jezik;

		private IgraZvj igra;
		private Igrac igrac;
		private FormIgra frmIgra;
		
		private Flota izvornaFlota;
		
		private Dictionary<Dizajn, long> poslaniBrodovi = new Dictionary<Dizajn,long>();
		private Zvijezda polaznaZvijezda = null;

		public FormFlotaPokret(FormIgra frmIgra)
		{
			InitializeComponent();

			this.frmIgra = frmIgra;
			this.Font = Postavke.FontSucelja(this.Font);
		}

		private void initPoslaniBrodovi()
		{
			poslaniBrodovi.Clear();
			foreach (Dizajn dizajn in izvornaFlota.brodovi.Keys)
				poslaniBrodovi.Add(dizajn, 0);
		}

		public void pomicanjeBroda(Flota izvornaFlota, Brod brod, Igrac igrac, IgraZvj igra)
		{
			this.izvornaFlota = izvornaFlota;
			this.igrac = igrac;
			this.igra = igra;
			initPoslaniBrodovi();
			poslaniBrodovi[brod.dizajn] = brod.kolicina;
			postaviGUI();
			Show();
		}

		public void pomicanjeFlote(Flota izvornaFlota, Igrac igrac, IgraZvj igra)
		{
			this.izvornaFlota = izvornaFlota;
			this.igrac = igrac;
			this.igra = igra;
			initPoslaniBrodovi();
			foreach (Brod brod in izvornaFlota.brodovi.Values)
				poslaniBrodovi[brod.dizajn] = brod.kolicina;
			postaviGUI();
			Show();
		}

		private void postaviGUI()
		{
			this.polaznaZvijezda = igra.mapa.najblizaZvijezda(izvornaFlota.x, izvornaFlota.y, Epsilon);
			this.jezik = Postavke.Jezik[Kontekst.FormFlotaPokret];
			Dictionary<string, double> varijable = new Dictionary<string, double>();

			this.Text = jezik["naslov"].tekst();

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

			btnOdustani.Text = jezik["btnOdustani"].tekst();
			btnPosalji.Text = jezik["btnPosalji"].tekst();
			lblOdrediste.Text = jezik["lblOdrediste"].tekst();
			lblKolicina.Text = jezik["lblKolicina"].tekst();
			lblPridruzi.Text = jezik["lblPridruzi"].tekst();

			lblKolicina.Hide();
			hscbKolicina.Hide();
			txtKolicina.Hide();
		}

		private int dizajnSort(Dizajn a, Dizajn b)
		{
			return a.ime.CompareTo(b.ime);
		}

		private string stavkaListe(Dizajn dizajn)
		{
			return Fje.PrefiksFormater(poslaniBrodovi[dizajn]) + " x " + dizajn.ime;
		}

		private List<Brod> listaBrodova()
		{
			List<Brod> rez = new List<Brod>();
			foreach (KeyValuePair<Dizajn, long> brod in poslaniBrodovi)
				rez.Add(new Brod(brod.Key, brod.Value));

			return rez;
		}

		private void procjenaBrzine()
		{
			Zvijezda odredisnaZvj = ((TagTekst<Zvijezda>)cbOdrediste.SelectedItem).tag;
			if (odredisnaZvj.id == polaznaZvijezda.id) {
				lblBrPoteza.Hide();
				return;
			}
			else if (!lblBrPoteza.Visible)
				lblBrPoteza.Show();

			double brzina = igrac.procjenaBrzineFlote(listaBrodova());
			if (brzina > 0 && polaznaZvijezda != null)
				if (polaznaZvijezda.crvotocine.Contains(odredisnaZvj))
					brzina += igrac.efekti["BRZINA_CRVOTOCINA"];

			Dictionary<string, double> varijable = new Dictionary<string, double>();

			if (brzina == 0) {
				lblBrPoteza.Text = jezik["lblBrPotezaNikad"].tekst();
				btnPosalji.Enabled = false;
			}
			else {
				varijable.Add("BR_POTEZA", Math.Ceiling(odredisnaZvj.udaljenost(izvornaFlota.x, izvornaFlota.y) / brzina));
				lblBrPoteza.Text = jezik["lblBrPoteza"].tekst(varijable);
				btnPosalji.Enabled = true;
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

			if (max < hscrKolicinaMax)
				hscbKolicina.Maximum = (int)max;
			else
				hscbKolicina.Maximum = hscrKolicinaMax;
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

		private void pridruzivanjaNema()
		{
			cbPridruzi.Items.Clear();
			cbPridruzi.Items.Add(new TagTekst<Flota>(null, jezik["nePridruzuj"].tekst()));
			cbPridruzi.SelectedIndex = 0;
		}

		private void pridruziPokretnojFloti(List<Flota> slicneFlote)
		{
			cbPridruzi.Items.Clear();
			cbPridruzi.Items.Add(new TagTekst<Flota>(null, jezik["nePridruzuj"].tekst()));

			Dictionary<string, double> varijable = new Dictionary<string, double>();
			foreach (Flota flota in slicneFlote) {
				varijable.Add("ID", flota.id);
				cbPridruzi.Items.Add(new TagTekst<Flota>(null, jezik["lblNazivFlote"].tekst(varijable)));
			}
			cbPridruzi.SelectedIndex = 0;
		}

		private void pridruziStacFloti()
		{
			cbPridruzi.Items.Clear();
			cbPridruzi.Items.Add(new TagTekst<Flota>(
				igrac.floteStacionarne[polaznaZvijezda],
				Postavke.Jezik[Kontekst.FormIgra]["flotaObrana"].tekst()));
			cbPridruzi.SelectedIndex = 0;
		}

		private void cbOdrediste_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbOdrediste.SelectedItem == null) return;
			
			igrac.odredisnaZvijezda = ((TagTekst<Zvijezda>)cbOdrediste.SelectedItem).tag;
			frmIgra.osvjeziMapu();
			procjenaBrzine();

			List<Flota> slicneFlote = igrac.slicneFlote(polaznaZvijezda, igrac.odredisnaZvijezda);
			if (polaznaZvijezda.id == igrac.odredisnaZvijezda.id)
				if (slicneFlote.Count == 0)
					pridruzivanjaNema();
				else
					pridruziStacFloti();
			else
				pridruziPokretnojFloti(slicneFlote);
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
			long trenutno;
			if (izvornaKolicina <= hscrKolicinaMax)
				trenutno = (long)Math.Ceiling((e.NewValue / (double)hscbKolicina.Maximum) * izvornaKolicina);
			else
				trenutno = (long)Math.Ceiling(Math.Pow(e.NewValue / (double)hscbKolicina.Maximum, 2) * izvornaKolicina);
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

		private void btnPosalji_Click(object sender, EventArgs e)
		{
			igrac.dodajPokretnuFlotu(
				poslaniBrodovi,
				izvornaFlota,
				polaznaZvijezda,
				((TagTekst<Zvijezda>)cbOdrediste.SelectedItem).tag,
				((TagTekst<Flota>)cbPridruzi.SelectedItem).tag);
			frmIgra.prikaziFlotu(polaznaZvijezda);
			DialogResult = DialogResult.OK;
			Close();
		}

		private void bnOdustani_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
