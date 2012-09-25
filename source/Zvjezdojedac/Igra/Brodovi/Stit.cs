using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Zvjezdojedac.Podaci.Formule;

namespace Zvjezdojedac.Igra.Brodovi
{
	public class Stit : Komponenta<Stit.StitInfo>
	{
		public class StitInfo : AKomponentaInfo
		{
			#region Statično
			public static List<StitInfo> Stitovi = new List<StitInfo>();
			public static Dictionary<string, StitInfo> Kodovi = new Dictionary<string, StitInfo>();

			public static void UcitajStitInfo(Dictionary<string, string> podaci)
			{
				string naziv = podaci["IME"];
				string opis = podaci["OPIS"];
				Image slika = Image.FromFile(podaci["SLIKA"]);
				List<Preduvjet> preduvjeti = Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
				int maxNivo = int.Parse(podaci["MAX_NIVO"]);

				Formula izdrzljivost = Formula.IzStringa(podaci["IZDRZLJIVOST"]);
				Formula debljina = Formula.IzStringa(podaci["DEBLJINA"]);
				Formula ublazavanjeStete = Formula.IzStringa(podaci["UBLAZAVANJE"]);
				Formula obnavljanje = Formula.IzStringa(podaci["OBNAVLJANJE"]);
				Formula potrosnjaSnage = Formula.IzStringa(podaci["SNAGA"]);
				Formula prikrivanje = Formula.IzStringa(podaci["PRIKRIVANJE"]);
				Formula ometanje = Formula.IzStringa(podaci["OMETANJE"]);
				Formula cijena = Formula.IzStringa(podaci["CIJENA"]);

				StitInfo info = new StitInfo(
					naziv, opis, slika, preduvjeti, maxNivo,
					izdrzljivost, debljina, ublazavanjeStete, obnavljanje,
					potrosnjaSnage, prikrivanje, ometanje, cijena
					);

				Stitovi.Add(info);
				Kodovi.Add(podaci["KOD"], info);
			}

			public static List<Stit> DostupniStitovi(Dictionary<string, double> varijable, double velicinaStita)
			{
				List<Stit> ret = new List<Stit>();
				foreach (StitInfo si in Stitovi)
					if (si.dostupno(varijable))
						ret.Add(si.naciniKomponentu(varijable, velicinaStita));
				return ret;
			}

			public static StitInfo IzIda(int id)
			{
				foreach (StitInfo info in Stitovi)
					if (info.id == id)
						return info;
				throw new ArgumentException("Nepostojeći id štita.");
			}
			#endregion

			private Formula izdrzljivost;
			private Formula debljina;
			private Formula ublazavanjeStete;
			private Formula obnavljanje;
			private Formula snaga;
			private Formula prikrivanje;
			private Formula ometanje;
			private Formula cijena;

			private StitInfo(string naziv, string opis, Image slika,
				List<Preduvjet> preduvjeti, int maxNivo,
				Formula izdrzljivost, Formula debljina, Formula ublazavanjeStete,
				Formula obnavljanje, Formula snaga, Formula prikrivanje,
				Formula ometanje, Formula cijena)
				:
				base(naziv, opis, slika, preduvjeti, maxNivo)
			{
				this.izdrzljivost = izdrzljivost;
				this.debljina = debljina;
				this.ublazavanjeStete = ublazavanjeStete;
				this.obnavljanje = obnavljanje;
				this.snaga = snaga;
				this.prikrivanje = prikrivanje;
				this.ometanje = ometanje;
				this.cijena = cijena;
			}

			public Stit naciniKomponentu(Dictionary<string, double> varijable, double velicinaStita)
			{
				int nivo = maxDostupanNivo(varijable);
				return naciniKomponentu(nivo, velicinaStita);
			}

			public Stit naciniKomponentu(int nivo, double velicinaStita)
			{
				return new Stit(
					this,
					nivo,
					Evaluiraj(izdrzljivost, nivo),
					Evaluiraj(debljina, nivo),
					Evaluiraj(ublazavanjeStete, nivo),
					Evaluiraj(obnavljanje, nivo),
					Evaluiraj(snaga, nivo, velicinaStita),
					Evaluiraj(prikrivanje, nivo),
					Evaluiraj(ometanje, nivo),
					Evaluiraj(cijena, nivo, velicinaStita)
					);
			}
		}

		public double izdrzljivost { get; private set; }
		public double debljina { get; private set; }
		public double ublazavanjeStete { get; private set; }
		public double obnavljanje { get; private set; }
		public double snaga { get; private set; }
		public double prikrivanje { get; private set; }
		public double ometanje { get; private set; }
		public double cijena { get; private set; }

		public Stit(StitInfo info, int nivo,
			double izdrzljivost, double debljina, double ublazavanjeStete, double obnavljanje,
			double snaga, double prikrivanje, double ometanje, double cijena)
			: base(info, nivo)
		{	
			this.izdrzljivost = izdrzljivost;
			this.debljina = debljina;
			this.ublazavanjeStete = ublazavanjeStete;
			this.obnavljanje = obnavljanje;
			this.snaga = snaga;
			this.prikrivanje = prikrivanje;
			this.ometanje = ometanje;
			this.cijena = cijena;
		}
	}
}
