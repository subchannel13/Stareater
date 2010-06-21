using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototip
{
	public class Poruka : IPohranjivoSB
	{
		public enum Tip
		{
			Prica = 0,
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

		#region Pohrana
		public const string PohranaTip = "PORUKA";
		private const string PohTip = "TIP";

		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohranaTip, (int)tip);
		}
		
		public static Poruka Ucitaj(PodaciCitac ulaz)
		{
			int tip = ulaz.podatakInt(PohTip);

			return new Poruka((Tip)tip, "");
		}
		#endregion
	}
}
