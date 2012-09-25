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
				List<Preduvjet> preduvjeti = Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
				int maxNivo = int.Parse(podaci["MAX_NIVO"]);

				Formula izdrzljivost = Formula.IzStringa(podaci["IZDRZLJIVOST"]);
				Formula ublazavanjeSteteKoef = Formula.IzStringa(podaci["UBLAZAVANJE_KOEF"]);
				Formula ublazavanjeSteteMax = Formula.IzStringa(podaci["UBLAZAVANJE_MAX"]);

				Oklopi.Add(new OklopInfo(
					naziv, opis, slika, preduvjeti, maxNivo,
					izdrzljivost, ublazavanjeSteteKoef, ublazavanjeSteteMax)
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

			public static List<Oklop> Dostupni(Dictionary<string, double> varijable)
			{
				List<Oklop> ret = new List<Oklop>();
				foreach (OklopInfo oklopInfo in Oklopi)
					if (oklopInfo.dostupno(varijable))
						ret.Add(oklopInfo.naciniKomponentu(varijable));

				return ret;
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
			private Formula ublazavanjeSteteKoef;
			private Formula ublazavanjeSteteMax;

			private OklopInfo(string naziv, string opis, Image slika,
				List<Preduvjet> preduvjeti, int maxNivo,
				Formula izdrzljivost, Formula ublazavanjeSteteKoef, Formula ublazavanjeSteteMax)
				:
				base(naziv, opis, slika, preduvjeti, maxNivo)
			{
				this.izdrzljivost = izdrzljivost;
				this.ublazavanjeSteteKoef = ublazavanjeSteteKoef;
				this.ublazavanjeSteteMax = ublazavanjeSteteMax;
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
					Evaluiraj(izdrzljivost, nivo),
					Evaluiraj(ublazavanjeSteteKoef, nivo),
					Evaluiraj(ublazavanjeSteteMax, nivo)
					);
			}
		}

		public double izdrzljivost { get; private set; }
		public double ublazavanjeSteteKoef { get; private set; }
		public double ublazavanjeSteteMax { get; private set; }

		public Oklop(OklopInfo info, int nivo, double izdrzljivost,
			double ublazavanjeSteteKoef, double ublazavanjeSteteMax)
			: base(info, nivo)
		{
			this.izdrzljivost = izdrzljivost;
			this.ublazavanjeSteteKoef = ublazavanjeSteteKoef;
			this.ublazavanjeSteteMax = ublazavanjeSteteMax;
		}
	}
}
