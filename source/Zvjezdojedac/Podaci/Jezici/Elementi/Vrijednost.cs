using System;
using System.Collections.Generic;
using System.Text;
using Alati;

namespace Prototip.Podaci.Jezici.Elementi
{
	public class Vrijednost : ITekst
	{
		private enum FormatTip
		{
			Decimalan,
			Prefiks
		}
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
			this.formula = formula;

			if (format.StartsWith("D")) {
				this.formatTip = FormatTip.Decimalan;
				if (format.Length > 1) {
					int preciznost = int.Parse(format.Substring(1));
					this.format = "0:0.";
					for (int i = 0; i < preciznost; i++)
						this.format += "#";
					this.format = "{" + this.format + "}";
				}
				else
					this.format = "{0:0.##}";
			}
			else
				formatTip = FormatTip.Prefiks;
		}

		private FormatTip formatTip;
		private string format;
		private Formula formula;

		#region ITekst Members

		public string tekst()
		{
			return tekst(null);
		}

		public string tekst(Dictionary<string, double> varijable)
		{
			if (formatTip == FormatTip.Decimalan)
				return String.Format(format, formula.iznos(varijable));
			else 
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
