using System;
using System.Collections.Generic;
using Stareater.AppData;
using Stareater.GameData.Ships;

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
				return Settings.Get.Language[LangContext][this.Type.NameCode].Text(this.Level);
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
