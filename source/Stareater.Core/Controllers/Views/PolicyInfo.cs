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
			get { return LocalizationManifest.Get.CurrentLanguage["SystemPolicies"][this.Data.LangCode].Text(); }
		}

		public string Id
		{
			get { return this.Data.Id; }
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
