using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Podaci.Formule;

namespace Zvjezdojedac.Podaci
{
	public class PodaciCitac
	{
		private Dictionary<string, string> podaci = new Dictionary<string, string>();
		private Dictionary<string, PodaciCitac> podObjekti = new Dictionary<string, PodaciCitac>();

		private PodaciCitac(Dictionary<string, string> podaci,
			Dictionary<string, PodaciCitac> podObjekti)
		{
			this.podaci = podaci;
			this.podObjekti = podObjekti;
		}

		public bool ima(string kljuc)
		{
			if (podaci.ContainsKey(kljuc)) return true;
			if (podObjekti.ContainsKey(kljuc)) return true;
			return false;
		}
		
		public PodaciCitac this[string kljuc]
		{
			get { return podObjekti[kljuc]; }
		}

		public int podatakInt(string kljuc)
		{
			return int.Parse(podaci[kljuc]);
		}

		public long podatakLong(string kljuc)
		{
			return long.Parse(podaci[kljuc]);
		}

		public double podatakDouble(string kljuc)
		{
			return double.Parse(podaci[kljuc], PodaciAlat.DecimalnaTocka);
		}

		public string podatak(string kljuc)
		{
			return podaci[kljuc];
		}

		public int[] podatakIntPolje(string kljuc)
		{
			return podatakIntPolje(kljuc, ' ');
		}

		public int[] podatakIntPolje(string kljuc, char medja)
		{
			string[] vrijednosti = podaci[kljuc].Split(new char[] { medja }, StringSplitOptions.RemoveEmptyEntries);
			int[] rez = new int[vrijednosti.Length];
			
			for (int i = 0; i < vrijednosti.Length; i++)
				rez[i] = int.Parse(vrijednosti[i]);
			
			return rez;
		}

		public Dictionary<string, double> podatakDoubleRjecnik(string kljuc)
		{
			Queue<string> vrijednosti = new Queue<string>(podaci[kljuc].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
			Dictionary<string, double> rez = new Dictionary<string, double>();

			while (vrijednosti.Count > 0) {
				string rezKljuc = vrijednosti.Dequeue();
				double rezVrijednost = double.Parse(vrijednosti.Dequeue(), PodaciAlat.DecimalnaTocka);

				rez.Add(rezKljuc, rezVrijednost);
			}

			return rez;
		}

		public Formula podatakFormula(string kljuc)
		{
			return Formula.IzStringa(podaci[kljuc]);
		}

		#region Staticno
		public static PodaciCitac Procitaj(string ulaz)
		{
			Queue<string> red =	new Queue<string>(
				ulaz.Split(new char[] { '\n' }, 
				StringSplitOptions.RemoveEmptyEntries));

			string linija = null;
			do
			{
				linija = red.Dequeue();
			} while (!linija.StartsWith("<") || !linija.EndsWith(">"));

			return Procitaj(red);
		}

		public static PodaciCitac Procitaj(Queue<string> ulaz)
		{
			Dictionary<string, string> podaci = new Dictionary<string, string>();
			Dictionary<string, PodaciCitac> podObjekti = new Dictionary<string, PodaciCitac>();
			string linija = ulaz.Dequeue().Trim();

			while (linija != "----") {
				if (linija.Contains('=')){
					int split = linija.IndexOf('=');
					string kljuc = linija.Substring(0, split).Trim();
					string vrijednost = linija.Substring(split + 1).Trim();
					podaci.Add(kljuc, vrijednost);
				}
				else if (linija.StartsWith("<") && linija.EndsWith(">")) {
					string tip = linija.Substring(1, linija.Length - 2);
					podObjekti.Add(tip, Procitaj(ulaz));
				}

				linija = ulaz.Dequeue().Trim();
			}

			return new PodaciCitac(podaci, podObjekti);
		}
		#endregion
	}
}
