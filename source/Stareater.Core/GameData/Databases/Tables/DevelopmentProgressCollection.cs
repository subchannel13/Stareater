using System;
using Stareater.Utils.Collections;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class DevelopmentProgressCollection : AIndexedCollection<DevelopmentProgress>
	{
		public CollectionIndex<DevelopmentProgress, Player> Of { get; private set; }

		public DevelopmentProgressCollection()
		{
			this.Of = new CollectionIndex<DevelopmentProgress, Player>(x => x.Owner);
			this.registerIndices(this.Of);
		}
	}
}
