using System;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Views.Ships;
using Stareater.Utils;

namespace Stareater.GLRenderers
{
	public class VisualPositioner : IVisualPositioner
	{
		public Vector2D FleetPosition(Vector2D realPosition, MissionsInfo missions, bool atStar)
		{
			if (missions.Waypoints.Length == 0)
				return realPosition + new Vector2D(0.5, 0.5);
			if (missions.Waypoints.Length > 0 && atStar)
				return realPosition + new Vector2D(-0.5, 0.5);
			
			return realPosition;
		}
	}
}
