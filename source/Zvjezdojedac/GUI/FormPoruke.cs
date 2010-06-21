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
		//public delegate void FokusNaPlanet(Planet planet);

		public Poruka odabranaProuka;

		public FormPoruke(Igrac igrac)
		{
			InitializeComponent();

			foreach (Poruka poruka in igrac.poruke)
			{
				ListViewItem item = new ListViewItem(poruka.tekst);
				item.Tag = poruka;
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
