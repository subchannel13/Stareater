using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Ships
{
	class ReactorType : AComponentType
	{
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

		public static Component<ReactorType> MakeBest(IEnumerable<ReactorType> reactors, Dictionary<string, int> playersTechLevels, Component<HullType> shipHull)
		{
			var hullVars = new Var(AComponentType.LevelKey, shipHull.Level).Get;

			double reactorSize = shipHull.TypeInfo.SizeReactor.Evaluate(hullVars);
			var reactorVars = new Var(AComponentType.LevelKey, 0).
					And(AComponentType.SizeKey, reactorSize).Get;

			return Methods.FindBest(
				reactors.Where(x => x.IsAvailable(playersTechLevels)).
				Select(x => new Component<ReactorType>(x, x.HighestLevel(playersTechLevels))).
				Where(x =>
				      {
				      	reactorVars[AComponentType.LevelKey] = x.Level;
				      	return x.TypeInfo.MinSize.Evaluate(reactorVars) <= reactorSize;
				      }),
				x =>
				{
					reactorVars[AComponentType.LevelKey] = x.Level;
					return x.TypeInfo.Power.Evaluate(reactorVars);
				}
			);
		}

		public static double PowerOf(Component<ReactorType> reactor, Component<HullType> shipHull)
		{
			var hullVars = new Var("level", shipHull.Level).Get;	//TODO(v0.5) make constants for variable names

			double reactorSize = shipHull.TypeInfo.SizeReactor.Evaluate(hullVars);
			var reactorVars = new Var("level", reactor.Level).
					And(AComponentType.SizeKey, reactorSize).Get;

			return reactor.TypeInfo.Power.Evaluate(reactorVars);
		}
	}
}
