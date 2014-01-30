using System;

namespace Stareater.GameLogic
{
	class ConstructionAddBuilding : AConstructionEffect
	{
		private string buildingCode;
		private long quantity;
		
		public ConstructionAddBuilding(string buildingCode, long quantity)
		{
			this.buildingCode = buildingCode;
			this.quantity = quantity;
		}
	}
}
