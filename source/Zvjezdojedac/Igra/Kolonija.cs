using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Igra.Poruke;

namespace Zvjezdojedac.Igra
{
	public class Kolonija : AGradiliste, IPohranjivoSB
	{
		#region Ključevi efekata
		public const string PopulacijaBr = "POP";
		public const string PopulacijaMax = "POP_MAX";
		public const string PopulacijaPromjena = "POP_DELTA";
		public const string PopulacijaVisak = "POP_VISAK";
		public const string RadnaMjesta = "RAD_MJ_UK";
		public const string RadnaMjestaDelta = "RAD_MJ_DELTA";
		public const string AktivnaRadnaMjesta = "RAD_MJ";
		public const string FaktorCijeneOrbitalnih = "ORBITALNI_KOEF";
		public const string MigracijaMax = "MIGRACIJA_MAX"; 

		public const string RudePovrsinske = "RUDE_POV";
		public const string RudeDubinske = "RUDE_DUB";
		public const string RudeDubina = "RUDE_DUBINA";
		public const string RudeEfektivno = "RUDE_EFEKTIVNO";

		public const string VelicinaPlaneta = "PLANET_VELICINA";
		public const string Gravitacija = "GRAV";
		public const string Zracenje = "ZRACENJE";
		public const string Temperatura = "TEMP";
		public const string AtmKvaliteta = "ATM_KVAL";
		public const string AtmGustoca = "ATM_GUST";
		public const string NedostupanDioPlaneta = "NEDOSTUPNO";

		public const string TeraformGravitacija = "TERAFORM_GRAV";
		public const string TeraformZracenje = "TERAFORM_ZRACENJE";
		public const string TeraformTemperatura = "TERAFORM_TEMP";
		public const string TeraformAtmKvaliteta = "TERAFORM_ATM_KVAL";
		public const string TeraformAtmGustoca = "TERAFORM_ATM_GUST";

		public const string OdrzavanjeGravitacija = "ODR_GRAV";
		public const string OdrzavanjeZracenje = "ODR_ZRACENJE";
		public const string OdrzavanjeTemperatura = "ODR_TEMP";
		public const string OdrzavanjeAtmKvaliteta = "ODR_ATM_KVAL";
		public const string OdrzavanjeAtmGustoca = "ODR_ATM_GUST";
		public const string OdrzavanjeZgrada = "ODR_ZGRADA";
		public const string OdrzavanjeUkupno = "ODR_UKUPNO";

		public const string HranaPoFarmeru = "HRANA_PO_FARM";
		public const string IndustrijaPoRadniku = "IND_PO_RAD";
		public const string RazvojPoRadniku = "RAZVOJ_PO_RAD";
		public const string RudePoRudaru = "RUDE_PO_RUD";
		
		public const string RudariPoGraditelju = "RUDARI_PO_IND";
		public const string RudariPoOdrzavatelju = "RUDARI_PO_ODRZ";
		public const string RudariPoZnanstveniku = "RUDARI_PO_RAZ";

		public const string IndPoRadnikuEfektivno = "IND_PO_RAD_EF";
		public const string RazPoRadnikuEfektivno = "RAZ_PO_RAD_EF";

		public const string BrFarmera = "BR_FARMERA";
		public const string BrOdrzavatelja = "BR_ODRZAVATELJA";
		public const string BrRadnika = "BR_RADNIKA";
		#endregion

		public Planet planet { get; private set; }

		private long populacija;
		private long radnaMjesta;
		private double udioGradnje;

		public Kolonija(Igrac igrac, Planet planet, long populacija, long radnaMjesta)
			: base(igrac)
		{
			this.planet = planet;
			this.populacija = populacija;
			this.radnaMjesta = radnaMjesta;
			this.udioGradnje = 0;

			inicijalizirajEfekte();
			izracunajEfekte();
		}

		private Kolonija(Igrac igrac, Planet planet, long populacija, long radnaMjesta,
			double udioCivilneIndustrije, List<Zgrada> zgrade, Dictionary<string, double> ostatakGradnje,	
			LinkedList<Zgrada.ZgradaInfo> redGradnje)
			: base(igrac, redGradnje, ostatakGradnje)
		{
			this.planet = planet;
			this.populacija = populacija;
			this.radnaMjesta = radnaMjesta;
			this.udioGradnje = udioCivilneIndustrije;
			//this.ostatakGradnje = ostatakCivilneGradnje;
			
			foreach (Zgrada zgrada in zgrade)
				this.Zgrade.Add(zgrada.tip, zgrada);
		}

