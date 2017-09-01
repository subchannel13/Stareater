using Stareater.Galaxy;
using Stareater.GameData.Databases;
using Stareater.Ships;
using Stareater.Ships.Missions;

namespace Stareater.GameData.Construction
{
	class ConstructionAddShip : IConstructionEffect
	{
		public Design Design { get; private set; }
		
		public ConstructionAddShip(Design design)
		{
			this.Design = design;
		}

		public void Apply(StatesDB states, TemporaryDB derivates, AConstructionSite site, long quantity)
		{
			//TODO(v0.7) report new ship construction
			derivates.Of(site.Owner).SpawnShip(site.Location.Star, this.Design, quantity, new AMission[0], states);
		}
	}
}
