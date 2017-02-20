using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Galaxy.Builders
{
	public class StarPositions
	{
		/// <summary>
		/// Positions of stars.
		/// </summary>
		public Vector2D[] Stars;
 			
		/// <summary>
		/// Indices of positions for home systems.
		/// </summary>
		public int[] HomeSystems;

		public StarPositions(IEnumerable<Vector2D> stars, IEnumerable<int> homeSystems)
		{
			this.HomeSystems = homeSystems.ToArray();
			this.Stars = stars.ToArray();
		}
	}
}
