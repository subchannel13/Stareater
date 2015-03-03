using System;
using Stareater.Controllers;
using Stareater.Controllers.Views;

namespace Stareater.GLRenderers
{
	public interface IGalaxyViewListener
	{
		void FleetDeselected();
		void FleetSelected(FleetController fleetController);
		
		void SystemOpened(StarSystemController systemController);
		void SystemSelected(StarSystemController systemController);
	}
}
