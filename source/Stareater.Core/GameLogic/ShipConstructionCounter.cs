using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Ships;

namespace Stareater.GameLogic
{
	class ShipConstructionCounter : IConstructionVisitor
	{
		private Dictionary<Design, long> shipsInConstruction = new Dictionary<Design, long>();
		private long quantity = 0;
		
		public void Check(IEnumerable<AConstructionSiteProcessor> constructionSites)
		{
			foreach(var item in constructionSites.SelectMany(x => x.SpendingPlan))
			{
				this.quantity = item.CompletedCount;
				foreach(var effect in item.Type.Effects)
					effect.Accept(this);
			}
		}

		public IEnumerable<Design> Designs
		{
			get { return shipsInConstruction.Keys; }
		}
			
		private void add(Design design)
		{
			if (shipsInConstruction.ContainsKey(design))
				shipsInConstruction[design] += quantity;
			else
				shipsInConstruction.Add(design, quantity);
		}
		
		#region IConstructionVisitor implementation
		public void Visit(ConstructionAddColonizer constructionEffect)
		{
			this.add(constructionEffect.ColonizerDesign);
		}

		public void Visit(ConstructionAddShip constructionEffect)
		{
			this.add(constructionEffect.Design);
		}

		public void Visit(ConstructionAddBuilding constructionEffect)
		{
			//no operation
		}
		#endregion
	}
}
