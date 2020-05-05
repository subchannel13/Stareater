using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class ArmorInfo
	{
		internal const string LangContext = "Armors";
		
		internal Component<ArmorType> Component { get; private set; }
		
		private readonly IDictionary<string, double> vars;
		
		internal ArmorInfo(Component<ArmorType> component)
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
		
		public double Absorption
		{
			get
			{
				return this.Component.TypeInfo.Absorption.Evaluate(vars);
			}
		}
		
		public double AbsorptionMax
		{
			get
			{
				return this.Component.TypeInfo.AbsorptionMax.Evaluate(vars);
			}
		}
		
		public double ArmorFactor
		{
			get
			{
				return this.Component.TypeInfo.ArmorFactor.Evaluate(vars);
			}
		}
	}
}
