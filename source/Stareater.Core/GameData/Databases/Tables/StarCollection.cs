using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.GameData.Databases.Tables
{
	class StarCollection : AIndexedCollection<StarData>
	{
		public ScalarIndex<StarData, Vector2D> At { get; private set; }

		public StarCollection()
		{
			this.At = new ScalarIndex<StarData, Vector2D>(x => x.Position);
			this.registerIndices(this.At);
		}
	}
}
