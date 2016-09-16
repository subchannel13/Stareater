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
		public bool UnlimitedFramerate { get; set; }
		public bool VSync { get; set; }
		public BusySpinMode FramerateBusySpinUsage { get; set; }

		public Font FormFont
		{
			get
			{
				return new Font(SystemFonts.DefaultFont.FontFamily, SystemFonts.DefaultFont.Size * GuiScale);
			}
		}
		
		public bool ReportTechnology { get; set; }

		#region Initialization
		protected override void initDefault()
		{
			base.initDefault();
			
			this.GuiScale = 1;
			
			this.Framerate = 120;
			this.FramerateBusySpinUsage = BusySpinMode.NotOnBattery;
			this.UnlimitedFramerate = false;
			this.VSync = true;
			
			this.ReportTechnology = true;
		}
	
		protected override void load(TaggableQueue<object, IkadnBaseObject> data)
		{
			base.load(data);
			          
			var wfSettignsData = data.Dequeue(WinformsSettingsTag).To<IkonComposite>();
			
			this.GuiScale = wfSettignsData[GuiScaleKey].To<float>();
			
			this.Framerate = wfSettignsData[FpsKey].To<int>();
			this.FramerateBusySpinUsage = (BusySpinMode)wfSettignsData[FpsBusyWaitKey].To<int>();
			this.UnlimitedFramerate = wfSettignsData[FpsUnlimitedKey].To<int>() >= 0;
			this.VSync = wfSettignsData[VSyncKey].To<int>() >= 0;
			
			this.ReportTechnology = wfSettignsData[ReportTechnologyKey].To<int>() >= 0;
		}
		#endregion
		
		#region Attribute keys
		const string WinformsSettingsTag = "winforms";
		const string FpsBusyWaitKey = "fpsBusyWait";
		const string FpsKey = "fps";
		const string FpsUnlimitedKey = "noFps";
		const string GuiScaleKey = "guiscale";
		const string ReportTechnologyKey = "reportTech";
		const string VSyncKey = "vsync";
		#endregion

		protected override void buildSaveData(IkadnWriter writer)
		{
			base.buildSaveData(writer);

			var settings = new IkonComposite(WinformsSettingsTag);
			settings.Add(GuiScaleKey, new IkonFloat(this.GuiScale));
			
			settings.Add(FpsBusyWaitKey, new IkonInteger((int)this.FramerateBusySpinUsage));
			settings.Add(FpsKey, new IkonInteger(this.Framerate));
			settings.Add(FpsUnlimitedKey, new IkonInteger(this.UnlimitedFramerate ? 1 : -1));
			settings.Add(VSyncKey, new IkonInteger(this.VSync ? 1 : -1));
			
			settings.Add(ReportTechnologyKey, new IkonInteger(this.ReportTechnology ? 1 : -1));
			settings.Compose(writer);
		}
	}
}
