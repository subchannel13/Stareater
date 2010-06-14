using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototip
{
	public class Mapa : IPohranjivoSB
	{
		public class GraditeljMape
		{
			public const double UDALJENOST = 3;	//Udaljenost izmedju zvijezda

			public Mapa mapa;
			public List<Planet> pocetnePozicije;

			public GraditeljMape(int tipMape, int brIgraca)
			{
				mapa = new Mapa();
				pocetnePozicije = new List<Planet>();

				Random rnd = new Random();
				double x, y;
				int velicinaMape = Mapa.velicinaMape[tipMape].velicina;
				const int BR_PLANETA = 15;
				HashSet<Zvijezda> pozicijeIgraca = new HashSet<Zvijezda>();

				#region Stvaranje zvijezda
				{
					const double OSTUPANJE_ZVJ = 0.35;

					for (y = 0; y < velicinaMape; y++)
						for (x = 0; x < velicinaMape; x++)
							mapa.zvijezde.Add(new Zvijezda(
								mapa.zvijezde.Count,
								Zvijezda.Tip_Nedodijeljen,
								(x + OSTUPANJE_ZVJ * 2 * rnd.NextDouble()) * UDALJENOST,
								(y + OSTUPANJE_ZVJ * 2 * rnd.NextDouble()) * UDALJENOST
								));
				}
				#endregion

				#region Pozicije igrača
				{
					List<Zvijezda> igraceveZvijezde = new List<Zvijezda>();
					const double UDALJENOST_OD_SREDISTA = 0.85;
					double pomakFi = rnd.Next(2) * Math.PI + 0.25 * Math.PI;
					double R = UDALJENOST * (velicinaMape - 1) / 2.0;

					for (double igrac = 0; igrac < brIgraca; igrac++)
					{
						x = R + UDALJENOST_OD_SREDISTA * R * Math.Cos(pomakFi + (igrac * 2 * Math.PI) / brIgraca);
						y = R + UDALJENOST_OD_SREDISTA * R * Math.Sin(pomakFi + (igrac * 2 * Math.PI) / brIgraca);
						double minR = double.MaxValue;
						Zvijezda zvjNajbolja = null; ;

						foreach (Zvijezda zvj in mapa.zvijezde)
						{
							double r = Math.Sqrt((x - zvj.x) * (x - zvj.x) + (y - zvj.y) * (y - zvj.y));
							if (r < minR)
							{
								minR = r;
								zvjNajbolja = zvj;
							}
						}

						zvjNajbolja.tip = Zvijezda.Tip_PocetnaPozicija;
						pozicijeIgraca.Add(zvjNajbolja);
						igraceveZvijezde.Add(zvjNajbolja);
					}
				}
				#endregion

				#region Imenovanje zvijezda
				{
					List<int> zvjezdja = new List<int>();
					Dictionary<int, List<Zvijezda>> zvjezdeUZvjezdju = new Dictionary<int, List<Zvijezda>>();
					int zvje;
					int i;

					Alati.Vadjenje<int> vz = new Alati.Vadjenje<int>();
					Alati.Vadjenje<int> dodZ = new Alati.Vadjenje<int>();
					for (i = 0; i < Podaci.zvijezdja.Count; i++)
						vz.dodaj(i);

					for (i = 0; i < Mapa.velicinaMape[tipMape].brZvijezdja; i++)
					{
						zvje = vz.izvadi();
						for (int j = 0; j < 8; j++)
							zvjezdja.Add(zvje);
					}

					vz = new Alati.Vadjenje<int>(zvjezdja);
					foreach (Zvijezda zvj in mapa.zvijezde)
					{
						zvje = vz.izvadi();
						if (!zvjezdeUZvjezdju.ContainsKey(zvje))
							zvjezdeUZvjezdju.Add(zvje, new List<Zvijezda>());

						zvjezdeUZvjezdju[zvje].Add(zvj);
					}

					foreach (int izvj in zvjezdeUZvjezdju.Keys)
					{
						if (zvjezdeUZvjezdju[izvj].Count == 1)
							zvjezdeUZvjezdju[izvj][0].ime = Podaci.zvijezdja[izvj].nominativ;
						else
						{
							Alati.Vadjenje<Zvijezda> vZvj = new Alati.Vadjenje<Zvijezda>(zvjezdeUZvjezdju[izvj]);
							for (int j = 0; j < zvjezdeUZvjezdju[izvj].Count; j++)
								vZvj.izvadi().ime =
									Podaci.prefiksZvijezdja[(Podaci.PrefiksZvijezdja)j] +
									" " +
									Podaci.zvijezdja[izvj].genitiv;
						}
					}
				}
				#endregion

				#region Dodijela tipa zvijezdama i stvaranje planeta
				{
					Alati.Vadjenje<int> tipovi = new Alati.Vadjenje<int>();
					int brZvj = mapa.zvijezde.Count - brIgraca;
					double kontrolniBroj = 0;

					//Određivanje broja zvijezda pojedinog tipa
					for (int tip = 0; tip < Zvijezda.Tipovi.Count; tip++)
					{
						for (int i = 0; i < Zvijezda.Tipovi[tip].udioPojave * brZvj; i++)
							tipovi.dodaj(tip);

						kontrolniBroj += Zvijezda.Tipovi[tip].udioPojave * brZvj;
						if (kontrolniBroj - tipovi.kolicina() >= 0.5)
							tipovi.dodaj(tip);
					}
					for (int i = 0; brZvj > tipovi.kolicina(); i++)
						tipovi.dodaj(Zvijezda.Tip_Nikakva);

					//Pridjeljivanje tipova
					Dictionary<int, List<Zvijezda>> zvijezdePoTipu = new Dictionary<int, List<Zvijezda>>();
					foreach (Zvijezda zvj in mapa.zvijezde)
						if (zvj.tip != Zvijezda.Tip_PocetnaPozicija)
						{
							zvj.tip = tipovi.izvadi();
							if (!zvijezdePoTipu.ContainsKey(zvj.tip)) zvijezdePoTipu[zvj.tip] = new List<Zvijezda>();
							zvijezdePoTipu[zvj.tip].Add(zvj);
						}

					//Određivanje raspodijeljivih planeta
					Dictionary<int, Alati.Vadjenje<Planet.Tip>[]> tipoviPlaneta = new Dictionary<int, Alati.Vadjenje<Planet.Tip>[]>();
					Planet.Tip[] enumTipova = new Planet.Tip[] { Planet.Tip.NIKAKAV, Planet.Tip.ASTEROIDI, Planet.Tip.KAMENI, Planet.Tip.PLINOVITI };
					Dictionary<Planet.Tip, int> brPlanetaPoTipu = new Dictionary<Planet.Tip, int>();
					foreach(Planet.Tip tip in enumTipova)
						brPlanetaPoTipu.Add(tip, 0);
					foreach (int tipZvj in zvijezdePoTipu.Keys)
					{
						if (tipZvj == Zvijezda.Tip_Nikakva)
							continue;

						Dictionary<Planet.Tip, double[]> tipoviPoPozicijama = new Dictionary<Planet.Tip, double[]>();
						foreach (Planet.Tip tipPlaneta in enumTipova)
						{
							tipoviPoPozicijama[tipPlaneta] = new double[BR_PLANETA];
							Zvijezda.TipInfo.PojavnostPlaneta parametri = Zvijezda.Tipovi[tipZvj].pojavnostPlaneta[tipPlaneta];
							
							double suma = 0;
							for (int i = 0; i < BR_PLANETA; i++)
							{
								tipoviPoPozicijama[tipPlaneta][i] = (1 - parametri.odstupanje) + rnd.NextDouble() * parametri.odstupanje;
								if (i <= parametri.gomiliste)
									tipoviPoPozicijama[tipPlaneta][i] *= parametri.unutarnjaUcestalost + (1 - parametri.unutarnjaUcestalost) * i / (BR_PLANETA * parametri.gomiliste);
								else
									tipoviPoPozicijama[tipPlaneta][i] *= 1 + (parametri.vanjskaUcestalost - 1) * (i - BR_PLANETA * parametri.gomiliste) / (BR_PLANETA * (1 - parametri.gomiliste));
								suma += tipoviPoPozicijama[tipPlaneta][i];
							}

							if (suma > 0)
								for (int i = 0; i < BR_PLANETA; i++)
									tipoviPoPozicijama[tipPlaneta][i] *= parametri.tezinaPojave / suma;
						}

						tipoviPlaneta[tipZvj] = new Alati.Vadjenje<Planet.Tip>[BR_PLANETA];
						for (int i = 0; i < BR_PLANETA; i++)
						{
							tipoviPlaneta[tipZvj][i] = new Alati.Vadjenje<Planet.Tip>();
							double suma = 0;
							foreach (Planet.Tip tipPlaneta in enumTipova)
								suma += tipoviPoPozicijama[tipPlaneta][i];

							foreach (Planet.Tip tipPlaneta in enumTipova)
							{
								double n = tipoviPoPozicijama[tipPlaneta][i] * zvijezdePoTipu[tipZvj].Count / suma;
								brPlanetaPoTipu[tipPlaneta] += (int)n;
								for (int j = 1; j <= n; j++)
									tipoviPlaneta[tipZvj][i].dodaj(tipPlaneta);
							}

							while (tipoviPlaneta[tipZvj][i].kolicina() < zvijezdePoTipu[tipZvj].Count)
								tipoviPlaneta[tipZvj][i].dodaj(Planet.Tip.NIKAKAV);
						}
					}
					Dictionary<Planet.Tip, Alati.Vadjenje<double>> randVelicina = new Dictionary<Planet.Tip, Alati.Vadjenje<double>>();
					Dictionary<Planet.Tip, Alati.Vadjenje<double>> randAtmKval = new Dictionary<Planet.Tip, Alati.Vadjenje<double>>();
					Dictionary<Planet.Tip, Alati.Vadjenje<double>> randAtmGust = new Dictionary<Planet.Tip, Alati.Vadjenje<double>>();
					Dictionary<Planet.Tip, Alati.Vadjenje<double>> randMinerPov = new Dictionary<Planet.Tip, Alati.Vadjenje<double>>();
					Dictionary<Planet.Tip, Alati.Vadjenje<double>> randMinerDub = new Dictionary<Planet.Tip, Alati.Vadjenje<double>>();
					foreach (Planet.Tip tipPl in enumTipova) {
						randAtmGust.Add(tipPl, new Alati.Vadjenje<double>());
						randAtmKval.Add(tipPl, new Alati.Vadjenje<double>());
						randMinerDub.Add(tipPl, new Alati.Vadjenje<double>());
						randMinerPov.Add(tipPl, new Alati.Vadjenje<double>());
						randVelicina.Add(tipPl, new Alati.Vadjenje<double>());
						for (double i = 0; i < brPlanetaPoTipu[tipPl]; i++) {
							randAtmGust[tipPl].dodaj(i / brPlanetaPoTipu[tipPl]);
							randAtmKval[tipPl].dodaj(i / brPlanetaPoTipu[tipPl]);
							randMinerDub[tipPl].dodaj(i / brPlanetaPoTipu[tipPl]);
							randMinerPov[tipPl].dodaj(i / brPlanetaPoTipu[tipPl]);
							randVelicina[tipPl].dodaj(i / brPlanetaPoTipu[tipPl]);
						}
					}

					//Dodavanje planeta
					foreach (Zvijezda zvj in mapa.zvijezde)
					{
						//if (zvj.tip == Zvijezda.Tip_Nikakva || mapa.pozicijeIgraca.Contains(zvj))
						if (zvj.tip == Zvijezda.Tip_Nikakva || zvj.tip == Zvijezda.Tip_PocetnaPozicija)
							continue;

						for (int i = 0; i < BR_PLANETA; i++)
						{
							Planet.Tip tip = tipoviPlaneta[zvj.tip][i].izvadi();
							double velicina = randVelicina[tip].izvadi();
							double AtmGust = randAtmGust[tip].izvadi();
							double AtmKval = randAtmKval[tip].izvadi();
							double MinerPov = randMinerPov[tip].izvadi();
							double MinerDub = randMinerDub[tip].izvadi();
							zvj.planeti.Add(
								new Planet(tip, i, zvj, velicina, 
									AtmKval, AtmGust, 
									MinerPov, MinerDub));
						}
					}
				}
				#endregion

				int br = 0;
				foreach (Zvijezda zvj in pozicijeIgraca)
				{
					br++;
					zvj.ime = "Doma " + br;
					PocetnaPozicija konf = PocetnaPozicija.konfiguracije[rnd.Next(PocetnaPozicija.konfiguracije.Count)];
					zvj.tip = konf.tipZvjezde;
					zvj.promjeniVelicinu(konf.velicinaZvijezde);
					for (int i = 0; i < BR_PLANETA; i++)
					{
						Planet planet = new Planet(konf.planeti[i], zvj, null);
						if (konf.maticniPlanet == i)
							pocetnePozicije.Add(planet);
						zvj.planeti.Add(planet);
					}
				}
			}
		}

		public List<Zvijezda> zvijezde
		{
			get;
			private set;
		}

		public static List<VelicinaMape> velicinaMape
		{
			get;
			private set;
		}

		public static void dodajVelicinuMape(Dictionary<string, string> podatci)
		{
			if (velicinaMape == null)
				velicinaMape = new List<VelicinaMape>();
			int velicina = int.Parse(podatci["VELICINA"]);
			int brZvijezda = int.Parse(podatci["BR_ZVIJEZDJA"]);
			velicinaMape.Add(new VelicinaMape(velicina, podatci["NAZIV"], brZvijezda));
		}

		public class VelicinaMape
		{
			public int velicina;

			public string naziv;

			public int brZvijezdja;

			public VelicinaMape(int velicina, string naziv, int brZvijezdja)
			{
				this.naziv = naziv;
				this.velicina = velicina;
				this.brZvijezdja = brZvijezdja;
			}
		}

		public Mapa()
		{
			this.zvijezde = new List<Zvijezda>();
		}

		public Zvijezda najblizaZvijezda(double x, double y, double r)
		{
			double minR = double.MaxValue;
			Zvijezda ret = null;

			foreach (Zvijezda zvj in this.zvijezde)
			{
				if (zvj.tip == Zvijezda.Tip_Nikakva) continue;
				double tr = Math.Sqrt((zvj.x - x) * (zvj.x - x) + (zvj.y - y) * (zvj.y - y));
				if (tr < minR)
				{
					minR = tr;
					ret = zvj;
				}
			}

			if (r >= 0 && minR > r)
				return null;

			return ret;
		}

		public HashSet<Kolonija> kolonije()
		{
			HashSet<Kolonija> rez = new HashSet<Kolonija>();
			
			foreach (Zvijezda zvj in zvijezde)
				foreach (Planet pl in zvj.planeti)
					if (pl.kolonija != null)
						rez.Add(pl.kolonija);
			return rez;
		}

		#region Pohrana
		public const string PohranaTip = "MAPA";
		public const string PohBrZijezda = "BR_ZVIJEZDA";
		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohBrZijezda, zvijezde.Count);
			for (int i = 0; i < zvijezde.Count; i++)
				izlaz.dodaj(Zvijezda.PohranaTip + i, (IPohranjivoSB)zvijezde[i]);
		}
		#endregion

	}
}
