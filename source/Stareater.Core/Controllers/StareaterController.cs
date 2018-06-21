using Stareater.Players;
using System.Linq;
using System.Collections.Generic;
using Stareater.Controllers.Views;

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
				return this.game.Derivates.Of(this.player).ControlsStareater;
			}
		}

		public IEnumerable<StarInfo> EjectableStars
		{
			get
			{
				return this.game.States.Stars.Where(x => x != this.game.States.StareaterBrain).Select(x => new StarInfo(x));
			}
		}

		public StarInfo EjectTarget
		{
			get
			{
				return new StarInfo(this.game.Orders[this.player].EjectingStar);
			}
			set
			{
				if (this.game.IsReadOnly || !this.EjectableStars.Contains(value))
					return;

				this.game.Orders[this.player].EjectingStar = value.Data;
				this.game.Derivates.Of(this.player).CalculateStareater(this.game);
            }
		}

		public EjectionProgressInfo EjectionProgress
		{
			get
			{
				var playerProc = this.game.Derivates.Of(this.player);

                return new EjectionProgressInfo(
					this.HasControl && this.EjectTarget != null,
					playerProc.EjectEta,
					playerProc.EjectVictoryPoints[this.player]
				);
			}
		}

		public IEnumerable<PlayerProgressInfo> GameProgress
		{
			get
			{
				return this.game.MainPlayers.Select(x => new PlayerProgressInfo(x));
			}
		}
	}
}
