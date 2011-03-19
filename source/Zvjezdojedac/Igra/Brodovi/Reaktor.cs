using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Prototip
{
	public class Reaktor : Komponenta<Reaktor.ReaktorInfo>
	{
		public class ReaktorInfo : AKomponentaInfo
		{
			#region Statično
			public static List<ReaktorInfo> Reaktori = new List<ReaktorInfo>();

			public static void UcitajReaktorInfo(Dictionary<string, string> podaci)
			{
				string naziv = podaci["IME"];
				string opis = podaci["OPIS"];
				Image slika = Image.FromFile(podaci["SLIKA"]);
				List<Tehnologija.Preduvjet> preduvjeti = Tehnologija.Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
				int maxNivo = int.Parse(podaci["MAX_NIVO"]);

				Formula minVelicina = Formula.IzStringa(podaci["VELICINA_MIN"]);
				Formula snaga = Formula.IzStringa(podaci["SNAGA"]);

				Reaktori.Add(new ReaktorInfo(
					naziv, opis, slika, preduvjeti, maxNivo,
					snaga, minVelicina)
					);
			}

			public static IEnumerable<Reaktor> Dostupni(Dictionary<string, double> varijable)
			{
				List<Reaktor> rez = new List<Reaktor>();
				foreach (ReaktorInfo reaktor in Reaktori)
					if (reaktor.dostupno(varijable)) {
						int nivo = reaktor.maxDostupanNivo(varijable);
						rez.Add(reaktor.naciniKomponentu(nivo, reaktor.minimalnaVelicina(nivo)));
					}
				return rez;
			}

			public static Reaktor NajboljiReaktor(Dictionary<string, double> varijable, double velicinaReaktora)
			{
				double max = double.MinValue;
				Reaktor naj = null;

				foreach (ReaktorInfo ri in Reaktori)
					if (ri.dostupno(varijable))
					{
						int nivo = ri.maxDostupanNivo(varijable);
						if (ri.minimalnaVelicina(nivo) > velicinaReaktora)
							continue;
						Reaktor trenutni = ri.naciniKomponentu(varijable, velicinaReaktora);

						if (trenutni.snaga > max)
						{
							max = trenutni.snaga;
							naj = trenutni;
						}
					}

				return naj;
			}

			public static ReaktorInfo IzIda(int id)
			{
				foreach (ReaktorInfo info in Reaktori)
					if (info.id == id)
						return info;
				throw new ArgumentException("Nepostojeći id reaktora.");
			}
			#endregion

			private Formula snaga;
			private Formula minVelicina;

			private ReaktorInfo(string naziv, string opis, Image slika,
				List<Tehnologija.Preduvjet> preduvjeti, int maxNivo,
				Formula snaga, Formula minVelicina)
				:
				base(naziv, opis, slika, preduvjeti, maxNivo)
			{
				this.snaga = snaga;
				this.minVelicina = minVelicina;
			}

			public double minimalnaVelicina(int nivo)
			{
				return Evaluiraj(minVelicina, nivo);
			}

			public Reaktor naciniKomponentu(Dictionary<string, double> varijable, double velicinaReaktora)
			{
				int nivo = maxDostupanNivo(varijable);
				return naciniKomponentu(nivo, velicinaReaktora);
			}

			public Reaktor naciniKomponentu(int nivo, double velicinaReaktora)
			{
				return new Reaktor(
					this,
					nivo,
					Evaluiraj(snaga, nivo, velicinaReaktora)
					);
			}
			
		}

		public double snaga { get; private set; }

		private Reaktor(ReaktorInfo info, int nivo, double snaga)
			: base(info, nivo)
		{
			this.snaga = snaga;
		}
	}
}
