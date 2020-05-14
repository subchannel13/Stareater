using Stareater.Controllers;
using Stareater.Controllers.Views.Ships;
using System.Linq;

namespace Stareater.GameScenes
{
	class SelectedFleet : IGalaxySelection
	{
		public FleetInfo Fleet { get; private set; }

		public SelectedFleet(FleetInfo fleet)
		{
			this.Fleet = fleet;
		}

		public IGalaxySelection Update(PlayerController currentPlayer)
		{
			//TODO(v0.9) make smarter check for changing fleets (moving, colonizing, etc.)
			if (currentPlayer.FleetsAll.Any(this.Fleet.Equals))
				return this;

			return null;
		}
	}
}
