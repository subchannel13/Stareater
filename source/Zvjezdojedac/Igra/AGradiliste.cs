using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Igra
{
	public abstract class AGradiliste
	{
		public Dictionary<string, double> Efekti { get; private set; }
		public Igrac Igrac { get; protected set; }
		public LinkedList<Zgrada.ZgradaInfo> RedGradnje { get; private set; }
		public Dictionary<Zgrada.ZgradaInfo, Zgrada> Zgrade { get; private set; }

		protected Dictionary<string, double> ostatakGradnje { get; private set; }

		public AGradiliste(Igrac igrac, LinkedList<Zgrada.ZgradaInfo> redGradnje,
			Dictionary<string, double> ostatakGradnje)
		{
			this.Efekti = new Dictionary<string, double>();
			this.Igrac = igrac;
			this.RedGradnje = redGradnje;
			this.Zgrade = new Dictionary<Zgrada.ZgradaInfo, Zgrada>();

			this.ostatakGradnje = new Dictionary<string, double>();
			foreach (string grupa in Zgrada.Grupe)
				this.ostatakGradnje.Add(grupa, 0);
			foreach (var element in ostatakGradnje)
				this.ostatakGradnje[element.Key] = element.Value;
		}

		public AGradiliste(Igrac igrac)
			: this(igrac, new LinkedList<Zgrada.ZgradaInfo>(), new Dictionary<string,double>())
		{ }

		public abstract Zvijezda LokacijaZvj { get; }
		public abstract List<Zgrada.ZgradaInfo> MoguceGraditi();
	}
}
