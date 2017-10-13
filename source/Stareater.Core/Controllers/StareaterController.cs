using Stareater.Players;
using System.Linq;

namespace Stareater.Controllers
{
	public class StareaterController
	{
		private MainGame game;
		private Player player;

		internal StareaterController(MainGame game, Player player)
		{
			this.game = game;
			this.player = player;
		}

		public bool IsReadOnly { get { return this.game.IsReadOnly; } }

		public bool HasControl
		{
			get
			{
				return this.game.States.Fleets.
					At[this.game.States.StareaterBrain.Position].
					Where(x => x.Owner == player).
					Any();
			}
		}
	}
}
