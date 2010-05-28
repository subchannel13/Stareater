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

		public static Image FlotaTab = null;
		public static Image SlikaOdabiraZvijezde;
		public static Image Poruka;

		public static void DodajSliku(Dictionary<string, string> podatci)
		{
			string putanja = podatci["DATOTEKA"];
			string skupina = podatci["SKUPINA"].Trim().ToLower();
			int indeks = int.Parse(podatci["INDEKS"]);

			Image img = Image.FromFile(putanja);

			if (skupina == "flota_tab") FlotaTab = img;
			if (skupina == "planet_tab") {
				Planet.Tip tip = (Planet.Tip)indeks;
				double atmGust = double.Parse(podatci["ATM_GUST"]);
				double atmKval = double.Parse(podatci["ATM_KVAL"]);
				double temp = double.Parse(podatci["TEMPERATURA"]);

				if (!PlanetTab.ContainsKey(tip))
					PlanetTab.Add(tip, new List<SlikaPlaneta>());
				
				PlanetTab[tip].Add(new SlikaPlaneta(img, atmGust, atmKval, temp));
				PlanetImageIndex.Add(img, PlanetImageIndex.Count);
			}
			if (skupina == "odabir_zvijezde") SlikaOdabiraZvijezde = img;
			if (skupina == "poruka") Poruka = img;
			if (skupina == "flota")
				foreach (Color boja in Igrac.BojeIgraca)
					Flota.Add(boja, ModulirajBoju(img, boja));
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
	}
}
