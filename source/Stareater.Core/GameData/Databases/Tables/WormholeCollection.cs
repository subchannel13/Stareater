using Stareater.Utils.Collections;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases.Tables
{
	class WormholeCollection : AIndexedCollection<Wormhole>
	{
		public PairScalarIndex<Wormhole, StarData> At { get; private set; }

		public WormholeCollection()
		{
			this.At = new PairScalarIndex<Wormhole, StarData>(x => x.Endpoints);
			this.registerIndices(this.At);
		}
	}
}
