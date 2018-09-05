using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.Ships.Missions;

namespace Stareater.GameLogic.Planning
{
	class InvalidMissionVisitor : IMissionVisitor
	{
		private readonly MainGame game;
		private LinkedList<AMission> remainingMissions;
		private Player owner;
		
		public InvalidMissionVisitor(MainGame game)
		{
			this.game = game;
		}
		
		public Fleet Check(Fleet fleet)
		{
			this.remainingMissions = new LinkedList<AMission>();
			this.owner = fleet.Owner;
			
			foreach (var mission in fleet.Missions)
				mission.Accept(this);
			
			if (fleet.Missions.SequenceEqual(remainingMissions))
				return null;
					
			var newFleet = new Fleet(fleet.Owner, fleet.Position, this.remainingMissions);
			foreach(var ship in fleet.Ships)
				newFleet.Ships.Add(new ShipGroup(ship.Design, ship.Quantity, ship.Damage, ship.UpgradePoints, ship.PopulationTransport));
			
			return newFleet;
		}

		#region IMissionVisitor implementation

		public void Visit(ColonizationMission mission)
		{
			if (game.States.ColonizationProjects.Of[mission.Target].Any(x => x.Owner == owner))
				this.remainingMissions.AddLast(mission);
		}

		public void Visit(MoveMission mission)
		{
			if (this.game.States.Stars.Contains(mission.Destination))
				this.remainingMissions.AddLast(mission);
		}

		public void Visit(SkipTurnMission mission)
		{
			this.remainingMissions.AddLast(mission);
		}

		#endregion
	}
}
