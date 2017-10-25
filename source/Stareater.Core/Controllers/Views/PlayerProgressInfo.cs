using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public class PlayerProgressInfo
	{
		public PlayerInfo Player { get; private set; }
		public double VictoryPoints { get; private set; }

		internal PlayerProgressInfo(Player player)
		{
			this.Player = new PlayerInfo(player);
			this.VictoryPoints = player.VictoryPoints;
		}
	}
}
