using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	partial class ColonyProcessorCollection : AIndexedCollection<ColonyProcessor>
	{
		public CollectionIndex<ColonyProcessor, StarData> At { get; private set; }
		public ScalarIndex<ColonyProcessor, Colony> Of { get; private set; }
		public CollectionIndex<ColonyProcessor, Player> OwnedBy { get; private set; }

		public ColonyProcessorCollection()
		{
			this.At = new CollectionIndex<ColonyProcessor, StarData>(x => x.Colony.Star);
			this.Of = new ScalarIndex<ColonyProcessor, Colony>(x => x.Colony);
			this.OwnedBy = new CollectionIndex<ColonyProcessor, Player>(x => x.Owner);

			this.RegisterIndices(this.At, this.Of, this.OwnedBy);
		}
	}
}
