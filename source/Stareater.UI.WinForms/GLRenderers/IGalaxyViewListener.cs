using System;
using Stareater.Controllers;
using Stareater.Controllers.Data;

namespace Stareater.GLRenderers
{
	public interface IGalaxyViewListener
	{
		void FleetSelected(IdleFleetInfo fleetInfo);
		
		void SystemOpened(StarSystemController systemController);
		void SystemSelected(StarSystemController systemController);
	}
}
