using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.Controllers.Views.Ships;
using Stareater.GameData.Ships;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers
{
	public class ShipDesignController
	{
		private readonly MainGame game;
		private Player player;
		private readonly Dictionary<string, double> playersTechLevels;
		
		internal ShipDesignController(MainGame game, Player player)
		{
			this.game = game;
			this.player = player;
			
			this.playersTechLevels = game.States.DevelopmentAdvances.Of[this.player]
				.ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			
			this.armorInfo = bestArmor();
			this.sensorInfo = bestSensor();
			this.thrusterInfo = bestThruster();
		}

		private ArmorInfo bestArmor()
		{
			var armor = AComponentType.MakeBest(
				game.Statics.Armors.Values,
				playersTechLevels
			);
			
			return armor != null ? new ArmorInfo(armor.TypeInfo, armor.Level) : null;
		}
		
		private IsDriveInfo bestIsDrive()
		{
			var hull = new Component<HullType>(this.selectedHull.Type, this.selectedHull.Level);
			var reactor = new Component<ReactorType>(this.reactorInfo.Type, this.reactorInfo.Level);
			
			var drive = IsDriveType.MakeBest(
				playersTechLevels, 
				hull,
				reactor,
				this.selectedSpecialEquipment,
				game.Statics
			);

			return drive != null ? 
				new IsDriveInfo(drive.TypeInfo, drive.Level, PlayerProcessor.DesignPoweredVars(hull,reactor, this.selectedSpecialEquipment, game.Statics).Get) : 
				null;
		}

		private ReactorInfo bestReactor()
		{
			var hull = new Component<HullType>(this.selectedHull.Type, this.selectedHull.Level);
			var reactor = ReactorType.MakeBest(
				playersTechLevels,
				hull,
				this.selectedSpecialEquipment,
				game.Statics
			);

			return reactor != null ? 
				new ReactorInfo(reactor.TypeInfo, reactor.Level, PlayerProcessor.DesignBaseVars(hull, this.selectedSpecialEquipment, game.Statics).Get) : 
				null;
		}

		private SensorInfo bestSensor()
		{
			var sensor = AComponentType.MakeBest(
				game.Statics.Sensors.Values,
				playersTechLevels
			);

			return sensor != null ? new SensorInfo(sensor.TypeInfo, sensor.Level) : null;
		}
		
		private ThrusterInfo bestThruster()
		{
			var thruster = AComponentType.MakeBest(
				game.Statics.Thrusters.Values,
				playersTechLevels
			);

			return thruster != null ? new ThrusterInfo(thruster.TypeInfo, thruster.Level) : null;
		}
		
		private Var shipBaseVars()
		{
			return PlayerProcessor.DesignBaseVars(
				new Component<HullType>(this.selectedHull.Type, this.selectedHull.Level), 
				this.selectedSpecialEquipment, 
				this.game.Statics);
		}
		
		private Var shipPoweredVars()
		{
			return PlayerProcessor.DesignPoweredVars(
				new Component<HullType>(this.selectedHull.Type, this.selectedHull.Level), 
				new Component<ReactorType>(this.reactorInfo.Type, this.reactorInfo.Level),
				this.selectedSpecialEquipment,
				this.game.Statics);
		}
		
		#region Component lists
		public IEnumerable<HullInfo> Hulls()
		{
			return game.Statics.Hulls.Values.Where(x => x.CanPick).Select(x => new HullInfo(x, x.HighestLevel(playersTechLevels)));
		}
		
		public ArmorInfo Armor
		{
			get { return this.armorInfo; }
		}
		
		public IsDriveInfo AvailableIsDrive
		{
			get { return this.availableIsDrive; }
		}

		public ReactorInfo Reactor
		{
			get { return this.reactorInfo; }
		}

		public SensorInfo Sensor
		{
			get { return this.sensorInfo; }
		}
		
		public IEnumerable<ShieldInfo> Shields()
		{
			return this.game.Statics.Shields.Values.
				Where(x => x.IsAvailable(playersTechLevels) && x.CanPick).
				Select(x => new ShieldInfo(x, x.HighestLevel(playersTechLevels), this.selectedHull));
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
				Select(x => new SpecialEquipInfo(x, x.HighestLevel(playersTechLevels), this.selectedHull));
		}
		
		public ThrusterInfo Thrusters
		{
			get { return this.thrusterInfo; }
		}
		#endregion
		
		#region Selected components
		private HullInfo selectedHull = null;

		private ArmorInfo armorInfo = null;
		private IsDriveInfo availableIsDrive = null;
		private ReactorInfo reactorInfo = null;
		private SensorInfo sensorInfo = null;
		private ThrusterInfo thrusterInfo = null;

		private readonly List<Component<MissionEquipmentType>> selectedMissionEquipment = new List<Component<MissionEquipmentType>>();
		private readonly List<Component<SpecialEquipmentType>> selectedSpecialEquipment = new List<Component<SpecialEquipmentType>>();

		void onHullChange()
		{
			if (this.ImageIndex < 0 || this.ImageIndex >= this.selectedHull.ImagePaths.Length)
				this.ImageIndex = 0;

			this.reactorInfo = bestReactor();
			this.availableIsDrive = bestIsDrive();
			this.HasIsDrive &= availableIsDrive != null;
		}

		#endregion

		#region Combat info
		public double Cloaking
		{
			get 
			{
				return game.Statics.ShipFormulas.Cloaking.Evaluate(
					this.shipPoweredVars().
					And("shieldCloak", Shield != null ? Shield.Cloaking : 0).Get
				);
			}
		}
		
		public double CombatSpeed
		{
			get 
			{
				return game.Statics.ShipFormulas.CombatSpeed.Evaluate(
					this.shipBaseVars().
					And("thrust", this.thrusterInfo.Evasion).Get
				);
			}
		}
		
		public double Detection
		{
			get 
			{
				return game.Statics.ShipFormulas.Detection.Evaluate(
					this.shipBaseVars().
					And("sensor", this.thrusterInfo.Evasion).Get
				);
			}
		}
		
		public double Evasion
		{
			get 
			{
				return game.Statics.ShipFormulas.Evasion.Evaluate(
					this.shipBaseVars().
					And("baseEvasion", this.thrusterInfo.Evasion).Get
				);
			}
		}
		
		public double HitPoints
		{
			get 
			{
				
				return game.Statics.ShipFormulas.HitPoints.Evaluate(
					this.shipBaseVars().
					And("armorFactor", this.armorInfo.ArmorFactor).Get
				);
			}
		}
		
		public double Jamming
		{
			get 
			{
				return game.Statics.ShipFormulas.Jamming.Evaluate(
					this.shipPoweredVars().
					And("shieldJamming", Shield != null ? Shield.Jamming : 0).Get
				);
			}
		}
		#endregion

		#region Design info
		public double Cost
		{
			get
			{
				return Design.CalculateCost(
					new Component<HullType>(this.selectedHull.Type, this.selectedHull.Level),
					this.HasIsDrive ? new Component<IsDriveType>(this.availableIsDrive.Type, this.availableIsDrive.Level) : null,
					this.Shield != null ? new Component<ShieldType>(this.Shield.Type, this.Shield.Level) : null,
					selectedMissionEquipment,
					selectedSpecialEquipment
				);
			}
		}
		
		public double PowerUsed
		{
			get { return (this.Shield != null) ? this.Shield.PowerUsage : 0; }
		}
		
		public double SpaceTotal
		{
			get { return this.selectedHull.Space; }
		}
		
		public double SpaceUsed
		{
			get 
			{ 
				Func<Component<SpecialEquipmentType>, IDictionary<string, double>> specEquipVars = x => 
					new Var(AComponentType.LevelKey, x.Level).
						And(AComponentType.SizeKey, this.selectedHull.Size).Get;
				
				return (this.HasIsDrive ? this.selectedHull.IsDriveSize : 0) + 
					(this.Shield != null ? this.selectedHull.ShieldSize : 0) +
					this.selectedMissionEquipment.Sum(x => x.TypeInfo.Size.Evaluate(new Var(AComponentType.LevelKey, x.Level).Get) * x.Quantity) + 
					this.selectedSpecialEquipment.Sum(x => x.TypeInfo.Size.Evaluate(specEquipVars(x)) * x.Quantity);
			} 
		}
		#endregion
		
		#region Designer actions
		public string Name { get; set; } 
		public int ImageIndex { get; set; }

		public void AddMissionEquip(MissionEquipInfo equipInfo)
		{
			this.selectedMissionEquipment.Add(new Component<MissionEquipmentType>(equipInfo.Type, equipInfo.Level, 1));
		}
		
		public void AddSpecialEquip(SpecialEquipInfo equipInfo)
		{
			if (this.selectedSpecialEquipment.Any(x => x.TypeInfo == equipInfo.Type))
				return;

			this.selectedSpecialEquipment.Add(new Component<SpecialEquipmentType>(equipInfo.Type, equipInfo.Level, 1));
		}

		public HullInfo Hull 
		{ 
			get { return this.selectedHull; }
			set
			{
				this.selectedHull = value;
				this.onHullChange();
			}
		}
		
		public bool HasIsDrive { get; set; }

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
		}
		
		public ShieldInfo Shield { get; set; }

		public int SpecialEquipCount(SpecialEquipInfo equipInfo)
		{
			return this.selectedSpecialEquipment.
				Where(x => x.TypeInfo == equipInfo.Type).
				Aggregate(0, (sum, x) => x.Quantity);
		}
				
		public void SpecialEquipSetAmount(SpecialEquipInfo equipInfo, int amount)
		{
			int i = this.selectedSpecialEquipment.FindIndex(x => x.TypeInfo == equipInfo.Type);
			
			if (i >= 0)
				if (amount == 0)
					this.selectedSpecialEquipment.RemoveAt(i);
				else if (amount > 0 && amount <= equipInfo.MaxCount)
					this.selectedSpecialEquipment[i] = new Component<SpecialEquipmentType>(equipInfo.Type, equipInfo.Level, amount);
		}

		//TODO(later) consider returning a reason for invalidity 
		public bool IsDesignValid
		{
			get
			{
				//TODO(v0.7): check name length and uniqueness
				return this.selectedHull != null && this.ImageIndex >= 0 && this.ImageIndex < this.selectedHull.ImagePaths.Length &&
					(this.availableIsDrive != null || !this.HasIsDrive) &&
					this.SpaceUsed <= this.SpaceTotal;
			}
		}
		
		public void Commit()
		{
			if (!IsDesignValid)
				return;

			var design = new Design(
				this.game.States.MakeDesignId(),
				this.player,
				false,
				false,
				this.Name,
				this.ImageIndex,
				new Component<ArmorType>(this.armorInfo.Type, this.armorInfo.Level),
				new Component<HullType>(this.selectedHull.Type, this.selectedHull.Level),
				this.HasIsDrive ? new Component<IsDriveType>(this.availableIsDrive.Type, this.availableIsDrive.Level) : null,
				new Component<ReactorType>(this.reactorInfo.Type, this.reactorInfo.Level),
				new Component<SensorType>(this.sensorInfo.Type, this.sensorInfo.Level),
				this.Shield != null ? new Component<ShieldType>(this.Shield.Type, this.Shield.Level) : null,
				selectedMissionEquipment,
				selectedSpecialEquipment, 
				new Component<ThrusterType>(this.thrusterInfo.Type, this.thrusterInfo.Level)
			);
			design.CalcHash(this.game.Statics);
			
			if (this.game.States.Designs.Contains(design))
				return; //TODO(v0.7) move the check to IsDesignValid
			
			game.States.Designs.Add(design); //TODO(v0.7) add to changes DB and propagate to states during turn processing
			game.Derivates.Players.Of[this.player].Analyze(design, this.game.Statics);
		}
		#endregion
	}
}
