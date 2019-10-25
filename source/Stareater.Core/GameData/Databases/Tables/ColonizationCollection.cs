using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;
using System;

namespace Stareater.GameData.Databases.Tables
{
	class ColonizationCollection : AIndexedCollection<ColonizationProject>
	{
		public CollectionIndex<ColonizationProject, Player> OwnedBy { get; private set; }
		public CollectionIndex<ColonizationProject, Planet> Of { get; private set; }

		public ColonizationCollection()
		{
			this.OwnedBy = new CollectionIndex<ColonizationProject, Player>(x => x.Owner);
			this.Of = new CollectionIndex<ColonizationProject, Planet>(x => x.Destination);
			
			this.registerIndices(this.OwnedBy, this.Of);
		}
	}
}
