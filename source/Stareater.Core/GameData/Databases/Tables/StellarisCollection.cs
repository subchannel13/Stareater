using System;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class StellarisCollection : AIndexedCollection<StellarisAdmin>
	{
		public Scalar2Index<StellarisAdmin, StarData, Player> At { get; private set; }
		public CollectionIndex<StellarisAdmin, Player> OwnedBy { get; private set; }

		public StellarisCollection()
		{
			this.At = new Scalar2Index<StellarisAdmin, StarData, Player>(x => new Tuple<StarData, Player>(x.Location.Star, x.Owner));
			this.OwnedBy = new CollectionIndex<StellarisAdmin, Player>(x => x.Owner);
			
			this.registerIndices(this.At, this.OwnedBy);
		}
	}
}
