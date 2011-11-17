using System;
using System.Collections.Generic;
using System.Text;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Podaci.Formule;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Igra.Brodovi;
using Zvjezdojedac.Igra.Igraci.OsnovniRI;

namespace Zvjezdojedac.Igra
{
	public class IgraZvj
	{
		public const int MaxIgraca = 4;

		public List<Igrac> igraci { get; private set; }
		private int trenutniIgracIndex;

		public Mapa mapa { get; private set; }
		public int brKruga;
		public Dictionary<string, Formula> osnovniEfekti;

		public IgraZvj(List<Igrac.ZaStvoriti> igraci, Mapa.GraditeljMape mapa, PocetnaPopulacija pocetnaPop)
		{
			this.igraci = new List<Igrac>();
			this.mapa = mapa.mapa;
			trenutniIgracIndex = 0;
			brKruga = 0;
			osnovniEfekti = PodaciAlat.ucitajBazuEfekata();

			foreach (Igrac.ZaStvoriti igrac in igraci)
				if (igrac.tip == Igrac.Tip.COVJEK)
					this.igraci.Add(igrac.stvoriIgraca(this.igraci.Count));

			foreach (Igrac.ZaStvoriti igrac in igraci)
				if (igrac.tip == Igrac.Tip.RACUNALO)
					this.igraci.Add(igrac.stvoriRacunalnogIgraca(this.igraci.Count));

			Vadjenje<Zvijezda> pocetnePozicije = new Vadjenje<Zvijezda>();
			foreach (Zvijezda pl in mapa.pocetnePozicije)
				pocetnePozicije.dodaj(pl);

			for (int i = 0; i < igraci.Count; i++)
			{
				this.igraci[i].izracunajEfekte(this);
				postaviIgraca(this.igraci[i], pocetnePozicije.izvadi(), pocetnaPop);
				this.igraci[i].staviNoveTehnologije(this);
				this.igraci[i].izracunajPoeneIstrazivanja(this);
				this.igraci[i].staviPredefiniraneDizajnove();
			}

			foreach (Zvijezda zvj in this.mapa.zvijezde) {
				foreach (Planet pl in zvj.planeti)
					if (pl.kolonija != null)
						pl.kolonija.resetirajEfekte();
				zvj.IzracunajEfekte();

				//	Za potrebe debugiranja
				  
				/* for (int i = 0; i < igraci.Count; i++)
					this.igraci[i].posjeceneZvjezde.Add(zvj); */
			}
		}

		private IgraZvj(List<Igrac> igraci, int trenutniIgracIndex,
			Mapa mapa, int brKruga)
		{
			this.igraci = igraci;
			this.trenutniIgracIndex = trenutniIgracIndex;
			this.mapa = mapa;
			this.brKruga = brKruga;

			osnovniEfekti = PodaciAlat.ucitajBazuEfekata();
			for (int i = 0; i < igraci.Count; i++) {
				this.igraci[i].izracunajEfekte(this);
				this.igraci[i].staviNoveTehnologije(this);
				this.igraci[i].izracunajPoeneIstrazivanja(this);
				this.igraci[i].staviPredefiniraneDizajnove();
				foreach (Kolonija kolonija in igraci[i].kolonije)
					kolonija.resetirajEfekte();
			}

			foreach (Zvijezda zvj in mapa.zvijezde)
				zvj.IzracunajEfekte();
		}

