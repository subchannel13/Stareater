using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Ikadn.Ikon.Types;
using Stareater.AppData.Expressions;
using Stareater.Controllers.Views;
using Stareater.GameData.Databases.Tables;
using Stareater.GameData.Reading;
using Stareater.GameLogic;
using Stareater.Utils;

namespace Stareater.GameData.Databases
{
	internal class StaticsDB
	{
		public Dictionary<string, BuildingType> Buildings { get; private set; }
		public ColonyFormulaSet ColonyFormulas { get; private set; }
		public List<Constructable> Constructables { get; private set; }
		public List<DevelopmentFocus> DevelopmentFocusOptions { get; private set; }
		public PlayerFormulaSet PlayerFormulas { get; private set; }
		public List<PredefinedDesign> PredeginedDesigns { get; private set; }
		public List<Technology> Technologies { get; private set; }
		
		public Dictionary<string, HullType> Hulls { get; private set; }
		public Dictionary<string, IsDriveType> IsDrives { get; private set; }
		
		public StaticsDB()
		{
			this.Buildings = new Dictionary<string, BuildingType>();
			this.Constructables = new List<Constructable>();
			this.DevelopmentFocusOptions = new List<DevelopmentFocus>();
			this.Hulls = new Dictionary<string, HullType>();
			this.IsDrives = new Dictionary<string, IsDriveType>();
			this.PredeginedDesigns = new List<PredefinedDesign>();
			this.Technologies = new List<Technology>();
		}
		
		public IEnumerable<double> Load(params string[] paths)
		{
			double progressScale = 1.0 / paths.Length;
			double fileReadWeight = 0.5 / paths.Length;
			double dataTranslateWeight = 0.5 / paths.Length;
			
			for(int i = 0; i < paths.Length; i++) {
				using (var parser = new Parser(new StreamReader(paths[i]))) {
					var dataSet = parser.ParseAll();
					double progressOffset = i * progressScale + fileReadWeight;
					yield return progressOffset;
					
					foreach (double p in Methods.ProgressReportHelper(progressOffset, dataTranslateWeight, dataSet.Count)) {
						var data = dataSet.Dequeue().To<IkonComposite>();
						
						switch((string)data.Tag) {
							case BuildingTag:
								Buildings.Add(data[GeneralCodeKey].To<string>(), loadBuilding(data));
								break;
							case ColonyFormulasTag:
								ColonyFormulas = loadColonyFormulas(data);
								break;
							case ConstructableTag:
								Constructables.Add(loadConstructable(data));
								break;
							case DevelopmentFocusesTag:
								DevelopmentFocusOptions.AddRange(loadFocusOptions(data));
								break;
							case DevelopmentTag:
								Technologies.Add(loadTech(data, TechnologyCategory.Development));
								break;
							case PlayerFormulasTag:
								PlayerFormulas = loadPlayerFormulas(data);
								break;
							case PredefinedDesignTag:
								PredeginedDesigns.Add(loadPredefDesign(data));
								break;
							case ResearchTag:
								Technologies.Add(loadTech(data, TechnologyCategory.Research));
								break;

							case HullTag:
								Hulls.Add(data[GeneralCodeKey].To<string>(), loadHull(data));
								break;
							case IsDriveTag:
								IsDrives.Add(data[GeneralCodeKey].To<string>(), loadIsDrive(data));
								break;
							default:
								throw new FormatException("Invalid game data object with tag " + data.Tag);
						}
						
						yield return p;
					}
				}
			}
			
			yield return 1;
		}
		
		#region Colony Formulas
		private ColonyFormulaSet loadColonyFormulas(IkonComposite data)
		{
			return new ColonyFormulaSet(
				data[ColonyMaxPopulation].To<Formula>(),
				loadDerivedStat(data[ColonyPopulationGrowth].To<IkonComposite>()),
				data[ColonyOrganization].To<Formula>(),
				loadPopulationActivity(data, ColonyFarming),
				loadPopulationActivity(data, ColonyGardening),
				loadPopulationActivity(data, ColonyMining),
				loadPopulationActivity(data, ColonyDevelopment),
				loadPopulationActivity(data, ColonyIndustry)
			);
		}
		
