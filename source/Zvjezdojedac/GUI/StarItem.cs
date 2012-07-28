using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Igra;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Podaci.Jezici;

namespace Zvjezdojedac.GUI
{
	public partial class StarItem : UserControl
	{
		static Dictionary<string, ITekst> jezik;
		ZvjezdanaUprava uprava;

		public StarItem()
		{
			InitializeComponent();
		}

		public StarItem(ZvjezdanaUprava uprava) : this()
		{
			jezik = Postavke.Jezik[Kontekst.Kolonija];
			this.uprava = uprava;

			starImage.Image = Slike.ZvijezdaTab[uprava.LokacijaZvj.tip];
			starName.Text = uprava.LokacijaZvj.ime;

			double populacija = 0;
			foreach (Planet planet in uprava.LokacijaZvj.planeti)
				if (planet.kolonija != null && planet.kolonija.Igrac == uprava.Igrac)
					populacija += planet.kolonija.Populacija;

			populationText.Text = Fje.PrefiksFormater(populacija);
			industryText.Text = Fje.PrefiksFormater(uprava.Efekti[ZvjezdanaUprava.MaxGradnja]);

			resourceSlider.Value = (int)(uprava.UdioGradnje * resourceSlider.Maximum);
			buildingInfo.Text = uprava.ProcjenaVremenaGradnje();
			osvjeziLabele();
		}

		private void buildingButton_Click(object sender, EventArgs e)
		{
			using (FormGradnja frmGradnja = new FormGradnja(uprava))
				if (frmGradnja.ShowDialog() == DialogResult.OK)
					osvjeziLabele();
		}

		private void resourceSlider_Scroll(object sender, ScrollEventArgs e)
		{
			int systemValue = (int)Math.Round(uprava.UdioGradnje * resourceSlider.Maximum, MidpointRounding.AwayFromZero);
			if (e.NewValue != systemValue) {
				uprava.UdioGradnje = e.NewValue / (double)resourceSlider.Maximum;
				osvjeziLabele();
			}
		}

		private void osvjeziLabele()
		{
			uprava.IzracunajEfekte();
			uprava.OsvjeziInfoGradnje();

			buildingInfo.Text = uprava.ProcjenaVremenaGradnje();

			if (uprava.RedGradnje.Count > 0)
				buildingButton.BackgroundImage = uprava.RedGradnje.First.Value.slika;
			else
				buildingButton.BackgroundImage = null;
		}
	}
}
