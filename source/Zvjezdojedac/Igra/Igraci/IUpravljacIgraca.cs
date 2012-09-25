using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Igra.Bitka;

namespace Zvjezdojedac.Igra.Igraci
{
	public interface IUpravljacIgraca
	{
		void OdigrajKrug(IgraZvj igra);
		void OdigrajKrugBitke(ModeratorBorbe bitka);
	}
}
