using System;
using System.Collections.Generic;
using System.Text;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Igra.Brodovi;

namespace Zvjezdojedac.Igra
{
	public class Flota : IPohranjivoSB
	{
		public struct Kolonizacija : IPohranjivoSB
		{
			public int planet;
			public Brod brod;
			public long brBrodova;

			public Kolonizacija(int planet, Brod brodovi, long brBrodova)
			{
				this.planet = planet;
				this.brod = brodovi;
				this.brBrodova = brBrodova;
			}

			#region Pohrana
			public const string PohranaTip = "KOLONIZACIJA";
			private const string PohPlanet = "PLANET";
			private const string PohBrodovi = "BROD";
			private const string PohBrBrodova = "BR_BRODOVA";
			public void pohrani(PodaciPisac izlaz)
			{
				izlaz.dodaj(PohPlanet, planet);
				izlaz.dodaj(PohBrodovi, brod.dizajn.id);
				izlaz.dodaj(PohBrBrodova, brBrodova);
			}
			public static Kolonizacija Ucitaj(PodaciCitac ulaz, Dictionary<int, Dizajn> dizajnovi, Flota flota)
			{
				return new Kolonizacija(
					ulaz.podatakInt(PohPlanet),
					flota[dizajnovi[ulaz.podatakInt(PohBrodovi)]],
					ulaz.podatakLong(PohBrBrodova));
			}
			#endregion
		}

		protected HashSet<Misija.Tip> misije = new HashSet<Misija.Tip>();

		public Dictionary<Dizajn, Brod> brodovi { get; protected set; }
		public int id { get; protected set; }
		public double x { get; protected set; }
		public double y { get; protected set; }
		public List<Kolonizacija> kolonizacije { get; protected set; }

		public Flota(Zvijezda zvijezda, int id)
			: this(zvijezda.x, zvijezda.y, id)
		{ }

		public Flota(double x, double y, int id)
		{
			this.brodovi = new Dictionary<Dizajn,Brod>();
			this.id = id;
			this.x = x;
			this.y = y;

			this.kolonizacije = new List<Kolonizacija>();
		}

		public void dodajBrod(Brod brod)
		{
			Dizajn dizajn = brod.dizajn;
			Sazetak stil = dizajn.stil;

			if (brodovi.ContainsKey(dizajn))
				brodovi[dizajn].dodaj(brod);
			else
				brodovi.Add(dizajn, brod);

			if (dizajn.primarnoOruzje != null) misije.Add(dizajn.primarnoOruzje.komponenta.misija);
			if (dizajn.sekundarnoOruzje != null) misije.Add(dizajn.sekundarnoOruzje.komponenta.misija);
		}

		public void dodajBrodove(Flota flota)
		{
			foreach (Brod brod in flota.brodovi.Values)
				dodajBrod(brod);
		}

		public void dodajKolonizacije(Brod brod, long[] brodoviPoPlanetu)
		{
			kolonizacije.RemoveAll(kolonizacija => kolonizacija.brod == brod);
			brod = brodovi[brod.dizajn];
			long raspoloziviBrodovi = brod.kolicina;

			for (int i = 0; (i < brodoviPoPlanetu.Length) && (raspoloziviBrodovi > 0); i++) {
				long brDodjeljenih = Math.Min(raspoloziviBrodovi, brodoviPoPlanetu[i]);
				if (brDodjeljenih > 0) {
					kolonizacije.Add(new Kolonizacija(i, brod, brDodjeljenih));
					raspoloziviBrodovi -= brDodjeljenih;
				}
			}
		}

		public bool imaMisiju(Misija.Tip misija)
		{
			return misije.Contains(misija);
		}

		public void preuzmiBrodove(Flota izvornaFlota, Dictionary<Dizajn, long> brodovi)
		{
			foreach (KeyValuePair<Dizajn, long> brod in brodovi)
				izvornaFlota.ukloniBrod(brod.Key, brod.Value);
			foreach (KeyValuePair<Dizajn, long> brod in brodovi)
				if (brod.Value > 0)
					dodajBrod(new Brod(brod.Key, brod.Value));
		}

		public void ukloniBrod(Dizajn dizajn, long kolicina)
		{
			brodovi[dizajn].ukloni(kolicina);

			if (brodovi[dizajn].kolicina <= 0)
				brodovi.Remove(dizajn);
		}

		public Brod this[Dizajn dizajn]
		{
			get { return brodovi[dizajn]; }
		}

		#region Pohrana
		public const string PohranaTip = "FLOTA";
		protected const string PohId = "id";
		protected const string PohX = "X";
		protected const string PohY = "Y";
		public virtual void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohId, id);
			izlaz.dodaj(PohX, x);
			izlaz.dodaj(PohY, y);

			izlaz.dodaj(Brod.PohranaTip, brodovi.Count);
			izlaz.dodajKolekciju(Brod.PohranaTip, brodovi.Values);
			izlaz.dodaj(Kolonizacija.PohranaTip, kolonizacije.Count);
			izlaz.dodajKolekciju(Kolonizacija.PohranaTip, kolonizacije);
		}

		public static Flota Ucitaj(PodaciCitac ulaz, Dictionary<int, Dizajn> dizajnovi)
		{
			int id = ulaz.podatakInt(PohId);
			double x = ulaz.podatakDouble(PohX);
			double y = ulaz.podatakDouble(PohY);
			Flota flota = new Flota(x, y, id);
			
			int brBrodova = ulaz.podatakInt(Brod.PohranaTip);
			for (int i = 0; i < brBrodova; i++)
				flota.dodajBrod(Brod.Ucitaj(ulaz[Brod.PohranaTip + i], dizajnovi));

			List<Kolonizacija> kolonizacije = new List<Kolonizacija>();
			for (int i = 0; i < ulaz.podatakInt(Kolonizacija.PohranaTip); i++)
				kolonizacije.Add(Kolonizacija.Ucitaj(
					ulaz[Kolonizacija.PohranaTip + i], 
					dizajnovi, 
					flota));
			flota.kolonizacije.AddRange(kolonizacije);

			return flota;
		}
		#endregion
	}
}
