using Stareater.Controllers;
using Stareater.Controllers.Views;
using System.Linq;

namespace Stareater.GameScenes
{
	class SelectedStar : IGalaxySelection
	{
		public StarInfo Star { get; private set; }

		public SelectedStar(StarInfo star)
		{
			this.Star = star;
		}

		public IGalaxySelection Update(PlayerController currentPlayer)
		{
			if (currentPlayer.Stars.Any(this.Star.Equals))
				return this;

			return null;
		}
	}
}
