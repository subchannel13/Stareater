using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.Utils;
using System;

namespace Stareater.GameData.Databases.Tables
{
	class FleetCollection : AIndexedCollection<Fleet>
	{
		public Collection2Index<Fleet, Vector2D, Player> At { get; private set; }
		public CollectionIndex<Fleet, Player> OwnedBy { get; private set; }

		public FleetCollection()
		{
			this.At = new Collection2Index<Fleet, Vector2D, Player>(x => new Tuple<Vector2D, Player>(x.Position, x.Owner));
			this.OwnedBy = new CollectionIndex<Fleet, Player>(x => x.Owner);
			
			this.registerIndices(this.At, this.OwnedBy);
		}
	}
}
