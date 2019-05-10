using Stareater.Utils.Collections;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases.Tables
{
	class WormholeCollection : AIndexedCollection<Wormhole>
	{
		//TODO(v0.8) there can be at most one wormhole per pair of stars, consider scalar version of index
		public PairCollectionIndex<Wormhole, StarData> At { get; private set; }

		public WormholeCollection()
		{
			this.At = new PairCollectionIndex<Wormhole, StarData>(x => x.Endpoints);
			this.registerIndices(this.At);
		}
	}
}
