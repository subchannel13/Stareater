using Stareater.Localization;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class StaticTooltip
	{
		private string context;
		private string textKey;

		public StaticTooltip(string context, string textKey)
		{
			this.context = context;
			this.textKey = textKey;
		}

		public AGuiElement Make()
		{
			return new GuiText()
			{
				Text = LocalizationManifest.Get.CurrentLanguage[this.context][this.textKey].Text(),
				TextColor = Color.White,
				TextSize = 20
			};
		}
	}
}
