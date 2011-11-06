using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Podaci.Formule;
using Zvjezdojedac.Alati;

namespace Zvjezdojedac.Igra
{
	public class Zgrada : IPohranjivoSB
	{
		public const string BrojZgrada = "BR_ZGRADA";

		#region Ucinci

		public abstract class Ucinak
		{
			protected Formula intenzitet;

			public abstract void djeluj(AGradiliste gradiliste, Dictionary<string, double> varijable);

			public abstract void noviKrug(AGradiliste gradiliste, Dictionary<string, double> varijable);

			#region Statično
			protected delegate Ucinak TvornicaUcinka(string[] parametri);
			private static Dictionary<string, TvornicaUcinka> prototipovi = null;

			public static Ucinak napraviUcinak(string tip)
			{
				if (prototipovi == null)
				{
					prototipovi = new Dictionary<string, TvornicaUcinka>();
					prototipovi.Add("SET_VAR", UcinakSetVar.NapraviSe);
				}

				string[] parametriStr = tip.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < parametriStr.Length; i++)
					parametriStr[i] = parametriStr[i].Trim();

				return prototipovi[parametriStr[0]](parametriStr);
			}
			#endregion
		}

		public class UcinakSetVar : Ucinak
		{
			private string varijabla;
			private Formula formula;

			public UcinakSetVar()
			{}

			private UcinakSetVar(string varijabla, Formula formula)
			{
				this.varijabla = varijabla;
				this.formula = formula;
			}

			public static Ucinak NapraviSe(string[] parametri)
			{
				return new UcinakSetVar(parametri[1], Formula.IzStringa(parametri[2]));
			}

			public override void djeluj(AGradiliste gradiliste, Dictionary<string, double> varijable)
			{
				gradiliste.Efekti[varijabla] = formula.iznos(varijable);
			}

			public override void noviKrug(AGradiliste gradiliste, Dictionary<string, double> varijable)
			{ }
		}
		#endregion

		public class ZgradaInfo : IIdentifiable
		{
			protected string _ime;
			public int id { get; private set; }

			public Formula CijenaGradnje { get; private set; }
			public Formula CijenaOdrzavanja { get; private set; }
			public Formula DopustenaKolicina { get; private set; }
			public Formula DopustenaKolicinaPoKrugu { get; private set; }
			public List<Preduvjet> preduvjeti { get; private set; }

			public Image slika { get; private set; }
			public string kod { get; private set; }
			public string grupa { get; private set; }
			protected string opis;

			public List<Ucinak> ucinci { get; private set; }

			public bool orbitalna { get; private set; }
			public bool instantEfekt { get; private set; }
			public bool ostaje { get; private set; }
			public bool ponavljaSe { get; private set; }
			public bool brod { get; private set; }

			public ZgradaInfo(int id, string ime, string grupa, Formula cijenaGradnje, 
				Formula dopustenaKolicina, Formula dopustenaKolicinaPoKrugu, Formula cijenaOdrzavanja,
				Image slika, string kod, string opis, List<Ucinak> ucinci, string svojstva, 
				List<Preduvjet> preduvjeti)
			{
				this.id = id;
				this._ime = ime;
				this.CijenaGradnje = cijenaGradnje;
				this.DopustenaKolicina = dopustenaKolicina;
				this.DopustenaKolicinaPoKrugu = dopustenaKolicinaPoKrugu;
				this.CijenaOdrzavanja = cijenaOdrzavanja;
				this.slika = slika;
				this.kod = kod;
				this.opis = opis;
				this.ucinci = ucinci;
				this.preduvjeti = preduvjeti;

				HashSet<string> svojstvaSet = new HashSet<string>();
				string[] svojstvaArray = svojstva.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
				foreach (string s in svojstvaArray)
					svojstvaSet.Add(s.Trim());

				orbitalna = svojstvaSet.Contains("ORBITALNA");
				instantEfekt = svojstvaSet.Contains("INSTANT");
				ostaje = !svojstvaSet.Contains("NE_OSTAJE");
				ponavljaSe = svojstvaSet.Contains("PONAVLJA_SE");
				brod = svojstvaSet.Contains("BROD");
			}

			public bool dostupna(Dictionary<string, double> varijable, long prisutnaKolicina)
			{
				foreach (Preduvjet pred in preduvjeti)
					if (!pred.zadovoljen(varijable))
						return false;

				if ((long)DopustenaKolicina.iznos(varijable) <= prisutnaKolicina)
					return false;

				return true;
			}

			public virtual string Ime
			{
				get { return Postavke.Jezik[Kontekst.Zgrade, _ime].tekst(); }
			}

			public virtual string Opis
			{
				get { return Postavke.Jezik[Kontekst.Zgrade, opis].tekst(); }
			}

			public override string ToString()
			{
				return Ime;
			}
		}

		#region Statičko
		public static List<ZgradaInfo> CivilneZgradeInfo = new List<ZgradaInfo>();
		public static List<ZgradaInfo> VojneZgradeInfo = new List<ZgradaInfo>();
		public static Dictionary<int, ZgradaInfo> ZgradaInfoID = new Dictionary<int, ZgradaInfo>();
		public static HashSet<string> Grupe = initGrupe();

