using System;
using System.Collections.Generic;
using System.Text;

namespace Zvjezdojedac.Igra.Brodovi
{
	public class PredefiniraniDizajn
	{
		public static HashSet<PredefiniraniDizajn> dizajnovi = new HashSet<PredefiniraniDizajn>();

		public static void Dodaj(Dictionary<string, string> podaci)
		{
			Dictionary<SpecijalnaOprema.SpecijalnaOpremaInfo, int> specijalnaOprema = new Dictionary<SpecijalnaOprema.SpecijalnaOpremaInfo,int>();
			string[] specOpremaPodaci = podaci["SPEC_OPREMA"].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string str in specOpremaPodaci)
			{
				string[] soPodatak = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				specijalnaOprema.Add(SpecijalnaOprema.SpecijalnaOpremaInfo.Kodovi[soPodatak[0]], int.Parse(soPodatak[1]));
			}

			List<Tehnologija.Preduvjet> preduvjeti = Tehnologija.Preduvjet.NaciniPreduvjete(podaci["PREDUVJETI"]);
			
			Oruzje.OruzjeInfo primOruzje = null;
			Oruzje.OruzjeInfo sekOruzje = null;
			if (podaci["PRIM_ORUZJE"].Length != 0) primOruzje = Oruzje.OruzjeInfo.KodoviOruzja[podaci["PRIM_ORUZJE"]];
			if (podaci["SEK_ORUZJE"].Length != 0) sekOruzje = Oruzje.OruzjeInfo.KodoviOruzja[podaci["SEK_ORUZJE"]];

			Stit.StitInfo stit = null;
			if (podaci["STIT"].Length != 0) stit = Stit.StitInfo.Kodovi[podaci["STIT"]];

			dizajnovi.Add(new PredefiniraniDizajn(
				podaci["NAZIV"], 
				preduvjeti,
				Trup.TrupInfo.KodoviTrupova[podaci["TRUP"]],
				primOruzje,
				sekOruzje,
				stit,
				specijalnaOprema,
				double.Parse(podaci["UDIO_PRIM_ORUZJA"]),
				podaci.ContainsKey("MZ_POGON"),
				Taktika.Kodovi[podaci["TAKTIKA"]]));
		}

		public string naziv { get; private set; }
		public Trup.TrupInfo trup { get; private set; }
		public Oruzje.OruzjeInfo primarnoOruzje { get; private set; }
		public Oruzje.OruzjeInfo sekundarnoOruzje { get; private set; }
		public Stit.StitInfo stit { get; private set; }
		public Dictionary<SpecijalnaOprema.SpecijalnaOpremaInfo, int> specijalnaOprema { get; private set; }
		public double udioPrimarnogOruzja { get; private set; }
		public bool mzPogon { get; private set; }
		public Taktika taktika { get; private set; }
		public List<Tehnologija.Preduvjet> preduvjeti { get; private set; }

		private PredefiniraniDizajn(string naziv, List<Tehnologija.Preduvjet> preduvjeti,
			Trup.TrupInfo trup,	Oruzje.OruzjeInfo primarnoOruzje, 
			Oruzje.OruzjeInfo sekundarnoOruzje, Stit.StitInfo stit, 
			Dictionary<SpecijalnaOprema.SpecijalnaOpremaInfo, int> specijalnaOprema,
			double udioPrimarnogOruzja, bool mzPogon, Taktika taktika)
		{
			this.naziv = naziv;
			this.preduvjeti = preduvjeti;
			this.trup = trup;
			this.primarnoOruzje = primarnoOruzje;
			this.sekundarnoOruzje = sekundarnoOruzje;
			this.stit = stit;
			this.specijalnaOprema = specijalnaOprema;
			this.udioPrimarnogOruzja = udioPrimarnogOruzja;
			this.mzPogon = mzPogon;
			this.taktika = taktika;
		}

		public bool dostupan(Dictionary<string, double> varijable)
		{
			foreach (Tehnologija.Preduvjet pred in preduvjeti)
				if (!pred.zadovoljen(varijable))
					return false;

			if (!trup.dostupno(varijable)) return false;
			if (stit != null)
				if (!stit.dostupno(varijable)) return false;
			if (primarnoOruzje != null)
				if (!primarnoOruzje.dostupno(varijable)) return false;
			if (sekundarnoOruzje != null)
				if (!sekundarnoOruzje.dostupno(varijable)) return false;

			foreach (SpecijalnaOprema.SpecijalnaOpremaInfo soi in specijalnaOprema.Keys)
				if (!soi.dostupno(varijable))
					return false;
		
			return true;
		}

		public Dizajn naciniDizajn(Dictionary<string, double> varijable)
		{
			Trup trup = this.trup.naciniKomponentu(varijable);
			Oruzje primarnoOruzje = (this.primarnoOruzje != null) ? this.primarnoOruzje.naciniKomponentu(varijable) : null;
			Oruzje sekundarnoOruzje = (this.sekundarnoOruzje != null) ? this.sekundarnoOruzje.naciniKomponentu(varijable) : null;
			Oklop oklop = Oklop.OklopInfo.NajboljiOklop(varijable);
			Stit stit = (this.stit != null) ? this.stit.naciniKomponentu(varijable, trup.velicina_stita) : null;
			Senzor senzor = Senzor.SenzorInfo.NajboljiSenzor(varijable);
			Potisnici potisnici = Potisnici.PotisnikInfo.NajboljiPotisnici(varijable);
			Reaktor reaktor = Reaktor.ReaktorInfo.NajboljiReaktor(varijable, trup.velicina_reaktora);

			Dictionary<SpecijalnaOprema, int> specijalnaOprema = new Dictionary<SpecijalnaOprema,int>();
			foreach(SpecijalnaOprema.SpecijalnaOpremaInfo soi in this.specijalnaOprema.Keys)
				specijalnaOprema.Add(soi.naciniKomponentu(varijable, trup.velicina), this.specijalnaOprema[soi]);

			MZPogon mzPogon = null;
			if (this.mzPogon) mzPogon = MZPogon.MZPogonInfo.NajboljiMZPogon(varijable, trup.velicina_MZPogona);

			return new Dizajn(naziv, trup, primarnoOruzje, sekundarnoOruzje,
				udioPrimarnogOruzja, oklop, stit, specijalnaOprema,
				senzor, potisnici, mzPogon, reaktor, taktika);
		}
	}
}
