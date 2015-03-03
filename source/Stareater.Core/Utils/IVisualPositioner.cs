using System;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Views.Ships;

namespace Stareater.Utils
{
	public interface IVisualPositioner
	{
		Vector2D FleetPosition(Vector2D realPosition, AFleetMission currentMission, AFleetMission oldMission);
	}
}
