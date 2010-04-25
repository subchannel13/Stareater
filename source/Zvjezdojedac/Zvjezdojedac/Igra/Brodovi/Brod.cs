using System;
using System.Collections.Generic;
using System.Text;

namespace Prototip
{
	public class Brod
	{
		public Dizajn dizajn { get; private set; }
		public int kolicina { get; private set; }

		public Brod(Dizajn dizajn, int kolicina)
		{
			this.dizajn = dizajn;
			this.kolicina = kolicina;
		}

		public void dodaj(Brod brod)
		{
			kolicina += brod.kolicina;
		}
	}
}
