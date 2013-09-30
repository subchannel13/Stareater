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
	}
}
