using Stareater.Localization;

namespace Stareater.Controllers.Views
{
	public class PolicyInfo
	{
		public string Name
		{
			//TODO(v0.8) unstub
			get { return LocalizationManifest.Get.CurrentLanguage["SystemPolicies"]["develop"].Text(); }
		}

		public string Id
		{
			//TODO(v0.8) unstub
			get { return "develop"; }
		}
	}
}
