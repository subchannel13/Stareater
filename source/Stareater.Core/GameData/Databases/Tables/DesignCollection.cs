using System;
using Stareater.Utils.Collections;
using Stareater.Ships;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class DesignCollection : AIndexedCollection<Design>
	{
		public CollectionIndex<Design, Player> OwnedBy { get; private set; }

		public DesignCollection()
		{
			this.OwnedBy = new CollectionIndex<Design, Player>(x => x.Owner);
			this.RegisterIndices(this.OwnedBy);
		}
	}
}
