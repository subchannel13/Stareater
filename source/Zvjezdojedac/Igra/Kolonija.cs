using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Alati;

namespace Prototip
{
	public class Kolonija : IPohranjivoSB
	{
		public const string Populacija = "POP";
		public const string PopulacijaMax = "POP_MAX";
		public const string PopulacijaPromjena = "POP_DELTA";
		public const string RadnaMjesta = "RAD_MJ_UK";
		public const string RadnaMjestaDelta = "RAD_MJ_DELTA";
		public const string AktivnaRadnaMjesta = "RAD_MJ";
		public const string FaktorCijeneOrbitalnih = "ORBITALNI_KOEF"; 

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
		public const string RudePoIndustriji = "RUDE_PO_IND";
		public const string RudePoRudaru = "RUDE_PO_RUD";

		public const string BrFarmera = "BR_FARMERA";
		public const string BrRudara = "BR_RUDARA";
		public const string BrOdrzavatelja = "BR_ODRZAVATELJA";
		public const string BrRadnika = "BR_RADNIKA";
		
		public Dictionary<string, double> efekti = new Dictionary<string, double>();

		public Igrac igrac;
		public Planet planet { get; private set; }

		private long _populacija;
		private long radnaMjesta;
		private double udioCivilneIndustrije;
		private double udioVojneIndustrije;

		public List<Zgrada> zgrade;

		public long ostatakCivilneGradnje;
		public long ostatakVojneGradnje;
		public LinkedList<Zgrada.ZgradaInfo> redCivilneGradnje;
		public LinkedList<Zgrada.ZgradaInfo> redVojneGradnje;

		public Kolonija(Igrac igrac, Planet planet, long populacija, long radnaMjesta)
		{
			this.igrac = igrac;
			this.planet = planet;
			this._populacija = populacija;
			this.radnaMjesta = radnaMjesta;
			this.udioVojneIndustrije = 0;
			this.udioCivilneIndustrije = 0.5;
			this.zgrade = new List<Zgrada>();
			this.redCivilneGradnje = new LinkedList<Zgrada.ZgradaInfo>();
			this.redVojneGradnje = new LinkedList<Zgrada.ZgradaInfo>();
			this.ostatakCivilneGradnje = 0;

			inicijalizirajEfekte();
			izracunajEfekte();
		}

		private Kolonija(Igrac igrac, Planet planet, long populacija, long radnaMjesta,
			double udioCivilneIndustrije, double udioVojneIndustrije, List<Zgrada> zgrade,
			long ostatakCivilneGradnje, long ostatakVojneGradnje,
			LinkedList<Zgrada.ZgradaInfo> redCivilneGradnje, LinkedList<Zgrada.ZgradaInfo> redVojneGradnje)
		{
			this.igrac = igrac;
			this.planet = planet;
			this._populacija = populacija;
			this.radnaMjesta = radnaMjesta;
			this.udioCivilneIndustrije = udioCivilneIndustrije;
			this.udioVojneIndustrije = udioVojneIndustrije;
			this.zgrade = zgrade;
			this.ostatakCivilneGradnje = ostatakCivilneGradnje;
			this.ostatakVojneGradnje = ostatakVojneGradnje;
			this.redCivilneGradnje = redCivilneGradnje;
			this.redVojneGradnje = redVojneGradnje;
		}

		public void resetirajEfekte()
		{
			inicijalizirajEfekte();
			izracunajEfekte();
		}