		/// <summary>
		/// Nanovo izračunava efekte za trenutno stanje.
		/// Ne utječe na stanje kolonije.
		/// </summary>
		public void resetirajEfekte()
		{
			inicijalizirajEfekte();
			izracunajEfekte();
		}

		public Dictionary<string, double> maxEfekti()
		{
			Dictionary<string, double> rez = new Dictionary<string, double>();
			inicijalizirajEfekte(rez);
			
			rez[PopulacijaBr] = rez[PopulacijaMax];
			rez[RadnaMjesta] = rez[PopulacijaBr];
			rez[AktivnaRadnaMjesta] = rez[RadnaMjesta];

			izracunajEfekte(rez);
			return rez;
		}

		private void inicijalizirajEfekte()
		{
			inicijalizirajEfekte(Efekti);
		}

		private void inicijalizirajEfekte(Dictionary<string, double> efekti)
		{
			efekti[PopulacijaBr] = populacija;
			efekti[PopulacijaMax] = 10000000 * (Math.Pow(planet.velicina, 1.5));
			efekti[PopulacijaPromjena] = populacija * Igrac.efekti["NATALITET"];
			efekti[RadnaMjesta] = radnaMjesta;
			efekti[RadnaMjestaDelta] = 0;
			efekti[AktivnaRadnaMjesta] = Math.Min(populacija, radnaMjesta);
			efekti[MigracijaMax] = 0;
			if (!efekti.ContainsKey(PopulacijaVisak))
				efekti[PopulacijaVisak] = 0;

			efekti[RudeDubina] = (planet.tip == Planet.Tip.ASTEROIDI) ? 1 : Igrac.efekti["DUBINA_RUDARENJA"];
			efekti[RudeDubinske] = planet.mineraliDubinski;
			efekti[RudeEfektivno] = Fje.IzIntervala(efekti[RudeDubina], planet.mineraliPovrsinski, planet.mineraliDubinski);
			efekti[RudePovrsinske] = planet.mineraliPovrsinski;

			efekti[VelicinaPlaneta] = planet.velicina;
			efekti[Gravitacija] = planet.gravitacija();
			efekti[Zracenje] = planet.ozracenost();
			efekti[Temperatura] = planet.temperatura();
			efekti[AtmGustoca] = planet.gustocaAtmosfere;
			efekti[AtmKvaliteta] = planet.kvalitetaAtmosfere;
			efekti[NedostupanDioPlaneta] = 1;

			efekti[TeraformGravitacija] = 0;
			efekti[TeraformZracenje] = 0;
			efekti[TeraformTemperatura] = 0;
			efekti[TeraformAtmGustoca] = 0;
			efekti[TeraformAtmKvaliteta] = 0;
		}

		private void izracunajEfekte()
		{
			izracunajEfekte(Efekti);
		}

