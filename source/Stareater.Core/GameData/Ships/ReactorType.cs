using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.AppData.Expressions;
using Stareater.GameData.Databases;
using Stareater.GameLogic;
using Stareater.Ships;
using Stareater.Utils;

namespace Stareater.GameData.Ships
{
	class ReactorType : AComponentType
	{
		public const string TotalPowerKey = "totalPower";
		
		public string ImagePath { get; private set; }
		
		public Formula Power { get; private set; }
		public Formula MinSize { get; private set; }
		
		public ReactorType(string code, string nameCode, string descCode, string imagePath, 
		                   IEnumerable<Prerequisite> prerequisites, int maxLevel, 
		                   Formula power, Formula minSize)
			: base(code, nameCode, descCode, prerequisites, maxLevel)
		{
			this.ImagePath = imagePath;
			this.Power = power;
			this.MinSize = minSize;
			
		}

		public static Component<ReactorType> MakeBest(Dictionary<string, double> playersTechLevels, Component<HullType> hull, IEnumerable<Component<SpecialEquipmentType>> specialEquipment, StaticsDB statics)
		{
			var shipVars = PlayerProcessor.DesignBaseVars(hull, specialEquipment, statics).Get;

			return Methods.FindBest(
				statics.Reactors.Values.Where(x => x.IsAvailable(playersTechLevels)).
				Select(x => new Component<ReactorType>(x, x.HighestLevel(playersTechLevels))).
				Where(x =>
				      {
				      	shipVars[AComponentType.LevelKey] = x.Level;
				      	return x.TypeInfo.MinSize.Evaluate(shipVars) <= shipVars[HullType.ReactorSizeKey]; //TODO use final reactor size
				      }),
				x =>
				{
					shipVars[AComponentType.LevelKey] = x.Level;
					return x.TypeInfo.Power.Evaluate(shipVars);
				}
			);
		}

		public static double PowerOf(Component<ReactorType> reactor, Component<HullType> hull, IEnumerable<Component<SpecialEquipmentType>> specialEquipment, StaticsDB statics)
		{
			var shipVars = PlayerProcessor.DesignBaseVars(hull, specialEquipment, statics).Get;
			shipVars[AComponentType.LevelKey] = reactor.Level;

			return reactor.TypeInfo.Power.Evaluate(shipVars);
		}
	}
}
