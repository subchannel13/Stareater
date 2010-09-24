using System;
using System.Collections.Generic;
using System.Text;

namespace Alati
{
	public static class Fje
	{

		public static int Ogranici(int sto, int min, int max)
		{
			if (sto < min) return min;
			if (sto > max) return max;
			else return sto;
		}

		public static long Ogranici(long sto, long min, long max)
		{
			if (sto < min) return min;
			if (sto > max) return max;
			else return sto;
		}

		public static double Ogranici(double sto, double min, double max)
		{
			if (sto < min) return min;
			if (sto > max) return max;
			else return sto;
		}

		public static double IzIntervala(double vrijednost, double min, double max)
		{
			return min + vrijednost * (max - min);
		}

		public static string PrefiksFormater(double broj)
		{
			return PrefiksFormater((long)broj);
		}

		public static string PrefiksFormater(long broj)
		{
			string[] prefiksi = new string[] { "", " k", " M", " G", " T", " P", " E", " Z", " Y" };

			int prefIndex = 0;
			double tezina = 1;
			for (; prefIndex < prefiksi.Length && broj >= tezina * 1000; prefIndex++)
				tezina *= 1000;

			string ret = (broj / tezina).ToString("0.###");

			if (ret.Length > 5)
				return ret.Substring(0, 5) + prefiksi[prefIndex];
			else
				return ret + prefiksi[prefIndex];
		}

		public static double IntegralPolinoma(double x, double redPolinoma)
		{
			if (redPolinoma == 0)
				return x;
			else
				return Math.Pow(x, redPolinoma + 1) / (redPolinoma + 1);
		}

	}

	public class Sazetak
	{
		private uint[] sadrzaj;
		private int hash;

		public Sazetak(List<uint> sadrzaj, List<uint> maxVelicina)
		{
			List<uint> tmpSadrzaj = new List<uint>();
			uint trenutni = 0;
			uint faktor = 1;

			for (int i = 0; i < sadrzaj.Count; i++)
			{
				if (uint.MaxValue / maxVelicina[i] < faktor)
				{
					faktor = 1;
					tmpSadrzaj.Add(trenutni);
					trenutni = 0;
				}

				trenutni = sadrzaj[i] * faktor;

				uint faktorKorak = 2;
				while (faktorKorak < maxVelicina[i])
					faktorKorak *= 2;
				faktor *= faktorKorak;
			}
			if (faktor > 1)
				tmpSadrzaj.Add(trenutni);

			this.sadrzaj = tmpSadrzaj.ToArray();
			this.hash = 0;

			foreach(uint i in sadrzaj)
			{
				hash *= 31;
				hash += i.GetHashCode();
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() != GetType()) return false;
			
			Sazetak o = (Sazetak)obj;
			if (o.sadrzaj.Length != sadrzaj.Length) return false;
			for (int i = 0; i < sadrzaj.Length; i++)
				if (o.sadrzaj[i] != sadrzaj[i])
					return false;
			
			return true;
		}

		public override int GetHashCode()
		{
			return hash;
		}
	}

	public class Vadjenje<T>
	{
		private List<T> elementi;

		private static Random random = new Random();

		public Vadjenje()
		{
			this.elementi = new List<T>();
		}

		public Vadjenje(List<T> lista)
		{
			this.elementi = new List<T>(lista);
		}

		public T izvadi()
		{
			return izvadi(false);
		}

		public T izvadi(bool ostavi)
		{
			if (elementi.Count < 1)
				return default(T);

			int kojeg = random.Next(elementi.Count);
			T ret = elementi[kojeg];
			if (!ostavi)
			{
				elementi[kojeg] = elementi[elementi.Count - 1];
				elementi.RemoveAt(elementi.Count - 1);
			}

			return ret;
		}

		public void dodaj(T element)
		{
			elementi.Add(element);
		}

		public int kolicina()
		{
			return elementi.Count;
		}

		public List<T> lista
		{
			get
			{
				return elementi;
			}
		}
	}

	public class TagTekst<T>
	{
		public T tag;
		public string tekst;

		public TagTekst(T tag, string tekst)
		{
			this.tag = tag;
			this.tekst = tekst;
		}

		public override string ToString()
		{
			return tekst;
		}
	}

	public class Tocka<T>
	{
		public T x;
		public T y;
		public Tocka(T x, T y)
		{
			this.x = x;
			this.y = y;
		}
	}
	
}

