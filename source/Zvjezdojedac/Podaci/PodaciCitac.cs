using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototip
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
		
		public PodaciCitac podObjekt(string kljuc)
		{
			return podObjekti[kljuc];
		}
		
		public string this[string kljuc]
		{
			get { return podaci[kljuc]; }
		}

		public static PodaciCitac Procitaj(string ulaz)
		{
			return Procitaj(new Queue<string>(
				ulaz.Split(new char[] { '\n' }, 
				StringSplitOptions.RemoveEmptyEntries)
				));
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
	}
}
