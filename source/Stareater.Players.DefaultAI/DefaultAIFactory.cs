using Stareater.AppData;

namespace Stareater.Players.DefaultAI
{
	public class DefaultAIFactory : IOffscreenPlayerFactory
	{
		public string Name
		{
			get { return Settings.Get.Language["DefaultAI"]["name"]; }
		}

		public IOffscreenPlayer Create()
		{
			return new DefaultAIPlayer();
		}
	}
}
