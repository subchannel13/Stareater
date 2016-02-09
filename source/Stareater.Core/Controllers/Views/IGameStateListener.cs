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
		void OnCombatPhaseStart();
		
		void OnDoCombat(SpaceBattleController battleController);
		
		void OnNewTurn();
	}
}
