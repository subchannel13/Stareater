using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Igra;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Podaci.Jezici;

namespace Zvjezdojedac.GUI
{
	public partial class BriefPlanetItem : UserControl
	{
		static Font normalniFont;
		static Font italicFont;

		public Planet Planet { get; private set; }

		public BriefPlanetItem()
		{
			InitializeComponent();

			if (normalniFont == null) normalniFont = planetInfo1.Font;
			if (italicFont == null) italicFont = new Font(planetInfo1.Font, FontStyle.Italic);

			Planet = null;
		}

		public void PostaviNeistrazeno(int indeks)
		{
			if (indeks == 0) {
				Visible = true;
				planetInfo1.Text = Postavke.Jezik[Kontekst.FormIgra]["zvjNeistrazeno"].tekst();
				planetInfo1.Font = italicFont;

				planetInfo2.Text = "";
				planetImage.Image = null;
			}
			else
				Visible = false;

			Planet = null;
		}

		public void PostaviPlanet(Planet planet, Igrac igrac)
		{
			if (planet.tip == Planet.Tip.NIKAKAV)
				Visible = false;
			else {
				Visible = true;
				planetImage.Image = planet.slika;
				planetInfo1.Text = planet.ime;
				planetInfo1.Font = normalniFont;

				if (planet.kolonija != null) {
					planetInfo1.ForeColor = planet.kolonija.igrac.boja;
					planetInfo2.ForeColor = planet.kolonija.igrac.boja;

					if (planet.kolonija.igrac == igrac)
						planetInfo2.Text = Fje.PrefiksFormater(planet.kolonija.populacija) + " / " + Fje.PrefiksFormater(planet.kolonija.efekti[Kolonija.PopulacijaMax]);
					else
						planetInfo2.Text = "";
				}
				else {
					planetInfo1.ForeColor = Color.White;
					planetInfo2.ForeColor = Color.White;
					planetInfo2.Text = "";
				}

				this.Planet = planet;
			}
		}

		private void ChildControl_Click(object sender, EventArgs e)
		{
			this.OnClick(e);
		}
	}
}
