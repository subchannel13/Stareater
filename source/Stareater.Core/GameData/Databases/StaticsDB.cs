using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Ikadn.Ikon.Types;
using Stareater.AppData.Expressions;
using Stareater.GameData.Databases.Tables;
using Stareater.GameData.Reading;
using Stareater.GameData.Ships;
using Stareater.Utils;
using Stareater.Galaxy.BodyTraits;
using Stareater.GameData.Construction;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases
{
	//TODO(later) let composite keys ignore case
	class StaticsDB
	{
		public Dictionary<string, BuildingType> Buildings { get; private set; }
		public ColonyFormulaSet ColonyFormulas { get; private set; }
		public List<ConstructableType> Constructables { get; private set; }
		public List<DevelopmentFocus> DevelopmentFocusOptions { get; private set; }
		public Dictionary<string, DevelopmentRequirement> DevelopmentRequirements { get; private set; }
		public List<DevelopmentTopic> DevelopmentTopics { get; private set; }
		public Dictionary<string, PredefinedDesign> NativeDesigns { get; private set; }
		public Dictionary<PlanetType, PlanetForumlaSet> PlanetForumlas { get; private set; }
		public PlayerFormulaSet PlayerFormulas { get; private set; }
		public List<DesignTemplate> PredeginedDesigns { get; private set; }
		public List<ResearchTopic> ResearchTopics { get; private set; }
		public ShipFormulaSet ShipFormulas { get; private set; }
		public StellarisFormulaSet StellarisFormulas { get; private set; }
		public List<SystemPolicy> Policies { get; private set; }
		public Dictionary<string, PlanetTraitType> PlanetTraits { get; private set; }
		public Dictionary<string, StarTraitType> StarTraits { get; private set; }

		public Dictionary<string, ArmorType> Armors { get; private set; }
		public Dictionary<string, HullType> Hulls { get; private set; }
		public Dictionary<string, IsDriveType> IsDrives { get; private set; }
		public Dictionary<string, MissionEquipmentType> MissionEquipment { get; private set; }
		public Dictionary<string, ReactorType> Reactors { get; private set; }
		public Dictionary<string, SensorType> Sensors { get; private set; }
		public Dictionary<string, ShieldType> Shields { get; private set; }
		public Dictionary<string, SpecialEquipmentType> SpecialEquipment { get; private set; }
		public Dictionary<string, ThrusterType> Thrusters { get; private set; }

		private StaticsDB()
		{
			this.Armors = new Dictionary<string, ArmorType>();
			this.Hulls = new Dictionary<string, HullType>();
			this.IsDrives = new Dictionary<string, IsDriveType>();
			this.Reactors = new Dictionary<string, ReactorType>();
			this.MissionEquipment = new Dictionary<string, MissionEquipmentType>();
			this.Sensors = new Dictionary<string, SensorType>();
			this.Shields = new Dictionary<string, ShieldType>();
			this.SpecialEquipment = new Dictionary<string, SpecialEquipmentType>();
			this.Thrusters = new Dictionary<string, ThrusterType>();

			this.NativeDesigns = new Dictionary<string, PredefinedDesign>();
			this.PredeginedDesigns = new List<DesignTemplate>();

			this.Buildings = new Dictionary<string, BuildingType>();
			this.Constructables = new List<ConstructableType>();
			this.DevelopmentFocusOptions = new List<DevelopmentFocus>();
			this.DevelopmentRequirements = new Dictionary<string, DevelopmentRequirement>();
			this.DevelopmentTopics = new List<DevelopmentTopic>();
			this.PlanetForumlas = new Dictionary<PlanetType, PlanetForumlaSet>();
			this.ResearchTopics = new List<ResearchTopic>();
			this.Policies = new List<SystemPolicy>();
			this.PlanetTraits = new Dictionary<string, PlanetTraitType>();
			this.StarTraits = new Dictionary<string, StarTraitType>();
		}
		
		public static StaticsDB Load(IEnumerable<TracableStream> dataSources)
		{
			//TODO(v0.8) update IKON with HasNext with tag
			//TODO(v0.8) support nested subformulas
			var subformulas = new Dictionary<string, Formula>();
			foreach (var source in dataSources)
			{
				try
				{
					var queue = new Parser(source.Stream).ParseAll();

					while (queue.CountOf("Subformulas") > 0)
					{
						var data = queue.Dequeue("Subformulas").To<IkonComposite>();
						foreach (var key in data.Keys)
							subformulas[key] = data[key].To<Formula>();
					}
				}
				catch (IOException e)
				{
					throw new IOException(source.SourceInfo, e);
				}
				catch (FormatException e)
				{
					throw new FormatException(source.SourceInfo, e);
				}
			}
			subformulas = ExpressionParser.ResloveSubformulaNesting(subformulas);

			var db = new StaticsDB();
			
			foreach(var source in dataSources) 
			{
				var parser = new Parser(source.Stream, subformulas);
				try
				{
					foreach (var data in parser.ParseAll().Select(x => x.Value.To<IkonComposite>())) 
					{
						switch((string)data.Tag) {
							case "Building":
								db.Buildings.Add(data[GeneralCodeKey].To<string>(), loadBuilding(data));
								break;
							case "ColonyFormulas":
								db.ColonyFormulas = loadColonyFormulas(data);
								break;
							case "Constructable":
								db.Constructables.Add(loadConstructable(data));
								break;
							case "DevelopmentFocusOptions":
								db.DevelopmentFocusOptions.AddRange(loadFocusOptions(data));
								break;
							case "DevelopmentTopic":
								db.DevelopmentTopics.Add(loadDevelopmentTopic(data));
								break;
							case "Subformulas":
								//TODO(v0.8) remove after IKON update
								break;
							case "Natives":
								db.loadNatives(data.To<IkonComposite>());
								break;
							case "PlanetFormulas":
								var formulaSet = loadPlanetFormulas(data);
                                db.PlanetForumlas[formulaSet.Key] = formulaSet.Value;
								break;
							case "PlayerFormulas":
								db.PlayerFormulas = loadPlayerFormulas(data);
								break;
							case "SystemPolicy":
								db.Policies.Add(loadPolicy(data));
								break;
							case "PredefinedDesign":
								db.PredeginedDesigns.Add(loadDesignTemplate(data));
								break;
							case "ResearchTopic":
								db.ResearchTopics.Add(loadResearchTopic(data));
								break;
							case "ShipFormulas":
								db.ShipFormulas = loadShipFormulas(data);
								break;
							case "StarFormulas":
								db.StellarisFormulas = loadStarFormulas(data);
								break;
							case "PlanetTrait":
								db.PlanetTraits.Add(data[GeneralCodeKey].To<string>(), loadPlanetTrait(data));
								break;
							case "StarTrait":
								db.StarTraits.Add(data[GeneralCodeKey].To<string>(), loadStarTrait(data));
								break;

							case ArmorTag:
								db.Armors.Add(data[GeneralCodeKey].To<string>(), loadArmor(data));
								break;
							case HullTag:
								db.Hulls.Add(data[GeneralCodeKey].To<string>(), loadHull(data));
								break;
							case IsDriveTag:
								db.IsDrives.Add(data[GeneralCodeKey].To<string>(), loadIsDrive(data));
								break;
							case MissionEquipmentTag:
								db.MissionEquipment.Add(data[GeneralCodeKey].To<string>(), loadMissionEquiptment(data));
								break;
							case ReactorTag:
								db.Reactors.Add(data[GeneralCodeKey].To<string>(), loadReactor(data));
								break;
							case SensorTag:
								db.Sensors.Add(data[GeneralCodeKey].To<string>(), loadSensor(data));
								break;
							case ShieldTag:
								db.Shields.Add(data[GeneralCodeKey].To<string>(), loadShield(data));
								break;
							case SpecialEquipmentTag:
								db.SpecialEquipment.Add(data[GeneralCodeKey].To<string>(), loadSpecialEquiptment(data));
								break;
							case ThrusterTag:
								db.Thrusters.Add(data[GeneralCodeKey].To<string>(), loadThruster(data));
								break;
							default:
								throw new FormatException("Invalid game data object with tag " + data.Tag);
						}
					}
				}
				catch (IOException e)
				{
					throw new IOException(source.SourceInfo, e);
				}
				catch(FormatException e)
				{
					throw new FormatException(source.SourceInfo, e);
				}
			}
			
			foreach(var research in db.ResearchTopics)
				for(int level = 0; level <= research.MaxLevel; level++)
					foreach(var unlock in research.Unlocks[level])
						db.DevelopmentRequirements[unlock] = new DevelopmentRequirement(research.IdCode, level);
			
			return db;
		}

		#region Colony Formulas
		private static ColonyFormulaSet loadColonyFormulas(IkonComposite data)
		{
			return new ColonyFormulaSet(
				data[ColonizationPopulationThreshold].To<Formula>(),
				data[UncolonizedMaxPopulation].To<Formula>(),
				data[ColonyVictoryWorth].To<Formula>(),
                data[ColonyFarmFields].To<Formula>(),
				data[ColonyEnvironment].To<Formula>(),
				data[ColonyMaxPopulation].To<Formula>(),
				loadDerivedStat(data[ColonyPopulationGrowth].To<IkonComposite>()),
				data[ColonyEmigrants].To<Formula>(),
				data[ColonyOrganization].To<Formula>(),
				data[ColonySpaceliftFactor].To<Formula>(),
				data[ColonyDesirability].To<Formula>(),
				loadPopulationActivity(data, ColonyFarming),
				loadPopulationActivity(data, ColonyGardening),
				loadPopulationActivity(data, ColonyMining),
				loadPopulationActivity(data, ColonyDevelopment),
				loadPopulationActivity(data, ColonyIndustry),
				data[ColonyFuelProduction].To<Formula>(),
				data[ColonyFuelCost].To<Formula>(),
				data[ColonyRepairPoints].To<Formula>(),
				data[ColonyPopulationHitPoints].To<Formula>(),
				data[ColonyMaintenanceLimit].To<Formula>()
			);
		}
		
		private static DerivedStatistic loadDerivedStat(IkonComposite data)
		{
			return new DerivedStatistic(
				data[DerivedStatBase].To<Formula>(),
				data[DerivedStatTotal].To<Formula>()
			);
		}
		
		private static PopulationActivityFormulas loadPopulationActivity(IkonComposite data, string key)
		{
			return new PopulationActivityFormulas(
				data[key].To<IkonComposite>()[PopulationActivityImprovised].To<Formula>(),
				data[key].To<IkonComposite>()[PopulationActivityOrganized].To<Formula>(),
                data[key].To<IkonComposite>()[PopulationActivityOrganizationFactor].To<Formula>()
			);
		}
		#endregion

		private static KeyValuePair<PlanetType, PlanetForumlaSet> loadPlanetFormulas(IkonComposite data)
		{
			PlanetType key;
			switch(data[PlanetTypeKey].To<string>())
			{
				case PlanetTypeAsteroid:
					key = PlanetType.Asteriod;
					break;
				case PlanetTypeGasGiant:
					key = PlanetType.GasGiant;
					break;
				case PlanetTypeRock:
					key = PlanetType.Rock;
					break;
				default:
					throw new FormatException("Unknown planet type " + data[PlanetTypeKey].To<string>());
			}

			return new KeyValuePair<PlanetType, PlanetForumlaSet>(
				key,
				new PlanetForumlaSet(
					data[PlanetBaseTraits].To<string[]>(),
					data[PlanetBestTraits].To<string[]>(),
					data[PlanetWorstTraits].To<string[]>(),
					data[PlanetUnchangeableTraits].To<string[]>(),
					data[PlanetStartingScore].To<Formula>(),
					data[PlanetPotentialScore].To<Formula>()
			));
		}

        private static PlayerFormulaSet loadPlayerFormulas(IkonComposite data)
		{
			return new PlayerFormulaSet(
				data[PlayerResearchFocusWeight].To<Formula>()
			);
		}

		private static ShipFormulaSet loadShipFormulas(IkonComposite data)
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
				data[ShipScaneRange].To<Formula>(),
				data[ShipColonyPopulation].To<Formula>(),
				colonizerBuildings,
				data[ShipReactorSize].To<Formula>(),
				data[ShipShieldSize].To<Formula>(),
				data[ShipNaturalCloakBonus].To<Formula>().Evaluate(null),
				data[ShipSensorRangePenalty].To<Formula>().Evaluate(null),
				data[ShipRepairCostFactor].To<Formula>().Evaluate(null),
				data[ShipLevelRefitCost].To<Formula>(),
				data[ShipArmorCostPortion].To<Formula>().Evaluate(null),
				data[ShipReactorCostPortion].To<Formula>().Evaluate(null),
				data[ShipSensorCostPortion].To<Formula>().Evaluate(null),
				data[ShipThrustersCostPortion].To<Formula>().Evaluate(null),
				data[ShipFuelUsage].To<Formula>(),
				data[ShipWormholeSpeed].To<Formula>()
			);
		}
		private static StellarisFormulaSet loadStarFormulas(IkonComposite data)
		{
			return new StellarisFormulaSet(
				data["scanRange"].To<Formula>()
			);
		}
		
		private static SystemPolicy loadPolicy(IkonComposite data)
		{
			return new SystemPolicy(
				data[GeneralLangKey].To<string>(),
				data[GeneralCodeKey].To<string>(),
				data[PolicySpendingRatioKey].To<double>(),
				data[PolicyBuildingQueueKey].To<string[]>()
			);
		}

		private static PlanetTraitType loadPlanetTrait(IkonComposite data)
		{
			return new PlanetTraitType(
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				data[GeneralCodeKey].To<string>(),
				data[TraitMaintenanceKey].To<Formula>()
			);
		}
		private static StarTraitType loadStarTrait(IkonComposite data)
		{
			return new StarTraitType(
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				data[GeneralCodeKey].To<string>(),
				loadTraitEffect(data[TraitEffectKey].To<IkonComposite>())
			);
		}

		private static ITraitEffectType loadTraitEffect(IkonComposite data)
		{
			switch ((string)data.Tag)
			{
				case AfflictTraitTag:
					return new EffectTypeAfflictPlanets(
						data[AfflictionListKey].To<IEnumerable<IkonComposite>>().
						Select(x => new Affliction(x[AfflictionTraitKey].To<string>(), x[AfflictionConditionKey].To<Formula>())).
						ToArray()
					);
				case TemporaryTraitTag:
					return new EffectTypeTemporary(data[DurationTraitKey].To<Formula>().Evaluate(null));
				default:
					throw new FormatException("Unknown trait effect type " + data.Tag);
			}
		}
		
		#region Constructables
		private static BuildingType loadBuilding(IkonComposite data)
		{
			return new BuildingType(
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				data[BuildingHitPointsKey].To<Formula>()
			);
		}
		
		private static ConstructableType loadConstructable(IkonComposite data)
		{
			return new ConstructableType(
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				data[GeneralCodeKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()).ToArray(), 
				siteType(data[ConstructableSiteKey].To<string>()),
				data[ConstructableStockpileKey].To<string>(),
				data[ConstructableConditionKey].To<Formula>(),
				data[GeneralCostKey].To<Formula>(),
				data[ConstructableLimitKey].To<Formula>(),
				loadConstructionEffects(data[ConstructableEffectsKey].To<IEnumerable<IkonComposite>>()).ToArray()
			);
		}
		
		private static SiteType siteType(string rawData)
		{
			switch(rawData.ToLower(System.Globalization.CultureInfo.InvariantCulture))
			{
				case SiteColony:
					return SiteType.Colony;
				case SiteSystem:
					return SiteType.StarSystem;
				default:
					throw new FormatException("Invalid building site type: " + rawData);
			}
		}
		
		private static IEnumerable<IConstructionEffect> loadConstructionEffects(IEnumerable<IkonComposite> data)
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
					case ConstructionAddTraitTag:
						yield return new ConstructionAddTrait(
							effectData[AddTraitId].To<string>()
						);
						break;
					default:
						throw new FormatException("Invalid construction effect with tag " + effectData.Tag);
				}
		}
		#endregion

		private void loadNatives(IkonComposite data)
		{
			foreach(var designName in data.Keys)
				this.NativeDesigns[designName] = loadPredefDesign(data[designName].To<IkonComposite>());
		}
		
		private static DesignTemplate loadDesignTemplate(IkonComposite data)
		{
			return new DesignTemplate(
				data[DesignName].To<string>(),
				data[DesignHull].To<string>(),
				data[DesignHullImageIndex].To<int>(),
				data.Keys.Contains(DesignHasIsDrive),
				data.Keys.Contains(DesignShield) ? data[DesignShield].To<string>() : null,
				loadDesignMissionEquipment(data[DesignMissionEquipment].To<IkonArray>()),
				loadDesignSpecialEquipment(data[DesignSpecialEquipment].To<IkonArray>())
			);
		}

		private static List<KeyValuePair<string, int>> loadDesignMissionEquipment(IList<Ikadn.IkadnBaseObject> data)
		{
			var result = new List<KeyValuePair<string, int>>();
			
			for(int i = 0; i < data.Count / 2; i++)
				result.Add(new KeyValuePair<string, int>(data[i * 2].To<string>(), data[i * 2 + 1].To<int>()));
			
			return result;
		}
		
		private static Dictionary<string, int> loadDesignSpecialEquipment(IList<Ikadn.IkadnBaseObject> data)
		{
			var result = new Dictionary<string, int>();
			
			for(int i = 0; i < data.Count / 2; i++)
				result.Add(data[i * 2].To<string>(), data[i * 2 + 1].To<int>());
			
			return result;
		}

		private static PredefinedDesign loadPredefDesign(IkonComposite data)
		{
			return new PredefinedDesign(
				data[DesignName].To<string>(),
				loadPredefinedComponent(data[DesignHull].To<IkonArray>()),
				data[DesignHullImageIndex].To<int>(),
				!data.Keys.Contains(DesignNotFuelUser),
				data.Keys.Contains(DesignIsDrive) ? loadPredefinedComponent(data[DesignIsDrive].To<IkonArray>()) : null,
				data.Keys.Contains(DesignShield) ? loadPredefinedComponent(data[DesignShield].To<IkonArray>()) : null,
				loadPredefinedEquipment(data[DesignMissionEquipment].To<IkonArray>()),
				loadPredefinedEquipment(data[DesignSpecialEquipment].To<IkonArray>()),
				loadPredefinedComponent(data[DesignArmor].To<IkonArray>()),
				loadPredefinedComponent(data[DesignReactor].To<IkonArray>()),
				loadPredefinedComponent(data[DesignSensor].To<IkonArray>()),
				loadPredefinedComponent(data[DesignThrusters].To<IkonArray>())
			);
		}

		private static PredefinedComponent loadPredefinedComponent(IkonArray data)
		{
			return new PredefinedComponent(
				data[0].To<string>(),
				data[1].To<int>(),
				0
			);
		}

		private static List<PredefinedComponent> loadPredefinedEquipment(IkonArray data)
		{
			var result = new List<PredefinedComponent>();

			for (int i = 0; i < data.Count / 3; i++)
				result.Add(new PredefinedComponent(
					data[i * 3].To<string>(),
					data[i * 3 + 1].To<int>(),
					data[i * 3 + 2].To<int>()
				));

			return result;
		}

		#region Ship components
		private static ArmorType loadArmor(IkonComposite data)
		{
			return new ArmorType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				!data.Keys.Contains(GeneralCannotPickKey),
				data[ArmorFactor].To<Formula>(),
				data[ArmorAbsorb].To<Formula>(),
				data[ArmorAbsorbMax].To<Formula>()
			);
		}

		private static IEnumerable<AAbilityType> loadEquipmentAbilities(IEnumerable<IkonComposite> data)
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
							abilityData[AbilityRange].To<Formula>(),
							abilityData[AbilityEnergyCost].To<Formula>(),
							abilityData.ToOrDefault(DirectShootAccuracyRangePenalty, new Formula(0)),
							abilityData.ToOrDefault(AbilityAmmo, new Formula(double.PositiveInfinity)),
							abilityData.ToOrDefault(DirectShootArmorEfficiency, new Formula(1)),
							abilityData.ToOrDefault(DirectShootShieldEfficiency, new Formula(1)),
							abilityData.ToOrDefault(DirectShootPlanetEfficiency, new Formula(1))
						);
						break;
					case ProjectileShotTag:
						yield return new ProjectileAbility(
							abilityData[GeneralImageKey].To<string>(),
							abilityData[DirectShootFirepower].To<Formula>(),
							abilityData[DirectShootAccuracy].To<Formula>(),
							abilityData.ToOrDefault(AbilityAmmo, new Formula(double.PositiveInfinity)),
							abilityData[ProjectileSpeed].To<Formula>(),
							abilityData.ToOrDefault(DirectShootArmorEfficiency, new Formula(1)),
							abilityData.ToOrDefault(DirectShootShieldEfficiency, new Formula(1)),
							abilityData.ToOrDefault(DirectShootPlanetEfficiency, new Formula(1)),
							abilityData.ToOrDefault(SplashMaxTargets, new Formula(1)),
							abilityData.ToOrDefault(SplashFirepower, new Formula(1)),
							abilityData.ToOrDefault(SplashArmorEfficiency, new Formula(1)),
							abilityData.ToOrDefault(SplashShieldEfficiency, new Formula(1)),
							abilityData[ProjectileShootImage].To<string>()
						);
                        break;
					case StarShotTag:
						yield return new StarShootAbility(
							abilityData[GeneralImageKey].To<string>(),
							abilityData[AbilityRange].To<Formula>(),
							abilityData[AbilityEnergyCost].To<Formula>(),
							abilityData[StarShootTrait].To<string>()
						);
						break;
					default:
						throw new FormatException("Invalid construction effect with tag " + abilityData.Tag);
				}
			}
		}
		
		private static HullType loadHull(IkonComposite data)
		{
			return new HullType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralLangKey].To<string>(),
				data[HullImages].To<string[]>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				!data.Keys.Contains(GeneralCannotPickKey),
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
		
		private static IsDriveType loadIsDrive(IkonComposite data)
		{
			return new IsDriveType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				!data.Keys.Contains(GeneralCannotPickKey),
				data[GeneralCostKey].To<Formula>(),
				data[IsDriveSpeed].To<Formula>(),
				data[IsDriveMinSize].To<Formula>()
			);
		}
		
		private static MissionEquipmentType loadMissionEquiptment(IkonComposite data)
		{
			return new MissionEquipmentType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				data[GeneralCostKey].To<Formula>(),
				!data.Keys.Contains(GeneralCannotPickKey),
				data[SpecialEquipmentSizeKey].To<Formula>(),
				loadEquipmentAbilities(data[MissionEquipmentAbilitiesKey].To<IEnumerable<IkonComposite>>()).ToArray()
			);
		}
		
		private static ReactorType loadReactor(IkonComposite data)
		{
			return new ReactorType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				!data.Keys.Contains(GeneralCannotPickKey),
				data[EquipmentPowerKey].To<Formula>(),
				data[ReactorMinSize].To<Formula>()
			);
		}
		
		private static SensorType loadSensor(IkonComposite data)
		{
			return new SensorType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				!data.Keys.Contains(GeneralCannotPickKey),
				data[SensorDetection].To<Formula>()
			);
		}
		
		private static ShieldType loadShield(IkonComposite data)
		{
			return new ShieldType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				!data.Keys.Contains(GeneralCannotPickKey),
				data[GeneralCostKey].To<Formula>(),
				data[ShieldHpFactor].To<Formula>(),
				data[ShieldRegeneration].To<Formula>(),
				data[ShieldThickness].To<Formula>(),
				data[ShieldReduction].To<Formula>(),
				data[ShieldCloaking].To<Formula>(),
				data[ShieldJamming].To<Formula>(),
				data[EquipmentPowerKey].To<Formula>()
			);
		}
		
		private static SpecialEquipmentType loadSpecialEquiptment(IkonComposite data)
		{
			return new SpecialEquipmentType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				!data.Keys.Contains(GeneralCannotPickKey),
				data[GeneralCostKey].To<Formula>(),
				data[SpecialEquipmentSizeKey].To<Formula>(),
				data[SpecialEquipmentMaxCountKey].To<Formula>()
			);
		}

		private static ThrusterType loadThruster(IkonComposite data)
		{
			return new ThrusterType(
				data[GeneralCodeKey].To<string>(),
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				loadPrerequisites(data[GeneralPrerequisitesKey].To<IkonArray>()),
				data[GeneralMaxLevelKey].To<int>(),
				!data.Keys.Contains(GeneralCannotPickKey),
				data[ThrusterEvasion].To<Formula>(),
				data[ThrusterSpeed].To<Formula>()
			);
		}
		#endregion

		#region Technologies
		private static IEnumerable<DevelopmentFocus> loadFocusOptions(IkonComposite data)
		{
			foreach(var array in data[FocusList].To<IkonArray>()) {
				double[] weights = array.To<double[]>();
				double sum = weights.Sum();
				yield return new DevelopmentFocus(weights.Select(x => x /sum).ToArray());
			}
		}
			
		private static IEnumerable<Prerequisite> loadPrerequisites(IList<Ikadn.IkadnBaseObject> dataArray)
		{
			for(int i = 0; i < dataArray.Count; i += 2)
				yield return new Prerequisite(
					dataArray[i].To<string>(), 
					dataArray[i + 1].To<Formula>()
				);
		}
		
		private static DevelopmentTopic loadDevelopmentTopic(IkonComposite data)
		{
			return new DevelopmentTopic(
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				data[GeneralCodeKey].To<string>(),
				data[GeneralCostKey].To<Formula>(),
				data[GeneralMaxLevelKey].To<int>()
			 );
		}
		
		private static ResearchTopic loadResearchTopic(IkonComposite data)
		{
			return new ResearchTopic(
				data[GeneralLangKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				data[GeneralCodeKey].To<string>(),
				data[GeneralCostKey].To<Formula>(),
				data[ResearchUnlocksKey].To<IkonArray>().Select(x => x.To<string[]>()).ToArray()
			 );
		}
		#endregion
		
		#region Loading tags and keys
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
		private const string ProjectileShotTag = "Projectile";
		private const string StarShotTag = "StarShot";

		private const string AfflictTraitTag = "AfflictPlanets";
		private const string TemporaryTraitTag = "Temporary";
		private const string AfflictionEffectTag = "Affliction";

		private const string ColonizationPopulationThreshold = "colonizationPopThreshold";
		private const string ColonyDevelopment = "development";
		private const string ColonyEnvironment = "environment";
		private const string ColonyEmigrants = "emigrants";
		private const string ColonyFarmFields = "farmFields";
		private const string ColonyFarming = "farming";
		private const string ColonyFuelCost = "fuelCost";
		private const string ColonyFuelProduction = "fuel";
		private const string ColonyGardening = "gardening";
		private const string ColonyIndustry = "industry";
		private const string ColonyMaintenanceLimit = "maintenanceLimit";
		private const string ColonyMaxPopulation = "maxPopulation";
		private const string ColonyMining = "mining";
		private const string ColonyOrganization = "organization";
		private const string ColonyPopulationGrowth = "populationGrowth";
		private const string ColonyPopulationHitPoints = "popHp";
		private const string ColonyRepairPoints = "repair";
		private const string ColonySpaceliftFactor = "spaceliftFactor";
		private const string ColonyDesirability = "desirability";
		private const string ColonyVictoryWorth = "victoryPointWorth";
		private const string UncolonizedMaxPopulation = "uncolonizedMaxPopulation";

		private const string PlanetTypeKey = "type";
		private const string PlanetTypeAsteroid = "asteriod";
		private const string PlanetTypeGasGiant = "gasGiant";
		private const string PlanetTypeRock = "rock";
		private const string PlanetBaseTraits = "baseTraits";
		private const string PlanetBestTraits = "bestTraits";
		private const string PlanetUnchangeableTraits = "unchangeableTraits";
		private const string PlanetPotentialScore = "bestScore";
		private const string PlanetStartingScore = "startScore";
		private const string PlanetWorstTraits = "worstTraits";

		private const string PlayerResearchFocusWeight = "focusedResearchWeight";
		
		private const string ShipCloaking = "cloaking";
		private const string ShipCombatSpeed = "combatSpeed";
		private const string ShipDetection = "detection";
		private const string ShipEvasion = "evasion";
		private const string ShipFuelUsage = "fuelUsage";
		private const string ShipHitPoints = "hitPoints";
		private const string ShipJamming = "jamming";
		private const string ShipScaneRange = "scanRange";
		private const string ShipColonyPopulation = "colonyPop";
		private const string ShipColonyBuildings = "colonyBuildings";
		private const string ShipReactorSize = "reactorSize";
		private const string ShipShieldSize = "shieldSize";
		private const string ShipNaturalCloakBonus = "naturalCloakBonus";
		private const string ShipSensorRangePenalty = "sensorRangePenalty";
		private const string ShipRepairCostFactor = "repairCostFactor";
		private const string ShipLevelRefitCost = "levelRefitCost";
		private const string ShipArmorCostPortion = "armorCostPortion";
		private const string ShipReactorCostPortion = "reactorCostPortion";
		private const string ShipSensorCostPortion = "sensorCostPortion";
		private const string ShipThrustersCostPortion = "thrustersCostPortion";
		private const string ShipWormholeSpeed = "wormholeSpeed";

		private const string BuildingHitPointsKey = "hitPoints";
		
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
		private const string ConstructionAddTraitTag = "addtrait";
		private const string AddTraitId = "traitId";

		private const string DerivedStatBase = "base";
		private const string DerivedStatTotal = "total";
		
		private const string DesignName = "name";
		private const string DesignArmor = "armor";
		private const string DesignHasIsDrive = "hasIsDrive";
		private const string DesignIsDrive = "isDrive";
		private const string DesignHull = "hull";
		private const string DesignHullImageIndex = "hullImageIndex";
		private const string DesignMissionEquipment = "equipment";
		private const string DesignNotFuelUser = "notFuelUser";
		private const string DesignReactor = "reactor";
		private const string DesignSensor = "sensor";
		private const string DesignShield = "shield";
		private const string DesignSpecialEquipment = "specials";
		private const string DesignThrusters = "thrusters";

		private const string FocusList = "list";
		private const string ResearchUnlocksKey = "devTopics";

		private const string GeneralLangKey = "langCode";
		private const string GeneralImageKey = "image";
		private const string GeneralCodeKey = "code";
		private const string GeneralPrerequisitesKey = "prerequisites";
		private const string GeneralMaxLevelKey = "maxLvl";
		private const string GeneralCannotPickKey = "cannotPick";
		private const string GeneralCostKey = "cost";
		
		private const string PopulationActivityImprovised = "improvised";
		private const string PopulationActivityOrganized = "organized";
		private const string PopulationActivityOrganizationFactor = "orgFactor";

		private const string PolicySpendingRatioKey = "spendingRatio";
		private const string PolicyBuildingQueueKey = "queue";

		private const string TraitMaintenanceKey = "cost";
		private const string TraitEffectKey = "effect";

		
		private const string ArmorAbsorb = "reduction";
		private const string ArmorAbsorbMax = "reductionMax";
		private const string ArmorFactor = "armorFactor";
		
		private const string EquipmentPowerKey = "power";
		
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
		
		private const string ReactorMinSize = "minSize";

		private const string SensorDetection = "detection";
		
		private const string ShieldCloaking = "cloaking";
		private const string ShieldJamming = "jamming";
		private const string ShieldHpFactor = "shieldFactor";
		private const string ShieldReduction = "reduction";
		private const string ShieldRegeneration = "restoration";
		private const string ShieldThickness = "thickness";
		
		private const string SpecialEquipmentMaxCountKey = "maxCount";
		private const string SpecialEquipmentSizeKey = "size";
		
		private const string ThrusterEvasion = "evasion";
		private const string ThrusterSpeed = "speed";


		private const string AbilityAmmo = "ammo";
		private const string AbilityEnergyCost = "energyCost";
		private const string AbilityRange = "range";

		private const string DirectShootAccuracy = "accuracy";
		private const string DirectShootAccuracyRangePenalty = "accuracyRangePenalty";
		private const string DirectShootArmorEfficiency = "armorEfficiency";
		private const string DirectShootFirepower = "firePower";
		private const string DirectShootPlanetEfficiency = "planetEfficiency";
		private const string DirectShootShieldEfficiency = "shieldEfficiency";

		private const string ProjectileShootImage = "projectileImage";
		private const string ProjectileSpeed = "speed";

		private const string SplashArmorEfficiency = "splashArmorEfficiency";
		private const string SplashFirepower = "splashFirePower";
		private const string SplashMaxTargets = "splashMaxTargets";
		private const string SplashShieldEfficiency = "splashShieldEfficiency";

		private const string StarShootTrait = "applyTrait";

		public const string AfflictionListKey = "afflictions";
		public const string AfflictionTraitKey = "trait";
		public const string AfflictionConditionKey = "condition";
		public const string DurationTraitKey = "duration";
		#endregion
	}
}
