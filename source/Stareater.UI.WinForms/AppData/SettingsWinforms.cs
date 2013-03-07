using System.Collections.Generic;
using System.Drawing;
using Ikadn;
using Ikadn.Ikon.Values;

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

		public SettingsWinforms(ValueQueue data)
			:base(data)
		{
			if (data.CountOf(WinformsSettingsKey) > 0)
			{
				ObjectValue wfSpecificData = data.Dequeue(WinformsSettingsKey).To<ObjectValue>();
				GuiScale = wfSpecificData[GuiScaleKey].To<float>();
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

		protected override void buildSaveData(IkadnWriter writer)
		{
			base.buildSaveData(writer);

			ObjectValue settings = new ObjectValue(WinformsSettingsKey);
			settings.Add(GuiScaleKey, new NumericValue(GuiScale));
			settings.Compose(writer);
		}
	}
}
