using Stareater.GameData.Ships;
using Stareater.Players;
using Stareater.Utils.StateEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.Ships
{
	class Design : IEquatable<Design>
	{
		[StatePropertyAttribute]
		public Player Owner { get; private set; }

		[StatePropertyAttribute]
		public string Name { get; private set; }

		[StatePropertyAttribute]
		public int Version { get; private set; }

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

		[StatePropertyAttribute]
		public bool IsObsolete { get; set; }


		public Design(Player owner, string name, int version, int imageIndex, bool usesFuel,
			Component<ArmorType> armor, Component<HullType> hull, Component<IsDriveType> isDrive, Component<ReactorType> reactor, Component<SensorType> sensors, Component<ThrusterType> thrusters,
			Component<ShieldType> shield, List<Component<MissionEquipmentType>> missionEquipment, List<Component<SpecialEquipmentType>> specialEquipment)
		{
			this.Owner = owner;
			this.Name = name;
			this.Version = version;
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

			this.IsObsolete = false;
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

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			return obj is Design other ? this.Equals(other) : false;
		}

		public bool Equals(Design other)
		{
			return this.Owner == other.Owner &&
				this.Hull == other.Hull &&
				this.IsDrive == other.IsDrive &&
				this.Shield == other.Shield &&
				this.Armor == other.Armor &&
				this.Reactor == other.Reactor &&
				this.Sensors == other.Sensors &&
				this.Thrusters == other.Thrusters &&
				this.MissionEquipment.OrderBy(x => x.TypeInfo.IdCode).SequenceEqual(other.MissionEquipment.OrderBy(x => x.TypeInfo.IdCode)) &&
				this.SpecialEquipment.OrderBy(x => x.TypeInfo.IdCode).SequenceEqual(other.SpecialEquipment.OrderBy(x => x.TypeInfo.IdCode));
		}

		public override int GetHashCode()
		{
			return this.Name.GetHashCode() * 31 + this.Hull.GetHashCode();
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
