using System;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.Ships.Missions;

namespace Stareater.Controllers.Data
{
	class MissionInfoFactory : IMissionVisitor
	{
		private Fleet fleet; 
		private AMissionInfo result = null;
		
		private MissionInfoFactory(Fleet fleet)
		{
			this.fleet = fleet;
		}
		
		public static AMissionInfo Create(AMission mission, Fleet fleet)
		{
			if (mission == null)
				return new StationaryMissionInfo();
			
			var factory = new MissionInfoFactory(fleet);
			mission.Accept(factory);
			return factory.result;
		}

		#region IMissionVisitor implementation

		public void Visit(MoveMission mission)
		{
			result = new MoveMissionInfo(mission.Waypoints);
		}

		#endregion
	}
}
