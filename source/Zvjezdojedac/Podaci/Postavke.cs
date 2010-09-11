using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Prototip.Podaci.Jezici;

namespace Prototip.Podaci
{
	public class Postavke
	{
		public class ProslaIgra
		{
			public static int brIgraca;

			public static string imeIgraca;

			public static int organizacija;
			
			public static int velicinaMape;

			public static void Postavi(Dictionary<string, string> podatci)
			{
				imeIgraca = (podatci.ContainsKey("IME_IGRACA")) ? podatci["IME_IGRACA"] : "Marko Kovač";

				brIgraca = 4;
				if (podatci.ContainsKey("BR_IGRACA"))
					if (!int.TryParse(podatci["BR_IGRACA"], out brIgraca))
						brIgraca = 4;
				if (brIgraca < 1) brIgraca = 1;
				if (brIgraca > Igra.maxIgraca) brIgraca = Igra.maxIgraca;

				organizacija = 0;
				if (podatci.ContainsKey("ORGANIZACIJA"))
					if (!int.TryParse(podatci["ORGANIZACIJA"], out organizacija))
						organizacija = 0;

				velicinaMape = Mapa.velicinaMape.Count / 2;
				if (podatci.ContainsKey("VELICINA_MAPE"))
					if (!int.TryParse(podatci["VELICINA_MAPE"], out velicinaMape))
						velicinaMape = Mapa.velicinaMape.Count / 2;
			}
		}

		public static Jezik jezik { get; private set; }

		public static void PostaviJezik(string kod)
		{
			jezik = Jezik.IzDatoteka(kod);
		}

		public static void Ucitaj(Dictionary<string, string> podatci)
		{
			string jezikKod;
			if (podatci.ContainsKey("JEZIK"))
				jezikKod = podatci["JEZIK"];
			else
				jezikKod = Jezik.Popis[Jezik.UobicajeniJezik];
			jezik = Jezik.IzDatoteka(jezikKod);
		}

		public static void Spremi()
		{
			StreamWriter fajla = new StreamWriter("postavke.txt");
			Dictionary<string, string> podatci = new Dictionary<string, string>();
			podatci.Add("IME_IGRACA", ProslaIgra.imeIgraca);
			podatci.Add("BR_IGRACA", ProslaIgra.brIgraca.ToString());
			podatci.Add("ORGANIZACIJA", ProslaIgra.organizacija.ToString());
			podatci.Add("VELICINA_MAPE", ProslaIgra.velicinaMape.ToString());
			PodaciAlat.spremi(fajla, podatci, "PROSLA_IGRA");

			podatci.Clear();
			podatci.Add("JEZIK", jezik.kod);
			PodaciAlat.spremi(fajla, podatci, "POSTAVKE");
			fajla.Close();
		}
	}
}
