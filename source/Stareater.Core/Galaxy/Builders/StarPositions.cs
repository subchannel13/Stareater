using System.Collections.Generic;
using System.Linq;
using Stareater.Utils;

namespace Stareater.Galaxy.Builders
{
	public class StarPositions
	{
		/// <summary>
		/// Positions of stars.
		/// </summary>
		public Vector2D[] Stars { get; private set; }
 			
		/// <summary>
		/// Indices of positions for home systems.
		/// </summary>
		public int[] HomeSystems { get; private set; }

		/// <summary>
		/// Index of position for stareater main system.
		/// </summary>
		public int StareaterMain { get; private set; }

		public StarPositions(IEnumerable<Vector2D> stars, IEnumerable<int> homeSystems, int stareaterMain)
		{
			this.HomeSystems = homeSystems.ToArray();
			this.Stars = stars.ToArray();
			this.StareaterMain = stareaterMain;
		}
	}
}
