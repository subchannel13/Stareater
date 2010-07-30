using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Prototip
{
	public class Oruzje : Komponenta<Oruzje.OruzjeInfo>
	{
		public enum Ciljanje
		{
			Obrana,
			Normalno,
			Veliki_brodovi
		}

		public class OruzjeInfo : AKomponentaInfo
		{
			#region Statično
			public static Dictionary<Misija.Tip, List<OruzjeInfo>> Oruzja = new Dictionary<Misija.Tip, List<OruzjeInfo>>();
			public static Dictionary<string, OruzjeInfo> KodoviOruzja = new Dictionary<string, OruzjeInfo>();
			public static Dictionary<Ciljanje, string> OpisCiljanja = initOpisCiljanja();
			private static Dictionary<string, Ciljanje> StringUCiljanje = initStringUCiljanje();

			private static Dictionary<string, Ciljanje> initStringUCiljanje()
			{
				Dictionary<string, Ciljanje> stringUCiljanje = new Dictionary<string, Ciljanje>();
				stringUCiljanje.Add("OBRANA", Ciljanje.Obrana);
				stringUCiljanje.Add("NORMALNO", Ciljanje.Normalno);
				stringUCiljanje.Add("VELIKI_BRODOVI", Ciljanje.Veliki_brodovi);
				return stringUCiljanje;
			}
			private static Dictionary<Ciljanje, string> initOpisCiljanja()
			{
				Dictionary<Ciljanje, string> ret = new Dictionary<Ciljanje, string>();
				ret.Add(Ciljanje.Obrana, "mali objekti");
				ret.Add(Ciljanje.Normalno, "normalno");
				ret.Add(Ciljanje.Veliki_brodovi, "veliki brodovi");
				return ret;
			}

			public static void UcitajOruzjeInfo(Dictionary<string, string> podaci)
			{
				string naziv = podaci["IME"];
				string opis = podaci["OPIS"];
				Image slika = Image.FromFile(podaci["SLIKA"]);
				List<Tehnologija.Preduvjet> preduvjeti = Tehnologija.Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
				int maxNivo = int.Parse(podaci["MAX_NIVO"]);
				Misija.Tip misijaTip = Misija.StringUMisiju[podaci["MISIJA"]];

				Misija misija = Misija.Opisnici[misijaTip];
				Formula[] parametri = new Formula[misija.brParametara];
				for (int i = 0; i < misija.brParametara; i++)
					parametri[i] = Formula.IzStringa(podaci[misija.parametri[i].kod]);

				Ciljanje ciljanje = Ciljanje.Normalno;
				if (misija.imaCiljanje) ciljanje = StringUCiljanje[podaci["CILJANJE"]];

				if (!Oruzja.ContainsKey(misijaTip))
					Oruzja.Add(misijaTip, new List<OruzjeInfo>());

				OruzjeInfo info = new OruzjeInfo(
					naziv, opis, slika, preduvjeti, maxNivo,
					misijaTip, ciljanje,
					parametri,
					Formula.IzStringa(podaci["CIJENA"]),
					Formula.IzStringa(podaci["SNAGA"]),
					Formula.IzStringa(podaci["VELICINA"])
					);
				Oruzja[misijaTip].Add(info);
				KodoviOruzja.Add(podaci["KOD"], info);
			}

			public static Dictionary<Misija.Tip, List<Oruzje>> DostupnaOruzja(Dictionary<string, double> varijable)
			{
				Dictionary<Misija.Tip, List<Oruzje>> ret = new Dictionary<Misija.Tip, List<Oruzje>>();
				foreach (Misija.Tip misija in Oruzja.Keys)
				{
					ret.Add(misija, new List<Oruzje>());
					foreach(OruzjeInfo oi in Oruzja[misija])
						if (oi.dostupno(varijable))
							ret[misija].Add(oi.naciniKomponentu(varijable));
				}

				return ret;
			}

			public static OruzjeInfo IzIda(int id)
			{
				foreach (OruzjeInfo info in KodoviOruzja.Values)
					if (info.id == id)
						return info;
				throw new ArgumentException("Nepostojeći id oružja.");
			}
			#endregion

			public Misija.Tip misija { get; private set; }
			public Ciljanje ciljanje { get; private set; }
			private Formula[] parametri;
			private Formula snaga;
			private Formula cijena;
			private Formula velicina;

			private OruzjeInfo(string naziv, string opis, Image slika,
				List<Tehnologija.Preduvjet> preduvjeti, int maxNivo,
				Misija.Tip misija, Ciljanje ciljanje, Formula[] parametri,
				Formula cijena, Formula snaga, Formula velicina)
				:
				base(naziv, opis, slika, preduvjeti, maxNivo)
			{
				this.misija = misija;
				this.ciljanje = ciljanje;
				this.parametri = parametri;
				this.cijena = cijena;
				this.snaga = snaga;
				this.velicina = velicina;
			}

			public Oruzje naciniKomponentu(Dictionary<string, double> varijable)
			{
				int nivo = maxDostupanNivo(varijable);
				return naciniKomponentu(nivo);
			}

			public Oruzje naciniKomponentu(int nivo)
			{
				double[] parametri = new double[this.parametri.Length];
				
				for (int i = 0; i < parametri.Length; i++)
					parametri[i] = Evaluiraj(this.parametri[i], nivo);

				return new Oruzje(
					this,
					nivo,
					parametri,
					Evaluiraj(cijena, nivo),
					Evaluiraj(snaga, nivo),
					Evaluiraj(velicina, nivo)
					);
			}
		}

		public double[] parametri { get; private set; }
		public double cijena { get; private set; }
		public double snaga { get; private set; }
		public double velicina { get; private set; }

		public Oruzje(OruzjeInfo info, int nivo,
			double[] parametri, double cijena, double snaga, double velicina)
			: base(info, nivo)
		{
			this.parametri = parametri;
			this.cijena = cijena;
			this.snaga = snaga;
			this.velicina = velicina;
		}

		public Misija.Tip misija 
		{
			get { return info.misija; }
		}
		public Ciljanje ciljanje 
		{
			get { return info.ciljanje; }
		}

		public double vatrenaMoc
		{
			get { return parametri[Misija.VatrenaMoc]; }
		}

		public double brNapada
		{
			get { return parametri[Misija.UcinkovitostStitova]; }
		}

		public double preciznost
		{
			get { return parametri[Misija.Preciznost]; }
		}

		public double brKolonista
		{
			get { return parametri[Misija.BrKolonista]; }
		}

		public double radnaMjesta
		{
			get { return parametri[Misija.RadnaMjesta]; }
		}

		public double kapacitet
		{
			get { return parametri[Misija.Kapacitet]; }
		}
	}
}
