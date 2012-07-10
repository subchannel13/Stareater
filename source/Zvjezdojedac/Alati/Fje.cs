using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Zvjezdojedac.Alati
{
	public static class Fje
	{
		public static Random Random = new Random();


		public static int BinarySearch(IList list, object obj, Comparison<object> comparator)
		{
			int min = 0;
			int max = list.Count;

			while (max != min) {
				int testI = (min + max) / 2;
				int equality = comparator(list[testI], obj);

				if (equality == 0)
					return testI;
				else if (equality < 0)
					min = ((max - min) % 2 == 0) ? testI : testI + 1;
				else
					max = testI;
			}

			return min;
		}

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

		public static string PrefiksFormater(double broj, string infinityText)
		{
			if (double.IsPositiveInfinity(broj))
				return infinityText;
			else
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

		public static int Multikriterij(params int[] kriteriji)
		{
			for (int i = 0; i < kriteriji.Length; i++)
				if (kriteriji[i] != 0)
					return kriteriji[i];

			return 0;
		}
	}
}

