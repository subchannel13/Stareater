using System;

namespace Stareater.Controllers.Views
{
	/// <summary>
	/// Listener for game state changes.
	/// </summary>
	/// <remarks>
	/// Methods may be called from internal background threads. 
	/// </remarks>
	public interface IGameStateListener
	{
		void OnDoAudience(AudienceController controller);
		IBattleEventListener OnDoCombat(SpaceBattleController battleController);
		IBombardEventListener OnDoBombardment(BombardmentController battleController);
		
		void OnNewTurn();
		void OnGameOver();
		void OnResearchComplete(ResearchCompleteController controller);
	}
}
