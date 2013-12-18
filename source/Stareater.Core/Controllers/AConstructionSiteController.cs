using System;
using System.Linq;
using Stareater.Galaxy;
using System.Collections.Generic;
using Stareater.Controllers.Data;

namespace Stareater.Controllers
{
	public abstract class AConstructionSiteController
	{
		internal Game Game { get; private set; }
		internal AConstructionSite Site { get; private set; }
		
		internal AConstructionSiteController(AConstructionSite site, bool readOnly, Game game)
		{
			this.Site = site;
			this.IsReadOnly = readOnly;
			this.Game = game;
		}

		public bool IsReadOnly { get; private set; }
		
		public abstract IEnumerable<ConstructableItem> ConstructableItems { get; }
		public abstract IEnumerable<ConstructableItem> ConstructionQueue { get; }
		
		public bool CanPick(ConstructableItem data)
		{
			return ConstructionQueue.Where(x => x.IdCode == data.IdCode).Count() == 0;	//TODO: consider building count
		}
		
		public void Enqueue(ConstructableItem data)
		{
			if (IsReadOnly)
				return;
			
			Game.CurrentPlayer.Orders.Constructions[Site].Queue.Add(data.Constructable);
		}
		
		public void Dequeue(int index)
		{
			if (IsReadOnly)
				return;
			
			Game.CurrentPlayer.Orders.Constructions[Site].Queue.RemoveAt(index);
		}
		
		public void ReorderQueue(int fromIndex, int toIndex)
		{
			if (IsReadOnly)
				return;
			
			var item = Game.CurrentPlayer.Orders.Constructions[Site].Queue[fromIndex];
			
			Game.CurrentPlayer.Orders.Constructions[Site].Queue.RemoveAt(fromIndex);
			Game.CurrentPlayer.Orders.Constructions[Site].Queue.Insert(toIndex, item);
		}
	}
}
