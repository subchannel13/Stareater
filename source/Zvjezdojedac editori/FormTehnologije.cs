using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Prototip;
using Zvjezdojedac_editori.Validation;

namespace Zvjezdojedac_editori
{
	public partial class FormTehnologije : ValidatorForm
	{
		const int OdgodaPromjene = 300;
		public const string imeTag = "IME";
		public const string opisTag = "OPIS";
		public const string kodTag = "KOD";
		public const string cijenaTag = "CIJENA";
		public const string maxNivoTag = "MAX_LVL";
		public const string preduvjetiTag = "PREDUVJETI";
		public const string slikaTag = "SLIKA";
		public const string novaTag = "NOVA";

		private int selektiranaTehnologija = -1;
		List<Dictionary<string, string>> tehnologijeIst = null;
		List<Dictionary<string, string>> tehnologijeRaz = null;
		List<Dictionary<string, string>> popis = null;
		private HashSet<string> kodovi = new HashSet<string>();
		private List<Tehnologija.Preduvjet> preduvjeti = null;
		private string stariKod = null;
		private ValidTextBoxImage slikaValidator = null;

		public FormTehnologije(List<Dictionary<string, string>> tehnologijeIst, List<Dictionary<string, string>> tehnologijeRaz)
		{
			InitializeComponent();
			this.tehnologijeIst = tehnologijeIst;
			this.tehnologijeRaz = tehnologijeRaz;
			if (radIstrazivanje.Checked)
				this.popis = tehnologijeIst;
			else
				this.popis = tehnologijeRaz;
			postaviPopis();

			addValidation(new ValidTextBoxFormula(txtCijena, lblCijenaGreska));
			addValidation(new ValidTextBoxInteger(txtMaxNivo, lblMaxNivoGreska));
			addValidation(new ValidTextBoxSetUniqeness(txtKod, lblKodGreska, kodovi));
			this.slikaValidator = new ValidTextBoxImage(txtSlika, lblSlikaGreska, new Size(80, 80));
			addValidation(slikaValidator);

			foreach (Dictionary<string, string> info in tehnologijeIst)
				kodovi.Add(info[kodTag]);
			foreach (Dictionary<string, string> info in tehnologijeRaz)
				kodovi.Add(info[kodTag]);
		}

		private void postaviPopis()
		{
			lstvTehnologije.Items.Clear();
			foreach (Dictionary<string, string> info in popis)
			{
				ListViewItem item = new ListViewItem(info[imeTag]);
				item.Tag = info;
				lstvTehnologije.Items.Add(item);
			}
		}

		private void postaviPreduvjete(string preduvjetiString)
		{
			postaviPreduvjete(Tehnologija.Preduvjet.NaciniPreduvjete(preduvjetiString, false));
		}

		private void postaviPreduvjete(List<Tehnologija.Preduvjet> preduvjeti)
		{
			lstvPreduvjeti.Items.Clear();
			foreach (Tehnologija.Preduvjet p in preduvjeti)
			{
				ListViewItem item = new ListViewItem(p.kod);
				item.SubItems.Add(p.nivo.ToString());
				lstvPreduvjeti.Items.Add(item);
			}

			this.preduvjeti = new List<Tehnologija.Preduvjet>(preduvjeti);
		}

		private void spremiTehnologiju(int indeks)
		{
			if (indeks < 0 || indeks >= popis.Count || !valid())
				return;

			Dictionary<string, string> info = (Dictionary<string, string>)lstvTehnologije.Items[indeks].Tag;
			info[imeTag] = txtNaziv.Text.Trim();
			info[kodTag] = txtKod.Text.Trim();
			info[opisTag] = txtOpis.Text.Trim();
			info[cijenaTag] = txtCijena.Text;
			info[maxNivoTag] = txtMaxNivo.Text;
			info[preduvjetiTag] = Tehnologija.Preduvjet.UString(preduvjeti, false);
			info[slikaTag] = txtSlika.Text;
		}

		protected override void changeOccured(HashSet<Control> changedControles)
		{
			if (changedControles.Contains(txtSlika))
				if (slikaValidator.valid())
					picSlika.Image = Image.FromFile(txtSlika.Text);
		}

		private void lstvTehnologije_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstvTehnologije.SelectedIndices.Count == 0)
				return;

			if (selektiranaTehnologija >= 0 && selektiranaTehnologija != lstvTehnologije.SelectedIndices[0])
				if (valid())
				{
					spremiTehnologiju(selektiranaTehnologija);
					kodovi.Add(txtKod.Text.Trim());
					lstvTehnologije.Items[selektiranaTehnologija].Text = txtNaziv.Text;
				}
				else if (stariKod != null)
					kodovi.Add(stariKod);

			selektiranaTehnologija = lstvTehnologije.SelectedIndices[0];
			Dictionary<string, string> info = (Dictionary<string, string>)lstvTehnologije.SelectedItems[0].Tag;
			stariKod = info[kodTag];
			kodovi.Remove(info[kodTag]);

			Image img = null;
			try
			{
				img = Image.FromFile(info[slikaTag]);
			} catch
			{}

