using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Igra
{
	public interface IGradiliste
	{
		Dictionary<string, double> Efekti { get; }
		Igrac Igrac { get; }
		Zvijezda LokacijaZvj { get; }
		LinkedList<Zgrada.ZgradaInfo> RedGradnje { get; }
		List<Zgrada.ZgradaInfo> MoguceGraditi();
	}
}
