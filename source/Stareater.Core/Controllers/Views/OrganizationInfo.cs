using Stareater.Localization;
using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public class OrganizationInfo
	{
		internal const string LangContext = "Organizations";

		internal Organization Data { get; private set; }

		internal OrganizationInfo(Organization data)
		{
			this.Data = data;
		}

		public string Name
		{
			get
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext].Name(this.Data.LanguageCode).Text();
			}
		}

		public string Description
		{
			get
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext].Description(this.Data.LanguageCode).Text();
			}
		}
	}
}
