using System;
using System.Collections.Generic;
using Stareater.GameData.Databases.Tables;
using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class ShieldInfo
	{
		internal const string LangContext = "Shields";
		
		internal ShieldType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> vars;
		
		internal ShieldInfo(ShieldType type, int level, double shieldSize)
		{
			this.Type = type;
			this.Level = level;
			
			this.vars = new Var(AComponentType.LevelKey, level).
				And(ShieldType.SizeKey, shieldSize).Get;
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
		
		public double HpFactor 
		{
			get
			{
				return this.Type.HpFactor.Evaluate(vars);
			}
		}
		
		public double RegenerationFactor 
		{
			get
			{
				return this.Type.RegenerationFactor.Evaluate(vars);
			}
		}
		
		public double Thickness 
		{
			get
			{
				return this.Type.Thickness.Evaluate(vars);
			}
		}
		
		public double Reduction 
		{
			get
			{
				return this.Type.Reduction.Evaluate(vars);
			}
		}
		
		
		public double Cloaking 
		{
			get
			{
				return this.Type.Cloaking.Evaluate(vars);
			}
		}
		
		public double Jamming 
		{
			get
			{
				return this.Type.Jamming.Evaluate(vars);
			}
		}
		
		
		public double PowerUsage 
		{
			get
			{
				return this.Type.PowerUsage.Evaluate(vars);
			}
		}
	}
}
