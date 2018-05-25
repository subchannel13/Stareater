using Stareater.Galaxy;
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

		public void Apply(MainGame game, AConstructionSite site, long quantity)
		{
			//TODO(v0.8) report new ship construction
			game.Derivates.Of(site.Owner).
				SpawnShip(site.Location.Star, this.Design, quantity, new AMission[0], game.States);
		}
	}
}