		private void izracunajEfekte(Dictionary<string, double> efekti)
		{
			postaviEfekteIgracu();
			foreach (Zgrada z in Zgrade.Values)
				z.djeluj(this, Igrac.efekti);

			efekti[Gravitacija] = planet.gravitacija() + efekti[TeraformGravitacija] * Math.Sign(Igrac.efekti["OPTIMUM_GRAVITACIJA"] - planet.gravitacija());
			efekti[Zracenje] = planet.ozracenost() + efekti[TeraformZracenje] * Math.Sign(Igrac.efekti["OPTIMUM_ZRACENJE"] - planet.gravitacija());
			efekti[Temperatura] = planet.temperatura() + efekti[TeraformTemperatura] * Math.Sign(Igrac.efekti["OPTIMUM_TEMP_ATM"] - planet.gravitacija());
			efekti[AtmGustoca] = planet.gustocaAtmosfere + efekti[TeraformAtmGustoca] * Math.Sign(Igrac.efekti["OPTIMUM_GUST_ATM"] - planet.gravitacija());
			efekti[AtmKvaliteta] = planet.kvalitetaAtmosfere + efekti[TeraformAtmKvaliteta] * Math.Sign(Igrac.efekti["OPTIMUM_KVAL_ATM"] - planet.gravitacija());

			double odstupGravitacije = Math.Pow(Math.Abs(efekti[Gravitacija] - Igrac.efekti["OPTIMUM_GRAVITACIJA"]), 2);
			double odstupZracenja = Math.Pow(Math.Abs(efekti[Zracenje] - Igrac.efekti["OPTIMUM_ZRACENJE"]), 1);
			double odstupTemperature = Math.Pow(Math.Abs(efekti[Temperatura] - Igrac.efekti["OPTIMUM_TEMP_ATM"]), 1);
			double odstupAtmGustoce = Math.Pow(Math.Abs(efekti[AtmGustoca] - Igrac.efekti["OPTIMUM_GUST_ATM"]), 1);
			double odstupAtmKvalitete = Math.Pow(Math.Abs(efekti[AtmKvaliteta] - Igrac.efekti["OPTIMUM_KVAL_ATM"]), 2);
			
			odstupAtmKvalitete *= Math.Min(efekti[AtmGustoca], Igrac.efekti["OPTIMUM_GUST_ATM"]);

			efekti[OdrzavanjeGravitacija] = Igrac.efekti["ODRZAVANJE_GRAVITACIJA"] * efekti[PopulacijaBr] * odstupGravitacije;
			efekti[OdrzavanjeZracenje] = Igrac.efekti["ODRZAVANJE_ZRACENJE"] * efekti[PopulacijaBr] * odstupZracenja;
			efekti[OdrzavanjeTemperatura] = Igrac.efekti["ODRZAVANJE_TEMP_ATM"] * efekti[PopulacijaBr] * odstupTemperature;
			efekti[OdrzavanjeAtmGustoca] = Igrac.efekti["ODRZAVANJE_GUST_ATM"] * efekti[PopulacijaBr] * odstupAtmGustoce;
			efekti[OdrzavanjeAtmKvaliteta] = Igrac.efekti["ODRZAVANJE_KVAL_ATM"] * efekti[PopulacijaBr] * odstupAtmKvalitete;
			efekti[OdrzavanjeUkupno] = efekti[OdrzavanjeGravitacija] + efekti[OdrzavanjeZracenje] + efekti[OdrzavanjeTemperatura] + efekti[OdrzavanjeAtmKvaliteta] + efekti[OdrzavanjeAtmGustoca];

			efekti[OdrzavanjeZgrada] = 0;
			foreach (Zgrada zgrada in Zgrade.Values)
				efekti[OdrzavanjeZgrada] += zgrada.tip.CijenaOdrzavanja.iznos(efekti);
			efekti[OdrzavanjeUkupno] += efekti[OdrzavanjeZgrada];

			double zaposlenost = efekti[AktivnaRadnaMjesta] / (double)efekti[PopulacijaBr];
			efekti[HranaPoFarmeru] = Fje.IzIntervala(zaposlenost, Igrac.efekti["HRANA_PO_STANOVNIKU"], Igrac.efekti["HRANA_PO_FARMERU"]);
			efekti[RudePoRudaru] = Fje.IzIntervala(zaposlenost, Igrac.efekti["MINERALI_PO_STANOVNIKU"], Igrac.efekti["MINERALI_PO_RUDNIKU"]) * efekti[RudeEfektivno];

			efekti[IndustrijaPoRadniku] = Fje.IzIntervala(zaposlenost, Igrac.efekti["INDUSTRIJA_PO_STANOVNIKU"], Igrac.efekti["INDUSTRIJA_PO_TVORNICI"]);
			efekti[RazvojPoRadniku] = Fje.IzIntervala(zaposlenost, Igrac.efekti["RAZVOJ_PO_STANOVNIKU"], Igrac.efekti["RAZVOJ_PO_LABORATORIJU"]);

			efekti[RudariPoGraditelju] = efekti[IndustrijaPoRadniku] * Igrac.efekti["RUDE_PO_IND"] / efekti[RudePoRudaru];
			efekti[RudariPoOdrzavatelju] = efekti[RudariPoGraditelju] * Igrac.efekti["RUDE_ZA_ODRZAVANJE"];
			efekti[RudariPoZnanstveniku] = (efekti[RazvojPoRadniku] * Igrac.efekti["RUDE_ZA_RAZVOJ"] + Igrac.efekti["RUDE_PO_ZNAN"]) / efekti[RudePoRudaru];

			efekti[IndPoRadnikuEfektivno] = efekti[IndustrijaPoRadniku] / (1 + efekti[RudariPoGraditelju]);
			efekti[RazPoRadnikuEfektivno] = efekti[RazvojPoRadniku] / (1 + efekti[RudariPoZnanstveniku]);

			efekti[BrFarmera] = Math.Ceiling(efekti[PopulacijaBr] / efekti[HranaPoFarmeru]);
			efekti[BrOdrzavatelja] = Math.Ceiling(efekti[OdrzavanjeUkupno] / efekti[IndustrijaPoRadniku]);
			efekti[BrRadnika] = efekti[PopulacijaBr] - efekti[BrFarmera] - efekti[BrOdrzavatelja] * (1 + efekti[RudariPoOdrzavatelju]);

			if (efekti[BrRadnika] / efekti[PopulacijaBr] < Igrac.efekti["MIN_UDIO_RADNIKA"]) {
				efekti[BrRadnika] = efekti[PopulacijaBr] * Igrac.efekti["MIN_UDIO_RADNIKA"];
				efekti[BrOdrzavatelja] = (efekti[PopulacijaBr] - efekti[BrFarmera] - efekti[BrRadnika]) / (1 + efekti[RudariPoOdrzavatelju]);
			}

			efekti[FaktorCijeneOrbitalnih] = 1 + Math.Pow(planet.gravitacija(), 2) * 2 + planet.gustocaAtmosfere / 5;
			double negostoljubivost =
				odstupAtmGustoce * Igrac.efekti["VELICINA_GUST_ATM"] +
				odstupAtmKvalitete * Igrac.efekti["VELICINA_KVAL_ATM"] +
				odstupGravitacije * Igrac.efekti["VELICINA_GRAVITACIJA"] +
				odstupTemperature * Igrac.efekti["VELICINA_TEMP_ATM"] +
				odstupZracenja * Igrac.efekti["VELICINA_ZRACENJE"];

			efekti[NedostupanDioPlaneta] = Math.Pow(2, -negostoljubivost);

			efekti[PopulacijaMax] *= efekti[NedostupanDioPlaneta];
			efekti[PopulacijaMax] = Math.Floor(efekti[PopulacijaMax]);
		}

