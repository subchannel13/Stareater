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

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != this.GetType())
				return false;

			return (this.CompareTo(obj as Usporediv<T, C>) == 0);
		}

		public override int GetHashCode()
		{
			return objekt.GetHashCode();
		}
	}
}
