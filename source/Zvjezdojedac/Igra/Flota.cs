using System;
using System.Collections.Generic;
using System.Text;
using Alati;

namespace Prototip
{
	public class Flota : IPohranjivoSB
	{
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
			Sazetak stil = brod.dizajn.stil;
			if (!brodovi.ContainsKey(stil))
				brodovi.Add(stil, new Dictionary<Dizajn, Brod>());

			if (brodovi[stil].ContainsKey(brod.dizajn))
				brodovi[stil][brod.dizajn].dodaj(brod);
			else
				brodovi[stil].Add(brod.dizajn, brod);
		}

		#region Pohrana
		public const string PohranaTip = "FLOTA";
		public const string PohX = "X";
		public const string PohY = "Y";
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
		#endregion
	}
}
