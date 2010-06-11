using System;
using System.Collections.Generic;
using System.Text;

namespace Prototip
{
	public class Igra
	{
		public const int maxIgraca = 4;

		private List<Igrac> igraci;

		private Mapa _mapa;

		public Mapa mapa
		{
			get { return _mapa;  }
		}

		private int trenutniIgracIndex;

		public Dictionary<string, Formula> osnovniEfekti;

		public int brKruga;

		public Igra(List<Igrac.ZaStvoriti> igraci, Mapa.GraditeljMape mapa)
		{
			this.igraci = new List<Igrac>();
			this._mapa = mapa.mapa;
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

		public string spremi()
		{
			PodaciPisac podaci = new PodaciPisac("IGRA");

			podaci.dodaj("MAPA", mapa);

			return podaci.ToString();
		}
	}
}