			txtNaziv.Text = info[imeTag];
			txtKod.Text = info[kodTag];
			txtOpis.Text = info[opisTag];
			txtCijena.Text = info[cijenaTag];
			txtMaxNivo.Text = info[maxNivoTag];
			txtSlika.Text = info[slikaTag];
			picSlika.Image = img;
			postaviPreduvjete(info[preduvjetiTag]);
		}

		private void btnSlika_Click(object sender, EventArgs e)
		{
			if (lstvTehnologije.SelectedIndices.Count == 0)
				return;

			Dictionary<string, string> info = (Dictionary<string, string>)lstvTehnologije.SelectedItems[0].Tag;
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.InitialDirectory = Environment.CurrentDirectory;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					string putanja = ofd.FileName;
					if (putanja.StartsWith(Environment.CurrentDirectory))
						putanja = "." + putanja.Remove(0, Environment.CurrentDirectory.Length).Replace('\\', '/');
					
					txtSlika.Text = putanja;
				}
				catch (Exception) { }
			}
		}

		private void btnPreduvjeti_Click(object sender, EventArgs e)
		{
			if (lstvTehnologije.SelectedIndices.Count == 0)
				return;

			FormPreduvjeti forma = new FormPreduvjeti(preduvjeti, tehnologijeIst, tehnologijeRaz);
			if (forma.ShowDialog() == DialogResult.OK)
				postaviPreduvjete(forma.preduvjeti);
		}

		private void btnGore_Click(object sender, EventArgs e)
		{
			if (lstvTehnologije.SelectedItems.Count == 0)
				return;

			int indeks = lstvTehnologije.SelectedIndices[0];
			if (indeks == 0) 
				return;

			popis.Reverse(indeks - 1, 2);

			ListViewItem item = lstvTehnologije.Items[indeks];
			lstvTehnologije.Items.Remove(item);
			lstvTehnologije.Items.Insert(indeks - 1, item);
		}

		private void btnDolje_Click(object sender, EventArgs e)
		{
			if (lstvTehnologije.SelectedItems.Count == 0)
				return;

			int indeks = lstvTehnologije.SelectedIndices[0];
			if (indeks + 1 >= lstvTehnologije.Items.Count)
				return;

			popis.Reverse(indeks, 2);

			ListViewItem item = lstvTehnologije.Items[indeks];
			lstvTehnologije.Items.Remove(item);
			lstvTehnologije.Items.Insert(indeks + 1, item);
		}

		private void btnNovaTeh_Click(object sender, EventArgs e)
		{
			string kod = "";
			Random rand = new Random();
			while(kodovi.Contains(kod) || kod.Length < 3)
				kod = kod + (char)('A' + rand.Next('Z' - 'A'));
			kodovi.Add(kod);

			Dictionary<string, string> teh = new Dictionary<string, string>();
			teh[imeTag] = "Nova tehnologija";
			teh[opisTag] = "Bez opisa";
			teh[kodTag] = kod;
			teh[cijenaTag] = "0";
			teh[maxNivoTag] = "1";
			teh[preduvjetiTag] = "";
			teh[slikaTag] = "";
			teh[novaTag] = "";
			popis.Add(teh);

			ListViewItem item = new ListViewItem(teh[imeTag]);
			item.Tag = teh;
			lstvTehnologije.Items.Add(item);
			item.Selected = true;
		}

		private void radRazvoj_CheckedChanged(object sender, EventArgs e)
		{
			if (!radRazvoj.Checked) return;

			if (valid())
			{
				spremiTehnologiju(selektiranaTehnologija);
				kodovi.Add(txtKod.Text.Trim());
			}
			popis = tehnologijeRaz;
			postaviPopis();
		}

		private void radIstrazivanje_CheckedChanged(object sender, EventArgs e)
		{
			if (!radIstrazivanje.Checked) return;

			if (valid())
			{
				spremiTehnologiju(selektiranaTehnologija);
				kodovi.Add(txtKod.Text.Trim());
			}
			popis = tehnologijeIst;
			postaviPopis();
		}

		private void btnUkloni_Click(object sender, EventArgs e)
		{
			if (lstvTehnologije.SelectedItems.Count == 0)
				return;

			int indeks = lstvTehnologije.SelectedIndices[0];
			lstvTehnologije.Items.RemoveAt(indeks);
			popis.RemoveAt(indeks);

			if (lstvTehnologije.Items.Count > indeks)
				lstvTehnologije.Items[indeks].Selected = true;
		}

		private void zapisi(StreamWriter pisac, Dictionary<string, string> podaci)
		{
			if (podaci.ContainsKey(novaTag))
				if (podaci[slikaTag].Trim().Length == 0)
					return;

			pisac.WriteLine("<TEHNOLOGIJA>");
			pisac.WriteLine("IME = " + podaci[imeTag]);
			pisac.WriteLine("OPIS = " + podaci[opisTag]);
			pisac.WriteLine("KOD = " + podaci[kodTag]);
			pisac.WriteLine("CIJENA = " + podaci[cijenaTag]);
			pisac.WriteLine("MAX_LVL = " + podaci[maxNivoTag]);
			pisac.WriteLine("PREDUVJETI = " + podaci[preduvjetiTag]);
			pisac.WriteLine("SLIKA = " + podaci[slikaTag]);
			pisac.WriteLine("----");
			pisac.WriteLine();
		}

		private void FormTehnologije_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (valid())
				spremiTehnologiju(selektiranaTehnologija);

			StreamWriter pisac = new StreamWriter("./podaci/teh_razvoj.txt");
			foreach (Dictionary<string, string> teh in tehnologijeRaz)
				zapisi(pisac, teh);
			pisac.Close();

			pisac = new StreamWriter("./podaci/teh_istrazivanje.txt");
			foreach (Dictionary<string, string> teh in tehnologijeIst)
				zapisi(pisac, teh);
			pisac.Close();
		}

	}
}
