using Stareater.GLData;
using Stareater.Localization;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class SimpleTooltip : ITooltip
	{
		private readonly string context;
		private readonly string textKey;

		public SimpleTooltip(string context, string textKey)
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
				TextSize = 12
			};
			text.Position.WrapContent(10, 5).ParentRelative(0, 0, 0, 0);

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
