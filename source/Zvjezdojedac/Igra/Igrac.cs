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

		public static Color[] BojeIgraca = new Color[]{Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.DarkViolet, Color.Turquoise};

		public int id { get; private set; }
		public Tip tip;
		private string ime;
		public System.Drawing.Color boja;
		private Random random;

		private Organizacija organizacija;

		public Zvijezda odabranaZvijezda;
		private Planet _odabranPlanet;

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

		public void noviKrug(Igra igra)
		{
			poruke.Clear();
			izracunajPoeneIstrazivanja(igra);
			istraziTehnologije(igra);
			izracunajEfekte(igra);
			staviNoveTehnologije(igra);
			staviPredefiniraneDizajnove();			
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

		public long poeniRazvoja()
		{
			long sum = 0;
			foreach (Kolonija kolonija in kolonije)
				sum += kolonija.poeniRazvoja();

			return sum;
		}

		private void istraziTehnologije(Igra igra)
		{
			List<long> rasporedPoena = Tehnologija.RasporedPoena(poeniRazvoja(), tehnologijeURazvoju.Count, koncentracijaPoenaRazvoja);
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

			rasporedPoena = Tehnologija.RasporedPoena(istrazivanjePoSustavu[istrazivanjeSustav], tehnologijeUIstrazivanju.Count, 0.5 + random.NextDouble());
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

		public void dodajBrod(Dizajn dizajn, int kolicina, Zvijezda zvijezda)
		{
			if (!floteStacionarne.ContainsKey(zvijezda))
				floteStacionarne.Add(zvijezda, new Flota(zvijezda.x, zvijezda.y));

			floteStacionarne[zvijezda].dodajBrod(new Brod(dizajn, kolicina));
			dizajn.brojBrodova += kolicina;
		}

		public void dodajDizajn(Dizajn dizajn)
		{
			dizajnoviBrodova.Add(new DizajnZgrada(dizajn));
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
				odabranaZvijezda.x.ToString(Podaci.DecimalnaTocka) 
				+ " " 
				+ odabranaZvijezda.y.ToString(Podaci.DecimalnaTocka));
			izlaz.dodaj(PohPogledPlanet, odabranPlanet.pozicija);

			izlaz.dodaj(PohPoruka, poruke.Count);
			izlaz.dodajKolekciju(PohPoruka, poruke);

			izlaz.dodaj(PohDizajn, dizajnoviBrodova.Count);
			for (int i = 0; i < dizajnoviBrodova.Count; i++)
				izlaz.dodaj(PohDizajn+i, (IPohranjivoSB)dizajnoviBrodova[i].dizajn);

			izlaz.dodaj(PohTehnologija, tehnologije.Count);
			izlaz.dodajKolekciju(PohTehnologija, tehnologije.Values);
			izlaz.dodaj(PohTehRazKonc, koncentracijaPoenaRazvoja);
			//izlaz.dodaj(PohTehURazvoju, tehnologijeURazvoju.Count);
			//izlaz.dodaj(PohTehUIstraz, tehnologijeUIstrazivanju.Count);
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
				double.Parse(pozicija[0], Podaci.DecimalnaTocka),
				double.Parse(pozicija[1], Podaci.DecimalnaTocka),
				-1);
		}

		public static Igrac Ucitaj(PodaciCitac ulaz, Mapa mapa)
		{
			Tip tip = Tip.COVJEK;
			if (ulaz.podatak(PohTip) != PohTipCovjek)
				tip = Tip.RACUNALO;

			int id = ulaz.podatakInt(PohId);
			string ime = ulaz.podatak(PohIme);
			Color boja = OdrediBoju(ulaz.podatak(PohBoja));
			Organizacija organizacija = Organizacija.lista[ulaz.podatakInt(PohOrganizacija)];

			Zvijezda odabranaZvj = OdrediOdabranuZvj(mapa, ulaz.podatak(PohPogledZvj));
			Planet odabranPlanet = odabranaZvj.planeti[ulaz.podatakInt(PohPogledPlanet)];

			int brPoruka = ulaz.podatakInt(PohPoruka);
			LinkedList<Poruka> poruke = new LinkedList<Poruka>();
			for (int i = 0; i < brPoruka; i++)
				poruke.AddLast(Poruka.Ucitaj(ulaz[PohPoruka + i]));

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

			Dictionary<int, Zvijezda> zvijezdeID = new Dictionary<int,Zvijezda>();
			foreach(Zvijezda zvj in mapa.zvijezde)
				zvijezdeID.Add(zvj.id, zvj);
			tmpIntovi = ulaz.podatakIntPolje(PohPosjeceneZvj);
			HashSet<Zvijezda> posjeceneZvijezde = new HashSet<Zvijezda>();
			foreach (int zvjId in tmpIntovi)
				posjeceneZvijezde.Add(zvijezdeID[zvjId]);

			Dictionary<int, Dizajn> dizajnID = new Dictionary<int, Dizajn>();
			foreach (DizajnZgrada dizajnZgrada in dizajnovi)
				dizajnID.Add(dizajnZgrada.dizajn.id, dizajnZgrada.dizajn);
			tmpIntovi = ulaz.podatakIntPolje(PohFloteStac);
			Dictionary<Zvijezda, Flota> floteStacionarne = new Dictionary<Zvijezda,Flota>();
			for (int zvjId = 0; zvjId < tmpIntovi.Length; zvjId++)
				floteStacionarne.Add(
					zvijezdeID[zvjId],
					Flota.Ucitaj(ulaz[PohFloteStac + zvjId], dizajnID));

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
