using System;
using System.Collections.Generic;

using Stareater.Galaxy;
using Stareater.GameData.Databases;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using Stareater.GameData.Construction;
using Stareater.GameLogic.Planning;

namespace Stareater.GameLogic
{
	abstract class AConstructionSiteProcessor
	{
		private const string BuidingCountPrefix = "_count";

		[StateProperty]
		public double Production { get; protected set; }
		[StateProperty]
		public double SpendingRatioEffective { get; protected set; }
		[StateProperty]
		public IEnumerable<ConstructionResult> SpendingPlan { get; protected set; }

		protected AConstructionSiteProcessor()
		{
			this.SpendingPlan = new ConstructionResult[0];
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

		public virtual void ProcessPrecombat(MainGame game)
		{
			foreach (var construction in this.SpendingPlan) {
				if (construction.CompletedCount >= 1)
					foreach(var effect in construction.Project.Effects)
						effect.Apply(game, this.Site, construction.CompletedCount);
				
				var stockpileKey = construction.Project.StockpileGroup;
				if (construction.FromStockpile > 0 && this.Site.Stockpile.ContainsKey(stockpileKey))
					this.Site.Stockpile[stockpileKey] -= construction.FromStockpile;
				
				if (construction.LeftoverPoints > 0)
					if (!this.Site.Stockpile.ContainsKey(stockpileKey))
						this.Site.Stockpile.Add(stockpileKey, construction.LeftoverPoints);
					else
						this.Site.Stockpile[stockpileKey] += construction.LeftoverPoints;
				
				if (this.Site.Stockpile.ContainsKey(stockpileKey) && this.Site.Stockpile[stockpileKey] <= 0)
					this.Site.Stockpile.Remove(stockpileKey);
			}
		}
		
		protected abstract AConstructionSite Site { get; }
		
		protected static IEnumerable<ConstructionResult> SimulateSpending(
			AConstructionSite site, double industryPoints, 
			IEnumerable<IConstructionProject> queue, IDictionary<string, double> vars)
		{
			var spendingPlan = new List<ConstructionResult>();
			var planStockpile = new Dictionary<string, double>(site.Stockpile);

			foreach (var buildingItem in queue) 
			{
				var stockpileKey = buildingItem.StockpileGroup;
				if (buildingItem.Condition.Evaluate(vars) < 0) 
				{
					spendingPlan.Add(new ConstructionResult(0, 0, site.Stockpile[stockpileKey], buildingItem, site.Stockpile[stockpileKey]));
					continue;
				}
				
				double cost = buildingItem.Cost.Evaluate(vars);
				double stockpile = planStockpile.ContainsKey(stockpileKey) ? planStockpile[stockpileKey] : 0;
				double totalInvestment = industryPoints + stockpile;

				double completed = cost > 0 ? Math.Floor(totalInvestment / cost) : double.PositiveInfinity;
				double countLimit = buildingItem.TurnLimit.Evaluate(vars);
				planStockpile[stockpileKey] = 0;

				if (completed > countLimit) 
				{
					double totalCost = countLimit * cost;
					
					if (stockpile >= totalCost) 
					{
						spendingPlan.Add(new ConstructionResult(
							(long)countLimit,
							0, 
							totalCost,
							buildingItem,
							stockpile - totalCost
						));
						planStockpile[stockpileKey] = stockpile - totalCost;
					}
					else 
					{
						spendingPlan.Add(new ConstructionResult(
							(long)countLimit,
							totalCost - stockpile,
							stockpile,
							buildingItem,
							0
						));
						
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
