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
		
		private HullType hullType;
		private int level;
		
		private IDictionary<string, double> levelVar;
		
		internal HullInfo(HullType hullType, int level)
		{
			this.hullType = hullType;
			this.level = level;
			
			this.levelVar = new Var(AComponentType.LevelKey, level).Get;
		}
		
		public string Name 
		{ 
			get
			{
				return Settings.Get.Language[LangContext][hullType.NameCode].Text();
			}
		}
		
		public double Size
		{
			get
			{
				return hullType.Size.Evaluate(levelVar);
			}
		}
		
		public string[] ImagePaths
		{
			get
			{
				return hullType.ImagePaths;
			}
		}
	}
}
