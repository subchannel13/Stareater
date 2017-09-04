using Stareater.Galaxy;
using Stareater.Ships.Missions;

namespace Stareater.Controllers.Data
{
	class ColonizerFinder : IMissionVisitor
	{
		private readonly Planet destination;
		private bool isMatch;
		
		public ColonizerFinder(Planet destination)
		{
			this.destination = destination;
		}

		public bool Check(Fleet fleet)
		{
			this.isMatch = false;
			
			foreach(var mission in fleet.Missions)
				mission.Accept(this);
			
			return this.isMatch;
		}
		
		#region IMissionVisitor implementation

		void IMissionVisitor.Visit(ColonizationMission mission)
		{
			this.isMatch |= mission.Target == this.destination;
		}

		void IMissionVisitor.Visit(MoveMission mission)
		{
			//no operation
		}

		void IMissionVisitor.Visit(SkipTurnMission mission)
		{
			//no operation
		}

		#endregion
	}
}
