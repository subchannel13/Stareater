using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Prototip.Podaci.Jezici
{
	public class Jezik
	{
		public static Dictionary<string, string> Popis = new Dictionary<string,string>();
		public static string UobicajeniJezik;

		private static Dictionary<string, Kontekst> StringUKontekst = initStringUKontekst();
		private static Dictionary<string, Kontekst> initStringUKontekst()
		{
			Dictionary<string, Kontekst> rez = new Dictionary<string,Kontekst>();
			rez.Add("FORM_GRADNJA", Kontekst.FormGradnja);
			rez.Add("FORM_IGRA", Kontekst.FormIgra);
			rez.Add("FORM_KOLONIZACIJA", Kontekst.FormKolonizacija);
			rez.Add("FORM_MAIN", Kontekst.FormMain);
			rez.Add("FORM_NOVA_IGRA", Kontekst.FormNovaIgra);
			rez.Add("FORM_PLANET_INFO", Kontekst.FormPlanetInfo);
			rez.Add("FORM_PORUKE", Kontekst.FormPoruke);
			rez.Add("FORM_POSTAVKE", Kontekst.FormPostavke);
			rez.Add("FORM_TECH", Kontekst.FormTech);
			rez.Add("KOLONIJA", Kontekst.Kolonija);
			rez.Add("TEHNOLOGIJE", Kontekst.Tehnologije);
			rez.Add("VELICINA_MAPE", Kontekst.VelicinaMape);
			rez.Add("WINDOWS_DIJALOZI", Kontekst.WindowsDijalozi);
			rez.Add("ZGRADE", Kontekst.Zgrade);

			return rez;
		}

		public static Jezik IzDatoteka(string kod)
		{
			string infoPut = "./jezici/" + kod + "/";
			StreamReader citacPopisa = new StreamReader(infoPut + "info.txt");
			string nazivJezika = citacPopisa.ReadLine();

			List<string> datoteke = new List<string>();
			while (!citacPopisa.EndOfStream) {
				string linija = citacPopisa.ReadLine().Trim();
				if (linija.Length > 0)
					datoteke.Add(linija);
			}
			citacPopisa.Close();

			Dictionary<Kontekst, Queue<string>> redovi = new Dictionary<Kontekst, Queue<string>>();
			foreach (string datoteka in datoteke) {
				Kontekst kontekst = Kontekst.Opcenito;
				StreamReader citac = new StreamReader(infoPut + datoteka);

				while (!citac.EndOfStream) {
					string linija = citac.ReadLine();
					if (linija.Length == 0) continue;

					if (linija.StartsWith("::")) {
						kontekst = StringUKontekst[linija.Substring(2)];
						if (!redovi.ContainsKey(kontekst))
							redovi.Add(kontekst, new Queue<string>());
					}
					else
						redovi[kontekst].Enqueue(linija);
				}
				citac.Close();
			}

			Dictionary<Kontekst, Dictionary<string, ITekst>> tablica = new Dictionary<Kontekst, Dictionary<string, ITekst>>();
			foreach (Kontekst kontekst in redovi.Keys) {
				tablica.Add(kontekst, new Dictionary<string, ITekst>());
				foreach (KeyValuePair<string, ITekst> redak in Tekst.IzStringa(redovi[kontekst]))
					tablica[kontekst].Add(redak.Key, redak.Value);
			}

			return new Jezik(nazivJezika, kod, tablica);
		}

		private Jezik(string nazivJezika, string kod, Dictionary<Kontekst, Dictionary<string, ITekst>> tablica)
		{
			this.kod = kod;
			this.naziv = nazivJezika;
			this.tablica = tablica;
		}

		public string naziv { get; private set; }
		public string kod { get; private set; }
		private Dictionary<Kontekst, Dictionary<string, ITekst>> tablica;

		public Dictionary<string, ITekst> this[Kontekst kontekst]
		{
			get { return tablica[kontekst]; }
		}

		public ITekst this[Kontekst kontekst, string kljuc]
		{
			get { return tablica[kontekst][kljuc]; }
		}
	}
}
