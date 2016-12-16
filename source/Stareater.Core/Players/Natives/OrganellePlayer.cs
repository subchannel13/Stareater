using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Ikadn.Ikon.Types;
using Stareater.Controllers.Views.Combat;
using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.Players.Natives
{
	class OrganellePlayer : IOffscreenPlayer, IBattleEventListener
	{
		private PlayerController playerController;
		private SpaceBattleController battleController;

		public PlayerController Controller
		{
			set { this.playerController = value; }
		}

		public void PlayTurn()
		{
			var ownFleet = this.playerController.Fleets.Where(x => x.Owner == this.playerController.Info).ToList();
			var inhabitedStars = new HashSet<StarData>(this.playerController.Stars.Where(x => this.playerController.KnownColonies(x).Any()));
			
			foreach(var movingFleet in ownFleet.Where(x => x.IsMoving))
				inhabitedStars.ExceptWith(movingFleet.Missions.Waypoints.Select(x => this.playerController.Star(x.Destionation)));
			
			foreach(var fleet in ownFleet.Where(x => !x.IsMoving))
			{
				if (!inhabitedStars.Any())
					break;
				
				var destination = inhabitedStars.First();
				var fleetControl = this.playerController.SelectFleet(fleet);
				foreach(var shipGroup in fleetControl.ShipGroups)
					fleetControl.SelectGroup(shipGroup, shipGroup.Quantity);
				fleetControl.Send(new []{ destination.Position });
				inhabitedStars.Remove(destination);
			}
		}

		public void OnResearchComplete(ResearchCompleteController controller)
		{
			//no operation
		}

		public IBattleEventListener StartBattle(SpaceBattleController controller)
		{
			this.battleController = controller;
			return this;
		}

		public Ikadn.IkadnBaseObject Save()
		{
			return new IkonComposite(PlayerType.OrganelleControllerTag);
		}

		public void PlayUnit(CombatantInfo unitInfo)
		{
			var destination = unitInfo.ValidMoves.Aggregate(
				unitInfo.Position,
				(a, b) => (Methods.HexDistance(a) <= Methods.HexDistance(b)) ? a : b
			);
			
			if (destination != unitInfo.Position)
				battleController.MoveTo(destination);
			
			//TODO(v0.6) shoot catalysis beam at star
			
			battleController.UnitDone();
		}
	}
}
