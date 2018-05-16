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
		[StateProperty]
		public string IdCode { get; private set; }

		[StateProperty]
		public Player Owner { get; private set; }

		[StateProperty]
		public bool IsObsolete { get; set; }

		[StateProperty]
		public bool IsVirtual { get; private set; }

		[StateProperty]
		public string Name { get; private set; }

		[StateProperty]
		public int ImageIndex { get; private set; }

		[StateProperty]
		public Component<ArmorType> Armor { get; private set; }

		[StateProperty]
		public Component<HullType> Hull { get; private set; }

		[StateProperty]
		public Component<IsDriveType> IsDrive { get; private set; }

		[StateProperty]
		public Component<ReactorType> Reactor { get; private set; }

		[StateProperty]
		public Component<SensorType> Sensors { get; private set; }

		[StateProperty]
		public Component<ShieldType> Shield { get; private set; }

		[StateProperty]
		public List<Component<MissionEquipmentType>> MissionEquipment { get; private set; }

		[StateProperty]
		public List<Component<SpecialEquipmentType>> SpecialEquipment { get; private set; }

		[StateProperty]
		public Component<ThrusterType> Thrusters { get; private set; }

		[StateProperty(doSave: false)]
		private BitHash hash { get; set; } //TODO(v0.7) try to move design stats

		[StateProperty]
		public double Cost { get; private set; } //TODO(v0.7) try to move design stats

		public Design(string idCode, Player owner, bool isObsolete, bool isVirtual, string name, int imageIndex, Component<ArmorType> armor, Component<HullType> hull, Component<IsDriveType> isDrive, Component<ReactorType> reactor, Component<SensorType> sensors, Component<ShieldType> shield, List<Component<MissionEquipmentType>> missionEquipment, List<Component<SpecialEquipmentType>> specialEquipment, Component<ThrusterType> thrusters) 
		{
			this.IdCode = idCode;
			this.Owner = owner;
			this.IsObsolete = isObsolete;
			this.IsVirtual = isVirtual;
			this.Name = name;
			this.ImageIndex = imageIndex;
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
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
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
