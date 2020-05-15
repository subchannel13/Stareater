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
			this.Fleet = currentPlayer.FleetsAll.FirstOrDefault(this.Fleet.IsPreviousStateOf);
			if (this.Fleet != null)
				return this;

			return null;
		}
	}
}
