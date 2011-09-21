using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Podaci;

namespace Zvjezdojedac.Igra.Poruke
{
	public class PorukaZgradaSustav : Poruka
	{
		public PorukaZgradaSustav(Zvijezda zvijezda, Zgrada.ZgradaInfo zgradaInfo)
			: base(Tip.ZgradaSustav)
		{
			this.zvijezda = zvijezda;
			this.zgradaInfo = zgradaInfo;
		}

		public Zvijezda zvijezda { get; private set; }
		private Zgrada.ZgradaInfo zgradaInfo;

		public override string tekst
		{
			get
			{
				Dictionary<string, string> stringVars = new Dictionary<string, string>();
				stringVars.Add("ZGRADA", zgradaInfo.ime);
				stringVars.Add("ZVIJEZDA", zvijezda.ime);
				return Postavke.Jezik[Kontekst.FormPoruke, "porukaZgrada"].tekst(null, stringVars);
			}
		}

		#region Pohrana
		protected const string PohIzvor = "IZVOR";
		protected const string PohZgrada = "ZGRADA";
		public override void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohTip, (int)tip);
			izlaz.dodaj(PohZgrada, zgradaInfo);
			izlaz.dodaj(PohIzvor, zvijezda.id);
		}

		public static Poruka Ucitaj(PodaciCitac ulaz, Dictionary<int, Zvijezda> zvijezdeID)
		{
			string[] izvor = ulaz.podatak(PohIzvor).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			Zvijezda zvj = zvijezdeID[int.Parse(izvor[0])];
			int zgradaId = ulaz.podatakInt(PohZgrada);

			return new PorukaZgradaSustav(zvj, Zgrada.ZgradaInfoID[zgradaId]);
		}
		#endregion
	}
}
