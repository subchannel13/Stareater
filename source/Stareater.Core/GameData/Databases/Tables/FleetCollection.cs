using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.Utils;

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
