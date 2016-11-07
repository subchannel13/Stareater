using Ikadn;
using Stareater.Controllers;
using Stareater.Controllers.Views;

namespace Stareater.Players
{
	public interface IOffscreenPlayer
	{
		PlayerController Controller { set; }
		
		void PlayTurn();
		void OnResearchComplete(ResearchCompleteController controller);
		IBattleEventListener StartBattle(SpaceBattleController controller);
		
		IkadnBaseObject Save();
	}
}
