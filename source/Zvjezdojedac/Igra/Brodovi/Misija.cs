using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Podaci.Jezici;

namespace Zvjezdojedac.Igra.Brodovi
{
	public class Misija
	{
		public enum Tip
		{
			DirektnoOruzje = 0,
			Projektil,
			Kolonizacija,
			Popravak,
			Spijunaza,
			Tegljenje,
			CivilniTransport,
			VojniTransport,
			N
		}

		public const int VatrenaMoc = 0;
		public const int UcinkovitostStitova = 1;
		public const int Preciznost = 2;

		public const int BrKolonista = 0;
		public const int RadnaMjesta = 1;

		public const int Infiltracija = 0;
		public const int Spijunaza = 3;

		public const int Kapacitet = 0;

		public enum TipParameta
		{
			Cijelobrojni,
			Postotak
		}
		
		public struct Parametar
		{
			public string kod;
			public TipParameta tip;
			public bool mnoziKolicinom;

			public Parametar(string kod,TipParameta tip, bool mnoziKolicinom)
			{
				this.kod = kod;
				this.mnoziKolicinom = mnoziKolicinom;
				this.tip = tip;
			}

			public string opis
			{
				get
				{
					return Postavke.Jezik[Kontekst.Misije, kod].tekst();
				}
			}
		}

		#region Statično
		public static Dictionary<string, Tip> StringUMisiju = initStringUMisiju();
		public static Dictionary<Tip, Misija> Opisnici = initOpisniciMisija();

		private static Dictionary<Tip, Misija> initOpisniciMisija()
		{
			Dictionary<Tip, Misija> ret = new Dictionary<Tip,Misija>();
			ret.Add(Tip.DirektnoOruzje,
				new Misija("MIS_DIREKT_IME",
					"MIS_DIREKT_OPIS",
					true, false, false,
					new Parametar[] { 
						new Parametar("VATRENA_MOC", TipParameta.Cijelobrojni, false),
						new Parametar("BR_NAPADA", TipParameta.Postotak, false),
						new Parametar("PRECIZNOST", TipParameta.Cijelobrojni, false)
					}));

			ret.Add(Tip.Projektil,
				new Misija("MIS_PROJEKTIL_IME",
					"MIS_PROJEKTIL_OPIS",
					true, false, false,
					new Parametar[] { 
						new Parametar("VATRENA_MOC", TipParameta.Cijelobrojni, false),
						new Parametar("BR_NAPADA", TipParameta.Postotak, false),
						new Parametar("PRECIZNOST", TipParameta.Cijelobrojni, false)
					}));

			ret.Add(Tip.Kolonizacija,
				new Misija("MIS_KOLONIZ_IME",
					"MIS_KOLONIZ_OPIS",
					false, true, true,
					new Parametar[] { 
						new Parametar("POP", TipParameta.Cijelobrojni, true),
						new Parametar("RADNA_MJ", TipParameta.Cijelobrojni, true)
					}));

			ret.Add(Tip.Popravak,
				new Misija("MIS_POPRAVAK_IME",
					"MIS_POPRAVAK_OPIS",
					false, false, true,
					new Parametar[] { 
						new Parametar("IND", TipParameta.Cijelobrojni, true)
					}));

			ret.Add(Tip.Spijunaza,
				new Misija("MIS_SPIJUNAZ_IME",
					"MIS_SPIJUNAZ_OPIS",
					true, false, false,
					new Parametar[] { 
						new Parametar("INFILTRACIJA", TipParameta.Cijelobrojni, false),
						new Parametar("BR_NAPADA", TipParameta.Postotak, false),
						new Parametar("PRECIZNOST", TipParameta.Cijelobrojni, false),
						new Parametar("SPIJUNAZA", TipParameta.Cijelobrojni, true)
					}));

			ret.Add(Tip.Tegljenje,
				new Misija("MIS_TEGLJA_IME",
					"MIS_TEGLJA_OPIS",
					false, false, true,
					new Parametar[] { 
						new Parametar("KAPACITET", TipParameta.Cijelobrojni, true)
					}));

			ret.Add(Tip.CivilniTransport,
				new Misija("MIS_CIV_TRANS_IME",
					"MIS_CIV_TRANS_OPIS",
					false, true, true,
					new Parametar[] { 
						new Parametar("KAPACITET", TipParameta.Cijelobrojni, true)
					}));

			ret.Add(Tip.VojniTransport,
				new Misija("MIS_VOJ_TRANS_IME",
					"MIS_VOJ_TRANS_OPIS",
					false, true, true,
					new Parametar[] { 
						new Parametar("KAPACITET", TipParameta.Cijelobrojni, true)
					}));

			return ret;
		}

		private static Dictionary<string, Tip> initStringUMisiju()
		{
			Dictionary<string, Tip> stringUMisiju = new Dictionary<string, Tip>();
			stringUMisiju.Add("DIREKTNO_ORUZJE", Tip.DirektnoOruzje);
			stringUMisiju.Add("PROJEKTIL", Tip.Projektil);
			stringUMisiju.Add("KOLONIZACIJA", Tip.Kolonizacija);
			stringUMisiju.Add("POPRAVAK", Tip.Popravak);
			stringUMisiju.Add("SPIJUNAZA", Tip.Spijunaza);
			stringUMisiju.Add("TEGLJENJE", Tip.Tegljenje);
			stringUMisiju.Add("CIV_TRANSPORT", Tip.CivilniTransport);
			stringUMisiju.Add("VOJNI_TRANSPORT", Tip.VojniTransport);
			return stringUMisiju;
		}
		#endregion

		private string nazivKod;
		private string opisKod;
		public bool imaCiljanje { get; private set; }
		public bool jednokratna { get; private set; }
		public bool grupirana { get; private set; }
		public Parametar[] parametri { get; private set; }

		private Misija(string naziv, string opis, 
			bool imaCiljanje, bool jednokratna, bool grupirana,
			Parametar[] parametri)
		{
			this.naziv = naziv;
			this.opis = opis;
			this.imaCiljanje = imaCiljanje;
			this.jednokratna = jednokratna;
			this.grupirana = grupirana;
			this.parametri = parametri;
		}

		public int brParametara
		{
			get { return parametri.Length; }
		}

		public string naziv
		{
			get
			{
				return Postavke.Jezik[Kontekst.Misije, nazivKod].tekst();
			}
			private set
			{
				nazivKod = value;
			}
		}

		public string opis
		{
			get
			{
				return Postavke.Jezik[Kontekst.Misije, opisKod].tekst();
			}
			private set
			{
				opisKod = value;
			}
		}
	}
}
