using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Ikadn.Ikon.Types;
using Stareater.AppData.Expressions;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.GameData.Reading;
using Stareater.GameData.Ships;
using Stareater.GameLogic;
using Stareater.Utils;

namespace Stareater.GameData.Databases
{
	internal class StaticsDB
	{
		public Dictionary<string, BuildingType> Buildings { get; private set; }
		public ColonyFormulaSet ColonyFormulas { get; private set; }
		public List<PredefinedDesign> ColonyShipDesigns { get; private set; }
		public List<Constructable> Constructables { get; private set; }
		public List<DevelopmentFocus> DevelopmentFocusOptions { get; private set; }
		public PlayerFormulaSet PlayerFormulas { get; private set; }
		public List<PredefinedDesign> PredeginedDesigns { get; private set; }
		public ShipFormulaSet ShipFormulas { get; private set; }
		public List<PredefinedDesign> SystemColonizerDesigns { get; private set; }
		public List<Technology> Technologies { get; private set; }
		public Dictionary<string, BodyTraitType> Traits { get; private set; }
		
		public Dictionary<string, ArmorType> Armors { get; private set; }
		public Dictionary<string, HullType> Hulls { get; private set; }
		public Dictionary<string, IsDriveType> IsDrives { get; private set; }
		public Dictionary<string, MissionEquipmentType> MissionEquipment { get; private set; }
		public Dictionary<string, ReactorType> Reactors { get; private set; }
		public Dictionary<string, SensorType> Sensors { get; private set; }
		public Dictionary<string, ShieldType> Shields { get; private set; }
		public Dictionary<string, SpecialEquipmentType> SpecialEquipment { get; private set; }
		public Dictionary<string, ThrusterType> Thrusters { get; private set; }
		
