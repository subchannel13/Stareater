using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Utils.Collections;

namespace Stareater.GUI
{
	public partial class ColonizationSourceView : UserControl
	{
		private StellarisInfo sourceData = null;
		private ColonizationController controller;
		
		public event Action OnStateChange;
		
		public ColonizationSourceView()
		{
			InitializeComponent();
		}
		
		public ColonizationSourceView(ColonizationController controller) : this()
		{
			if (controller == null)
				throw new ArgumentNullException("controller");
			this.controller = controller;
		}
		
		public StellarisInfo Data 
		{
			get
			{
				return sourceData;
			}
			set
			{
				this.sourceData = value;
				
				updateView();
			}
		}
		
		private void updateView()
		{
			var context = SettingsWinforms.Get.Language["FormColonization"];
			
			if (controller.Sources().Contains(sourceData))
			{
				controlButton.Image = Stareater.Properties.Resources.start;
				starName.Text = sourceData.HostStar.Name.ToText(SettingsWinforms.Get.Language);
			}
			else
			{
				controlButton.Image = Stareater.Properties.Resources.stop;
				starName.Text = context["stoppedColonization"].Text(
					new TextVar("star", sourceData.HostStar.Name.ToText(SettingsWinforms.Get.Language)).Get
				);
			}
		}
		
		private void controlButton_Click(object sender, EventArgs e)
		{
			if (controller.IsColonizing)
				controller.StopColonization(sourceData);
			else
				controller.StartColonization(sourceData);
			
			updateView();
			
			if (OnStateChange != null)
				OnStateChange();
		}
	}
}
