using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Players;
using Stareater.Utils;
using Stareater.GameData.Databases;
using System.Linq;

namespace Stareater.Ships
{
	class Design 
	{
		[StatePropertyAttribute]
		public string IdCode { get; private set; }

		[StatePropertyAttribute]
		public Player Owner { get; private set; }

		[StatePropertyAttribute]
		public bool IsObsolete { get; set; } //TODO(v0.8) move to stats

		[StatePropertyAttribute]
		public string Name { get; private set; }

		[StatePropertyAttribute]
		public int ImageIndex { get; private set; }

		[StatePropertyAttribute]
		public bool UsesFuel { get; private set; }

		[StatePropertyAttribute]
		public Component<ArmorType> Armor { get; private set; }

		[StatePropertyAttribute]
		public Component<HullType> Hull { get; private set; }

		[StatePropertyAttribute]
		public Component<IsDriveType> IsDrive { get; private set; }

		[StatePropertyAttribute]
		public Component<ReactorType> Reactor { get; private set; }

		[StatePropertyAttribute]
		public Component<SensorType> Sensors { get; private set; }

		[StatePropertyAttribute]
		public Component<ShieldType> Shield { get; private set; }

		[StatePropertyAttribute]
		public List<Component<MissionEquipmentType>> MissionEquipment { get; private set; }

		[StatePropertyAttribute]
		public List<Component<SpecialEquipmentType>> SpecialEquipment { get; private set; }

		[StatePropertyAttribute]
		public Component<ThrusterType> Thrusters { get; private set; }

		[StatePropertyAttribute(doSave: false)]
		private BitHash hash { get; set; } //TODO(v0.9) try to move design stats

		[StatePropertyAttribute]
		public double Cost { get; private set; } //TODO(v0.9) try to move design stats

		public Design(string idCode, Player owner, bool isObsolete, string name, int imageIndex, bool usesFuel, 
			Component<ArmorType> armor, Component<HullType> hull, Component<IsDriveType> isDrive, Component<ReactorType> reactor, Component<SensorType> sensors, Component<ThrusterType> thrusters, 
			Component<ShieldType> shield, List<Component<MissionEquipmentType>> missionEquipment, List<Component<SpecialEquipmentType>> specialEquipment) 
		{
			this.IdCode = idCode;
			this.Owner = owner;
			this.IsObsolete = isObsolete;
			this.Name = name;
			this.ImageIndex = imageIndex;
			this.UsesFuel = usesFuel;
			this.Armor = armor;
			this.Hull = hull;
			this.IsDrive = isDrive;
			this.Reactor = reactor;
			this.Sensors = sensors;
			this.Shield = shield;
			this.MissionEquipment = missionEquipment;
			this.SpecialEquipment = specialEquipment;
			this.Thrusters = thrusters;
			
			this.Cost = initCost();
 		}

		private Design() 
		{ }

		public void CalcHash(StaticsDB statics)
		{
			var hashBuilder = new BitHashBuilder();

			HashComponent(hashBuilder, this.Armor, statics.Armors);
			HashComponent(hashBuilder, this.Reactor, statics.Reactors);
			HashComponent(hashBuilder, this.Sensors, statics.Sensors);
			HashComponent(hashBuilder, this.Thrusters, statics.Thrusters);

			HashComponent(hashBuilder, this.Hull, statics.Hulls);
			hashBuilder.Add(this.SpecialEquipment.Count, statics.SpecialEquipment.Count);

			if (this.IsDrive != null)
			{
				hashBuilder.Add(1, 2);
				HashComponent(hashBuilder, this.IsDrive, statics.IsDrives);
			}
			else
				hashBuilder.Add(0, 2);

			HashComponent(hashBuilder, this.Shield, statics.Shields);

			int maxEquips = this.SpecialEquipment.Count > 0 ? (this.SpecialEquipment.Max(x => x.Quantity) + 1) : 0;
			foreach (var equip in this.SpecialEquipment.OrderBy(x => x.TypeInfo.IdCode))
			{
				HashComponent(hashBuilder, equip, statics.SpecialEquipment);
				hashBuilder.Add(equip.Quantity, maxEquips);
			}

			maxEquips = this.MissionEquipment.Count > 0 ? (this.MissionEquipment.Max(x => x.Quantity) + 1) : 0;
			foreach (var equip in this.MissionEquipment.OrderBy(x => x.TypeInfo.IdCode))
			{
				HashComponent(hashBuilder, equip, statics.MissionEquipment);
				hashBuilder.Add(equip.Quantity, maxEquips);
			}

			this.hash = hashBuilder.Create();
		}

		public string ImagePath
		{
			get
			{
				return Hull.TypeInfo.ImagePaths[this.ImageIndex];
			}
		}

		private double initCost()
		{
			return CalculateCost(this.Hull, this.IsDrive, this.Shield, this.MissionEquipment, this.SpecialEquipment);
		}

		private static void HashComponent<T>(BitHashBuilder hashBuilder, Component<T> component, IDictionary<string, T> componentAssortiment) where T : AComponentType
		{
			var indices = componentAssortiment.Keys.OrderBy(x => x).ToList();
			var index = component != null ? indices.IndexOf(component.TypeInfo.IdCode) : componentAssortiment.Count;

			hashBuilder.Add(index, componentAssortiment.Count + 1);
			if (component != null)
				hashBuilder.Add(component.Level, component.TypeInfo.MaxLevel + 1);
		}

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as Design;
			if (other == null)
				return false;
			return this.hash == other.hash && this.Owner == other.Owner;
		}

		public override int GetHashCode()
		{
			return this.hash.GetHashCode();
		}

		public static bool operator ==(Design lhs, Design rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (lhs is null || rhs is null)
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Design lhs, Design rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

		public static double CalculateCost(Component<HullType> hull, Component<IsDriveType> isDrive, Component<ShieldType> shield,
										   List<Component<MissionEquipmentType>> missionEquipment, List<Component<SpecialEquipmentType>> specialEquipment)
		{
			var hullVars = new Var(AComponentType.LevelKey, hull.Level).Get;
			double hullCost = hull.TypeInfo.Cost.Evaluate(hullVars);

			double isDriveCost = 0;
			if (isDrive != null)
			{
				var driveVars = new Var(AComponentType.LevelKey, isDrive.Level).
					And(AComponentType.SizeKey, hull.TypeInfo.SizeIS.Evaluate(hullVars)).Get;
				isDriveCost = isDrive.TypeInfo.Cost.Evaluate(driveVars);
			}

			double shieldCost = 0;
			if (shield != null)
			{
				var shieldVars = new Var(AComponentType.LevelKey, shield.Level).
					And(AComponentType.SizeKey, hull.TypeInfo.SizeShield.Evaluate(hullVars)).Get;
				shieldCost = shield.TypeInfo.Cost.Evaluate(shieldVars);
			}

			double weaponsCost = missionEquipment.Sum(
				x => x.Quantity * x.TypeInfo.Cost.Evaluate(
					new Var(AComponentType.LevelKey, x.Level).Get
				)
			);

			double hullSize = hull.TypeInfo.Size.Evaluate(hullVars);
			double specialsCost = specialEquipment.Sum(
				x => x.Quantity * x.TypeInfo.Cost.Evaluate(
					new Var(AComponentType.LevelKey, x.Level).
					And(AComponentType.SizeKey, hullSize).Get
				)
			);

			return hullCost + isDriveCost + shieldCost + weaponsCost + specialsCost;
		}
	}
}
