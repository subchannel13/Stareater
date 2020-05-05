using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class ThrusterInfo
	{
		internal const string LangContext = "Thrusters";

		internal Component<ThrusterType> Component { get; private set; }
		
		private readonly IDictionary<string, double> vars;

		internal ThrusterInfo(Component<ThrusterType> component)
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
		
		public double Evasion
		{
			get
			{
				return this.Component.TypeInfo.Evasion.Evaluate(vars);
			}
		}
		
		public double Speed
		{
			get
			{
				return this.Component.TypeInfo.Speed.Evaluate(vars);
			}
		}
	}
}
