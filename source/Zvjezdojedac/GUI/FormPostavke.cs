using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Alati;

namespace Zvjezdojedac.GUI
{
	public partial class FormPostavke : Form
	{
		List<string> kodoviJezika = new List<string>();
		Font pocetniFont;

		public FormPostavke()
		{
			InitializeComponent();

			this.pocetniFont = this.Font;

			kodoviJezika.AddRange(Jezik.Popis.Keys);
			kodoviJezika.Sort((a, b) => Jezik.Popis[a].CompareTo(Jezik.Popis[b]));

			int trenutniJezik = 0;
			foreach (string kod in kodoviJezika) {
				if (kod == Postavke.Jezik.kod)
					trenutniJezik = cbJezik.Items.Count;
				cbJezik.Items.Add(Jezik.Popis[kod]);
			}
			cbJezik.SelectedIndex = trenutniJezik;

			List<int> velicine = new List<int>();
			velicine.AddRange(Yielder.Raspon(5, 100, 5));
			velicine.AddRange(Yielder.Raspon(100, 200, 10));
			velicine.AddRange(Yielder.Raspon(200, 400, 20, true));
			velicine.Reverse();

			int trenutnaVelicina = 0;
			foreach (int velicina in velicine) {
				if (Math.Abs(velicine[trenutnaVelicina] - Postavke.VelicinaSucelja) > Math.Abs(velicina - Postavke.VelicinaSucelja))
					trenutnaVelicina = cbVelicina.Items.Count;
				cbVelicina.Items.Add(new TagTekst<int>(velicina, velicina + " %"));
			}
			cbVelicina.SelectedIndex = trenutnaVelicina;

			postaviJezik();
		}

		private void postaviJezik()
		{
			Jezik jezik = Postavke.Jezik;

			btnOk.Text = jezik[Kontekst.FormPostavke, "BTN_OK"].tekst(null);
			lblJezik.Text = jezik[Kontekst.FormPostavke, "LBL_JEZIK"].tekst(null);
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (kodoviJezika[cbJezik.SelectedIndex] != Postavke.Jezik.kod)
				Postavke.PostaviJezik(kodoviJezika[cbJezik.SelectedIndex]);

			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void cbVelicina_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbVelicina.SelectedItem == null) return;

			int velicina = (cbVelicina.SelectedItem as TagTekst<int>).tag;
			
			Postavke.VelicinaSucelja = velicina;
			this.Font = Postavke.FontSucelja(pocetniFont);
		}
	}
}
