using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Data;
using Stareater.Controllers.Data.Ships;
using Stareater.Galaxy;
using Stareater.Ships.Missions;
using Stareater.Utils;

namespace Stareater.Controllers
{
	public class FleetController
	{
		private Game game;
		private IVisualPositioner visualPositoner;
		
		public FleetInfo Fleet { get; private set; }
		
		private HashSet<ShipGroup> selection = new HashSet<ShipGroup>();
		private List<Vector2D> simulationWaypoints = null;
		
		internal FleetController(FleetInfo fleet, Game game, IVisualPositioner visualPositoner)
		{
			this.Fleet = fleet;
			this.game = game;
			this.visualPositoner = visualPositoner;
		}
		
		public bool Valid
		{
			get { return this.game.States.Fleets.Contains(this.Fleet.FleetData); }
		}
		
		public IEnumerable<ShipGroupInfo> ShipGroups
		{
			get
			{
				return this.Fleet.FleetData.Ships.Select(x => new ShipGroupInfo(x));
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
		
		public FleetController Send(IEnumerable<Vector2D> waypoints)
		{
			//TODO(0.5) fleet splitting and merging
			var newMission = new MoveMission(waypoints);
			this.game.CurrentPlayer.Orders.ShipOrders[this.Fleet.FleetData] = newMission;
			
			return new FleetController(
				new FleetInfo(this.Fleet.FleetData, newMission, this.Fleet.FleetData.Mission, this.game, this.visualPositoner), 
				this.game, 
				this.visualPositoner
			);
		}
		
		public void SimulateTravel(StarData destination)
		{
			if (destination.Position == this.Fleet.FleetData.Position) {
				this.simulationWaypoints = null;
				return;
			}
				
			this.simulationWaypoints = new List<Vector2D>();
			//TODO(later): find shortest path
			this.simulationWaypoints.Add(this.Fleet.FleetData.Position);
			this.simulationWaypoints.Add(destination.Position);
		}
	}
}