		public StaticsDB()
		{
			this.Armors = new Dictionary<string, ArmorType>();
			this.Buildings = new Dictionary<string, BuildingType>();
			this.Constructables = new List<Constructable>();
			this.ColonyShipDesigns = new List<PredefinedDesign>();
			this.DevelopmentFocusOptions = new List<DevelopmentFocus>();
			this.Hulls = new Dictionary<string, HullType>();
			this.IsDrives = new Dictionary<string, IsDriveType>();
			this.Reactors = new Dictionary<string, ReactorType>();
			this.MissionEquipment = new Dictionary<string, MissionEquipmentType>();
			this.Sensors = new Dictionary<string, SensorType>();
			this.Shields = new Dictionary<string, ShieldType>();
			this.SpecialEquipment = new Dictionary<string, SpecialEquipmentType>();
			this.SystemColonizerDesigns = new List<PredefinedDesign>();
			this.Thrusters = new Dictionary<string, ThrusterType>();
			this.Traits = new Dictionary<string, BodyTraitType>();
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
							case ColonizersTag:
								loadColonizers(data.To<IkonComposite>());
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
							case ShipFormulasTag:
								ShipFormulas = loadShipFormulas(data);
								break;
							case TraitTag:
								Traits.Add(data[GeneralCodeKey].To<string>(), loadTrait(data));
								break;

							case ArmorTag:
								Armors.Add(data[GeneralCodeKey].To<string>(), loadArmor(data));
								break;
							case HullTag:
								Hulls.Add(data[GeneralCodeKey].To<string>(), loadHull(data));
								break;
							case IsDriveTag:
								IsDrives.Add(data[GeneralCodeKey].To<string>(), loadIsDrive(data));
								break;
							case MissionEquipmentTag:
								MissionEquipment.Add(data[GeneralCodeKey].To<string>(), loadMissionEquiptment(data));
								break;
							case ReactorTag:
								Reactors.Add(data[GeneralCodeKey].To<string>(), loadReactor(data));
								break;
							case SensorTag:
								Sensors.Add(data[GeneralCodeKey].To<string>(), loadSensor(data));
								break;
							case ShieldTag:
								Shields.Add(data[GeneralCodeKey].To<string>(), loadShield(data));
								break;
							case SpecialEquipmentTag:
								SpecialEquipment.Add(data[GeneralCodeKey].To<string>(), loadSpecialEquiptment(data));
								break;
							case ThrusterTag:
								Thrusters.Add(data[GeneralCodeKey].To<string>(), loadThruster(data));
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
				data[ColonizationPopulationThreshold].To<Formula>(),
				data[UncolonizedMaxPopulation].To<Formula>(),
				data[ColonyFarmFields].To<Formula>(),
				data[ColonyEnvironment].To<Formula>(),
				data[ColonyMaxPopulation].To<Formula>(),
				loadDerivedStat(data[ColonyPopulationGrowth].To<IkonComposite>()),
				data[ColonyOrganization].To<Formula>(),
				data[ColonySpaceliftFactor].To<Formula>(),
				loadPopulationActivity(data, ColonyFarming),
				loadPopulationActivity(data, ColonyGardening),
				loadPopulationActivity(data, ColonyMining),
				loadPopulationActivity(data, ColonyDevelopment),
				loadPopulationActivity(data, ColonyIndustry),
				data[ColonyPopulationHitPoints].To<Formula>()
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

		private ShipFormulaSet loadShipFormulas(IkonComposite data)
		{
			var colonizerBuildings = new Dictionary<string, Formula>();
			var buildingData = data[ShipColonyBuildings].To<IkonComposite>();
			foreach(var buildingId in buildingData.Keys)
				colonizerBuildings.Add(buildingId, buildingData[buildingId].To<Formula>());
					
			return new ShipFormulaSet(
				data[ShipCloaking].To<Formula>(),
				data[ShipCombatSpeed].To<Formula>(),
				data[ShipDetection].To<Formula>(),
				data[ShipEvasion].To<Formula>(),
				data[ShipHitPoints].To<Formula>(),
				data[ShipJamming].To<Formula>(),
				data[ShipColonyPopulation].To<Formula>(),
				colonizerBuildings,
				data[ShipNaturalCloakBonus].To<Formula>().Evaluate(null),
				data[ShipSensorRangePenalty].To<Formula>().Evaluate(null)
			);
		}

		private BodyTraitType loadTrait(IkonComposite data)
		{
			return new BodyTraitType(
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				data[GeneralCodeKey].To<string>()
			);
		}
		
		#region Constructables
		private BuildingType loadBuilding(IkonComposite data)
		{
			return new BuildingType(
				data[GeneralNameKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				data[BuildingHitPointsKey].To<Formula>()
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
				false,
				data[ConstructableStockpileKey].To<string>(),
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

		private void loadColonizers(IkonComposite data)
		{
			foreach(var designData in data[ColonizerInterstellar].To<IEnumerable<IkonComposite>>())
				this.ColonyShipDesigns.Add(loadPredefDesign(designData));
			
			foreach(var designData in data[ColonizerSystem].To<IEnumerable<IkonComposite>>())
				this.SystemColonizerDesigns.Add(loadPredefDesign(designData));
		}
		
		private PredefinedDesign loadPredefDesign(IkonComposite data)
		{
			return new PredefinedDesign(
				data[DesignName].To<string>(),
				data[DesignHull].To<string>(),
				data[DesignHullImageIndex].To<int>(),
				data.Keys.Contains(DesignIsDrive),
				data.Keys.Contains(DesignShield) ? data[DesignShield].To<string>() : null,
				loadDesignMissionEquipment(data[DesignMissionEquipment].To<IkonArray>()),
				loadDesignSpecialEquipment(data[DesignSpecialEquipment].To<IkonArray>())
			);
		}
		
		private List<KeyValuePair<string, int>> loadDesignMissionEquipment(IList<Ikadn.IkadnBaseObject> data)
		{
			var result = new List<KeyValuePair<string, int>>();
			
			for(int i = 0; i < data.Count / 2; i++)
				result.Add(new KeyValuePair<string, int>(data[i * 2].To<string>(), data[i * 2 + 1].To<int>()));
			
			return result;
		}
		
		private Dictionary<string, int> loadDesignSpecialEquipment(IList<Ikadn.IkadnBaseObject> data)
		{
			var result = new Dictionary<string, int>();
			
			for(int i = 0; i < data.Count / 2; i++)
				result.Add(data[i * 2].To<string>(), data[i * 2 + 1].To<int>());
			
			return result;
		}
		
		#region Ship components
		private ArmorType loadArmor(IkonComposite data)
		{
			return new ArmorType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				data[ArmorFactor].To<Formula>(),
				data[ArmorAbsorb].To<Formula>(),
				data[ArmorAbsorbMax].To<Formula>()
			);
		}

		private IEnumerable<AAbilityType> loadEquipmentAbilities(IEnumerable<IkonComposite> data)
		{
			foreach(var abilityData in data)
			{
				switch(abilityData.Tag.ToString())
				{
					case DirectShotTag:
						yield return new DirectShootAbility(
							abilityData[GeneralImageKey].To<string>(),
							abilityData[DirectShootFirepower].To<Formula>(),
							abilityData[DirectShootAccuracy].To<Formula>(),
							abilityData[DirectShootRange].To<Formula>(),
							abilityData[DirectShootEnergyCost].To<Formula>(),
							abilityData.ToOrDefault(DirectShootAccuracyRangePenalty, new Formula(0)),
							abilityData.ToOrDefault(DirectShootArmorEfficiency, new Formula(1)),
							abilityData.ToOrDefault(DirectShootShieldEfficiency, new Formula(1)),
							abilityData.ToOrDefault(DirectShootPlanetEfficiency, new Formula(1))
						);
						break;
					default:
						throw new FormatException("Invalid construction effect with tag " + abilityData.Tag);
				}
			}
		}
		
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
		
		private MissionEquipmentType loadMissionEquiptment(IkonComposite data)
		{
			return new MissionEquipmentType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				data[GeneralCostKey].To<Formula>(),
				data[SpecialEquipmentSizeKey].To<Formula>(),
				loadEquipmentAbilities(data[MissionEquipmentAbilitiesKey].To<IEnumerable<IkonComposite>>()).ToArray()
			);
		}
		
		private ReactorType loadReactor(IkonComposite data)
		{
			return new ReactorType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				data[ReactorPower].To<Formula>(),
				data[ReactorMinSize].To<Formula>()
			);
		}
		
		private SensorType loadSensor(IkonComposite data)
		{
			return new SensorType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				data[SensorDetection].To<Formula>()
			);
		}
		
