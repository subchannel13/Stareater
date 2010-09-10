using System;
using System.Collections.Generic;
using System.Text;
using Alati;

namespace Prototip.Podaci.Jezici.Elementi
{
	public class Vrijednost : ITekst
	{
		public static Vrijednost Nacini(Queue<string> linija)
		{
			string formaterTip = linija.Dequeue();

			StringBuilder sb = new StringBuilder();
			for (string rijec = linija.Dequeue(); rijec != "#"; rijec = linija.Dequeue()) {
				sb.Append(rijec);
				sb.Append(" ");
			}

			return new Vrijednost(formaterTip, Formula.IzStringa(sb.ToString()));
		}

		public Vrijednost(string format, Formula formula)
		{
			this.format = format;
			this.formula = formula;
		}

		private string format;
		private Formula formula;

		#region ITekst Members

		public string tekst(Dictionary<string, double> varijable)
		{
			if (format == "D")
				return String.Format("{0:0.##}", formula.iznos(varijable));
			else
				return Fje.PrefiksFormater(formula.iznos(varijable));
		}

		#endregion
	}
}
