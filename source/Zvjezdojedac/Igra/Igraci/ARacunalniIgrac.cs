using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Igra.Igraci
{
	abstract class ARacunalniIgrac
	{
		public Igrac Igrac { get; private set; }

		protected AUpravljacGalaksijom upravljacGalaksijom;

		protected ARacunalniIgrac(Igrac igrac,
			AUpravljacGalaksijom upravljacGalaksijom)
		{
			this.Igrac = igrac;
			
			this.upravljacGalaksijom = upravljacGalaksijom;
		}

		public void OdigrajKrug()
		{
			upravljacGalaksijom.OdrediZahtjeve();
			upravljacGalaksijom.Djeluj();
		}
	}
}
