﻿using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy
{
	[StateType(saveMethod: "Save")]
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

		public IkonBaseObject Save(SaveSession session)
		{
			return this.Planet == null ?
				new IkonComposite(StarTag).Add(IdKey, session.Serialize(this.Star)) :
				new IkonComposite(PlanetTag).Add(IdKey, session.Serialize(this.Planet));
		}

		//TODO(v0.7) remove
		public IkonBaseObject Save(ObjectIndexer indexer)
		{
			return this.Planet == null ? 
				new IkonComposite(StarTag).Add(IdKey, new IkonInteger(indexer.IndexOf(this.Star))) : 
				new IkonComposite(PlanetTag).Add(IdKey, new IkonInteger(indexer.IndexOf(this.Planet)));
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
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			return (obj is LocationBody) && Equals((LocationBody)obj);
		}

		public bool Equals(LocationBody other)
		{
			return object.Equals(this.Star, other.Star) && object.Equals(this.Planet, other.Planet);
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (Star != null)
					hashCode += 1000000007 * Star.GetHashCode();
				if (Planet != null)
					hashCode += 1000000009 * Planet.GetHashCode();
			}
			return hashCode;
		}

		public static bool operator ==(LocationBody lhs, LocationBody rhs) {
			return lhs.Equals(rhs);
		}

		public static bool operator !=(LocationBody lhs, LocationBody rhs) {
			return !(lhs == rhs);
		}
		#endregion
		
		#region Saving keys
		private const string PlanetTag = "Planet";
		private const string StarTag = "Star";
		private const string IdKey = "id";
 		#endregion
	}
}
