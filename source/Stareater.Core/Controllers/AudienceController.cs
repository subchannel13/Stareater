using System;
using System.Collections.Generic;
using Stareater.Controllers.Views;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Controllers
{
	public class AudienceController
	{
		private readonly GameController gameController;
		private readonly Player[] participants;
		private readonly HashSet<Treaty> treaties = new HashSet<Treaty>();
		
		internal AudienceController(GameController gameController, Player[] participants)
		{
			this.gameController = gameController;
			this.participants = participants;
			//TODO(v0.6) fetch treaties between parties
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

		public bool IsAtWar 
		{
			get { return this.treaties.Count != 0; }
		}

		public void DecleareWar()
		{
			if (!this.IsAtWar)
				this.treaties.Add(new Treaty(this.participants[0], this.participants[1]));
		}

		public void DeclearePeace()
		{
			if (this.IsAtWar)
				this.treaties.Clear();
		}
	}
}
