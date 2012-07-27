using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Igra;

namespace Zvjezdojedac.GUI
{
	public partial class FormZvijezde : Form
	{
		IgraZvj igra;

		public FormZvijezde()
		{
			InitializeComponent();
		}

		public FormZvijezde(IgraZvj igra) : this()
		{
			this.igra = igra;

			foreach(var uprava in igra.mapa.ZvjezdaneUprave())
				if (uprava.Igrac == igra.trenutniIgrac()) {
					starList.Controls.Add(new StarItem(uprava));
				}
		}
	}
}
