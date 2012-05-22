using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Igra.Poruke;
using Zvjezdojedac.Igra.Bitka;

namespace Zvjezdojedac.Igra.Igraci.OsnovniRI
{
	class ORIKoordinator : IUpravljacIgraca
	{
		public Igrac Igrac { get; private set; }

		private ASlojUpravljanja[] podSlojevi;

		public HashSet<Zvijezda> PromjeniRedGradnje = new HashSet<Zvijezda>();
		
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
			PromjeniRedGradnje.Clear();
			foreach(var poruka in Igrac.poruke)
				if (poruka.tip == Poruka.Tip.Brod) {
					PorukaBrod porukaBrod = poruka as PorukaBrod;
					PromjeniRedGradnje.Add(porukaBrod.zvijezda);
				}

			// pozivi moraju biti asocijativni i komutativni (neovisni o redoslijedu)
			foreach (var sloj in podSlojevi)
				sloj.OdrediZahtjeve(igra);

			foreach (var sloj in podSlojevi)
				sloj.Djeluj(igra);
		}

		public void OdigrajKrugBitke(ModeratorBorbe bitka)
		{
		}
	}
}