		private void inicijalizirajEfekte()
		{
			efekti[Populacija] = _populacija;
			efekti[PopulacijaMax] = 10000000 * (Math.Pow(planet.velicina, 1.5));
			efekti[PopulacijaPromjena] = _populacija * igrac.efekti["NATALITET"];
			efekti[RadnaMjesta] = radnaMjesta;
			efekti[RadnaMjestaDelta] = 0;
			efekti[AktivnaRadnaMjesta] = Math.Min(_populacija, radnaMjesta);

			efekti[RudeDubina] = igrac.efekti["DUBINA_RUDARENJA"];
			efekti[RudeDubinske] = planet.mineraliDubinski;
			efekti[RudeEfektivno] = Fje.IzIntervala(igrac.efekti["DUBINA_RUDARENJA"], planet.mineraliPovrsinski, planet.mineraliDubinski);
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
			efekti[Gravitacija] = planet.gravitacija() + efekti[TeraformGravitacija] * Math.Sign(igrac.efekti["OPTIMUM_GRAVITACIJA"] - planet.gravitacija());
			efekti[Zracenje] = planet.ozracenost() + efekti[TeraformZracenje] * Math.Sign(igrac.efekti["OPTIMUM_ZRACENJE"] - planet.gravitacija());
			efekti[Temperatura] = planet.temperatura() + efekti[TeraformTemperatura] * Math.Sign(igrac.efekti["OPTIMUM_TEMP_ATM"] - planet.gravitacija());
			efekti[AtmGustoca] = planet.gustocaAtmosfere + efekti[TeraformAtmGustoca] * Math.Sign(igrac.efekti["OPTIMUM_GUST_ATM"] - planet.gravitacija());
			efekti[AtmKvaliteta] = planet.kvalitetaAtmosfere + efekti[TeraformAtmKvaliteta] * Math.Sign(igrac.efekti["OPTIMUM_KVAL_ATM"] - planet.gravitacija());

			double odstupGravitacije = Math.Pow(Math.Abs(efekti[Gravitacija] - igrac.efekti["OPTIMUM_GRAVITACIJA"]), 2);
			double odstupZracenja = Math.Pow(Math.Abs(efekti[Zracenje] - igrac.efekti["OPTIMUM_ZRACENJE"]), 2);
			double odstupTemperature = Math.Pow(Math.Abs(efekti[Temperatura] - igrac.efekti["OPTIMUM_TEMP_ATM"]), 2);
			double odstupAtmGustoce = Math.Pow(Math.Abs(efekti[AtmGustoca] - igrac.efekti["OPTIMUM_GUST_ATM"]), 2);
			double odstupAtmKvalitete = Math.Pow(Math.Abs(efekti[AtmKvaliteta] - igrac.efekti["OPTIMUM_KVAL_ATM"]), 2);

			efekti[OdrzavanjeGravitacija] = igrac.efekti["ODRZAVANJE_GRAVITACIJA"] * _populacija * odstupGravitacije;
			efekti[OdrzavanjeZracenje] = igrac.efekti["ODRZAVANJE_ZRACENJE"] * _populacija * odstupZracenja;
			efekti[OdrzavanjeTemperatura] = igrac.efekti["ODRZAVANJE_TEMP_ATM"] * _populacija * odstupTemperature;
			efekti[OdrzavanjeAtmGustoca] = igrac.efekti["ODRZAVANJE_GUST_ATM"] * _populacija * odstupAtmGustoce;
			efekti[OdrzavanjeAtmKvaliteta] = igrac.efekti["ODRZAVANJE_KVAL_ATM"] * _populacija * odstupAtmKvalitete;
			efekti[OdrzavanjeUkupno] = efekti[OdrzavanjeGravitacija] + efekti[OdrzavanjeZracenje] + efekti[OdrzavanjeTemperatura] + efekti[OdrzavanjeAtmKvaliteta] + efekti[OdrzavanjeAtmGustoca];

			efekti[HranaPoFarmeru] = Fje.IzIntervala(efekti[AktivnaRadnaMjesta] / (double)_populacija, igrac.efekti["HRANA_PO_STANOVNIKU"], igrac.efekti["HRANA_PO_FARMERU"]);
			efekti[IndustrijaPoRadniku] = Fje.IzIntervala(efekti[AktivnaRadnaMjesta] / (double)_populacija, igrac.efekti["INDUSTRIJA_PO_STANOVNIKU"], igrac.efekti["INDUSTRIJA_PO_TVORNICI"]);
			efekti[RazvojPoRadniku] = Fje.IzIntervala(efekti[AktivnaRadnaMjesta] / (double)_populacija, igrac.efekti["RAZVOJ_PO_STANOVNIKU"], igrac.efekti["RAZVOJ_PO_LABORATORIJU"]);
			efekti[RudePoIndustriji] = 1 + Fje.IzIntervala(efekti[AktivnaRadnaMjesta] / (double)_populacija, igrac.efekti["MINERALI_PO_STANOVNIKU"], igrac.efekti["MINERALI_PO_RUDNIKU"]) * efekti[RudeEfektivno];
			efekti[RudePoRudaru] = efekti[RudePoIndustriji] * efekti[IndustrijaPoRadniku];

			efekti[BrFarmera] = Math.Ceiling(_populacija / efekti[HranaPoFarmeru]);
			efekti[BrOdrzavatelja] = Math.Ceiling(efekti[OdrzavanjeUkupno] / efekti[IndustrijaPoRadniku]);
			efekti[BrRudara] = Math.Ceiling((_populacija - efekti[BrFarmera]) / (1 + efekti[RudePoIndustriji]));
			efekti[BrRadnika] = _populacija - efekti[BrFarmera] - efekti[BrOdrzavatelja] - efekti[BrRudara];

			efekti[FaktorCijeneOrbitalnih] = 1 + Math.Pow(planet.gravitacija(), 2) + planet.gustocaAtmosfere;
			efekti[NedostupanDioPlaneta] = 1 +
				odstupAtmGustoce * igrac.efekti["VELICINA_GUST_ATM"] +
				odstupAtmKvalitete * planet.gustocaAtmosfere * igrac.efekti["VELICINA_KVAL_ATM"] +
				odstupGravitacije * igrac.efekti["VELICINA_GRAVITACIJA"] +
				odstupTemperature * igrac.efekti["VELICINA_TEMP_ATM"] +
				odstupZracenja * igrac.efekti["VELICINA_ZRACENJE"];

			efekti[PopulacijaMax] /= efekti[NedostupanDioPlaneta];
			efekti[OdrzavanjeZgrada] = 0;
			foreach (Zgrada zgrada in zgrade)
				efekti[OdrzavanjeZgrada] += zgrada.tip.cijenaOdrzavanja.iznos(efekti);
			efekti[OdrzavanjeUkupno] += efekti[OdrzavanjeZgrada];
		}

