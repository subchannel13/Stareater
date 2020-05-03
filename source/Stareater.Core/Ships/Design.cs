using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Players;
using Stareater.Utils;
using Stareater.GameData.Databases;
using System.Linq;
using Stareater.GameLogic;
using Stareater.GameData.Databases.Tables;

namespace Stareater.Ships
{
	class Design 
	{
		[StatePropertyAttribute]
		public string IdCode { get; private set; } //TODO(v0.9) remove, use equals instead

		[StatePropertyAttribute]
		public Player Owner { get; private set; }

		[StatePropertyAttribute]
		public bool IsObsolete { get; set; } //TODO(v0.9) move to stats

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

		public Design(string idCode, Player owner, bool isObsolete, string name, int imageIndex, bool usesFuel, 
			Component<ArmorType> armor, Component<HullType> hull, Component<IsDriveType> isDrive, Component<ReactorType> reactor, Component<SensorType> sensors, Component<ThrusterType> thrusters, 
			Component<ShieldType> shield, List<Component<MissionEquipmentType>> missionEquipment, List<Component<SpecialEquipmentType>> specialEquipment, StaticsDB statics) 
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

			this.hash = this.calcHash(statics);
 		}

		private Design() 
		{ }

		public string ImagePath
		{
			get
			{
				return Hull.TypeInfo.ImagePaths[this.ImageIndex];
			}
		}

		private BitHash calcHash(StaticsDB statics)
		{
			var hashBuilder = new BitHashBuilder();

			hashComponent(hashBuilder, this.Armor, statics.Armors);
			hashComponent(hashBuilder, this.Reactor, statics.Reactors);
			hashComponent(hashBuilder, this.Sensors, statics.Sensors);
			hashComponent(hashBuilder, this.Thrusters, statics.Thrusters);

			hashComponent(hashBuilder, this.Hull, statics.Hulls);
			hashBuilder.Add(this.SpecialEquipment.Count, statics.SpecialEquipment.Count);

			if (this.IsDrive != null)
			{
				hashBuilder.Add(1, 2);
				hashComponent(hashBuilder, this.IsDrive, statics.IsDrives);
			}
			else
				hashBuilder.Add(0, 2);

			hashComponent(hashBuilder, this.Shield, statics.Shields);

			int maxEquips = this.SpecialEquipment.Count > 0 ? (this.SpecialEquipment.Max(x => x.Quantity) + 1) : 0;
			foreach (var equip in this.SpecialEquipment.OrderBy(x => x.TypeInfo.IdCode))
			{
				hashComponent(hashBuilder, equip, statics.SpecialEquipment);
				hashBuilder.Add(equip.Quantity, maxEquips);
			}

			maxEquips = this.MissionEquipment.Count > 0 ? (this.MissionEquipment.Max(x => x.Quantity) + 1) : 0;
			foreach (var equip in this.MissionEquipment.OrderBy(x => x.TypeInfo.IdCode))
			{
				hashComponent(hashBuilder, equip, statics.MissionEquipment);
				hashBuilder.Add(equip.Quantity, maxEquips);
			}

			return hashBuilder.Create();
		}

		private static void hashComponent<T>(BitHashBuilder hashBuilder, Component<T> component, IDictionary<string, T> componentAssortiment) where T : AComponentType
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
	}
}
