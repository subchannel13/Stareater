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
	public partial class FormPoruke : Form
	{
		public Poruka odabranaProuka;

		public FormPoruke(Igrac igrac)
		{
			InitializeComponent();

			lstvPoruke.SmallImageList = new ImageList();
			lstvPoruke.SmallImageList.Images.Add(Slike.TipPoruke[Poruka.Tip.Prica]);
			lstvPoruke.SmallImageList.Images.Add(Slike.TipPoruke[Poruka.Tip.Tehnologija]);
			lstvPoruke.SmallImageList.Images.Add(Slike.TipPoruke[Poruka.Tip.Kolonija]);
			lstvPoruke.SmallImageList.Images.Add(Slike.TipPoruke[Poruka.Tip.Zgrada]);
			lstvPoruke.SmallImageList.Images.Add(Slike.TipPoruke[Poruka.Tip.Brod]);

			foreach (Poruka poruka in igrac.poruke)
			{
				ListViewItem item = new ListViewItem(poruka.tekst);
				item.Tag = poruka;
				item.ImageIndex = (int)poruka.tip;
				lstvPoruke.Items.Add(item);				
			}

			odabranaProuka = null;
		}

		private void lstvPoruke_ItemActivate(object sender, EventArgs e)
		{
			if (lstvPoruke.SelectedItems.Count == 0)
				return;

			odabranaProuka = (Poruka)lstvPoruke.SelectedItems[0].Tag;
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
