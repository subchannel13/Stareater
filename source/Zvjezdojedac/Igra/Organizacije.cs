using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Podaci;

namespace Zvjezdojedac.Igra
{
	public class Organizacija : IIdentifiable
	{
		#region Statično
		public static List<Organizacija> lista
		{
			get;
			private set;
		}

		public static void dodajOrganizaciju(Dictionary<string, string> podatci)
		{
			if (lista == null) lista = new List<Organizacija>();

			List<string> pocetneTehnologije = new List<string>();
			foreach (string tehKod in podatci.Keys)
				if (tehKod.StartsWith("TECH"))
					pocetneTehnologije.Add(podatci[tehKod]);
			
			lista.Add(new Organizacija(
				lista.Count,
				podatci["NAZIV"], 
				podatci["OPIS"], 
				pocetneTehnologije));
		}

		#endregion

		public int id { get; private set; }
		public string naziv { get; private set; }
		public string opis { get; private set; }
		public List<string> pocetneTehnologije { get; private set; }

		public Organizacija(int id, string naziv, string opis, List<string> pocetneTehnologije)
		{
			this.id = id;
			this.naziv = naziv;
			this.opis = opis;
			this.pocetneTehnologije = pocetneTehnologije;
		}
	}
}
