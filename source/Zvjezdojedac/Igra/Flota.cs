using System;
using System.Collections.Generic;
using System.Text;
using Alati;

namespace Prototip
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
					flota.brodSDizajnom(dizajnovi[ulaz.podatakInt(PohBrodovi)]),
					ulaz.podatakLong(PohBrBrodova));
			}
			#endregion
		}

		private HashSet<Misija.Tip> misije = new HashSet<Misija.Tip>();

		public Dictionary<Sazetak, Dictionary<Dizajn, Brod>> brodovi { get; private set; }
		public double x { get; private set; }
		public double y { get; private set; }
		public List<Kolonizacija> kolonizacije { get; private set; }

		public Flota(double x, double y)
		{
			this.brodovi = new Dictionary<Sazetak, Dictionary<Dizajn, Brod>>();
			this.x = x;
			this.y = y;

			this.kolonizacije = new List<Kolonizacija>();
		}

		public Brod brodSDizajnom(Dizajn dizajn)
		{
			return brodovi[dizajn.stil][dizajn];
		}

		public void dodajBrod(Brod brod)
		{
			Dizajn dizajn = brod.dizajn;
			Sazetak stil = dizajn.stil;
			if (!brodovi.ContainsKey(stil))
				brodovi.Add(stil, new Dictionary<Dizajn, Brod>());

			if (brodovi[stil].ContainsKey(dizajn))
				brodovi[stil][dizajn].dodaj(brod);
			else
				brodovi[stil].Add(dizajn, brod);

			if (dizajn.primarnoOruzje != null) misije.Add(dizajn.primarnoOruzje.komponenta.misija);
			if (dizajn.sekundarnoOruzje != null) misije.Add(dizajn.sekundarnoOruzje.komponenta.misija);
		}

		public void dodajKolonizacije(Brod brod, long[] brodoviPoPlanetu)
		{
			kolonizacije.RemoveAll(kolonizacija => kolonizacija.brod == brod);
			brod = brodovi[brod.dizajn.stil][brod.dizajn];
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

		public void ukloniBrod(Dizajn dizajn, long kolicina)
		{
			brodovi[dizajn.stil][dizajn].ukloni(kolicina);

			if (brodovi[dizajn.stil][dizajn].kolicina <= 0)
				brodovi[dizajn.stil].Remove(dizajn);

			if (brodovi[dizajn.stil].Count == 0)
				brodovi.Remove(dizajn.stil);
		}

		#region Pohrana
		public const string PohranaTip = "FLOTA";
		private const string PohX = "X";
		private const string PohY = "Y";
		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohX, x);
			izlaz.dodaj(PohY, y);

			HashSet<Brod> brodovi = new HashSet<Brod>();
			foreach (Dictionary<Dizajn, Brod> dizajnovi in this.brodovi.Values)
				brodovi.UnionWith(dizajnovi.Values);

			izlaz.dodaj(Brod.PohranaTip, brodovi.Count);
			izlaz.dodajKolekciju(Brod.PohranaTip, brodovi);
			izlaz.dodaj(Kolonizacija.PohranaTip, kolonizacije.Count);
			izlaz.dodajKolekciju(Kolonizacija.PohranaTip, kolonizacije);
		}

		public static Flota Ucitaj(PodaciCitac ulaz, Dictionary<int, Dizajn> dizajnovi)
		{
			double x = ulaz.podatakDouble(PohX);
			double y = ulaz.podatakDouble(PohY);
			Flota flota = new Flota(x, y);
			
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
