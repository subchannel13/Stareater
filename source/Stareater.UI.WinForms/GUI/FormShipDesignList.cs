using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stareater.Controllers;

namespace Stareater.GUI
{
	public partial class FormShipDesignList : Form
	{
		private GameController controller;
		
		public FormShipDesignList()
		{
			InitializeComponent();
		}
		
		public FormShipDesignList(GameController controller) : this()
		{
			this.controller = controller;
			updateList();
		}
		
		private void updateList()
		{
			var designs = controller.ShipsDesigns().ToArray();
			designList.SuspendLayout();
			
			while (designList.Controls.Count < designs.Length)
				designList.Controls.Add(new DesignItem());
			while (designList.Controls.Count > designs.Length)
				designList.Controls.RemoveAt(designList.Controls.Count - 1);

			for (int i = 0; i < designs.Length; i++) {
				(designList.Controls[i] as DesignItem).Data = designs[i];
				(designList.Controls[i] as DesignItem).Count = 0; //TODO(v0.5): count built ships 
			}
			
			designList.ResumeLayout();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void newDesignButton_Click(object sender, EventArgs e)
		{
			using(var form = new FormShipDesigner(controller))
				form.ShowDialog();
		}
	}
}
