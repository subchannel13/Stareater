using System;
using System.Collections.Generic;
using System.Text;

namespace Prototip.Podaci.Jezici
{
	public interface ITekst
	{
		string tekst();
		string tekst(Dictionary<string, double> varijable);
		string tekst(Dictionary<string, double> varijable, Dictionary<string, string> tekstVarijable);
	}
}
