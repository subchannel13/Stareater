using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototip
{
	public class Organizacije : IIdentifiable
	{
		public static List<Organizacije> lista
		{
			get;
			private set;
		}

		public static void dodajOrganizaciju(Dictionary<string, string> podatci)
		{
			if (lista == null) lista = new List<Organizacije>();

			List<string> pocetneTehnologije = new List<string>();
			foreach (string tehKod in podatci.Keys)
				if (tehKod.StartsWith("TECH"))
					pocetneTehnologije.Add(podatci[tehKod]);
			
			lista.Add(new Organizacije(podatci["NAZIV"], podatci["OPIS"], pocetneTehnologije));
		}

		public int id { get; private set; }
		public string naziv { get; private set; }
		public string opis { get; private set; }
		public List<string> pocetneTehnologije { get; private set; }

		public Organizacije(string naziv, string opis, List<string> pocetneTehnologije)
		{
			this.naziv = naziv;
			this.opis = opis;
			this.pocetneTehnologije = pocetneTehnologije;
		}
	}
}
