using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Zvjezdojedac.Podaci.Formule;

namespace Zvjezdojedac.Igra.Brodovi
{
	public class Oklop : Komponenta<Oklop.OklopInfo>
	{
		public class OklopInfo : AKomponentaInfo
		{
			#region Statično
			public static List<OklopInfo> Oklopi = new List<OklopInfo>();

			public static void UcitajOklopInfo(Dictionary<string, string> podaci)
			{
				string naziv = podaci["IME"];
				string opis = podaci["OPIS"];
				Image slika = Image.FromFile(podaci["SLIKA"]);
				List<Tehnologija.Preduvjet> preduvjeti = Tehnologija.Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
				int maxNivo = int.Parse(podaci["MAX_NIVO"]);

				Formula izdrzljivost = Formula.IzStringa(podaci["IZDRZLJIVOST"]);

				Oklopi.Add(new OklopInfo(
					naziv, opis, slika, preduvjeti, maxNivo,
					izdrzljivost)
					);
			}

			public static Oklop NajboljiOklop(Dictionary<string, double> varijable)
			{
				double max = double.MinValue;
				Oklop naj = null;

				foreach (OklopInfo oi in Oklopi)
					if (oi.dostupno(varijable, 0))
					{
						Oklop trenutni = oi.naciniKomponentu(varijable);

						if (trenutni.izdrzljivost > max)
						{
							max = trenutni.izdrzljivost;
							naj = trenutni;
						}
					}

				return naj;
			}

			public static OklopInfo IzIda(int id)
			{
				foreach (OklopInfo info in Oklopi)
					if (info.id == id)
						return info;
				throw new ArgumentException("Nepostojeći id oklopa.");
			}
			#endregion

			private Formula izdrzljivost;

			private OklopInfo(string naziv, string opis, Image slika,
				List<Tehnologija.Preduvjet> preduvjeti, int maxNivo,
				Formula izdrzljivost)
				:
				base(naziv, opis, slika, preduvjeti, maxNivo)
			{
				this.izdrzljivost = izdrzljivost;
			}

			public Oklop naciniKomponentu(Dictionary<string, double> varijable)
			{
				int nivo = maxDostupanNivo(varijable);
				return naciniKomponentu(nivo);
			}

			public Oklop naciniKomponentu(int nivo)
			{
				return new Oklop(
					this, 
					nivo, 
					Evaluiraj(izdrzljivost, nivo)
					);
			}
		}

		public double izdrzljivost { get; private set; }

		public Oklop(OklopInfo info, int nivo, double izdrzljivost)
			: base(info, nivo)
		{
			this.izdrzljivost = izdrzljivost;
		}
	}
}
