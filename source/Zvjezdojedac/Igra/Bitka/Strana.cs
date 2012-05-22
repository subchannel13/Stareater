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
		public MySet<Borac> Borci;
		public HashSet<Borac> PaliBorci = new HashSet<Borac>();
		
		double[] snagaSenzora = new double[Pozicije.MaxPozicija + 1];
		double snagaSenzoraFlote;
		int pozicijaSenzoraFlote;

		public Strana(MySet<Borac> borci)
		{
			this.Borci = borci;
		}

		public void PostaviSnaguSenzora(double[] snagaSenzora)
		{
			snagaSenzoraFlote = double.NegativeInfinity;

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

				snagaSenzora[pozicija] = snagaNaPoziciji;
				if (!double.IsNaN(snagaNaPoziciji)) {
					double efektUdaljenosti = Pozicije.EfektUdaljenosti.Izracunaj(pozicija).SnagaSenzora;
					if (snagaSenzora[senzorSPozicije] + efektUdaljenosti > snagaSenzoraFlote) {
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
	}
}
