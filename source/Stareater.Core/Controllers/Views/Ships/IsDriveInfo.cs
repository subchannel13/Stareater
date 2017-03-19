using System;
using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Localization;

namespace Stareater.Controllers.Views.Ships
{
	public class IsDriveInfo
	{
		internal const string LangContext = "IsDrives";
		
		internal IsDriveType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> vars;
		
		internal IsDriveInfo(IsDriveType isDriveType, int level, IDictionary<string, double> shipVars)
		{
			this.Type = isDriveType;
			this.Level = level;
			
			this.vars = new Dictionary<string, double>(shipVars);
			this.vars[AComponentType.LevelKey] = level;
		}
		
		public string Name
		{ 
			get
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext].Name(this.Type.LanguageCode).Text(this.Level);
			}
		}
		
		public string ImagePath
		{
			get
			{
				return this.Type.ImagePath;
			}
		}
		
		public double Speed
		{
			get
			{
				return this.Type.Speed.Evaluate(vars);
			}
		}
	}
}
