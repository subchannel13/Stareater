using System.Collections.Generic;
using Ikon.Ston;
using Ikon.Ston.Values;
using System.Drawing;
using Ikon;

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

		public float GuiScale { get; set; }
		
		public SettingsWinforms(Dictionary<string, Object> data)
			:base(data)
		{
			if (data.ContainsKey(WinformsSettingsKey))
			{
				Object wfSpecificData = data[WinformsSettingsKey];
				GuiScale = (wfSpecificData[GuiScaleKey] as Numeric).GetFloat;
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

		protected override void buildSaveData(IkonWriter writer)
		{
			base.buildSaveData(writer);

			Object settings = new Object(WinformsSettingsKey);
			settings.Add(GuiScaleKey, new Numeric(GuiScale));
			settings.Compose(writer);
		}
	}
}
