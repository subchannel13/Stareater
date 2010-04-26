using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Prototip
{
	public class Podaci
	{
		#region Nestatički članovi
		private StreamReader fajla;

		public Dictionary<string, string> podatci;

		public string tip;
		private int linijaPocetak;
		private int linijaKraj;
		private int linijaTrenutno;

		public Podaci(string datoteka)
		{
			this.podatci = new Dictionary<string, string>();
			this.fajla = new StreamReader(datoteka);
			this.tip = null;
			linijaPocetak = 0;
			linijaKraj = 0;
			linijaTrenutno = -1;
		}

		public bool dalje()
		{
			podatci.Clear();
			tip = procitajSlijedece(fajla);

			if (tip == null)
				return false;
			return true;
		}

		private string procitajSlijedece(StreamReader input)
		{
			string linija = "", svojstvo, vrijednost;
			string ret = null;
			while (!input.EndOfStream)
			{
				linija = input.ReadLine().Trim();
				linijaTrenutno++;

				if (linija.StartsWith("#"))
					continue;

				if (linija.Equals("----"))
				{
					linijaKraj = linijaTrenutno;
					return ret;
				}

				if (Regex.IsMatch(linija, "[<].*[>]"))
				{
					ret = linija;
					linijaPocetak = linijaTrenutno;
				}
				else if (Regex.IsMatch(linija, ".+[=].*"))
				{
					svojstvo = linija.Substring(0, linija.IndexOf('=')).Trim().ToUpper();
					vrijednost = linija.Substring(linija.IndexOf('=') + 1).Trim();
					podatci[svojstvo] = vrijednost;
				}
			}

			return null;
		}

		public void zatvori()
		{
			fajla.Close();
		}
		#endregion

		public static void spremi(StreamWriter datoteka, Dictionary<string, string> podatci, string razred)
		{
			datoteka.WriteLine("<" + razred + ">");
			foreach (string kljuc in podatci.Keys)
				datoteka.WriteLine(kljuc + "=" + podatci[kljuc]);
			datoteka.WriteLine("----");
		}

		#region Zvjezđa
		public enum PrefiksZvijezdja
		{
			ALFA,
			BETA,
			GAMA,
			DELTA,
			EPSILON,
			ZETA,
			ETA,
			THETA
		}

		public static Dictionary<PrefiksZvijezdja, string> prefiksZvijezdja = new Dictionary<PrefiksZvijezdja, string>();

		public class Zvijezdje
		{
			public string nominativ;

			public string genitiv;

			public Zvijezdje(string nominativ, string genitiv)
			{
				this.nominativ = nominativ;
				this.genitiv = genitiv;
			}

			public static Zvijezdje napravi(Dictionary<string, string> podaci)
			{
				return new Zvijezdje(podaci["NOMINATIV"], podaci["GENITIV"]);
			}
		}

		public static List<Zvijezdje> zvijezdja = new List<Zvijezdje>();
		#endregion

		public static Dictionary<string, Formula> ucitajBazuEfekata()
		{
			Podaci citac = new Podaci("./podaci/osnovni_efekti.txt");
			Dictionary<string, Formula> ret = new Dictionary<string, Formula>();

			if (citac.dalje())
			{
				foreach (string efekt in citac.podatci.Keys)
					ret.Add(efekt, Formula.IzStringa(citac.podatci[efekt]));
			}

			return ret;
		}
        public static NumberFormatInfo DecimalnaTocka = napraviTockaNFI();

        private static NumberFormatInfo napraviTockaNFI()
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return nfi;
        }

		private static Podaci citac = null;
		/*private static void ucitajBrodove()
		{
			citac = new Podaci("podaci/brod_trup.txt");
			while (citac.dalje())
				if (citac.tip == "<TRUP>")
					Trup.TrupInfo.UcitajTrupInfo(citac.podatci);
			citac.zatvori();

			citac = new Podaci("podaci/brod_oklop.txt");
			while (citac.dalje())
				if (citac.tip == "<OKLOP>")
					Oklop.OklopInfo.UcitajOklopInfo(citac.podatci);
			citac.zatvori();

			citac = new Podaci("podaci/brod_MZpogon.txt");
			while (citac.dalje())
				if (citac.tip == "<MZ_POGON>")
					MZPogon.MZPogonInfo.UcitajMZPogonInfo(citac.podatci);
			citac.zatvori();

			citac = new Podaci("podaci/brod_reaktor.txt");
			while (citac.dalje())
				if (citac.tip == "<REAKTOR>")
					Reaktor.ReaktorInfo.UcitajReaktorInfo(citac.podatci);
			citac.zatvori();

			citac = new Podaci("podaci/brod_oruzje.txt");
			while (citac.dalje())
				if (citac.tip == "<ORUZJE>")
					Oruzje.OruzjeInfo.UcitajOruzjeInfo(citac.podatci);
			citac.zatvori();

			citac = new Podaci("podaci/brod_potisnici.txt");
			while (citac.dalje())
				if (citac.tip == "<POTISNIK>")
					Potisnici.PotisnikInfo.UcitajPotisnikInfo(citac.podatci);
			citac.zatvori();

			citac = new Podaci("podaci/brod_senzor.txt");
			while (citac.dalje())
				if (citac.tip == "<SENZOR>")
					Senzor.SenzorInfo.UcitajSenzorInfo(citac.podatci);
			citac.zatvori();

			citac = new Podaci("podaci/brod_spec_oprema.txt");
			while (citac.dalje())
				if (citac.tip == "<SPEC_OPREMA>")
					SpecijalnaOprema.SpecijalnaOpremaInfo.UcitajSpecijalnaOpremaInfo(citac.podatci);
			citac.zatvori();

			citac = new Podaci("podaci/brod_stit.txt");
			while (citac.dalje())
				if (citac.tip == "<STIT>")
					Stit.StitInfo.UcitajStitInfo(citac.podatci);
			citac.zatvori();

			citac = new Podaci("podaci/taktike.txt");
			while (citac.dalje())
				if (citac.tip == "<TAKTIKA_PROJEKTIL>")
					Taktika.DodajTaktikuProjektila(citac.podatci);
				else if (citac.tip == "<TAKTIKA_BROD>")
					Taktika.DodajTaktikuBroda(citac.podatci);
			citac.zatvori();
		}*/
		/*private static void ucitajMape()
		{
			citac = new Podaci("./podaci/mapa.txt");

			while(citac.dalje())
				if (citac.tip == "<VELICINA MAPE>")
					Mapa.dodajVelicinuMape(citac.podatci);
			citac.zatvori();
		}*/
		/*private static void ucitajOrganizacije()
		{
			citac = new Podaci("./podaci/organizacije.txt");

			while (citac.dalje())
				if (citac.tip == "<ORGANIZACIJA>")
					Organizacije.dodajOrganizaciju(citac.podatci);
			citac.zatvori();
		}*/
		/*private static void ucitajPlanete()
		{
			citac = new Podaci("./podaci/planeti.txt");

			while (citac.dalje())
				if (citac.tip == "<PLANET>")
					Planet.TipInfo.noviTip(citac.podatci);
			citac.zatvori();
		}*/
		/*private static void ucitajPocetnePozicije()
		{
			citac = new Podaci("pocetne_pozicije.txt");

			while (citac.dalje())
				if (citac.tip == "<POCETNA_POZICIJA>")
					PocetnaPozicija.novaKonfiguracija(citac.podatci);
			citac.zatvori();
		}*/
		private static void ucitajPostavke()
		{
			citac = new Podaci("postavke.txt");

			while (citac.dalje())
				if (citac.tip == "<PROSLA_IGRA>")
					Postavke.ProslaIgra.postavi(citac.podatci);
			citac.zatvori();
		}
		/*private static void ucitajPredefDizajnove()
		{
			citac = new Podaci("./podaci/predef_dizajnovi.txt");

			while (citac.dalje())
				if (citac.tip == "<DIZAJN>")
					PredefiniraniDizajn.Dodaj(citac.podatci);
			citac.zatvori();
		}*/
		private static void ucitajSlike()
		{
			citac = new Podaci("slika.txt");

			while (citac.dalje())
				if (citac.tip == "<SLIKA>")
					Slike.DodajSliku(citac.podatci);
			citac.zatvori();
		}
		/*private static void ucitajTehnologije()
		{
			citac = new Podaci("./podaci/teh_razvoj.txt");

			while (citac.dalje())
				if (citac.tip == "<TEHNOLOGIJA>")
					Tehnologija.TechInfo.dodajTehnologiju(citac.podatci, Tehnologija.Kategorija.RAZVOJ);
			citac.zatvori();

			citac = new Podaci("./podaci/teh_istrazivanje.txt");

			while (citac.dalje())
				if (citac.tip == "<TEHNOLOGIJA>")
					Tehnologija.TechInfo.dodajTehnologiju(citac.podatci, Tehnologija.Kategorija.ISTRAZIVANJE);
			citac.zatvori();
		}*/
		/*private static void ucitajZgrade()
		{
			citac = new Podaci("./podaci/zgrade_civ.txt");
			while (citac.dalje())
				if (citac.tip == "<ZGRADA>")
					Zgrada.ucitajInfoZgrade(citac.podatci, true);
			citac.zatvori();

			citac = new Podaci("./podaci/zgrade_voj.txt");
			while (citac.dalje())
				if (citac.tip == "<ZGRADA>")
					Zgrada.ucitajInfoZgrade(citac.podatci, false);
			citac.zatvori();
		}*/
		/*private static void ucitajZvijezde()
		{
			citac = new Podaci("./podaci/zvijezde.txt");

			while (citac.dalje())
				if (citac.tip == "<ZVIJEZDA>")
					Zvijezda.TipInfo.noviTip(citac.podatci);
			citac.zatvori();
		}*/
		private static void ucitajZvjezdja()
		{
			citac = new Podaci("./podaci/ime_zvijezde.txt");

			while (citac.dalje())
			{
				string tip = citac.tip;
				if (tip == "<PREFIKS>")
				{
					prefiksZvijezdja.Clear();
					prefiksZvijezdja[PrefiksZvijezdja.ALFA] = citac.podatci["ALFA"];
					prefiksZvijezdja[PrefiksZvijezdja.BETA] = citac.podatci["BETA"];
					prefiksZvijezdja[PrefiksZvijezdja.GAMA] = citac.podatci["GAMA"];
					prefiksZvijezdja[PrefiksZvijezdja.DELTA] = citac.podatci["DELTA"];
					prefiksZvijezdja[PrefiksZvijezdja.EPSILON] = citac.podatci["EPSILON"];
					prefiksZvijezdja[PrefiksZvijezdja.ZETA] = citac.podatci["ZETA"];
					prefiksZvijezdja[PrefiksZvijezdja.ETA] = citac.podatci["ETA"];
					prefiksZvijezdja[PrefiksZvijezdja.THETA] = citac.podatci["THETA"];
				}
				else if (tip == "<ZVIJEZDJE>")
					zvijezdja.Add(Zvijezdje.napravi(citac.podatci));
			}
			citac.zatvori();
		}

		private static List<Dictionary<string, string>> pokupi(string datoteka, string tag)
		{
			List<Dictionary<string, string>> rez = new List<Dictionary<string, string>>();
			citac = new Podaci(datoteka);

			while (citac.dalje())
				if (citac.tip == tag)
					rez.Add(new Dictionary<string, string>(citac.podatci));
			citac.zatvori();

			return rez;
		}

		public const string MapaTag = "MAPA";
		public const string MZpogonTag = "MZ_POGON";
		public const string OklopTag = "OKLOP";
		public const string OrgTag = "ORG";
		public const string OruzjeTag = "ORUZJE";
		public const string PlanetTag = "PLANET";
		public const string PocetnaPozTag = "POCETNA_POZ";
		public const string PotisnikTag = "POTISNIK";
		public const string PredefDizjanTag = "PREDEF_DIZ";
		public const string ReaktorTag = "REAKTOR";
		public const string SenzorTag = "SENZOR";
		public const string SpecOpremaTag = "SPEC_OPREMA";
		public const string StitTag = "STIT";
		public const string TaktikaProjTag = "TAKTIKA_PROJEKTIL";
		public const string TaktikaBrodTag = "TAKTIKA_BROD";
		public const string TehnoIstTag = "TEH_IST";
		public const string TehnoRazTag = "TEH_RAZ";
		public const string TrupTag = "TRUP";
		public const string ZgradeCivTag = "ZGRADE_CIV";
		public const string ZgradVojTag = "ZGRADE_VOJ";
		public const string ZvijezdeTag = "ZVIJEZDE";

		public static Dictionary<string, List<Dictionary<string, string>>> ucitajPodatke()
		{
			Dictionary<string, List<Dictionary<string, string>>> ret = new Dictionary<string, List<Dictionary<string, string>>>();

			ret.Add(ZvijezdeTag, pokupi("./podaci/zvijezde.txt", "<ZVIJEZDA>"));
			ret.Add(ZgradeCivTag, pokupi("./podaci/zgrade_civ.txt", "<ZGRADA>"));
			ret.Add(ZgradVojTag, pokupi("./podaci/zgrade_voj.txt", "<ZGRADA>"));
			ret.Add(TehnoRazTag, pokupi("./podaci/teh_razvoj.txt", "<TEHNOLOGIJA>"));
			ret.Add(TehnoIstTag, pokupi("./podaci/teh_istrazivanje.txt", "<TEHNOLOGIJA>"));
			ret.Add(PredefDizjanTag, pokupi("./podaci/predef_dizajnovi.txt", "<DIZAJN>"));
			ret.Add(PocetnaPozTag, pokupi("./podaci/pocetne_pozicije.txt", "<POCETNA_POZICIJA>"));
			ret.Add(PlanetTag, pokupi("./podaci/planeti.txt", "<PLANET>"));
			ret.Add(OrgTag, pokupi("./podaci/organizacije.txt", "<ORGANIZACIJA>"));
			ret.Add(MapaTag, pokupi("./podaci/mapa.txt", "<VELICINA MAPE>"));

			ret.Add(TrupTag, pokupi("./podaci/brod_trup.txt", "<TRUP>"));
			ret.Add(OklopTag, pokupi("./podaci/brod_oklop.txt", "<OKLOP>"));
			ret.Add(MZpogonTag, pokupi("./podaci/brod_MZpogon.txt", "<MZ_POGON>"));
			ret.Add(ReaktorTag, pokupi("./podaci/brod_reaktor.txt", "<REAKTOR>"));
			ret.Add(OruzjeTag, pokupi("./podaci/brod_oruzje.txt", "<ORUZJE>"));
			ret.Add(PotisnikTag, pokupi("./podaci/brod_potisnici.txt", "<POTISNIK>"));
			ret.Add(SenzorTag, pokupi("./podaci/brod_senzor.txt", "<SENZOR>"));
			ret.Add(SpecOpremaTag, pokupi("./podaci/brod_spec_oprema.txt", "<SPEC_OPREMA>"));
			ret.Add(StitTag, pokupi("./podaci/brod_stit.txt", "<STIT>"));
			ret.Add(TaktikaProjTag, pokupi("./podaci/taktike.txt", "<TAKTIKA_PROJEKTIL>"));
			ret.Add(TaktikaBrodTag, pokupi("./podaci/taktike.txt", "<TAKTIKA_BROD>"));

			return ret;
		}

		public static void postaviPodatke()
		{
#if !DEBUG
			try
			{
#endif
				Dictionary<string, List<Dictionary<string, string>>> podaci = ucitajPodatke();
				ucitajPostavke();
				foreach (Dictionary<string, string> unos in podaci[MapaTag]) Mapa.dodajVelicinuMape(unos);
				foreach (Dictionary<string, string> unos in podaci[OrgTag]) Organizacije.dodajOrganizaciju(unos);
				foreach (Dictionary<string, string> unos in podaci[PlanetTag]) Planet.TipInfo.noviTip(unos);
				
				ucitajSlike();
				foreach (Dictionary<string, string> unos in podaci[TehnoRazTag]) Tehnologija.TechInfo.Dodaj(unos, Tehnologija.Kategorija.RAZVOJ);
				foreach (Dictionary<string, string> unos in podaci[TehnoIstTag]) Tehnologija.TechInfo.Dodaj(unos, Tehnologija.Kategorija.ISTRAZIVANJE);
				foreach (Dictionary<string, string> unos in podaci[ZgradeCivTag]) Zgrada.ucitajInfoZgrade(unos, true);
				foreach (Dictionary<string, string> unos in podaci[ZgradVojTag]) Zgrada.ucitajInfoZgrade(unos, false);
				foreach (Dictionary<string, string> unos in podaci[ZvijezdeTag]) Zvijezda.TipInfo.noviTip(unos);
				ucitajZvjezdja();
				foreach (Dictionary<string, string> unos in podaci[PocetnaPozTag]) PocetnaPozicija.novaKonfiguracija(unos);
				
				foreach (Dictionary<string, string> unos in podaci[TrupTag]) Trup.TrupInfo.UcitajTrupInfo(unos);
				foreach (Dictionary<string, string> unos in podaci[OklopTag]) Oklop.OklopInfo.UcitajOklopInfo(unos);
				foreach (Dictionary<string, string> unos in podaci[MZpogonTag]) MZPogon.MZPogonInfo.UcitajMZPogonInfo(unos);
				foreach (Dictionary<string, string> unos in podaci[ReaktorTag]) Reaktor.ReaktorInfo.UcitajReaktorInfo(unos);
				foreach (Dictionary<string, string> unos in podaci[OruzjeTag]) Oruzje.OruzjeInfo.UcitajOruzjeInfo(unos);
				foreach (Dictionary<string, string> unos in podaci[PotisnikTag]) Potisnici.PotisnikInfo.UcitajPotisnikInfo(unos);
				foreach (Dictionary<string, string> unos in podaci[SenzorTag]) Senzor.SenzorInfo.UcitajSenzorInfo(unos);
				foreach (Dictionary<string, string> unos in podaci[SpecOpremaTag]) SpecijalnaOprema.SpecijalnaOpremaInfo.UcitajSpecijalnaOpremaInfo(unos);
				foreach (Dictionary<string, string> unos in podaci[StitTag]) Stit.StitInfo.UcitajStitInfo(unos);
				foreach (Dictionary<string, string> unos in podaci[TaktikaProjTag]) Taktika.DodajTaktikuProjektila(unos);
				foreach (Dictionary<string, string> unos in podaci[TaktikaBrodTag]) Taktika.DodajTaktikuBroda(unos);

				foreach (Dictionary<string, string> unos in podaci["PREDEF_DIZ"]) PredefiniraniDizajn.Dodaj(unos);
#if !DEBUG
			}
			catch (FileNotFoundException e)
			{
				throw new Exception("Ne mogu otvoriti datoteku " + e.FileName);
			}
			catch (System.IO.IOException e)
			{
				throw new Exception("Neispravan sadržaj datoteke.\n\n" + e.Message);
			}
#endif
		}
	}
}
