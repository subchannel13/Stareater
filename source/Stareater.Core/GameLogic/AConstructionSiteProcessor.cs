using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.Utils.Collections;

namespace Stareater.GameLogic
{
	abstract class AConstructionSiteProcessor
	{
		private const string BuidingCountPrefix = "_count";
		
		public double Production { get; protected set; }
		public double SpendingRatioEffective { get; protected set; }
		public IEnumerable<ConstructionResult> SpendingPlan { get; protected set; }

		protected AConstructionSiteProcessor()
		{
			this.SpendingPlan = new ConstructionResult[0];
		}
		
		protected AConstructionSiteProcessor(AConstructionSiteProcessor original)
		{
			this.Production = original.Production;
			this.SpendingPlan = new List<ConstructionResult>(original.SpendingPlan);
			this.SpendingRatioEffective = original.SpendingRatioEffective;
		}
		
		public virtual Var LocalEffects(StaticsDB statics)
		{
			var vars = new Var();
			
			foreach(var building in statics.Buildings)
				if (Site.Buildings.ContainsKey(building.Key))
					vars.And(building.Key.ToLower() + BuidingCountPrefix, Site.Buildings[building.Key]);
				else
					vars.And(building.Key.ToLower() + BuidingCountPrefix, 0);
			
			return vars;
		}

		public virtual void ProcessPrecombat(StatesDB states)
		{
			foreach (var construction in this.SpendingPlan) {
				if (construction.CompletedCount >= 1)
					foreach(var effect in construction.Type.Effects)
						effect.Apply(states, this.Site, construction.CompletedCount);
				
				if (construction.LeftoverPoints > 0)
					if (!this.Site.Stockpile.ContainsKey(construction.Type))
						this.Site.Stockpile.Add(construction.Type, construction.LeftoverPoints);
					else
						this.Site.Stockpile[construction.Type] += construction.LeftoverPoints;
			}
		}
		
		protected abstract AConstructionSite Site { get; }
		
		protected static IEnumerable<ConstructionResult> SimulateSpending(
			AConstructionSite site, double industryPoints, 
			IEnumerable<Constructable> queue, IDictionary<string, double> vars)
		{
			var spendingPlan = new List<ConstructionResult>();
			var planStockpile = new Dictionary<Constructable, double>(site.Stockpile);

			foreach (var buildingItem in queue) {
				if (buildingItem.Condition.Evaluate(vars) < 0) {
					spendingPlan.Add(new ConstructionResult(0, 0, site.Stockpile[buildingItem], buildingItem, site.Stockpile[buildingItem]));
					continue;
				}
				
				double cost = buildingItem.Cost.Evaluate(vars);
				double stockpile = planStockpile.ContainsKey(buildingItem) ? planStockpile[buildingItem] : 0;
				double totalInvestment = industryPoints + stockpile;

				double completed = Math.Floor(totalInvestment / cost); //FIXME(v0.5): possible division by zero
				double countLimit = buildingItem.TurnLimit.Evaluate(vars);

				if (completed > countLimit) {
					double totalCost = countLimit * cost;
					
					if (stockpile >= totalCost) {
						spendingPlan.Add(new ConstructionResult(
							(long)countLimit,
							0, 
							totalCost,
							buildingItem,
							0
						));
						planStockpile[buildingItem] = stockpile - totalCost;
					}
					else {
						spendingPlan.Add(new ConstructionResult(
							(long)countLimit,
							totalCost - stockpile,
							stockpile,
							buildingItem,
							0
						));
						planStockpile[buildingItem] = 0;
						industryPoints -= totalCost - stockpile;
					}
				}
				else {
					spendingPlan.Add(new ConstructionResult(
						(long)completed,
						industryPoints,
						stockpile,
						buildingItem,
						totalInvestment - completed * cost
					));

					industryPoints = 0;
				}
			}

			return spendingPlan;
		}
	}
}
