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
		/// <summary>
		/// Callback for informing that AIs are done with galaxy phase.
		/// </summary>
		/// <remarks>Called from AI thread.</remarks>
		void OnAiGalaxyPhaseDone();
		
		void OnCombatPhaseStart();
		
		void OnNewTurn();
	}
}
