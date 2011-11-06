using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Zvjezdojedac.Podaci.Formule;

namespace Zvjezdojedac.Igra.Brodovi
{
	public class Senzor : Komponenta<Senzor.SenzorInfo>
	{
		public static double BonusKolicine(double kolicina)
		{
			return Math.Pow(kolicina, 1 / 3.0);
		}

		public class SenzorInfo : AKomponentaInfo
		{
			#region Statično
			public static List<SenzorInfo> Senzori = new List<SenzorInfo>();

			public static void UcitajSenzorInfo(Dictionary<string, string> podaci)
			{
				string naziv = podaci["IME"];
				string opis = podaci["OPIS"];
				Image slika = Image.FromFile(podaci["SLIKA"]);
				List<Preduvjet> preduvjeti = Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
				int maxNivo = int.Parse(podaci["MAX_NIVO"]);

				Formula razlucivost = Formula.IzStringa(podaci["RAZLUCIVOST"]);

				Senzori.Add(new SenzorInfo(
					naziv, opis, slika, preduvjeti, maxNivo,
					razlucivost)
					);
			}

			public static IEnumerable<Senzor> Dostupni(Dictionary<string, double> varijable)
			{
				List<Senzor> rez = new List<Senzor>();
				foreach (SenzorInfo si in Senzori)
					if (si.dostupno(varijable))
						rez.Add(si.naciniKomponentu(varijable));
				return rez;
			}

			public static Senzor NajboljiSenzor(Dictionary<string, double> varijable)
			{
				double max = double.MinValue;
				Senzor naj = null;

				foreach (SenzorInfo si in Senzori)
					if (si.dostupno(varijable, 0))
					{
						Senzor trenutni = si.naciniKomponentu(varijable);

						if (trenutni.razlucivost > max)
						{
							max = trenutni.razlucivost;
							naj = trenutni;
						}
					}

				return naj;
			}

			public static SenzorInfo IzIda(int id)
			{
				foreach (SenzorInfo info in Senzori)
					if (info.id == id)
						return info;
				throw new ArgumentException("Nepostojeći id senzora.");
			}
			#endregion

			private Formula razlucivost;

			private SenzorInfo(string naziv, string opis, Image slika,
				List<Preduvjet> preduvjeti, int maxNivo,
				Formula razlucivost)
				:
				base(naziv, opis, slika, preduvjeti, maxNivo)
			{
				this.razlucivost = razlucivost;
			}

			public Senzor naciniKomponentu(Dictionary<string, double> varijable)
			{
				int nivo = maxDostupanNivo(varijable);
				return naciniKomponentu(nivo);
			}

			public Senzor naciniKomponentu(int nivo)
			{
				return new Senzor(
					this, 
					nivo,
					Evaluiraj(razlucivost, nivo));
			}
		}

		/// <summary>
		/// Sposobnost senzora da detektira prikrivene brodove.
		/// </summary>
		public double razlucivost { get; private set; }

		public Senzor(SenzorInfo info, int nivo, double razlucivost)
			: base(info, nivo)
		{
			this.razlucivost = razlucivost;
		}
	}
}
