using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototip
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
			public string opis;
			public TipParameta tip;
			public bool mnoziKolicinom;

			public Parametar(string kod, string opis,
				TipParameta tip, bool mnoziKolicinom)
			{
				this.kod = kod;
				this.mnoziKolicinom = mnoziKolicinom;
				this.opis = opis;
				this.tip = tip;
			}
		}

		#region Statično
		public static Dictionary<string, Tip> StringUMisiju = initStringUMisiju();
		public static Dictionary<Tip, Misija> Opisnici = initOpisniciMisija();

		private static Dictionary<Tip, Misija> initOpisniciMisija()
		{
			Dictionary<Tip, Misija> ret = new Dictionary<Tip,Misija>();
			ret.Add(Tip.DirektnoOruzje, 
				new Misija("Oružje",
					"direktno oružje",
					true, false,
					new Parametar[] { 
						new Parametar("VATRENA_MOC", "Vatrena moć", TipParameta.Cijelobrojni, false),
						new Parametar("BR_NAPADA", "Učinkovitost štitova", TipParameta.Postotak, false),
						new Parametar("PRECIZNOST", "Preciznost", TipParameta.Cijelobrojni, false)
					}));

			ret.Add(Tip.Projektil,
				new Misija("Oružje (projektil)",
					"projektil",
					true, false,
					new Parametar[] { 
						new Parametar("VATRENA_MOC", "Vatrena moć", TipParameta.Cijelobrojni, false),
						new Parametar("BR_NAPADA", "Učinkovitost štitova", TipParameta.Postotak, false),
						new Parametar("PRECIZNOST", "Preciznost", TipParameta.Cijelobrojni, false)
					}));

			ret.Add(Tip.Kolonizacija, 
				new Misija("Kolonizacija",
					"moduli za uspostavljanje kolonije",
					false, true,
					new Parametar[] { 
						new Parametar("POP", "Br. kolonista", TipParameta.Cijelobrojni, true),
						new Parametar("RADNA_MJ", "Radna mjesta", TipParameta.Postotak, true)
					}));

			ret.Add(Tip.Popravak, 
				new Misija("Popravak i nadogradnja",
					"postrojenje za popravak i nadogradnju brodova",
					false, true,
					new Parametar[] { 
						new Parametar("IND", "Industrija", TipParameta.Cijelobrojni, true)
					}));

			ret.Add(Tip.Spijunaza, 
				new Misija("Špijunaža",
					"spijunska oprema",
					true, false,
					new Parametar[] { 
						new Parametar("INFILTRACIJA", "Infiltracija", TipParameta.Cijelobrojni, false),
						new Parametar("BR_NAPADA", "Učinkovitost štitova", TipParameta.Postotak, false),
						new Parametar("PRECIZNOST", "Preciznost", TipParameta.Cijelobrojni, false),
						new Parametar("SPIJUNAZA", "Špijunaža", TipParameta.Cijelobrojni, true)
					}));

			ret.Add(Tip.Tegljenje, 
				new Misija("Tegljenje",
					"međuzvjezdani transport brodova",
					false, true,
					new Parametar[] { 
						new Parametar("KAPACITET", "Kapacitet", TipParameta.Cijelobrojni, true)
					}));

			ret.Add(Tip.CivilniTransport, 
				new Misija("Civilni transport",
					"civilni transport",
					false, true,
					new Parametar[] { 
						new Parametar("KAPACITET", "Kapacitet", TipParameta.Cijelobrojni, true)
					}));

			ret.Add(Tip.VojniTransport,
				new Misija("Vojni transport",
					"vojni transport",
					false, true,
					new Parametar[] { 
						new Parametar("KAPACITET", "Kapacitet", TipParameta.Cijelobrojni, true)
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

		public string naziv { get; private set; }
		public string opis { get; private set; }
		public bool imaCiljanje { get; private set; }
		public bool grupirana { get; private set; }
		public Parametar[] parametri { get; private set; }

		private Misija(string naziv, string opis, 
			bool imaCiljanje, bool grupirana,
			Parametar[] parametri)
		{
			this.naziv = naziv;
			this.opis = opis;
			this.imaCiljanje = imaCiljanje;
			this.grupirana = grupirana;
			this.parametri = parametri;
		}

		public int brParametara
		{
			get { return parametri.Length; }
		}
	}
}
