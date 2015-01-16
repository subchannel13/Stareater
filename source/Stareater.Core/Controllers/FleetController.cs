using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Data;
using Stareater.Galaxy;

namespace Stareater.Controllers
{
	public class FleetController
	{
		private Game game;
		private IdleFleet fleet;
		
		private HashSet<ShipGroup> selection = new HashSet<ShipGroup>();
		private List<Vector2D> simulationWaypoints = null;
		
		internal FleetController(IdleFleet fleet, Game game)
		{
			this.fleet = fleet;
			this.game = game;
		}
		
		public bool Valid
		{
			get { return this.game.States.IdleFleets.Contains(this.fleet); }
		}
		
		public IEnumerable<ShipGroupInfo> ShipGroups
		{
			get
			{
				return this.fleet.Ships.Select(x => new ShipGroupInfo(x));
			}
		}
		
		public bool CanMove
		{
			get { return true; }
		}
		
		public IList<Vector2D> SimulationWaypoints
		{
			get { return this.simulationWaypoints; }
		}
		
		public void DeselectGroup(ShipGroupInfo group)
		{
			selection.Remove(group.Data);
		}
		
		public void SelectGroup(ShipGroupInfo group)
		{
			selection.Add(group.Data);
		}
		
		public void SimulateTravel(StarData destination)
		{
			this.simulationWaypoints = new List<Vector2D>();
			//TODO(later): find shortest path
			this.simulationWaypoints.Add(this.fleet.Location.Position);
			this.simulationWaypoints.Add(destination.Position);
		}
	}
}
