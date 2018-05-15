using System;
using Stareater.Players;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Databases.Tables
{
	class TreatyCollection : AIndexedCollection<Treaty>
	{
		public PairCollectionIndex<Treaty, Player> Of { get; private set; }
		
		public TreatyCollection()
		{
			this.Of = new PairCollectionIndex<Treaty, Player>(x => x.Parties);
			this.RegisterIndices(this.Of);
		}
	}
}
