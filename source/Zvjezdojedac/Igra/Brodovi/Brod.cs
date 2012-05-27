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
		public double izdrzljivostOklopa { get; private set; }

		public Brod(Dizajn dizajn, long kolicina)
			: this(dizajn, kolicina, dizajn.izdrzljivostOklopa * kolicina)
		{ }

		public Brod(Dizajn dizajn, long kolicina, double izdrzljivostOklopa)
		{
			this.dizajn = dizajn;
			this.kolicina = kolicina;
			this.izdrzljivostOklopa = izdrzljivostOklopa;
		}

		public void dodaj(Brod brod)
		{
			kolicina += brod.kolicina;
			izdrzljivostOklopa += brod.izdrzljivostOklopa;
		}

		public void ukloni(long kolicina)
		{
			long zaUklonit = this.kolicina - Math.Max(this.kolicina - kolicina, 0);
			
			if (zaUklonit > 0) {
				this.izdrzljivostOklopa -= zaUklonit * (this.izdrzljivostOklopa / this.kolicina);
				this.kolicina -= zaUklonit;
			}
		}

		#region Pohrana
		public const string PohranaTip = "BROD";
		private const string PohDizajn = "DIZAJN";
		private const string PohKolicina = "KOL";
		private const string PohIzdrzljivost = "OKLOP";
		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohDizajn, dizajn.id);
			izlaz.dodaj(PohKolicina, kolicina);
			izlaz.dodaj(PohIzdrzljivost, izdrzljivostOklopa);
		}

		public static Brod Ucitaj(PodaciCitac ulaz, Dictionary<int, Dizajn> dizajnovi)
		{
			Dizajn dizajn = dizajnovi[ulaz.podatakInt(PohDizajn)];
			long kolicina = ulaz.podatakLong(PohKolicina);
			double izdrzljivostOklopa = ulaz.podatakDouble(PohIzdrzljivost);

			return new Brod(dizajn, kolicina, izdrzljivostOklopa);
		}
		#endregion
	}
}