		private void osvjeziRedGradnje()
		{
			for (LinkedListNode<Zgrada.ZgradaInfo> uGradnji = RedGradnje.First; uGradnji != null; ) {
				Zgrada.ZgradaInfo zgradaTip = uGradnji.Value;
				long kolicina = 0;
				if (Zgrade.ContainsKey(zgradaTip)) kolicina = Zgrade[zgradaTip].kolicina;

				if (!zgradaTip.dostupna(Igrac.efekti, kolicina)) {
					if (zgradaTip.ponavljaSe)
						Igrac.poruke.AddLast(Poruka.NovaZgrada(this, zgradaTip));
					LinkedListNode<Zgrada.ZgradaInfo> slijedeci = uGradnji.Next;
					RedGradnje.Remove(uGradnji);
					uGradnji = slijedeci;
				}
				else
					uGradnji = uGradnji.Next;
			}
		}

		protected override void gradi(bool simulacija)
		{
			double pocetniPoeniGradnje = poeniIndustrije();
			double poeniGradnje = pocetniPoeniGradnje;
			LinkedListNode<Zgrada.ZgradaInfo> uGradnji = RedGradnje.First;
			
			Dictionary<string, double> ostatakGradnje;
			if (simulacija)
				ostatakGradnje = new Dictionary<string, double>(this.ostatakGradnje);
			else
				ostatakGradnje = this.ostatakGradnje;

			while (uGradnji != null && poeniGradnje > 0) {
				Zgrada.ZgradaInfo zgradaTip = uGradnji.Value;
				double cijena = zgradaTip.CijenaGradnje.iznos(Igrac.efekti);
				if (zgradaTip.orbitalna) cijena *= Efekti[FaktorCijeneOrbitalnih];
				poeniGradnje += ostatakGradnje[zgradaTip.grupa];

				long brZgrada = (long)(poeniGradnje / cijena);
				long dopustenaKolicina = (long)Math.Min(
					zgradaTip.DopustenaKolicina.iznos(Igrac.efekti),
					zgradaTip.DopustenaKolicinaPoKrugu.iznos(Igrac.efekti));

				if (brZgrada < dopustenaKolicina) {
					ostatakGradnje[zgradaTip.grupa] = poeniGradnje - brZgrada * cijena;
					poeniGradnje = 0;
				}
				else {
					brZgrada = dopustenaKolicina;
					poeniGradnje = poeniGradnje - (brZgrada * cijena - ostatakGradnje[zgradaTip.grupa]);
					ostatakGradnje[zgradaTip.grupa] = 0;
				}

				if (!simulacija) {
					if (brZgrada > 0) {
						Zgrada z = new Zgrada(zgradaTip, brZgrada);

						if (z.tip.instantEfekt)
							z.djeluj(this, Igrac.efekti);
						else {
							if (Zgrade.ContainsKey(z.tip))
								Zgrade[z.tip].kolicina += brZgrada;
							else
								Zgrade.Add(z.tip, z);
						}

						if (!z.tip.brod && !z.tip.ponavljaSe)
							Igrac.poruke.AddLast(Poruka.NovaZgrada(this, z.tip));
					}

					long brNovih = brZgrada;
					if (Zgrade.ContainsKey(zgradaTip))
						brZgrada = Zgrade[zgradaTip].kolicina;
					else
						brZgrada = 0;
				}

				uGradnji = uGradnji.Next;
			}

			this.UtroseniPoeniIndustrije = pocetniPoeniGradnje - poeniGradnje;
			this.UtrosenUdioIndustrije = UtroseniPoeniIndustrije / (Efekti[BrRadnika] * Efekti[IndPoRadnikuEfektivno]);
		}

