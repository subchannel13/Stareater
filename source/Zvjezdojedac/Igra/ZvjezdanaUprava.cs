using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Igra.Poruke;

namespace Zvjezdojedac.Igra
{
	public class ZvjezdanaUprava : AGradiliste, IPohranjivoSB
	{
		#region Statično i konstante

		const string Razvoj = "RAZVOJ";
		const string Gradnja = "GRADNJA";

		private static string[] KljuceviEfekata = new string[]
		{
			Kolonija.PopulacijaVisak,
			Kolonija.MigracijaMax,
			Razvoj,
			Gradnja
		};
		#endregion

		private Zvijezda zvijezda;
		private double udioGradnje = 0;

		public ZvjezdanaUprava(Zvijezda zvijezda, Igrac igrac)
			: base (igrac)
		{
			this.zvijezda = zvijezda;
		
			foreach (string kljuc in KljuceviEfekata)
				Efekti.Add(kljuc, 0);
		}

		public ZvjezdanaUprava(Zvijezda zvijezda, Igrac igrac, Dictionary<string, double> ostatakGradnje,
			double udioGradnje, LinkedList<Zgrada.ZgradaInfo> redGradnje, IEnumerable<Zgrada> zgrade)
			: base (igrac, redGradnje, ostatakGradnje)
		{
			this.zvijezda = zvijezda;
			this.udioGradnje = udioGradnje;

			foreach (Zgrada zgrada in zgrade)
				this.Zgrade.Add(zgrada.tip, zgrada);

			foreach (string kljuc in KljuceviEfekata)
				Efekti.Add(kljuc, 0);
		}

		private void gradi()
		{
			double poeniGradnje = Efekti[Gradnja];
			LinkedListNode<Zgrada.ZgradaInfo> uGradnji = RedGradnje.First;

			while (uGradnji != null && poeniGradnje > 0) {
				Zgrada.ZgradaInfo zgradaTip = uGradnji.Value;
				double cijena = zgradaTip.CijenaGradnje.iznos(Igrac.efekti);
				poeniGradnje += ostatakGradnje[zgradaTip.grupa];

				long brZgrada = (long)((poeniGradnje) / cijena);
				long dopustenaKolicina = (long)Math.Min(
					zgradaTip.DopustenaKolicina.iznos(Igrac.efekti),
					zgradaTip.DopustenaKolicinaPoKrugu.iznos(Igrac.efekti));

				if (brZgrada < dopustenaKolicina) {
					ostatakGradnje[zgradaTip.grupa] = poeniGradnje - brZgrada * cijena;
					poeniGradnje = 0;
				}
				else {
					brZgrada = dopustenaKolicina;
					poeniGradnje = poeniGradnje - (brZgrada * cijena - ostatakGradnje[zgradaTip.grupa]);
					ostatakGradnje[zgradaTip.grupa] = 0;
				}

				if (brZgrada > 0) {
					Zgrada z = new Zgrada(zgradaTip, brZgrada);

					if (z.tip.ostaje) {
						if (Zgrade.ContainsKey(z.tip))
							Zgrade[z.tip].kolicina += brZgrada;
						else
							Zgrade.Add(z.tip, z);
					}
					else
						z.djeluj(this, Igrac.efekti);

					if (!z.tip.brod && !z.tip.ponavljaSe)
						Igrac.poruke.AddLast(Poruka.NovaZgrada(this.zvijezda, z.tip));
				}

				long brNovih = brZgrada;
				if (Zgrade.ContainsKey(zgradaTip))
					brZgrada = Zgrade[zgradaTip].kolicina;
				else
					brZgrada = 0;

				if (brNovih < dopustenaKolicina)
					break;

				uGradnji = uGradnji.Next;
			}
		}

		public void postaviEfekteIgracu()
		{
			foreach (string s in Efekti.Keys)
				Igrac.efekti[s] = Efekti[s];
		}

		public void ResetirajEfekte()
		{
			foreach (string kljuc in KljuceviEfekata)
				Efekti[kljuc] = 0;
		}

		public void IzracunajEfekte()
		{
			ResetirajEfekte();
			
			foreach (Planet pl in zvijezda.planeti)
				if (pl.kolonija != null && pl.kolonija.Igrac == Igrac) {
					Dictionary<string, double> efektiPl = pl.kolonija.Efekti;

					Efekti[Kolonija.PopulacijaVisak] += efektiPl[Kolonija.PopulacijaVisak];
					Efekti[Kolonija.MigracijaMax] += efektiPl[Kolonija.MigracijaMax];

					Efekti[Gradnja] += pl.kolonija.NeiskoristenaPopulacija * efektiPl[Kolonija.IndPoRadnikuEfektivno] * UdioGradnje / efektiPl[Kolonija.FaktorCijeneOrbitalnih];
					Efekti[Razvoj] += pl.kolonija.NeiskoristenaPopulacija * efektiPl[Kolonija.RazPoRadnikuEfektivno] * (1 - UdioGradnje);
				}

			Efekti[Kolonija.PopulacijaVisak] = Math.Min(Efekti[Kolonija.PopulacijaVisak], Efekti[Kolonija.MigracijaMax]);
		}

