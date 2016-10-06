using System;
using System.Collections.Generic;
using Stareater.GameData;
using Stareater.GameLogic;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views
{
	public class ConstructableItem
	{
		private const string LangContext = "Constructables";
		
		private readonly IDictionary<string, double> vars;
		
		internal ConstructableItem(Constructable constructable, PlayerProcessor playerProcessor, 
		                           ConstructionResult progress, double stockpile)
		{
			this.Constructable = constructable;
			this.Stockpile = stockpile;
			this.vars = new Var().UnionWith(playerProcessor.TechLevels).Get;
			
			if (progress != null)
			{
				this.Investment = progress.InvestedPoints;
				this.CompletedCount = progress.CompletedCount;
				this.FromStockpile = progress.FromStockpile;
				this.Overflow = progress.LeftoverPoints;
			}
		}
		
		internal ConstructableItem(Constructable constructable, PlayerProcessor playerProcessor) :
			this(constructable, playerProcessor, null, 0)
		{ }
		
		internal Constructable Constructable { get; private set; }
		
		public string Name 
		{
			get 
			{
				return Constructable.LiteralText ? 
					Constructable.NameCode : 
					LocalizationManifest.Get.CurrentLanguage[LangContext][Constructable.NameCode].Text();
			}
		}
		
		public string Description 
		{ 
			get 
			{
				return Constructable.LiteralText ? 
					Constructable.DescriptionCode : 
					LocalizationManifest.Get.CurrentLanguage[LangContext][Constructable.DescriptionCode].Text();
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
		
		public bool IsVirtual
		{
			get 
			{
				return Constructable.IsVirtual;
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
		public double FromStockpile { get; private set; }
		public long CompletedCount { get; private set; }
		public double Overflow { get; private set; }
		
		public double Stockpile { get; private set; }
	}
}
