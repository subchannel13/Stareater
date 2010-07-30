using System;
using System.Collections.Generic;
using System.Text;
using Alati;

namespace Prototip
{
	public class Flota : IPohranjivoSB
	{
		private HashSet<Misija.Tip> misije = new HashSet<Misija.Tip>();
		public Dictionary<Sazetak, Dictionary<Dizajn, Brod>> brodovi;
		public double x;
		public double y;

		public Flota(double x, double y)
		{
			this.brodovi = new Dictionary<Sazetak, Dictionary<Dizajn, Brod>>();
			this.x = x;
			this.y = y;
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

		public bool imaMisiju(Misija.Tip misija)
		{
			return misije.Contains(misija);
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
			
		}

		public static Flota Ucitaj(PodaciCitac ulaz, Dictionary<int, Dizajn> dizajnovi)
		{
			double x = ulaz.podatakDouble(PohX);
			double y = ulaz.podatakDouble(PohY);
			Flota flota = new Flota(x, y);
			
			int brBrodova = ulaz.podatakInt(Brod.PohranaTip);
			for (int i = 0; i < brBrodova; i++)
				flota.dodajBrod(Brod.Ucitaj(ulaz[Brod.PohranaTip + i], dizajnovi));

			return flota;
		}
		#endregion
	}
}
