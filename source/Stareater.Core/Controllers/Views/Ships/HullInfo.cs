using System;
using System.Collections.Generic;
using Stareater.AppData;
using Stareater.GameData;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class HullInfo
	{
		private const string LangContext = "Hulls";
		
		internal HullType HullType { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> levelVar;
		
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
				return Settings.Get.Language[LangContext][this.HullType.NameCode].Text();
			}
		}
		
		public double Size
		{
			get
			{
				return this.HullType.Size.Evaluate(levelVar);
			}
		}
		
		public string[] ImagePaths
		{
			get
			{
				return this.HullType.ImagePaths;
			}
		}
		
		public double IsDriveSize
		{
			get
			{
				return this.HullType.SizeIS.Evaluate(levelVar);
			}
		}
		
		//TODO(v0.5) add other hull properties
	}
}
