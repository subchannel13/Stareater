using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Data
{
	class GalaxyObjects
	{
		private QuadTree<StarData> stars = new QuadTree<StarData>();
		private QuadTree<FleetInfo> fleets = new QuadTree<FleetInfo>();

		public IEnumerable<FleetInfo> Fleets
		{
			get
			{
				return (IEnumerable<FleetInfo>)fleets.GetAll();
			}
		}

		public void Add(FleetInfo fleet)
		{
			this.fleets.Add(fleet, fleet.Position, new Vector2D());
		}
		
		public void Rebuild(IEnumerable<StarData> stars, IEnumerable<FleetInfo> fleets)
		{
			rebuildTree(this.fleets, fleets, x => x.Position);
			rebuildTree(this.stars, stars, x => x.Position);
		}

		public void Remove(FleetInfo fleet)
		{
			this.fleets.Remove(fleet);
		}
		public void Replace(FleetInfo oldFleet, FleetInfo newFleet)
		{
			this.fleets.Remove(oldFleet);
			this.fleets.Add(newFleet, newFleet.Position, newFleet.Position);
		}

		public GalaxySearchResult Search(float x, float y, double searchRadius)
		{
			var searchCenter = new Vector2D(x, y);
			var foundObjects = new List<FoundGalaxyObject>();

			var foundStars = searchTree(this.stars, searchCenter, searchRadius, foundObjects, GalaxyObjectType.Star, i => i.Position);
			var foundFleets = searchTree(this.fleets, searchCenter, searchRadius, foundObjects, GalaxyObjectType.Fleet, i => i.Position);

			return new GalaxySearchResult(
				foundStars,
				foundFleets,
				foundObjects
			);
		}

		private static void rebuildTree<T>(QuadTree<T> tree, IEnumerable<T> items, Func<T, Vector2D> positionFunc)
		{
			tree.Clear();

			foreach (var item in items) {
				var position = positionFunc(item);
				tree.Add(item, position, new Vector2D(0, 0));
			}
		}

		private static IList<T> searchTree<T>(QuadTree<T> tree, Vector2D searchCenter, double searchRadius, ICollection<FoundGalaxyObject> allObjects, GalaxyObjectType type, Func<T, Vector2D> positionFunc)
		{
			var searchSquare = new Vector2D(searchRadius, searchRadius);
			
			var inSquare = tree.Query(searchCenter, searchSquare).ToList();
			var distances = inSquare.ToDictionary(x => x, x => (positionFunc(x) - searchCenter).Magnitude());

			var sortedResults = new List<T>(inSquare.Where(x => distances[x] <= searchRadius));
			sortedResults.Sort((a, b) => distances[a].CompareTo(distances[b]));

			for (int i = 0; i < sortedResults.Count; i++)
				allObjects.Add(new FoundGalaxyObject(
					type,
					i,
					(positionFunc(sortedResults[i]) - searchCenter).Magnitude()
				));

			return sortedResults;
		}
	}
}
