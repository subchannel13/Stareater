using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Podaci
{
	public class PodaciPisac
	{
		const int Uvlacenje = 4;
		
		private string uvlacenje = null;
		public List<string> linije = new List<string>();

		public PodaciPisac(string tip)
			: this(tip, 0)
		{ }

		private PodaciPisac(string tip, int uvuceno)
		{
			this.uvlacenje = new string(' ', uvuceno * Uvlacenje);
			linije.Add(uvlacenje + "<" + tip + ">");
		}

		public PodaciPisac podPodatak(string tip)
		{
			return new PodaciPisac(tip, uvlacenje.Length / Uvlacenje + 1);
		}

		public void dodaj(string kljuc, string vrijednost)
		{
			linije.Add(uvlacenje + kljuc + " = " + vrijednost);
		}

		public void dodaj(string kljuc, int vrijednost)
		{
			linije.Add(uvlacenje + kljuc + " = " + vrijednost);
		}

		public void dodaj(string kljuc, long vrijednost)
		{
			linije.Add(uvlacenje + kljuc + " = " + vrijednost);
		}

		public void dodaj(string kljuc, double vrijednost)
		{
			linije.Add(uvlacenje + kljuc + " = " + vrijednost.ToString(PodaciAlat.DecimalnaTocka));
		}

		public void dodaj(PodaciPisac podaci)
		{
			linije.AddRange(podaci.linije);
			linije.Add(podaci.uvlacenje + "----");
		}

		public void dodaj(string tip, IPohranjivoSB objekt)
		{
			PodaciPisac podPodatak = this.podPodatak(tip);
			objekt.pohrani(podPodatak);
			dodaj(podPodatak);
		}

		public void dodaj(string kljuc, IIdentifiable objekt)
		{
			linije.Add(uvlacenje + kljuc + " = " + objekt.id);
		}

		public void dodajIdeve<T>(string kljuc, IEnumerable<T> kolekcija) where T : IIdentifiable
		{
			StringBuilder sb = new StringBuilder();
			foreach (IIdentifiable element in kolekcija)
				sb.Append(element.id + " ");
			linije.Add(uvlacenje + kljuc + " = " + sb.ToString());
		}

		public void dodajKolekciju<T>(string tip, IEnumerable<T> kolekcija) where T : IPohranjivoSB
		{
			int i = 0;
			foreach (IPohranjivoSB element in kolekcija) {
				dodaj(tip + i, element);
				i++;
			}
		}

		public void noviRed()
		{
			linije.Add("");
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			for(int i = 0; i < linije.Count; i++) {
				sb.Append(linije[i]);
				sb.Append('\n');
			}
			sb.Append("----");

			return sb.ToString();
		}
	}
}
