using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Igra;
using System.Drawing;

namespace Zvjezdojedac.Podaci
{
	public class Postavke
	{
		public class ProslaIgra
		{
			public static int BrIgraca;

			public static string ImeIgraca;

			public static int Organizacija;
			
			public static int PocetnaPop;

			public static int VelicinaMape;

			public static void Postavi(Dictionary<string, string> podatci)
			{
				ImeIgraca = (podatci.ContainsKey("IME_IGRACA")) ? podatci["IME_IGRACA"] : "Marko Kovač";

				BrIgraca = 4;
				if (podatci.ContainsKey("BR_IGRACA"))
					if (!int.TryParse(podatci["BR_IGRACA"], out BrIgraca))
						BrIgraca = 4;
				if (BrIgraca < 1) BrIgraca = 1;
				if (BrIgraca > IgraZvj.maxIgraca) BrIgraca = IgraZvj.maxIgraca;

				Organizacija = 0;
				if (podatci.ContainsKey("ORGANIZACIJA"))
					if (!int.TryParse(podatci["ORGANIZACIJA"], out Organizacija))
						Organizacija = 0;

				PocetnaPop = PocetnaPopulacija.konfiguracije.Count / 2;
				if (podatci.ContainsKey("POCETNA_POPULACIJA"))
					if (!int.TryParse(podatci["POCETNA_POPULACIJA"], out PocetnaPop))

				VelicinaMape = Mapa.velicinaMape.Count / 2;
				if (podatci.ContainsKey("VELICINA_MAPE"))
					if (!int.TryParse(podatci["VELICINA_MAPE"], out VelicinaMape))
						VelicinaMape = Mapa.velicinaMape.Count / 2;
			}
		}

		public static Jezik Jezik { get; private set; }

		public static int VelicinaSucelja = 100;

		public static void PostaviJezik(string kod)
		{
			Jezik = Jezik.IzDatoteka(kod);
		}

		public static Font FontSucelja(Font prototip)
		{
			return new Font(prototip.FontFamily, prototip.Size * VelicinaSucelja / 100f);
		}

		public static void Ucitaj(Dictionary<string, string> podatci)
		{
			string jezikKod;
			if (podatci.ContainsKey("JEZIK"))
				jezikKod = podatci["JEZIK"];
			else
				jezikKod = Jezik.UobicajeniJezik;
			Jezik = Jezik.IzDatoteka(jezikKod);

			VelicinaSucelja = 100;
			if (podatci.ContainsKey("VELICINA_SUCELJA"))
				try {
					VelicinaSucelja = int.Parse(podatci["VELICINA_SUCELJA"]);
				}
				catch { }
		}

		public static void Spremi()
		{
			StreamWriter fajla = new StreamWriter("postavke.txt");
			Dictionary<string, string> podatci = new Dictionary<string, string>();
			podatci.Add("IME_IGRACA", ProslaIgra.ImeIgraca);
			podatci.Add("BR_IGRACA", ProslaIgra.BrIgraca.ToString());
			podatci.Add("ORGANIZACIJA", ProslaIgra.Organizacija.ToString());
			podatci.Add("VELICINA_MAPE", ProslaIgra.VelicinaMape.ToString());
			podatci.Add("POCETNA_POPULACIJA", ProslaIgra.PocetnaPop.ToString());
			PodaciAlat.spremi(fajla, podatci, "PROSLA_IGRA");

			podatci.Clear();
			podatci.Add("JEZIK", Jezik.kod);
			PodaciAlat.spremi(fajla, podatci, "POSTAVKE");
			fajla.Close();
		}
	}
}
