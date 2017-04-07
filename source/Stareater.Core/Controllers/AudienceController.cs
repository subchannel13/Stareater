using System;
using System.Collections.Generic;
using Stareater.Controllers.Views;
using Stareater.Players;

namespace Stareater.Controllers
{
	public class AudienceController
	{
		private readonly GameController gameController;
		private readonly Player[] participants;
		
		internal AudienceController(GameController gameController, Player[] participants)
		{
			this.gameController = gameController;
			this.participants = participants;
		}
		
		public void Done()
		{
			gameController.AudienceConcluded(this);
		}

		public PlayerInfo Participant1 
		{
			get { return new PlayerInfo(this.participants[0]); }
		}

		public PlayerInfo Participant2 
		{
			get { return new PlayerInfo(this.participants[1]); }
		}
	}
}
