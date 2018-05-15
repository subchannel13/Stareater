using System;
using Stareater.Utils.Collections;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases.Tables
{
	class WormholeCollection : AIndexedCollection<Wormhole>
	{
		public PairCollectionIndex<Wormhole, StarData> At { get; private set; }

		public WormholeCollection()
		{
			this.At = new PairCollectionIndex<Wormhole, StarData>(x => x.Endpoints);
			this.RegisterIndices(this.At);
		}
	}
}
