﻿using Stareater.GLData;
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
				TextHeight = 12
			};
			text.Position.WrapContent().Then.ParentRelative(0, 0);

			var panel = new GuiPanel()
			{
				Background = new BackgroundTexture(GalaxyTextures.Get.PanelBackground, 3),
				MaskMouseClick = false
			};
			panel.AddChild(text);
			panel.Position.WrapContent().WithPadding(10, 5);

			return panel;
		}
	}
}
