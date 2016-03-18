using System.Drawing;
using Ikadn;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using Stareater.Utils;

namespace Stareater.AppData
{
	class SettingsWinforms : Settings
	{
		#region Singleton
		static SettingsWinforms winformsInstance = null;

		public static new SettingsWinforms Get
		{
			get
			{
				if (winformsInstance == null)
					winformsInstance = new SettingsWinforms(loadFile());
				return winformsInstance;
			}
		}
		#endregion

		public float GuiScale { get; set; }
		
		public int Framerate { get; set; }
		public int BatteryFramerate { get; set; }
		public bool UnlimitedFramerate { get; set; }

		public SettingsWinforms(TaggableQueue<object, IkadnBaseObject> data) : base(data)
		{
			Settings.instance = this;
			
			var wfSettignsData = data.CountOf(WinformsSettingsTag) > 0 ?
				data.Dequeue(WinformsSettingsTag).To<IkonComposite>():
				new IkonComposite(WinformsSettingsTag);
			
				
			this.GuiScale = wfSettignsData.ToOrDefault(GuiScaleKey, 1f);
			
			this.BatteryFramerate = wfSettignsData.ToOrDefault(FpsBatteryKey, 60);
			this.Framerate = wfSettignsData.ToOrDefault(FpsKey, 120);
			this.UnlimitedFramerate = wfSettignsData.ToOrDefault(FpsUnlimitedKey, x => x.To<int>() >= 0, false);
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
