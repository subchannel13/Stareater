using System;
using Stareater.Utils.Collections;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class ResearchProgressCollection : AIndexedCollection<ResearchProgress>
	{
		public CollectionIndex<ResearchProgress, Player> Of { get; private set; }

		public ResearchProgressCollection()
		{
			this.Of = new CollectionIndex<ResearchProgress, Player>(x => x.Owner);
			this.RegisterIndices(this.Of);
		}
	}
}
