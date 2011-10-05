using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Igra;

namespace Zvjezdojedac.Podaci
{
	public class PocetnaPozicija
	{
		public static List<PocetnaPozicija> konfiguracije = new List<PocetnaPozicija>();

		public List<Planet> planeti;

		public int maticniPlanet;

		public int tipZvjezde;

		public double velicinaZvijezde;

		private PocetnaPozicija(int tipZvjezde, double velicinaZvijezde, List<Planet> planeti, int maticniPlanet)
		{
			this.tipZvjezde = tipZvjezde;
			this.velicinaZvijezde = velicinaZvijezde;
			this.planeti = planeti;
			this.maticniPlanet = maticniPlanet;
		}

		public static void novaKonfiguracija(Dictionary<string, string> podatci)
		{
			string[] splitano = podatci["TIP_ZVJEDE"].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < splitano.Length; i++)
				splitano[i] = splitano[i].Trim();
			
			string[] tipPlaneta = podatci["PLANETI_TIPOVI"].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string[] velPlaneta = podatci["PLANETI_VELICINE"].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string[] gustocaAtm = podatci["PLANETI_GUST_ATM"].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string[] kvalAtm = podatci["PLANETI_KVAL_ATM"].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string[] minerlPov = podatci["MINERALI_POVRSINA"].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string[] minerlDub = podatci["MINERALI_DUBINSKI"].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			List<Planet>  planeti = new List<Planet>();
			for (int i = 0; i < tipPlaneta.Length; i++)
			{
				Planet.Tip tip = Planet.Tip.NIKAKAV;
				tipPlaneta[i] = tipPlaneta[i].Trim();
				if (tipPlaneta[i] == "K") tip = Planet.Tip.KAMENI;
				else if (tipPlaneta[i] == "A") tip = Planet.Tip.ASTEROIDI;
				else if (tipPlaneta[i] == "P") tip = Planet.Tip.PLINOVITI;
				else if (tipPlaneta[i] == "N") tip = Planet.Tip.NIKAKAV;

                double velicina = double.Parse(velPlaneta[i], PodaciAlat.DecimalnaTocka); ;
                double gustocaAtmosfere = double.Parse(gustocaAtm[i], PodaciAlat.DecimalnaTocka);
                double kvalitetaAtmosfere = double.Parse(kvalAtm[i], PodaciAlat.DecimalnaTocka);
                double mineraliPovrsinski = double.Parse(minerlPov[i], PodaciAlat.DecimalnaTocka);
                double mineraliDubinski = double.Parse(minerlDub[i], PodaciAlat.DecimalnaTocka);
				planeti.Add(new Planet(tip, i, null, velicina, kvalitetaAtmosfere, gustocaAtmosfere, mineraliPovrsinski, mineraliDubinski));
			}

			konfiguracije.Add(
				new PocetnaPozicija(
					Zvijezda.imeTipa[splitano[0]],
                    double.Parse(splitano[1], PodaciAlat.DecimalnaTocka), 
					planeti, 
					int.Parse(podatci["MATICNI_PLANET"])
					));
		}

	}
}
