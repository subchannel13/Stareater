using System;
using System.Collections.Generic;
using Stareater.AppData;
using Stareater.GameData;
using Stareater.GameLogic;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Data
{
	public class ConstructableItem
	{
		private const string LangContext = "Constructables";
		
		private IDictionary<string, double> vars;
		
		internal ConstructableItem(Constructable constructable, PlayerProcessor playerProcessor, 
		                           double? perTurnDone, double stockpile, double investment)
		{
			this.Constructable = constructable;
			this.vars = new Var().UnionWith(playerProcessor.TechLevels).Get;
			
			this.Investment = investment;
			this.PerTurnDone = perTurnDone;
			this.Stockpile = stockpile;
		}
		
		internal ConstructableItem(Constructable constructable, PlayerProcessor playerProcessor) :
			this(constructable, playerProcessor, null, 0, 0)
		{ }
		
		internal Constructable Constructable { get; private set; }
		
		public string Name 
		{
			get 
			{
				if (Constructable.LiteralText)
					return Constructable.NameCode;
				
				return Settings.Get.Language[LangContext][Constructable.NameCode].Text();
			}
		}
		
		public string Description 
		{ 
			get 
			{
				if (Constructable.LiteralText)
					return Constructable.DescriptionCode;
				
				return Settings.Get.Language[LangContext][Constructable.DescriptionCode].Text();
			}
		}
		
		public string ImagePath 
		{
			get
			{
				return Constructable.ImagePath;
			}
		}
		
		public string IdCode
		{
			get 
			{
				return Constructable.IdCode;
			}
		}
		
		public double Cost
		{
			get 
			{
				return Constructable.Cost.Evaluate(vars);
			}
		}
		
		public double Investment { get; private set; }
		
		public double? PerTurnDone { get; private set; }
		
		public double Stockpile { get; private set; }
	}
}
