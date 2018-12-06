using Stareater.Galaxy;
using Stareater.Ships.Missions;
using Stareater.Utils;

namespace Stareater.GameLogic.Planning
{
	class TransportDestinationVisitior : IMissionVisitor
	{
		private bool stop;
		private Vector2D? destination;

		public Vector2D? Trace(Fleet fleet)
		{
			this.stop = false;
			this.destination = fleet.Position;

			foreach (var mission in fleet.Missions)
				if (!this.stop)
					mission.Accept(this);
				else
					break;

			return this.destination;
		}

		void IMissionVisitor.Visit(DisembarkMission mission)
		{
			this.stop = true;
		}

		void IMissionVisitor.Visit(LoadMission mission)
		{
			this.stop = true;
			this.destination = null;
		}

		void IMissionVisitor.Visit(MoveMission mission)
		{
			this.destination = mission.Destination.Position;
		}

		void IMissionVisitor.Visit(SkipTurnMission mission)
		{
			//no operation
		}
	}
}
