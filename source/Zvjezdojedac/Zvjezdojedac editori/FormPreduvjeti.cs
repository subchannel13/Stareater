using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prototip;

namespace Zvjezdojedac_editori
{
	public partial class FormPreduvjeti : ValidatorForm
	{
		public List<Tehnologija.Preduvjet> preduvjeti { get; private set; }
		private Dictionary<string, string> tehKodovi = new Dictionary<string,string>();
		
		public FormPreduvjeti(List<Tehnologija.Preduvjet> preduvjeti)
		{
			InitializeComponent();

			addValidation(new Validation(txtNivo, InputType.Forumla, lblNivoGreska));

			foreach(Tehnologija.TechInfo teh in Tehnologija.TechInfo.tehnologijeIstrazivanje)
				tehKodovi.Add(teh.kod + "_LVL", teh.ime);
			foreach(Tehnologija.TechInfo teh in Tehnologija.TechInfo.tehnologijeRazvoj)
				tehKodovi.Add(teh.kod + "_LVL", teh.ime);

			this.preduvjeti = new List<Tehnologija.Preduvjet>(preduvjeti);
			foreach (Tehnologija.Preduvjet p in preduvjeti)
			{
				ListViewItem item = new ListViewItem(tehKodovi[p.kod]);
				item.SubItems.Add(p.nivo.ToString());
				lstvPreduvjeti.Items.Add(item);
			}
		}

		protected override void addoditionalChangeHandle()
		{
			Formula nivo = Formula.IzStringa(txtNivo.Text);
			List<Formula.Varijabla> varijable = nivo.popisVarijabli();

			if (varijable.Count == 1) return;
			//???
		}

		protected override bool valid()
		{
			return base.valid();

			Formula nivo = Formula.IzStringa(txtNivo.Text);
			List<Formula.Varijabla> varijable = nivo.popisVarijabli();
			//???
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void btnUkloni_Click(object sender, EventArgs e)
		{
			if (lstvPreduvjeti.SelectedItems.Count == 0)
				return;

			lstvPreduvjeti.Items.RemoveAt(lstvPreduvjeti.SelectedIndices[0]);
		}
	}
}
