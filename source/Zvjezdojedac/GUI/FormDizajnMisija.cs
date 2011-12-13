using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Igra.Brodovi;

namespace Zvjezdojedac.GUI
{
	public partial class FormDizajnMisija : Form
	{
		Dictionary<Misija.Tip, List<Oruzje>> misije;

		public Oruzje OdabranaMisija { get; private set; }

		public FormDizajnMisija(Dictionary<Misija.Tip, List<Oruzje>> misije, Oruzje trenutnaMisija)
		{
			this.misije = misije;
			this.OdabranaMisija = trenutnaMisija;

			InitializeComponent();
		}
	}
}
