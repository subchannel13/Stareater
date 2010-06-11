using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Prototip
{
	public class Igrac : IIdentifiable
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
			public Organizacije organizacija;
			public Color boja;

			public ZaStvoriti(Tip tip, string ime, Organizacije organizacija, Color boja)
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

		private Organizacije organizacija;

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

		public Igrac(Tip tip, string ime, Organizacije organizacija, 
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
	}
}