		private void postaviIgraca(Igrac igrac, Zvijezda pocetnaZvj, PocetnaPopulacija pocetnaPop)
		{
			igrac.odabranaZvijezda = pocetnaZvj;
			igrac.posjeceneZvjezde.Add(pocetnaZvj);
			pocetnaZvj.Naseli(igrac);

			List<PotencijalnaPocetnaKolonija> potencijalneKolonije = new List<PotencijalnaPocetnaKolonija>();
			foreach (Planet pl in pocetnaZvj.planeti)
				if (pl.tip != Planet.Tip.NIKAKAV) {
					Dictionary<string, double> efekti = new Kolonija(igrac, pl, 1000, 1000).maxEfekti();
					potencijalneKolonije.Add(new PotencijalnaPocetnaKolonija(
						pl,
						efekti[Kolonija.BrRadnika] / efekti[Kolonija.PopulacijaBr],
						efekti[Kolonija.PopulacijaMax],
						efekti[Kolonija.RudePoRudaru]));
				}

			potencijalneKolonije.Sort(
				(k1, k2) =>
					(k1.prikladnost != k2.prikladnost) ?
					-(k1.prikladnost).CompareTo(k2.prikladnost) :
					-(k1.rudePoRudaru).CompareTo(k2.rudePoRudaru));

			if (potencijalneKolonije.Count > pocetnaPop.BrKolonija)
				potencijalneKolonije.RemoveRange(pocetnaPop.BrKolonija, potencijalneKolonije.Count - pocetnaPop.BrKolonija);

			double[] dodjeljenaPop = new double[potencijalneKolonije.Count];
			double preostalaPop = pocetnaPop.Populacija;
			double ukupnaPop = pocetnaPop.Populacija;
			double sumaDobrota = 0;

			for (int i = 0; i < dodjeljenaPop.Length; i++)
				sumaDobrota += potencijalneKolonije[i].prikladnost;

			for (int i = 0; i < dodjeljenaPop.Length; i++) {
				double dodjela = ukupnaPop * potencijalneKolonije[i].prikladnost / sumaDobrota;
				dodjela = Math.Floor(Math.Min(dodjela, potencijalneKolonije[i].populacijaMax));
				preostalaPop -= dodjela;
				dodjeljenaPop[i] = dodjela;
			}

			int najboljiPlanet = 0;
			for (int i = 0; i < dodjeljenaPop.Length; i++) {
				double dodjela = Math.Floor(
					Math.Min(
						preostalaPop, 
						potencijalneKolonije[i].populacijaMax - dodjeljenaPop[i])
					);

				preostalaPop -= dodjela;
				dodjeljenaPop[i] += dodjela;

				if (dodjeljenaPop[i] > dodjeljenaPop[najboljiPlanet]) najboljiPlanet = i;
			}

			for (int i = 0; i < dodjeljenaPop.Length; i++) {
				Planet pl = potencijalneKolonije[i].planet;

				pl.kolonija = new Kolonija(
					igrac,
					pl,
					(long)dodjeljenaPop[i],
					(long)Math.Floor(dodjeljenaPop[i] * pocetnaPop.UdioRadnihMjesta));
				
				igrac.kolonije.Add(pl.kolonija);
			}

			igrac.OdabranPlanet = potencijalneKolonije[najboljiPlanet].planet;
		}

		public void slijedeciIgrac()
		{
			while(true)
			{
				trenutniIgracIndex++;
				if (trenutniIgracIndex >= igraci.Count)
				{
					zavrsiKrug();
					trenutniIgracIndex = 0;
				}

				if (igraci[trenutniIgracIndex].tip != Igrac.Tip.COVJEK)
					igraci[trenutniIgracIndex].Upravljac.OdigrajKrug(this);
				else
					break;
			}
		}

		private void zavrsiKrug()
		{
			long[] poeniRazvoja = new long[igraci.Count];
			long[] poeniIstraz = new long[igraci.Count];
			for (int i = 0; i < igraci.Count; i++) {
				igraci[i].poruke.Clear();
				igraci[i].izracunajPoeneIstrazivanja(this);

				poeniRazvoja[i] = igraci[i].poeniRazvoja();
				poeniIstraz[i] = igraci[i].poeniIstrazivanja();
			}

			foreach (Zvijezda zvj in mapa.zvijezde) {
				zvj.NoviKrugPriprema();
				foreach (Planet planet in zvj.planeti)
					if (planet.kolonija != null)
						planet.kolonija.NoviKrugPrviProlaz();

				zvj.NoviKrug();
			}

			foreach (Igrac igrac in igraci) {
				igrac.IzvrsiKolonizacije();
				igrac.PomakniFlote();
			}

			for (int i = 0; i < igraci.Count; i++)
				igraci[i].NoviKrug(this, poeniRazvoja[i], poeniIstraz[i]);

			foreach (Zvijezda zvj in mapa.zvijezde)
				foreach (Planet planet in zvj.planeti)
					if (planet.kolonija != null)
						planet.kolonija.NoviKrugDrugiProlaz();
			brKruga++;
		}

