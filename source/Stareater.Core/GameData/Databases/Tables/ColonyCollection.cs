using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class ColonyCollection : AIndexedCollection<Colony>
	{
		public ScalarIndex<Colony, Planet> AtPlanet { get; private set; }
		public CollectionIndex<Colony, StarData> AtStar { get; private set; }
		public CollectionIndex<Colony, Player> OwnedBy { get; private set; }

		public ColonyCollection()
		{
			this.AtPlanet = new ScalarIndex<Colony, Planet>(x => x.Location.Planet);
			this.AtStar = new CollectionIndex<Colony, StarData>(x => x.Star);
			this.OwnedBy = new CollectionIndex<Colony, Player>(x => x.Owner);

			this.RegisterIndices(this.AtPlanet, this.AtStar, this.OwnedBy);
		}
	}
}
