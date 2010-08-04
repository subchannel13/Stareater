using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Prototip
{
	public class AKomponentaInfo : IIdentifiable
	{
		#region Statično
		private static Dictionary<string, double> varNivo = new Dictionary<string, double>();
		private static Dictionary<string, double> varNivoVelicina = new Dictionary<string, double>();
		private static Dictionary<Type, uint> slijedeciIndeks = new Dictionary<Type, uint>();

		protected static double Evaluiraj(Formula formula, int nivo)
		{
			varNivo["LVL"] = nivo;
			return formula.iznos(varNivo);
		}

		protected static double Evaluiraj(Formula formula, int nivo, double velicina)
		{
			varNivoVelicina["LVL"] = nivo;
			varNivoVelicina["VELICINA"] = velicina;
			return formula.iznos(varNivoVelicina);
		}
		#endregion

		public string naziv { get; private set; }
		public string opis { get; private set; }
		public Image slika { get; private set; }
		public List<Tehnologija.Preduvjet> preduvjeti { get; private set; }
		public int maxNivo { get; private set; }
		public uint indeks { get; private set; }

		protected AKomponentaInfo(string naziv, string opis, Image slika, List<Tehnologija.Preduvjet> preduvjeti, int maxNivo)
		{
			this.naziv = naziv;
			this.opis = opis;
			this.slika = slika;
			this.preduvjeti = preduvjeti;
			this.maxNivo = maxNivo;
			indeks = noviIndeks();
		}

		public bool dostupno(Dictionary<string, double> varijable)
		{
			return dostupno(varijable, 0);
		}

		public bool dostupno(Dictionary<string, double> varijable, int nivo)
		{
			if (preduvjeti.Count > 0)
			{
				varijable["LVL"] = nivo;
				foreach (Tehnologija.Preduvjet p in preduvjeti)
					if (!p.zadovoljen(varijable))
						return false;
			}
			return true;
		}

		public int maxDostupanNivo(Dictionary<string, double> varijable)
		{
			if (maxNivo == 0) return 0;
			if (dostupno(varijable, maxNivo)) return maxNivo;
			int min = 1, max = maxNivo;

			while (max - min > 1)
			{
				int sredina = (max + min) / 2;
				
				if (dostupno(varijable, sredina))
					min = sredina;
				else
					max = sredina;
			}
			for (; min < max; min++)
				if (!dostupno(varijable, min))
					return min - 1;

			return min;
		}

		private uint noviIndeks()
		{
			Type tip = GetType();
			if (!slijedeciIndeks.ContainsKey(tip))
				slijedeciIndeks.Add(tip, 1);
			else
				slijedeciIndeks[tip]++;

			return slijedeciIndeks[tip] - 1;
		}

		public uint brojIndeksa()
		{
			return slijedeciIndeks[GetType()];
		}

		public int id 
		{ 
			get { return (int)indeks; } 
		}
	}
}
