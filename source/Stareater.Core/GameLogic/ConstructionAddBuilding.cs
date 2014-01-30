using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameLogic
{
	class ConstructionAddBuilding : AConstructionEffect
	{
		private string buildingCode;
		private Formula quantity;
		
		public ConstructionAddBuilding(string buildingCode, Formula quantity)
		{
			this.buildingCode = buildingCode;
			this.quantity = quantity;
		}
	}
}
