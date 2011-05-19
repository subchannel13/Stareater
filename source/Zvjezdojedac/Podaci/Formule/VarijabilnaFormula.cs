using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Podaci.Formule
{
	public class VarijabilnaFormula : Formula
	{
		private Element vrh;
		private List<Varijabla> varijable;

		public VarijabilnaFormula(Element vrh, List<Varijabla> varijable)
		{
			this.vrh = vrh;
			this.varijable = varijable;
		}

		public override double iznos(Dictionary<string, double> varijable)
		{
			foreach (Varijabla v in this.varijable)
				v.postaviVrijednost(varijable[v.ime]);

			return vrh.vrijednost();
		}

		public override List<Formula.Varijabla> popisVarijabli()
		{
			List<Varijabla> varijable = new List<Varijabla>();
			vrh.popisiVarijable(varijable);
			return varijable;
		}

		public override void preimenujVarijablu(string staroIme, string novoIme)
		{
			List<Varijabla> varijable = popisVarijabli();
			foreach (Varijabla v in varijable)
				if (v.ime == staroIme)
					v.ime = novoIme;
		}

		public override string ToString()
		{
			return vrh.ToString();
		}
	}
}
