using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Alati
{
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
}
