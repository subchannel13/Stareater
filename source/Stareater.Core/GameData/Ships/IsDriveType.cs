using System;
using System.Linq;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Ships
{
	class IsDriveType : AComponentType
	{
		public Formula Cost { get; private set; }
		public string ImagePath { get; private set; }
		
		public Formula Speed { get; private set; }
		public Formula MinSize { get; private set; }
		
		public IsDriveType(string code, string nameCode, string descCode, string imagePath,
		                   IEnumerable<Prerequisite> prerequisites, int maxLevel, Formula cost, 
		                   Formula speed, Formula minSize)
			: base(code, nameCode, descCode, prerequisites, maxLevel)
		{
			this.Cost = cost;
			this.ImagePath = imagePath;
			this.Speed = speed;
			this.MinSize = minSize;
		}
		
		public static Component<IsDriveType> MakeBest(IEnumerable<IsDriveType> drives, Dictionary<string, int> playersTechLevels, Component<HullType> shipHull, double shipPower)
		{
			Component<IsDriveType> bestComponent = null;
			var hullVars = new Var("level", shipHull.Level).Get;	//TODO(v0.5) make constants for variable names
			
			double driveSize = shipHull.TypeInfo.SizeIS.Evaluate(hullVars);
			var driveVars = new Var("level", 0).
					And("size", driveSize).
					And("power", shipPower).Get;
			
			foreach(var drive in drives.Where(x => x.IsAvailable(playersTechLevels))) {
				int driveLevel = drive.HighestLevel(playersTechLevels);
				driveVars["level"] = driveLevel;
				
				if (drive.MinSize.Evaluate(driveVars) <= driveSize &&
				    (bestComponent == null || drive.Speed.Evaluate(driveVars) > bestComponent.TypeInfo.Speed.Evaluate(driveVars)))
						bestComponent = new Component<IsDriveType>(drive, driveLevel);
			}
			
			return bestComponent;
		}
	}
}
