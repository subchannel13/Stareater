using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Maps
{
	public struct StarPositions
	{
		public IEnumerable<Vector2D> Stars;
		public IEnumerable<Vector2D> HomeSystems;

		public StarPositions(IEnumerable<Vector2D> stars, IEnumerable<Vector2D> homeSystems)
		{
			this.HomeSystems = homeSystems;
			this.Stars = stars;
		}
	}
}