		private static HashSet<string> initGrupe()
		{
			HashSet<string> grupe = new HashSet<string>();
			grupe.Add(Zvjezdojedac.Igra.Brodovi.DizajnZgrada.Grupa);

			return grupe;
		}

		public static void UcitajInfoZgrade(Dictionary<string, string> podaci, bool jeLiCivilna)
		{
			List<Ucinak> ucinci = new List<Ucinak>();
			for(int i = 0; podaci.ContainsKey("UCINAK" + i); i++)
				ucinci.Add(Ucinak.napraviUcinak(podaci["UCINAK" + i]));
			List<Preduvjet> preduvjeti = Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
			
			List<ZgradaInfo> popis = null;
			if (jeLiCivilna)
				popis = CivilneZgradeInfo;
			else
				popis = VojneZgradeInfo;

			ZgradaInfo zgradaInfo = new ZgradaInfo(
				SlijedeciId(),
				podaci["IME"],
				podaci["GRUPA"],
				Formula.IzStringa(podaci["CIJENA"]),
				Formula.IzStringa(podaci["KOLICINA"]),
				Formula.IzStringa(podaci["PO_KRUGU"]),
				Formula.IzStringa(podaci["ODRZAVANJE"]),
				Image.FromFile(podaci["SLIKA"]),
				podaci["KOD"],
				podaci["OPIS"],
				ucinci,
				podaci["SVOJSTVA"],
				preduvjeti);
			
			popis.Add(zgradaInfo);
			ZgradaInfoID.Add(zgradaInfo.id, zgradaInfo);
			Grupe.Add(podaci["GRUPA"]);
		}

		public static string ProcjenaVremenaGradnje(double poeniIndustrije, double ostatakGradnje, Zgrada.ZgradaInfo uGradnji, Igrac igrac)
		{
			if (uGradnji == null) return "";
			double cijena = uGradnji.CijenaGradnje.iznos(igrac.efekti);

			double brZgrada = (ostatakGradnje + poeniIndustrije) / cijena;

			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.Kolonija];
			Dictionary<string, double> vars = new Dictionary<string, double>();

			if (brZgrada >= 1) {
				long dopustenaKolicina = (long)Math.Min(
					uGradnji.DopustenaKolicina.iznos(igrac.efekti),
					uGradnji.DopustenaKolicinaPoKrugu.iznos(igrac.efekti));
				brZgrada = Fje.Ogranici(brZgrada, 0, dopustenaKolicina);

				vars.Add("BR_ZGRADA", brZgrada);
				return jezik["gradPoKrugu"].tekst(vars);
			}
			else {
				if (poeniIndustrije == 0)
					return jezik["gradNikad"].tekst();

				double brKrugova = (cijena - ostatakGradnje) / (double)poeniIndustrije;
				double zaokruzeno = Math.Ceiling(brKrugova * 10) / 10;
				long tmp = (long)Math.Ceiling(brKrugova * 10);

				vars.Add("BR_KRUGOVA", Math.Ceiling(brKrugova * 10) / 10);
				vars.Add("DECIMALA", ((long)Math.Ceiling(brKrugova * 10)) % 10);

				if (brKrugova < 10)
					return jezik["gradVrijemeDec"].tekst(vars);
				else
					return jezik["gradVrijemePref"].tekst(vars);
			}
		}

		private static int _SlijedeciId = 0;
		public static int SlijedeciId()
		{
			return ++_SlijedeciId;
		}
		public static int ZadnjiId()
		{
			return _SlijedeciId;
		}
		#endregion

		public ZgradaInfo tip { get; private set; }
		public long kolicina;

		public Zgrada(ZgradaInfo tip, long kolicina)
		{
			this.tip = tip;
			this.kolicina = kolicina;
		}

		public void djeluj(AGradiliste gradiliste, Dictionary<string, double> varijable)
		{
			varijable["BR_ZGRADA"] = kolicina;
			foreach (Ucinak u in tip.ucinci)
				u.djeluj(gradiliste, varijable);
		}

		public void noviKrug(AGradiliste gradiliste, Dictionary<string, double> varijable)
		{
			varijable["BR_ZGRADA"] = kolicina;
			foreach (Ucinak u in tip.ucinci)
				u.noviKrug(gradiliste, varijable);
		}

		public override string ToString()
		{
			return tip.Ime;
		}

		#region Pohrana
		public const string PohranaTip = "ZGRADA";
		private const string PohTip = "TIP";
		private const string PohKolicina = "KOL";
		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohTip, tip);
			izlaz.dodaj(PohKolicina, kolicina);
		}

		public static Zgrada Ucitaj(PodaciCitac ulaz)
		{
			ZgradaInfo tip = ZgradaInfoID[ulaz.podatakInt(PohTip)];
			long kolicina = ulaz.podatakLong(PohKolicina);
			return new Zgrada(tip, kolicina);
		}
		#endregion
	}
}
