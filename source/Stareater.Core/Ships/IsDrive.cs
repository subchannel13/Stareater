using System;
using System.Collections.Generic;
using System.Linq;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.Utils.Collections;

namespace Stareater.Ships
{
	class IsDrive
	{
		public IsDriveType TypeInfo { get; private set; }
		public int Level { get; private set; }
		
		public IsDrive(IsDriveType typeInfo, int level)
		{
			this.TypeInfo = typeInfo;
			this.Level = level;
		}
		
		public static IsDrive Best(IEnumerable<IsDriveType> drives, Dictionary<string, int> playersTechLevels, Hull shipHull, double shipPower)
		{
			var hullVars = new Var("level", shipHull.Level).Get;
			IsDrive bestDrive = null;
			
			foreach(var drive in drives.Where(x => x.IsAvailable(playersTechLevels))) {
				int driveLevel = drive.HighestLevel(playersTechLevels);
				double driveSize = shipHull.TypeInfo.SizeIS.Evaluate(hullVars);

				var driveVars = new Var("level", driveLevel).
					And("size", driveSize).
					And("power", shipPower).Get; 
				
				if (drive.MinSize.Evaluate(driveVars) <= driveSize &&
				    (bestDrive == null || drive.Speed.Evaluate(driveVars) > bestDrive.TypeInfo.Speed.Evaluate(driveVars)))
						bestDrive = new IsDrive(drive, driveLevel);
			}
			
			return bestDrive;
		}
		
		public IkadnBaseObject Save()
		{
			var data = new IkonComposite(IsDriveTag);
			
			data.Add(TypeKey, new IkonText(this.TypeInfo.IdCode));
			data.Add(LevelKey, new IkonInteger(this.Level));
			
			return data;
		}
		
		public static IsDrive Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			return new IsDrive(
				deindexer.Get<IsDriveType>(rawData[TypeKey].To<string>()),
				rawData[LevelKey].To<int>()
			);
		}
		
		#region Saving keys
		private const string IsDriveTag = "IsDrive"; 
		private const string TypeKey = "type";
		private const string LevelKey = "level";
 		#endregion
	}
}
