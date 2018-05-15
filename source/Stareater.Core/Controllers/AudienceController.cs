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
		internal readonly Player[] Participants; //TODO(v0.7) use Pair instead of array
		internal readonly HashSet<Treaty> TreatyData = new HashSet<Treaty>();

		internal AudienceController(Player[] participants, GameController gameController, MainGame gameObj)
		{
			this.gameController = gameController;
			this.Participants = participants;

			foreach (var treaty in gameObj.States.Treaties.Of[participants[0], participants[1]])
				TreatyData.Add(treaty);
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

		public IEnumerable<TreatyInfo> Treaties
		{
			get { return this.TreatyData.Select(x => new TreatyInfo()); }
		}
		
		public bool IsAtWar 
		{
			get { return this.TreatyData.Count != 0; }
		}

		public void DecleareWar()
		{
			if (!this.IsAtWar)
				this.TreatyData.Add(new Treaty(this.Participants[0], this.Participants[1]));
		}

		public void DeclearePeace()
		{
			if (this.IsAtWar)
				this.TreatyData.Clear();
		}
	}
}
