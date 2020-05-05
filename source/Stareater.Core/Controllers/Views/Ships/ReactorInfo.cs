using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Ships;

namespace Stareater.Controllers.Views.Ships
{
	public class ReactorInfo
	{
		internal const string LangContext = "Reactors";
		
		internal Component<ReactorType> Component { get; private set; }

		public double Power { get; private set; }

		internal ReactorInfo(Component<ReactorType> component, double power)
		{
			this.Component = component;
			this.Power = power;
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
