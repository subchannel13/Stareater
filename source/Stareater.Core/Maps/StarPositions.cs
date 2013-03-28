using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils;

namespace Stareater.Maps
{
	public struct StarPositions
	{
		public IEnumerable<Point2d> Stars;
		public IEnumerable<Point2d> HomeSystems;

		public StarPositions(IEnumerable<Point2d> stars, IEnumerable<Point2d> homeSystems)
		{
			this.HomeSystems = homeSystems;
			this.Stars = stars;
		}
	}
}