		private DerivedStatistic loadDerivedStat(IkonComposite data)
		{
			return new DerivedStatistic(
				data[DerivedStatBase].To<Formula>(),
				data[DerivedStatTotal].To<Formula>()
			);
		}
		
		private PopulationActivityFormulas loadPopulationActivity(IkonComposite data, string key)
		{
			return new PopulationActivityFormulas(
				data[key].To<IkonComposite>()[PopulationActivityImprovised].To<Formula>(),
				data[key].To<IkonComposite>()[PopulationActivityOrganized].To<Formula>()
			);
		}
		#endregion
		
		private PlayerFormulaSet loadPlayerFormulas(IkonComposite data)
		{
			return new PlayerFormulaSet(
				data[PlayerResearch].To<Formula>(),
				data[PlayerResearchFocusWeight].To<Formula>()
			);
		}
		
		#region Constructables
		private BuildingType loadBuilding(IkonComposite data)
		{
			return new BuildingType(
				data[GeneralNameKey].To<string>(),
				data[GeneralImageKey].To<string>()
			);
		}
		
		private Constructable loadConstructable(IkonComposite data)
		{
			return new Constructable(
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				false,
				data[GeneralImageKey].To<string>(),
				data[GeneralCodeKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()).ToArray(), 
				siteType(data[ConstructableSiteKey].To<string>()),
				data[ConstructableConditionKey].To<Formula>(),
				data[GeneralCostKey].To<Formula>(),
				data[ConstructableLimitKey].To<Formula>(),
				loadConstructionEffects(data[ConstructableEffectsKey].To<IEnumerable<IkonComposite>>()).ToArray()
			);
		}
		
		private SiteType siteType(string rawData)
		{
			switch(rawData.ToLower())
			{
				case SiteColony:
					return SiteType.Colony;
				case SiteSystem:
					return SiteType.StarSystem;
				default:
					throw new FormatException("Invalid building site type: " + rawData);
			}
		}
		
		private IEnumerable<IConstructionEffect> loadConstructionEffects(IEnumerable<IkonComposite> data)
		{
			foreach (var effectData in data) 
				switch (effectData.Tag.ToString().ToLower()) 
				{
					case ConstructionAddBuildingTag:
						yield return new ConstructionAddBuilding(
							effectData[AddBuildingBuildingId].To<string>(),
							effectData[AddBuildingQuantity].To<Formula>()
						);
						break;
					default:
						throw new FormatException("Invalid construction effect with tag " + effectData.Tag);
				}
			
			yield break;
		}
		#endregion

		private PredefinedDesign loadPredefDesign(IkonComposite data)
		{
			return new PredefinedDesign(
				data[DesignName].To<string>(),
				data[DesignHull].To<string>(),
				data[DesignHullImageIndex].To<int>(),
				data.Keys.Contains(DesignIsDrive)
			);
		}
		
		#region Ship components
		private HullType loadHull(IkonComposite data)
		{
			return new HullType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				data[HullImages].To<string[]>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				data[GeneralCostKey].To<Formula>(),
				data[HullSize].To<Formula>(),
				data[HullSpace].To<Formula>(),
				data[HullSizeIS].To<Formula>(),
				data[HullSizeReactor].To<Formula>(),
				data[HullSizeShield].To<Formula>(),
				data[HullArmorBase].To<Formula>(),
				data[HullArmorAbsorb].To<Formula>(),
				data[HullShieldBase].To<Formula>(),
				data[HullInertia].To<Formula>(),
				data[HullJamming].To<Formula>(),
				data[HullCloaking].To<Formula>(),
				data[HullSensors].To<Formula>()
			);
		}
		
