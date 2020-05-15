using Stareater.Controllers;
using Stareater.Controllers.Views.Ships;
using System.Linq;

namespace Stareater.GameScenes
{
	class SelectedFleet : IGalaxySelection
	{
		public FleetInfo Fleet { get; private set; }
		public FleetController Controller { get; private set; }

		public SelectedFleet(FleetInfo fleet, PlayerController currentPlayer)
		{
			this.Fleet = fleet;
			this.Controller = currentPlayer.SelectFleet(fleet);
		}

		public SelectedFleet(FleetController controller)
		{
			this.Fleet = controller.Fleet;
			this.Controller = controller;
		}

		public IGalaxySelection Update(PlayerController currentPlayer)
		{
			this.Fleet = currentPlayer.FleetsAll.FirstOrDefault(this.Fleet.IsPreviousStateOf);
			
			if (this.Fleet is null)
				return null;

			this.Controller = currentPlayer.SelectFleet(this.Fleet);
			return this;
		}
	}
}
