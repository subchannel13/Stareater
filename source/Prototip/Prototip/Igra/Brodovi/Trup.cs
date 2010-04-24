using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Prototip
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
				List<Tehnologija.Preduvjet> preduvjeti = Tehnologija.Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
				int maxNivo = int.Parse(podaci["MAX_NIVO"]);

				int velicina = int.Parse(podaci["VELICINA"]);
				Formula kapacitetPrikrivanja = Formula.NaciniFormulu(podaci["KAPACITET_PRIKRIVANJA"]);
				Formula velicina_MZPogona_p = Formula.NaciniFormulu(podaci["VELICINA_MZPOGONA"]);
				Formula velicina_reaktora_p = Formula.NaciniFormulu(podaci["VELICINA_REAKTORA"]);
				Formula velicina_stita_p = Formula.NaciniFormulu(podaci["VELICINA_STITA"]);
				Formula nosivost = Formula.NaciniFormulu(podaci["NOSIVOST"]);
				Formula tromost = Formula.NaciniFormulu(podaci["TROMOST"]);
				Formula bazaOklopa = Formula.NaciniFormulu(podaci["BAZA_OKLOPA"]);
				Formula bazaStita = Formula.NaciniFormulu(podaci["BAZA_STITA"]);
				Formula cijena = Formula.NaciniFormulu(podaci["CIJENA"]);
				Formula brojSenzora = Formula.NaciniFormulu(podaci["BR_SENZORA"]);

				TrupInfo trupInfo = new TrupInfo(
					naziv, opis, slika, preduvjeti, maxNivo,
					velicina, kapacitetPrikrivanja, 
					velicina_MZPogona_p, velicina_reaktora_p,
					velicina_stita_p, nosivost, tromost, bazaOklopa,
					bazaStita, brojSenzora, cijena
					);
				
				Trupovi.Add(trupInfo);
				KodoviTrupova.Add(podaci["KOD"].Trim(), trupInfo);
			}
			
			public static List<Trup> DostupniTrupovi(Dictionary<string, double> varijable)
			{
				List<Trup> ret = new List<Trup>();
				foreach (TrupInfo ti in Trupovi)
					if (ti.dostupno(varijable))
						ret.Add(ti.napraviKomponentu(varijable));
				return ret;
			}
			#endregion

			public int velicina { get; private set; }
			private Formula kapacitetPrikrivanja;
			private Formula nosivost;
			private Formula velicina_MZPogona;
			private Formula velicina_reaktora;
			private Formula velicina_stita;
			private Formula tromost;
			private Formula bazaOklopa;
			private Formula bazaStita;
			private Formula cijena;
			private Formula brojSenzora;

			private TrupInfo(string naziv, string opis, Image slika,
				List<Tehnologija.Preduvjet> preduvjeti, int maxNivo,
				int velicina, Formula kapacitetPrikrivanja,
				Formula velicina_MZPogona, Formula velicina_reaktora,
				Formula velicina_stita,	Formula nosivost, Formula tromost,
				Formula bazaOklopa, Formula bazaStita, Formula brojSenzora,
				Formula cijena)
				: 
				base(naziv, opis, slika, preduvjeti, maxNivo)
			{
				this.velicina = velicina;
				this.kapacitetPrikrivanja = kapacitetPrikrivanja;
				this.velicina_MZPogona = velicina_MZPogona;
				this.velicina_reaktora = velicina_reaktora;
				this.velicina_stita = velicina_stita;
				this.tromost = tromost;
				this.nosivost = nosivost;
				this.bazaOklopa = bazaOklopa;
				this.bazaStita = bazaStita;
				this.brojSenzora = brojSenzora;
				this.cijena = cijena;
			}

			public Trup napraviKomponentu(Dictionary<string, double> varijable)
			{
				int nivo = maxDostupanNivo(varijable);
				return new Trup(
					this,
					nivo,
					Evaluiraj(kapacitetPrikrivanja, nivo),
					Evaluiraj(velicina_MZPogona, nivo),
					Evaluiraj(velicina_reaktora, nivo),
					Evaluiraj(velicina_stita, nivo),
					Evaluiraj(nosivost, nivo),
					Evaluiraj(tromost, nivo),
					Evaluiraj(bazaOklopa, nivo),
					Evaluiraj(bazaStita, nivo),
					(int)Evaluiraj(brojSenzora, nivo),
					Evaluiraj(cijena, nivo)
					);
			}
		}

		public double kapacitetPrikrivanja { get; private set; }
		public double velicina_MZPogona { get; private set; }
		public double velicina_reaktora { get; private set; }
		public double velicina_stita { get; private set; }
		public double nosivost { get; private set; }
		public double tromost { get; private set; }
		public double bazaOklopa { get; private set; }
		public double bazaStita { get; private set; }
		public int brojSenzora { get; private set; }
		public double cijena { get; private set; }

		private Trup(TrupInfo info, int nivo,
			double kapacitetPrikrivanja, double velicina_MZPogona,
			double velicina_reaktora, double velicina_stita, double nosivost,
			double tromost, double bazaOklopa, double bazaStita, int brojSenzora,
			double cijena)
			: base(info, nivo)
		{			
			this.kapacitetPrikrivanja = kapacitetPrikrivanja;
			this.velicina_MZPogona = velicina_MZPogona;
			this.velicina_reaktora = velicina_reaktora;
			this.velicina_stita = velicina_stita;
			this.nosivost = nosivost;
			this.tromost = tromost;
			this.bazaOklopa = bazaOklopa;
			this.bazaStita = bazaStita;
			this.brojSenzora = brojSenzora;
			this.cijena = cijena;
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
