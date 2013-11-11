using System;
using System.Collections.Generic;

namespace Stareater.GameData.Databases.Tables
{
	class ConstructionOrders
	{
		public double SpendingRatio { get; private set; }
		public IList<Constructable> Queue { get; private set; }
		
		public ConstructionOrders(double spendingRatio)
		{
			this.SpendingRatio = spendingRatio;
			this.Queue = new List<Constructable>();
		}

		internal ConstructionOrders Copy()
		{
			ConstructionOrders copy = new ConstructionOrders(this.SpendingRatio);

			foreach (var enqueued in this.Queue)
				copy.Queue.Add(enqueued);

			return copy;
		}
	}
}
