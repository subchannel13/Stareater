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
		Image imageCanvas;

		public BriefPlanetItem()
		{
			InitializeComponent();
			imageCanvas = new Bitmap(planetImage.Width, planetImage.Height);

			if (normalniFont == null) normalniFont = planetInfo1.Font;
			if (italicFont == null) italicFont = new Font(planetInfo1.Font, FontStyle.Italic);

			Planet = null;
		}

		public void PostaviNeistrazeno(int indeks)
		{
			if (indeks == 0) {
				postaviVidljivost(true);
				planetInfo1.Text = Postavke.Jezik[Kontekst.FormIgra]["zvjNeistrazeno"].tekst();
				planetInfo1.Font = italicFont;

				planetInfo2.Text = "";
				planetImage.Image = null;
			}
			else
				postaviVidljivost(false);

			Planet = null;
		}

		public void PostaviPlanet(Planet planet, Igrac igrac)
		{
			if (planet.tip == Planet.Tip.NIKAKAV)
				postaviVidljivost(false);
			else {
				postaviVidljivost(true);

				bool igracevPlanet = (planet.kolonija != null && planet.kolonija.Igrac == igrac);
				planetImage.Image = sastaviSlikuPlaneta(planet, igracevPlanet);
				planetInfo1.Text = planet.ime;
				planetInfo1.Font = normalniFont;

				if (planet.kolonija != null) {
					planetInfo1.ForeColor = planet.kolonija.Igrac.boja;
					planetInfo2.ForeColor = planet.kolonija.Igrac.boja;

					if (igracevPlanet)
						planetInfo2.Text = Fje.PrefiksFormater(planet.kolonija.populacija) + " / " + Fje.PrefiksFormater(planet.kolonija.Efekti[Kolonija.PopulacijaMax]);
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

		private Image sastaviSlikuPlaneta(Planet planet, bool igracevPlanet)
		{
			Graphics g = Graphics.FromImage(imageCanvas);
			g.Clear(Color.Transparent);

			if (igracevPlanet)
				Slike.NacrtajRazvijenost(g, 0, 0, planet.kolonija.Razvijenost);
			g.DrawImage(planet.slika, imageCanvas.Width - planet.slika.Width, 0);
			g.Dispose();

			return imageCanvas;
		}

		private void postaviVidljivost(bool seVidi)
		{
			foreach (Control dijete in this.Controls)
				dijete.Visible = seVidi;
		}

		private void ChildControl_Click(object sender, EventArgs e)
		{
			this.OnClick(e);
		}
	}
}