		private long gradi(long ostatakGradnje, long poeniIndustrije, LinkedList<Zgrada.ZgradaInfo> redGradnje)
		{
			ostatakGradnje += poeniIndustrije;
			LinkedListNode<Zgrada.ZgradaInfo> uGradnji = redGradnje.First;
			while (uGradnji != null)
			{
				double cijena = uGradnji.Value.cijenaGradnje.iznos(igrac.efekti);
				if (uGradnji.Value.orbitalna) cijena *= efekti[FaktorCijeneOrbitalnih];

				long brZgrada = (long)(ostatakGradnje / cijena);
				long dopustenaKolicina = (long)uGradnji.Value.dopustenaKolicina.iznos(igrac.efekti);
				brZgrada = Fje.Ogranici(brZgrada, 0, dopustenaKolicina);

				if (brZgrada > 0)
				{
					ostatakGradnje -= (long)(cijena * brZgrada);
					Zgrada z = new Zgrada(uGradnji.Value);

					if (z.tip.ostaje)
						zgrade.Add(z);
					else
					{
						igrac.efekti["BR_ZGRADA"] = brZgrada;
						z.djeluj(this, igrac.efekti);
					}
					if (!z.tip.ponavljaSe) redGradnje.Remove(uGradnji);
				}
				else if (dopustenaKolicina <= 0)
				{
					redGradnje.Remove(uGradnji);
					break;
				}

				uGradnji = uGradnji.Next;
			}

			return ostatakGradnje;
		}

		public void postaviEfekteIgracu()
		{
			foreach (string s in efekti.Keys)
				igrac.efekti[s] = efekti[s];
		}

