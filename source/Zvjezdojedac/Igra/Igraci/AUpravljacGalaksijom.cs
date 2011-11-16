using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Igra.Igraci
{
	abstract class AUpravljacGalaksijom
	{
		protected ARacunalniIgrac koordinator;

		public AUpravljacGalaksijom(ARacunalniIgrac koordinator)
		{
			this.koordinator = koordinator;
		}

		public abstract void OdrediZahtjeve();
		public abstract void Djeluj();
	}
}
