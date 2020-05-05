using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Ships;

namespace Stareater.Controllers.Views.Ships
{
	public class IsDriveInfo
	{
		internal const string LangContext = "IsDrives";
		
		internal Component<IsDriveType> Component { get; private set; }

		public double Speed { get; private set; }

		internal IsDriveInfo(Component<IsDriveType> component, double speed)
		{
			this.Component = component;
			this.Speed = speed;
		}

		public string Name
		{ 
			get
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext].Name(this.Component.TypeInfo.LanguageCode).Text(this.Component.Level);
			}
		}
		
		public string ImagePath
		{
			get
			{
				return this.Component.TypeInfo.ImagePath;
			}
		}
	}
}
