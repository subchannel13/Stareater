using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.Ships;
using Stareater.Ships.Missions;
using Stareater.Utils;
using Stareater.Controllers.Data;

namespace Stareater.Controllers
{
	public class FleetController
	{
		private Game game;
		private GalaxyObjects mapObjects;
		private IVisualPositioner visualPositoner;
		
		public FleetInfo Fleet { get; private set; }

		private Dictionary<Design, long> selection = new Dictionary<Design, long>();
		private List<Vector2D> simulationWaypoints = null;
		
		internal FleetController(FleetInfo fleet, Game game, GalaxyObjects mapObjects, IVisualPositioner visualPositoner)
		{
			this.Fleet = fleet;
			this.game = game;
			this.mapObjects = mapObjects;
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
			selection.Remove(group.Data.Design);
		}
		
		public void SelectGroup(ShipGroupInfo group)
		{
			selection.Add(group.Data.Design, group.Data.Quantity);
		}
		
		public FleetController Send(IEnumerable<Vector2D> waypoints)
		{
			if (this.selection.Count == 0)
				return this;

			//create regroup order if there is none
			HashSet<Fleet> shipOrders;
			if (!this.Fleet.FleetData.Owner.Orders.ShipOrders.ContainsKey(this.Fleet.FleetData.Position)) {
				shipOrders = new HashSet<Fleet>();
				this.Fleet.FleetData.Owner.Orders.ShipOrders.Add(this.Fleet.FleetData.Position, shipOrders);
			}
			else
				shipOrders = this.Fleet.FleetData.Owner.Orders.ShipOrders[this.Fleet.FleetData.Position];
			
			//remove current fleet from regroup
			shipOrders.Remove(this.Fleet.FleetData);
			this.mapObjects.Remove(this.Fleet);
			
			//add moving fleet
			var moveFleet = new Fleet(this.Fleet.FleetData.Owner, this.Fleet.FleetData.Position, new MoveMission(waypoints));
			foreach(var selectedGroup in this.selection)
				moveFleet.Ships.Add(new ShipGroup(selectedGroup.Key, selectedGroup.Value));
			
			var moveFleetInfo = this.addFleet(shipOrders, moveFleet);
			
			//add stationary fleet
			var stationaryFleet = new Fleet(this.Fleet.FleetData.Owner, this.Fleet.FleetData.Position, this.Fleet.FleetData.Mission);
			foreach(var group in this.Fleet.FleetData.Ships) 
				if (this.selection.ContainsKey(group.Design) && group.Quantity - this.selection[group.Design] > 0)
					stationaryFleet.Ships.Add(new ShipGroup(group.Design, group.Quantity - this.selection[group.Design]));
			if (stationaryFleet.Ships.Count > 0)
				this.addFleet(shipOrders, stationaryFleet);

			return new FleetController(
				moveFleetInfo, 
				this.game,
				this.mapObjects,
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

		private FleetInfo addFleet(ICollection<Stareater.Galaxy.Fleet> shipOrders, Fleet newFleet)
		{
			var similarFleet = shipOrders.FirstOrDefault(x => x.Mission == newFleet.Mission);
			
			if (similarFleet != null) {
				foreach(var shipGroup in newFleet.Ships)
					if (similarFleet.Ships.DesignContains(shipGroup.Design))
						similarFleet.Ships.Design(shipGroup.Design).Quantity += shipGroup.Quantity;
					else
						similarFleet.Ships.Add(shipGroup);
				
				var fleetInfo = this.mapObjects.InfoOf(similarFleet, this.Fleet.AtStar, this.visualPositoner);
				if (fleetInfo == null) {
					fleetInfo = new FleetInfo(similarFleet, this.Fleet.AtStar, this.visualPositoner);
					this.mapObjects.Add(fleetInfo);
				}
				
				return fleetInfo;
			}
			else {
				shipOrders.Add(newFleet);
				
				var fleetInfo = new FleetInfo(newFleet, this.Fleet.AtStar, this.visualPositoner);
				this.mapObjects.Add(fleetInfo);
				
				return fleetInfo;
			}
			
		}
	}
}
