using System;
using System.Collections.Generic;
using System.Text;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Podaci.Formule;

namespace Zvjezdojedac.Igra.Brodovi
{
	public static class Pozicije
	{
		static Formula preciznost;
		static Formula snagaSenzora;
		static Dictionary<int, string> kodoviNaziva = new Dictionary<int, string>();

		public static int PocetnaPozicija { get; private set; }
		public static int UobicajenaPozicija { get; private set; }
		public static int MaxPozicija { get; private set; }

		public static void DodajImenovanuPoziciju(Dictionary<string, string> podaci)
		{
			kodoviNaziva.Add(int.Parse(podaci["POZICIJA"]), podaci["NAZIV_KOD"]);

			MaxPozicija = int.MinValue;
			foreach (int pozicija in kodoviNaziva.Keys)
				MaxPozicija = Math.Max(pozicija, MaxPozicija);
		}

		public static void DefinirajPozicije(Dictionary<string, string> podaci)
		{
			preciznost = Formula.IzStringa(podaci["PRECIZNOST"]);
			snagaSenzora = Formula.IzStringa(podaci["SNAGA_SENZORA"]);

			PocetnaPozicija = int.Parse(podaci["POCETNA"]);
			UobicajenaPozicija = int.Parse(podaci["UOBICAJENA"]);
		}

		public static IEnumerable<int> PonudjenePozicije()
		{
			List<int> rez = new List<int>(kodoviNaziva.Keys);
			rez.Sort();
			return rez;
		}

		public static string Naziv(int pozicija)
		{
			return Postavke.Jezik[Kontekst.Bitka, kodoviNaziva[pozicija]].tekst();
		}

		public class EfektUdaljenosti
		{
			public static EfektUdaljenosti Izracunaj(double udaljenost)
			{
				Dictionary<string, double> varijable = new Dictionary<string, double>();
				varijable.Add("DELTA", Math.Round(udaljenost));

				return new EfektUdaljenosti(
					preciznost.iznos(varijable),
					snagaSenzora.iznos(varijable));
			}

			public double Preciznost { get; private set; }
			public double SnagaSenzora { get; private set; }

			private EfektUdaljenosti(double preciznost, double snagaSenzora)
			{
				Preciznost = preciznost;
				SnagaSenzora = snagaSenzora;
			}
		}
	}
}
