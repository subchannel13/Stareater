using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Igra.Brodovi;

namespace Zvjezdojedac.Igra.Bitka
{
	public class Borac
	{
		public Dizajn Dizajn { get; private set; }
		public Igrac Igrac { get; private set; }
		public double Pozicija;
		public int CiljnaPozicija;

		public double IzdrzljivostOklopa;
		public double IzdrzljivostStita;
		public bool[] Vidljiv = new bool[IgraZvj.MaxIgraca];

		public Borac(Dizajn dizajn, Igrac igrac, double izdrzljivostOklopa)
		{
			this.Dizajn = dizajn;
			this.Igrac = igrac;
			this.IzdrzljivostOklopa = izdrzljivostOklopa;
			this.IzdrzljivostStita = dizajn.izdrzljivostStita;

			this.Pozicija = Pozicije.PocetnaPozicija;
			this.CiljnaPozicija = Dizajn.pozeljnaPozicija;
		}
	}
}
