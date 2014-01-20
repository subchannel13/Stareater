using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.Controllers;

namespace Stareater.GUI
{
	public partial class FormColonyDetails : Form
	{
		private ColonyController controller;

		public FormColonyDetails()
		{
			InitializeComponent();
		}

		public FormColonyDetails(ColonyController controller) : this()
		{
			this.controller = controller;
			//TODO: set language
		}
	}
}
