using System;
using System.Collections.Generic;
using System.Text;
using Zvjezdojedac.Podaci;

namespace Zvjezdojedac.Igra.Brodovi
{
	public class Brod : IPohranjivoSB
	{
		public Dizajn dizajn { get; private set; }
		public long kolicina { get; private set; }

		public Brod(Dizajn dizajn, long kolicina)
		{
			this.dizajn = dizajn;
			this.kolicina = kolicina;
		}

		public void dodaj(Brod brod)
		{
			kolicina += brod.kolicina;
		}

		public void ukloni(long kolicina)
		{
			this.kolicina = Math.Max(this.kolicina - kolicina, 0);
		}

		#region Pohrana
		public const string PohranaTip = "BROD";
		private const string PohDizajn = "DIZAJN";
		private const string PohKolicina = "KOL";
		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohDizajn, dizajn.id);
			izlaz.dodaj(PohKolicina, kolicina);
		}

		public static Brod Ucitaj(PodaciCitac ulaz, Dictionary<int, Dizajn> dizajnovi)
		{
			Dizajn dizajn = dizajnovi[ulaz.podatakInt(PohDizajn)];
			long kolicina = ulaz.podatakLong(PohKolicina);

			return new Brod(dizajn, kolicina);
		}
		#endregion
	}
}
