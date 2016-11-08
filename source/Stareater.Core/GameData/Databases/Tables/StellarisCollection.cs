using System;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class StellarisCollection : AIndexedCollection<StellarisAdmin>
	{
		public CollectionIndex<StellarisAdmin, StarData> At { get; private set; }
		public CollectionIndex<StellarisAdmin, Player> OwnedBy { get; private set; }

		public StellarisCollection()
		{
			this.At = new CollectionIndex<StellarisAdmin, StarData>(x => x.Location.Star);
			this.OwnedBy = new CollectionIndex<StellarisAdmin, Player>(x => x.Owner);
			
			this.RegisterIndices(this.At, this.OwnedBy);
		}
	}
}
