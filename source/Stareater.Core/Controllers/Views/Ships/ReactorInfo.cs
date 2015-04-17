using System;
using System.Collections.Generic;
using Stareater.AppData;
using Stareater.GameData.Ships;

namespace Stareater.Controllers.Views.Ships
{
	public class ReactorInfo
	{
		private const string LangContext = "Reactors";
		
		internal ReactorType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> vars;
		
		internal ReactorInfo(ReactorType type, int level, HullInfo shipHull, double shipPower)
		{
			this.Type = type;
			this.Level = level;
			
			//TODO(v0.5)
			/*
			this.driveVars = new Var(AComponentType.LevelKey, level).
				And("size", shipHull.siz).Get;*/ 
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
