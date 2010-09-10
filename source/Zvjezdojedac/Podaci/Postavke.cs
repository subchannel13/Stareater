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

			public static Jezik jezik;

			public static void postavi(Dictionary<string, string> podatci)
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

				string jezikKod;
				if (podatci.ContainsKey("JEZIK"))
					jezikKod = podatci["JEZIK"];
				else
					jezikKod = Jezik.Popis[0];
				jezik = Jezik.IzDatoteka("./jezici/" + jezikKod + "/");
			}

			public static void spremi()
			{
				StreamWriter fajla = new StreamWriter("postavke.txt");
				Dictionary<string, string> podatci = new Dictionary<string, string>();
				podatci.Add("IME_IGRACA", imeIgraca);
				podatci.Add("BR_IGRACA", brIgraca.ToString());
				podatci.Add("ORGANIZACIJA", organizacija.ToString());
				podatci.Add("VELICINA_MAPE", velicinaMape.ToString());
				PodaciAlat.spremi(fajla, podatci, "PROSLA_IGRA");
				fajla.Close();
			}
		}
	}
}
