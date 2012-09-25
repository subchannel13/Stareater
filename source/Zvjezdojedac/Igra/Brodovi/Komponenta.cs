using System;
using System.Collections.Generic;

namespace Zvjezdojedac.Igra.Brodovi
{
	public class Komponenta<Info> : IKomponenta where Info : AKomponentaInfo
	{
		/// <summary>
		/// Nivo komponente.
		/// </summary>
		public int nivo { get; private set; }
		/// <summary>
		/// Objekt s informacijama o tipu komponente.
		/// </summary>
		public Info info { get; private set; }

		protected Komponenta(Info info, int nivo)
		{
			this.info = info;
			this.nivo = nivo;
		}

		public bool dostupno(Dictionary<string, double> varijable)
		{
			foreach(Preduvjet preduvjet in info.preduvjeti)
				if (!preduvjet.zadovoljen(varijable))
					return false;
			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() != GetType()) return false;
			Komponenta<Info> other = (Komponenta<Info>)obj;

			return (other.info == info) && (other.nivo == nivo);
		}

		public override int GetHashCode()
		{
			return info.GetHashCode() * 31 + nivo;
		}

		public int maxNivo
		{
			get { return info.maxNivo; }
		}

		public string naziv
		{
			get { return info.naziv; }
		}

		public string opis
		{
			get { return info.opis; }
		}

		public List<Preduvjet> preduvjeti
		{
			get { return info.preduvjeti; }
		}

		public System.Drawing.Image slika
		{
			get { return info.slika; }
		}

		public string pohrani()
		{
			return info.id + " " + nivo;
		}
	}
}
