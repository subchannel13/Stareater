using System;
using Stareater.AppData;
using Stareater.GameData;

namespace Stareater.Controllers.Views
{
	public class BuildingInfo
	{
		private const string LangContext = "Buildings";
		
		private BuildingType building;
		
		public BuildingInfo(BuildingType building, double quantity)
		{
			this.building = building;
			this.Quantity = quantity;
		}
		
		public string Name 
		{
			get 
			{
				return Settings.Get.Language[LangContext][building.NameCode].Text();
			}
		}
		
		public string ImagePath 
		{
			get
			{
				return building.ImagePath;
			}
		}
		
		public double Quantity { get; private set; }
	}
}
