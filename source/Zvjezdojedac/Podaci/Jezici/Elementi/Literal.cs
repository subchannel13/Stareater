using System;
using System.Collections.Generic;
using System.Text;

namespace Zvjezdojedac.Podaci.Jezici.Elementi
{
	public class Literal: ITekst
	{
		public static Literal Nacini(Queue<string> linija)
		{
			StringBuilder sb = new StringBuilder();
			bool citaj = true;
			bool prvaRijec = true;

			do {
				string rijec = linija.Dequeue();

				if (prvaRijec) {
					rijec = rijec.Remove(0, 1);
					prvaRijec = false;
				}
				
				if (rijec.EndsWith("\"")) {
					rijec = rijec.Remove(rijec.Length - 1, 1);
					if (!rijec.EndsWith("\""))
						citaj = false;
					if (rijec.EndsWith("\"\"")) {
						rijec = rijec.Remove(rijec.Length - 1, 1);
						citaj = false;
					}
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

		public string tekst()
		{
			return _tekst;
		}

		public string tekst(Dictionary<string, double> varijable)
		{
			return _tekst;
		}

		public string tekst(Dictionary<string, double> varijable, Dictionary<string, string> tekstVarijable)
		{
			return _tekst;
		}
		#endregion
	}
}
