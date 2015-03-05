using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Controllers.Views.Ships;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Views;

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

		public void Rebuild(IEnumerable<StarData> stars, IEnumerable<FleetInfo> fleets)
		{
			rebuildTree(this.fleets, fleets, x => x.VisualPosition);
			rebuildTree(this.stars, stars, x => x.Position);
		}

		public void Replace(FleetInfo oldFleet, FleetInfo newFleet)
		{
			this.fleets.Remove(oldFleet);
			this.fleets.Add(newFleet, newFleet.VisualPosition, newFleet.VisualPosition);
		}

		public GalaxySearchResult Search(float x, float y, double searchRadius)
		{
			var searchCenter = new Vector2D(x, y);
			var foundObjects = new List<FoundGalaxyObject>();

			var stars = searchTree(this.stars, searchCenter, searchRadius, foundObjects, GalaxyObjectType.Star, i => i.Position);
			var fleets = searchTree(this.fleets, searchCenter, searchRadius, foundObjects, GalaxyObjectType.Fleet, i => i.VisualPosition);

			return new GalaxySearchResult(
				stars,
				fleets,
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

		private static IList<T> searchTree<T>(QuadTree<T> tree, Vector2D searchCenter, double searchRadius, List<FoundGalaxyObject> allObjects, GalaxyObjectType type, Func<T, Vector2D> positionFunc)
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
