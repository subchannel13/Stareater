using System;
using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class SensorInfo
	{
		internal const string LangContext = "Sensors";

		internal SensorType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> vars;
		
		internal SensorInfo(SensorType type, int level)
		{
			this.Type = type;
			this.Level = level;
			
			this.vars = new Var(AComponentType.LevelKey, level).Get;
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
		
		public double Detection
		{
			get
			{
				return this.Type.Detection.Evaluate(vars);
			}
		}
	}
}
