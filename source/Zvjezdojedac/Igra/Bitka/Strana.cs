using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Igra.Brodovi;
using Zvjezdojedac.Alati.Strukture;

namespace Zvjezdojedac.Igra.Bitka
{
	public class Strana
	{
		public Igrac Igrac { get; private set; }
		public MySet<Borac> Borci;
		public HashSet<Borac> PaliBorci = new HashSet<Borac>();
		
		double[] snagaSenzora = new double[Pozicije.MaxPozicija + 1];
		double snagaSenzoraFlote;
		int pozicijaSenzoraFlote;

		public Strana(Igrac igrac, MySet<Borac> borci)
		{
			this.Borci = borci;
			this.Igrac = igrac;
		}

		public void PostaviSnaguSenzora(double[] snagaSenzora)
		{
			double snagaSenzoraFlote = double.NaN;
			pozicijaSenzoraFlote = int.MaxValue;

			for (int pozicija = 0; pozicija < snagaSenzora.Length; pozicija++) {
				int senzorSPozicije = 0;
				double snagaNaPoziciji = double.NaN;

				for (int i = 0; i < snagaSenzora.Length; i++) {
					if (double.IsNaN(snagaSenzora[i])) continue;

					double efektUdaljenosti = Pozicije.EfektUdaljenosti.Izracunaj(Math.Abs(pozicija - i)).SnagaSenzora;
					double snaga = snagaSenzora[i] + efektUdaljenosti;
					if (double.IsNaN(snagaNaPoziciji) || snaga > snagaNaPoziciji) {
						senzorSPozicije = i;
						snagaNaPoziciji = snaga;
					}
				}

				this.snagaSenzora[pozicija] = snagaNaPoziciji;
				if (!double.IsNaN(snagaNaPoziciji)) {
					double efektUdaljenosti = Pozicije.EfektUdaljenosti.Izracunaj(pozicija).SnagaSenzora;
					if (double.IsNaN(snagaSenzoraFlote) || snagaSenzora[senzorSPozicije] + efektUdaljenosti > snagaSenzoraFlote) {
						this.snagaSenzoraFlote = snagaSenzora[senzorSPozicije];
						snagaSenzoraFlote = snagaSenzora[senzorSPozicije] + efektUdaljenosti;
						pozicijaSenzoraFlote = senzorSPozicije;
					}
				}
			}
		}

		public double SnagaSenzora(int pozicija)
		{
			if (pozicija >= 0)
				return snagaSenzora[pozicija];
			else {
				var efektUdaljenosti = Pozicije.EfektUdaljenosti.Izracunaj(pozicijaSenzoraFlote - pozicija);
				return snagaSenzoraFlote + efektUdaljenosti.SnagaSenzora;
			}
		}

		public void BitkaZavrsena(Zvijezda lokacija)
		{
			Flota flota = Igrac.floteStacionarne[lokacija];
			flota.ukloniSve();

			foreach (Borac borac in Borci)
				flota.dodajBrod(new Brod(borac.Dizajn, 1, borac.IzdrzljivostOklopa));
		}
	}
}
