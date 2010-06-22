using System;
using System.Collections.Generic;
using System.Text;

namespace Prototip
{
	public class DizajnZgrada : Zgrada.ZgradaInfo
	{
		public class UcinakSagradiBrod : Zgrada.Ucinak
		{
			private Dizajn dizajn;

			public UcinakSagradiBrod(Dizajn dizajn)
			{
				this.dizajn = dizajn;
			}

			public override void djeluj(Kolonija kolonija, Dictionary<string, double> varijable)
			{
				kolonija.igrac.dodajBrod(dizajn, (int)varijable[Zgrada.BrojZgrada], kolonija.planet.zvjezda);
			}

			public override void noviKrug(Kolonija kolonija, Dictionary<string, double> varijable)
			{ }
		}

		public Dizajn dizajn { get; private set; }

		public DizajnZgrada(Dizajn dizajn)
			: base(Zgrada.ZadnjiId() + dizajn.id, dizajn.ime, 
			new KonstantnaFormula(dizajn.cijena),
			new KonstantnaFormula(int.MaxValue),
			new KonstantnaFormula(0),
			dizajn.trup.info.slika,
			"",
			dizajn.trup.info.opis,
			new List<Zgrada.Ucinak>(),
			"PONAVLJA_SE NE_OSTAJE ORBITALNA",
			new List<Tehnologija.Preduvjet>())
		{
			ucinci.Add(new UcinakSagradiBrod(dizajn));
			this.dizajn = dizajn;
		}
	}
}
