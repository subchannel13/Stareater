using Stareater.GameData.Construction;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.GameData.Databases.Tables
{
	class ConstructionOrders
	{
		[StatePropertyAttribute]
		public double SpendingRatio { get; set; }

		[StatePropertyAttribute]
		public List<IConstructionProject> Queue { get; private set; }

		public ConstructionOrders(double spendingRatio)
		{
			this.SpendingRatio = spendingRatio;
			this.Queue = new List<IConstructionProject>();
		}

		private ConstructionOrders()
		{ }
	}
}
