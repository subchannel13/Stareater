using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Podaci.Jezici.Elementi
{
	public class TekstVrijednost : ITekst
	{
		public static TekstVrijednost Nacini(Queue<string> linija)
		{
			string var = linija.Dequeue();

			return new TekstVrijednost(var);
		}

		public TekstVrijednost(string var)
		{
			this.varijabla = var;
		}

		private string varijabla;

		#region ITekst Members

		public string tekst()
		{
			throw new InvalidOperationException();
		}

		public string tekst(Dictionary<string, double> varijable)
		{
			throw new InvalidOperationException();
		}

		public string tekst(Dictionary<string, double> varijable, Dictionary<string, string> testVarijable)
		{
			return testVarijable[varijabla];
		}

		#endregion
	}
}
