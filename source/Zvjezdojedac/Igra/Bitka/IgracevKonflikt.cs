using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Igra.Bitka
{
	class IgracevKonflikt
	{
		public const int MaxBrPoteza = 15;

		public int BrojPoteza { get; private set; }
		public StanjeKonflikta Stanje { get; private set; }

		public IgracevKonflikt()
		{
			this.BrojPoteza = MaxBrPoteza;
			this.Stanje = StanjeKonflikta.SvemirskiSukob;
		}

		public void ZavrsiKrug()
		{
			BrojPoteza--;
		}

		public void SlijedecaFaza()
		{
			/*switch (Stanje) {
				case StanjeKonflikta.SvemirskiSukob:
					Stanje = StanjeKonflikta.Bombardiranje;
					break;
				case StanjeKonflikta.Bombardiranje:
					Stanje = StanjeKonflikta.Invazija;
					break;
				case StanjeKonflikta.Invazija:
					Stanje = StanjeKonflikta.Razrijeseno;
					break;
			}*/
			Stanje = StanjeKonflikta.Razrijeseno;
		}
	}
}
