using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Controllers
{
	public class AudienceController
	{
		private readonly GameController gameController;
		internal readonly Player[] Participants;
		internal readonly HashSet<Treaty> Treaties = new HashSet<Treaty>();

		internal AudienceController(Player[] participants, GameController gameController, MainGame gameObj)
		{
			this.gameController = gameController;
			this.Participants = participants;

			//TODO(later) make fetching with array simpler
			foreach(var treaty in gameObj.States.Treaties.Of[participants[0]].Where(x => x.Party2 == participants[1]))
				Treaties.Add(treaty);
		}
		
		public void Done()
		{
			gameController.AudienceConcluded(this);
		}

		public PlayerInfo Participant1 
		{
			get { return new PlayerInfo(this.Participants[0]); }
		}

		public PlayerInfo Participant2 
		{
			get { return new PlayerInfo(this.Participants[1]); }
		}

		public bool IsAtWar 
		{
			get { return this.Treaties.Count != 0; }
		}

		public void DecleareWar()
		{
			if (!this.IsAtWar)
				this.Treaties.Add(new Treaty(this.Participants[0], this.Participants[1]));
		}

		public void DeclearePeace()
		{
			if (this.IsAtWar)
				this.Treaties.Clear();
		}
	}
}
