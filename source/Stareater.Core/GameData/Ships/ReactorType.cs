using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.AppData.Expressions;
using Stareater.Ships;
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
			Component<ReactorType> bestComponent = null;
			var hullVars = new Var("level", shipHull.Level).Get;	//TODO(v0.5) make constants for variable names

			double reactorSize = shipHull.TypeInfo.SizeReactor.Evaluate(hullVars);
			var reactorVars = new Var("level", 0).
					And("size", reactorSize).Get;

			foreach (var reactor in reactors.Where(x => x.IsAvailable(playersTechLevels))) {
				int level = reactor.HighestLevel(playersTechLevels);
				reactorVars["level"] = level;

				if (reactor.MinSize.Evaluate(reactorVars) <= reactorSize &&
					(bestComponent == null || reactor.Power.Evaluate(reactorVars) > bestComponent.TypeInfo.Power.Evaluate(reactorVars)))
					bestComponent = new Component<ReactorType>(reactor, level);
			}

			return bestComponent;
		}

		public static double PowerOf(Component<ReactorType> reactor, Component<HullType> shipHull)
		{
			var hullVars = new Var("level", shipHull.Level).Get;	//TODO(v0.5) make constants for variable names

			double reactorSize = shipHull.TypeInfo.SizeReactor.Evaluate(hullVars);
			var reactorVars = new Var("level", reactor.Level).
					And("size", reactorSize).Get;

			return reactor.TypeInfo.Power.Evaluate(reactorVars);
		}
	}
}
