using Ikadn.Ikon.Types;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Utils.StateEngine;

namespace Stareater.Players
{
	[StateType(saveMethod: "Save")]
	public interface IOffscreenPlayer
	{
		PlayerController Controller { set; }
		
		void PlayTurn();
		void OnResearchComplete(ResearchCompleteController controller);
		IBattleEventListener StartBattle(SpaceBattleController controller);
		IBombardEventListener StartBombardment(BombardmentController controller);

		IkonBaseObject Save(SaveSession session);

		//TODO(v0.7) remove
		IkonBaseObject Save();
	}
}
