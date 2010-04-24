using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototip
{
	public class Organizacije
	{
		private static List<Organizacije> _lista = new List<Organizacije>();

		public static List<Organizacije> lista
		{
			get { return _lista; }
		}

		public static void dodajOrganizaciju(Dictionary<string, string> podatci)
		{
			List<string> pocetneTehnologije = new List<string>();
			foreach (string tehKod in podatci.Keys)
				if (tehKod.StartsWith("TECH"))
					pocetneTehnologije.Add(podatci[tehKod]);
			
			_lista.Add(new Organizacije(podatci["NAZIV"], podatci["OPIS"], pocetneTehnologije));
		}

		public Organizacije(string naziv, string opis, List<string> pocetneTehnologije)
		{
			this.naziv = naziv;
			this.opis = opis;
			this.pocetneTehnologije = pocetneTehnologije;
		}

		public string naziv;
		
		public string opis;

		public List<string> pocetneTehnologije;
	}
}
