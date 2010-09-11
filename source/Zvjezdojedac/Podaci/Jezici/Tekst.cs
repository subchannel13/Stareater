using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Prototip.Podaci.Jezici
{
	public class Tekst : ITekst
	{
		#region Parser
		public static IEnumerable<KeyValuePair<string, ITekst>> IzStringa(Queue<string> ulaz)
		{
			List<KeyValuePair<string, ITekst>> rez = new List<KeyValuePair<string, ITekst>>();

			while (ulaz.Count > 0) {
				Queue<string> linija = new Queue<string>(ulaz.Dequeue().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
				string kljuc = linija.Dequeue();
				ITekst vrijedost = null;

				string format = linija.Dequeue();
				if (format == "=")
					vrijedost = JednostavanFormat(linija);
				else if (format == ":=")
					vrijedost = LinijskiFormat(linija);
				else if (format == "{")
					vrijedost = BlokFormat(ulaz);
				else
					throw new FormatException();

				rez.Add(new KeyValuePair<string, ITekst>(kljuc, vrijedost));
			}

			return rez;
		}

		private class Supstitucija
		{
			public int pozicija { get; private set; }
			public ITekst element { get; private set; }
			public int kraj { get; private set; }

			public Supstitucija(int pozicija, ITekst element, int duljina)
			{
				this.kraj = pozicija + duljina;
				this.pozicija = pozicija;
				this.element = element;
			}
		}

		private static ITekst JednostavanFormat(Queue<string> linija)
		{
			StringBuilder sb = new StringBuilder();
			while (linija.Count > 0) {
				sb.Append(linija.Dequeue());
				if (linija.Count > 0)
					sb.Append(" ");
			}

			return new Elementi.Literal(sb.ToString());
		}

		private static ITekst LinijskiFormat(Queue<string> linija)
		{
			List<ITekst> elementi = new List<ITekst>();
			while (linija.Count > 0)
				elementi.Add(NaciniElement(linija));

			if (elementi.Count == 1)
				return elementi[0];
			else
				return new Tekst(elementi);
		}

		private static Tekst BlokFormat(Queue<string> ulaz)
		{
			List<string> linije = new List<string>();
			string linija;
			for (linija = ulaz.Dequeue(); !linija.Trim().StartsWith("}"); linija = ulaz.Dequeue())
				linije.Add(linija);

			int uvlacenje = int.Parse(linija.TrimStart().Remove(0, 1).Trim());
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < linije.Count; i++) {
				if (uvlacenje > 0)
					linije[i] = linije[i].Remove(0, uvlacenje);
				sb.Append(linije[i]);
				sb.Append("\n");
			}
			string tekst = sb.ToString();

			List<Supstitucija> supstitucije = new List<Supstitucija>();
			for (linija = ulaz.Dequeue(); !linija.Trim().EndsWith(";"); linija = ulaz.Dequeue()) {
				Queue<string> linijaQ = new Queue<string>(linija.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
				
				string kljuc = linijaQ.Dequeue();
				ITekst vrijednost = LinijskiFormat(linijaQ);
				
				for (int poz = tekst.IndexOf(kljuc); poz >= 0; poz = tekst.IndexOf(kljuc, poz + 1))
					supstitucije.Add(new Supstitucija(poz, vrijednost, kljuc.Length));
			}
			supstitucije.Sort((a, b) => a.pozicija.CompareTo(b.pozicija));

			List<ITekst> elementi = new List<ITekst>();
			int pocetak = 0;
			foreach (Supstitucija sups in supstitucije) {
				int duljinaLiterala = sups.pozicija - pocetak;
				if (duljinaLiterala > 0)
					elementi.Add(new Elementi.Literal(tekst.Substring(pocetak, duljinaLiterala)));
				elementi.Add(sups.element);
				pocetak = sups.kraj;
			}
			if (pocetak < tekst.Length)
				elementi.Add(new Elementi.Literal(tekst.Substring(pocetak)));

			return new Tekst(elementi);
		}

		public static ITekst NaciniElement(Queue<string> linija)
		{
			if (linija.Peek().StartsWith("\""))
				return Elementi.Literal.Nacini(linija);

			string tip = linija.Dequeue();
			if (tip == "?")
				return Elementi.UvjetniLiteral.Nacini(linija);
			else if (tip == "$")
				return Elementi.Vrijednost.Nacini(linija);

			throw new FormatException();
		}
		#endregion

		public Tekst(IEnumerable<ITekst> elementi)
		{
			this.elementi = elementi;
		}

		private IEnumerable<ITekst> elementi;

		#region ITekst Members

		public string tekst(Dictionary<string, double> varijable)
		{
			StringBuilder sb = new StringBuilder();
			foreach (ITekst element in elementi)
				sb.Append(element.tekst(varijable));

			return sb.ToString();
		}

		#endregion
	}
}
