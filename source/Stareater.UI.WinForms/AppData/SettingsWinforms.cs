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
				{
					winformsInstance = new SettingsWinforms();
					Settings.instance = winformsInstance;
					Settings.initialize();
				}
				
				return winformsInstance;
			}
		}
		#endregion

		public float GuiScale { get; set; }
		
		public int Framerate { get; set; }
		public int BatteryFramerate { get; set; }
		public bool UnlimitedFramerate { get; set; }

		public Font FormFont
		{
			get
			{
				return new Font(SystemFonts.DefaultFont.FontFamily, SystemFonts.DefaultFont.Size * GuiScale);
			}
		}

		#region Initialization
		protected override void initDefault()
		{
			base.initDefault();
			
			this.GuiScale = 1;
			
			this.BatteryFramerate = 60;
			this.Framerate = 120;
			this.UnlimitedFramerate = false;
		}
	
		protected override void load(TaggableQueue<object, IkadnBaseObject> data)
		{
			base.load(data);
			          
			var wfSettignsData = data.Dequeue(WinformsSettingsTag).To<IkonComposite>();
			
			this.GuiScale = wfSettignsData[GuiScaleKey].To<float>();
			
			this.BatteryFramerate = wfSettignsData[FpsBatteryKey].To<int>();
			this.Framerate = wfSettignsData[FpsKey].To<int>();
			this.UnlimitedFramerate = wfSettignsData[FpsUnlimitedKey].To<int>() >= 0;
		}
		#endregion
		
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
