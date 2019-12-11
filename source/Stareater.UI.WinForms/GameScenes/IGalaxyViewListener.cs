using Stareater.Controllers;

namespace Stareater.GameScenes
{
	public interface IGalaxyViewListener
	{
		void TurnEnded();

		//TODO(v0.9) see if need
		void SystemOpened(StarSystemController systemController);
		void SystemSelected(StarSystemController systemController);
	}
}
