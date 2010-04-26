using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Prototip;

namespace Zvjezdojedac_editori
{
	public partial class FormTehnologije : ValidatorForm
	{
		const int OdgodaPromjene = 300;
		const string imeTag = "IME";
		const string opisTag = "OPIS";
		const string kodTag = "KOD";
		const string cijenaTag = "CIJENA";
		const string maxNivoTag = "MAX_LVL";
		const string preduvjetiTag = "PREDUVJETI";
		const string slikaTag = "SLIKA";

		private int selektiranaTehnologija = -1;
		List<Dictionary<string, string>> tehnologijeIst = null;
		List<Dictionary<string, string>> tehnologijeRaz = null;
		List<Dictionary<string, string>> popis = null;
		private HashSet<string> kodovi = new HashSet<string>();
		private List<Tehnologija.Preduvjet> preduvjeti = null;
		private string stariKod = null;

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

			addValidation(new Validation(txtCijena, InputType.Forumla, lblCijenaGreska));
			addValidation(new Validation(txtMaxNivo, InputType.IntegerNum, lblMaxNivoGreska));
		}

		private void postaviPopis()
		{
			lstvTehnologije.Items.Clear();
			foreach (Dictionary<string, string> info in popis)
			{
				ListViewItem item = new ListViewItem(info[imeTag]);
				item.Tag = info;
				lstvTehnologije.Items.Add(item);

				kodovi.Add(info[kodTag]);
			}
		}

		private void postaviPreduvjete(string preduvjetiString)
		{
			List<Tehnologija.Preduvjet> preduvjeti = Tehnologija.Preduvjet.NaciniPreduvjete(preduvjetiString, false);
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
			info[cijenaTag] = txtCijena.Text;
			info[maxNivoTag] = txtMaxNivo.Text;
			info[preduvjetiTag] = Tehnologija.Preduvjet.UString(preduvjeti, true);
			info[slikaTag] = txtSlika.Text;
		}

		protected override void addoditionalChangeHandle()
		{
			lblSlikaGreska.Visible = (picSlika.Image == null);
			lblKodGreska.Visible = (kodovi.Contains(txtKod.Text.Trim()) || txtKod.Text.Trim().Length == 0);
		}

		protected override bool valid()
		{
			if (!base.valid()) return false;
			
			if (picSlika.Image == null) return false;
			if (kodovi.Contains(txtKod.Text.Trim()) || txtKod.Text.Trim().Length == 0) return false;

			return true;
		}

		private void lstvTehnologije_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstvTehnologije.SelectedIndices.Count == 0)
				return;

			if (selektiranaTehnologija >= 0 && selektiranaTehnologija != lstvTehnologije.SelectedIndices[0])
				if (!valid())
				{
					spremiTehnologiju(selektiranaTehnologija);
					kodovi.Add(txtKod.Text.Trim());
				}
				else if (stariKod != null)
					kodovi.Add(stariKod);

			selektiranaTehnologija = lstvTehnologije.SelectedIndices[0];
			Dictionary<string, string> info = (Dictionary<string, string>)lstvTehnologije.SelectedItems[0].Tag;
			stariKod = info[kodTag];
			kodovi.Remove(info[kodTag]);

			txtNaziv.Text = info[imeTag];
			txtKod.Text = info[kodTag];
			txtCijena.Text = info[cijenaTag];
			txtMaxNivo.Text = info[maxNivoTag];
			txtSlika.Text = info[slikaTag];
			picSlika.Image = Image.FromFile(info[slikaTag]);
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
					
					Image slika = Image.FromFile(putanja);
					if (slika.Width != 80 || slika.Height != 80)
						throw new ArgumentException();

					info[slikaTag] = putanja;
					picSlika.Image = slika;
				}
				catch (Exception) { }
			}
		}

		private void txtKod_TextChanged(object sender, EventArgs e)
		{
			postChange();
		}

		private void btnPreduvjeti_Click(object sender, EventArgs e)
		{
			if (lstvTehnologije.SelectedIndices.Count == 0)
				return;

			/*FormPreduvjeti forma = new FormPreduvjeti(preduvjeti);
			if (forma.ShowDialog() == DialogResult.OK)
				postaviPreduvjete(forma.preduvjeti);
			 */
		}

	}
}
