using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using Alati;
using Prototip.Podaci;
using Prototip.Podaci.Jezici;

namespace Prototip
{
	public class Tehnologija : IPohranjivoSB, IIdentifiable
	{
		public class Preduvjet
		{
			public static List<Preduvjet> NaciniPreduvjete(string podaci)
			{
				return NaciniPreduvjete(podaci, true);
			}
			public static List<Preduvjet> NaciniPreduvjete(string podaci, bool dodajLvl)
			{
				List<Preduvjet> ret = new List<Preduvjet>();
				string[] preduvjetiStr = podaci.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < preduvjetiStr.Length; i += 2)
					ret.Add(new Preduvjet(preduvjetiStr[i].Trim(), Formula.IzStringa(preduvjetiStr[i + 1]), dodajLvl));
				return ret;
			}
			public static string UString(List<Preduvjet> preduvjeti, bool ukloniLvl)
			{
				if (preduvjeti.Count == 0)
					return "";

				StringBuilder sb = new StringBuilder();
				foreach (Preduvjet p in preduvjeti)
				{
					if (ukloniLvl)
					{
						int lvlIndex = p.kod.IndexOf("_LVL");
						if (lvlIndex > 0)
						sb.Append(p.kod.Substring(0, lvlIndex));
					}
					else
						sb.Append(p.kod);
					sb.Append(" | ");
					sb.Append(p.nivo.ToString());
					sb.Append(" | ");
				}
				sb.Remove(sb.Length - 3, 3);

				return sb.ToString();
			}

			public string kod;
			public Formula nivo;

			public Preduvjet(string kod, Formula nivo, bool dodajLvl)
			{
				if (dodajLvl)
					this.kod = kod + "_LVL";
				else
					this.kod = kod;
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
			ISTRAZIVANJE = 0,
			RAZVOJ,
			NISTA
		}

		public class TechInfo : IIdentifiable
		{
			public static List<TechInfo> tehnologijeRazvoj = new List<TechInfo>();
			public static List<TechInfo> tehnologijeIstrazivanje = new List<TechInfo>();

			public static void Dodaj(Dictionary<string, string> podaci, Kategorija kategorija)
			{
				long maxNivo = long.Parse(podaci["MAX_LVL"]);
				List<Preduvjet> preduvjeti = Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
				
				foreach (Preduvjet pred in preduvjeti)
					pred.nivo.preimenujVarijablu("LVL", podaci["KOD"] + "_LVL");

				List<TechInfo> popis;
				if (kategorija == Kategorija.RAZVOJ)
					popis = tehnologijeRazvoj;
				else
					popis = tehnologijeIstrazivanje;

				TechInfo techInfo = new TechInfo(
					popis.Count,
					podaci["IME"],
					podaci["OPIS"],
					podaci["KOD"],
					Formula.IzStringa(podaci["CIJENA"]),
					maxNivo,
					preduvjeti,
					Image.FromFile(podaci["SLIKA"]),
					kategorija);
				popis.Add(techInfo);
			}

			public int id { get; private set; }
			private string naziv_m;
			public string kod;
			private string _opis;
			public Formula cijena;
			public List<Preduvjet> preduvjeti;
			public long maxNivo;
			public Image slika;
			public Kategorija kategorija { get; private set; }

			private TechInfo(int id, string ime, string opis, string kod, 
				Formula cijena, long maxNivo, List<Preduvjet> preduvjeti,
				Image slika, Kategorija kategorija)
			{
				this.id = id;
				this.naziv_m = ime;
				this._opis = opis;
				this.kod = kod;
				this.cijena = cijena;
				this.preduvjeti = preduvjeti;
				this.maxNivo = maxNivo;
				this.slika = slika;
				this.kategorija = kategorija;
				
				cijena.preimenujVarijablu("LVL", kod + "_LVL");
			}

			public string naziv
			{
				get
				{
					return Postavke.jezik[Kontekst.Tehnologije, naziv_m].tekst();
				}
			}

			public string opis(long nivo)
			{
				Dictionary<string, double> vars = new Dictionary<string, double>();
				vars.Add("LVL", nivo);
				return Postavke.jezik[Kontekst.Tehnologije, _opis].tekst();
			}
		}

		public class KnjiznicaSort : IComparer<Tehnologija>
		{
			#region IComparer<Tehnologija> Members
			public int Compare(Tehnologija x, Tehnologija y)
			{
				int kategorijaX = (int)x.tip.kategorija;
				int kategorijaY = (int)y.tip.kategorija;
				if (kategorijaX != kategorijaY)
					return kategorijaX.CompareTo(kategorijaY);
				return x.tip.naziv.CompareTo(y.tip.naziv);
			}
			#endregion
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

		private Tehnologija(TechInfo techInfo, long nivo, long ulozenoPoena)
		{
			tip = techInfo;
			this.nivo = nivo;
			this.ulozenoPoena = ulozenoPoena;
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

		public string opis
		{
			get
			{
				return tip.opis(nivo);
			}
		}

		public int id
		{
			get { return tip.id; }
		}

		#region Pohrana
		public const string PohranaTip = "MAPA";
		private const string PohKategorija = "KATEG";
		private const string PohId = "ID";
		private const string PohNivo = "NIVO";
		private const string PohUlozeno = "ULOZENO";
		public void pohrani(PodaciPisac izlaz)
		{
			bool istrazivanje = false;
			if (tip.id < TechInfo.tehnologijeIstrazivanje.Count)
				if (TechInfo.tehnologijeIstrazivanje[tip.id] == tip)
					istrazivanje = true;

			if (istrazivanje)
				izlaz.dodaj(PohKategorija, (int)Kategorija.ISTRAZIVANJE);
			else
				izlaz.dodaj(PohKategorija, (int)Kategorija.RAZVOJ);

			izlaz.dodaj(PohId, tip);
			izlaz.dodaj(PohNivo, nivo);
			izlaz.dodaj(PohUlozeno, ulozenoPoena);
		}

		public static Tehnologija Ucitaj(PodaciCitac ulaz)
		{
			Kategorija kategorija = (Kategorija)ulaz.podatakInt(PohKategorija);

			TechInfo info = null;
			int id = ulaz.podatakInt(PohId);
			if (kategorija == Kategorija.ISTRAZIVANJE)
				info = TechInfo.tehnologijeIstrazivanje[id];
			else
				info = TechInfo.tehnologijeRazvoj[id];

			long nivo = ulaz.podatakLong(PohNivo);
			long ulozeno = ulaz.podatakLong(PohUlozeno);

			return new Tehnologija(info, nivo, ulozeno);
		}
		#endregion
	}
}
