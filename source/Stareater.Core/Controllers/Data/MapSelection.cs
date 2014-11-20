using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.Controllers.Data
{
	class MapSelection
	{
		private Vector2D searchCenter;
		private double searchRadius;
		private double closestDist = double.PositiveInfinity;
		
		public IdleFleet IdleFleetCandidate { get; private set; }
		public StarData StarCandidate { get; private set; }
				
		public MapSelection(float x, float y, double searchRadius)
		{
			this.searchCenter = new Vector2D(x, y);
			this.searchRadius = searchRadius;
			
			this.IdleFleetCandidate = null;
			this.StarCandidate = null;
		}
		
		private T findClosest<T>(IEnumerable<T> objects, Func<T, Vector2D> positionFunc) where T : class
		{
			T candidate = null;
			double candidateDist = double.PositiveInfinity;
			
			foreach (var entity in objects) {
				double entityDist = (positionFunc(entity) - searchCenter).Magnitude();
				
				if (candidate == null || entityDist < candidateDist)
				{
					candidate = entity;
					candidateDist = entityDist;
				}
			}
			
			if (candidateDist < closestDist)
			{
				forgetAll();
				closestDist = candidateDist;
			}
			else
				return null;
			
			if (candidate != null && candidateDist <= searchRadius)
				return candidate;
			else
				return null;
		}
		
		private void forgetAll()
		{
			this.IdleFleetCandidate = null;
			this.StarCandidate = null;
		}
		
		public void Compare(IEnumerable<IdleFleet> idleFleets, Methods.VisualPositionFunc visualPositionFunc)
		{
			this.IdleFleetCandidate = findClosest(idleFleets, x => visualPositionFunc(x.Location.Position));
		}
		
		public void Compare(IEnumerable<StarData> stars)
		{
			this.StarCandidate = findClosest(stars, x => x.Position);
		}
	}
}
