using System;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Views.Ships;
using Stareater.Utils;

namespace Stareater.GLRenderers
{
	public class VisualPositioner : IVisualPositioner
	{
		public Vector2D FleetPosition(Vector2D realPosition, AFleetMission currentMission, bool atStar)
		{
			if (currentMission.Type == FleetMissionType.Stationary)
				return realPosition + new Vector2D(0.5, 0.5);
			else if (currentMission.Type == FleetMissionType.Move && atStar)
				return realPosition + new Vector2D(-0.5, 0.5);
			
			return realPosition;
		}
	}
}
