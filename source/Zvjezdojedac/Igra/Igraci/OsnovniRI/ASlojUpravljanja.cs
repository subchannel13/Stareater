using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Igra.Igraci.OsnovniRI
{
	abstract class ASlojUpravljanja
	{
		protected ORIKoordinator koordinator;
		protected Igrac igrac;

		protected ASlojUpravljanja(ORIKoordinator koordinator)
		{
			this.igrac = koordinator.Igrac;
			this.koordinator = koordinator;
		}

		public abstract void OdrediZahtjeve(IgraZvj igra);
		public abstract void Djeluj(IgraZvj igra);
	}
}
