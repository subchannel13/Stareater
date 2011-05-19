using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Alati
{
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
