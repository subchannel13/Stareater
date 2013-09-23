using System;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.GameLogic
{
	class ColonyProcessor
	{
		private const string PlanetSize = "size";
		
		public Colony Colony { get; set; }
		
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
		
		#if !DEBUG
		public double MaxPopulation { get; private set; }
		
		public double Development { get; private set; }
		public double Production { get; private set; }
		#else
		
		private double maxPopulation;
		public double MaxPopulation 
		{
			get { 
				checkDirty();
				return maxPopulation;
			}
			private set {
				maxPopulation = value;
			}
		}
		
		private double development;
		public double Development 
		{
			get { 
				checkDirty();
				return development;
			}
			private set {
				development = value;
			}
		}
		
		private double production;
		public double Production 
		{
			get { 
				checkDirty();
				return production;
			}
			private set {
				production = value;
			}
		}
		
		private void checkDirty()
		{
			if (Colony.Dirty)
				throw new InvalidOperationException("Colony derived stats are dirty");
		}
		#endif
		
		public void Calculate(ColonyFormulaSet formulas, PlayerProcessor playerProcessor)
		{
			//TODO: add player's techs and colony buildings
			var vars = new Var(PlanetSize, Colony.Location.Size).UnionWith(playerProcessor.TechLevels).Get;
			var organizationRatio = Methods.Clamp(Colony.Infrastructure / Colony.Population, 0, 1);
			
			this.MaxPopulation = formulas.MaxPopulation.Evaluate(vars);
			//TODO: include farming and mining
			this.Development = (1 - Colony.Owner.Orders.SiteSpendingRatios[Colony]) * Colony.Population * formulas.Development.Evaluate(organizationRatio, vars);
			this.Production = Colony.Owner.Orders.SiteSpendingRatios[Colony] * Colony.Population * formulas.Industry.Evaluate(organizationRatio, vars);
			
			Colony.Cleaned();
		}
	}
}