		public void noviKrug()
		{
			postaviEfekteIgracu();

			ostatakCivilneGradnje = gradi(ostatakCivilneGradnje, poeniCivilneIndustrije(), redCivilneGradnje);
			ostatakVojneGradnje = gradi(ostatakVojneGradnje, poeniVojneIndustrije(), redVojneGradnje);

			_populacija = (long)Math.Min(efekti[PopulacijaPromjena] + _populacija, efekti[PopulacijaMax]);
			radnaMjesta += (long)efekti[RadnaMjestaDelta];

			inicijalizirajEfekte();

			foreach (Zgrada z in zgrade)
				z.djeluj(this, igrac.efekti);

			izracunajEfekte();
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

		public long poeniCivilneIndustrije()
		{
			return (long)(efekti[BrRadnika] * efekti[IndustrijaPoRadniku] * udioCivilneIndustrije);
		}

		public long poeniVojneIndustrije()
		{
			return (long)(efekti[BrRadnika] * efekti[IndustrijaPoRadniku] * udioVojneIndustrije);
		}

		public long poeniRazvoja()
		{
			return (long)(efekti[BrRadnika] * efekti[RazvojPoRadniku] * (1 - udioCivilneIndustrije - udioVojneIndustrije));
		}

		public List<Zgrada.ZgradaInfo> moguceGraditi(bool civilnaGradnja)
		{
			HashSet<Zgrada.ZgradaInfo> uRedu;
			List<Zgrada.ZgradaInfo> popis;
			if (civilnaGradnja)
			{
				uRedu = new HashSet<Zgrada.ZgradaInfo>(redCivilneGradnje);
				popis = Zgrada.civilneZgradeInfo;
			}
			else
			{
				uRedu = new HashSet<Zgrada.ZgradaInfo>(redVojneGradnje);
				popis = new List<Zgrada.ZgradaInfo>(Zgrada.vojneZgradeInfo);
				foreach (Zgrada.ZgradaInfo zi in igrac.dizajnoviBrodova)
					popis.Add(zi);
			}

			List<Zgrada.ZgradaInfo> ret = new List<Zgrada.ZgradaInfo>();
			foreach (Zgrada.ZgradaInfo z in popis)
				if (!uRedu.Contains(z))
					ret.Add(z);

			return ret;
		}

		public double civilnaIndustrija
		{
			get
			{
				return udioCivilneIndustrije;
			}
			set
			{
				udioCivilneIndustrije = Math.Min(value, 1 - udioVojneIndustrije);
			}
		}

		public double vojnaIndustrija
		{
			get
			{
				return udioVojneIndustrije;
			}
			set
			{
				udioVojneIndustrije = Math.Min(value, 1 - udioCivilneIndustrije);
			}
		}

		public long populacija
		{
			get
			{
				return _populacija;
			}
		}

		public string procjenaVremenaCivilneGradnje()
		{
			if (redCivilneGradnje.First != null)
				return procjenaVremenaGradnje(poeniCivilneIndustrije(), ostatakCivilneGradnje, redCivilneGradnje.First.Value);
			else
				return "";
		}

		public string procjenaVremenaVojneGradnje()
		{
			if (redVojneGradnje.First != null)
				return procjenaVremenaGradnje(poeniVojneIndustrije(), ostatakVojneGradnje, redVojneGradnje.First.Value);
			else
				return "";
		}

		private string procjenaVremenaGradnje(long poeniIndustrije, long ostatakGradnje, Zgrada.ZgradaInfo uGradnji)
		{
			if (uGradnji == null) return "";
			double cijena = uGradnji.cijenaGradnje.iznos(igrac.efekti);
			if (uGradnji.orbitalna) cijena *= efekti[FaktorCijeneOrbitalnih];

			long brZgrada = (long)((ostatakGradnje + poeniIndustrije) / cijena);
			if (brZgrada > 0)
			{
				long dopustenaKolicina = (long)uGradnji.dopustenaKolicina.iznos(igrac.efekti);
				brZgrada = Fje.Ogranici(brZgrada, 0, dopustenaKolicina);
				return Fje.PrefiksFormater(brZgrada) + " po krugu";
			}
			else
			{
				if (poeniIndustrije == 0)
					return "nikad";

				double brKrugova = (cijena - ostatakGradnje) / (double)poeniIndustrije;
				double zaokruzeno = Math.Ceiling(brKrugova * 10) / 10;
				long tmp = (long)Math.Ceiling(brKrugova * 10);
				if (tmp == 10 || tmp == 1)
					return "Za " + zaokruzeno.ToString("0.#") + " krug";
				else if (tmp <= 40)
					return "Za " + zaokruzeno.ToString("0.#") + " kruga";
				else if (tmp <= 100)
					return "Za " + zaokruzeno.ToString("0.#") + " krugova";
				else
					return "Za " + Fje.PrefiksFormater(brKrugova) + " krugova";
			}
		}

		#region Pohrana
		public const string PohranaTip = "KOLONIJA";
		private const string PohIgrac = "IGRAC";
		private const string PohZvijezda = "ZVJ";
		private const string PohPlanet = "PLANET";
		private const string PohPopulacija = "POP";
		private const string PohRadnaMj = "RADNA_MJ";
		private const string PohCivGradUdio = "UDIO_CIV";
		private const string PohVojGradUdio = "UDIO_VOJ";
		private const string PohCivGradOst = "CIV_OST";
		private const string PohVojGradOst = "VOJ_OST";
		private const string PohCivGrad = "CIV_GRAD";
		private const string PohVojGrad = "VOJ_GRAD";
		private const string PohZgrada = "ZGRADA";
		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohIgrac, igrac.id);
			izlaz.dodaj(PohZvijezda, planet.zvjezda.id);
			izlaz.dodaj(PohPlanet, planet.pozicija);
			izlaz.dodaj(PohPopulacija, populacija);
			izlaz.dodaj(PohRadnaMj, radnaMjesta);
			izlaz.dodaj(PohCivGradUdio, civilnaIndustrija);
			izlaz.dodaj(PohVojGradUdio, vojnaIndustrija);
			izlaz.dodaj(PohCivGradOst, ostatakCivilneGradnje);
			izlaz.dodaj(PohVojGradOst, ostatakVojneGradnje);

