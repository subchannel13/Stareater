using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stareater.Controllers;

namespace Stareater.GUI
{
	public partial class ColonizationTargetView : UserControl
	{
		private EmptyPlanetController controller;
		
		public ColonizationTargetView()
		{
			InitializeComponent();
		}
		
		public ColonizationTargetView(EmptyPlanetController controller) : this()
		{
			this.controller = controller;
		}
	}
}