		private Dictionary<Zvijezda, List<Planet>> mozeKolonizirati(Igrac igrac)
		{
			Dictionary<Zvijezda, List<Planet>> rez = new Dictionary<Zvijezda, List<Planet>>();

			foreach (KeyValuePair<Zvijezda, Flota> par in igrac.floteStacionarne) {
				if (!par.Value.imaMisiju(Misija.Tip.Kolonizacija))
					continue;

				List<Planet> naseljiviPlaneti = new List<Planet>();
				foreach (Planet planet in par.Key.planeti)
					if (planet.kolonija == null)
						naseljiviPlaneti.Add(planet);

				if (naseljiviPlaneti.Count > 0)
					rez.Add(par.Key, naseljiviPlaneti);
			}

			return rez;
		}

		public Igrac trenutniIgrac()
		{
			return igraci[trenutniIgracIndex];
		}

		#region Pohrana
		private const string PohKrug = "KRUG";
		private const string PohTrenutniIgrac = "TREN_IGRAC";
		private const string PohBrIgraca = "BR_IGRACA";
		public string spremi()
		{
			PodaciPisac podaci = new PodaciPisac("IGRA");

			podaci.dodaj(PohKrug, brKruga);
			podaci.dodaj(PohTrenutniIgrac, trenutniIgracIndex);
			podaci.dodaj(PohBrIgraca, igraci.Count);

			podaci.dodaj(Mapa.PohranaTip, mapa);
			podaci.dodajKolekciju(Igrac.PohranaTip, igraci);

			ICollection<Kolonija> kolonije = mapa.Kolonije();
			podaci.dodaj(Kolonija.PohranaTip, kolonije.Count);
			podaci.dodajKolekciju(Kolonija.PohranaTip, kolonije);

			ICollection<ZvjezdanaUprava> uprave = mapa.ZvjezdaneUprave();
			podaci.dodaj(ZvjezdanaUprava.PohranaTip, uprave.Count);
			podaci.dodajKolekciju(ZvjezdanaUprava.PohranaTip, uprave);

			return podaci.ToString();
		}

		public static IgraZvj Ucitaj(string ulaz)
		{
			PodaciCitac citac = PodaciCitac.Procitaj(ulaz);

			int brKruga = citac.podatakInt(PohKrug);
			int trenutniIgrac = citac.podatakInt(PohTrenutniIgrac);
			int brIgraca = citac.podatakInt(PohBrIgraca);

			Mapa mapa = Mapa.Ucitaj(citac[Mapa.PohranaTip]);
			List<Igrac> igraci = new List<Igrac>();
			for (int i = 0; i < brIgraca; i++) {
				Igrac igrac = Igrac.Ucitaj(citac[Igrac.PohranaTip + i], mapa);
				
				if(igrac.tip == Igrac.Tip.RACUNALO)
					igrac.Upravljac = new ORIKoordinator(igrac);

				igraci.Add(igrac);
			}

			Dictionary<int, Zvijezda> zvijezdeID = new Dictionary<int,Zvijezda>();
			foreach(Zvijezda zvj in mapa.zvijezde)
				zvijezdeID.Add(zvj.id, zvj);

			Dictionary<int, Zgrada.ZgradaInfo> zgradeInfoID = new Dictionary<int, Zgrada.ZgradaInfo>(Zgrada.ZgradaInfoID);
			foreach (Igrac igrac in igraci)
				foreach (DizajnZgrada dizajZgrada in igrac.dizajnoviBrodova) {
					zgradeInfoID.Add(dizajZgrada.id, dizajZgrada);
					Dizajn.ProvjeriId(dizajZgrada.dizajn);
				}

			int brKolonija = citac.podatakInt(Kolonija.PohranaTip);
			for (int i = 0; i < brKolonija; i++) {
				Kolonija kolonija = Kolonija.Ucitaj(
					citac[Kolonija.PohranaTip + i],
					igraci,
					zvijezdeID,
					zgradeInfoID);
				kolonija.planet.kolonija = kolonija;
			}

			int brUprava = citac.podatakInt(ZvjezdanaUprava.PohranaTip);
			for (int i = 0; i < brUprava; i++) {
				ZvjezdanaUprava zvjUprava = ZvjezdanaUprava.Ucitaj(
					citac[ZvjezdanaUprava.PohranaTip + i],
					igraci,
					zvijezdeID,
					zgradeInfoID);
				zvjUprava.LokacijaZvj.uprave[zvjUprava.Igrac.id] = zvjUprava;
			}

			return new IgraZvj(igraci, trenutniIgrac, mapa, brKruga);
		}
		#endregion
	}
}
