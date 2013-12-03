using System;

namespace Stareater.Controllers.Data
{
	public interface IGameStateListener
	{
		/// <summary>
		/// Callback for informing that AIs are done with galaxy phase. <remarks>Called on AI thread.</remarks>
		/// </summary>
		void OnAiGalaxyPhaseDone();
		
		void OnNewTurn();
	}
}
