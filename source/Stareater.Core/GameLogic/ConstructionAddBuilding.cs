using System;
using Stareater.AppData.Expressions;
using Stareater.Galaxy;
using Stareater.GameData.Databases;
using Stareater.Utils.Collections;

namespace Stareater.GameLogic
{
	class ConstructionAddBuilding : IConstructionEffect
	{
		private readonly string buildingCode;
		private readonly Formula quantity;
		
		public ConstructionAddBuilding(string buildingCode, Formula quantity)
		{
			this.buildingCode = buildingCode;
			this.quantity = quantity;
		}
		
		public void Apply(StatesDB states, AConstructionSite site, double quantity)
		{
			//TODO(v0.5) report new building construction
			var vars = new Var("quantity", quantity);
			quantity = this.quantity.Evaluate(vars.Get);
			
			if (!site.Buildings.ContainsKey(buildingCode))
				site.Buildings.Add(buildingCode, quantity);
			else
				site.Buildings[buildingCode] += quantity;
		}
	}
}
