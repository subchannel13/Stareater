using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Igra;
using Zvjezdojedac.Igra.Poruke;

namespace Zvjezdojedac.GUI
{
	public partial class FormPoruke : Form
	{
		public Poruka odabranaProuka;
		
		private Igrac igrac;
		private Dictionary<CheckBox, Poruka.Tip> tipZaCheckBox = new Dictionary<CheckBox, Poruka.Tip>();

		public FormPoruke(Igrac igrac)
		{
			InitializeComponent();
			this.igrac = igrac;

			lstvPoruke.SmallImageList = new ImageList();
			lstvPoruke.SmallImageList.Images.Add(Slike.TipPoruke[Poruka.Tip.Prica]);
			lstvPoruke.SmallImageList.Images.Add(Slike.TipPoruke[Poruka.Tip.Tehnologija]);
			lstvPoruke.SmallImageList.Images.Add(Slike.TipPoruke[Poruka.Tip.Kolonija]);
			lstvPoruke.SmallImageList.Images.Add(Slike.TipPoruke[Poruka.Tip.Zgrada]);
			lstvPoruke.SmallImageList.Images.Add(Slike.TipPoruke[Poruka.Tip.Brod]);

			this.Enabled = false;
			chTipBrod.Checked = igrac.filtarPoruka[Poruka.Tip.Brod];
			chTipKolonija.Checked = igrac.filtarPoruka[Poruka.Tip.Kolonija];
			chTipTehnologije.Checked = igrac.filtarPoruka[Poruka.Tip.Tehnologija];
			chTipZgrade.Checked = igrac.filtarPoruka[Poruka.Tip.Zgrada];

			tipZaCheckBox.Add(chTipBrod, Poruka.Tip.Brod);
			tipZaCheckBox.Add(chTipKolonija, Poruka.Tip.Kolonija);
			tipZaCheckBox.Add(chTipTehnologije, Poruka.Tip.Tehnologija);
			tipZaCheckBox.Add(chTipZgrade, Poruka.Tip.Zgrada);
			this.Enabled = true;

			postaviPoruke();

			odabranaProuka = null;

			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormPoruke];
			chTipBrod.Text = jezik["chTipBrod"].tekst();
			chTipKolonija.Text = jezik["chTipKolonija"].tekst();
			chTipTehnologije.Text = jezik["chTipTehnologije"].tekst();
			chTipZgrade.Text = jezik["chTipZgrade"].tekst();

			this.Text = jezik["naslov"].tekst();
		}

		private void postaviPoruke()
		{
			lstvPoruke.Items.Clear();
			foreach (Poruka poruka in igrac.poruke) {
				if (!igrac.filtarPoruka[poruka.tip]) continue;

				ListViewItem item = new ListViewItem(poruka.tekst);
				item.Tag = poruka;
				item.ImageIndex = (int)poruka.tip;
				lstvPoruke.Items.Add(item);
			}
		}

		private void promjenaFiltra(CheckBox chBox)
		{
			if (!this.Enabled) return;
			igrac.filtarPoruka[tipZaCheckBox[chBox]] = chBox.Checked;
			postaviPoruke();
		}

		private void lstvPoruke_ItemActivate(object sender, EventArgs e)
		{
			if (lstvPoruke.SelectedItems.Count == 0)
				return;

			odabranaProuka = (Poruka)lstvPoruke.SelectedItems[0].Tag;
			DialogResult = DialogResult.OK;
			Close();
		}

		private void chTipBrod_CheckedChanged(object sender, EventArgs e)
		{
			promjenaFiltra(chTipBrod);
		}

		private void chTipKolonija_CheckedChanged(object sender, EventArgs e)
		{
			promjenaFiltra(chTipKolonija);
		}

		private void chTipTehnologije_CheckedChanged(object sender, EventArgs e)
		{
			promjenaFiltra(chTipTehnologije);
		}

		private void chTipZgrade_CheckedChanged(object sender, EventArgs e)
		{
			promjenaFiltra(chTipZgrade);
		}
	}
}
