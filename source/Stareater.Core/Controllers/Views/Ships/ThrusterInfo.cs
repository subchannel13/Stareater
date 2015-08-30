using System;
using System.Collections.Generic;
using Stareater.AppData;
using Stareater.GameData.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class ThrusterInfo
	{
		private const string LangContext = "Thrusters";

		internal ThrusterType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> vars;

		internal ThrusterInfo(ThrusterType type, int level)
		{
			this.Type = type;
			this.Level = level;
			
			this.vars = new Var(AComponentType.LevelKey, level).Get;
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
		
		public double Evasion
		{
			get
			{
				return this.Type.Evasion.Evaluate(vars);
			}
		}
		
		public double Speed
		{
			get
			{
				return this.Type.Speed.Evaluate(vars);
			}
		}
	}
}
