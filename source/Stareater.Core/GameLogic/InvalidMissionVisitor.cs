using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.Ships.Missions;

namespace Stareater.GameLogic
{
	class InvalidMissionVisitor : IMissionVisitor
	{
		private readonly MainGame game;
		private LinkedList<AMission> remainingMissions;
		
		public InvalidMissionVisitor(MainGame game)
		{
			this.game = game;
		}
		
		public Fleet Check(Fleet fleet)
		{
			this.remainingMissions = new LinkedList<AMission>();
			
			foreach (var mission in fleet.Missions)
				mission.Accept(this);
			
			return fleet.Missions.SequenceEqual(remainingMissions) ? 
				null : 
				new Fleet(fleet.Owner, fleet.Position, this.remainingMissions);
		}

		#region IMissionVisitor implementation

		public void Visit(ColonizationMission mission)
		{
			//TODO(v0.5) allow multiple players have colonization plan for the same planet
			if (game.States.ColonizationProjects.OfContains(mission.Target))
				this.remainingMissions.AddLast(mission);
		}

		public void Visit(MoveMission mission)
		{
			this.remainingMissions.AddLast(mission);
		}

		public void Visit(SkipTurnMission mission)
		{
			this.remainingMissions.AddLast(mission);
		}

		#endregion
	}
}
