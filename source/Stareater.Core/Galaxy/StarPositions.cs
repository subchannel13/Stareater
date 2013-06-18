using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Galaxy
{
	public struct StarPositions
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
