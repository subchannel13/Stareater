using System.Collections.Generic;
using System.Linq;
using Stareater.Ships;
using Stareater.GameData.Construction;

namespace Stareater.GameLogic.Planning
{
	class ShipConstructionCounter : IConstructionProjectVisitor
	{
		private Dictionary<Design, long> shipsInConstruction = new Dictionary<Design, long>();
		private long quantity = 0;
		
		public void Check(IEnumerable<AConstructionSiteProcessor> constructionSites)
		{
			foreach(var item in constructionSites.SelectMany(x => x.SpendingPlan))
			{
				this.quantity = item.CompletedCount;
				item.Project.Accept(this);
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

		#region IConstructionProjectVisitor implementation
		public void Visit(ShipProject project)
		{
			this.add(project.Type);
		}

		public void Visit(StaticProject project)
		{
			//no operation
		}
		#endregion
	}
}
