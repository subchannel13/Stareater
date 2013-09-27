using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stareater.Controllers;

namespace Stareater.GUI
{
	public partial class ConstructionSiteView : UserControl
	{
		private AConstructionSiteController controller;
		
		public ConstructionSiteView()
		{
			InitializeComponent();
		}
		
		public void SetView(ColonyController colonyController)
		{
			controller = colonyController;
		}
		
		private void queueButton_Click(object sender, EventArgs e)
		{
			if (controller == null)
				return;
			
			using (var form = new FormBuildingQueue(controller))
				form.ShowDialog();
		}
	}
}