		private ShieldType loadShield(IkonComposite data)
		{
			return new ShieldType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				data[GeneralCostKey].To<Formula>(),
				data[ShieldHpFactor].To<Formula>(),
				data[ShieldRegeneration].To<Formula>(),
				data[ShieldThickness].To<Formula>(),
				data[ShieldReduction].To<Formula>(),
				data[ShieldCloaking].To<Formula>(),
				data[ShieldJamming].To<Formula>(),
				data[ShieldPower].To<Formula>()
			);
		}
		
		private SpecialEquipmentType loadSpecialEquiptment(IkonComposite data)
		{
			return new SpecialEquipmentType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				!data.Keys.Contains(SpecialEquipmentCannotPick),
				data[GeneralCostKey].To<Formula>(),
				data[SpecialEquipmentSizeKey].To<Formula>(),
				data[SpecialEquipmentMaxCountKey].To<Formula>()
			);
		}

		private ThrusterType loadThruster(IkonComposite data)
		{
			return new ThrusterType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				data[ThrusterEvasion].To<Formula>(),
				data[ThrusterSpeed].To<Formula>()
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
		private const string ColonizersTag = "Colonizers";
		private const string ConstructableTag = "Constructable";
		private const string DevelopmentFocusesTag = "DevelopmentFocusOptions";
		private const string DevelopmentTag = "DevelopmentTopic";
		private const string PlayerFormulasTag = "PlayerFormulas";
		private const string PredefinedDesignTag = "PredefinedDesign";
		private const string ResearchTag = "ResearchTopic";
		private const string ShipFormulasTag = "ShipFormulas";
		private const string TraitTag = "Trait";
		
		private const string ArmorTag = "Armor";
		private const string HullTag = "Hull";
		private const string IsDriveTag = "IsDrive"; 
		private const string MissionEquipmentTag = "MissionEquipment";
		private const string ReactorTag = "Reactor";
		private const string SensorTag = "Sensor";
		private const string ShieldTag = "Shield";
		private const string SpecialEquipmentTag = "SpecialEquipment";
		private const string ThrusterTag = "Thruster";
		
		private const string DirectShotTag = "DirectShot";
		
		private const string ColonizationPopulationThreshold = "colonizationPopThreshold";
		private const string ColonyDevelopment = "development";
		private const string ColonyEnvironment = "environment";
		private const string ColonyFarmFields = "farmFields";
		private const string ColonyFarming = "farming";
		private const string ColonyGardening = "gardening";
		private const string ColonyIndustry = "industry";
		private const string ColonyMaxPopulation = "maxPopulation";
		private const string ColonyMining = "mining";
		private const string ColonyOrganization = "organization";
		private const string ColonyPopulationGrowth = "populationGrowth";
		private const string ColonyPopulationHitPoints = "popHp";
		private const string ColonySpaceliftFactor = "spaceliftFactor";
		private const string UncolonizedMaxPopulation = "uncolonizedMaxPopulation";
		
		private const string PlayerResearch = "research";
		private const string PlayerResearchFocusWeight = "focusedResearchWeight";
		
		private const string ShipCloaking = "cloaking";
		private const string ShipCombatSpeed = "combatSpeed";
		private const string ShipDetection = "detection";
		private const string ShipEvasion = "evasion";
		private const string ShipHitPoints = "hitPoints";
		private const string ShipJamming = "jamming";
		private const string ShipColonyPopulation = "colonyPop";
		private const string ShipColonyBuildings = "colonyBuildings";
		private const string ShipNaturalCloakBonus = "naturalCloakBonus";
		private const string ShipSensorRangePenalty = "sensorRangePenalty";

		private const string BuildingHitPointsKey = "hitPoints";
		
		private const string ConstructableCostKey = "cost";
		private const string ConstructableSiteKey = "site";
		private const string ConstructableConditionKey = "condition";
		private const string ConstructableLimitKey = "turnLimit";
		private const string ConstructableEffectsKey = "effects";
		private const string ConstructableStockpileKey = "stockpile";
		private const string SiteColony = "colony";
		private const string SiteSystem = "system";
		
		private const string ConstructionAddBuildingTag = "addbuilding";
		private const string AddBuildingBuildingId = "buildingId";
		private const string AddBuildingQuantity = "quantity";
		
		private const string ColonizerInterstellar = "interstellar";
		private const string ColonizerSystem = "system";
		
		private const string DerivedStatBase = "base";
		private const string DerivedStatTotal = "total";
		
		private const string DesignName = "name";
		private const string DesignIsDrive = "hasIsDrive";
		private const string DesignHull = "hull";
		private const string DesignHullImageIndex = "hullImageIndex";
		private const string DesignShield = "shield";
		private const string DesignMissionEquipment = "equipment";
		private const string DesignSpecialEquipment = "specials";
		private const string DesignType = "type";
		
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
		
		
		private const string ArmorAbsorb = "reduction";
		private const string ArmorAbsorbMax = "reductionMax";
		private const string ArmorFactor = "armorFactor";
		
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
		
		private const string MissionEquipmentAbilitiesKey = "abilities";
		private const string MissionEquipmentSizeKey = "size";
		
		private const string ReactorMinSize = "minSize";
		private const string ReactorPower = "power";

		private const string SensorDetection = "detection";
		
		private const string ShieldCloaking = "cloaking";
		private const string ShieldJamming = "jamming";
		private const string ShieldHpFactor = "shieldFactor";
		private const string ShieldPower = "power"; //TODO(v0.5) replace with equipment power
		private const string ShieldReduction = "reduction";
		private const string ShieldRegeneration = "restoration";
		private const string ShieldThickness = "thickness";
		
		private const string SpecialEquipmentCannotPick = "cannotPick";
		private const string SpecialEquipmentMaxCountKey = "maxCount";
		private const string SpecialEquipmentSizeKey = "size";
		
		private const string ThrusterEvasion = "evasion";
		private const string ThrusterSpeed = "speed";
		
		private const string DirectShootAccuracy = "accuracy";
		private const string DirectShootAccuracyRangePenalty = "accuracyRangePenalty";
		private const string DirectShootArmorEfficiency = "armorEfficiency";
		private const string DirectShootEnergyCost = "energyCost";
		private const string DirectShootFirepower = "firePower";
		private const string DirectShootPlanetEfficiency = "planetEfficiency";
		private const string DirectShootRange = "range";
		private const string DirectShootShieldEfficiency = "shieldEfficiency";
		#endregion
	}
}
