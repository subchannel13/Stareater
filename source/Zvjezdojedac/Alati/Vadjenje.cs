using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Alati
{
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
			if (!ostavi) {
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
}
