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

		public string tekst()
		{
			return tekst(null);
		}

		public string tekst(Dictionary<string, double> varijable)
		{
			if (format == "D" || format == "D2")
				return String.Format("{0:0.##}", formula.iznos(varijable));
			if (format == "D1")
				return String.Format("{0:0.#}", formula.iznos(varijable));
			else if (format == "P")
				return Fje.PrefiksFormater(formula.iznos(varijable));

			throw new FormatException();
		}

		public string tekst(Dictionary<string, double> varijable, Dictionary<string, string> tekstVarijable)
		{
			return tekst(varijable);
		}

		#endregion
	}
}
