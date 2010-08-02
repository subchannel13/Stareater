using System;
using System.Collections.Generic;
using System.Text;

namespace Prototip
{
	public class Igra
	{
		public const int maxIgraca = 4;

		public List<Igrac> igraci { get; private set; }
		private int trenutniIgracIndex;

		public Mapa mapa { get; private set; }
		public int brKruga;
		public Dictionary<string, Formula> osnovniEfekti;

		public Igra(List<Igrac.ZaStvoriti> igraci, Mapa.GraditeljMape mapa)
		{
			this.igraci = new List<Igrac>();
			this.mapa = mapa.mapa;
			trenutniIgracIndex = 0;
			brKruga = 0;
			osnovniEfekti = Podaci.ucitajBazuEfekata();

			foreach (Igrac.ZaStvoriti igrac in igraci)
				if (igrac.tip == Igrac.Tip.COVJEK)
					this.igraci.Add(igrac.stvoriIgraca(this.igraci.Count));

			foreach (Igrac.ZaStvoriti igrac in igraci)
				if (igrac.tip != Igrac.Tip.COVJEK)
					this.igraci.Add(igrac.stvoriIgraca(this.igraci.Count));

			Alati.Vadjenje<Planet> pocetnePozicije = new Alati.Vadjenje<Planet>();
			foreach (Planet pl in mapa.pocetnePozicije)
				pocetnePozicije.dodaj(pl);

			for (int i = 0; i < igraci.Count; i++)
			{
				this.igraci[i].izracunajEfekte(this);
				postaviIgraca(this.igraci[i], pocetnePozicije.izvadi());
				this.igraci[i].staviNoveTehnologije(this);
				this.igraci[i].izracunajPoeneIstrazivanja(this);
				this.igraci[i].staviPredefiniraneDizajnove();
			}
		}

		private Igra(List<Igrac> igraci, int trenutniIgracIndex,
			Mapa mapa, int brKruga)
		{
			this.igraci = igraci;
			this.trenutniIgracIndex = trenutniIgracIndex;
			this.mapa = mapa;
			this.brKruga = brKruga;

			osnovniEfekti = Podaci.ucitajBazuEfekata();
			for (int i = 0; i < igraci.Count; i++) {
				this.igraci[i].izracunajEfekte(this);
				this.igraci[i].staviNoveTehnologije(this);
				this.igraci[i].izracunajPoeneIstrazivanja(this);
				this.igraci[i].staviPredefiniraneDizajnove();
				foreach (Kolonija kolonija in igraci[i].kolonije)
					kolonija.resetirajEfekte();
			}
		}

		private void postaviIgraca(Igrac igrac, Planet pocetniPlanet)
		{
			igrac.odabranaZvijezda = pocetniPlanet.zvjezda;
			igrac.posjeceneZvjezde.Add(igrac.odabranaZvijezda);

			pocetniPlanet.kolonija = new Kolonija(
				igrac,
				pocetniPlanet,
				(long)osnovniEfekti["POCETNA_POPULACIJA"].iznos(null),
				(long)osnovniEfekti["POCETNA_RADNA_MJESTA"].iznos(null));
			igrac.odabranPlanet = pocetniPlanet;
			igrac.kolonije.Add(pocetniPlanet.kolonija);
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

				if (igraci[trenutniIgracIndex].tip == Igrac.Tip.RACUNALO)
					igraj();
				else
					break;
			}
		}

