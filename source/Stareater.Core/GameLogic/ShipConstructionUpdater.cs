using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.GameData;
using Stareater.Ships;

namespace Stareater.GameLogic
{
	class ShipConstructionUpdater : IConstructionVisitor
	{
		private List<Constructable> oldQueue;
		private Dictionary<Design, Design> refitOrders;
		private List<Constructable> newQueue = new List<Constructable>();
		private bool changeItem;
		private bool deleteItem;
		private List<IConstructionEffect> newEffects = new List<IConstructionEffect>();
		private Formula newCost;
		
		public ShipConstructionUpdater(List<Constructable> oldQueue, Dictionary<Design, Design> refitOrders)
		{
			this.oldQueue = new List<Constructable>(oldQueue);
			this.refitOrders = refitOrders;
		}
		
		public IEnumerable<Constructable> Run()
		{
			foreach(var item in this.oldQueue)
			{
				this.changeItem = false;
				this.deleteItem = false;
				this.newEffects.Clear();
				this.newCost = item.Cost;
				foreach(var effect in item.Effects)
					effect.Accept(this);
				
				if (!this.changeItem)
					this.newQueue.Add(item);
				else if (!this.deleteItem)
					this.newQueue.Add(new Constructable(
						item.LanguageCode, item.LiteralText, item.ImagePath, item.IdCode,
						item.Prerequisites, item.ConstructableAt, item.IsVirtual, item.StockpileGroup,
						item.Condition, newCost, item.TurnLimit,
						this.newEffects.ToArray()));
			}
			
			return this.newQueue;
		}

		private Design checkDesign(Design design)
		{
			if (!this.refitOrders.ContainsKey(design))
				return design;
			
			this.changeItem = true;
			this.deleteItem |= this.refitOrders[design] == null;
			if (!this.deleteItem)
				this.newCost = new Formula(this.refitOrders[design].Cost);
			
			return this.refitOrders[design];
		}
			
		
		#region IConstructionVisitor implementation

		public void Visit(ConstructionAddColonizer constructionEffect)
		{
			this.newEffects.Add(new ConstructionAddColonizer(
				checkDesign(constructionEffect.ColonizerDesign),
				constructionEffect.Destination
			));
		}

		public void Visit(ConstructionAddShip constructionEffect)
		{
			this.newEffects.Add(new ConstructionAddShip(
				checkDesign(constructionEffect.Design)
			));
		}

		public void Visit(ConstructionAddBuilding constructionEffect)
		{
			this.newEffects.Add(constructionEffect);
		}

		#endregion
	}
}
