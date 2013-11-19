using System;
using System.Collections.Generic;
using Stareater.Controllers.Data;
using Stareater.Galaxy;

namespace Stareater.Controllers
{
	public class StarManagementController : AConstructionSiteController
	{
		private Game game;
		private object starManagement; //TODO: make type
		
		internal StarManagementController(Game game, AConstructionSite starManagement, bool readOnly): base(starManagement, readOnly)
		{
			this.game = game;
			this.starManagement = starManagement;
		}
		
		public override void ReorderQueue(int fromIndex, int toIndex)
		{
			//TODO
			//throw new NotImplementedException();
		}
		
		public override void Enqueue(Stareater.Controllers.Data.ConstructableItem data)
		{
			//TODO
			//throw new NotImplementedException();
		}
		
		public override void Dequeue(int selectedIndex)
		{
			//TODO
			//throw new NotImplementedException();
		}
		
		public override IEnumerable<ConstructableItem> ConstructionQueue {
			get {
				//TODO
				//throw new NotImplementedException();
				yield break;
			}
		}
		
		public override IEnumerable<ConstructableItem> ConstructableItems {
			get {
				//TODO
				//throw new NotImplementedException();
				yield break;
			}
		}
	}
}
