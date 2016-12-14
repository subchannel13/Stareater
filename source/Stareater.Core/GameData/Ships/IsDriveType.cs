using System;
using System.Linq;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.GameData.Databases;
using Stareater.GameLogic;
using Stareater.Ships;
using Stareater.Utils;

namespace Stareater.GameData.Ships
{
	class IsDriveType : AComponentType
	{
		public Formula Cost { get; private set; }
		public string ImagePath { get; private set; }
		
		public Formula Speed { get; private set; }
		public Formula MinSize { get; private set; }
		
		public IsDriveType(string code, string nameCode, string descCode, bool isVirtual, string imagePath,
		                   IEnumerable<Prerequisite> prerequisites, int maxLevel, Formula cost, 
		                   Formula speed, Formula minSize)
			: base(code, nameCode, descCode, isVirtual, prerequisites, maxLevel)
		{
			this.Cost = cost;
			this.ImagePath = imagePath;
			this.Speed = speed;
			this.MinSize = minSize;
		}
		
		public static Component<IsDriveType> MakeBest(IDictionary<string, double> playersTechLevels, Component<HullType> hull, Component<ReactorType> reactor, IEnumerable<Component<SpecialEquipmentType>> specialEquipment, StaticsDB statics)
		{
			var shipVars = PlayerProcessor.DesignPoweredVars(hull, reactor, specialEquipment, statics).Get;
			
			return Methods.FindBest(
				statics.IsDrives.Values.Where(x => x.IsAvailable(playersTechLevels)).
				Select(x => new Component<IsDriveType>(x, x.HighestLevel(playersTechLevels))).
				Where(x =>
				      {
				      	shipVars[AComponentType.LevelKey] = x.Level;
				      	return x.TypeInfo.MinSize.Evaluate(shipVars) <= shipVars[HullType.IsDriveSizeKey];
				      }),
				x =>
				{
					shipVars[AComponentType.LevelKey] = x.Level;
					return x.TypeInfo.Speed.Evaluate(shipVars);
				}
			);
		}
	}
}
