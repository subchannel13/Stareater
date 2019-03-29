using System;
using System.Linq;
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
		private readonly Random random = new Random(); //TODO(v0.8) find better place for RNG

		public PlayerController Controller
		{
			set { this.playerController = value; }
		}

		public void PlayTurn()
		{
			var ownFleet = this.playerController.FleetsMine.ToList();
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

		//TODO(later) assumes unit is catalyzer
		public void PlayUnit(CombatantInfo unitInfo)
		{
			var ability = unitInfo.Abilities.FirstOrDefault();
			var canShoot = ability != null && ability.Quantity > 0;

			var destination = unitInfo.Position;
			if (unitInfo.ValidMoves.Any())
				destination = canShoot ?
					Methods.FindBest(unitInfo.ValidMoves, x => -Methods.HexDistance(x)) :
					Methods.FindBest(unitInfo.ValidMoves, x => Methods.HexDistance(x));

			if (destination != unitInfo.Position)
				battleController.MoveTo(destination);			
			else if (unitInfo.Position == new Vector2D(0, 0) && canShoot)
				battleController.UseAbilityOnStar(ability);
			else 
				battleController.UnitDone();
		}

		#region IBombardEventListener implementation
		void IBattleEventListener.OnStart()
		{
			//no operation
		}

		public void BombardTurn()
		{
			this.bombardController.Leave();
		}

		#endregion
	}
}
