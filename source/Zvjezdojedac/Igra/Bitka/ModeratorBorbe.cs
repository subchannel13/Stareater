using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Igra.Brodovi;

namespace Zvjezdojedac.Igra.Bitka
{
	public class ModeratorBorbe
	{
		public const double sigmoidBase = 0.90483741803595957316424905944644;

		Dictionary<Igrac, Strana> strane;
		Konflikt konflikt;

		List<Igrac> igraci;
		int trenutniIgrac = 0;
		Random random = new Random();

		HashSet<Borac> sviBorci = new HashSet<Borac>();

		public ModeratorBorbe(Dictionary<Igrac, Strana> borci, Konflikt konflikt)
		{
			this.strane = borci;
			this.konflikt = konflikt;

			this.igraci = new List<Igrac>(borci.Keys);
			igraci.Sort((a, b) => a.id - b.id);

			popisiBorce();
			fazaOtkrivanja();
		}

		private void popisiBorce()
		{
			sviBorci.Clear();

			foreach (var strana in strane)
				sviBorci.UnionWith(strana.Value.Borci);
		}


		#region Getteri
		public Igrac TrenutniIgrac
		{
			get { return igraci[trenutniIgrac]; }
		}

		public HashSet<Igrac> LijevaStrana
		{
			get
			{
				HashSet<Igrac> rez = new HashSet<Igrac>();
				rez.Add(igraci[trenutniIgrac]);
				return rez;
			}
		}

		public HashSet<Igrac> DesnaStrana
		{
			get 
			{
				HashSet<Igrac> rez = new HashSet<Igrac>(igraci);
				rez.Remove(igraci[trenutniIgrac]);
				return rez; 
			}
		}

		public IEnumerable<Borac> SviBorci
		{
			get { return sviBorci; }
		}

		public int PreostaloKrugova
		{
			get { return konflikt.PreostaloKrugova; }
		}
		#endregion

		public void SlijedecaFaza()
		{
			if (konflikt.Faza != StanjeKonflikta.SvemirskiSukob)
				return;

			do {
				trenutniIgrac = (trenutniIgrac + 1) % igraci.Count;
				
				if (trenutniIgrac == 0) {
					konflikt.ZavrsiKrug();
					ZavrsiKrug();

					if (Razrjeseno()) {
						konflikt.ZavrsiSvemirskuBitku();
						foreach (var strana in strane)
							strana.Value.BitkaZavrsena(konflikt.Lokacija);
						break;
					}
				}
				else if (TrenutniIgrac.tip == Igrac.Tip.RACUNALO)
					TrenutniIgrac.Upravljac.OdigrajKrugBitke(this);

			} while (TrenutniIgrac.tip != Igrac.Tip.COVJEK);
		}

		public bool Razrjeseno()
		{
			if (konflikt.PreostaloKrugova <= 0)
				return true;

			foreach (Strana strana in strane.Values)
				if (sviBorci.Count == strana.Borci.Count)
					return true;

			return false;
		}

		private static double Vjerojatnost(double napadac, double meta)
		{
			double x = napadac - meta;

			if (x < 0)
				return 0.5 * Math.Pow(sigmoidBase, -x);
			else
				return 1 - 0.5 * Math.Pow(sigmoidBase, x);
		}

		private static double Upijanje(double snagaNapada, double snagaUpijanja)
		{
			if (snagaNapada <= 0)
				return 0;

			if (double.IsPositiveInfinity(snagaUpijanja))
				return snagaNapada;
			
			return snagaNapada * Math.Pow(2, -snagaUpijanja / snagaNapada);
		}

		private void ZavrsiKrug()
		{
			foreach (Strana strana in strane.Values)
				fazaPaljbe(strana);

			foreach (Igrac igrac in strane.Keys)
				fazaOporavka(igrac);

			fazaOtkrivanja();
			popisiBorce();
		}
		
		private void fazaOtkrivanja()
		{
			foreach (Strana strana in strane.Values) {
				double[] snagaSenzora = new double[Pozicije.MaxPozicija + 1];
				for (int i = 0; i < snagaSenzora.Length; i++)
					snagaSenzora[i] = double.NaN;

				foreach (Borac borac in strana.Borci) {
					int slot = Fje.Ogranici((int)Math.Round(borac.Pozicija), 0, Pozicije.MaxPozicija);
					if (double.IsNaN(snagaSenzora[slot]))
						snagaSenzora[slot] = borac.Dizajn.snagaSenzora;
					else
						snagaSenzora[slot] = Math.Max(borac.Dizajn.snagaSenzora, snagaSenzora[slot]);
				}

				strana.PostaviSnaguSenzora(snagaSenzora);
			}
			
			foreach (Borac borac in sviBorci)
				foreach (var strana in strane)
					if (borac.Igrac != strana.Key) {
						double vjerojatnost = Vjerojatnost(strana.Value.SnagaSenzora((int)Math.Round(-borac.Pozicija)), borac.Dizajn.prikrivenost);
						borac.Vidljiv[strana.Key.id] = (vjerojatnost > random.NextDouble());
					}
		}