		private IsDriveType loadIsDrive(IkonComposite data)
		{
			return new IsDriveType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				data[GeneralCostKey].To<Formula>(),
				data[IsDriveSpeed].To<Formula>(),
				data[IsDriveMinSize].To<Formula>()
			);
		}
		#endregion

		#region Technologies
		private IEnumerable<DevelopmentFocus> loadFocusOptions(IkonComposite data)
		{
			foreach(var array in data[FocusList].To<IkonArray>()) {
				double[] weights = array.To<double[]>();
				double sum = weights.Sum();
				yield return new DevelopmentFocus(weights.Select(x => x /sum).ToArray()); //TODO(later): possible div by 0
			}
		}
			
		private IEnumerable<Prerequisite> loadPrerequisites(IList<Ikadn.IkadnBaseObject> dataArray)
		{
			for(int i = 0; i < dataArray.Count; i += 2)
				yield return new Prerequisite(
					dataArray[i].To<string>(), 
					dataArray[i + 1].To<Formula>()
				);
		}
		
		private Technology loadTech(IkonComposite data, TechnologyCategory category)
		{
			return new Technology(
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				data[GeneralCodeKey].To<string>(),
				data[GeneralCostKey].To<Formula>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()).ToArray(),
				data[GeneralMaxLevelKey].To<int>(),
				category
			 );
		}
		#endregion
		
		#region Loading tags and keys
		private const string BuildingTag = "Building";
		private const string ColonyFormulasTag = "ColonyFormulas";
		private const string ConstructableTag = "Constructable";
		private const string DevelopmentFocusesTag = "DevelopmentFocusOptions";
		private const string DevelopmentTag = "DevelopmentTopic";
		private const string PlayerFormulasTag = "PlayerFormulas";
		private const string PredefinedDesignTag = "PredefinedDesign";
		private const string ResearchTag = "ResearchTopic";
		
		private const string HullTag = "Hull";
		private const string IsDriveTag = "IsDrive"; 
		
		
		private const string ColonyMaxPopulation = "maxPopulation";
		private const string ColonyPopulationGrowth = "populationGrowth";
		private const string ColonyOrganization = "organization";
		private const string ColonyDevelopment = "development";
		private const string ColonyFarming = "farming";
		private const string ColonyGardening = "gardening";
		private const string ColonyIndustry = "industry";
		private const string ColonyMining = "mining";
		
		private const string PlayerResearch = "research";
		private const string PlayerResearchFocusWeight = "focusedResearchWeight";

		private const string ConstructableCostKey = "cost";
		private const string ConstructableSiteKey = "site";
		private const string ConstructableConditionKey = "condition";
		private const string ConstructableLimitKey = "turnLimit";
		private const string ConstructableEffectsKey = "effects";
		private const string SiteColony = "colony";
		private const string SiteSystem = "system";
		
		private const string ConstructionAddBuildingTag = "addbuilding";
		private const string AddBuildingBuildingId = "buildingId";
		private const string AddBuildingQuantity = "quantity";
		
		private const string DerivedStatBase = "base";
		private const string DerivedStatTotal = "total";
		
		private const string DesignName = "name";
		private const string DesignIsDrive = "hasIsDrive";
		private const string DesignHull = "hull";
		private const string DesignHullImageIndex = "hullImageIndex";
		
		private const string FocusList = "list";
		
		private const string GeneralNameKey = "nameCode";
		private const string GeneralDescriptionKey = "descCode";
		private const string GeneralImageKey = "image";
		private const string GeneralCodeKey = "code";
		private const string GeneralPrerequisitesKey = "prerequisites";
		private const string GeneralMaxLevelKey = "maxLvl";
		private const string GeneralCostKey = "cost";
		
		private const string PopulationActivityImprovised = "improvised";
		private const string PopulationActivityOrganized = "organized";
		
		
		private const string HullImages = "images";
		private const string HullSize = "size";
		private const string HullSpace = "space";
		
		private const string HullSizeIS = "sizeIS";
		private const string HullSizeReactor = "sizeReactor";
		private const string HullSizeShield = "sizeShield";
		
		private const string HullArmorBase = "armorBase";
		private const string HullArmorAbsorb = "armorAbsorb";
		private const string HullShieldBase = "shieldBase";
		
		private const string HullInertia = "inertia";
		private const string HullJamming = "jamming";
		private const string HullCloaking = "cloaking";
		private const string HullSensors = "sensors";
		
		
		private const string IsDriveMinSize = "minSize";
		private const string IsDriveSpeed = "speed";
		#endregion
	}
}
