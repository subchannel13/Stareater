using Ikadn;
using Stareater.Controllers;
using Stareater.Controllers.Views;

namespace Stareater.Players
{
	public interface IOffscreenPlayer
	{
		void PlayTurn(PlayerController controller);
		void PlayBattle(SpaceBattleController controller);
		
		IkadnBaseObject Save();

	}
}
