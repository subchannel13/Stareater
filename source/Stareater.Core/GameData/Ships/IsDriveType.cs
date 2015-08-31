using System;
using System.Linq;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.Utils;
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
			var hullVars = new Var(AComponentType.LevelKey, shipHull.Level).Get;
			
			double driveSize = shipHull.TypeInfo.SizeIS.Evaluate(hullVars);
			var driveVars = new Var(AComponentType.LevelKey, 0).
					And("size", driveSize).
					And("power", shipPower).Get;
			
			return Methods.FindBest(
				drives.Where(x => x.IsAvailable(playersTechLevels)).
				Select(x => new Component<IsDriveType>(x, x.HighestLevel(playersTechLevels))).
				Where(x =>
				      {
				      	driveVars[AComponentType.LevelKey] = x.Level;
				      	return x.TypeInfo.MinSize.Evaluate(driveVars) <= driveSize;
				      }),
				x =>
				{
					driveVars[AComponentType.LevelKey] = x.Level;
					return x.TypeInfo.Speed.Evaluate(driveVars);
				}
			);
		}
	}
}
