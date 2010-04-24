using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prototip;

namespace Zvjezdojedac_editori
{
	public partial class FormPreduvjeti : Form
	{
		public List<Tehnologija.Preduvjet> preduvjeti { get; private set; }
		private Dictionary<string, string> tehKodovi = new Dictionary<string,string>();
		
		public FormPreduvjeti(List<Tehnologija.Preduvjet> preduvjeti)
		{
			InitializeComponent();

			foreach(Tehnologija.TechInfo teh in Tehnologija.TechInfo.tehnologijeIstrazivanje)
				tehKodovi.Add(teh.kod + "_LVL", teh.ime);
			foreach(Tehnologija.TechInfo teh in Tehnologija.TechInfo.tehnologijeRazvoj)
				tehKodovi.Add(teh.kod + "_LVL", teh.ime);

			this.preduvjeti = new List<Tehnologija.Preduvjet>(preduvjeti);
			foreach (Tehnologija.Preduvjet p in preduvjeti)
			{
				ListViewItem item = new ListViewItem(tehKodovi[p.kod]);
				item.SubItems.Add(p.nivo.ToString());
				lstvPreduvjeti.Items.Add(item);
			}
		}
	}
}
