using System;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.Players.Natives
{
	[StateType(saveTag: PlayerType.OrganelleControllerTag)]
	class OrganellePlayer : IOffscreenPlayer, IBattleEventListener, IBombardEventListener
	{
		private PlayerController playerController;
		private SpaceBattleController battleController;
		private BombardmentController bombardController;
		private Random random = new Random(); //TODO(v0.8) find better place for RNG

		public PlayerController Controller
		{
			set { this.playerController = value; }
		}

		public void PlayTurn()
		{
			var ownFleet = this.playerController.Fleets.Where(x => x.Owner == this.playerController.Info).ToList();
			var investigating = ownFleet.Where(x => x.IsMoving).SelectMany(x => x.Missions.Waypoints).ToList();
			var stars = new PickList<StarInfo>(random, this.playerController.Stars.Where(s => investigating.All(x => x.Destionation != s.Position)));
			
			foreach(var fleet in ownFleet.Where(x => !x.IsMoving))
			{
				if (stars.Count() == 0)
					break;
				
				var destination = stars.Pick();
				var fleetControl = this.playerController.SelectFleet(fleet);
				foreach(var shipGroup in fleetControl.ShipGroups)
					fleetControl.SelectGroup(shipGroup, shipGroup.Quantity);
				fleetControl.Send(new []{ destination.Position });
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

		public IBombardEventListener StartBombardment(BombardmentController controller)
		{
			this.bombardController = controller;
			return this;
		}

		public void PlayUnit(CombatantInfo unitInfo)
		{
			var destination = unitInfo.ValidMoves.Aggregate(
				unitInfo.Position,
				(a, b) => (Methods.HexDistance(a) <= Methods.HexDistance(b)) ? a : b
			);
			
			if (destination != unitInfo.Position)
			{
				battleController.MoveTo(destination);
				return;
			}
			
			if (unitInfo.Position == new Vector2D(0, 0))
			{
				var ability = unitInfo.Abilities.FirstOrDefault();
				if (ability != null && ability.Quantity > 0)
				{
					battleController.UseAbilityOnStar(ability);
					return;
				}
			}
			
			battleController.UnitDone();
		}

		#region IBombardEventListener implementation

		public void BombardTurn()
		{
			this.bombardController.Leave();
		}

		#endregion
	}
}
