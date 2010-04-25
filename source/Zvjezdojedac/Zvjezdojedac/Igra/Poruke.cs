using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototip
{
	public class Poruka
	{
		public enum Tip
		{
			Prica,
			Tehnologija
		};

		public static Poruka NovaTehnologija(Tehnologija tehnologija)
		{
			return new Poruka(Tip.Tehnologija, tehnologija.tip.ime + " " + tehnologija.nivo + ". nivo");
		}

		public Tip tip;
		public string tekst;
		//public bool procitana;

		private Poruka(Tip tip, string tekst)
		{
			this.tip = tip;
			this.tekst = tekst;
			//this.procitana = false;
		}
	}
}
