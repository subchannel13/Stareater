using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stareater
{
	public partial class FormShipDesigner : Form
	{
		private GameController controller;
		
		public FormShipDesigner()
		{
			InitializeComponent();
		}
		
		public FormShipDesigner(GameController controller) : this()
		{
			this.controller = controller;
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
