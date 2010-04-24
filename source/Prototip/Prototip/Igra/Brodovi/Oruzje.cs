using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Prototip
{
	public class Oruzje : Komponenta<Oruzje.OruzjeInfo>
	{
		public enum Misija
		{
			DirektnoOruzje,
			Projektil,
			Kolonizacija,
			Popravak,
			Spijunaza,
			Tegljenje,
			CivilniTransport,
			VojniTransport,
		}

		public enum Ciljanje
		{
			Obrana,
			Normalno,
			Veliki_brodovi
		}

		public class OruzjeInfo : AKomponentaInfo
		{
			#region Statično
			public static Dictionary<Misija, List<OruzjeInfo>> Oruzja = new Dictionary<Misija,List<OruzjeInfo>>();
			public static Dictionary<string, OruzjeInfo> KodoviOruzja = new Dictionary<string, OruzjeInfo>();
			public static Dictionary<Misija, string> OpisMisije = initOpisMisija();
			public static Dictionary<Ciljanje, string> OpisCiljanja = initOpisCiljanja();
			private static Dictionary<string, Misija> StringUMisiju = initStringUMisiju();
			private static Dictionary<string, Ciljanje> StringUCiljanje = initStringUCiljanje();

			private static Dictionary<string, Misija> initStringUMisiju()
			{
				Dictionary<string, Misija> stringUMisiju = new Dictionary<string,Misija>();
				stringUMisiju.Add("DIREKTNO_ORUZJE", Misija.DirektnoOruzje);
				stringUMisiju.Add("PROJEKTIL", Misija.Projektil);
				stringUMisiju.Add("KOLONIZACIJA", Misija.Kolonizacija);
				stringUMisiju.Add("POPRAVAK", Misija.Popravak);
				stringUMisiju.Add("SPIJUNAZA", Misija.Spijunaza);
				stringUMisiju.Add("TEGLJENJE", Misija.Tegljenje);
				stringUMisiju.Add("CIV_TRANSPORT", Misija.CivilniTransport);
				stringUMisiju.Add("VOJNI_TRANSPORT", Misija.VojniTransport);
				return stringUMisiju;
			}
			private static Dictionary<string, Ciljanje> initStringUCiljanje()
			{
				Dictionary<string, Ciljanje> stringUCiljanje = new Dictionary<string, Ciljanje>();
				stringUCiljanje.Add("OBRANA", Ciljanje.Obrana);
				stringUCiljanje.Add("NORMALNO", Ciljanje.Normalno);
				stringUCiljanje.Add("VELIKI_BRODOVI", Ciljanje.Veliki_brodovi);
				return stringUCiljanje;
			}
			private static Dictionary<Misija, string> initOpisMisija()
			{
				Dictionary<Misija, string> ret = new Dictionary<Misija, string>();
				ret.Add(Misija.DirektnoOruzje, "direktno oružje");
				ret.Add(Misija.Projektil, "projektil");
				ret.Add(Misija.Kolonizacija, "moduli za uspostavljanje kolonije");
				ret.Add(Misija.Popravak, "postrojenje za popravak i nadogradnju brodova");
				ret.Add(Misija.Spijunaza, "spijunska oprema");
				ret.Add(Misija.Tegljenje, "međuzvjezdani transport brodova");
				ret.Add(Misija.CivilniTransport, "civilni transport");
				ret.Add(Misija.VojniTransport, "vojni transport");
				return ret;
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
				Misija misija = StringUMisiju[podaci["MISIJA"]];

				if (!Oruzja.ContainsKey(misija))
					Oruzja.Add(misija, new List<OruzjeInfo>());

				OruzjeInfo info = new OruzjeInfo(
					naziv, opis, slika, preduvjeti, maxNivo,
					misija, StringUCiljanje[podaci["CILJANJE"]],
					Formula.NaciniFormulu(podaci["VATRENA_MOC"]),
					Formula.NaciniFormulu(podaci["BR_NAPADA"]),
					Formula.NaciniFormulu(podaci["PRECIZNOST"]),
					Formula.NaciniFormulu(podaci["CIJENA"]),
					Formula.NaciniFormulu(podaci["SNAGA"]),
					Formula.NaciniFormulu(podaci["VELICINA"])
					);
				Oruzja[misija].Add(info);
				KodoviOruzja.Add(podaci["KOD"], info);
			}

			public static Dictionary<Misija, List<Oruzje>> DostupnaOruzja(Dictionary<string, double> varijable)
			{
				Dictionary<Misija, List<Oruzje>> ret = new Dictionary<Misija, List<Oruzje>>();
				foreach (Misija misija in Oruzja.Keys)
				{
					ret.Add(misija, new List<Oruzje>());
					foreach(OruzjeInfo oi in Oruzja[misija])
						if (oi.dostupno(varijable))
							ret[misija].Add(oi.napraviKomponentu(varijable));
				}

				return ret;
			}
			#endregion

			public Misija misija { get; private set; }
			public Ciljanje ciljanje { get; private set; }
			private Formula vatrenaMoc;
			private Formula brNapada;
			private Formula preciznost;
			private Formula snaga;
			private Formula cijena;
			private Formula velicina;

			private OruzjeInfo(string naziv, string opis, Image slika,
				List<Tehnologija.Preduvjet> preduvjeti, int maxNivo,
				Misija misija, Ciljanje ciljanje, Formula moc, Formula brNapada,
				Formula preciznost, Formula cijena, Formula snaga,
				Formula velicina)
				:
				base(naziv, opis, slika, preduvjeti, maxNivo)
			{
				this.misija = misija;
				this.ciljanje = ciljanje;
				this.vatrenaMoc = moc;
				this.brNapada = brNapada;
				this.preciznost = preciznost;
				this.cijena = cijena;
				this.snaga = snaga;
				this.velicina = velicina;
			}

			public Oruzje napraviKomponentu(Dictionary<string, double> varijable)
			{
				int nivo = maxDostupanNivo(varijable);
				return new Oruzje(
					this,
					nivo,
					Evaluiraj(vatrenaMoc, nivo),
					Evaluiraj(brNapada, nivo),
					Evaluiraj(preciznost, nivo),
					Evaluiraj(cijena, nivo),
					Evaluiraj(snaga, nivo),
					Evaluiraj(velicina, nivo)
					);
					
			}

		}

		public double vatrenaMoc { get; private set; }
		public double brNapada { get; private set; }
		public double preciznost { get; private set; }
		public double cijena { get; private set; }
		public double snaga { get; private set; }
		public double velicina { get; private set; }

		public Oruzje(OruzjeInfo info, int nivo,
			double vatrenaMoc, double brNapada,
			double preciznost, double cijena, double snaga, double velicina)
			: base(info, nivo)
		{
			this.vatrenaMoc = vatrenaMoc;
			this.brNapada = brNapada;
			this.preciznost = preciznost;
			this.cijena = cijena;
			this.snaga = snaga;
			this.velicina = velicina;
		}

		public Misija misija 
		{
			get { return info.misija; }
		}
		public Ciljanje ciljanje 
		{
			get { return info.ciljanje; }
		}
	}
}
