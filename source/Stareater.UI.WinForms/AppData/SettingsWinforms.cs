using System.Collections.Generic;
using System.Drawing;
using Ikadn;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;

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
		
		public int Framerate { get; set; }
		public int BatteryFramerate { get; set; }
		public bool UnlimitedFramerate { get; set; }

		public SettingsWinforms(TaggableQueue<object, IkadnBaseObject> data) : base(data)
		{
			if (data.CountOf(WinformsSettingsTag) > 0)
			{
				var wfSpecificData = data.Dequeue(WinformsSettingsTag).To<IkonComposite>();
				this.GuiScale = wfSpecificData[GuiScaleKey].To<float>();
				
				this.BatteryFramerate = wfSpecificData[FpsBatteryKey].To<int>();
				this.Framerate = wfSpecificData[FpsKey].To<int>();
				this.UnlimitedFramerate = wfSpecificData[FpsUnlimitedKey].To<int>() >= 0;
			}
			else
			{
				this.GuiScale = 1;
				
				this.BatteryFramerate = 60;
				this.Framerate = 120;
				this.UnlimitedFramerate = false;
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
		const string WinformsSettingsTag = "winforms";
		const string FpsBatteryKey = "fpsBattery";
		const string FpsKey = "fps";
		const string FpsUnlimitedKey = "noFps";
		const string GuiScaleKey = "guiscale";
		#endregion

		protected override void buildSaveData(IkadnWriter writer)
		{
			base.buildSaveData(writer);

			var settings = new IkonComposite(WinformsSettingsTag);
			settings.Add(GuiScaleKey, new IkonFloat(GuiScale));
			
			settings.Add(FpsBatteryKey, new IkonInteger(BatteryFramerate));
			settings.Add(FpsKey, new IkonInteger(Framerate));
			settings.Add(FpsUnlimitedKey, new IkonInteger(UnlimitedFramerate ? 1 : -1));
			settings.Compose(writer);
		}
	}
}
