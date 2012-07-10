using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Zvjezdojedac.Podaci.Formule;

namespace Zvjezdojedac.Igra.Brodovi
{
	public class Trup : Komponenta<Trup.TrupInfo>
	{
		public class TrupInfo : AKomponentaInfo
		{
			#region Statično
			public static List<TrupInfo> Trupovi = new List<TrupInfo>();
			public static Dictionary<string, TrupInfo> KodoviTrupova = new Dictionary<string, TrupInfo>();

			public static void UcitajTrupInfo(Dictionary<string, string> podaci)
			{
				string naziv = podaci["IME"];
				string opis = podaci["OPIS"];
				Image slika = Image.FromFile(podaci["SLIKA"]);
				List<Preduvjet> preduvjeti = Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
				int maxNivo = int.Parse(podaci["MAX_NIVO"]);

				int velicina = int.Parse(podaci["VELICINA"]);
				Formula ometanje = Formula.IzStringa(podaci["OMETANJE"]);
				Formula prikrivanje = Formula.IzStringa(podaci["PRIKRIVANJE"]);
				Formula velicina_MZPogona_p = Formula.IzStringa(podaci["VELICINA_MZPOGONA"]);
				Formula velicina_reaktora_p = Formula.IzStringa(podaci["VELICINA_REAKTORA"]);
				Formula velicina_stita_p = Formula.IzStringa(podaci["VELICINA_STITA"]);
				Formula nosivost = Formula.IzStringa(podaci["NOSIVOST"]);
				Formula tromost = Formula.IzStringa(podaci["TROMOST"]);
				Formula bazaOklopa = Formula.IzStringa(podaci["BAZA_OKLOPA"]);
				Formula bazaOklopUblazavanja = Formula.IzStringa(podaci["BAZA_OKLOP_UBL"]);
				Formula bazaStita = Formula.IzStringa(podaci["BAZA_STITA"]);
				Formula cijena = Formula.IzStringa(podaci["CIJENA"]);
				Formula senzorPlus = Formula.IzStringa(podaci["SENZOR_PLUS"]);

				TrupInfo trupInfo = new TrupInfo(
					naziv, opis, slika, preduvjeti, maxNivo,
					velicina, ometanje, prikrivanje,
					velicina_MZPogona_p, velicina_reaktora_p,
					velicina_stita_p, nosivost, tromost, 
					bazaOklopa, bazaOklopUblazavanja, bazaStita,
					senzorPlus, cijena
					);
				
				Trupovi.Add(trupInfo);
				KodoviTrupova.Add(podaci["KOD"].Trim(), trupInfo);
			}
			
			public static List<Trup> DostupniTrupovi(Dictionary<string, double> varijable)
			{
				List<Trup> ret = new List<Trup>();
				foreach (TrupInfo ti in Trupovi)
					if (ti.dostupno(varijable))
						ret.Add(ti.naciniKomponentu(varijable));
				return ret;
			}

			public static TrupInfo IzIda(int id)
			{
				foreach (TrupInfo info in Trupovi)
					if (info.id == id)
						return info;
				throw new ArgumentException("Nepostojeći id taktike.");
			}
			#endregion

			public int velicina { get; private set; }
			private Formula ometanje;
			private Formula prikrivanje;
			private Formula nosivost;
			private Formula velicina_MZPogona;
			private Formula velicina_reaktora;
			private Formula velicina_stita;
			private Formula tromost;
			private Formula bazaOklopa;
			private Formula bazaOklopUblazavanja;
			private Formula bazaStita;
			private Formula cijena;
			private Formula senzorPlus;

			private TrupInfo(string naziv, string opis, Image slika,
				List<Preduvjet> preduvjeti, int maxNivo,
				int velicina, Formula ometanje, Formula prikrivanje,
				Formula velicina_MZPogona, Formula velicina_reaktora,
				Formula velicina_stita,	Formula nosivost, Formula tromost,
				Formula bazaOklopa, Formula bazaOklopUblazavanja, Formula bazaStita, 
				Formula senzorPlus,
				Formula cijena)
				: 
				base(naziv, opis, slika, preduvjeti, maxNivo)
			{
				this.velicina = velicina;
				this.ometanje = ometanje;
				this.prikrivanje = prikrivanje;
				this.velicina_MZPogona = velicina_MZPogona;
				this.velicina_reaktora = velicina_reaktora;
				this.velicina_stita = velicina_stita;
				this.tromost = tromost;
				this.nosivost = nosivost;
				this.bazaOklopa = bazaOklopa;
				this.bazaOklopUblazavanja = bazaOklopUblazavanja;
				this.bazaStita = bazaStita;
				this.senzorPlus = senzorPlus;
				this.cijena = cijena;
			}

			public Trup naciniKomponentu(Dictionary<string, double> varijable)
			{
				int nivo = maxDostupanNivo(varijable);
				return naciniKomponentu(nivo);
			}

			public Trup naciniKomponentu(int nivo)
			{
				return new Trup(
					this,
					nivo,
					Evaluiraj(ometanje, nivo),
					Evaluiraj(prikrivanje, nivo),
					Evaluiraj(velicina_MZPogona, nivo),
					Evaluiraj(velicina_reaktora, nivo),
					Evaluiraj(velicina_stita, nivo),
					Evaluiraj(nosivost, nivo),
					Evaluiraj(tromost, nivo),
					Evaluiraj(bazaOklopa, nivo),
					Evaluiraj(bazaOklopUblazavanja, nivo),
					Evaluiraj(bazaStita, nivo),
					(int)Evaluiraj(senzorPlus, nivo),
					Evaluiraj(cijena, nivo)
					);
			}
		}

		public double OmetanjeBaza { get; private set; }
		public double PrikrivanjeBaza { get; private set; }
		public double VelicinaMZPogona { get; private set; }
		public double VelicinaReaktora { get; private set; }
		public double VelicinaStita { get; private set; }
		public double Nosivost { get; private set; }
		public double Tromost { get; private set; }
		public double BazaOklopa { get; private set; }
		public double BazaOklopUblazavanja { get; private set; }
		public double BazaStita { get; private set; }
		public int SenzorPlus { get; private set; }
		public double Cijena { get; private set; }

		private Trup(TrupInfo info, int nivo,
			double ometanjeBaza, double prikrivanjeBaza, double velicina_MZPogona,
			double velicina_reaktora, double velicina_stita, double nosivost,
			double tromost, double bazaOklopa, double bazaOklopUblazavanja, double bazaStita,
			int senzorPlus,
			double cijena)
			: base(info, nivo)
		{
			this.OmetanjeBaza = ometanjeBaza;
			this.PrikrivanjeBaza = prikrivanjeBaza;
			this.VelicinaMZPogona = velicina_MZPogona;
			this.VelicinaReaktora = velicina_reaktora;
			this.VelicinaStita = velicina_stita;
			this.Nosivost = nosivost;
			this.Tromost = tromost;
			this.BazaOklopa = bazaOklopa;
			this.BazaOklopUblazavanja = bazaOklopUblazavanja;
			this.BazaStita = bazaStita;
			this.SenzorPlus = senzorPlus;
			this.Cijena = cijena;
		}

		public int velicina
		{
			get { return info.velicina; }
		}

		public override string ToString()
		{
			return info.naziv;
		}
	}
}
