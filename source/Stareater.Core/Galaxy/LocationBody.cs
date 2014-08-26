using System;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;

namespace Stareater.Galaxy
{
	struct LocationBody
	{
		public StarData Star;
		public Planet Planet;
		
		public LocationBody(StarData star, Planet planet)
		{
			this.Star = star;
			this.Planet = planet;
		}
		
		public LocationBody(StarData star) : this(star, null)
		{ }
		
		public IkadnBaseObject Save(ObjectIndexer indexer)
		{
			if (this.Planet == null)
				return new IkonComposite(StarTag).Add(IdKey, new IkonInteger(indexer.IndexOf(this.Star)));
			else
				return new IkonComposite(PlanetTag).Add(IdKey, new IkonInteger(indexer.IndexOf(this.Planet)));
		}
		
		public static LocationBody Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			if (rawData.Tag.Equals(StarTag))
				return new LocationBody(deindexer.Get<StarData>(rawData[IdKey].To<int>()));
			else {
				Planet planet = deindexer.Get<Planet>(rawData[IdKey].To<int>());
				return new LocationBody(planet.Star, planet);
			}
		}
		
		#region Saving keys
		private const string PlanetTag = "Planet";
		private const string StarTag = "Star";
		private const string IdKey = "id";
 		#endregion
	}
}
