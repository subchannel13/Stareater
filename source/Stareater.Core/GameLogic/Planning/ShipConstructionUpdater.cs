using System.Collections.Generic;
using Stareater.Ships;
using Stareater.GameData.Construction;
using Stareater.GameLogic.Combat;

namespace Stareater.GameLogic.Planning
{
	class ShipConstructionUpdater : IConstructionProjectVisitor
	{
		private readonly List<IConstructionProject> oldQueue;
		private readonly Dictionary<Design, Design> refitOrders;
		private readonly List<IConstructionProject> newQueue = new List<IConstructionProject>();
		private readonly Dictionary<Design, DesignStats> designStats;
		private bool changeItem;
		private bool deleteItem;
		private IConstructionProject newProject;
		
		public ShipConstructionUpdater(List<IConstructionProject> oldQueue, Dictionary<Design, Design> refitOrders, Dictionary<Design, DesignStats> designStats)
		{
			this.oldQueue = new List<IConstructionProject>(oldQueue);
			this.refitOrders = refitOrders;
			this.designStats = designStats;
		}
		
		public IEnumerable<IConstructionProject> Run()
		{
			foreach(var item in this.oldQueue)
			{
				this.changeItem = false;
				this.deleteItem = false;
				this.newProject = null;

				item.Accept(this);
				
				if (!this.changeItem)
					this.newQueue.Add(item);
				else if (!this.deleteItem)
					this.newQueue.Add(newProject);
			}
			
			return this.newQueue;
		}

		private Design checkDesign(Design design)
		{
			if (!this.refitOrders.ContainsKey(design))
				return design;
			
			this.changeItem = true;
			this.deleteItem |= this.refitOrders[design] == null;
			
			return this.refitOrders[design];
		}

		#region IConstructionProjectVisitor implementation
		public void Visit(ShipProject project)
		{
			var design = checkDesign(project.Type);
			newProject = new ShipProject(design, this.designStats[design].Cost, false);
        }

		public void Visit(StaticProject project)
		{
			//no operation
		}
		#endregion
	}
}
