using System;
using System.Collections.Generic;
using System.Text;

namespace Prototip
{
	public class Taktika
	{
		#region Statično
		/// <summary>
		/// Ključ je pozicija projektila. Pozicija je >= 0 a kad
		/// padne ispod 0, projektil pogađa metu.
		/// </summary>
		public static Dictionary<int, Taktika> ProjektilPozicije = new Dictionary<int, Taktika>();
		public static Dictionary<Taktika, int> Taktike = new Dictionary<Taktika, int>();
		public static Dictionary<string, Taktika> Kodovi = new Dictionary<string, Taktika>();

		public static void DodajTaktikuProjektila(Dictionary<string, string> podaci)
		{
			ProjektilPozicije.Add(int.Parse(podaci["POZICIJA"]), NaciniTaktiku(podaci));
		}

		public static void DodajTaktikuBroda(Dictionary<string, string> podaci)
		{
			Taktike.Add(NaciniTaktiku(podaci), int.Parse(podaci["POZICIJA"]));
		}

		private static Taktika NaciniTaktiku(Dictionary<string, string> podaci)
		{
			Taktika taktika = new Taktika(
				podaci["NAZIV"],
				double.Parse(podaci["KOEF_OMENANJE"]),
				double.Parse(podaci["KOEF_PRECIZNOST"]),
				double.Parse(podaci["KOEF_PRIKRIVANJE"]),
				double.Parse(podaci["KOEF_SNAGA_SENZORA"])
				);
			
			Kodovi.Add(podaci["KOD"], taktika);
			return taktika;
		}
		#endregion

		public string naziv { get; private set; }

		public double koefOmetanje { get; private set; }
		public double koefPreciznost { get; private set; }
		public double koefPrikrivanje { get; private set; }
		public double koefSnagaSenzora { get; private set; }

		public Taktika(string naziv, double koefOmetanje,
			double koefPreciznost, double koefPrikrivanje,
			double koefSnagaSenzora)
		{
			this.naziv = naziv;
			this.koefOmetanje = koefOmetanje;
			this.koefPreciznost = koefPreciznost;
			this.koefPrikrivanje = koefPrikrivanje;
			this.koefSnagaSenzora = koefSnagaSenzora;
		}
	}
}
