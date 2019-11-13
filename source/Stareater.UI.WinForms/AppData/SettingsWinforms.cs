using System;
using System.Drawing;
using System.IO;
using Ikadn;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;

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
		
		public SettingsWinforms()
		{
			this.FileStorageRootPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Stareater/";
		}
		#endregion

		public string FileStorageRootPath { get; private set; }
		
		public float GuiScale { get; set; }
		
		public int Framerate { get; set; }
		public bool UnlimitedFramerate { get; set; }
		public bool VSync { get; set; }
		public BusySpinMode FramerateBusySpinUsage { get; set; }

		public string DataRootPath { get; set; }
		public string PluginRootPath { get; set; }

		public Font FormFont
		{
			get
			{
				return new Font(SystemFonts.DefaultFont.FontFamily, SystemFonts.DefaultFont.Size * GuiScale);
			}
		}
		
		public bool ReportTechnology { get; set; }
		public bool ShowScanRange { get; set; }

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
			this.ShowScanRange = false;

			this.DataRootPath = null;
		}
	
		protected override LabeledQueue<object, IkadnBaseObject> loadData()
		{
			LabeledQueue<object, IkadnBaseObject> data;

			if (File.Exists(SettingsFilePath))
				using (var parser = new IkonParser(new StreamReader(SettingsFilePath)))
					data = parser.ParseAll();
			else
				data = null;

			return data;
		}
		
		protected override void load(LabeledQueue<object, IkadnBaseObject> data)
		{
			base.load(data);
			          
			var wfSettignsData = data.Dequeue(WinformsSettingsTag).To<IkonComposite>();
			
			this.GuiScale = wfSettignsData[GuiScaleKey].To<float>();
			
			this.Framerate = wfSettignsData[FpsKey].To<int>();
			this.FramerateBusySpinUsage = (BusySpinMode)wfSettignsData[FpsBusyWaitKey].To<int>();
			this.UnlimitedFramerate = wfSettignsData[FpsUnlimitedKey].To<int>() >= 0;
			this.VSync = wfSettignsData[VSyncKey].To<int>() >= 0;
			
			this.ReportTechnology = wfSettignsData[ReportTechnologyKey].To<int>() >= 0;
			this.ShowScanRange = wfSettignsData[ShowScanRangeKey].To<int>() >= 0;
		}
		#endregion
		
		#region Attribute keys
		const string WinformsSettingsTag = "winforms";
		const string FpsBusyWaitKey = "fpsBusyWait";
		const string FpsKey = "fps";
		const string FpsUnlimitedKey = "noFps";
		const string GuiScaleKey = "guiscale";
		const string ReportTechnologyKey = "reportTech";
		const string ShowScanRangeKey = "showRadar";
		const string VSyncKey = "vsync";
		#endregion

		#region Saving
		public void Save()
		{
			var saveFile = new FileInfo(SettingsFilePath);
			saveFile.Directory.Create();

			using (var output = new StreamWriter(SettingsFilePath))
			{
				var writer = new IkadnWriter(output);
				buildSaveData(writer);
			}
		}
		
		protected override void buildSaveData(IkadnWriter writer)
		{
			base.buildSaveData(writer);

			var settings = new IkonComposite(WinformsSettingsTag)
			{
				{ GuiScaleKey, new IkonFloat(this.GuiScale) },

				{ FpsBusyWaitKey, new IkonInteger((int)this.FramerateBusySpinUsage) },
				{ FpsKey, new IkonInteger(this.Framerate) },
				{ FpsUnlimitedKey, new IkonInteger(this.UnlimitedFramerate ? 1 : -1) },
				{ VSyncKey, new IkonInteger(this.VSync ? 1 : -1) },

				{ ReportTechnologyKey, new IkonInteger(this.ReportTechnology ? 1 : -1) },
				{ ShowScanRangeKey, new IkonInteger(this.ShowScanRange ? 1 : -1) }
			};
			settings.Compose(writer);
		}
		
		private string SettingsFilePath {
			get {
				return this.FileStorageRootPath + "settings.txt";
			}
		}
		#endregion
	}
}
