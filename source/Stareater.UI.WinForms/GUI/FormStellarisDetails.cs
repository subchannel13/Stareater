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
	public partial class FormStellarisDetails : Form
	{
		private StellarisAdminController controller;

		public FormStellarisDetails()
		{
			InitializeComponent();
		}

		public FormStellarisDetails(StellarisAdminController controller) : this()
		{
			this.controller = controller;
			//TODO: set language
		}
	}
}
