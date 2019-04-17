using Stareater.GLData;
using Stareater.Localization;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class StaticTooltip
	{
		private readonly string context;
		private readonly string textKey;

		public StaticTooltip(string context, string textKey)
		{
			this.context = context;
			this.textKey = textKey;
		}

		public AGuiElement Make()
		{
			var text = new GuiText()
			{
				Text = LocalizationManifest.Get.CurrentLanguage[this.context][this.textKey].Text(),
				TextColor = Color.White,
				TextSize = 20
			};
			text.Position.WrapContent();

			var panel = new GuiPanel()
			{
				Background = new BackgroundTexture(GalaxyTextures.Get.PanelBackground, 3)
			};
			panel.AddChild(text);
			panel.Position.WrapContent();

			return panel;
		}
	}
}
