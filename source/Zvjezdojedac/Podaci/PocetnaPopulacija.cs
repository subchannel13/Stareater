using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Podaci
{
	public class PocetnaPopulacija
	{
		public static List<PocetnaPopulacija> konfiguracije = new List<PocetnaPopulacija>();

		public long Populacija;
		public double UdioRadnihMjesta;
		public int BrKolonija;
		public string NazivKod;

		public PocetnaPopulacija(long populacija, int brKolonija, string nazivKod, double udioRadnihMjesta)
		{
			this.BrKolonija = brKolonija;
			this.NazivKod = nazivKod;
			this.Populacija = populacija;
			this.UdioRadnihMjesta = udioRadnihMjesta;
		}

		public static void novaKonfiguracija(Dictionary<string, string> podatci)
		{
			long pocetnaPop = (long)double.Parse(podatci["POPULACIJA"], PodaciAlat.DecimalnaTocka);
			int brKolonija = int.Parse(podatci["BR_KOLONIJA"]);
			string nazivKod = podatci["NAZIV"];
			double udioRadnihMjesta = double.Parse(podatci["UDIO_RADNIH_MJ"], PodaciAlat.DecimalnaTocka);

			konfiguracije.Add(new PocetnaPopulacija(pocetnaPop, brKolonija, nazivKod, udioRadnihMjesta)); ;
		}
	}
}
