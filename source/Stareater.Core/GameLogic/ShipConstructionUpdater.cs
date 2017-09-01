﻿using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.GameData;
using Stareater.Ships;
using Stareater.GameData.Construction;
using System;

namespace Stareater.GameLogic
{
	class ShipConstructionUpdater : IConstructionProjectVisitor
	{
		private List<IConstructionProject> oldQueue;
		private Dictionary<Design, Design> refitOrders;
		private List<IConstructionProject> newQueue = new List<IConstructionProject>();
		private bool changeItem;
		private bool deleteItem;
		private IConstructionProject newProject;
		
		public ShipConstructionUpdater(List<IConstructionProject> oldQueue, Dictionary<Design, Design> refitOrders)
		{
			this.oldQueue = new List<IConstructionProject>(oldQueue);
			this.refitOrders = refitOrders;
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
		public void Visit(ColonizerProject project)
		{
			newProject = new ColonizerProject(checkDesign(project.Colonizer), project.Plan);
		}

		public void Visit(ShipProject project)
		{
			newProject = new ShipProject(checkDesign(project.Type));
        }

		public void Visit(StaticProject project)
		{
			//no operation
		}
		#endregion
	}
}
