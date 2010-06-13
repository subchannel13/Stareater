using System;
using System.Collections.Generic;
using System.Text;
using Alati;

namespace Prototip
{
	public class Zvijezda : IPohranjivoSB, IIdentifiable
	{
		public const int Tip_Nikakva = -1;
		public const int Tip_PocetnaPozicija = -2;
		public const int Tip_Nedodijeljen = -3;

		public class TipInfo
		{
			public double udioPojave;

			public double velicinaMin;

			public double velicinaMax;

			public int zracenje;

			public class PojavnostPlaneta
			{
				public double tezinaPojave;
				public double gomiliste;
				public double unutarnjaUcestalost;
				public double vanjskaUcestalost;
				public double odstupanje;
				public PojavnostPlaneta(double tezinaPojave, double gomiliste, double unutarnjaUcestalost, double vanjskaUcestalost, double odstupanje)
				{
					this.tezinaPojave = tezinaPojave;
					this.gomiliste = gomiliste;
					this.unutarnjaUcestalost = unutarnjaUcestalost;
					this.vanjskaUcestalost = vanjskaUcestalost;
					this.odstupanje = odstupanje;
				}
			}

			public Dictionary<Planet.Tip, PojavnostPlaneta> pojavnostPlaneta;

			private TipInfo(double velicinaMin, double velicinaMax, double udioPojave, int zracenje, Dictionary<Planet.Tip, PojavnostPlaneta> pojavnostPlaneta)
			{
				this.velicinaMax = velicinaMax;
				this.velicinaMin = velicinaMin;
				this.udioPojave = udioPojave;
				this.zracenje = zracenje;
				this.pojavnostPlaneta = pojavnostPlaneta;
			}

			private static PojavnostPlaneta postaviPlanete(string podatci)
			{
				string[] pojediniPodatci= podatci.Split(new char[]{','});

                double tezinaPojave = Double.Parse(pojediniPodatci[0], Podaci.DecimalnaTocka);
                double gomiliste = Double.Parse(pojediniPodatci[1], Podaci.DecimalnaTocka);
                double unutarnjaUcestalost = Double.Parse(pojediniPodatci[2], Podaci.DecimalnaTocka);
                double vanjskaUcestalost = Double.Parse(pojediniPodatci[3], Podaci.DecimalnaTocka);
                double odstupanje = Double.Parse(pojediniPodatci[4], Podaci.DecimalnaTocka);

				return new PojavnostPlaneta(tezinaPojave, gomiliste, unutarnjaUcestalost, vanjskaUcestalost, odstupanje);
			}

			public static void noviTip(Dictionary<string, string> podatci)
			{
				if (Tipovi == null)	Tipovi = new List<TipInfo>();
				int tip = Tipovi.Count;
				string tipStr = podatci["TIP"];

				Dictionary<Planet.Tip, PojavnostPlaneta> pojavnostPlaneta = new Dictionary<Planet.Tip, PojavnostPlaneta>();
				pojavnostPlaneta[Planet.Tip.NIKAKAV] = postaviPlanete(podatci["PLANETI_NIKAKVI"]);
				pojavnostPlaneta[Planet.Tip.ASTEROIDI] = postaviPlanete(podatci["PLANETI_ASTEROIDI"]);
				pojavnostPlaneta[Planet.Tip.KAMENI] = postaviPlanete(podatci["PLANETI_KAMENI"]);
				pojavnostPlaneta[Planet.Tip.PLINOVITI] = postaviPlanete(podatci["PLANETI_PLINOVITI"]);
                
				imeTipa.Add(podatci["TIP"], tip);
				Tipovi.Add(new TipInfo(
                        double.Parse(podatci["VELICINA_MIN"], Podaci.DecimalnaTocka),
                        double.Parse(podatci["VELICINA_MAX"], Podaci.DecimalnaTocka),
                        double.Parse(podatci["UCESTALOST"], Podaci.DecimalnaTocka),
                        int.Parse(podatci["ZRACENJE"], Podaci.DecimalnaTocka),
						pojavnostPlaneta
						));

				Slike.DodajZvjezdaMapaSliku(podatci["MAPA_SLIKA"], tip);
				Slike.DodajZvjezdaTabSliku(podatci["TAB_SLIKA"], tip);
			}
		}

		public static List<TipInfo> Tipovi = new List<TipInfo>();

		public static Dictionary<string, int> imeTipa = new Dictionary<string, int>();

		private static Random random = new Random();

		private int _tip;
		public double x;
		public double y;
		public List<Planet> planeti;
		public double velicina;
		public string ime;
		public int id { get; private set; }

		public Zvijezda(int id, int tip, double x, double y)
		{
			this.id = id;
			this._tip = tip;
			this.x = x;
			this.y = y;
			this.planeti = new List<Planet>();
			if (tip > Tip_Nikakva)
				this.velicina = Fje.IzIntervala(random.NextDouble(), Tipovi[tip].velicinaMin, Tipovi[tip].velicinaMax);					
			else
				this.velicina = random.NextDouble();
		}

		public int tip
		{
			get
			{
				return _tip;
			}
			set
			{
				double x = 0;
				if (_tip > Tip_Nikakva)
					x = (velicina - Tipovi[_tip].velicinaMin) / (Tipovi[_tip].velicinaMax - Tipovi[_tip].velicinaMin);
				else
					x = velicina;
				
				_tip = value;

				if (_tip > Tip_Nikakva) velicina = Tipovi[value].velicinaMin + x * (Tipovi[value].velicinaMax - Tipovi[value].velicinaMin);
			}
		}

		public override string ToString()
		{
			return ime;
		}

		public int zracenje()
		{
			return (int)Math.Round(Tipovi[tip].zracenje * velicina / Tipovi[tip].velicinaMax);
		}

		public void promjeniVelicinu(double v)
		{
			this.velicina = Tipovi[tip].velicinaMin +
				v * (Tipovi[tip].velicinaMax - Tipovi[tip].velicinaMin);
		}

		public double udaljenost(Zvijezda zvj)
		{
			return Math.Sqrt((this.x - zvj.x) * (this.x - zvj.x) + (this.y - zvj.y) * (this.y - zvj.y));
		}

		#region Pohrana
		public const string PohranaTip = "ZVIJEZDA";
		public const string PohTip = "TIP";
		public const string PohX = "X";
		public const string PohY = "Y";
		public const string PohVelicina = "VELICINA";
		public const string PohIme = "IME";
		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohTip, tip);
			izlaz.dodaj(PohX, x);
			izlaz.dodaj(PohY, y);
			izlaz.dodaj(PohVelicina, velicina);
			izlaz.dodaj(PohIme, ime);
			for (int i = 0; i < planeti.Count; i++)
				izlaz.dodaj(Planet.PohranaTip + i, planeti[i]);
		}
		#endregion
	}
}
