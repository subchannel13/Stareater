using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.Controllers.Views.Ships
{
	public class WaypointInfo
	{
		internal StarData DestionationStar { get; private set; }
		internal Wormhole UsedWormhole { get; private set; }
		
		internal WaypointInfo(StarData destionation, Wormhole usedWormhole)
		{
			this.DestionationStar = destionation;
			this.UsedWormhole = usedWormhole;
		}

		public Vector2D Destionation
		{
			get { return this.DestionationStar.Position; }
		}

		public bool UsingWormhole
		{
			get { return this.UsedWormhole != null; }
		}
	}
}
