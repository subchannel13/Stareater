using System;
using System.Collections.Generic;
using System.Text;

namespace Prototip.Podaci.Jezici
{
	public interface ITekst
	{
		string tekst();
		string tekst(Dictionary<string, double> varijable);
	}
}