		private void zavrsiKrug()
		{
			List<long> poeniRazvoja = new List<long>();
			List<long> poeniIstraz = new List<long>();
			foreach (Igrac igrac in igraci) {
				poeniRazvoja.Add(igrac.poeniRazvoja());
				igrac.izracunajPoeneIstrazivanja(this);
				poeniIstraz.Add(igrac.poeniIstrazivanja());
			}

			foreach (Zvijezda zvj in mapa.zvijezde)
				foreach (Planet planet in zvj.planeti)
					if (planet.kolonija != null)
						planet.kolonija.noviKrug();

			for (int i = 0; i < igraci.Count; i++) {
				igraci[i].noviKrug(this, poeniRazvoja[i], poeniIstraz[i]);

				HashSet<Zvijezda> prazneFloteStac = new HashSet<Zvijezda>();
				foreach (KeyValuePair<Zvijezda, Flota> flotaStac in igraci[i].floteStacionarne) {
					Zvijezda zvijezda = flotaStac.Key;
					Flota flota = flotaStac.Value;
					foreach (Flota.Kolonizacija kolonizacija in flota.kolonizacije) {
						Planet planet = zvijezda.planeti[kolonizacija.planet];
						double maxDodatnaPopulacija = 0;
						if (planet.kolonija == null) {
							Kolonija kolonija = new Kolonija(igraci[i], planet, 10, 0);
							maxDodatnaPopulacija = kolonija.efekti[Kolonija.PopulacijaMax];
						}
						else
							maxDodatnaPopulacija = (planet.kolonija.efekti[Kolonija.PopulacijaMax] - planet.kolonija.populacija);
						
						long populacijaBroda = kolonizacija.brod.dizajn.populacijaKolonizatora;
						long radnaMjestaBroda = kolonizacija.brod.dizajn.radnaMjestaKolonizatora;
						long brBrodova = (long)(Math.Min(kolonizacija.brBrodova, Math.Ceiling(maxDodatnaPopulacija / populacijaBroda)));
						if (planet.kolonija == null)
							planet.kolonija = new Kolonija(
								igraci[i],
								planet,
								populacijaBroda * brBrodova,
								radnaMjestaBroda * brBrodova);
						else
							planet.kolonija.dodajKolonizator(
								populacijaBroda * brBrodova, 
								radnaMjestaBroda * brBrodova);

						flota.ukloniBrod(kolonizacija.brod.dizajn, brBrodova);
					}
					flota.kolonizacije.Clear();
					if (flota.brodovi.Count == 0)
						prazneFloteStac.Add(zvijezda);
				}
				foreach (Zvijezda zvj in prazneFloteStac)
					igraci[i].floteStacionarne.Remove(zvj);
			}

			foreach (Zvijezda zvj in mapa.zvijezde)
				foreach (Planet planet in zvj.planeti)
					if (planet.kolonija != null)
						planet.kolonija.resetirajEfekte();
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

		private void igraj()
		{
			//TODO
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
			
			HashSet<Kolonija> kolonije =  mapa.kolonije();
			podaci.dodaj(Kolonija.PohranaTip, kolonije.Count);
			podaci.dodajKolekciju(Kolonija.PohranaTip, kolonije);

			return podaci.ToString();
		}

		public static Igra Ucitaj(string ulaz)
		{
			PodaciCitac citac = PodaciCitac.Procitaj(ulaz);

			int brKruga = citac.podatakInt(PohKrug);
			int trenutniIgrac = citac.podatakInt(PohTrenutniIgrac);
			int brIgraca = citac.podatakInt(PohBrIgraca);

			Mapa mapa = Mapa.Ucitaj(citac[Mapa.PohranaTip]);
			List<Igrac> igraci = new List<Igrac>();
			for (int i = 0; i < brIgraca; i++)
				igraci.Add(Igrac.Ucitaj(citac[Igrac.PohranaTip + i], mapa));

			Dictionary<int, Zvijezda> zvijezdeID = new Dictionary<int,Zvijezda>();
			foreach(Zvijezda zvj in mapa.zvijezde)
				zvijezdeID.Add(zvj.id, zvj);

			Dictionary<int, Zgrada.ZgradaInfo> zgradeInfoID = new Dictionary<int, Zgrada.ZgradaInfo>(Zgrada.ZgradaInfoID);
			foreach (Igrac igrac in igraci)
				foreach (DizajnZgrada dizajZgrada in igrac.dizajnoviBrodova)
					zgradeInfoID.Add(dizajZgrada.id, dizajZgrada);

			int brKolonija = citac.podatakInt(Kolonija.PohranaTip);
			for (int i = 0; i < brKolonija; i++) {
				Kolonija kolonija = Kolonija.Ucitaj(
					citac[Kolonija.PohranaTip + i],
					igraci,
					zvijezdeID,
					zgradeInfoID);
				kolonija.planet.kolonija = kolonija;
			}

			return new Igra(igraci, trenutniIgrac, mapa, brKruga);
		}
		#endregion
	}
}
