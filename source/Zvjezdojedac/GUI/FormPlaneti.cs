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
	public partial class FormPlaneti : Form
	{
		IgraZvj igra;

		public FormPlaneti()
		{
			InitializeComponent();
		}

		public FormPlaneti(IgraZvj igra)
			: this()
		{
			this.igra = igra;

			foreach (var kolonija in igra.trenutniIgrac().kolonije)
				planetList.Controls.Add(new PlanetItem(kolonija));

		}
	}
}
