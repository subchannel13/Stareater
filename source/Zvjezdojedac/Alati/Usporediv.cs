using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Alati
{
	public class Usporediv<T, C> : IComparable<Usporediv<T, C>> where C : IComparable<C>
	{
		public C usporedjivac;
		public T objekt;

		public Usporediv(T objekt, C usporedjivac)
		{
			this.objekt = objekt;
			this.usporedjivac = usporedjivac;
		}

		public int CompareTo(Usporediv<T, C> other)
		{
			return usporedjivac.CompareTo(other.usporedjivac);
		}
	}
}
