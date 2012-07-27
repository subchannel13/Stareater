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

			setBuildingImage();
			resourceSlider.Value = (int)(uprava.UdioGradnje * resourceSlider.Maximum);
			buildingInfo.Text = uprava.ProcjenaVremenaGradnje();
		}

		void setBuildingImage()
		{
			if (uprava.RedGradnje.Count > 0) {
				buildingButton.Image = uprava.RedGradnje.First.Value.slika;
				buildingButton.Text = string.Empty;
			}
			else {
				buildingButton.Image = null;
				buildingButton.Text = jezik["Vojna_Gradnja"].tekst();
			}
		}

		private void buildingButton_Click(object sender, EventArgs e)
		{
			uprava.IzracunajEfekte();
			uprava.OsvjeziInfoGradnje();

			buildingInfo.Text = uprava.ProcjenaVremenaGradnje();

			if (uprava.RedGradnje.Count > 0) {
				buildingButton.Image = uprava.RedGradnje.First.Value.slika;
				buildingButton.Text = "";
			}
			else {
				buildingButton.Image = null;
				buildingButton.Text = jezik["Vojna_Gradnja"].tekst();
			}
		}

		private void resourceSlider_Scroll(object sender, ScrollEventArgs e)
		{
			int systemValue = (int)Math.Round(uprava.UdioGradnje * resourceSlider.Maximum, MidpointRounding.AwayFromZero);
			if (e.NewValue != systemValue) {
				uprava.UdioGradnje = e.NewValue / (double)resourceSlider.Maximum;
				int val = (int)Math.Round(uprava.UdioGradnje * resourceSlider.Maximum, MidpointRounding.AwayFromZero);
				if (resourceSlider.Value != val)
					e.NewValue = val;
				else
					osvjeziLabele();
			}
		}
	}
}
