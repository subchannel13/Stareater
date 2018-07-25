using Stareater.Utils;

namespace Stareater.Controllers.Views.Ships
{
	public class WaypointInfo
	{
		public Vector2D Destionation { get; private set; }
		public bool UsingWormhole { get; private set; }
		
		internal WaypointInfo(Vector2D destionation, bool usingWormhole)
		{
			this.Destionation = destionation;
			this.UsingWormhole = usingWormhole;
		}
	}
}
