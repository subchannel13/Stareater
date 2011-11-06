using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Podaci.Formule;

namespace Zvjezdojedac.Igra.Brodovi
{
	public class SpecijalnaOprema : Komponenta<SpecijalnaOprema.SpecijalnaOpremaInfo>
	{
		public class SpecijalnaOpremaInfo : AKomponentaInfo
		{
			#region Statično
			public static List<SpecijalnaOpremaInfo> SpecijalnaOprema = new List<SpecijalnaOpremaInfo>();
			public static Dictionary<string, SpecijalnaOpremaInfo> Kodovi = new Dictionary<string, SpecijalnaOpremaInfo>();
			
			public static void UcitajSpecijalnaOpremaInfo(Dictionary<string, string> podaci)
			{
				string naziv = podaci["IME"];
				string opis = podaci["OPIS"];
				Image slika = Image.FromFile(podaci["SLIKA"]);
				List<Preduvjet> preduvjeti = Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
				int maxNivo = int.Parse(podaci["MAX_NIVO"]);

				Dictionary<string, Formula> efekti = new Dictionary<string,Formula>();
				string[] naziviEfekata = podaci["EFEKTI"].Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
				for(int i = 0; i < naziviEfekata.Length; i++)
					efekti.Add(naziviEfekata[i].Trim(), Formula.IzStringa(podaci["FORMULA" + i]));

				SpecijalnaOpremaInfo info = new SpecijalnaOpremaInfo(
					naziv, opis, slika, preduvjeti, maxNivo,
					efekti,
					Formula.IzStringa(podaci["CIJENA"]),
					Formula.IzStringa(podaci["VELICINA"]),
					Formula.IzStringa(podaci["MAX_KOLICINA"])
					);
				SpecijalnaOprema.Add(info);
				Kodovi.Add(podaci["KOD"], info);
			}

			public static List<SpecijalnaOprema> DostupnaOprema(Dictionary<string, double> varijable, double velicinaTrupa)
			{
				List<SpecijalnaOprema> ret = new List<SpecijalnaOprema>();
				foreach (SpecijalnaOpremaInfo soi in SpecijalnaOprema)
					if (soi.dostupno(varijable))
						ret.Add(soi.naciniKomponentu(varijable, velicinaTrupa));
				return ret;
			}

			public static SpecijalnaOpremaInfo IzIda(int id)
			{
				foreach (SpecijalnaOpremaInfo info in SpecijalnaOprema)
					if (info.id == id)
						return info;
				throw new ArgumentException("Nepostojeći id specijalne opreme.");
			}
			#endregion

			private Dictionary<string, Formula> efekti;
			private Formula cijena;
			private Formula velicina;
			private Formula maxKolicina;

			public int maxKolicinaIkako { get; private set; }

			private SpecijalnaOpremaInfo(string naziv, string opis, Image slika,
				List<Preduvjet> preduvjeti, int maxNivo,
				Dictionary<string, Formula> efekti, Formula cijena,
				Formula velicina, Formula maxKolicina)
				:
				base(naziv, opis, slika, preduvjeti, maxNivo)
			{
				this.efekti = efekti;
				this.cijena = cijena;
				this.velicina = velicina;
				this.maxKolicina = maxKolicina;

				maxKolicinaIkako = int.MinValue;
				for (int i = 0; i <= maxNivo; i++)
				{
					int tmp = (int)Evaluiraj(maxKolicina, i);
					if (tmp > maxKolicinaIkako)
						maxKolicinaIkako = tmp;
				}
			}

			public SpecijalnaOprema naciniKomponentu(Dictionary<string, double> varijable, double velicinaTrupa)
			{
				int nivo = maxDostupanNivo(varijable);
				return naciniKomponentu(nivo, velicinaTrupa);
			}

			public SpecijalnaOprema naciniKomponentu(int nivo, double velicinaTrupa)
			{
				Dictionary<string, double> efekti = new Dictionary<string, double>();
				foreach (string s in this.efekti.Keys)
					efekti.Add(s, Evaluiraj(this.efekti[s], nivo, velicinaTrupa));

				return new SpecijalnaOprema(
					this,
					nivo,
					efekti,
					Math.Ceiling(Evaluiraj(velicina, nivo, velicinaTrupa)),
					Evaluiraj(cijena, nivo, velicinaTrupa),
					(int)Evaluiraj(maxKolicina, nivo)
					);
			}
		}

		public Dictionary<string, double> efekti { get; private set;}
		public double velicina { get; private set; }
		public double cijena { get; private set; }
		public int maxKolicina { get; private set; }

		public SpecijalnaOprema(SpecijalnaOpremaInfo info, int nivo,
			Dictionary<string, double> efekti, double velicina, double cijena,
			int maxKolicina)
			: base(info, nivo)
		{
			this.efekti = efekti;
			this.velicina = velicina;
			this.cijena = cijena;
			this.maxKolicina = maxKolicina;
		}

		public List<string> opisEfekata
		{
			get
			{
				List<string> opis = new List<string>();

				foreach (string efekt in efekti.Keys)
				{
					double predznak = 1;
					string predznakStr;
					if (efekti[efekt] < 0)
					{
						predznak = -1;
						predznakStr = "-";
					}
					else
						predznakStr = "+";

					string zaDodat = Dizajn.OpisiPrivrEfekata[efekt].opis + ": " + predznakStr;
					if (Dizajn.OpisiPrivrEfekata[efekt].tip == Dizajn.PrivEfektTip.Koeficijent)
						zaDodat += (efekti[efekt] * predznak * 100).ToString("0.#") + "%";
					else
						zaDodat += Fje.PrefiksFormater(efekti[efekt] * predznak);

					opis.Add(zaDodat);
				}

				return opis;
			}
		}
	}
}
