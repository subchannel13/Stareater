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
			
		public object PrimaryEquip { get; private set; } //TODO(v0.5): make type
		public object SecondaryEquip { get; private set; } //TODO(v0.5): make type
		
		public void CalcHash(StaticsDB statics)
		{
			var hashBuilder = new BitHashBuilder();
			
			HashComponent(hashBuilder, this.Armor, statics.Armors);
			HashComponent(hashBuilder, this.Reactor, statics.Reactors);
			HashComponent(hashBuilder, this.Sensors, statics.Sensors);
			HashComponent(hashBuilder, this.Thrusters, statics.Thrusters);
			
			HashComponent(hashBuilder, this.Hull, statics.Hulls);
			hashBuilder.Add(this.imageIndex, this.Hull.TypeInfo.ImagePaths.Length);
			
			if (this.IsDrive != null)
			{
				hashBuilder.Add(1, 2);
				HashComponent(hashBuilder, this.IsDrive, statics.IsDrives);
			}
			else
				hashBuilder.Add(0, 2);
			
			int maxEquips = this.SpecialEquipment.Count> 0 ? (this.SpecialEquipment.Values.Max() + 1) : 0;
			foreach(var equip in this.SpecialEquipment.OrderBy(x => x.Key.TypeInfo.IdCode))
			{
				HashComponent(hashBuilder, equip.Key, statics.SpecialEquipment);
				hashBuilder.Add(equip.Value, maxEquips);
			}
			
			this.hash = hashBuilder.Create();
		}
		
		private double initCost()
		{
			var hullVars = new Var(AComponentType.LevelKey, this.Hull.Level).Get;
			double hullCost = this.Hull.TypeInfo.Cost.Evaluate(hullVars);
			
			double isDriveCost = 0;
			if (this.IsDrive != null)
			{
				var driveVars = new Var(AComponentType.LevelKey, this.IsDrive.Level).
					And(AComponentType.SizeKey, this.Hull.TypeInfo.SizeIS.Evaluate(hullVars)).Get;
				isDriveCost	= this.IsDrive.TypeInfo.Cost.Evaluate(driveVars);
			}
			
			double hullSize = this.Hull.TypeInfo.Size.Evaluate(hullVars);
			double specialsCost = SpecialEquipment.Sum(
				x => x.Value * x.Key.TypeInfo.Cost.Evaluate(
					new Var(AComponentType.LevelKey, x.Key.Level).
					And(HullType.HullSizeKey, hullSize).Get
				)
			);
			
			return hullCost + isDriveCost + specialsCost;
		}
		
		private void initSpecials(Dictionary<Component<SpecialEquipmentType>, int> specialEquipment)
		{
			this.SpecialEquipment = new Dictionary<Component<SpecialEquipmentType>, int>();

			foreach(var equipment in specialEquipment)
				this.SpecialEquipment.Add(equipment.Key, equipment.Value);
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
						this.IdCode, new Prerequisite[0], SiteType.StarSystem, false,
						new Formula(true), new Formula(this.Cost), new Formula(double.PositiveInfinity),
						new IConstructionEffect[] { new ConstructionAddShip(this) }
					);
				
				return this.constructionProject;
			}
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			Design other = obj as Design;
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

			hashBuilder.Add(indices.IndexOf(component.TypeInfo.IdCode), componentAssortiment.Count);
			hashBuilder.Add(component.Level, component.TypeInfo.MaxLevel + 1);
		}
	}
}
