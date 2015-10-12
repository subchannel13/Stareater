using System;
using System.Collections.Generic;
using Stareater.AppData;
using Stareater.GameData.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class IsDriveInfo
	{
		private const string LangContext = "IsDrives";
		
		internal IsDriveType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> driveVars;
		
		internal IsDriveInfo(IsDriveType isDriveType, int level, HullInfo shipHull, double shipPower)
		{
			this.Type = isDriveType;
			this.Level = level;
			
			this.driveVars = new Var(AComponentType.LevelKey, level).
				And(AComponentType.SizeKey, shipHull.IsDriveSize).
				And("power", shipPower).Get; 
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
				return this.Type.Speed.Evaluate(driveVars);
			}
		}
	}
}
