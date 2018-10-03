using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;
using System;

namespace Stareater.GameData.Databases.Tables
{
	class ColonyCollection : AIndexedCollection<Colony>
	{
		public ScalarIndex<Colony, Planet> AtPlanet { get; private set; }
		public Collection2Index<Colony, StarData, Player> AtStar { get; private set; }
		public CollectionIndex<Colony, Player> OwnedBy { get; private set; }

		public ColonyCollection()
		{
			this.AtPlanet = new ScalarIndex<Colony, Planet>(x => x.Location.Planet);
			this.AtStar = new Collection2Index<Colony, StarData, Player>(x => new Tuple<StarData, Player>(x.Star, x.Owner));
			this.OwnedBy = new CollectionIndex<Colony, Player>(x => x.Owner);

			this.registerIndices(this.AtPlanet, this.AtStar, this.OwnedBy);
		}
	}
}
