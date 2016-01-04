using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.Controllers.Views.Ships;
using Stareater.GameData.Ships;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers
{
	public class ShipDesignController
	{
		private readonly Game game;
		private readonly Dictionary<string, int> playersTechLevels;
		
		internal ShipDesignController(Game game)
		{
			this.game = game;
			
			this.playersTechLevels = game.States.TechnologyAdvances.Of(game.CurrentPlayer)
				.ToDictionary(x => x.Topic.IdCode, x => x.Level);
			
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
			var drive = IsDriveType.MakeBest(
				game.Statics.IsDrives.Values, 
				playersTechLevels, 
				new Component<HullType>(this.selectedHull.Type, this.selectedHull.Level), 
				this.reactorInfo.Power
			);

			return drive != null ? new IsDriveInfo(drive.TypeInfo, drive.Level, this.selectedHull, this.reactorInfo.Power) : null;
		}

		private ReactorInfo bestReactor()
		{
			var reactor = ReactorType.MakeBest(
				game.Statics.Reactors.Values,
				playersTechLevels,
				new Component<HullType>(this.selectedHull.Type, this.selectedHull.Level)
			);

			return reactor != null ? new ReactorInfo(reactor.TypeInfo, reactor.Level, this.selectedHull) : null;
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
		
		#region Component lists
		public IEnumerable<HullInfo> Hulls()
		{
			return game.Statics.Hulls.Values.Select(x => new HullInfo(x, x.HighestLevel(playersTechLevels)));
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
				Where(x => x.IsAvailable(playersTechLevels)).
				Select(x => new ShieldInfo(x, x.HighestLevel(playersTechLevels), this.selectedHull));
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

		private Dictionary<Component<SpecialEquipmentType>, int> selectedSpecialEquipment = new Dictionary<Component<SpecialEquipmentType>, int>();

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
				var vars = new Var("shieldCloak", Shield != null ? Shield.Cloaking : 0).
					Init(this.game.Statics.SpecialEquipment.Keys, 0).Get;
				
				return game.Statics.ShipFormulas.Cloaking.Evaluate(vars);
			}
		}
		
		public double CombatSpeed
		{
			get 
			{
				var vars = new Var("thrust", thrusterInfo.Speed).
					Init(this.game.Statics.SpecialEquipment.Keys, 0).Get;
				
				return game.Statics.ShipFormulas.CombatSpeed.Evaluate(vars);
			}
		}
		
		public double Detection
		{
			get 
			{
				var vars = new Var("sensor", sensorInfo.Detection).
					Init(this.game.Statics.SpecialEquipment.Keys, 0).Get;
				
				return game.Statics.ShipFormulas.Detection.Evaluate(vars);
			}
		}
		
		public double Evasion
		{
			get 
			{
				var vars = new Var("thrust", thrusterInfo.Evasion).
					Init(this.game.Statics.SpecialEquipment.Keys, 0).Get;
				
				return game.Statics.ShipFormulas.Evasion.Evaluate(vars);
			}
		}
		
		public double HitPoints
		{
			get 
			{
				var vars = new Var("hullHp", selectedHull.HitPointsBase).
					And("armorFactor", armorInfo.ArmorFactor).
					Init(this.game.Statics.SpecialEquipment.Keys, 0).Get;
				
				return game.Statics.ShipFormulas.HitPoints.Evaluate(vars);
			}
		}
		
		public double Jamming
		{
			get 
			{
				var vars = new Var("shieldJamming", Shield != null ? Shield.Jamming : 0).
					Init(this.game.Statics.SpecialEquipment.Keys, 0).Get;
				
				return game.Statics.ShipFormulas.Jamming.Evaluate(vars);
			}
		}
		#endregion

		#region Design info
		public double PowerUsed
		{
			get { return (this.Shield != null) ? this.Shield.PowerUsage : 0; } //TODO(v0.5)
		}
		
		public double SpaceTotal
		{
			get { return this.selectedHull.Space; }
		}
		
		public double SpaceUsed
		{
			get 
			{ 
				//TODO(v0.5)
				return (this.HasIsDrive ? this.selectedHull.IsDriveSize : 0) + 
					(this.Shield != null ? this.selectedHull.ShieldSize : 0);
			} 
		}
		#endregion
		
		#region Designer actions
		public string Name { get; set; } 
		public int ImageIndex { get; set; }

		public void AddSpecialEquip(SpecialEquipInfo equipInfo)
		{
			if (this.selectedSpecialEquipment.Any(x => x.Key.TypeInfo == equipInfo.Type))
				return;

			this.selectedSpecialEquipment.Add(new Component<SpecialEquipmentType>(equipInfo.Type, equipInfo.Level), 1);
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
			return this.selectedSpecialEquipment.Any(x => x.Value > 0 && x.Key.TypeInfo == equipInfo.Type);
		}
		
		public void RemoveSpecialEquip(SpecialEquipInfo equipInfo)
		{
			var component = this.selectedSpecialEquipment.Keys.FirstOrDefault(x => x.TypeInfo == equipInfo.Type);
			
			if (component != null)
				this.selectedSpecialEquipment.Remove(component);
		}
			
		public ShieldInfo Shield { get; set; }

		public int SpecialEquipCount(SpecialEquipInfo equipInfo)
		{
			return this.selectedSpecialEquipment.
				Where(x => x.Key.TypeInfo == equipInfo.Type).
				Aggregate(0, (sum, x) => x.Value);
		}

		public bool IsDesignValid
		{
			get
			{
				//TODO(v0.5): check name length and uniqueness
				//TODO(v0.5): check image index
				//TODO(v0.5): check available space
				return this.selectedHull != null && this.ImageIndex >= 0 && this.ImageIndex < this.selectedHull.ImagePaths.Length &&
					(this.availableIsDrive != null || !this.HasIsDrive);
			}
		}
		
		public void Commit()
		{
			if (!IsDesignValid)
				return;

			var desing = new Design(
				this.game.States.MakeDesignId(),
				this.game.CurrentPlayer,
				false,
				this.Name,
				this.ImageIndex,
				new Component<ArmorType>(this.armorInfo.Type, this.armorInfo.Level),
				new Component<HullType>(this.selectedHull.Type, this.selectedHull.Level),
				this.HasIsDrive ? new Component<IsDriveType>(this.availableIsDrive.Type, this.availableIsDrive.Level) : null,
				new Component<ReactorType>(this.reactorInfo.Type, this.reactorInfo.Level),
				new Component<SensorType>(this.sensorInfo.Type, this.sensorInfo.Level),
				this.Shield != null ? new Component<ShieldType>(this.Shield.Type, this.Shield.Level) : null,
				selectedSpecialEquipment, 
				new Component<ThrusterType>(this.thrusterInfo.Type, this.thrusterInfo.Level)
			);
			desing.CalcHash(this.game.Statics);
			game.States.Designs.Add(desing); //TODO(v0.5) add to changes DB and propagate to states during turn processing
			game.Derivates.Players.Of(this.game.CurrentPlayer).Analyze(desing, this.game.Statics);
		}
		#endregion
	}
}
