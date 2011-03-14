#if __MonoCS__
using System;
using System.Collections.Generic;
using Prototip.Podaci.Jezici;
using Prototip.Podaci;
namespace Prototip
{
	public partial class GtkFormMain : Gtk.Window
	{
		public GtkFormMain () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			
#if DEBUG
#else
			try
			{
#endif
				PodaciAlat.postaviPodatke();
				postaviJezik();
#if DEBUG
#else
			}
			catch (Exception e)
			{
				//MessageBox.Show(e.Message, "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
#endif

		}
		
		private void postaviJezik()
		{
			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormMain];
			
			btnNovaIgra.Label = jezik["NOVA_IGRA"].tekst(null);
			btnPostavke.Label = jezik["POSTAVKE"].tekst(null);
			btnUcitaj.Label = jezik["UCITAJ"].tekst(null);
			btnUgasi.Label = jezik["UGASI"].tekst(null);
		}

		
	}
}
#endif
