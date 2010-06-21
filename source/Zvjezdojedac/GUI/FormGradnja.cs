using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prototip
{
	public partial class FormGradnja : Form
	{
		private Kolonija kolonija;
        private LinkedList<Zgrada.ZgradaInfo> redGradnje;
        private bool civilnaGradnja;

		public FormGradnja(Kolonija kolonija, bool civilnaGradnja)
		{
			InitializeComponent();
			lblZgradaInfo.Text = "";
            if (civilnaGradnja)
                redGradnje = kolonija.redCivilneGradnje;
            else
                redGradnje = kolonija.redVojneGradnje;

            this.civilnaGradnja = civilnaGradnja;
			this.kolonija = kolonija;
			foreach (Zgrada.ZgradaInfo z in kolonija.moguceGraditi(civilnaGradnja))
				lstMoguceGradit.Items.Add(z);

            foreach (Zgrada.ZgradaInfo z in redGradnje)
				lstRedGradnje.Items.Add(z);
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
            List<Zgrada.ZgradaInfo> ret = new List<Zgrada.ZgradaInfo>();
			foreach (object o in lstRedGradnje.Items)
				ret.Add((Zgrada.ZgradaInfo)o);

            redGradnje.Clear();
            foreach (Zgrada.ZgradaInfo z in ret)
                redGradnje.AddLast(z);

			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnDodaj_Click(object sender, EventArgs e)
		{
			if (lstMoguceGradit.SelectedItem == null)
				return;

			lstRedGradnje.Items.Add(lstMoguceGradit.SelectedItem);
			lstMoguceGradit.Items.Remove(lstMoguceGradit.SelectedItem);
		}

		private void btnUkloni_Click(object sender, EventArgs e)
		{
			if (lstRedGradnje.SelectedItem == null)
				return;

			lstMoguceGradit.Items.Add(lstRedGradnje.SelectedItem);
			lstRedGradnje.Items.Remove(lstRedGradnje.SelectedItem);
		}

		private void lstMoguceGradit_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstMoguceGradit.SelectedItem == null)
				return;

			Zgrada.ZgradaInfo zgrada = (Zgrada.ZgradaInfo)lstMoguceGradit.SelectedItem;
			picSlikaZgrade.Image = zgrada.slika;
			lblZgradaInfo.Text = zgrada.opis;
		}

		private void lstMoguceGradit_DoubleClick(object sender, EventArgs e)
		{
			btnDodaj_Click(sender, e);
		}

		private void btnPrije_Click(object sender, EventArgs e)
		{
			if (lstRedGradnje.SelectedItem == null || lstRedGradnje.SelectedIndex <= 0)
				return;

			object tmp = lstRedGradnje.SelectedItem;
			lstRedGradnje.Items[lstRedGradnje.SelectedIndex] = lstRedGradnje.Items[lstRedGradnje.SelectedIndex - 1];
			lstRedGradnje.Items[lstRedGradnje.SelectedIndex - 1] = tmp;
			lstRedGradnje.SelectedIndex--;
		}

		private void btnKasnije_Click(object sender, EventArgs e)
		{
			if (lstRedGradnje.SelectedItem == null || lstRedGradnje.SelectedIndex >= lstRedGradnje.Items.Count - 1)
				return;

			object tmp = lstRedGradnje.SelectedItem;
			lstRedGradnje.Items[lstRedGradnje.SelectedIndex] = lstRedGradnje.Items[lstRedGradnje.SelectedIndex + 1];
			lstRedGradnje.Items[lstRedGradnje.SelectedIndex + 1] = tmp;
			lstRedGradnje.SelectedIndex++;
		}

		private void lstRedGradnje_DoubleClick(object sender, EventArgs e)
		{
			btnUkloni_Click(sender, e);
		}

	}
}
