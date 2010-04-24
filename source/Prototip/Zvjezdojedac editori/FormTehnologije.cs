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

		private int selektiranaTehnologija = -1;
		private HashSet<string> kodovi = new HashSet<string>();
		private List<Tehnologija.Preduvjet> preduvjeti = null;

		public FormTehnologije()
		{
			InitializeComponent();
			postaviPopis();

			addValidation(new Validation(txtCijena, InputType.Forumla, lblCijenaGreska));
			addValidation(new Validation(txtMaxNivo, InputType.IntegerNum, lblMaxNivoGreska));
		}

		private void postaviPopis()
		{
			List<Tehnologija.TechInfo> popis = null;
			if (radIstrazivanje.Checked) popis = Tehnologija.TechInfo.tehnologijeIstrazivanje;
			else popis = Tehnologija.TechInfo.tehnologijeRazvoj;
			
			lstvTehnologije.Items.Clear();
			foreach(Tehnologija.TechInfo info in popis)
			{
				ListViewItem item = new ListViewItem(info.ime);
				item.Tag = info;
				lstvTehnologije.Items.Add(item);

				kodovi.Add(info.kod);
			}
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
			Tehnologija.TechInfo info = (Tehnologija.TechInfo)lstvTehnologije.Items[indeks].Tag;
			info.ime = txtNaziv.Text.Trim();
			info.kod = txtKod.Text.Trim();
			info.cijena = Formula.NaciniFormulu(txtCijena.Text);
			info.maxNivo = long.Parse(txtMaxNivo.Text);
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

			selektiranaTehnologija = lstvTehnologije.SelectedIndices[0];
			Tehnologija.TechInfo info = (Tehnologija.TechInfo)lstvTehnologije.SelectedItems[0].Tag;
			kodovi.Remove(info.kod);

			txtNaziv.Text = info.ime;
			txtKod.Text = info.kod;
			txtCijena.Text = info.cijena.ToString();
			txtMaxNivo.Text = info.maxNivo.ToString();
			picSlika.Image = info.slika;
			postaviPreduvjete(info.preduvjeti);
		}

		private void btnSlika_Click(object sender, EventArgs e)
		{
			if (lstvTehnologije.SelectedIndices.Count == 0)
				return;

			Tehnologija.TechInfo info = (Tehnologija.TechInfo)lstvTehnologije.SelectedItems[0].Tag;
			OpenFileDialog ofd = new OpenFileDialog();

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					Image slika = Image.FromFile(info.slikaPutanja);
					if (slika.Width != 80 || slika.Height != 80)
						throw new ArgumentException();
					info.slikaPutanja = ofd.FileName;
					info.slika = slika;
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

			FormPreduvjeti forma = new FormPreduvjeti(preduvjeti);
			if (forma.ShowDialog() == DialogResult.OK)
				postaviPreduvjete(forma.preduvjeti);
		}

	}
}
