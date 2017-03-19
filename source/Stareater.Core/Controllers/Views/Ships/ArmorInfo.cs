using System;
using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class ArmorInfo
	{
		internal const string LangContext = "Armors";
		
		internal ArmorType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> vars;
		
		internal ArmorInfo(ArmorType type, int level)
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
		
		public double Absorption
		{
			get
			{
				return this.Type.Absorption.Evaluate(vars);
			}
		}
		
		public double AbsorptionMax
		{
			get
			{
				return this.Type.AbsorptionMax.Evaluate(vars);
			}
		}
		
		public double ArmorFactor
		{
			get
			{
				return this.Type.ArmorFactor.Evaluate(vars);
			}
		}
	}
}
