using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Podaci.Formule
{
	public class KonstantnaFormula : Formula
	{
		private double _iznos;

		public KonstantnaFormula(double iznos)
		{
			this._iznos = iznos;
		}

		public override double iznos(Dictionary<string, double> varijable)
		{
			return _iznos;
		}

		public override string ToString()
		{
			return _iznos.ToString(PodaciAlat.DecimalnaTocka);
		}

		public override List<Varijabla> popisVarijabli()
		{
			return new List<Varijabla>();
		}

		public override void preimenujVarijablu(string staroIme, string novoIme)
		{
			return;
		}
	}
}
