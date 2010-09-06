using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Prototip
{
	public class Igrac : IIdentifiable, IPohranjivoSB
	{
		public enum Tip
		{
			COVJEK,
			RACUNALO
		}

		public class ZaStvoriti
		{
			public Tip tip;
			public string ime;
			public Organizacija organizacija;
			public Color boja;

			public ZaStvoriti(Tip tip, string ime, Organizacija organizacija, Color boja)
			{
				this.tip = tip;
				this.ime = ime;
				this.organizacija = organizacija;
				this.boja = boja;
			}

			public Igrac stvoriIgraca(int id)
			{
				return new Igrac(tip, ime, organizacija, boja, id);
			}
		}

		public static Color[] BojeIgraca = new Color[]{Color.LimeGreen, Color.LightGreen, Color.Blue, Color.Yellow, Color.DarkViolet, Color.DarkCyan};

		public int id { get; private set; }
		public Tip tip;
		private string ime;
		public System.Drawing.Color boja;
		private Random random;

		private Organizacija organizacija;

		public Zvijezda odabranaZvijezda;
		private Planet _odabranPlanet;
		public Dictionary<Poruka.Tip, bool> filtarPoruka = new Dictionary<Poruka.Tip,bool>();

		public List<Kolonija> kolonije = new List<Kolonija>();
		public LinkedList<Poruka> poruke = new LinkedList<Poruka>();
		public List<DizajnZgrada> dizajnoviBrodova = new List<DizajnZgrada>();
		public HashSet<PredefiniraniDizajn> predefiniraniDizajnovi = new HashSet<PredefiniraniDizajn>();

		public Dictionary<string, Tehnologija> tehnologije = new Dictionary<string,Tehnologija>();
		public LinkedList<Tehnologija> tehnologijeURazvoju = new LinkedList<Tehnologija>();
		public double koncentracijaPoenaRazvoja;
		public Dictionary<string, double> efekti = new Dictionary<string,double>();

		public Dictionary<Zvijezda, long> istrazivanjePoSustavu = new Dictionary<Zvijezda,long>();
		public Zvijezda istrazivanjeSustav;
		public LinkedList<Tehnologija> tehnologijeUIstrazivanju = new LinkedList<Tehnologija>();

		public HashSet<Zvijezda> posjeceneZvjezde = new HashSet<Zvijezda>();
		public Dictionary<Zvijezda, Flota> floteStacionarne = new Dictionary<Zvijezda,Flota>();
		public HashSet<Flota> flotePokretne = new HashSet<Flota>();

		private static void PrebrojiBrodove(IEnumerable<Flota> flote)
		{
			foreach (Flota flota in flote)
				foreach (Dictionary<Dizajn, Brod> brodovi in flota.brodovi.Values)
					foreach (Dizajn dizajn in brodovi.Keys)
						dizajn.brojBrodova += brodovi[dizajn].kolicina;
		}

		public Igrac(Tip tip, string ime, Organizacija organizacija, 
			System.Drawing.Color boja, int id)
		{
			this.id = id;
			this.tip = tip;
			this.ime = ime;
			this.boja = boja;
			this.organizacija = organizacija;
			random = new Random();
			odabranaZvijezda = null;
			_odabranPlanet = null;
			koncentracijaPoenaRazvoja = 1;
			
			foreach (Tehnologija.TechInfo t in Tehnologija.TechInfo.tehnologijeRazvoj)
				tehnologije.Add(t.kod, new Tehnologija(t));

			foreach (Tehnologija.TechInfo t in Tehnologija.TechInfo.tehnologijeIstrazivanje)
				tehnologije.Add(t.kod, new Tehnologija(t));

			filtarPoruka.Add(Poruka.Tip.Brod, true);
			filtarPoruka.Add(Poruka.Tip.Kolonija, true);
			filtarPoruka.Add(Poruka.Tip.Prica, true);
			filtarPoruka.Add(Poruka.Tip.Tehnologija, true);
			filtarPoruka.Add(Poruka.Tip.Zgrada, true);
		}

		private Igrac(int id, Tip tip, string ime, Color boja, Organizacija organizacija,
			 Zvijezda odabranaZvijezda, Planet odabranPlanet, LinkedList<Poruka> poruke,
			List<DizajnZgrada> dizajnoviBrodova, Dictionary<string, Tehnologija> tehnologije,
			LinkedList<Tehnologija> tehnologijeURazvoju, double koncentracijaPoenaRazvoja,
			LinkedList<Tehnologija> tehnologijeUIstrazivanju, HashSet<Zvijezda> posjeceneZvjezde,
			Dictionary<Zvijezda, Flota> floteStacionarne, HashSet<Flota> flotePokretne)
		{
			this.id = id;
			this.tip = tip;
			this.ime = ime;
			this.boja = boja;
			this.organizacija = organizacija;
			this.odabranaZvijezda = odabranaZvijezda;
			this._odabranPlanet = odabranPlanet;
			this.poruke = poruke;
			this.dizajnoviBrodova = dizajnoviBrodova;
			this.tehnologije = tehnologije;
			this.tehnologijeURazvoju = tehnologijeURazvoju;
			this.koncentracijaPoenaRazvoja = koncentracijaPoenaRazvoja;
			this.tehnologijeUIstrazivanju = tehnologijeUIstrazivanju;
			this.posjeceneZvjezde = posjeceneZvjezde;
			this.floteStacionarne = floteStacionarne;
			this.flotePokretne = flotePokretne;

			random = new Random();
			PrebrojiBrodove(this.flotePokretne);
			PrebrojiBrodove(this.floteStacionarne.Values);

			filtarPoruka.Add(Poruka.Tip.Brod, true);
			filtarPoruka.Add(Poruka.Tip.Kolonija, true);
			filtarPoruka.Add(Poruka.Tip.Prica, true);
			filtarPoruka.Add(Poruka.Tip.Tehnologija, true);
			filtarPoruka.Add(Poruka.Tip.Zgrada, true);
		}

		public void staviNoveTehnologije(Igra igra)
		{
			HashSet<Tehnologija.TechInfo> uProucavanju = new HashSet<Tehnologija.TechInfo>();
			foreach (Tehnologija teh in tehnologijeURazvoju)
				uProucavanju.Add(teh.tip);
			foreach (Tehnologija teh in tehnologijeUIstrazivanju)
				uProucavanju.Add(teh.tip);
			
			foreach (Tehnologija.TechInfo t in Tehnologija.TechInfo.tehnologijeRazvoj)
				if (!uProucavanju.Contains(t))
					if (tehnologije[t.kod].istrazivo(efekti))
						tehnologijeURazvoju.AddLast(tehnologije[t.kod]);

			foreach (Tehnologija.TechInfo t in Tehnologija.TechInfo.tehnologijeIstrazivanje)
				if (!uProucavanju.Contains(t))
					if (tehnologije[t.kod].istrazivo(efekti))
						tehnologijeUIstrazivanju.AddLast(tehnologije[t.kod]);
		}

		public void noviKrug(Igra igra, long poeniRazvoja, long poeniIstrazivanja)
		{
			//poruke.Clear();
			istraziTehnologije(igra, poeniRazvoja, poeniIstrazivanja);
			izracunajEfekte(igra);
			pomakniFlote();
			staviNoveTehnologije(igra);
			staviPredefiniraneDizajnove();
			staviNadogradjeneDizajnove();
			izracunajPoeneIstrazivanja(igra);
		}

		private void pomakniFlote()
		{
			HashSet<Zvijezda> prazneFloteStac = new HashSet<Zvijezda>();
			foreach (KeyValuePair<Zvijezda, Flota> flotaStac in floteStacionarne) {
				Zvijezda zvijezda = flotaStac.Key;
				Flota flota = flotaStac.Value;
				
				#region Kolonizacija
				foreach (Flota.Kolonizacija kolonizacija in flota.kolonizacije) {
					Planet planet = zvijezda.planeti[kolonizacija.planet];
					double maxDodatnaPopulacija = 0;
					if (planet.kolonija == null) {
						Kolonija kolonija = new Kolonija(this, planet, 10, 0);
						maxDodatnaPopulacija = kolonija.efekti[Kolonija.PopulacijaMax];
					}
					else
						maxDodatnaPopulacija = (planet.kolonija.efekti[Kolonija.PopulacijaMax] - planet.kolonija.populacija);

					long populacijaBroda = kolonizacija.brod.dizajn.populacija;
					long radnaMjestaBroda = kolonizacija.brod.dizajn.radnaMjesta;
					long brBrodova = (long)(Math.Min(kolonizacija.brBrodova, Math.Ceiling(maxDodatnaPopulacija / populacijaBroda)));
					if (planet.kolonija == null) {
						planet.kolonija = new Kolonija(
							this,
							planet,
							populacijaBroda * brBrodova,
							radnaMjestaBroda * brBrodova);
						poruke.AddLast(Poruka.NovaKolonija(planet.kolonija));
					}
					else
						planet.kolonija.dodajKolonizator(
							populacijaBroda * brBrodova,
							radnaMjestaBroda * brBrodova);

					flota.ukloniBrod(kolonizacija.brod.dizajn, brBrodova);
				}
				flota.kolonizacije.Clear();
				#endregion

				if (flota.brodovi.Count == 0)
					prazneFloteStac.Add(zvijezda);
			}

			foreach (Zvijezda zvj in prazneFloteStac)
				floteStacionarne.Remove(zvj);

			prebrojiBrodove();
		}

		public void izracunajEfekte(Igra igra)
		{
			kolonije.Clear();
			foreach (Zvijezda zvj in igra.mapa.zvijezde)
				foreach (Planet pl in zvj.planeti)
					if (pl.kolonija != null)
						if (pl.kolonija.igrac == this)
							kolonije.Add(pl.kolonija);

			efekti.Clear();
			foreach (string kod in tehnologije.Keys) efekti.Add(kod + "_LVL", tehnologije[kod].nivo);
			foreach (string s in igra.osnovniEfekti.Keys) efekti.Add(s, igra.osnovniEfekti[s].iznos(efekti));
		}

		public void izracunajPoeneIstrazivanja(Igra igra)
		{
			Dictionary<Zvijezda, long> istrazivanjePoSustavuBaza = new Dictionary<Zvijezda,long>();

			foreach (Zvijezda zvj in igra.mapa.zvijezde)
			{
				istrazivanjePoSustavuBaza[zvj] = 0;
				foreach(Planet pl in zvj.planeti)
					if (pl.kolonija != null)
						if (pl.kolonija.igrac == this)
							istrazivanjePoSustavuBaza[zvj] += (long)(pl.kolonija.populacija * efekti["ISTRAZIVANJE_PO_STANOVNIKU"]);
				istrazivanjePoSustavuBaza[zvj] = (long)Math.Floor(Math.Sqrt(istrazivanjePoSustavuBaza[zvj]));
			}

			istrazivanjeSustav = igra.mapa.zvijezde[0];
			foreach (Zvijezda zvjI in istrazivanjePoSustavuBaza.Keys)
			{
				istrazivanjePoSustavu[zvjI] = 0;
				foreach (Zvijezda zvjJ in istrazivanjePoSustavuBaza.Keys)
						istrazivanjePoSustavu[zvjI] += (long)(istrazivanjePoSustavuBaza[zvjJ] / (1 + zvjI.udaljenost(zvjJ) / efekti["ISTRAZIVANJE_KOM_UDALJENOST"]));

				if (istrazivanjePoSustavu[zvjI] > istrazivanjePoSustavu[istrazivanjeSustav])
					istrazivanjeSustav = zvjI;
			}
		}

		public long poeniIstrazivanja()
		{
			return istrazivanjePoSustavu[istrazivanjeSustav];
		}

		public long poeniRazvoja()
		{
			long sum = 0;
			foreach (Kolonija kolonija in kolonije)
				sum += kolonija.poeniRazvoja();

			return sum;
		}

		private void istraziTehnologije(Igra igra, long poeniRazvoja, long poeniIstrazivanja)
		{
			List<long> rasporedPoena = Tehnologija.RasporedPoena(poeniRazvoja, tehnologijeURazvoju.Count, koncentracijaPoenaRazvoja);
			int i = 0;
			long ulog = 0;
			foreach(Tehnologija teh in tehnologijeURazvoju)
			{
				long nivo = teh.nivo;
				ulog += rasporedPoena[i];
				ulog = teh.uloziPoene(ulog, efekti);
				if (teh.nivo != nivo)
					poruke.AddLast(Poruka.NovaTehnologija(teh));
				i++;
			}

			rasporedPoena = Tehnologija.RasporedPoena(poeniIstrazivanja, tehnologijeUIstrazivanju.Count, 0.5 + random.NextDouble());
			i = 0;
			ulog = 0;
			foreach (Tehnologija teh in tehnologijeUIstrazivanju)
			{
				long nivo = teh.nivo;
				ulog += rasporedPoena[i];
				ulog = teh.uloziPoene(ulog, efekti);
				if (teh.nivo != nivo)
					poruke.AddLast(Poruka.NovaTehnologija(teh));
				i++;
			}

			for (LinkedListNode<Tehnologija> t = tehnologijeURazvoju.First; t != null; )
				if (t.Value.istrazivo(efekti))
					t = t.Next;
				else
				{
					LinkedListNode<Tehnologija> slijedeci = t.Next;
					tehnologijeURazvoju.Remove(t);
					t = slijedeci;
				}

			for (LinkedListNode<Tehnologija> t = tehnologijeUIstrazivanju.First; t != null; )
				if (t.Value.istrazivo(efekti))
					t = t.Next;
				else
				{
					LinkedListNode<Tehnologija> slijedeci = t.Next;
					tehnologijeUIstrazivanju.Remove(t);
					t = slijedeci;
				}
		}

		public Planet odabranPlanet
		{
			get
			{
				return _odabranPlanet;
			}
			set
			{
				_odabranPlanet = value;
				if (_odabranPlanet.kolonija != null)
					_odabranPlanet.kolonija.postaviEfekteIgracu();
			}
		}

		public void dodajBrod(Dizajn dizajn, long kolicina, Zvijezda zvijezda)
		{
			if (!floteStacionarne.ContainsKey(zvijezda))
				floteStacionarne.Add(zvijezda, new Flota(zvijezda.x, zvijezda.y));

			floteStacionarne[zvijezda].dodajBrod(new Brod(dizajn, kolicina));
			dizajn.brojBrodova += kolicina;
		}

		public HashSet<Dizajn> dizajnoviUGradnji()
		{
			HashSet<Zgrada.ZgradaInfo> dizajnovi = new HashSet<Zgrada.ZgradaInfo>();
			foreach (DizajnZgrada dizajnZgrada in dizajnoviBrodova)
				dizajnovi.Add(dizajnZgrada);

			HashSet<Dizajn> rez = new HashSet<Dizajn>();
			foreach (Kolonija kolonija in kolonije)
				foreach (Zgrada.ZgradaInfo zgrada in kolonija.redVojneGradnje)
					if (dizajnovi.Contains(zgrada))
						rez.Add(((DizajnZgrada)zgrada).dizajn);
			return rez;
		}

		public void dodajDizajn(Dizajn dizajn)
		{
			dizajnoviBrodova.Add(new DizajnZgrada(dizajn));
		}

		public void prebrojiBrodove()
		{
			foreach (DizajnZgrada dizajnZgrada in dizajnoviBrodova)
				dizajnZgrada.dizajn.brojBrodova = 0;

			PrebrojiBrodove(this.flotePokretne);
			PrebrojiBrodove(this.floteStacionarne.Values);
		}

		private void staviNadogradjeneDizajnove()
		{
			HashSet<Dizajn> nadogradiviOdPrije = new HashSet<Dizajn>();
			List<Dizajn> noviDizajnovi = new List<Dizajn>();

			foreach (DizajnZgrada dizajnZgrada in dizajnoviBrodova)
				if (dizajnZgrada.dizajn.nadogradnja != null)
					nadogradiviOdPrije.Add(dizajnZgrada.dizajn);
				else {
					dizajnZgrada.dizajn.traziNadogradnju(efekti);
					if (dizajnZgrada.dizajn.nadogradnja != null)
						noviDizajnovi.Add(dizajnZgrada.dizajn.nadogradnja);
				}

			foreach (Dizajn dizajn in nadogradiviOdPrije)
				dizajn.traziNadogradnju(efekti);

			foreach (Dizajn dizajn in noviDizajnovi)
				dodajDizajn(dizajn);

			HashSet<Dizajn> uGradnji = dizajnoviUGradnji();
			List<DizajnZgrada> bezZastarjelih = new List<DizajnZgrada>();
			foreach(DizajnZgrada dizajnZgrada in dizajnoviBrodova)
			{
				Dizajn dizajn = dizajnZgrada.dizajn;
				if (dizajn.nadogradnja == null || dizajn.brojBrodova > 0 || uGradnji.Contains(dizajn))
					bezZastarjelih.Add(dizajnZgrada);
			}

			dizajnoviBrodova = bezZastarjelih;
		}

		public void staviPredefiniraneDizajnove()
		{
			foreach (PredefiniraniDizajn pd in PredefiniraniDizajn.dizajnovi)
				if (!predefiniraniDizajnovi.Contains(pd))
					if (pd.dostupan(efekti))
					{
						predefiniraniDizajnovi.Add(pd);
						dodajDizajn(pd.naciniDizajn(efekti));
					}
		}

		#region Pohrana
		public const string PohranaTip = "IGRAC";
		private const string PohId = "ID";
		private const string PohTip = "TIP", PohTipCovjek = "COVJEK", PohTipRacunalo = "RACUNALO";
		private const string PohIme = "IME";
		private const string PohBoja = "BOJA";
		private const string PohOrganizacija = "ORGANIZACIJA";
		private const string PohPogledZvj = "POGLED_ZVJ";
		private const string PohPogledPlanet = "POGLED_PLANET";
		private const string PohPoruka = "PORUKA";
		private const string PohDizajn = "DIZAJN";
		private const string PohTehnologija = "TEHNO";
		private const string PohTehURazvoju = "TEH_U_RAZ";
		private const string PohTehUIstraz = "TEH_U_IST";
		private const string PohTehRazKonc = "TEH_RAZ_KONC";
		private const string PohPosjeceneZvj = "ZVJ_POSJET";
		private const string PohFloteStac = "FLOTE_STAC";
		private const string PohFlotePokret = "FLOTE_POK";
		public void pohrani(PodaciPisac izlaz)
		{
			
			if (tip == Tip.COVJEK)
				izlaz.dodaj(PohTip, PohTipCovjek);
			else
				izlaz.dodaj(PohTip, PohTipRacunalo);

			izlaz.dodaj(PohId, id);
			izlaz.dodaj(PohIme, ime);
			izlaz.dodaj(PohBoja, boja.R + " " + boja.G + " " + boja.B);
			izlaz.dodaj(PohOrganizacija, organizacija);

			izlaz.dodaj(PohPogledZvj, 
				odabranaZvijezda.x.ToString(PodaciAlat.DecimalnaTocka) 
				+ " " 
				+ odabranaZvijezda.y.ToString(PodaciAlat.DecimalnaTocka));
			izlaz.dodaj(PohPogledPlanet, odabranPlanet.pozicija);

			izlaz.dodaj(PohPoruka, poruke.Count);
			izlaz.dodajKolekciju(PohPoruka, poruke);

			izlaz.dodaj(PohDizajn, dizajnoviBrodova.Count);
			for (int i = 0; i < dizajnoviBrodova.Count; i++)
				izlaz.dodaj(PohDizajn+i, (IPohranjivoSB)dizajnoviBrodova[i].dizajn);

			izlaz.dodaj(PohTehnologija, tehnologije.Count);
			izlaz.dodajKolekciju(PohTehnologija, tehnologije.Values);
			izlaz.dodaj(PohTehRazKonc, koncentracijaPoenaRazvoja);
			izlaz.dodajIdeve(PohTehURazvoju, tehnologijeURazvoju);
			izlaz.dodajIdeve(PohTehUIstraz, tehnologijeUIstrazivanju);

			izlaz.dodajIdeve(PohPosjeceneZvj, posjeceneZvjezde);

			List<Zvijezda> zvjezde = new List<Zvijezda>(floteStacionarne.Keys);
			List<Flota> flote = new List<Flota>();
			foreach (Zvijezda zvj in zvjezde)
				flote.Add(floteStacionarne[zvj]);
			izlaz.dodajIdeve(PohFloteStac, zvjezde);
			izlaz.dodajKolekciju(PohFloteStac, flote);

			izlaz.dodaj(PohFlotePokret, flotePokretne.Count);
			izlaz.dodajKolekciju(PohFlotePokret, flotePokretne);
		}

		private static Color OdrediBoju(string bojaString)
		{
			string[] kanali = bojaString.Split(new char[] { ' ' });
			return Color.FromArgb(
				int.Parse(kanali[0]), 
				int.Parse(kanali[1]), 
				int.Parse(kanali[2]));
		}

		private static Zvijezda OdrediOdabranuZvj(Mapa mapa, string zvjString)
		{
			string[] pozicija = zvjString.Split(new char[] { ' ' });
			return mapa.najblizaZvijezda(
				double.Parse(pozicija[0], PodaciAlat.DecimalnaTocka),
				double.Parse(pozicija[1], PodaciAlat.DecimalnaTocka),
				-1);
		}

		public static Igrac Ucitaj(PodaciCitac ulaz, Mapa mapa)
		{
			Tip tip = Tip.COVJEK;
			if (ulaz.podatak(PohTip) != PohTipCovjek)
				tip = Tip.RACUNALO;

			int id = ulaz.podatakInt(PohId);
			string ime = ulaz.podatak(PohIme);
			Organizacija organizacija = Organizacija.lista[ulaz.podatakInt(PohOrganizacija)];
			Color boja = OdrediBoju(ulaz.podatak(PohBoja));
			foreach (Color color in BojeIgraca)
				if (boja.R == color.R && boja.G == color.G && boja.B == color.B)
					boja = color;

			Zvijezda odabranaZvj = OdrediOdabranuZvj(mapa, ulaz.podatak(PohPogledZvj));
			Planet odabranPlanet = odabranaZvj.planeti[ulaz.podatakInt(PohPogledPlanet)];

			Dictionary<int, Zvijezda> zvijezdeID = new Dictionary<int, Zvijezda>();
			foreach (Zvijezda zvj in mapa.zvijezde)
				zvijezdeID.Add(zvj.id, zvj);
			int brPoruka = ulaz.podatakInt(PohPoruka);
			LinkedList<Poruka> poruke = new LinkedList<Poruka>();
			for (int i = 0; i < brPoruka; i++)
				poruke.AddLast(Poruka.Ucitaj(ulaz[PohPoruka + i], zvijezdeID));

			int brDizajnova = ulaz.podatakInt(PohDizajn);
			List<DizajnZgrada> dizajnovi = new List<DizajnZgrada>();
			for (int i = 0; i < brDizajnova; i++) {
				Dizajn dizajn = Dizajn.Ucitaj(ulaz[PohDizajn + i]);
				dizajnovi.Add(new DizajnZgrada(dizajn));
			}

			int brTeh = ulaz.podatakInt(PohTehnologija);
			Dictionary<string, Tehnologija> tehnologije = new Dictionary<string, Tehnologija>();
			for (int i = 0; i < brTeh; i++) {
				Tehnologija teh = Tehnologija.Ucitaj(ulaz[PohTehnologija + i]);
				tehnologije.Add(teh.tip.kod, teh);
			}
			double koncPoenaRazvoja = ulaz.podatakDouble(PohTehRazKonc);

			int[] tmpIntovi = ulaz.podatakIntPolje(PohTehURazvoju);
			LinkedList<Tehnologija> tehURazvoju = new LinkedList<Tehnologija>();
			foreach(int tehId in tmpIntovi)
				tehURazvoju.AddLast(tehnologije[Tehnologija.TechInfo.tehnologijeRazvoj[tehId].kod]);
			
			tmpIntovi = ulaz.podatakIntPolje(PohTehUIstraz);
			LinkedList<Tehnologija> tehUIstraz = new LinkedList<Tehnologija>();
			foreach (int tehId in tmpIntovi)
				tehUIstraz.AddLast(tehnologije[Tehnologija.TechInfo.tehnologijeIstrazivanje[tehId].kod]);

			tmpIntovi = ulaz.podatakIntPolje(PohPosjeceneZvj);
			HashSet<Zvijezda> posjeceneZvijezde = new HashSet<Zvijezda>();
			foreach (int zvjId in tmpIntovi)
				posjeceneZvijezde.Add(zvijezdeID[zvjId]);

			Dictionary<int, Dizajn> dizajnID = new Dictionary<int, Dizajn>();
			foreach (DizajnZgrada dizajnZgrada in dizajnovi)
				dizajnID.Add(dizajnZgrada.dizajn.id, dizajnZgrada.dizajn);
			tmpIntovi = ulaz.podatakIntPolje(PohFloteStac);
			Dictionary<Zvijezda, Flota> floteStacionarne = new Dictionary<Zvijezda,Flota>();
			for (int i = 0; i < tmpIntovi.Length; i++)
				floteStacionarne.Add(
					zvijezdeID[tmpIntovi[i]],
					Flota.Ucitaj(ulaz[PohFloteStac + i], dizajnID));

			int brPokFlota = ulaz.podatakInt(PohFlotePokret);
			HashSet<Flota> flotePokretne = new HashSet<Flota>();
			for (int i = 0; i < brPokFlota; i++)
				flotePokretne.Add(Flota.Ucitaj(ulaz[PohFlotePokret + i], dizajnID));

			return new Igrac(id, tip, ime, boja, organizacija, odabranaZvj, odabranPlanet,
				poruke, dizajnovi, tehnologije, tehURazvoju, koncPoenaRazvoja, tehUIstraz,
				posjeceneZvijezde, floteStacionarne, flotePokretne);
		}
		#endregion
	}
}
