using System.Collections.Generic;
using System.Linq;
using Stareater.AppData.Expressions;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.GameLogic;
using Stareater.Ships;
using Stareater.Utils;
using Stareater.Utils.StateEngine;

namespace Stareater.GameData.Ships
{
	[StateTypeAttribute(true)]
	class ReactorType : AComponentType
	{
		public const string SizeKey = "reactorSize";
		public const string TotalPowerKey = "totalPower";
		
		public string ImagePath { get; private set; }
		
		public Formula Power { get; private set; }
		public Formula MinSize { get; private set; }
		
		public ReactorType(string code, string languageCode, string imagePath, 
		                   IEnumerable<Prerequisite> prerequisites, int maxLevel, bool canPick,
		                   Formula power, Formula minSize)
			: base(code, languageCode, prerequisites, maxLevel, canPick)
		{
			this.ImagePath = imagePath;
			this.Power = power;
			this.MinSize = minSize;
			
		}

		public static Component<ReactorType> MakeBest(IDictionary<string, double> playersTechLevels, Component<HullType> hull, IEnumerable<Component<SpecialEquipmentType>> specialEquipment, IEnumerable<Component<MissionEquipmentType>> missionEquipment, StaticsDB statics)
		{
			var shipVars = PlayerProcessor.DesignBaseVars(hull, missionEquipment, specialEquipment, statics).Get;
			var reactorSize = statics.ShipFormulas.ReactorSize.Evaluate(shipVars);
			shipVars[SizeKey] = reactorSize;

			return Methods.FindBestOrDefault(
				statics.Reactors.Values.Where(x => x.IsAvailable(playersTechLevels)).
				Select(x => new Component<ReactorType>(x, x.HighestLevel(playersTechLevels))).
				Where(x =>
				      {
						  shipVars[LevelKey] = x.Level;
						  return x.TypeInfo.MinSize.Evaluate(shipVars) <= reactorSize && x.TypeInfo.CanPick; 
				      }),
				x =>
				{
					shipVars[LevelKey] = x.Level;
					return x.TypeInfo.Power.Evaluate(shipVars);
				}
			);
		}

		public static double PowerOf(Component<ReactorType> reactor, Component<HullType> hull, IEnumerable<Component<SpecialEquipmentType>> specialEquipment, IEnumerable<Component<MissionEquipmentType>> missionEquipment, StaticsDB statics)
		{
			var shipVars = PlayerProcessor.DesignBaseVars(hull, missionEquipment, specialEquipment, statics).Get;
			shipVars[LevelKey] = reactor.Level;

			return reactor.TypeInfo.Power.Evaluate(shipVars);
		}
	}
}
