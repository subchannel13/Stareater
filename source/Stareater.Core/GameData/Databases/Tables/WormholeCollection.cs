using System;
using Stareater.Utils.Collections;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases.Tables
{
	class WormholeCollection : AIndexedCollection<Wormhole>
	{
		public CollectionIndex<Wormhole, StarData> At { get; private set; }
		
		public WormholeCollection()
		{
			this.At = new CollectionIndex<Wormhole, StarData>(x => x.FromStar, x => x.ToStar);
			this.RegisterIndices(this.At);
		}
	}
}
