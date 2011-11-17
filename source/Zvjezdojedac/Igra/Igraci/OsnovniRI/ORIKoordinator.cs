using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Igra.Igraci.OsnovniRI
{
	class ORIKoordinator : IUpravljacIgraca
	{
		public Igrac Igrac { get; private set; }

		private ASlojUpravljanja[] podSlojevi;
		
		public ORIKoordinator(Igrac igrac)
		{
			this.Igrac = igrac;

			podSlojevi = new ASlojUpravljanja[]{
				new ORIGalaktickiSloj(this),
				new ORIZvijezdaniSloj(this),
				new ORIPlanetarniSloj(this)
			};
		}

		public void OdigrajKrug(IgraZvj igra)
		{
			// pozivi moraju biti asocijativni i komutativni (neovisni o redoslijedu)
			foreach (var sloj in podSlojevi)
				sloj.OdrediZahtjeve(igra);

			foreach (var sloj in podSlojevi)
				sloj.Djeluj(igra);
		}
	}
}
