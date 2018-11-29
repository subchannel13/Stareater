using System;
using System.Windows.Forms;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Localization;

namespace Stareater.GUI
{
	public partial class ColonizationSourceView : UserControl
	{
		private StellarisInfo sourceData = null;
		private readonly ColonizationController controller;
		
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
			var context = LocalizationManifest.Get.CurrentLanguage["FormColonization"];
			
			this.starName.Text = this.sourceData.HostStar.Name.ToText(LocalizationManifest.Get.CurrentLanguage);
		}
		
		private void controlButton_Click(object sender, EventArgs e)
		{
			if (this.controller.IsColonizing)
				this.controller.StopColonization(this.sourceData);
			else
				this.controller.StartColonization(this.sourceData);
			this.controller.RunAutomation();

			this.updateView();
			
			if (OnStateChange != null)
				OnStateChange();
		}
	}
}