		private void fazaPaljbe(Strana strana)
		{
			PopisMeta popisMeta = new PopisMeta();
			foreach (var drugaStrana in strane.Values)
				if (drugaStrana != strana)
					foreach (var borac in drugaStrana.Borci)
						if (borac.Vidljiv[strana.Igrac.id])
							popisMeta.Dodaj(borac);

			foreach (Borac borac in strana.Borci) {
				Dizajn dizajn = borac.Dizajn;

				if (dizajn.primarnoOruzje != null && dizajn.primarnaMisija == Misija.Tip.DirektnoOruzje) 
					paljba(strana, borac.Dizajn.primarnoOruzje, popisMeta, (int)Math.Round(borac.Pozicija));

				if (dizajn.sekundarnoOruzje != null && dizajn.sekundarnaMisija == Misija.Tip.DirektnoOruzje)
					paljba(strana, borac.Dizajn.sekundarnoOruzje, popisMeta, (int)Math.Round(borac.Pozicija));

				if (popisMeta.jePrazan())
					break;
			}
		}

		private void paljba(Strana strana, Dizajn.Zbir<Oruzje> zbir, PopisMeta popisMeta, int pozicijaNapadaca)
		{
			Borac meta = null;
			double vjerotanostPogotka = 1;
			Pozicije.EfektUdaljenosti efektUdaljenosti = null;
			Oruzje oruzje = zbir.komponenta;

			for (int i = 0; i < zbir.kolicina; i++) {

				if (meta == null)
					if (!popisMeta.jePrazan()) {
						meta = popisMeta.DajSlijedeci(zbir.komponenta.ciljanje, random.NextDouble());
						efektUdaljenosti = Pozicije.EfektUdaljenosti.Izracunaj(Math.Abs(pozicijaNapadaca - Math.Round(meta.Pozicija)));
						
						double ometanje = Math.Max(meta.Dizajn.ometanje - strana.SnagaSenzora((int)Math.Round(-meta.Pozicija)), 0);
						vjerotanostPogotka = Vjerojatnost(oruzje.preciznost + efektUdaljenosti.Preciznost, meta.Dizajn.pokretljivost) * Math.Pow(sigmoidBase, ometanje);
					}
					else
						break;
				
				if (vjerotanostPogotka > random.NextDouble()) {
					Dizajn dizajnMete = meta.Dizajn;
					double vatrenaMoc = oruzje.vatrenaMoc;

					if (meta.IzdrzljivostStita > 0) {
						double ulaznaSteta = Upijanje(vatrenaMoc, dizajnMete.debljinaStita);
						double ublazavanjeStete = Math.Max(0, dizajnMete.ublazavanjeSteteStita * Math.Sqrt(meta.IzdrzljivostStita / dizajnMete.izdrzljivostStita) - oruzje.penetracijaStita);

						double steta = Math.Min(meta.IzdrzljivostStita, Upijanje(ulaznaSteta, ublazavanjeStete));
						meta.IzdrzljivostStita -= steta;
						vatrenaMoc -= steta;
					}

					meta.IzdrzljivostOklopa -= Upijanje(vatrenaMoc, dizajnMete.ublazavanjeSteteOklopa);

					if (meta.IzdrzljivostOklopa <= 0) {
						popisMeta.Ukloni(meta);
						meta = null;
					}
				}
			}
		}

		private void fazaOporavka(Igrac igrac)
		{
			Strana igracevaStrana = strane[igrac];
			igracevaStrana.PaliBorci.Clear();

			foreach (Borac borac in igracevaStrana.Borci)
				if (borac.IzdrzljivostOklopa > 0) {
					if (borac.Dizajn.izdrzljivostStita > 0)
						borac.IzdrzljivostStita = Math.Min(
							borac.IzdrzljivostStita + borac.Dizajn.obnavljanjeStita,
							borac.Dizajn.izdrzljivostStita);

					foreach (var strana in strane)
						if (borac.Igrac != strana.Key) {
							double vjerojatnost = Vjerojatnost(strana.Value.SnagaSenzora((int)Math.Round(-borac.Pozicija)), borac.Dizajn.prikrivenost);
							
							borac.Vidljiv[strana.Key.id] = borac.Vidljiv[strana.Key.id] && 
								((vjerojatnost > random.NextDouble()) || 
								(vjerojatnost > random.NextDouble()));
						}

					if (Math.Abs(borac.CiljnaPozicija - borac.Pozicija) < borac.Dizajn.brzina)
						borac.Pozicija = borac.CiljnaPozicija;
					else
						borac.Pozicija += borac.Dizajn.brzina * Math.Sign(borac.CiljnaPozicija - borac.Pozicija);
					
				}
				else {
					igracevaStrana.Borci.PendRemove(borac);
					igracevaStrana.PaliBorci.Add(borac);
				}

			igracevaStrana.Borci.ApplyRemove();
		}
	}
}
