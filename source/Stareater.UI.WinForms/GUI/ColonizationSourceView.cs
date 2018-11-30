using System;
using System.Windows.Forms;
using Stareater.Controllers.Views;
using Stareater.Localization;

namespace Stareater.GUI
{
	public partial class ColonizationSourceView : UserControl
	{
		private StellarisInfo sourceData = null;
		private Action<StellarisInfo> cancelCallback = null;
		
		public ColonizationSourceView()
		{
			InitializeComponent();
		}

		public ColonizationSourceView(Action<StellarisInfo> cancelCallback) : this()
		{
			this.cancelCallback = cancelCallback;
		}

		public StellarisInfo Data
		{
			get { return this.sourceData; }
			set
			{
				this.sourceData = value;
				this.starName.Text = this.sourceData.HostStar.Name.ToText(LocalizationManifest.Get.CurrentLanguage);
				this.Refresh();
			}
		}
		
		private void controlButton_Click(object sender, EventArgs e)
		{
			this.cancelCallback(this.sourceData);
		}
	}
}
