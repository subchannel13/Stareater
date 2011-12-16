using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Zvjezdojedac.Igra.Brodovi
{
	public interface IKomponenta
	{
		int nivo { get; }
		string naziv { get; }
		string opis { get; }
		Image slika { get; }
	}
}