		public void NoviKrugPriprema()
		{
			ResetirajEfekte();
			postaviEfekteIgracu();

			foreach (Zgrada zgrada in Zgrade.Values)
				zgrada.djeluj(this, Igrac.efekti);
		}

		public void NoviKrug()
		{
			IzracunajEfekte();
			gradi();

			List<Kolonija> kolonije = new List<Kolonija>();
			foreach (Planet pl in zvijezda.planeti)
				if (pl.kolonija != null && pl.kolonija.Igrac == Igrac)
					kolonije.Add(pl.kolonija);

			kolonije.Sort((k1, k2) => k1.OdrzavanjePoStan.CompareTo(k2.OdrzavanjePoStan));

			foreach (Kolonija kolonija in kolonije) {
				Dictionary<string, double> efektiPl = kolonija.Efekti;

				double imigranti = kolonija.dodajMigrante(Efekti[Kolonija.PopulacijaVisak]);
				Efekti[Kolonija.PopulacijaVisak] -= imigranti;

				kolonija.resetirajEfekte();
			}

			//IzracunajEfekte();
		}

		public override List<Zgrada.ZgradaInfo> MoguceGraditi()
		{
			//HashSet<Zgrada.ZgradaInfo> uRedu = new HashSet<Zgrada.ZgradaInfo>(RedGradnje);
			List<Zgrada.ZgradaInfo> popis = new List<Zgrada.ZgradaInfo>(Zgrada.VojneZgradeInfo);
			foreach (Zgrada.ZgradaInfo zi in Igrac.dizajnoviBrodova)
				popis.Add(zi);
			
			List<Zgrada.ZgradaInfo> ret = new List<Zgrada.ZgradaInfo>();
			foreach (Zgrada.ZgradaInfo z in popis)
			//if (!uRedu.Contains(z)) 
			{
				long prisutnaKolicina = 0;
				if (Zgrade.ContainsKey(z))
					prisutnaKolicina = Zgrade[z].kolicina;
				if (z.dostupna(Igrac.efekti, prisutnaKolicina))
					ret.Add(z);
			}

			return ret;
		}

		public string ProcjenaVremenaGradnje()
		{
			if (RedGradnje.First != null) {
				Zgrada.ZgradaInfo zgrada = RedGradnje.First.Value;
				return Zgrada.ProcjenaVremenaGradnje(
					PoeniIndustrije,
					ostatakGradnje[zgrada.grupa],
					zgrada, Igrac);
			}
			else
				return "";
		}

		public override Zvijezda LokacijaZvj 
		{
			get { return zvijezda; }
		}

		public double PoeniIndustrije
		{
			get { return Efekti[Gradnja]; }
		}

		public double PoeniRazvoja
		{
			get { return Efekti[Razvoj]; }
		}

		public double UdioGradnje
		{
			get { return udioGradnje; }
			set
			{
				udioGradnje = value;
				IzracunajEfekte();
			}
		}

		public double this[string svojstvo]
		{
			get { return Efekti[svojstvo]; }
		}

		#region Pohrana
		public const string PohranaTip = "ZVJ_UP";
		private const string PohIgrac = "IGRAC";
		private const string PohZvijezda = "ZVJ";
		private const string PohGradUdio = "UDIO_GRAD";
		private const string PohGradOst = "GRAD_OST";
		private const string PohGrad = "GRADNJA";
		private const string PohZgrada = "ZGRADA";

		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohIgrac, Igrac.id);
			izlaz.dodaj(PohZvijezda, zvijezda.id);
			izlaz.dodaj(PohGradUdio, udioGradnje);
			izlaz.dodajRjecnik(PohGradOst, ostatakGradnje, x => (x > 0));

			izlaz.dodaj(PohZgrada, Zgrade.Count);
			izlaz.dodajKolekciju(PohZgrada, Zgrade.Values);

			izlaz.dodajIdeve(PohGrad, RedGradnje);
		}

		public static ZvjezdanaUprava Ucitaj(PodaciCitac ulaz, List<Igrac> igraci,
			Dictionary<int, Zvijezda> zvijezde,
			Dictionary<int, Zgrada.ZgradaInfo> zgradeInfoID)
		{
			Igrac igrac = igraci[ulaz.podatakInt(PohIgrac)];
			Zvijezda zvijezda = zvijezde[ulaz.podatakInt(PohZvijezda)];
			double udioInd = ulaz.podatakDouble(PohGradUdio);
			Dictionary<string, double> ostatakGradnje = ulaz.podatakDoubleRjecnik(PohGradOst);

			int brZgrada = ulaz.podatakInt(PohZgrada);
			List<Zgrada> zgrade = new List<Zgrada>();
			for (int i = 0; i < brZgrada; i++)
				zgrade.Add(Zgrada.Ucitaj(ulaz[PohZgrada + i]));

			int[] zgradeID = ulaz.podatakIntPolje(PohGrad);
			LinkedList<Zgrada.ZgradaInfo> redCivilneGradnje = new LinkedList<Zgrada.ZgradaInfo>();
			for (int i = 0; i < zgradeID.Length; i++)
				redCivilneGradnje.AddLast(zgradeInfoID[zgradeID[i]]);

			return new ZvjezdanaUprava(zvijezda, igrac, ostatakGradnje, udioInd, 
				redCivilneGradnje, zgrade);
		}
		#endregion
	}
}
