using Ikadn;
using Stareater.Controllers;

namespace Stareater.Players
{
	public interface IOffscreenPlayer
	{
		void PlayTurn(PlayerController controller);
		void PlayBattle(/*ModeratorBorbe bitka*/);
		
		IkadnBaseObject Save();

	}
}
