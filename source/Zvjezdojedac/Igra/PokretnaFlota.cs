using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototip.Igra
{
	public class PokretnaFlota : Flota
	{
		public Zvijezda polaznaZvj;
		public Zvijezda odredisnaZvj;

		public PokretnaFlota(Zvijezda polaznaZvj, Zvijezda odredisnaZvj,
			int id)
			: base(polaznaZvj, id)
		{
			this.odredisnaZvj = odredisnaZvj;
			this.polaznaZvj = polaznaZvj;
		}
	}
}
