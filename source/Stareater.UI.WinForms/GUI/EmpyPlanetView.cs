using System;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Localization;

namespace Stareater.GUI
{
	public partial class EmpyPlanetView : UserControl
	{
		private EmptyPlanetController controller;
		
		public EmpyPlanetView()
		{
			InitializeComponent();
		}
		
		public void SetView(EmptyPlanetController planetController)
		{
			controller = planetController;
			
			resetView();
		}
		
		private void resetView()
		{
			this.Font = SettingsWinforms.Get.FormFont;

			var context = SettingsWinforms.Get.Language["FormMain"];
			this.detailsButton.Text = context["SiteDetails"].Text();
		}
	}
}
