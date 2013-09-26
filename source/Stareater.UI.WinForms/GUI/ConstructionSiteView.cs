using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stareater.Controllers;

namespace Stareater.GUI
{
	public partial class ConstructionSiteView : UserControl
	{
		private AConstructionSiteController siteController;
		
		public ConstructionSiteView()
		{
			InitializeComponent();
		}
		
		public void SetView(ColonyController colonyController)
		{
			siteController = colonyController;
		}
		
		private void queueButton_Click(object sender, EventArgs e)
		{
			using (var form = new FormBuildingQueue())
				form.ShowDialog();
		}
	}
}
