﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Alati;

namespace Prototip
{
	public class Dizajn
	{
		public class Koef
		{
			public const string Brzine = "BRZINA_KOEF";
			public const string Izdrzljivosti = "IZDRZLJIVOST_KOEF";
			public const string Ometanja = "OMETANJE_KOEF";
			public const string StitDebljina = "STIT_DEBLJINA_KOEF";
			public const string StitIzdrzljivost = "STIT_IZDRZLJIVOST_KOEF";
			public const string StitObnavljanje = "STIT_OBNAVLJANJE_KOEF";
		}
		public class Plus
		{
			public const string BrSenzora = "BR_SENZORA_PLUS";
			public const string Inercija = "INERCIJA_PLUS";
			public const string Prikrivanje = "PRIKRIVANJE_PLUS";
			public const string VelicinaReaktora = "REAKTOR_PLUS";
		}
		public enum PrivEfektTip
		{
			Koeficijent,
			Pribrojnik
		}
		public class PrivEfektInfo
		{
			public string opis { get; private set; }
			public PrivEfektTip tip { get; private set; }

			public PrivEfektInfo(string opis, PrivEfektTip tip)
			{
				this.opis = opis;
				this.tip = tip;
			}
		}

		public static Dictionary<string, PrivEfektInfo> OpisiPrivrEfekata = initOpisiPrivrEfekata();
		private static Dictionary<string, PrivEfektInfo> initOpisiPrivrEfekata()
		{
			Dictionary<string, PrivEfektInfo> ret = new Dictionary<string, PrivEfektInfo>();

			ret.Add(Koef.Brzine, new PrivEfektInfo("Pokretljivost", PrivEfektTip.Koeficijent));
			ret.Add(Koef.Izdrzljivosti, new PrivEfektInfo("Izdržljivost oklopa", PrivEfektTip.Koeficijent));
			ret.Add(Koef.Ometanja, new PrivEfektInfo("Ometanje", PrivEfektTip.Koeficijent));
			ret.Add(Koef.StitDebljina, new PrivEfektInfo("Debljina štita", PrivEfektTip.Koeficijent));
			ret.Add(Koef.StitIzdrzljivost, new PrivEfektInfo("Izdržljivost štita", PrivEfektTip.Koeficijent));
			ret.Add(Koef.StitObnavljanje, new PrivEfektInfo("Obnavljanje štita", PrivEfektTip.Koeficijent));

			ret.Add(Plus.BrSenzora, new PrivEfektInfo("Br. senzora", PrivEfektTip.Pribrojnik));
			ret.Add(Plus.Inercija, new PrivEfektInfo("Tromost", PrivEfektTip.Pribrojnik));
			ret.Add(Plus.Prikrivanje, new PrivEfektInfo("Prikrivanje", PrivEfektTip.Pribrojnik));
			ret.Add(Plus.VelicinaReaktora, new PrivEfektInfo("Veličina reaktora", PrivEfektTip.Pribrojnik));
			return ret;
		}

		class Iznos
		{
			public const string BrSenzora = "BR_SENZORA";
			public const string KoefSnage = "KOEF_SNAGE_REAKTORA";
			public const string Inercija = "INERCIJA";
			public const string Izdrzljivosti = "OKLOP";
			public const string MZBrzina = "MZ_BRZINA";
			public const string Ometanje = "OMETANJE";
			public const string OpterecenjeReaktora = "OPTERECENJE_REAKTORA";
			public const string Pokretljivosti = "BRZINA";
			public const string Prikrivanje = "PRIKRIVANJE";
			public const string SnageReaktora = "SNAGA_REAKTORA";
			public const string SnageSenzora = "SENZORI_BONUS";
			public const string StitDebljina = "STIT_DEBLJINA";
			public const string StitIzdrzljivost = "STIT_IZDRZLJIVOST";
			public const string StitObnavljanje = "STIT_OBNAVLJANJE";
		}

		public class Zbir<T>
		{
			public long kolicina { get; private set; }
			public T komponenta { get; private set; }

			public Zbir(T komponenta, long kolicina)
			{
				this.kolicina = kolicina;
				this.komponenta = komponenta;
			}
		}

		public Trup trup { get; private set; }
		public Zbir<Oruzje> primarnoOruzje { get; private set; }
		public Zbir<Oruzje> sekundarnoOruzje { get; private set; }
		public Oklop oklop { get; private set; }
		public Stit stit { get; private set; }
		public Dictionary<SpecijalnaOprema, int> specijalnaOprema { get; private set; }
		public Senzor senzor { get; private set; }
		public Potisnici potisnici { get; private set; }
		public MZPogon MZPogon { get; private set; }
		public Reaktor reaktor { get; private set; }

