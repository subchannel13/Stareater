using Ikadn.Ikon.Types;
using Stareater.Localization;

namespace Stareater.Players.DefaultAI
{
	public class DefaultAIFactory : IOffscreenPlayerFactory
	{
		internal const string FactoryId = "DefaultAI";
		
		public string Id 
		{
			get { return FactoryId; }
		}
		
		public string Name
		{
			get { return LocalizationManifest.Get.CurrentLanguage["DefaultAI"]["name"].Text(); }
		}

		public IOffscreenPlayer Create()
		{
			return new DefaultAIPlayer();
		}
		
		public IOffscreenPlayer Load(IkonComposite rawData)
		{
			return new DefaultAIPlayer();
		}
	}
}
