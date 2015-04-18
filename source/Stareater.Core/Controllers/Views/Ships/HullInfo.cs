using System;
using System.Collections.Generic;
using Stareater.AppData;
using Stareater.GameData.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class HullInfo
	{
		private const string LangContext = "Hulls";
		
		internal HullType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> levelVar;
		
		internal HullInfo(HullType hullType, int level)
		{
			this.Type = hullType;
			this.Level = level;
			
			this.levelVar = new Var(AComponentType.LevelKey, level).Get;
		}
		
		public string Name 
		{ 
			get
			{
				return Settings.Get.Language[LangContext][this.Type.NameCode].Text();
			}
		}
		
		public double Size
		{
			get
			{
				return this.Type.Size.Evaluate(levelVar);
			}
		}
		
		public string[] ImagePaths
		{
			get
			{
				return this.Type.ImagePaths;
			}
		}
		
		public double IsDriveSize
		{
			get
			{
				return this.Type.SizeIS.Evaluate(levelVar);
			}
		}

		public double ReactorSize
		{
			get
			{
				return this.Type.SizeReactor.Evaluate(levelVar);
			}
		}
		
		//TODO(v0.5) add other hull properties
	}
}