		public void dodajKolonizator(long populacija, long radnaMjesta)
		{
			populacija = (long)Math.Min(populacija + populacija, Efekti[PopulacijaMax]);
			this.radnaMjesta = (long)Math.Min(this.radnaMjesta + radnaMjesta, Efekti[PopulacijaMax]);
		}

		public void postaviEfekteIgracu()
		{
			foreach (string s in Efekti.Keys)
				Igrac.efekti[s] = Efekti[s];
		}

		public double dodajMigrante(double imigrantiUkupno)
		{
			double imigranti = Math.Floor(Math.Min(imigrantiUkupno, Efekti[PopulacijaMax] - populacija));
			
			Efekti[PopulacijaPromjena] += imigranti;
			populacija += (long)imigranti;

			return imigranti;
		}

		public void NoviKrugPrviProlaz()
		{
			postaviEfekteIgracu();

			gradi(false);

			/*
			 * TODO: terraforming
			 */

			double populacijaTmp = Efekti[PopulacijaPromjena] + populacija;
			Efekti[PopulacijaVisak] = Math.Max(Efekti[PopulacijaMax] - populacijaTmp, 0);

			populacija = (long)Math.Min(populacijaTmp, Efekti[PopulacijaMax]);
		}

		public void NoviKrugDrugiProlaz()
		{
			radnaMjesta = (long)Math.Min(Efekti[RadnaMjestaDelta] + radnaMjesta, Efekti[PopulacijaMax]);

			List<Zgrada.ZgradaInfo> zaUklonit = new List<Zgrada.ZgradaInfo>();
			foreach (Zgrada z in Zgrade.Values) {
				z.noviKrug(this, Igrac.efekti);
				if (!z.tip.ostaje)
					zaUklonit.Add(z.tip);
			}

			foreach (var ukloni in zaUklonit)
				Zgrade.Remove(ukloni);

			inicijalizirajEfekte();
			izracunajEfekte();
			postaviEfekteIgracu();

			osvjeziRedGradnje();
		}

		public string ime
		{
			get
			{
				return planet.ime;
			}
		}

		public Image slika
		{
			get
			{
				return planet.slika;
			}
		}

		public override Zvijezda LokacijaZvj
		{
			get { return planet.zvjezda; }
		}

		public long poeniIndustrije()
		{
			return (long)(Efekti[BrRadnika] * Efekti[IndPoRadnikuEfektivno] * udioGradnje);
		}

		public long poeniRazvoja()
		{
			ZvjezdanaUprava uprava = LokacijaZvj.uprave[Igrac.id];
			return (long)(Efekti[BrRadnika] * Efekti[RazPoRadnikuEfektivno] * (1 - UtrosenUdioIndustrije) * (1 - uprava.UtrosenUdioIndustrije));
		}

		public override List<Zgrada.ZgradaInfo> MoguceGraditi()
		{
			List<Zgrada.ZgradaInfo> ret = new List<Zgrada.ZgradaInfo>();
			foreach (Zgrada.ZgradaInfo z in Zgrada.CivilneZgradeInfo) {
					long prisutnaKolicina = 0;
					if (Zgrade.ContainsKey(z))
						prisutnaKolicina = Zgrade[z].kolicina;
					if (z.dostupna(Igrac.efekti, prisutnaKolicina))
						ret.Add(z);
				}

			return ret;
		}

