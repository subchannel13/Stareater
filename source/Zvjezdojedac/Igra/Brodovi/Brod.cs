using System;
using System.Collections.Generic;
using System.Text;

namespace Prototip
{
	public class Brod : IPohranjivoSB
	{
		public Dizajn dizajn { get; private set; }
		public int kolicina { get; private set; }

		public Brod(Dizajn dizajn, int kolicina)
		{
			this.dizajn = dizajn;
			this.kolicina = kolicina;
		}

		public void dodaj(Brod brod)
		{
			kolicina += brod.kolicina;
		}

		#region Pohrana
		public const string PohranaTip = "BROD";
		public const string PohDizajn = "DIZAJN";
		public const string PohKolicina = "KOL";
		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohDizajn, dizajn.id);
			izlaz.dodaj(PohKolicina, kolicina);
		}
		#endregion
	}
}
