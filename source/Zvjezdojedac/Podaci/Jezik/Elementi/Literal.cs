using System;
using System.Collections.Generic;
using System.Text;

namespace Prototip.Podaci.Jezik.Elementi
{
	public class Literal: ITekst
	{
		public static Literal Nacini(Queue<string> linija)
		{
			StringBuilder sb = new StringBuilder();
			bool citaj = true;

			do {
				string rijec = linija.Dequeue();
				if (rijec.StartsWith("\""))
					rijec.Remove(0, 1);
				
				if (rijec.EndsWith("\"")) {
					rijec.Remove(rijec.Length - 1, 1);
					if (!rijec.EndsWith("\"\""))
						citaj = false;
				}
				sb.Append(rijec);
				if (citaj) sb.Append(" ");
			} while (citaj);

			return new Literal(sb.ToString());
		}

		public Literal(string tekst)
		{
			this._tekst = tekst;
		}

		private string _tekst;

		#region ITekstElement Members

		public string tekst(Dictionary<string, double> varijable)
		{
			return _tekst;
		}

		#endregion
	}
}
