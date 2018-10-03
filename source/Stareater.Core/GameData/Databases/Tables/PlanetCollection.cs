using Stareater.Utils.Collections;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases.Tables
{
	class PlanetCollection : AIndexedCollection<Planet>
	{
		public CollectionIndex<Planet, StarData> At { get; private set; }

		public PlanetCollection()
		{
			this.At = new CollectionIndex<Planet, StarData>(x => x.Star);
			this.registerIndices(this.At);
		}
	}
}
