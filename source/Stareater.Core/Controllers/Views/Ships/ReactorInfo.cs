using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.GameLogic.Combat;
using Stareater.Localization;

namespace Stareater.Controllers.Views.Ships
{
	public class ReactorInfo
	{
		internal const string LangContext = "Reactors";
		
		internal ReactorType Type { get; private set; }
		internal int Level { get; private set; }

		private readonly DesignStats designStats;

		internal ReactorInfo(ReactorType type, int level, DesignStats designStats)
		{
			this.Type = type;
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

		public double Power
		{
			get
			{
				//TODO(v0.9) make variables a member instead of designStats
				return this.Type.Power.Evaluate(new Dictionary<string, double>
				{
					[AComponentType.LevelKey] = this.Level,
					[ReactorType.SizeKey] = designStats.ReactorSize
				});
			}
		}
	}
}
