using System;
using System.Collections.Generic;

namespace Zvjezdojedac.Igra.Brodovi.Dizajner
{
	public class Komponente
	{
		public MZPogon mzPogon { get; private set; }
		public Reaktor reaktor { get; private set; }
		public List<SpecijalnaOprema> specijalnaOprema { get; private set; }
		public List<Stit> stitovi { get; private set; }

		public Dictionary<SpecijalnaOprema.SpecijalnaOpremaInfo, SpecijalnaOprema> specijalnaOpremaInfo { get; private set; }
		public Dictionary<Stit.StitInfo, Stit> stitoviInfo { get; private set; }

		public Komponente(Igrac igrac, Trup trup)
		{
			mzPogon = MZPogon.MZPogonInfo.NajboljiMZPogon(igrac.efekti, trup.VelicinaMZPogona);
			reaktor = Reaktor.ReaktorInfo.NajboljiReaktor(igrac.efekti, trup.VelicinaReaktora);
			specijalnaOprema = SpecijalnaOprema.SpecijalnaOpremaInfo.DostupnaOprema(igrac.efekti, trup.velicina);
			stitovi = Stit.StitInfo.DostupniStitovi(igrac.efekti, trup.VelicinaStita);

			specijalnaOpremaInfo = new Dictionary<SpecijalnaOprema.SpecijalnaOpremaInfo, SpecijalnaOprema>();
			foreach (SpecijalnaOprema so in specijalnaOprema)
				specijalnaOpremaInfo.Add(so.info, so);

			stitoviInfo = new Dictionary<Stit.StitInfo, Stit>();
			foreach (Stit stit in stitovi)
				stitoviInfo.Add(stit.info, stit);
		}
	}
}
