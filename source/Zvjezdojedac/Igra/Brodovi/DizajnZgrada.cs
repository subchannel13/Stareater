using System;
using System.Collections.Generic;
using System.Text;
using Zvjezdojedac.Podaci.Formule;
using Zvjezdojedac.Igra.Poruke;

namespace Zvjezdojedac.Igra.Brodovi
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
				long kolicina = (long)varijable[Zgrada.BrojZgrada];
				kolonija.igrac.dodajBrod(dizajn, kolicina, kolonija.planet.zvjezda);
				kolonija.igrac.poruke.AddLast(Poruka.NoviBrod(kolonija, dizajn, kolicina));
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
			"PONAVLJA_SE NE_OSTAJE ORBITALNA BROD",
			new List<Tehnologija.Preduvjet>())
		{
			ucinci.Add(new UcinakSagradiBrod(dizajn));
			this.dizajn = dizajn;
		}

		public override string ime
		{
			get { return _ime; }
		}

		public override string opis
		{
			get { return _opis; }
		}
	}
}
