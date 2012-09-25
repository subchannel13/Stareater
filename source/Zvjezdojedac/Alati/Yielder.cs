using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Alati
{
	static class Yielder
	{
		public static IEnumerable<int> Raspon(int prviBr, int zadnjiBr, int korak, bool ukljuciviZadnji = false)
		{
			int x = prviBr;
			for (; x < zadnjiBr; x += korak)
				yield return x;

			if (x == zadnjiBr && ukljuciviZadnji)
				yield return x;
		}
	}
}