		public string ime { get; private set; }
		public double cijena { get; private set; }
		private Dictionary<string, double> efekti = new Dictionary<string,double>();
		public Taktika taktika { get; private set; }
		public Sazetak stil { get; private set; }
		public long brojBrodova;

		public Dizajn(string ime, Trup trup,
			Oruzje primarnoOruzje, Oruzje sekundarnoOruzje, double udioPrimarnogOruzja,
			Oklop oklop, Stit stit, Dictionary<SpecijalnaOprema, int> specijalnaOprema,
			Senzor senzor, Potisnici potisnici, MZPogon MZPogon, Reaktor reaktor, Taktika taktika)
		{
			brojBrodova = 0;
			this.ime = ime;
			this.trup = trup;
			this.oklop = oklop;
			this.specijalnaOprema = specijalnaOprema;
			this.senzor = senzor;
			this.potisnici = potisnici;
			this.MZPogon = MZPogon;
			this.stit = stit;
			this.reaktor = reaktor;
			this.taktika = taktika;

			#region Postavljanje primarnog i sekundarnog oružja/misije
			{
				if (primarnoOruzje == null) udioPrimarnogOruzja = 0;
				if (sekundarnoOruzje == null) udioPrimarnogOruzja = 1;
				/*
				 * slobodan prostor za oruzja/misije
				 */
				double preostaliProstor = trup.nosivost;
				if (stit != null) preostaliProstor -= trup.velicina_stita;
				if (MZPogon != null) preostaliProstor -= trup.velicina_MZPogona;
				foreach (SpecijalnaOprema so in specijalnaOprema.Keys)
					preostaliProstor -= so.velicina * specijalnaOprema[so];
				/*
				 * kolicina (velicina) primarnog oruzja/misije
				 */
				double zaPrimarnoOruzje = preostaliProstor;
				if (sekundarnoOruzje != null) zaPrimarnoOruzje *= udioPrimarnogOruzja;
				if (primarnoOruzje != null)
				{
					long kolicinaPrimarnog = (long)Math.Round(zaPrimarnoOruzje / primarnoOruzje.velicina);
					kolicinaPrimarnog = (long)Math.Min(kolicinaPrimarnog, Math.Floor(preostaliProstor / primarnoOruzje.velicina));
					preostaliProstor -= primarnoOruzje.velicina * kolicinaPrimarnog;

					if (kolicinaPrimarnog > 0)
						this.primarnoOruzje = new Zbir<Oruzje>(primarnoOruzje, kolicinaPrimarnog);
					else
						this.primarnoOruzje = null;
				}
				else
				{
					preostaliProstor -= zaPrimarnoOruzje;
					this.primarnoOruzje = null;
				}
				/*
				 * kolicina (velicina) sekundarnog oruzja/misije
				 */
				if (sekundarnoOruzje != null)
				{
					long kolicinaSekundarnog = (long)Math.Round(preostaliProstor / sekundarnoOruzje.velicina);
					if (kolicinaSekundarnog > 0)
						this.sekundarnoOruzje = new Zbir<Oruzje>(sekundarnoOruzje, kolicinaSekundarnog);
					else
						this.sekundarnoOruzje = null;
				}
				else
					this.sekundarnoOruzje = null;
			}
			#endregion

			#region Cijena
			{
				cijena = trup.cijena;
				if (MZPogon != null) cijena += MZPogon.cijena;
				if (this.primarnoOruzje != null) cijena += primarnoOruzje.cijena * this.primarnoOruzje.kolicina;
				if (this.sekundarnoOruzje != null) cijena += sekundarnoOruzje.cijena * this.sekundarnoOruzje.kolicina;
				if (stit != null) cijena += stit.cijena;
				foreach (SpecijalnaOprema so in specijalnaOprema.Keys)
					cijena += so.cijena * specijalnaOprema[so];
			}
			#endregion

			#region Efekti
			{
				Dictionary<string, double> privremeniEfekti = new Dictionary<string, double>();
				privremeniEfekti[Koef.Brzine] = 1;
				privremeniEfekti[Koef.Izdrzljivosti] = 1;
				privremeniEfekti[Koef.Ometanja] = (stit != null) ? stit.ometanje : 1;
				privremeniEfekti[Koef.StitDebljina] = 1;
				privremeniEfekti[Koef.StitIzdrzljivost] = 1;
				privremeniEfekti[Koef.StitObnavljanje] = 1;
				privremeniEfekti[Plus.BrSenzora] = 0;
				privremeniEfekti[Plus.Inercija] = 0;
				privremeniEfekti[Plus.Prikrivanje] = (stit != null) ? stit.prikrivanje : 0;
				privremeniEfekti[Plus.VelicinaReaktora] = 0;
				foreach (SpecijalnaOprema so in specijalnaOprema.Keys)
					foreach (string e in so.efekti.Keys)
						privremeniEfekti[e] += so.efekti[e] * specijalnaOprema[so];

				#region Reaktor
				double snagaReaktora = reaktor.snaga * (trup.velicina_reaktora + privremeniEfekti[Plus.VelicinaReaktora]) / trup.velicina_reaktora;
				{
					double opterecenjeReaktora = 0;
					if (stit != null) opterecenjeReaktora += stit.snaga;
					if (this.primarnoOruzje != null) opterecenjeReaktora += this.primarnoOruzje.kolicina * primarnoOruzje.snaga;
					if (this.sekundarnoOruzje != null) opterecenjeReaktora += this.sekundarnoOruzje.kolicina * sekundarnoOruzje.snaga;
					efekti[Iznos.OpterecenjeReaktora] = opterecenjeReaktora;
					efekti[Iznos.SnageReaktora] = snagaReaktora;
					if (snagaReaktora >= opterecenjeReaktora)
						efekti[Iznos.KoefSnage] = 1;
					else
						efekti[Iznos.KoefSnage] = snagaReaktora / opterecenjeReaktora;
				}
				#endregion

				#region Oklop
				efekti[Iznos.Izdrzljivosti] = Math.Round(trup.bazaOklopa * oklop.izdrzljivost * privremeniEfekti[Koef.Izdrzljivosti]);
				#endregion

				#region MZ pogon
				if (MZPogon != null)
					efekti[Iznos.MZBrzina] = MZPogon.brzina * Math.Min(snagaReaktora / MZPogon.snaga, 1);
				else
					efekti[Iznos.MZBrzina] = 0;
				#endregion

				#region Pokretljivost
				{
					double inercija = Math.Max(Math.Round(trup.tromost + privremeniEfekti[Plus.Inercija]), 0);
					efekti[Iznos.Inercija] = inercija;
					efekti[Iznos.Pokretljivosti] = Math.Round(potisnici.brzina * Math.Pow(2, -inercija / 10) * privremeniEfekti[Koef.Brzine]);
				}
				#endregion

				#region Senzori
				{
					double brojSenzora = Math.Round(trup.brojSenzora + privremeniEfekti[Plus.BrSenzora]);
					efekti[Iznos.BrSenzora] = brojSenzora;
					efekti[Iznos.SnageSenzora] = Math.Round(senzor.razlucivost * Senzor.BonusKolicine(brojSenzora));
				}
				#endregion

				#region Štit
				{
					if (stit != null)
					{
						efekti[Iznos.StitDebljina] = Math.Round(stit.debljina * Math.Max(privremeniEfekti[Koef.StitDebljina], 0));
						efekti[Iznos.StitIzdrzljivost] = Math.Round(trup.bazaStita * stit.izdrzljivost * Math.Max(privremeniEfekti[Koef.StitIzdrzljivost], 0));
						efekti[Iznos.StitObnavljanje] = Math.Round(trup.bazaStita * stit.obnavljanje * Math.Max(privremeniEfekti[Koef.StitObnavljanje], 0));
					}
					else
					{
						efekti[Iznos.StitDebljina] = 0;
						efekti[Iznos.StitIzdrzljivost] = 0;
						efekti[Iznos.StitObnavljanje] = 0;
					}
				}
				#endregion

				#region Prikrivanje
				{
					efekti[Iznos.Ometanje] = Math.Round(trup.kapacitetPrikrivanja * Math.Max(privremeniEfekti[Koef.Ometanja], 0));
					efekti[Iznos.Prikrivanje] = Math.Round(Math.Max(privremeniEfekti[Plus.Prikrivanje], 0));
				}
				#endregion
			}
			#endregion

			#region Stil / sažetak
			{
				List<SpecijalnaOprema> specOprema = new List<SpecijalnaOprema>(specijalnaOprema.Keys);
				specOprema.Sort(delegate(SpecijalnaOprema a, SpecijalnaOprema b) { return a.info.indeks.CompareTo(b.info.indeks); });

				List<uint> brElemenata = new List<uint>();
				brElemenata.Add(2); // MZ pogon
				brElemenata.Add(2); // primarna misija
				brElemenata.Add(2); // sekundarna misija
				brElemenata.Add(2); // štit
				brElemenata.Add(101);
				brElemenata.Add(trup.info.brojIndeksa());
				if (primarnoOruzje != null) brElemenata.Add(primarnoOruzje.info.brojIndeksa());
				if (sekundarnoOruzje != null) brElemenata.Add(sekundarnoOruzje.info.brojIndeksa());
				if (stit != null) brElemenata.Add(stit.info.brojIndeksa());
				foreach (SpecijalnaOprema so in specOprema)
				{
					brElemenata.Add(so.info.brojIndeksa());
					brElemenata.Add((uint)so.info.maxKolicinaIkako);
				}

				List<uint> sadrzaj = new List<uint>();
				if (MZPogon == null) sadrzaj.Add(0); else sadrzaj.Add(1);
				if (primarnoOruzje == null) sadrzaj.Add(0); else sadrzaj.Add(1);
				if (sekundarnoOruzje == null) sadrzaj.Add(0); else sadrzaj.Add(1);
				if (stit == null) sadrzaj.Add(0); else sadrzaj.Add(1);

				sadrzaj.Add((uint)(Math.Round(udioPrimarnogOruzja * 100)));
				sadrzaj.Add(trup.info.indeks);

				if (primarnoOruzje != null) sadrzaj.Add(primarnoOruzje.info.indeks);
				if (sekundarnoOruzje != null) sadrzaj.Add(sekundarnoOruzje.info.indeks);

				if (stit != null) sadrzaj.Add(stit.info.indeks);

				foreach (SpecijalnaOprema so in specOprema)
				{
					sadrzaj.Add(so.info.indeks);
					sadrzaj.Add((uint)specijalnaOprema[so]);
				}

				this.stil = new Sazetak(sadrzaj, brElemenata);
			}
			#endregion
		}

