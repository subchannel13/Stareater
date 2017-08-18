using Ikadn.Ikon.Types;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Utils.StateEngine;

namespace Stareater.Players
{
	[StateBaseType("loadControl", typeof(Player))]
	public interface IOffscreenPlayer
	{
		PlayerController Controller { set; }
		
		void PlayTurn();
		void OnResearchComplete(ResearchCompleteController controller);
		IBattleEventListener StartBattle(SpaceBattleController controller);
		IBombardEventListener StartBombardment(BombardmentController controller);
	}
}
