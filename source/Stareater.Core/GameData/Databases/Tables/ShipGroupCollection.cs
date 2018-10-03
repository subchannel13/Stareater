using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Ships;

namespace Stareater.GameData.Databases.Tables
{
	class ShipGroupCollection : AIndexedCollection<ShipGroup>
	{
		public ScalarIndex<ShipGroup, Design> WithDesign { get; private set; }

		public ShipGroupCollection()
		{
			this.WithDesign = new ScalarIndex<ShipGroup, Design>(x => x.Design);
			this.registerIndices(this.WithDesign);
		}
	}
}
