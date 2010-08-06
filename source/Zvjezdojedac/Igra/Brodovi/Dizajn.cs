using System;
using System.Collections.Generic;
using System.Drawing;
using Alati;

namespace Prototip
{
	public class Dizajn : IIdentifiable, IPohranjivoSB
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
		public Taktika taktika { get; private set; }
		
		public Sazetak stil { get; private set; }
		public long brojBrodova;
		public Dizajn nadogradnja { get; private set; }

		public Dizajn(string ime, Trup trup,
			Oruzje primarnoOruzje, Oruzje sekundarnoOruzje, double udioPrimarnogOruzja,
			Oklop oklop, Stit stit, Dictionary<SpecijalnaOprema, int> specijalnaOprema,
			Senzor senzor, Potisnici potisnici, MZPogon MZPogon, Reaktor reaktor, Taktika taktika)
			:
			this(SlijedeciId(), ime, trup, primarnoOruzje, sekundarnoOruzje, 
			udioPrimarnogOruzja, oklop, stit, specijalnaOprema, senzor, potisnici, 
			MZPogon, reaktor, taktika)
		{ }

		private Dizajn(int id, string ime, Trup trup,
			Oruzje primarnoOruzje, Oruzje sekundarnoOruzje, double udioPrimarnogOruzja,
			Oklop oklop, Stit stit, Dictionary<SpecijalnaOprema, int> specijalnaOprema,
			Senzor senzor, Potisnici potisnici, MZPogon MZPogon, Reaktor reaktor, Taktika taktika)
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
			this.taktika = taktika;
			this.udioPrimarneMisije = udioPrimarnogOruzja;

			#region Postavljanje primarnog i sekundarnog oružja/misije
			{
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
				Reaktor reaktor = this.reaktor.info.naciniKomponentu(varijable, trup.velicina_reaktora);

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
					stit = this.stit.info.naciniKomponentu(varijable, trup.velicina_stita);
					nadogradiv |= stit.nivo > this.stit.nivo;
				}

				MZPogon MZPogon = null;
				if (this.MZPogon != null) {
					MZPogon = this.MZPogon.info.naciniKomponentu(varijable, trup.velicina_MZPogona);
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
						MZPogon, reaktor, taktika);
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
		private const string PohTaktika = "TAKTIKA";
		private const string PohTrup = "TRUP";
		private const string PohPrimOruzje = "PRIM_OR";
		private const string PohSekOruzje = "SEK_OR";
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
			izlaz.dodaj(PohTaktika, taktika);
		 
			izlaz.dodaj(PohTrup, trup.pohrani());
			if (primarnoOruzje != null)	izlaz.dodaj(PohPrimOruzje, primarnoOruzje.komponenta.pohrani() + " " + primarnoOruzje.kolicina);
			if (sekundarnoOruzje != null) izlaz.dodaj(PohSekOruzje, sekundarnoOruzje.komponenta.pohrani() + " " + sekundarnoOruzje.kolicina);
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
			Taktika taktika = Taktika.IzIda(ulaz.podatakInt(PohTaktika));

			UcitanaKomp komp = ucitajKomponentu(ulaz.podatak(PohTrup));
			Trup trup = Trup.TrupInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo);

			Zbir<Oruzje> primOruzje = null;
			if (ulaz.ima(PohPrimOruzje)) {
				komp = ucitajKomponentu(ulaz.podatak(PohPrimOruzje));
				primOruzje = new Zbir<Oruzje>(
					Oruzje.OruzjeInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo),
					komp.kolicina);
			}

			Zbir<Oruzje> sekOruzje = null;
			if (ulaz.ima(PohSekOruzje)) {
				komp = ucitajKomponentu(ulaz.podatak(PohSekOruzje));
				sekOruzje = new Zbir<Oruzje>(
					Oruzje.OruzjeInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo),
					komp.kolicina);
			}

			Stit stit = null;
			if (ulaz.ima(PohStit)) {
				komp = ucitajKomponentu(ulaz.podatak(PohStit));
				stit = Stit.StitInfo.
					IzIda(komp.idInfa).
					naciniKomponentu(komp.nivo, trup.velicina_stita);
			}

			MZPogon mzPogon = null;
			if (ulaz.ima(PohMZPogon)) {
				komp = ucitajKomponentu(ulaz.podatak(PohMZPogon));
				mzPogon = MZPogon.MZPogonInfo.
					IzIda(komp.idInfa).
					naciniKomponentu(komp.nivo, trup.velicina_MZPogona);
			}

			komp = ucitajKomponentu(ulaz.podatak(PohOklop));
			Oklop oklop = Oklop.OklopInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo);

			komp = ucitajKomponentu(ulaz.podatak(PohSenzor));
			Senzor senzor = Senzor.SenzorInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo);

			komp = ucitajKomponentu(ulaz.podatak(PohPotisnici));
			Potisnici potisnici = Potisnici.PotisnikInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo);

			komp = ucitajKomponentu(ulaz.podatak(PohReaktor));
			Reaktor reaktor = Reaktor.ReaktorInfo.IzIda(komp.idInfa).naciniKomponentu(komp.nivo, trup.velicina_reaktora);

			int brSpecOp = ulaz.podatakInt(PohSpecOp);
			Dictionary<SpecijalnaOprema, int> specOprema = new Dictionary<SpecijalnaOprema, int>();
			for (int i = 0; i < brSpecOp; i++) {
				komp = ucitajKomponentu(ulaz.podatak(PohSpecOp + i));
				SpecijalnaOprema so = SpecijalnaOprema.SpecijalnaOpremaInfo.
					IzIda(komp.idInfa).
					naciniKomponentu(komp.nivo, trup.velicina);

				specOprema.Add(so, komp.kolicina);
			}

			double udio = 0;
			{
				double udioPrim = 0, udioSek = 0, ukupno = 0;
				if (primOruzje != null) {
					ukupno += primOruzje.komponenta.velicina * primOruzje.kolicina;
					udioPrim = primOruzje.komponenta.velicina * primOruzje.kolicina;
				}
				else
					primOruzje = new Zbir<Oruzje>(null, 0);
				if (sekOruzje != null) {
					ukupno += sekOruzje.komponenta.velicina * sekOruzje.kolicina;
					udioSek = sekOruzje.komponenta.velicina * sekOruzje.kolicina;
				}
				else
					sekOruzje = new Zbir<Oruzje>(null, 0);
				udioPrim /= ukupno;
				udioSek /= ukupno;
				udio = (udioPrim + 1 - udioSek) / 2;
			}


			return new Dizajn(id, ime, trup, primOruzje.komponenta, sekOruzje.komponenta,
				udio, oklop, stit, specOprema, senzor, potisnici, mzPogon, reaktor,
				taktika);
		}
		#endregion
	}
}

