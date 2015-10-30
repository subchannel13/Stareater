using System;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.Ships;

namespace Stareater.GameLogic
{
	public class ConstructionAddColonizer : IConstructionEffect
	{
		private Design colonizerDesign;
		private ColonizationProject targetColony;
		
		public ConstructionAddColonizer(Design colonizerDesign, ColonizationProject targetColony)
		{
			this.colonizerDesign = colonizerDesign;
			this.targetColony = targetColony;
		}
			
		#region IConstructionEffect implementation
		public void Apply(StatesDB states, AConstructionSite site, double quantity)
		{
			throw new NotImplementedException();
		}
		#endregion
		
	}
}
