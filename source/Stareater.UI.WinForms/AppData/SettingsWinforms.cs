using System.Collections.Generic;
using IKON.STON;
using IKON.STON.Values;
using System.Drawing;

namespace Stareater.AppData
{
	class SettingsWinforms : Settings
	{
		#region Singleton
		static SettingsWinforms instance = null;

		public static new SettingsWinforms Get
		{
			get
			{
				if (instance == null)
					instance = new SettingsWinforms(loadFile());
				return instance;
			}
		}
		#endregion

		public float GuiScale { get; private set; }
		
		public SettingsWinforms(Dictionary<string, Object> data)
			:base(data)
		{
			if (data.ContainsKey(WinformsSettingsKey))
			{
				Object wfSpecificData = data[WinformsSettingsKey];
				GuiScale = wfSpecificData[GuiScaleKey].AsNumber().GetFloat;
			}
			else
			{
				GuiScale = 1;
			}
		}

		public Font FormFont
		{
			get
			{
				return new Font(SystemFonts.DefaultFont.FontFamily, SystemFonts.DefaultFont.Size * GuiScale);
			}
		}

		#region Attribute keys
		const string WinformsSettingsKey = "winforms";
		const string GuiScaleKey = "guiscale";
		#endregion
	}
}
