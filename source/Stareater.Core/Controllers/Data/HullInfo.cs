using System;
using System.Collections.Generic;
using Stareater.AppData;
using Stareater.GameData;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Data
{
	public class HullInfo
	{
		private const string LangContext = "Hulls";
		
		internal HullType HullType { get; private set; }
		internal int Level { get; private set; }
		
		private IDictionary<string, double> levelVar;
		
		internal HullInfo(HullType hullType, int level)
		{
			this.HullType = hullType;
			this.Level = level;
			
			this.levelVar = new Var(AComponentType.LevelKey, level).Get;
		}
		
		public string Name 
		{ 
			get
			{
				return Settings.Get.Language[LangContext][HullType.NameCode].Text();
			}
		}
		
		public double Size
		{
			get
			{
				return HullType.Size.Evaluate(levelVar);
			}
		}
		
		public string[] ImagePaths
		{
			get
			{
				return HullType.ImagePaths;
			}
		}
	}
}
