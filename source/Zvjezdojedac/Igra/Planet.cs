using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Alati;

namespace Prototip
{
	public class Planet
	{
		public enum Tip
		{
			NIKAKAV = 0,
			ASTEROIDI,
			KAMENI,
			PLINOVITI
		};

		public class TipInfo
		{
			public int velicinaMin;
			public int velicinaMax;
			
			public double povrsinskiMineraliMin;
			public double povrsinskiMineraliMax;
			public double dubinskiMineraliMin;
			public double dubinskiMineraliMax;

			public double slikaAtmGustKoef;
			public double slikaAtmKvalKoef;
			public double slikaAtmTempKoef;

			private TipInfo(int velicinaMin, int velicinaMax, 
				double povrsinskiMineraliMin, double povrsinskiMineraliMax, double dubinskiMineraliMin, double dubinskiMineraliMax,
				double slikaAtmGustKoef, double slikaAtmKvalKoef, double slikaAtmTempKoef)
			{
				this.velicinaMax = velicinaMax;
				this.velicinaMin = velicinaMin;
				this.povrsinskiMineraliMin = povrsinskiMineraliMin;
				this.povrsinskiMineraliMax = povrsinskiMineraliMax;
				this.dubinskiMineraliMin = dubinskiMineraliMin;
				this.dubinskiMineraliMax = dubinskiMineraliMax;
				this.slikaAtmGustKoef = slikaAtmGustKoef;
				this.slikaAtmKvalKoef = slikaAtmKvalKoef;
				this.slikaAtmTempKoef = slikaAtmTempKoef;
			}

			public static void noviTip(Dictionary<string, string> podatci)
			{
				Tip tip = Tip.NIKAKAV;
				if (podatci["TIP"] == "KAMENI") tip = Tip.KAMENI;
				else if (podatci["TIP"] == "ASTEROIDI") tip = Tip.ASTEROIDI;
				else if (podatci["TIP"] == "PLINOVITI") tip = Tip.PLINOVITI;
				else if (podatci["TIP"] == "NIKAKAV") tip = Tip.NIKAKAV;

				int velicinaMax = Int32.Parse(podatci["VELICINA_MAX"]);
				int velicinaMin = Int32.Parse(podatci["VELICINA_MIN"]);
                double povrsinskiMineraliMin = Double.Parse(podatci["POVRSINSKI_MINERALI_MIN"], Podaci.DecimalnaTocka);
                double povrsinskiMineraliMax = Double.Parse(podatci["POVRSINSKI_MINERALI_MAX"], Podaci.DecimalnaTocka);
                double dubinskiMineraliMin = Double.Parse(podatci["DUBINSKI_MINERALI_MIN"], Podaci.DecimalnaTocka);
                double dubinskiMineraliMax = Double.Parse(podatci["DUBINSKI_MINERALI_MAX"], Podaci.DecimalnaTocka);
				
				double slikaAtmGustKoef = double.Parse(podatci["SLIKA_KOEF_ATM_GUST"]);
				double slikaAtmKvalKoef = double.Parse(podatci["SLIKA_KOEF_ATM_KVAL"]);
				double slikaAtmTempKoef = double.Parse(podatci["SLIKA_KOEF_ATM_TEMP"]);

				tipovi.Add(tip, new TipInfo(velicinaMin, velicinaMax, 
					povrsinskiMineraliMin, povrsinskiMineraliMax, dubinskiMineraliMin, dubinskiMineraliMax,
					slikaAtmGustKoef, slikaAtmKvalKoef, slikaAtmTempKoef));
			}
		}

		public static Dictionary<Tip, TipInfo> tipovi = new Dictionary<Tip, TipInfo>();
		private static string[] rimskiBrojevi = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV" };

		public Tip tip;
		public string ime;

		public int velicina; // 100 = 1 Zemlja
		public double kvalitetaAtmosfere; // [0, 1]
		public double gustocaAtmosfere;	//10 = ko Zemlja
		public double mineraliPovrsinski;
		public double mineraliDubinski;

		public Zvijezda zvjezda;
		private int pozicija;
		public Kolonija kolonija;

		public Image slika;

		public Planet(Tip tip, int pozicija, Zvijezda zvjezda, double velicina, double kvalitetaAtmosfere, double gustocaAtmosfere, double mineraliPovrsinski, double mineraliDubinski)
		{
			this.tip = tip;
			this.zvjezda = zvjezda;
			this.pozicija = pozicija;
			if (zvjezda != null)
				ime = zvjezda.ime + " " + rimskiBrojevi[pozicija];
			else
				ime = "";
			this.kolonija = null;

			this.velicina = (int)(Fje.IzIntervala(velicina, tipovi[tip].velicinaMin, tipovi[tip].velicinaMax));
			this.kvalitetaAtmosfere = kvalitetaAtmosfere;
			this.gustocaAtmosfere = Fje.IzIntervala(gustocaAtmosfere, minGustocaAtmosfere(), maxGustocaAtmosfere());
			this.mineraliPovrsinski = Fje.IzIntervala(mineraliPovrsinski, tipovi[tip].povrsinskiMineraliMin, tipovi[tip].povrsinskiMineraliMax);
			this.mineraliDubinski = Fje.IzIntervala(mineraliDubinski, this.mineraliPovrsinski, tipovi[tip].dubinskiMineraliMax);

			if (zvjezda != null)
				slika = Slike.OdrediSlikuPlaneta(tip, this.gustocaAtmosfere, this.kvalitetaAtmosfere, this.temperatura());
			else
				slika = null;
		}

		public Planet(Planet predlozak, Zvijezda zvjezda, Kolonija kolonija)
		{
			this.tip = predlozak.tip;
			this.zvjezda = zvjezda;
			this.pozicija = predlozak.pozicija;
			this.ime = zvjezda.ime + " " + rimskiBrojevi[pozicija];
			this.kolonija = kolonija;
			this.slika = predlozak.slika;

			this.velicina = predlozak.velicina;
			this.kvalitetaAtmosfere = predlozak.kvalitetaAtmosfere;
			this.gustocaAtmosfere = predlozak.gustocaAtmosfere;
			this.mineraliDubinski = predlozak.mineraliDubinski;
			this.mineraliPovrsinski = predlozak.mineraliPovrsinski;

			slika = Slike.OdrediSlikuPlaneta(tip, this.gustocaAtmosfere, this.kvalitetaAtmosfere, this.temperatura());
		}

		public double gravitacija()
		{
			if (tip == Tip.KAMENI)
				return velicina / 100.0;
			else
				return 0;
		}

		public double ozracenost()
		{
			double ret = zvjezda.zracenje() - pozicija - gustocaAtmosfere / 5;
			if (ret < 0)
				return 0;
			else
				return ret;
		}

		public double temperatura()
		{
			return ozracenost() + gustocaAtmosfere*2 / 5;
		}

		const double MIN_ZA_GRAVITACIJU = 0.5;
		const double MAX_FAKTOR_ZA_GRAVITACIJU = 2;

		public double minGustocaAtmosfere()
		{
			if (gravitacija() <= MIN_ZA_GRAVITACIJU) return 0;

			return gravitacija() - MIN_ZA_GRAVITACIJU;
		}

		public double maxGustocaAtmosfere()
		{
			if (gravitacija() <= MIN_ZA_GRAVITACIJU) return 0;

			return (gravitacija() - MIN_ZA_GRAVITACIJU) * MAX_FAKTOR_ZA_GRAVITACIJU;
		}
	}
}
