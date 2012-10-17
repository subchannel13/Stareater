using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Utils
{
	public static class Methods
	{
		public static IEnumerable<int> Range(int first, int last, int step, bool skipLast = true)
		{
			int x = first;
			for (; x < last; x += step)
				yield return x;

			if (x == last && skipLast)
				yield return x;
		}
	}
}
