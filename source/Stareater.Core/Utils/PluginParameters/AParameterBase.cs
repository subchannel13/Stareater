using Stareater.Localization;

namespace Stareater.Utils.PluginParameters
{
	public abstract class AParameterBase
	{
		protected string contextKey { get; private set; }
		private readonly string nameKey;

		protected AParameterBase(string contextKey, string nameKey)
		{
			this.contextKey = contextKey;
			this.nameKey = nameKey;
		}

		public string Name
		{
			get { return LocalizationManifest.Get.CurrentLanguage[contextKey][nameKey].Text(); }
		}
		
		public abstract void Accept(IParameterVisitor visitor);
	}
}