		public bool izgradiv(Dictionary<string, double> varijable)
		{
			if (!trup.dostupno(varijable)) return false;
			if (primarnoOruzje != null)
				if (!primarnoOruzje.komponenta.dostupno(varijable)) return false;
			if (sekundarnoOruzje != null)
				if (!sekundarnoOruzje.komponenta.dostupno(varijable)) return false;
			if (oklop.dostupno(varijable)) return false;
			if (stit.dostupno(varijable)) return false;
			if (senzor.dostupno(varijable)) return false;
			if (potisnici.dostupno(varijable)) return false;
			if (MZPogon.dostupno(varijable)) return false;
			foreach (SpecijalnaOprema so in specijalnaOprema.Keys)
				if (!so.dostupno(varijable))
					return false;
			
			return true;
		}

		#region Getteri
		public Image slika
		{
			get { return trup.info.slika; }
		}
		
		public double izdrzljivostOklopa
		{
			get { return efekti[Iznos.Izdrzljivosti]; }
		}

		public double izdrzljivostStita
		{
			get { return efekti[Iznos.StitIzdrzljivost]; }
		}

		public double obnavljanjeStita
		{
			get { return efekti[Iznos.StitObnavljanje]; }
		}

		public double debljinaStita
		{
			get { return efekti[Iznos.StitDebljina]; }
		}

		public double brSenzora
		{
			get { return efekti[Iznos.BrSenzora]; }
		}

		public double inercija
		{
			get { return efekti[Iznos.Inercija]; }
		}

		public double koefSnageReaktora
		{
			get { return efekti[Iznos.KoefSnage]; }
		}

		public double MZbrzina
		{
			get { return efekti[Iznos.MZBrzina]; }
		}

		public double ometanje
		{
			get { return efekti[Iznos.Ometanje]; }
		}

		public double opterecenjeReaktora
		{
			get { return efekti[Iznos.OpterecenjeReaktora]; }
		}

		public double pokretljivost
		{
			get { return efekti[Iznos.Pokretljivosti]; }
		}

		public double prikrivenost
		{
			get { return efekti[Iznos.Prikrivanje]; }
		}

		public double snagaReaktora
		{
			get { return efekti[Iznos.SnageReaktora]; }
		}

		public double snagaSenzora
		{
			get { return efekti[Iznos.SnageSenzora]; }
		}
		#endregion
	}
}