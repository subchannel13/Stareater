using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Alati;

namespace Zvjezdojedac.Igra.Igraci.OsnovniRI
{
	class ORIZvijezdaniSloj : ASlojUpravljanja
	{
		public ORIZvijezdaniSloj(ORIKoordinator koordinator)
			: base(koordinator)
		{ }

		public override void OdrediZahtjeve(IgraZvj igra)
		{
			// ne radi nista
		}

		public override void Djeluj(IgraZvj igra)
		{
			foreach(var zvj in igra.mapa.zvijezde)
				if (zvj.uprave[igrac.id] != null) {
					ZvjezdanaUprava uprava = zvj.uprave[igrac.id];

					if (uprava.RedGradnje.Count == 0) {
						Vadjenje<Zgrada.ZgradaInfo> moguceGraditi = new Vadjenje<Zgrada.ZgradaInfo>(uprava.MoguceGraditi());

						if (moguceGraditi.lista.Count > 0)
							uprava.RedGradnje.AddLast(moguceGraditi.izvadi());
					}
					uprava.UdioGradnje = 1;
				}
		}
	}
}
