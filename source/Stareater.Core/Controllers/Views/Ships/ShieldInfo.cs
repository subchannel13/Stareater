using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class ShieldInfo
	{
		internal const string LangContext = "Shields";
		
		internal Component<ShieldType> Component { get; private set; }
		
		private readonly IDictionary<string, double> vars;
		
		internal ShieldInfo(Component<ShieldType> component, double shieldSize)
		{
			this.Component = component;
			
			this.vars = new Var(AComponentType.LevelKey, component.Level).
				And(ShieldType.SizeKey, shieldSize).Get;
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
		
		public double HpFactor 
		{
			get
			{
				return this.Component.TypeInfo.HpFactor.Evaluate(vars);
			}
		}
		
		public double RegenerationFactor 
		{
			get
			{
				return this.Component.TypeInfo.RegenerationFactor.Evaluate(vars);
			}
		}
		
		public double Thickness 
		{
			get
			{
				return this.Component.TypeInfo.Thickness.Evaluate(vars);
			}
		}
		
		public double Reduction 
		{
			get
			{
				return this.Component.TypeInfo.Reduction.Evaluate(vars);
			}
		}
		
		
		public double Cloaking 
		{
			get
			{
				return this.Component.TypeInfo.Cloaking.Evaluate(vars);
			}
		}
		
		public double Jamming 
		{
			get
			{
				return this.Component.TypeInfo.Jamming.Evaluate(vars);
			}
		}
		
		
		public double PowerUsage 
		{
			get
			{
				return this.Component.TypeInfo.PowerUsage.Evaluate(vars);
			}
		}
	}
}
