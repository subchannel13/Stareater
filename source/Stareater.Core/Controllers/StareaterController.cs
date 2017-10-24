using Stareater.Players;
using System.Linq;
using System.Collections.Generic;
using Stareater.Galaxy;
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

		public IEnumerable<StarData> EjectableStars
		{
			get
			{
				return this.game.States.Stars.Where(x => x != this.game.States.StareaterBrain);
			}
		}

		public StarData EjectTarget
		{
			get
			{
				return this.game.Orders[this.player].EjectingStar;
			}
			set
			{
				if (this.game.IsReadOnly && value != this.game.States.StareaterBrain)
					return;

				this.game.Orders[this.player].EjectingStar = value; //TODO(v0.7) check input
			}
		}

		public EjectionProgressInfo EjectionProgress
		{
			get
			{
				return new EjectionProgressInfo(
					this.HasControl && this.EjectTarget != null,
					1, //TODO(v0.7) calculate ETA
					0 //TODO(v0.7) calculate victory points
				);
			}
		}
	}
}
