using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Alati
{
	public class Sazetak
	{
		private uint[] sadrzaj;
		private int hash;

		public Sazetak(IList<uint> sadrzaj, IList<uint> maxVelicina)
		{
			List<uint> tmpSadrzaj = new List<uint>();
			uint trenutni = 0;
			uint faktor = 1;

			for (int i = 0; i < sadrzaj.Count; i++) {
				if (uint.MaxValue / maxVelicina[i] < faktor) {
					faktor = 1;
					tmpSadrzaj.Add(trenutni);
					trenutni = 0;
				}

				trenutni += sadrzaj[i] * faktor;

				uint faktorKorak = 2;
				while (faktorKorak < maxVelicina[i])
					faktorKorak *= 2;
				faktor *= faktorKorak;
			}
			if (faktor > 1)
				tmpSadrzaj.Add(trenutni);

			this.sadrzaj = tmpSadrzaj.ToArray();
			this.hash = 0;

			foreach (uint i in sadrzaj) {
				hash *= 31;
				hash += i.GetHashCode() & 0xff;
				hash &= 0x003fffff;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() != GetType()) return false;

			Sazetak o = (Sazetak)obj;
			if (o.sadrzaj.Length != sadrzaj.Length) return false;
			for (int i = 0; i < sadrzaj.Length; i++)
				if (o.sadrzaj[i] != sadrzaj[i])
					return false;

			return true;
		}

		public override int GetHashCode()
		{
			return hash;
		}
	}
}
