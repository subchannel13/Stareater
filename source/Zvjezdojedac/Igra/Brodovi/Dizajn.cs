using System;
using System.Collections.Generic;
using System.Drawing;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Podaci.Jezici;

namespace Zvjezdojedac.Igra.Brodovi
{
	public class Dizajn : IIdentifiable, IPohranjivoSB
	{
		static class Koef
		{
			public const string Izdrzljivosti = "IZDRZLJIVOST_KOEF";
			public const string InvIzdrzljivosti = "IZDRZLJIVOST_INV_KOEF";
			public const string UblazavanjeStete = "UBLAZAVANJE_KOEF";
			public const string StitDebljina = "STIT_DEBLJINA_KOEF";
			public const string StitIzdrzljivost = "STIT_IZDRZLJIVOST_KOEF";
			public const string StitObnavljanje = "STIT_OBNAVLJANJE_KOEF";
			public const string StitUblazavanjeStete = "STIT_UBLAZAVANJE_KOEF";
			public const string Nosivost = "NOSIVOST_KOEF";
		}
		static class Plus
		{
			public const string Brzina = "BRZINA_PLUS";
			public const string Pokretljivost = "POKRETLJIVOST_PLUS";
			public const string Inercija = "INERCIJA_PLUS";
			public const string Ometanje = "OMETANJE_PLUS";
			public const string Prikrivanje = "PRIKRIVANJE_PLUS";
			public const string SnagaSenzora = "SENZOR_PLUS";
			public const string VelicinaReaktora = "REAKTOR_PLUS";
		}
		public enum PrivEfektTip
		{
			Koeficijent,
			InvKoeficijent,
			Pribrojnik
		}
		public class PrivEfektInfo
		{
			private string opisKod;
			public PrivEfektTip tip { get; private set; }

			public PrivEfektInfo(string opis, PrivEfektTip tip)
			{
				this.opis = opis;
				this.tip = tip;
			}

			public string opis 
			{
				get
				{
					return Postavke.Jezik[Kontekst.Komponente, opisKod].tekst();
				}
				private set
				{
					opisKod = value;
				}
			}
		}

		public static Dictionary<string, PrivEfektInfo> OpisiPrivrEfekata = initOpisiPrivrEfekata();
		private static Dictionary<string, PrivEfektInfo> initOpisiPrivrEfekata()
		{
			Dictionary<string, PrivEfektInfo> ret = new Dictionary<string, PrivEfektInfo>();

			ret.Add(Koef.Izdrzljivosti, new PrivEfektInfo("SOE_KOEF_IZDRZ", PrivEfektTip.Koeficijent));
			ret.Add(Koef.InvIzdrzljivosti, new PrivEfektInfo("SOE_KOEF_INV_IZDRZ", PrivEfektTip.InvKoeficijent));
			ret.Add(Koef.StitDebljina, new PrivEfektInfo("SOE_KOEF_STIT_DEB", PrivEfektTip.Koeficijent));
			ret.Add(Koef.StitIzdrzljivost, new PrivEfektInfo("SOE_KOEF_STIT_IZD", PrivEfektTip.Koeficijent));
			ret.Add(Koef.StitObnavljanje, new PrivEfektInfo("SOE_KOEF_STIT_OBN", PrivEfektTip.Koeficijent));
			ret.Add(Koef.Nosivost, new PrivEfektInfo("SOE_KOEF_TERET", PrivEfektTip.Koeficijent));
			
			ret.Add(Plus.Brzina, new PrivEfektInfo("SOE_PLUS_BRZINA", PrivEfektTip.Pribrojnik));
			ret.Add(Plus.SnagaSenzora, new PrivEfektInfo("SOE_PLUS_SENZORI", PrivEfektTip.Pribrojnik));
			ret.Add(Plus.Inercija, new PrivEfektInfo("SOE_PLUS_INERC", PrivEfektTip.Pribrojnik));
			ret.Add(Plus.Ometanje, new PrivEfektInfo("SOE_PLUS_OMET", PrivEfektTip.Pribrojnik));
			ret.Add(Plus.Prikrivanje, new PrivEfektInfo("SOE_PLUS_PRIK", PrivEfektTip.Pribrojnik));
			ret.Add(Plus.VelicinaReaktora, new PrivEfektInfo("SOE_PLUS_REAKTOR", PrivEfektTip.Pribrojnik));
			return ret;
		}

