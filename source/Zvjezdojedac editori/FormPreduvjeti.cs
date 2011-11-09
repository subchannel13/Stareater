using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac;
using Zvjezdojedac_editori.Validation;
using Zvjezdojedac.Igra;
using Zvjezdojedac.Podaci.Formule;

namespace Zvjezdojedac_editori
{
	public partial class FormPreduvjeti : ValidatorForm
	{
		public const string imeTag = FormTehnologije.imeTag;
		public const string kodTag = FormTehnologije.kodTag;
		public const string preduvjetiTag = FormTehnologije.preduvjetiTag;

		struct TechId
		{
			public string naziv;
			public string kod;

			public TechId(string naziv, string kod)
			{
				this.naziv = naziv;
				this.kod = kod;
			}

			public override string ToString()
			{
				return naziv;
			}
		}

		public List<Preduvjet> preduvjeti { get; private set; }
		private Dictionary<string, string> tehKodovi = new Dictionary<string,string>();

		public FormPreduvjeti(List<Preduvjet> preduvjeti, List<Dictionary<string, string>> tehnologijeIst, List<Dictionary<string, string>> tehnologijeRaz)
		{
			InitializeComponent();

			addValidation(new ValidTextBoxFormula(txtNivo, lblNivoGreska, new string[]{"LVL"}));

			foreach (Dictionary<string, string> teh in tehnologijeIst)
				tehKodovi.Add(teh[kodTag], teh[imeTag]);
			foreach (Dictionary<string, string> teh in tehnologijeRaz)
				tehKodovi.Add(teh[kodTag], teh[imeTag]);

			this.preduvjeti = new List<Preduvjet>(preduvjeti);
			foreach (Preduvjet p in preduvjeti)
			{
				ListViewItem item = new ListViewItem(tehKodovi[p.kod]);
				item.SubItems.Add(p.nivo.ToString());
				lstvPreduvjeti.Items.Add(item);
			}

			foreach(string kod in tehKodovi.Keys)
				cbTehno.Items.Add(new TechId(tehKodovi[kod], kod));
			cbTehno.SelectedIndex = 0;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void btnUkloni_Click(object sender, EventArgs e)
		{
			if (lstvPreduvjeti.SelectedItems.Count == 0)
				return;

			int indeks = lstvPreduvjeti.SelectedIndices[0];
			preduvjeti.RemoveAt(indeks);
			lstvPreduvjeti.Items.RemoveAt(indeks);
		}

		private void lstvPreduvjeti_ItemActivate(object sender, EventArgs e)
		{
			if (lstvPreduvjeti.SelectedItems.Count == 0) return;

			int predI = lstvPreduvjeti.SelectedIndices[0];
			string kod = preduvjeti[predI].kod;

			for (int i = 0; i < cbTehno.Items.Count; i++)
			{
				TechId techId = (TechId)cbTehno.Items[i];
				if (techId.kod == kod)
				{
					cbTehno.SelectedIndex = i;
					break;
				}
			}

			txtNivo.Text = preduvjeti[predI].nivo.ToString();
		}

		private void btnDodaj_Click(object sender, EventArgs e)
		{
			if (!valid()) return;

			TechId tehId = (TechId)cbTehno.SelectedItem;
			Preduvjet pred = new Preduvjet(tehId.kod, Formula.IzStringa(txtNivo.Text), false);

			preduvjeti.Add(pred);

			ListViewItem item = new ListViewItem(tehId.naziv);
			item.SubItems.Add(pred.nivo.ToString());
			lstvPreduvjeti.Items.Add(item);
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (!valid()) return;

			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
