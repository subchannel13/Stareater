using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.AppData.Expressions;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Ships;
using Stareater.GameLogic;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.Ships
{
	partial class Design
	{
		private Constructable constructionProject = null;
			
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
			foreach(var equip in this.SpecialEquipment.OrderBy(x => x.TypeInfo.IdCode))
			{
				HashComponent(hashBuilder, equip, statics.SpecialEquipment);
				hashBuilder.Add(equip.Quantity, maxEquips);
			}
			
			maxEquips = this.MissionEquipment.Count > 0 ? (this.MissionEquipment.Max(x => x.Quantity) + 1) : 0;
			foreach(var equip in this.MissionEquipment.OrderBy(x => x.TypeInfo.IdCode))
			{
				HashComponent(hashBuilder, equip, statics.MissionEquipment);
				hashBuilder.Add(equip.Quantity, maxEquips);
			}
			
			this.hash = hashBuilder.Create();
		}
		
		private double initCost()
		{
			return CalculateCost(this.Hull, this.IsDrive, this.Shield, this.MissionEquipment, this.SpecialEquipment);
		}
		
		public string ImagePath 
		{ 
			get
			{
				return Hull.TypeInfo.ImagePaths[this.imageIndex];
			}
		}
		
		public Constructable ConstructionProject
		{
			get {
				if (this.constructionProject == null)
					this.constructionProject = new Constructable(
						this.Name, "", true, this.ImagePath,
						this.IdCode, new Prerequisite[0], SiteType.StarSystem, false, Constructable.ShipStockpile,
						new Formula(true), new Formula(this.Cost), new Formula(double.PositiveInfinity),
						new IConstructionEffect[] { new ConstructionAddShip(this) }
					);
				
				return this.constructionProject;
			}
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
			return hash.GetHashCode();
		}

		public static bool operator ==(Design lhs, Design rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Design lhs, Design rhs) {
			return !(lhs == rhs);
		}

		#endregion
		
		private static void HashComponent<T>(BitHashBuilder hashBuilder, Component<T> component, IDictionary<string, T> componentAssortiment) where T :AComponentType
		{
			var indices = componentAssortiment.Keys.OrderBy(x => x).ToList();
			var index = component != null ? indices.IndexOf(component.TypeInfo.IdCode) : componentAssortiment.Count;

			hashBuilder.Add(index, componentAssortiment.Count + 1);
			if (component != null)
				hashBuilder.Add(component.Level, component.TypeInfo.MaxLevel + 1);
		}
		
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
				isDriveCost	= isDrive.TypeInfo.Cost.Evaluate(driveVars);
			}
			
			double shieldCost = 0;
			if (shield != null)
			{
				var shieldVars = new Var(AComponentType.LevelKey, shield.Level).
					And(AComponentType.SizeKey, hull.TypeInfo.SizeShield.Evaluate(hullVars)).Get;
				shieldCost	= shield.TypeInfo.Cost.Evaluate(shieldVars);
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
					And(HullType.HullSizeKey, hullSize).Get
				)
			);

			return hullCost + isDriveCost + shieldCost + weaponsCost + specialsCost;
		}
	}
}
