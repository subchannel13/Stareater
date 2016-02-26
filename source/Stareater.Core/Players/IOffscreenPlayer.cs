using Ikadn;
using Stareater.Controllers;
using Stareater.Controllers.Views;

namespace Stareater.Players
{
	public interface IOffscreenPlayer
	{
		void PlayTurn(PlayerController controller);
		IBattleEventListener StartBattle(SpaceBattleController controller);
		
		IkadnBaseObject Save();

	}
}
