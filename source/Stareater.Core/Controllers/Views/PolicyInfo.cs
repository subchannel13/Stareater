using Stareater.GameData;
using Stareater.Localization;
using System.Collections.Generic;

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

		public override bool Equals(object obj)
		{
			var other = obj as PolicyInfo;
			return other != null &&
				   EqualityComparer<SystemPolicy>.Default.Equals(this.Data, other.Data);
		}

		public override int GetHashCode()
		{
			return -301143667 + EqualityComparer<SystemPolicy>.Default.GetHashCode(this.Data);
		}
	}
}
