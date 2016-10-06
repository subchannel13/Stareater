using System;
using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class HullInfo
	{
		internal const string LangContext = "Hulls";
		
		internal HullType Type { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> levelVar;
		
		internal HullInfo(HullType hullType, int level)
		{
			this.Type = hullType;
			this.Level = level;
			
			this.levelVar = new Var(AComponentType.LevelKey, level).Get;
		}
		
		public string Name 
		{ 
			get
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext][this.Type.NameCode].Text();
			}
		}
		
		public double Size
		{
			get
			{
				return this.Type.Size.Evaluate(levelVar);
			}
		}
		
		public double Cost
		{
			get
			{
				return this.Type.Cost.Evaluate(levelVar);
			}
		}
		
		public string[] ImagePaths
		{
			get
			{
				return this.Type.ImagePaths;
			}
		}
		
		public double HitPointsBase
		{
			get
			{
				return this.Type.ArmorBase.Evaluate(levelVar);
			}
		}
		
		public double ArmorAbsorbBase
		{
			get
			{
				return this.Type.ArmorAbsorption.Evaluate(levelVar);
			}
		}
		
		public double ShieldHpBase
		{
			get
			{
				return this.Type.ShieldBase.Evaluate(levelVar);
			}
		}
		
		public double IsDriveSize
		{
			get
			{
				return this.Type.SizeIS.Evaluate(levelVar);
			}
		}

		public double ReactorSize
		{
			get
			{
				return this.Type.SizeReactor.Evaluate(levelVar);
			}
		}
		
		public double ShieldSize
		{
			get
			{
				return this.Type.SizeShield.Evaluate(levelVar);
			}
		}
		
		public double Space
		{
			get
			{
				return this.Type.SpaceFree.Evaluate(levelVar);
			}
		}
		
		public double CloakingBase
		{
			get
			{
				return this.Type.CloakingBase.Evaluate(levelVar);
			}
		}
		
		public double JammingBase
		{
			get
			{
				return this.Type.JammingBase.Evaluate(levelVar);
			}
		}
		
		public double SensorsBase
		{
			get
			{
				return this.Type.SensorsBase.Evaluate(levelVar);
			}
		}
		
		public double InertiaBase
		{
			get
			{
				return this.Type.InertiaBase.Evaluate(levelVar);
			}
		}
	}
}
