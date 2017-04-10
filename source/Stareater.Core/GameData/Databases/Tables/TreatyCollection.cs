using System;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Databases.Tables
{
	class TreatyCollection : AIndexedCollection<Treaty>
	{
		public CollectionIndex<Treaty, Player> Of { get; private set; }
		
		public TreatyCollection()
		{
			this.Of = new CollectionIndex<Treaty, Player>(x => x.Party1, x => x.Party2);
			this.RegisterIndices(this.Of);
		}
	}
}
