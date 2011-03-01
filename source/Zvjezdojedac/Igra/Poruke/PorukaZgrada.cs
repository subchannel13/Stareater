using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototip.Podaci.Jezici;
using Prototip.Podaci;

namespace Prototip.Igra.Poruke
{
	public class PorukaZgrada : Poruka
	{
		public PorukaZgrada(Planet planet, Zgrada.ZgradaInfo zgradaInfo)
			: base(Tip.Zgrada)
		{
			this.planet = planet;
			this.zgradaInfo = zgradaInfo;
		}

		public Planet planet { get; private set; }
		private Zgrada.ZgradaInfo zgradaInfo;

		public override string tekst
		{
			get 
			{
				Dictionary<string, string> stringVars = new Dictionary<string, string>();
				stringVars.Add("PLANET", planet.ime);
				stringVars.Add("ZGRADA", zgradaInfo.ime);
				return Postavke.jezik[Kontekst.FormPoruke, "porukaZgrada"].tekst(null, stringVars);
			}
		}

		#region Pohrana
		protected const string PohIzvor = "IZVOR";
		protected const string PohZgrada = "ZGRADA";
		public override void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohTip, (int)tip);
			izlaz.dodaj(PohIzvor, planet.zvjezda.id + " " + planet.pozicija);
			izlaz.dodaj(PohZgrada, zgradaInfo);
		}

		public static Poruka Ucitaj(PodaciCitac ulaz, Dictionary<int, Zvijezda> zvijezdeID)
		{
			string[] izvor = ulaz.podatak(PohIzvor).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			Zvijezda zvj = zvijezdeID[int.Parse(izvor[0])];
			int planetId = int.Parse(izvor[1]);
			int zgradaId = ulaz.podatakInt(PohZgrada);

			return new PorukaZgrada(zvj.planeti[planetId], Zgrada.ZgradaInfoID[zgradaId]);
		}
		#endregion
	}
}
