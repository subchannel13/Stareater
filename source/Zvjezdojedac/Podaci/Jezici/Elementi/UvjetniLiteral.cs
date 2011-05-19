using System;
using System.Collections.Generic;
using System.Text;
using Zvjezdojedac.Podaci.Formule;

namespace Zvjezdojedac.Podaci.Jezici.Elementi
{
	public class UvjetniLiteral : ITekst
	{
		public static UvjetniLiteral Nacini(Queue<string> linija)
		{
			StringBuilder sb = new StringBuilder();
			for (string rijec = linija.Dequeue(); rijec != ":"; rijec = linija.Dequeue()) {
				sb.Append(rijec);
				sb.Append(" ");
			}

			Formula formula = Formula.IzStringa(sb.ToString());
			if (linija.Peek() == "#") {
				linija.Dequeue();
				return new UvjetniLiteral(formula, Vrijednost.Nacini(linija));
			}
			else if (linija.Peek() == "$") {
				linija.Dequeue();
				return new UvjetniLiteral(formula, TekstVrijednost.Nacini(linija));
			}
			else
				return new UvjetniLiteral(formula, Literal.Nacini(linija));
		}

		private UvjetniLiteral(Formula uvjet, ITekst izlaz)
		{
			this.izlaz = izlaz;
			this.uvjet = uvjet;
		}

		private Formula uvjet;
		private ITekst izlaz;

		#region ITekst Members

		public string tekst()
		{
			return tekst(null, null);
		}

		public string tekst(Dictionary<string, double> varijable)
		{
			if (uvjet.istina(varijable))
				return izlaz.tekst(varijable);
			else
				return "";
		}

		public string tekst(Dictionary<string, double> varijable, Dictionary<string, string> tekstVarijable)
		{
			if (uvjet.istina(varijable))
				return izlaz.tekst(varijable, tekstVarijable);
			else
				return "";
		}

		#endregion
	}
}
