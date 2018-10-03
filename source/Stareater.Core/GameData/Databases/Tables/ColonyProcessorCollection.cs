using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;
using System;

namespace Stareater.GameData.Databases.Tables
{
	class ColonyProcessorCollection : AIndexedCollection<ColonyProcessor>
	{
		public Collection2Index<ColonyProcessor, StarData, Player> At { get; private set; }
		public ScalarIndex<ColonyProcessor, Colony> Of { get; private set; }
		public CollectionIndex<ColonyProcessor, Player> OwnedBy { get; private set; }

		public ColonyProcessorCollection()
		{
			this.At = new Collection2Index<ColonyProcessor, StarData, Player>(x => new Tuple<StarData, Player>(x.Colony.Star, x.Owner));
			this.Of = new ScalarIndex<ColonyProcessor, Colony>(x => x.Colony);
			this.OwnedBy = new CollectionIndex<ColonyProcessor, Player>(x => x.Owner);

			this.registerIndices(this.At, this.Of, this.OwnedBy);
		}
	}
}
