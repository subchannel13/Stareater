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

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (this.GetType() != obj.GetType())
				return false;

			TagTekst<T> other = obj as TagTekst<T>;
			return tag.Equals(other.tag);
		}

		public override int GetHashCode()
		{
			return tag.GetHashCode();
		}

		public override string ToString()
		{
			return tekst;
		}
	}
}
