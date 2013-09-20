using System;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.GameLogic
{
	class ColonyProcessor
	{
		private const string PlanetSize = "size";
		
		public Colony Colony { get; set; }
		
		public double MaxPopulation { get; private set; }
		
		public ColonyProcessor(Colony colony)
		{
			this.Colony = colony;
		}
		
		public Player Owner 
		{ 
			get {
				return Colony.Owner;
			}
		}
		
		public void Calculate(ColonyFormulaSet formulas)
		{
			//TODO: add player's techs and colony buildings
			var vars = new Var(PlanetSize, Colony.Location.Size).Get;
			
			this.MaxPopulation = formulas.MaxPopulation.Evaluate(vars);
		}
	}
}
