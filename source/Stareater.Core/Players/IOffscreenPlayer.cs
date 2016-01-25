using Ikadn;
using Stareater.Controllers;

namespace Stareater.Players
{
	public interface IOffscreenPlayer
	{
		void PlayTurn(GameController controller);
		void PlayBattle(/*ModeratorBorbe bitka*/);
		
		IkadnBaseObject Save();

	}
}
