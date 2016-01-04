using System;
using System.Collections.Generic;
using Stareater.AppData;
using Stareater.GameData.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class ShieldInfo
	{
		private const string LangContext = "Shields";
		
		internal ShieldType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> vars;
		
		internal ShieldInfo(ShieldType type, int level, HullInfo shipHull)
		{
			this.Type = type;
			this.Level = level;
			
			this.vars = new Var(AComponentType.LevelKey, level).
				And(HullType.HullSizeKey, shipHull.Size).Get;
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