			izlaz.dodaj(PohZgrada, zgrade.Count);
			izlaz.dodajKolekciju(PohZgrada, zgrade);

			izlaz.dodajIdeve(PohCivGrad, redCivilneGradnje);
			izlaz.dodajIdeve(PohVojGrad, redVojneGradnje);
		}

		public static Kolonija Ucitaj(PodaciCitac ulaz, List<Igrac> igraci, Dictionary<int, Zvijezda> zvijezde)
		{
			Igrac igrac = igraci[ulaz.podatakInt(PohIgrac)];
			Planet planet = zvijezde[ulaz.podatakInt(PohZvijezda)].
				planeti[ulaz.podatakInt(PohPlanet)];
			long populacija = ulaz.podatakLong(PohPopulacija);
			long radnaMjesta = ulaz.podatakLong(PohRadnaMj);
			double civilnaInd = ulaz.podatakDouble(PohCivGradUdio);
			double vojnaInd = ulaz.podatakDouble(PohVojGradUdio);
			long ostatakCivilneGradnje = ulaz.podatakLong(PohCivGradOst);
			long ostatakVojneGradnje = ulaz.podatakLong(PohVojGradOst);

			int brZgrada = ulaz.podatakInt(PohZgrada);
			List<Zgrada> zgrade = new List<Zgrada>();
			for (int i = 0; i < brZgrada; i++)
				zgrade.Add(Zgrada.Ucitaj(ulaz[PohZgrada + i]));

			int[] zgradeID  = ulaz.podatakIntPolje(PohCivGrad);
			LinkedList<Zgrada.ZgradaInfo> redCivilneGradnje = new LinkedList<Zgrada.ZgradaInfo>();
			for (int i = 0; i < zgradeID.Length; i++)
				redCivilneGradnje.AddLast(Zgrada.ZgradaInfoID[zgradeID[i]]);

			zgradeID = ulaz.podatakIntPolje(PohVojGrad);
			LinkedList<Zgrada.ZgradaInfo> redVojneGradnje = new LinkedList<Zgrada.ZgradaInfo>();
			for (int i = 0; i < zgradeID.Length; i++)
				redVojneGradnje.AddLast(Zgrada.ZgradaInfoID[zgradeID[i]]);

			return new Kolonija(igrac, planet, populacija, radnaMjesta, civilnaInd,
				vojnaInd, zgrade, ostatakCivilneGradnje, ostatakVojneGradnje,
				redCivilneGradnje, redVojneGradnje);
		}
		#endregion
	}
}
