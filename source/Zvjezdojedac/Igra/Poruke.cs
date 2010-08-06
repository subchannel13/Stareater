using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alati;

namespace Prototip
{
	public class Poruka : IPohranjivoSB
	{
		public enum Tip
		{
			Prica = 0,
			Tehnologija,
			Kolonija,
			Zgrada,
			Brod
		};

		public static Poruka NovaTehnologija(Tehnologija tehnologija)
		{
			return new Poruka(Tip.Tehnologija, tehnologija.tip.ime + " " + tehnologija.nivo + ". nivo", null, null);
		}

		public static Poruka NovaKolonija(Kolonija kolonija)
		{
			return new Poruka(Tip.Kolonija, "Nova kolonija utemeljena na " + kolonija.planet.ime, kolonija.planet.zvjezda, kolonija.planet);
		}

		public static Poruka NovaZgrada(Kolonija kolonija, Zgrada.ZgradaInfo zgrada)
		{
			return new Poruka(Tip.Zgrada, "Na " + kolonija.planet.ime + " izgrađen " + zgrada.ime, kolonija.planet.zvjezda, kolonija.planet);
		}

		public static Poruka NoviBrod(Kolonija kolonija, Dizajn dizajn, long kolicina)
		{
			return new Poruka(Tip.Brod, 
				String.Format("Izgrađeno {0} x {1} kod {2}", Fje.PrefiksFormater(kolicina), dizajn.ime, kolonija.planet.zvjezda.ime), 
				kolonija.planet.zvjezda, 
				kolonija.planet);
		}

		public Tip tip { get; private set; }
		public string tekst { get; private set; }
		public Zvijezda izvorZvijezda { get; private set; }
		public Planet izvorPlanet { get; private set; }
		//public bool procitana;

		private Poruka(Tip tip, string tekst, Zvijezda izvorZvijezda, Planet izvorPlanet)
		{
			this.tip = tip;
			this.tekst = tekst;
			this.izvorPlanet = izvorPlanet;
			this.izvorZvijezda = izvorZvijezda;
			//this.procitana = false;
		}

		#region Pohrana
		public const string PohranaTip = "PORUKA";
		private const string PohTip = "TIP";
		private const string PohTekst = "TEKST";
		private const string PohIzvor = "IZVOR";
		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohTip, (int)tip);
			izlaz.dodaj(PohTekst, tekst);
			
			string izvor = "";
			if (izvorZvijezda != null) izvor += izvorZvijezda.id;
			if (izvorPlanet != null) izvor += " " + izvorPlanet.pozicija;
			izlaz.dodaj(PohIzvor, izvor);
		}

		public static Poruka Ucitaj(PodaciCitac ulaz, Dictionary<int, Zvijezda> zvijezdeID)
		{
			int tip = ulaz.podatakInt(PohTip);
			string tekst = ulaz.podatak(PohTekst);
			
			string[] izvorString = ulaz.podatak(PohIzvor).Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			Zvijezda izvorZvijezda = null;
			Planet izvorPlanet = null;
			if (izvorString.Length >= 1) {
				int zvjId = int.Parse(izvorString[0]);
				izvorZvijezda = zvijezdeID[zvjId];
			}
			if (izvorString.Length >= 2) {
				int planetId = int.Parse(izvorString[1]);
				izvorPlanet = izvorZvijezda.planeti[planetId];
			}

			return new Poruka((Tip)tip, tekst, izvorZvijezda, izvorPlanet);
		}
		#endregion
	}
}
