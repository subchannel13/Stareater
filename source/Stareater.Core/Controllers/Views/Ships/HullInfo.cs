using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class HullInfo
	{
		internal const string LangContext = "Hulls";

		internal Component<HullType> Component { get; private set; }

		private readonly IDictionary<string, double> levelVar;

		internal HullInfo(Component<HullType> component)
		{
			this.Component = component;

			this.levelVar = new Var(AComponentType.LevelKey, component.Level).Get;
		}

		public string Name
		{
			get
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext].Name(this.Component.TypeInfo.LanguageCode).Text();
			}
		}

		public double Size
		{
			get
			{
				return this.Component.TypeInfo.Size;
			}
		}

		public double Cost
		{
			get
			{
				return this.Component.TypeInfo.Cost.Evaluate(levelVar);
			}
		}

		public ReadOnlyCollection<string> ImagePaths
		{
			get
			{
				return this.Component.TypeInfo.ImagePaths;
			}
		}

		public double HitPointsBase
		{
			get
			{
				return this.Component.TypeInfo.ArmorBase.Evaluate(levelVar);
			}
		}

		public double ArmorAbsorbBase
		{
			get
			{
				return this.Component.TypeInfo.ArmorAbsorption.Evaluate(levelVar);
			}
		}

		public double ShieldHpBase
		{
			get
			{
				return this.Component.TypeInfo.ShieldBase.Evaluate(levelVar);
			}
		}

		public double Space
		{
			get
			{
				return this.Component.TypeInfo.SpaceFree.Evaluate(levelVar);
			}
		}

		public double CloakingBase
		{
			get
			{
				return this.Component.TypeInfo.CloakingBase.Evaluate(levelVar);
			}
		}

		public double JammingBase
		{
			get
			{
				return this.Component.TypeInfo.JammingBase.Evaluate(levelVar);
			}
		}

		public double SensorsBase
		{
			get
			{
				return this.Component.TypeInfo.SensorsBase.Evaluate(levelVar);
			}
		}

		public double InertiaBase
		{
			get
			{
				return this.Component.TypeInfo.InertiaBase.Evaluate(levelVar);
			}
		}
	}
}
