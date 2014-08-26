using Ikadn;

namespace Stareater.Players
{
	public interface IOffscreenPlayer
	{
		void PlayTurn(/*IgraZvj igra*/);
		void PlayBattle(/*ModeratorBorbe bitka*/);
		
		IkadnBaseObject Save();

	}
}
