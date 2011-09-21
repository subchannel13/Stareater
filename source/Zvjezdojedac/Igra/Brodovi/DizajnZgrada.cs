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

			public override void djeluj(IGradiliste gradiliste, Dictionary<string, double> varijable)
			{
				long kolicina = (long)varijable[Zgrada.BrojZgrada];
				gradiliste.Igrac.dodajBrod(dizajn, kolicina, gradiliste.LokacijaZvj);
				gradiliste.Igrac.poruke.AddLast(Poruka.NoviBrod(gradiliste, dizajn, kolicina));
			}

			public override void noviKrug(IGradiliste gradiliste, Dictionary<string, double> varijable)
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
