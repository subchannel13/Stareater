using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Igra;

namespace Zvjezdojedac.GUI
{
	public partial class PlanetItem : UserControl
	{
		private Igra.Kolonija kolonija;
		private Dictionary<string, ITekst> jezik;

		public PlanetItem()
		{
			InitializeComponent();
			populationIcon.Image = Slike.populacijaIkona;
			industryIcon.Image = Slike.industrijaIkona;
		}

		public PlanetItem(Igra.Kolonija kolonija) : this()
		{
			this.kolonija = kolonija;

			jezik = Postavke.Jezik[Kontekst.Kolonija];
			this.kolonija = kolonija;

			planetImage.Image = kolonija.slika;
			planetName.Text = kolonija.ime;

			populationText.Text = Fje.PrefiksFormater(kolonija.Populacija);
			industryText.Text = Fje.PrefiksFormater(kolonija.Efekti[Kolonija.IndustrijaPoRadniku] * kolonija.Efekti[Kolonija.BrRadnika]);

			resourceSlider.Value = (int)(kolonija.UdioIndustrije * resourceSlider.Maximum);
			buildingInfo.Text = kolonija.ProcjenaVremenaGradnje();
			osvjeziLabele();
		}

		private void osvjeziLabele()
		{
			kolonija.OsvjeziInfoGradnje();
			buildingInfo.Text = kolonija.ProcjenaVremenaGradnje();

			if (kolonija.RedGradnje.Count > 0)
				buildingButton.BackgroundImage = kolonija.RedGradnje.First.Value.slika;
			else
				buildingButton.BackgroundImage = null;
		}

		private void buildingButton_Click(object sender, EventArgs e)
		{
			using (FormGradnja frmGradnja = new FormGradnja(kolonija))
				if (frmGradnja.ShowDialog() == DialogResult.OK)
					osvjeziLabele();
		}

		private void resourceSlider_Scroll(object sender, ScrollEventArgs e)
		{
			int systemValue = (int)Math.Round(kolonija.UdioIndustrije * resourceSlider.Maximum, MidpointRounding.AwayFromZero);
			if (e.NewValue != systemValue) {
				kolonija.UdioIndustrije = e.NewValue / (double)resourceSlider.Maximum;
				osvjeziLabele();
			}
		}
	}
}
