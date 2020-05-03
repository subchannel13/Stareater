using System;
using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class SpecialEquipInfo
	{
		internal const string LangContext = "SpecialEquipment";
		
		internal SpecialEquipmentType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> vars;

		internal SpecialEquipInfo(SpecialEquipmentType type, int level, HullInfo shipHull)
		{
			this.Type = type;
			this.Level = level;
			
			this.vars = new Var(AComponentType.LevelKey, level).
				And(HullType.SizeKey, shipHull.Size).Get;
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
		
		public double MaxCount 
		{
			get
			{
				return this.Type.MaxCount.Evaluate(vars);
			}
		}
	}
}