		static class Iznos
		{
			public const string KoefSnage = "KOEF_SNAGE_REAKTORA";
			public const string Inercija = "INERCIJA";
			public const string Izdrzljivosti = "OKLOP";
			public const string UblazavanjaStete = "OKLOP_UBLAZAVANJE";
			public const string MZBrzina = "MZ_BRZINA";
			public const string Ometanje = "OMETANJE";
			public const string OpterecenjeReaktora = "OPTERECENJE_REAKTORA";
			public const string Nosivost = "NOSIVOST";
			public const string Brzina = "BRZINA";
			public const string Pokretljivosti = "POKRETLJIVOST";
			public const string Prikrivanje = "PRIKRIVANJE";
			public const string SnageReaktora = "SNAGA_REAKTORA";
			public const string SnageSenzora = "SENZORI_BONUS";
			public const string StitDebljina = "STIT_DEBLJINA";
			public const string StitIzdrzljivost = "STIT_IZDRZLJIVOST";
			public const string StitObnavljanje = "STIT_OBNAVLJANJE";
			public const string StitUblazavanjaStete = "STIT_UBLAZAVANJE";
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

		private static int _SlijedeciId = 0;
		private static int SlijedeciId()
		{
			return ++_SlijedeciId;
		}
		public static void ProvjeriId(Dizajn dizajn)
		{
			if (dizajn.id >= _SlijedeciId)
				_SlijedeciId = dizajn.id + 1;
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

		public int id { get; private set; }
		public string ime { get; private set; }
		public Image ikona { get; private set; }
		public double cijena { get; private set; }
		private Dictionary<string, double> efekti = new Dictionary<string,double>();
		private double udioPrimarneMisije = 0;
		public int pozeljnaPozicija { get; private set; }
		
		public Sazetak stil { get; private set; }
		public long brojBrodova;
		public Dizajn nadogradnja { get; private set; }

		public Dizajn(string ime, Trup trup,
			Oruzje primarnoOruzje, Oruzje sekundarnoOruzje, double udioPrimarnogOruzja,
			Oklop oklop, Stit stit, Dictionary<SpecijalnaOprema, int> specijalnaOprema,
			Senzor senzor, Potisnici potisnici, MZPogon MZPogon, Reaktor reaktor, int pozeljnaPozicija)
			:
			this(SlijedeciId(), ime, trup, primarnoOruzje, sekundarnoOruzje, 
			udioPrimarnogOruzja, oklop, stit, specijalnaOprema, senzor, potisnici, 
			MZPogon, reaktor, pozeljnaPozicija)
		{ }

		private Dizajn(int id, string ime, Trup trup,
			Oruzje primarnoOruzje, Oruzje sekundarnoOruzje, double udioPrimarnogOruzja,
			Oklop oklop, Stit stit, Dictionary<SpecijalnaOprema, int> specijalnaOprema,
			Senzor senzor, Potisnici potisnici, MZPogon MZPogon, Reaktor reaktor, int pozeljnaPozicija)
		{
			if (primarnoOruzje != null && sekundarnoOruzje != null)
				if (Misija.Opisnici[primarnoOruzje.misija].jednokratna &&
					Misija.Opisnici[sekundarnoOruzje.misija].jednokratna)
					sekundarnoOruzje = null;

			if (sekundarnoOruzje == null) udioPrimarnogOruzja = 1;

			this.id = id;
			brojBrodova = 0;
			nadogradnja = null;
			this.ime = ime;
			this.ikona = Slike.NaciniIkonuBroda(trup.info, primarnoOruzje, sekundarnoOruzje);
			this.trup = trup;
			this.oklop = oklop;
			this.specijalnaOprema = specijalnaOprema;
			this.senzor = senzor;
			this.potisnici = potisnici;
			this.MZPogon = MZPogon;
			this.stit = stit;
			this.reaktor = reaktor;
			this.pozeljnaPozicija = pozeljnaPozicija;
			this.udioPrimarneMisije = udioPrimarnogOruzja;

			Dictionary<string, double> privremeniEfekti = new Dictionary<string, double>();

			#region Postavljanje primarnog i sekundarnog oružja/misije
			{
				privremeniEfekti[Koef.Nosivost] = 1;
				foreach (SpecijalnaOprema so in specijalnaOprema.Keys)
					if (so.efekti.ContainsKey(Koef.Nosivost))
						privremeniEfekti[Koef.Nosivost] += so.efekti[Koef.Nosivost] * specijalnaOprema[so];
				efekti[Iznos.Nosivost] = trup.Nosivost * privremeniEfekti[Koef.Nosivost];

				/*
				 * slobodan prostor za oruzja/misije
				 */
				double preostaliProstor = efekti[Iznos.Nosivost];
				if (stit != null) preostaliProstor -= trup.VelicinaStita;
				if (MZPogon != null) preostaliProstor -= trup.VelicinaMZPogona;
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
				cijena = trup.Cijena;
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
				privremeniEfekti[Koef.Izdrzljivosti] = 1;
				privremeniEfekti[Koef.InvIzdrzljivosti] = 1;
				privremeniEfekti[Koef.UblazavanjeStete] = 1;
				privremeniEfekti[Koef.StitDebljina] = 1;
				privremeniEfekti[Koef.StitIzdrzljivost] = 1;
				privremeniEfekti[Koef.StitObnavljanje] = 1;
				privremeniEfekti[Koef.StitUblazavanjeStete] = 1;
				privremeniEfekti[Plus.Brzina] = 0;
				privremeniEfekti[Plus.Pokretljivost] = 0;
				privremeniEfekti[Plus.SnagaSenzora] = 0;
				privremeniEfekti[Plus.Inercija] = 0;
				privremeniEfekti[Plus.Ometanje] = (stit != null) ? stit.ometanje : 0;
				privremeniEfekti[Plus.Prikrivanje] = (stit != null) ? stit.prikrivanje : 0;
				privremeniEfekti[Plus.VelicinaReaktora] = 0;
				foreach (SpecijalnaOprema so in specijalnaOprema.Keys)
					foreach (string e in so.efekti.Keys)
						privremeniEfekti[e] += so.efekti[e] * specijalnaOprema[so];

				#region Reaktor
				double snagaReaktora = reaktor.snaga * (trup.VelicinaReaktora + privremeniEfekti[Plus.VelicinaReaktora]) / trup.VelicinaReaktora;
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
				efekti[Iznos.Izdrzljivosti] = Math.Round(trup.BazaOklopa * oklop.izdrzljivost * privremeniEfekti[Koef.Izdrzljivosti] / privremeniEfekti[Koef.InvIzdrzljivosti]);
				efekti[Iznos.UblazavanjaStete] = Math.Min(oklop.ublazavanjeSteteMax, oklop.ublazavanjeSteteKoef * trup.BazaOklopUblazavanja * privremeniEfekti[Koef.UblazavanjeStete]);
				#endregion

				#region MZ pogon
				if (MZPogon != null)
					efekti[Iznos.MZBrzina] = MZPogon.brzina * Math.Min(snagaReaktora / MZPogon.snaga, 1);
				else
					efekti[Iznos.MZBrzina] = 0;
				#endregion

				#region Pokretljivost
				{
					double inercija = Math.Max(Math.Round(trup.Tromost + privremeniEfekti[Plus.Inercija]), 0);
					efekti[Iznos.Inercija] = inercija;
					efekti[Iznos.Brzina] = potisnici.brzina + privremeniEfekti[Plus.Brzina];
					efekti[Iznos.Pokretljivosti] = potisnici.pokretljivost - inercija + privremeniEfekti[Plus.Pokretljivost];
				}
				#endregion

				#region Senzori
				{
					efekti[Iznos.SnageSenzora] = Math.Round(trup.SenzorPlus + senzor.razlucivost + privremeniEfekti[Plus.SnagaSenzora]);
				}
				#endregion

				#region Štit
				{
					if (stit != null)
					{
						efekti[Iznos.StitDebljina] = Math.Round(stit.debljina * Math.Max(privremeniEfekti[Koef.StitDebljina], 0));
						efekti[Iznos.StitIzdrzljivost] = Math.Round(trup.BazaStita * stit.izdrzljivost * Math.Max(privremeniEfekti[Koef.StitIzdrzljivost], 0));
						efekti[Iznos.StitObnavljanje] = Math.Round(trup.BazaStita * stit.obnavljanje * Math.Max(privremeniEfekti[Koef.StitObnavljanje], 0));
						efekti[Iznos.StitUblazavanjaStete] = stit.ublazavanjeStete * privremeniEfekti[Koef.StitUblazavanjeStete];
					}
					else
					{
						efekti[Iznos.StitDebljina] = 0;
						efekti[Iznos.StitIzdrzljivost] = 0;
						efekti[Iznos.StitObnavljanje] = 0;
						efekti[Iznos.StitUblazavanjaStete] = 0;
					}
				}
				#endregion

				#region Prikrivanje
				{
					efekti[Iznos.Ometanje] = Math.Round(trup.OmetanjeBaza + Math.Max(privremeniEfekti[Plus.Ometanje], 0));
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

		public void traziNadogradnju(Dictionary<string, double> varijable)
		{
			if (nadogradnja != null) {
				if (nadogradnja.nadogradnja != null)
					nadogradnja = nadogradnja.nadogradnja;
			}
			else {
				bool nadogradiv = false;

				Trup trup = this.trup.info.naciniKomponentu(varijable);
				Oklop oklop = this.oklop.info.naciniKomponentu(varijable);;
				Senzor senzor = this.senzor.info.naciniKomponentu(varijable);
				Potisnici potisnici = this.potisnici.info.naciniKomponentu(varijable);
				Reaktor reaktor = this.reaktor.info.naciniKomponentu(varijable, trup.VelicinaReaktora);

				nadogradiv |= trup.nivo > this.trup.nivo;
				nadogradiv |= oklop.nivo > this.oklop.nivo;
				nadogradiv |= senzor.nivo > this.senzor.nivo;
				nadogradiv |= potisnici.nivo > this.potisnici.nivo;
				nadogradiv |= reaktor.nivo > this.reaktor.nivo;

				Oruzje primarnoOruzje = null;
				if (this.primarnoOruzje != null) {
					primarnoOruzje = this.primarnoOruzje.komponenta.info.naciniKomponentu(varijable);
					nadogradiv |= primarnoOruzje.nivo > this.primarnoOruzje.komponenta.nivo;
				}
				
				Oruzje sekundarnoOruzje = null;
				if (this.sekundarnoOruzje != null) {
					sekundarnoOruzje = this.sekundarnoOruzje.komponenta.info.naciniKomponentu(varijable);
					nadogradiv |= sekundarnoOruzje.nivo > this.sekundarnoOruzje.komponenta.nivo;
				}

				Stit stit = null;
				if (this.stit != null) {
					stit = this.stit.info.naciniKomponentu(varijable, trup.VelicinaStita);
					nadogradiv |= stit.nivo > this.stit.nivo;
				}

				MZPogon MZPogon = null;
				if (this.MZPogon != null) {
					MZPogon = this.MZPogon.info.naciniKomponentu(varijable, trup.VelicinaMZPogona);
					nadogradiv |= MZPogon.nivo > this.MZPogon.nivo;
				}

				Dictionary<SpecijalnaOprema, int> specijalnaOprema = new Dictionary<SpecijalnaOprema, int>();
				foreach (KeyValuePair<SpecijalnaOprema, int> par in this.specijalnaOprema) {
					SpecijalnaOprema so = par.Key.info.naciniKomponentu(varijable, trup.velicina);
					specijalnaOprema.Add(so, par.Value);
					nadogradiv |= so.nivo > par.Key.nivo;
				}

				if (nadogradiv)
					nadogradnja = new Dizajn(ime, trup, primarnoOruzje, sekundarnoOruzje,
						udioPrimarneMisije, oklop, stit, specijalnaOprema, senzor, potisnici,
						MZPogon, reaktor, pozeljnaPozicija);
			}
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

		public double ublazavanjeSteteOklopa
		{
			get { return efekti[Iznos.UblazavanjaStete]; }
		}

		public double ublazavanjeSteteStita
		{
			get { return efekti[Iznos.StitUblazavanjaStete]; }
		}

		public double obnavljanjeStita
		{
			get { return efekti[Iznos.StitObnavljanje]; }
		}

		public double debljinaStita
		{
			get { return efekti[Iznos.StitDebljina]; }
		}

		public double inercija
		{
			get { return efekti[Iznos.Inercija]; }
		}

		public double nosivost
		{
			get { return efekti[Iznos.Nosivost]; }
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

		public double brzina
		{
			get { return efekti[Iznos.Brzina]; }
		}

		public double pokretljivost
		{
			get { return efekti[Iznos.Pokretljivosti]; }
		}

		public long populacija
		{
			get
			{
				long rez = 0;

				if (primarnoOruzje != null)
					if (primarnoOruzje.komponenta.misija == Misija.Tip.Kolonizacija)
						rez += (long)primarnoOruzje.komponenta.parametri[Misija.BrKolonista] * primarnoOruzje.kolicina;

				if (sekundarnoOruzje != null)
					if (sekundarnoOruzje.komponenta.misija == Misija.Tip.Kolonizacija)
						rez += (long)sekundarnoOruzje.komponenta.parametri[Misija.BrKolonista] * sekundarnoOruzje.kolicina;

				return rez;
			}
		}

		public double prikrivenost
		{
			get { return efekti[Iznos.Prikrivanje]; }
		}

		public Misija.Tip primarnaMisija
		{
			get { return (primarnoOruzje == null) ? Misija.Tip.N : primarnoOruzje.komponenta.misija; }
		}

		public long radnaMjesta
		{
			get
			{
				long rez = 0;

				if (primarnoOruzje != null)
					if (primarnoOruzje.komponenta.misija == Misija.Tip.Kolonizacija)
						rez += (long)primarnoOruzje.komponenta.parametri[Misija.RadnaMjesta] * primarnoOruzje.kolicina;

				if (sekundarnoOruzje != null)
					if (sekundarnoOruzje.komponenta.misija == Misija.Tip.Kolonizacija)
						rez += (long)sekundarnoOruzje.komponenta.parametri[Misija.RadnaMjesta] * sekundarnoOruzje.kolicina;

				return rez;
			}
		}

		public Misija.Tip sekundarnaMisija
		{
			get { return (sekundarnoOruzje == null) ? Misija.Tip.N : sekundarnoOruzje.komponenta.misija; }
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

		#region Pohrana
		public const string PohranaTip = "DIZAJN";
		private const string PohId = "ID";
		private const string PohIme = "IME";
		private const string PohCijena = "CIJENA";
		private const string PohPozicija = "POZICIJA";
		private const string PohTrup = "TRUP";
		private const string PohPrimOruzje = "PRIM_OR";
		private const string PohSekOruzje = "SEK_OR";
		private const string PohUdioPrimOruzja = "UDIO";
		private const string PohOklop = "OKLOP";
		private const string PohStit = "STIT";
		private const string PohSpecOp = "SPEC_OP";
		private const string PohSenzor = "SENZOR";
		private const string PohPotisnici = "POTISNICI";
		private const string PohMZPogon = "MZ_POGON";
		private const string PohReaktor = "REAKTOR";
		public void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohId, id);
			izlaz.dodaj(PohIme, ime);
			izlaz.dodaj(PohPozicija, pozeljnaPozicija);
		 
			izlaz.dodaj(PohTrup, trup.pohrani());
			if (primarnoOruzje != null)	izlaz.dodaj(PohPrimOruzje, primarnoOruzje.komponenta.pohrani() + " " + primarnoOruzje.kolicina);
			if (sekundarnoOruzje != null) izlaz.dodaj(PohSekOruzje, sekundarnoOruzje.komponenta.pohrani() + " " + sekundarnoOruzje.kolicina);
			izlaz.dodaj(PohUdioPrimOruzja, udioPrimarneMisije);
			if (stit != null) izlaz.dodaj(PohStit, stit.pohrani());
			if (MZPogon != null) izlaz.dodaj(PohMZPogon, MZPogon.pohrani());
			izlaz.dodaj(PohOklop, oklop.pohrani());
			izlaz.dodaj(PohSenzor, senzor.pohrani());
			izlaz.dodaj(PohPotisnici, potisnici.pohrani());
			izlaz.dodaj(PohReaktor, reaktor.pohrani());
			int i = 0;
			izlaz.dodaj(PohSpecOp, specijalnaOprema.Count);
			foreach (SpecijalnaOprema so in specijalnaOprema.Keys) {
				izlaz.dodaj(PohSpecOp + i, so.pohrani() + " " + specijalnaOprema[so]);
				i++;
			}
		}

		private struct UcitanaKomp
		{
			public int idInfa;
			public int nivo;
			public int kolicina;

			public UcitanaKomp(int idInfa, int nivo, int kolicina)
			{
				this.idInfa = idInfa;
				this.nivo = nivo;
				this.kolicina = kolicina;
			}
		}

		private static UcitanaKomp ucitajKomponentu(string str)
		{
			string[] parametri = str.Split(new char[] { ' ' });
			
			int kolicina = 1;
			if (parametri.Length > 2) kolicina = int.Parse(parametri[2]);

			return new UcitanaKomp(
				int.Parse(parametri[0]),
				int.Parse(parametri[1]),
				kolicina);
		}

		public static Dizajn Ucitaj(PodaciCitac ulaz)
		{
			int id = ulaz.podatakInt(PohId);
			string ime = ulaz.podatak(PohIme);
			int pozeljnaPozicija = ulaz.podatakInt(PohPozicija);

			UcitanaKomp komp = ucitajKomponentu(ulaz.podatak(PohTrup));
			Trup trup = Trup.TrupInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo);

			Zbir<Oruzje> primOruzje;
			if (ulaz.ima(PohPrimOruzje)) {
				komp = ucitajKomponentu(ulaz.podatak(PohPrimOruzje));
				primOruzje = new Zbir<Oruzje>(
					Oruzje.OruzjeInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo),
					komp.kolicina);
			}
			else
				primOruzje = new Zbir<Oruzje>(null, 0);

			Zbir<Oruzje> sekOruzje;
			if (ulaz.ima(PohSekOruzje)) {
				komp = ucitajKomponentu(ulaz.podatak(PohSekOruzje));
				sekOruzje = new Zbir<Oruzje>(
					Oruzje.OruzjeInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo),
					komp.kolicina);
			}
			else
				sekOruzje = new Zbir<Oruzje>(null, 0);

			double udio = ulaz.podatakDouble(PohUdioPrimOruzja);

			Stit stit = null;
			if (ulaz.ima(PohStit)) {
				komp = ucitajKomponentu(ulaz.podatak(PohStit));
				stit = Stit.StitInfo.
					IzIda(komp.idInfa).
					naciniKomponentu(komp.nivo, trup.VelicinaStita);
			}

			MZPogon mzPogon = null;
			if (ulaz.ima(PohMZPogon)) {
				komp = ucitajKomponentu(ulaz.podatak(PohMZPogon));
				mzPogon = MZPogon.MZPogonInfo.
					IzIda(komp.idInfa).
					naciniKomponentu(komp.nivo, trup.VelicinaMZPogona);
			}

			komp = ucitajKomponentu(ulaz.podatak(PohOklop));
			Oklop oklop = Oklop.OklopInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo);

			komp = ucitajKomponentu(ulaz.podatak(PohSenzor));
			Senzor senzor = Senzor.SenzorInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo);

			komp = ucitajKomponentu(ulaz.podatak(PohPotisnici));
			Potisnici potisnici = Potisnici.PotisnikInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo);

			komp = ucitajKomponentu(ulaz.podatak(PohReaktor));
			Reaktor reaktor = Reaktor.ReaktorInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo, trup.VelicinaReaktora);

			int brSpecOp = ulaz.podatakInt(PohSpecOp);
			Dictionary<SpecijalnaOprema, int> specOprema = new Dictionary<SpecijalnaOprema, int>();
			for (int i = 0; i < brSpecOp; i++) {
				komp = ucitajKomponentu(ulaz.podatak(PohSpecOp + i));
				SpecijalnaOprema so = SpecijalnaOprema.SpecijalnaOpremaInfo.
					IzIda(komp.idInfa).
					naciniKomponentu(komp.nivo, trup.velicina);

				specOprema.Add(so, komp.kolicina);
			}
			
			return new Dizajn(id, ime, trup, primOruzje.komponenta, sekOruzje.komponenta,
				udio, oklop, stit, specOprema, senzor, potisnici, mzPogon, reaktor,
				pozeljnaPozicija);
		}
		#endregion
	}
}

