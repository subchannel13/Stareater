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
		
		private Constructable constructable;
		private IDictionary<string, double> vars;
		
		internal ConstructableItem(Constructable constructable, PlayerProcessor playerProcessor)
		{
			this.constructable = constructable;
			this.vars = new Var().UnionWith(playerProcessor.TechLevels).Get;
		}
		
		public string Name 
		{
			get 
			{
				if (constructable.LiteralText)
					return constructable.NameCode;
				
				return Settings.Get.Language[LangContext][constructable.NameCode].Text();
			}
		}
		
		public string Description 
		{ 
			get 
			{
				if (constructable.LiteralText)
					return constructable.DescriptionCode;
				
				return Settings.Get.Language[LangContext][constructable.DescriptionCode].Text();
			}
		}
		
		public string ImagePath 
		{
			get
			{
				return constructable.ImagePath;
			}
		}
		
		public string IdCode
		{
			get 
			{
				return constructable.IdCode;
			}
		}
		
		public double Cost
		{
			get 
			{
				return constructable.Cost.Evaluate(vars);
			}
		}
	}
}
