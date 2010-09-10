using System;
using System.Collections.Generic;
using System.Text;

namespace Prototip.Podaci.Jezici.Elementi
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
			if (linija.Peek() == "$")
				return new UvjetniLiteral(formula, Vrijednost.Nacini(linija));
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

		public string tekst(Dictionary<string, double> varijable)
		{
			if (uvjet.istina(varijable))
				return izlaz.tekst(varijable);
			else
				return "";
		}

		#endregion
	}
}