		public double UdioIndustrije
		{
			get
			{
				return udioGradnje;
			}
			set
			{
				bool osvjeziInfo = (udioGradnje != value);
				
				udioGradnje = value;
				if (osvjeziInfo)
					OsvjeziInfoGradnje();
			}
		}

		public long Populacija
		{
			get
			{
				return populacija;
			}
		}

		public double OdrzavanjePoStan
		{
			get { return Efekti[OdrzavanjeUkupno] / populacija; }
		}

		public double Razvijenost
		{
			get { return (Efekti[AktivnaRadnaMjesta] * 2 / 3.0 + populacija / 3) / Efekti[PopulacijaMax]; }
		}

		public string ProcjenaVremenaGradnje()
		{
			if (RedGradnje.First != null) {
				Zgrada.ZgradaInfo zgrada = RedGradnje.First.Value; 
				double faktorCijene = (zgrada.orbitalna) ? 
					1 / Efekti[FaktorCijeneOrbitalnih] : 
					1;
				
				return Zgrada.ProcjenaVremenaGradnje(
					poeniIndustrije() * faktorCijene, 
					ostatakGradnje[zgrada.grupa],
					zgrada, Igrac);
			}
			else
				return "";
		}

		#region Pohrana
		public const string PohranaTip = "KOLONIJA";
		private const string PohIgrac = "IGRAC";
		private const string PohZvijezda = "ZVJ";
		private const string PohPlanet = "PLANET";
		private const string PohPopulacija = "POP";
		private const string PohRadnaMj = "RADNA_MJ";
		private const string PohCivGradUdio = "UDIO_CIV";
		private const string PohCivGradOst = "CIV_OST";
		private const string PohCivGrad = "GRADNJA";
		private const string PohZgrada = "ZGRADA";
		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohIgrac, Igrac.id);
			izlaz.dodaj(PohZvijezda, planet.zvjezda.id);
			izlaz.dodaj(PohPlanet, planet.pozicija);
			izlaz.dodaj(PohPopulacija, populacija);
			izlaz.dodaj(PohRadnaMj, radnaMjesta);
			izlaz.dodaj(PohCivGradUdio, UdioIndustrije);
			izlaz.dodajRjecnik(PohCivGradOst, ostatakGradnje, x => (x > 0));

			izlaz.dodaj(PohZgrada, Zgrade.Count);
			izlaz.dodajKolekciju(PohZgrada, Zgrade.Values);

			izlaz.dodajIdeve(PohCivGrad, RedGradnje);
		}

		public static Kolonija Ucitaj(PodaciCitac ulaz, List<Igrac> igraci, 
			Dictionary<int, Zvijezda> zvijezde, 
			Dictionary<int, Zgrada.ZgradaInfo> zgradeInfoID)
		{
			Igrac igrac = igraci[ulaz.podatakInt(PohIgrac)];
			Planet planet = zvijezde[ulaz.podatakInt(PohZvijezda)].
				planeti[ulaz.podatakInt(PohPlanet)];
			long populacija = ulaz.podatakLong(PohPopulacija);
			long radnaMjesta = ulaz.podatakLong(PohRadnaMj);
			double civilnaInd = ulaz.podatakDouble(PohCivGradUdio);
			Dictionary<string, double> ostatakGradnje = ulaz.podatakDoubleRjecnik(PohCivGradOst);

			int brZgrada = ulaz.podatakInt(PohZgrada);
			List<Zgrada> zgrade = new List<Zgrada>();
			for (int i = 0; i < brZgrada; i++)
				zgrade.Add(Zgrada.Ucitaj(ulaz[PohZgrada + i]));

			int[] zgradeID  = ulaz.podatakIntPolje(PohCivGrad);
			LinkedList<Zgrada.ZgradaInfo> redCivilneGradnje = new LinkedList<Zgrada.ZgradaInfo>();
			for (int i = 0; i < zgradeID.Length; i++)
				redCivilneGradnje.AddLast(zgradeInfoID[zgradeID[i]]);

			return new Kolonija(igrac, planet, populacija, radnaMjesta, civilnaInd,
				zgrade, ostatakGradnje, redCivilneGradnje);
		}
		#endregion
	}
}
