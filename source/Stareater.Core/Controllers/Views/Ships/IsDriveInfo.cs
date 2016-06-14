using System;
using System.Collections.Generic;
using Stareater.AppData;
using Stareater.GameData.Ships;

namespace Stareater.Controllers.Views.Ships
{
	public class IsDriveInfo
	{
		private const string LangContext = "IsDrives";
		
		internal IsDriveType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> vars;
		
		internal IsDriveInfo(IsDriveType isDriveType, int level, IDictionary<string, double> shipVars)
		{
			this.Type = isDriveType;
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
		
		public double Speed
		{
			get
			{
				return this.Type.Speed.Evaluate(vars);
			}
		}
	}
}
