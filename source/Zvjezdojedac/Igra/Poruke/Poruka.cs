using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Igra.Poruke;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Igra.Brodovi;

namespace Zvjezdojedac.Igra.Poruke
{
	public abstract class Poruka : IPohranjivoSB
	{
		public enum Tip
		{
			Prica = 0,
			Tehnologija,
			Kolonija,
			ZgradaKolonija,
			ZgradaSustav,
			Brod
		};

		#region Tvornice
		public static Poruka NovaTehnologija(Tehnologija tehnologija, bool istrazivnje)
		{
			return new PorukaTehnologija(tehnologija, istrazivnje); //new Poruka(Tip.Tehnologija, tehnologija.tip.ime + " " + tehnologija.nivo + ". nivo", null, null);
		}

		public static Poruka NovaKolonija(Kolonija kolonija)
		{
			return new PorukaKolonija(kolonija.planet);
		}

		public static Poruka NovaZgrada(Kolonija kolonija, Zgrada.ZgradaInfo zgrada)
		{
			return new PorukaZgradaKolonija(kolonija.planet, zgrada);
		}

		public static Poruka NovaZgrada(Zvijezda zvijezda, Zgrada.ZgradaInfo zgrada)
		{
			return new PorukaZgradaSustav(zvijezda, zgrada);
		}

		public static Poruka NoviBrod(AGradiliste gradiliste, Dizajn dizajn, long kolicina)
		{
			return new PorukaBrod(gradiliste.LokacijaZvj, dizajn, kolicina);
		}
		#endregion

		public Tip tip { get; private set; }
		
		protected Poruka(Tip tip)
		{
			this.tip = tip;
		}

		public abstract string tekst { get; }

		#region Pohrana
		public const string PohranaTip = "PORUKA";
		protected const string PohTip = "TIP";

		public abstract void pohrani(PodaciPisac izlaz);

		public static Poruka Ucitaj(PodaciCitac ulaz, Dictionary<int, Zvijezda> zvijezdeID, Dictionary<int, Dizajn> dizajnID)
		{
			int tip = ulaz.podatakInt(PohTip);
			switch ((Tip)tip) {
				case Tip.Tehnologija:
					return PorukaTehnologija.Ucitaj(ulaz);
				case Tip.Kolonija:
					return PorukaKolonija.Ucitaj(ulaz, zvijezdeID);
				case Tip.Brod:
					return PorukaBrod.Ucitaj(ulaz, zvijezdeID, dizajnID);
				case Tip.ZgradaKolonija:
					return PorukaZgradaKolonija.Ucitaj(ulaz, zvijezdeID);
				default:
					throw new ArgumentException();
			}
		}
		#endregion
	}
}
