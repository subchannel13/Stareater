using Stareater.GameData.Ships;
using Stareater.GameLogic.Combat;
using Stareater.Localization;
using System.Collections.Generic;

namespace Stareater.Controllers.Views.Ships
{
	public class IsDriveInfo
	{
		internal const string LangContext = "IsDrives";
		
		internal IsDriveType Type { get; private set; }
		internal int Level { get; private set; }

		private readonly DesignStats designStats;

		internal IsDriveInfo(IsDriveType isDriveType, int level, DesignStats designStats)
		{
			this.Type = isDriveType;
			this.Level = level;
			this.designStats = designStats;
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
				//TODO(v0.9) make variables a member instead of designStats
				return this.Type.Speed.Evaluate(new Dictionary<string, double>
				{
					[AComponentType.LevelKey] = this.Level,
					[IsDriveType.SizeKey] = designStats.IsDriveSize,
					["totalPower"] = designStats.GalaxyPower,
				});
			}
		}
	}
}
