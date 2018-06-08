using Stareater.GameData;
using Stareater.Localization;

namespace Stareater.Controllers.Views
{
	public class PolicyInfo
	{
		internal SystemPolicy Data { get; private set; }

		internal PolicyInfo(SystemPolicy data)
		{
			this.Data = data;
		}

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
