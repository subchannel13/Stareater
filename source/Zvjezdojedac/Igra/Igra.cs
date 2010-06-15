using System;
using System.Collections.Generic;
using System.Text;

namespace Prototip
{
	public class Igra
	{
		public const int maxIgraca = 4;

		private List<Igrac> igraci;
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
			foreach (Zvijezda zvj in mapa.zvijezde)
				foreach (Planet planet in zvj.planeti)
					if (planet.kolonija != null)
						planet.kolonija.noviKrug();

			foreach (Igrac igr in igraci)
				igr.noviKrug(this);

			brKruga++;
		}

		private void igraj()
		{
			//TODO
		}

		public Igrac trenutniIgrac()
		{
			return igraci[trenutniIgracIndex];
		}

		private const string PohKrug = "KRUG";
		private const string PohTrenutniIgrac = "TREN_IGRAC";
		public string spremi()
		{
			PodaciPisac podaci = new PodaciPisac("IGRA");

			podaci.dodaj(PohKrug, brKruga);
			podaci.dodaj(PohTrenutniIgrac, trenutniIgracIndex);

			podaci.dodaj(Mapa.PohranaTip, mapa);
			podaci.dodajKolekciju(Igrac.PohranaTip, igraci);
			
			HashSet<Kolonija> kolonije =  mapa.kolonije();
			podaci.dodaj(Kolonija.PohranaTip, kolonije.Count);
			podaci.dodajKolekciju(Kolonija.PohranaTip, kolonije);

			return podaci.ToString();
		}
	}
}
