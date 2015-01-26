using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using NGenerics.Extensions;
using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.Controllers.Data
{
	internal class GalaxySearch
	{
		private Vector2D searchCenter;
		private double searchRadius;
		
		private List<Fleet> idleFleets;
		private List<StarData> stars;
		private List<FoundGalaxyObject> foundObjects;
		
		public GalaxySearch(float x, float y, double searchRadius)
		{
			this.searchCenter = new Vector2D(x, y);
			this.searchRadius = searchRadius;
			
			this.idleFleets = null;
			this.stars = null;
			this.foundObjects = new List<FoundGalaxyObject>();
		}
		
		private List<T> sort<T>(IEnumerable<T> objects, Func<T, Vector2D> positionFunc) where T : class
		{
			var distances = objects.ToDictionary(x => x, x => (positionFunc(x) - this.searchCenter).Magnitude());
			
			var sorted = new List<T>(objects.Where(x => distances[x] <= this.searchRadius));
			sorted.Sort((a, b) => distances[a].CompareTo(distances[b]));
			return sorted;
		}
		
		private void markFound<T>(List<T> objects, GalaxyObjectType type, Func<T, Vector2D> positionFunc)
		{
			for (int i = 0; i < objects.Count; i++)
				this.foundObjects.Add(new FoundGalaxyObject(
					type,
					i,
					(positionFunc(objects[i]) - searchCenter).Magnitude()
				));
		}
		
		public void Compare(IEnumerable<Fleet> idleFleets, Methods.VisualPositionFunc visualPositionFunc)
		{
			this.idleFleets = this.sort(idleFleets, x => visualPositionFunc(x.Position));
		}
		
		public void Compare(IEnumerable<StarData> stars)
		{
			this.stars = sort(stars, x => x.Position);
		}
		
		//TODO(later) try to remove "game" parameter
		//TODO(v0.5) change Methods.VisualPositionFunc to class with other visual positioners
		public GalaxySearchResult Finish(Game game, Methods.VisualPositionFunc idleFleetVisualPositionFunc)
		{
			markFound(this.idleFleets, GalaxyObjectType.IdleFleet, x => idleFleetVisualPositionFunc(x.Position));
			markFound(this.stars, GalaxyObjectType.Star, x => x.Position);
			this.foundObjects.Sort((a, b) => a.Distance.CompareTo(b.Distance));
			
			return new GalaxySearchResult(
				this.stars,
				this.idleFleets.Select(x => new IdleFleetInfo(x, game, idleFleetVisualPositionFunc)).ToList(),
				this.foundObjects
			);
		}
	}
}
