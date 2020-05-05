using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.Controllers.Views.Ships;
using Stareater.GameData.Ships;
using Stareater.GameLogic;
using Stareater.GameLogic.Combat;
using Stareater.Players;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers
{
	public class ShipDesignController
	{
		private readonly MainGame game;
		private readonly Player player;
		private readonly Dictionary<string, double> playersTechLevels;
		
		internal ShipDesignController(MainGame game, Player player)
		{
			this.game = game;
			this.player = player;
			
			this.playersTechLevels = game.States.DevelopmentAdvances.Of[this.player]
				.ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			
			var hullType = game.Statics.Hulls.Values.First(x => x.CanPick);
			this.selectedHull = new Component<HullType>(hullType, hullType.HighestLevel(playersTechLevels));
			this.onHullChange();
		}

		private Component<ArmorType> bestArmor() => AComponentType.MakeBest(game.Statics.Armors.Values, playersTechLevels);

		private Component<IsDriveType> bestIsDrive() => IsDriveType.MakeBest(
				playersTechLevels,
				this.selectedHull,
				this.bestReactor(),
				this.selectedSpecialEquipment,
				this.selectedMissionEquipment,
				game.Statics
			);

		private Component<ReactorType> bestReactor() => ReactorType.MakeBest(
				playersTechLevels,
				this.selectedHull,
				this.selectedSpecialEquipment,
				this.selectedMissionEquipment,
				game.Statics
			);

		private Component<SensorType> bestSensor() => AComponentType.MakeBest(game.Statics.Sensors.Values, playersTechLevels);

		private Component<ThrusterType> bestThruster() => AComponentType.MakeBest(game.Statics.Thrusters.Values, playersTechLevels);

		private void makeDesign()
		{
			this.design = new Design(
				this.player,
				this.Name?.Trim() ?? "",
				this.ImageIndex,
				true,
				this.bestArmor(),
				this.selectedHull,
				this.HasIsDrive ? this.bestIsDrive() : null,
				this.bestReactor(),
				this.bestSensor(),
				this.bestThruster(),
				this.selectedShield,
				selectedMissionEquipment,
				selectedSpecialEquipment
			);

			this.stats = PlayerProcessor.StatsOf(this.design, this.game.Statics);
		}

		#region Component lists
		public IEnumerable<HullInfo> Hulls() => 
			game.Statics.Hulls.Values.
			Where(x => x.CanPick).
			Select(x => new HullInfo(new Component<HullType>(x, x.HighestLevel(playersTechLevels))));

		public ArmorInfo Armor => new ArmorInfo(this.bestArmor());
		
		public IsDriveInfo AvailableIsDrive
		{
			get 
			{
				var drive = this.bestIsDrive();
				return drive != null ? new IsDriveInfo(drive, this.stats.GalaxySpeed) : null;
			}
		}

		public ReactorInfo Reactor => new ReactorInfo(this.bestReactor(), this.stats.GalaxyPower);

		public SensorInfo Sensor => new SensorInfo(this.bestSensor());
		
		public IEnumerable<ShieldInfo> Shields()
		{
			return this.game.Statics.Shields.Values.
				Where(x => x.IsAvailable(playersTechLevels) && x.CanPick).
				Select(x => new ShieldInfo(new Component<ShieldType>(x, x.HighestLevel(playersTechLevels)), this.stats.ShieldSize));
		}

		public IEnumerable<MissionEquipInfo> MissionEquipment()
		{
			return this.game.Statics.MissionEquipment.Values.
				Where(x => x.IsAvailable(playersTechLevels) && x.CanPick).
				Select(x => new MissionEquipInfo(x, x.HighestLevel(playersTechLevels)));
		}
		
		public IEnumerable<SpecialEquipInfo> SpecialEquipment()
		{
			return this.game.Statics.SpecialEquipment.Values.
				Where(x => x.IsAvailable(playersTechLevels) && x.CanPick).
				Select(x => new SpecialEquipInfo(x, x.HighestLevel(playersTechLevels), this.selectedHull.TypeInfo));
		}

		public ThrusterInfo Thrusters => new ThrusterInfo(this.bestThruster());
		#endregion
		
		#region Selected components
		private Component<HullType> selectedHull = null;

		private bool hasIsDrive;
		private Component<ShieldType> selectedShield;
		private readonly List<Component<MissionEquipmentType>> selectedMissionEquipment = new List<Component<MissionEquipmentType>>();
		private readonly List<Component<SpecialEquipmentType>> selectedSpecialEquipment = new List<Component<SpecialEquipmentType>>();

		private Design design;
		private DesignStats stats;

		private void onHullChange()
		{
			if (this.ImageIndex < 0 || this.ImageIndex >= this.selectedHull.TypeInfo.ImagePaths.Count)
				this.ImageIndex = 0;

			this.HasIsDrive &= bestIsDrive() != null;
			this.makeDesign();
		}

		#endregion

		#region Combat info
		public double Cloaking => this.stats.Cloaking;

		public double CombatSpeed => this.stats.CombatSpeed;

		public double Detection => this.stats.Detection;

		public double Evasion => this.stats.Evasion;
		
		public double HitPoints =>  this.stats.HitPoints;

		public double Jamming => this.stats.Jamming;
		#endregion

		#region Design info
		public double Cost => this.stats.Cost;

		public double PowerUsed => this.stats.GalaxyPower - this.stats.CombatPower;

		public double SpaceTotal => new HullInfo(this.selectedHull).Space;
		
		public double SpaceUsed
		{
			get 
			{
				var shipVars = PlayerProcessor.DesignBaseVars(
					this.selectedHull,
					this.selectedMissionEquipment, this.selectedSpecialEquipment, game.Statics);
				var specEquipVars = new Var(HullType.SizeKey, this.selectedHull.TypeInfo.Size);

				return (this.HasIsDrive ? this.stats.IsDriveSize : 0) + 
					(this.Shield != null ? this.stats.ShieldSize : 0) +
					this.selectedMissionEquipment.Sum(x => x.TypeInfo.Size.Evaluate(new Var(AComponentType.LevelKey, x.Level).Get) * x.Quantity) + 
					this.selectedSpecialEquipment.Sum(x => x.TypeInfo.Size.Evaluate(specEquipVars.Set(AComponentType.LevelKey, x.Level).Get) * x.Quantity);
			} 
		}
		#endregion
		
		#region Designer actions
		public string Name { get; set; } 
		public int ImageIndex { get; set; }

		public void AddMissionEquip(MissionEquipInfo equipInfo)
		{
			if (equipInfo == null)
				throw new ArgumentNullException(nameof(equipInfo));

			this.selectedMissionEquipment.Add(new Component<MissionEquipmentType>(equipInfo.Type, equipInfo.Level, 1));
			this.makeDesign();
		}

		public HullInfo Hull 
		{
			get => new HullInfo(this.selectedHull);
			set
			{
				if (value is null)
					throw new ArgumentNullException(nameof(value));

				this.selectedHull = value.Component;
				this.onHullChange();
			}
		}

		public bool HasIsDrive 
		{
			get => this.hasIsDrive;
			set
			{
				this.hasIsDrive = value;
				this.makeDesign();
			}
		}

		public bool HasSpecialEquip(SpecialEquipInfo equipInfo)
		{
			return this.selectedSpecialEquipment.Any(x => x.Quantity > 0 && x.TypeInfo == equipInfo.Type);
		}
		
		public int MissionEquipCount(int index)
		{
			return this.selectedMissionEquipment[index].Quantity;
		}

		public void MissionEquipSetAmount(int index, int amount)
		{
			if (index < 0 || index >= this.selectedMissionEquipment.Count || amount < 0)
				return;
			
			if (amount == 0)
				this.selectedMissionEquipment.RemoveAt(index);
			else
			{
				var oldEquip = this.selectedMissionEquipment[index];
				this.selectedMissionEquipment[index] = new Component<MissionEquipmentType>(oldEquip.TypeInfo, oldEquip.Level, amount);
			}
			this.makeDesign();
		}

		public ShieldInfo Shield 
		{
			get => this.selectedShield != null ? new ShieldInfo(this.selectedShield, this.stats.ShieldSize) : null;
			set
			{
				this.selectedShield = value?.Component;
				this.makeDesign();
			}
		}

		public int SpecialEquipCount(SpecialEquipInfo equipInfo)
		{
			return this.selectedSpecialEquipment.
				Where(x => x.TypeInfo == equipInfo.Type).
				Aggregate(0, (sum, x) => x.Quantity);
		}
				
		public void SpecialEquipSetAmount(SpecialEquipInfo equipInfo, int amount)
		{
			if (equipInfo == null)
				throw new ArgumentNullException(nameof(equipInfo));

			int i = this.selectedSpecialEquipment.FindIndex(x => x.TypeInfo == equipInfo.Type);

			if (i < 0)
			{
				i = this.selectedSpecialEquipment.Count;
				this.selectedSpecialEquipment.Add(new Component<SpecialEquipmentType>(equipInfo.Type, equipInfo.Level, 0));
			}

			if (amount <= 0)
				this.selectedSpecialEquipment.RemoveAt(i);
			else if (amount <= equipInfo.MaxCount)
				this.selectedSpecialEquipment[i] = new Component<SpecialEquipmentType>(equipInfo.Type, equipInfo.Level, amount);

			this.onHullChange();
		}

		//TODO(later) consider returning a reason for invalidity 
		public bool IsDesignValid
		{
			get
			{

				return this.selectedHull != null && this.ImageIndex >= 0 && this.ImageIndex < this.selectedHull.TypeInfo.ImagePaths.Count &&
					!string.IsNullOrWhiteSpace(this.Name) && game.States.Designs.All(x => x.Name != this.Name.Trim()) &&
					(this.bestIsDrive() != null || !this.HasIsDrive) &&
					this.SpaceUsed <= this.SpaceTotal;
			}
		}
		
		public void Commit()
		{
			if (!IsDesignValid)
				return;

			this.makeDesign();
			
			if (this.game.States.Designs.Contains(this.design))
				return; //TODO(v0.8) move the check to IsDesignValid
			
			game.States.Designs.Add(design); //TODO(v0.8) add to changes DB and propagate to states during turn processing
			game.Derivates.Players.Of[this.player].Analyze(design, this.game.Statics);
		}
		#endregion
	}
}
