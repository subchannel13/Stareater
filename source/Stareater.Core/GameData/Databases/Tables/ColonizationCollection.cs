using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;
using System;

namespace Stareater.GameData.Databases.Tables
{
	class ColonizationCollection : AIndexedCollection<ColonizationProject>
	{
		public CollectionIndex<ColonizationProject, Player> OwnedBy { get; private set; }
		public Collection2Index<ColonizationProject, Planet, Player> Of { get; private set; }

		public ColonizationCollection()
		{
			this.OwnedBy = new CollectionIndex<ColonizationProject, Player>(x => x.Owner);
			this.Of = new Collection2Index<ColonizationProject, Planet, Player>(x => new Tuple<Planet, Player>(x.Destination, x.Owner));
			
			this.registerIndices(this.OwnedBy, this.Of);
		}
	}
}
