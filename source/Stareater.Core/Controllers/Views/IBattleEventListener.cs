using Stareater.Controllers.Views.Combat;

namespace Stareater.Controllers.Views
{
	/// <summary>
	/// Listener for space combat events
	/// </summary>
	public interface IBattleEventListener
	{
		void OnStart();
		void PlayUnit(CombatantInfo unitInfo);
	}
}
