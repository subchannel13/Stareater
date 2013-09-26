using System;
using Stareater.AppData;
using Stareater.GameData;

namespace Stareater.Controllers.Data
{
	public class ConstructableItem
	{
		private const string LangContext = "Constructables";
		
		private Constructable constructable;
		
		internal ConstructableItem(Constructable constructable)
		{
			this.constructable = constructable;
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
	}
}
