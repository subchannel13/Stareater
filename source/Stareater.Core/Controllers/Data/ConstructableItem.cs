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
		
		internal ConstructableItem(Constructable constructable, PlayerProcessor playerProcessor)
		{
			this.Constructable = constructable;
			this.vars = new Var().UnionWith(playerProcessor.TechLevels).Get;
		}
		
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
	}
}
