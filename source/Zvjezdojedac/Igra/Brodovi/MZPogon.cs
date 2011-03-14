using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Prototip
{
	public class MZPogon : Komponenta<MZPogon.MZPogonInfo>
	{
		public class MZPogonInfo : AKomponentaInfo
		{
			#region Statično
			public static List<MZPogonInfo> MZPogoni = new List<MZPogonInfo>();

			public static void UcitajMZPogonInfo(Dictionary<string, string> podaci)
			{
				string naziv = podaci["IME"];
				string opis = podaci["OPIS"];
				Image slika = Image.FromFile(podaci["SLIKA"]);
				List<Tehnologija.Preduvjet> preduvjeti = Tehnologija.Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
				int maxNivo = int.Parse(podaci["MAX_NIVO"]);

				Formula minVelicina = Formula.IzStringa(podaci["VELICINA_MIN"]);
				Formula brzina = Formula.IzStringa(podaci["BRZINA"]);
				Formula snaga = Formula.IzStringa(podaci["SNAGA"]);
				Formula cijena = Formula.IzStringa(podaci["CIJENA"]);

				MZPogoni.Add(new MZPogonInfo(
					naziv, opis, slika, preduvjeti, maxNivo,
					brzina, snaga, minVelicina, cijena)
					);
			}

			public static MZPogon NajboljiMZPogon(Dictionary<string, double> varijable, double velicinaMZPogona)
			{
				double max = double.MinValue;
				MZPogon naj = null;

				foreach (MZPogonInfo mzpi in MZPogoni)
					if (mzpi.dostupno(varijable, 0))
					{
						int nivo = mzpi.maxDostupanNivo(varijable);
						if (mzpi.minimalnaVelicina(nivo) > velicinaMZPogona)
							continue;
						MZPogon trenutni = mzpi.naciniKomponentu(varijable, velicinaMZPogona);

						if (trenutni.brzina > max)
						{
							max = trenutni.brzina;
							naj = trenutni;
						}
					}

				return naj;
			}

			public static MZPogonInfo IzIda(int id)
			{
				foreach (MZPogonInfo info in MZPogoni)
					if (info.id == id)
						return info;
				throw new ArgumentException("Nepostojeći id MZ pogona.");
			}
			#endregion

			private Formula brzina;
			private Formula snaga;
			private Formula minVelicina;
			private Formula cijena;

			private MZPogonInfo(string naziv, string opis, Image slika,
				List<Tehnologija.Preduvjet> preduvjeti, int maxNivo,
				Formula brzina, Formula snaga,
				Formula minVelicina, Formula cijena)
				:
				base(naziv, opis, slika, preduvjeti, maxNivo)
			{
				this.brzina = brzina;
				this.snaga = snaga;
				this.minVelicina = minVelicina;
				this.cijena = cijena;
			}

			public MZPogon naciniKomponentu(Dictionary<string, double> varijable, double velicinaTrupa)
			{
				int nivo = maxDostupanNivo(varijable);
				return naciniKomponentu(nivo, velicinaTrupa);
			}

			public MZPogon naciniKomponentu(int nivo, double velicinaTrupa)
			{
				return new MZPogon(
					this,
					nivo,
					Evaluiraj(brzina, nivo, velicinaTrupa),
					Evaluiraj(snaga, nivo, velicinaTrupa),
					Evaluiraj(cijena, nivo, velicinaTrupa)
					);
			}

			/// <summary>
			/// Minimalna količina prostora koju pogon zauzima.
			/// </summary>
			/// <param name="nivo">Nivo pogona.</param>
			/// <returns></returns>
			public double minimalnaVelicina(int nivo)
			{
				return Evaluiraj(minVelicina, nivo);
			}
		}

		public double brzina { get; private set; }
		public double snaga { get; private set; }
		public double cijena { get; private set; }

		private MZPogon(MZPogonInfo info, int nivo, double brzina,
			double snaga, double cijena)
			: base(info, nivo)
		{
			this.brzina = brzina;
			this.snaga = snaga;
			this.cijena = cijena;
		}
	}
}
