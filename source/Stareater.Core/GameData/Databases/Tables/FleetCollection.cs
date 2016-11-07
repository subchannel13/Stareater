using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.Ships;

namespace Stareater.GameData.Databases.Tables
{
	class FleetCollection : AIndexedCollection<Fleet>
	{
		public CollectionIndex<Fleet, Vector2D> At { get; private set; }
		public CollectionIndex<Fleet, Player> OwnedBy { get; private set; }

		public FleetCollection()
		{
			this.At = new CollectionIndex<Fleet, Vector2D>(x => x.Position);
			this.OwnedBy = new CollectionIndex<Fleet, Player>(x => x.Owner);
			
			this.RegisterIndices(this.At, this.OwnedBy);
		}
	}
}
