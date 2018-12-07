using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class StellarisProcessorCollection : AIndexedCollection<StellarisProcessor>
	{
		public ScalarIndex<StellarisProcessor, StarData> At { get; private set; } //TODO(v0.8) needs a player as secondary key
		public ScalarIndex<StellarisProcessor, StellarisAdmin> Of { get; private set; }
		public CollectionIndex<StellarisProcessor, Player> OwnedBy { get; private set; }

		public StellarisProcessorCollection()
		{
			this.At = new ScalarIndex<StellarisProcessor, StarData>(x => x.Location);
			this.Of = new ScalarIndex<StellarisProcessor, StellarisAdmin>(x => x.Stellaris);
			this.OwnedBy = new CollectionIndex<StellarisProcessor, Player>(x => x.Owner);
			
			this.registerIndices(this.At, this.Of, this.OwnedBy);
		}
	}
}
