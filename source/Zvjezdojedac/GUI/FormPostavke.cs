using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Prototip.Podaci.Jezici;
using Prototip.Podaci;

namespace Prototip
{
	public partial class FormPostavke : Form
	{
		List<string> kodoviJezika = new List<string>();

		public FormPostavke()
		{
			InitializeComponent();

			kodoviJezika.AddRange(Jezik.Popis.Keys);
			kodoviJezika.Sort((a, b) => Jezik.Popis[a].CompareTo(Jezik.Popis[b]));

			int trenutniJezik = 0;
			foreach (string kod in kodoviJezika) {
				if (kod == Postavke.jezik.kod)
					trenutniJezik = cbJezik.Items.Count;
				cbJezik.Items.Add(Jezik.Popis[kod]);
			}
			cbJezik.SelectedIndex = trenutniJezik;
			
			postaviJezik();
		}

		private void postaviJezik()
		{
			Jezik jezik = Postavke.jezik;

			btnOk.Text = jezik[Kontekst.FormPostavke, "BTN_OK"].tekst(null);
			lblJezik.Text = jezik[Kontekst.FormPostavke, "LBL_JEZIK"].tekst(null);
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (kodoviJezika[cbJezik.SelectedIndex] != Postavke.jezik.kod)
				Postavke.PostaviJezik(kodoviJezika[cbJezik.SelectedIndex]);

			this.DialogResult = DialogResult.OK;
			Close();
		}
	}
}
