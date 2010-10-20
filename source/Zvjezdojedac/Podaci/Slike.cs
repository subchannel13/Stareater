using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Prototip
{
	public class Slike
	{
		public class SlikaPlaneta
		{
			public Image image { get; private set; }
			public double atmGust { get; private set; }
			public double atmKval { get; private set; }
			public double temp { get; private set; }

			public SlikaPlaneta(Image image,
				double atmGust, double atmKval, double temp)
			{
				this.atmGust = atmGust;
				this.atmKval = atmKval;
				this.image = image;
				this.temp = temp;
			}
		}

		public static Dictionary<int, Image> ZvijezdaMapa = new Dictionary<int, Image>();
		public static Dictionary<int, Image> ZvijezdaTab = new Dictionary<int, Image>();
		public static Dictionary<Planet.Tip, List<SlikaPlaneta>> PlanetTab = new Dictionary<Planet.Tip, List<SlikaPlaneta>>();
		public static Dictionary<Image, int> PlanetImageIndex = new Dictionary<Image, int>();
		public static Dictionary<Color, Image> Flota = new Dictionary<Color,Image>();
		public static Dictionary<Poruka.Tip, Image> TipPoruke = initTipPoruke();

		private static Dictionary<Misija.Tip, Image> MisijaBroda = new Dictionary<Misija.Tip, Image>();

		public static Image FlotaTab = null;
		public static Image SlikaOdabiraZvijezde;

		private static Dictionary<Poruka.Tip, Image> initTipPoruke()
		{
			Dictionary<Poruka.Tip, Image> rez = new Dictionary<Poruka.Tip, Image>();
			rez.Add(Poruka.Tip.Brod, smallImageKvadrat(Color.Gray));
			rez.Add(Poruka.Tip.Kolonija, smallImageKvadrat(Color.Brown));
			rez.Add(Poruka.Tip.Prica, smallImageKvadrat(Color.Blue));
			rez.Add(Poruka.Tip.Tehnologija, smallImageKvadrat(Color.Turquoise));
			rez.Add(Poruka.Tip.Zgrada, smallImageKvadrat(Color.LightGray));
			return rez;
		}

		private static Image smallImageKvadrat(Color boja)
		{
			Image rez = new Bitmap(32, 32);
			Graphics g = Graphics.FromImage(rez);
			g.Clear(boja);
			g.Dispose();
			return rez;
		}

		private Slike()
		{ }

		public static void DodajSliku(Dictionary<string, string> podatci)
		{
			string putanja = podatci["DATOTEKA"];
			string skupina = podatci["SKUPINA"].Trim().ToLower();
			int indeks = int.Parse(podatci["INDEKS"]);

			Image slika = Image.FromFile(putanja);

			switch (skupina) {
				case "flota_tab":
					FlotaTab = slika;
					break;
				case "planet_tab":
					Planet.Tip tip = (Planet.Tip)indeks;
					double atmGust = double.Parse(podatci["ATM_GUST"]);
					double atmKval = double.Parse(podatci["ATM_KVAL"]);
					double temp = double.Parse(podatci["TEMPERATURA"]);

					if (!PlanetTab.ContainsKey(tip))
						PlanetTab.Add(tip, new List<SlikaPlaneta>());

					PlanetTab[tip].Add(new SlikaPlaneta(slika, atmGust, atmKval, temp));
					PlanetImageIndex.Add(slika, PlanetImageIndex.Count);
					break;
				case "odabir_zvijezde":
					SlikaOdabiraZvijezde = slika;
					break;
				case "flota":
					foreach (Color boja in Igrac.BojeIgraca)
						Flota.Add(boja, ModulirajBoju(slika, boja));
					break;
				case "brod_misija":
					MisijaBroda.Add((Misija.Tip)indeks, slika);
					break;
				default:
					throw new ArgumentException("Invalid picture group \"" + skupina + "\" in ./slike/slike.txt");
			}
		}

		public static void DodajZvjezdaMapaSliku(string datoteka, int indeks)
		{
			ZvijezdaMapa.Add(indeks, Image.FromFile(datoteka));
		}

		public static void DodajZvjezdaTabSliku(string datoteka, int indeks)
		{
			ZvijezdaTab.Add(indeks, Image.FromFile(datoteka));
		}

		public static Image ModulirajBoju(Image slika, Color boja)
		{
			Bitmap ret = new Bitmap(slika);
			double rf = boja.R / 256.0;
			double gf = boja.G / 256.0;
			double bf = boja.B / 256.0;

			for (int y = 0; y < ret.Height; y++)
				for (int x = 0; x < ret.Width; x++)
					ret.SetPixel(x, y, Color.FromArgb(
						(int)(ret.GetPixel(x, y).R * rf),
						(int)(ret.GetPixel(x, y).G * gf),
						(int)(ret.GetPixel(x, y).B * bf)));
			
			return ret;
		}

		public static Image OdrediSlikuPlaneta(Planet.Tip tip, double atmGust, double atmKval, double temp)
		{
			Planet.TipInfo tipInfo = Planet.tipovi[tip];

			Image ret = null;
			double min = double.PositiveInfinity;
			foreach (SlikaPlaneta slikaPl in PlanetTab[tip]) {
				double dist = Math.Pow((slikaPl.atmGust - atmGust) * tipInfo.slikaAtmGustKoef, 2)
					+ Math.Pow((slikaPl.atmKval - atmKval) * tipInfo.slikaAtmKvalKoef, 2)
					+ Math.Pow((slikaPl.temp - temp) * tipInfo.slikaAtmTempKoef, 2);
				if (dist < min) {
					ret = slikaPl.image;
					min = dist;
				}
			}

			return ret;
		}

		public static Image NaciniIkonuBroda(Trup.TrupInfo trup, Oruzje primMisija, Oruzje sekMisija)
		{
			Image rez = new Bitmap(60, 40);
			Graphics g = Graphics.FromImage(rez);
			
			g.Clear(Color.Black);
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
			g.DrawImage(trup.slika, new Rectangle(0, 0, 40, 40), 0, 0, trup.slika.Width, trup.slika.Height, GraphicsUnit.Pixel);
			
			if (primMisija != null) g.DrawImage(MisijaBroda[primMisija.misija], 40, 0);
			if (sekMisija != null) g.DrawImage(MisijaBroda[sekMisija.misija], 40, 20);
			g.Dispose();

			return rez;
		}
	}
}
