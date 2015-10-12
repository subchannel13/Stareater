using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.AppData.Expressions;
using Stareater.GameData;
using Stareater.GameData.Ships;
using Stareater.GameLogic;
using Stareater.Utils.Collections;

namespace Stareater.Ships
{
	partial class Design
	{
		private Constructable constructionProject = null;
			
		public object PrimaryEquip { get; private set; } //TODO(v0.5): make type
		public object SecondaryEquip { get; private set; } //TODO(v0.5): make type
		public object Shield { get; private set; } //TODO(v0.5): make type
		
		//public int id { get; private set; } //TODO(v0.5): might need id
		//private Dictionary<string, double> efekti = new Dictionary<string,double>(); //TODO(v0.5): might need
		//public object Hash { get; private set; } //TODO(v0.5): make type, might need
		
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
					And(AComponentType.SizeKey, hullSize).Get
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
						this.IdCode, new Prerequisite[0], SiteType.StarSystem,
						new Formula(true), new Formula(this.Cost), new Formula(double.PositiveInfinity),
						new IConstructionEffect[] { new ConstructionAddShip(this) }
					);
				
				return this.constructionProject;
			}
		}
	}
}
