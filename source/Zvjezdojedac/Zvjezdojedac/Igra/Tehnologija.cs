using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using Alati;

namespace Prototip
{
	public class Tehnologija
	{
		public class Preduvjet
		{
			public static List<Preduvjet> NaciniPreduvjete(string podaci)
			{
				List<Preduvjet> ret = new List<Preduvjet>();
				string[] preduvjetiStr = podaci.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < preduvjetiStr.Length; i += 2)
					ret.Add(new Preduvjet(preduvjetiStr[i].Trim(), Formula.IzStringa(preduvjetiStr[i + 1])));
				return ret;
			}

			public string kod;
			public Formula nivo;

			public Preduvjet(string kod, Formula nivo)
			{
				this.kod = kod + "_LVL";
				this.nivo = nivo;
			}

			public bool zadovoljen(Dictionary<string, double> varijable)
			{
				if (Math.Round(varijable[kod]) >= nivo.iznos(varijable))
					return true;
				else
					return false;
			}
		}

		public enum Kategorija
		{
			ISTRAZIVANJE,
			RAZVOJ
		}

		public class TechInfo
		{
			public static List<TechInfo> tehnologijeRazvoj = new List<TechInfo>();
			public static List<TechInfo> tehnologijeIstrazivanje = new List<TechInfo>();

			public static void dodajTehnologiju(Dictionary<string, string> podaci, Kategorija kategorija)
			{
				long maxNivo = long.Parse(podaci["MAX_LVL"]);
				List<Preduvjet> preduvjeti = Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
				
				foreach (Preduvjet pred in preduvjeti)
					pred.nivo.preimenujVarijablu("LVL", podaci["KOD"] + "_LVL");

				TechInfo techInfo = new TechInfo(
						podaci["IME"],
						podaci["OPIS"],
						podaci["KOD"],
						Formula.IzStringa(podaci["CIJENA"]),
						maxNivo,
						preduvjeti,
						Image.FromFile(podaci["SLIKA"]),
						podaci["SLIKA"]);

				if (kategorija == Kategorija.RAZVOJ)
					tehnologijeRazvoj.Add(techInfo);
				else
					tehnologijeIstrazivanje.Add(techInfo);
			}

			public string ime;
			public string kod;
			public string opis;
			public Formula cijena;
			public List<Preduvjet> preduvjeti;
			public long maxNivo;
			public Image slika;
			public string slikaPutanja;

			private TechInfo(string ime, string opis, string kod, Formula cijena, long maxNivo, List<Preduvjet> preduvjeti, Image slika, string slikaPutanja)
			{
				this.ime = ime;
				this.opis = opis;
				this.kod = kod;
				this.cijena = cijena;
				this.preduvjeti = preduvjeti;
				this.maxNivo = maxNivo;
				this.slika = slika;
				this.slikaPutanja = slikaPutanja;
				
				cijena.preimenujVarijablu("LVL", kod + "_LVL");
			}
		}

		public static List<long> RasporedPoena(long ukupnoPoena, int brTehnologija, double koncentracija)
		{
			List<long> ret = new List<long>();
			double suma = Fje.IntegralPolinoma(1, koncentracija);
			long ostaliPoeni = ukupnoPoena;
			
			for (int i = 0; i < brTehnologija; i++)
			{
				long ulog;
				if (i == brTehnologija - 1)
					ulog = ostaliPoeni;
				else
				{
					double x0 = (brTehnologija - i - 1) / (double)brTehnologija;
					double x1 = (brTehnologija - i) / (double)brTehnologija;
					ulog = (long)(ukupnoPoena * (Fje.IntegralPolinoma(x1, koncentracija) - Fje.IntegralPolinoma(x0, koncentracija)) / suma);
				}
				ostaliPoeni -= ulog;
				ret.Add(ulog);
			}
			
			return ret;
		}

		public TechInfo tip;
		public long nivo;
		public long ulozenoPoena;

		public Tehnologija(TechInfo techInfo)
		{
			tip = techInfo;
			nivo = 0;
			ulozenoPoena = 0;
		}

		public bool istrazivo(Dictionary<string, double> varijable)
		{
			if (nivo >= tip.maxNivo) return false;
			foreach (Preduvjet p in tip.preduvjeti)
				if (!p.zadovoljen(varijable))
					return false;
			return true;
		}

		public long cijena(Dictionary<string, double> varijable)
		{
			return (long)tip.cijena.iznos(varijable);
		}

		public long uloziPoene(long ulog, Dictionary<string, double> varijable)
		{
			while (ulog > 0)
			{
				long cijena = this.cijena(varijable);
				if (cijena > ulog + ulozenoPoena)
				{
					ulozenoPoena += ulog;
					ulog = 0;
				}
				else
				{
					ulog -= cijena - ulozenoPoena;
					nivo++; varijable[tip.kod + "_LVL"] = nivo;
					ulozenoPoena = 0;
					if (!istrazivo(varijable)) break;
				}
			}
			
			return ulog;
		}
	}
}
