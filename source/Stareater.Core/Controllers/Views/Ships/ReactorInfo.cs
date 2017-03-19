using System;
using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Localization;

namespace Stareater.Controllers.Views.Ships
{
	public class ReactorInfo
	{
		internal const string LangContext = "Reactors";
		
		internal ReactorType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> vars;
		
		internal ReactorInfo(ReactorType type, int level, IDictionary<string, double> shipVars)
		{
			this.Type = type;
			this.Level = level;
			
			this.vars = new Dictionary<string, double>(shipVars);
			this.vars[AComponentType.LevelKey] = level;
		}
		
		public string Name
		{ 
			get
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext].Name(this.Type.LanguageCode).Text(this.Level);
			}
		}
		
		public string ImagePath
		{
			get
			{
				return this.Type.ImagePath;
			}
		}
		
		public double Power
		{
			get
			{
				return this.Type.Power.Evaluate(vars);
			}
		}
	}
}
