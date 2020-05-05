using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class SensorInfo
	{
		internal const string LangContext = "Sensors";

		internal Component<SensorType> Component { get; private set; }
		
		private readonly IDictionary<string, double> vars;

		internal SensorInfo(Component<SensorType> component)
		{
			this.Component = component;
			this.vars = new Var(AComponentType.LevelKey, component.Level).Get;
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
		
		public double Detection
		{
			get
			{
				return this.Component.TypeInfo.Detection.Evaluate(vars);
			}
		}
	}
}
