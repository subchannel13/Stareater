using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stareater.GUI
{
	public partial class FormSaveLoad : Form
	{
		public FormSaveLoad()
		{
			InitializeComponent();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
